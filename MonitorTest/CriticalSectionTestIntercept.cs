using Castle.DynamicProxy;
using MonitorImplementation.HoareMonitor;

namespace MonitorTest
{
    [TestClass]
    public class CriticalSectionTestIntercept
    {
        private readonly ProxyGenerator proxyGenerator = new ProxyGenerator();

        [TestMethod]
        public void TestProtectedMethod()
        {
            // Prepare
            using (CriticalSectionTestClassProxy criticalSectionTestClass =
                                          proxyGenerator.CreateClassProxy<CriticalSectionTestClassProxy>
                                          (new LoggingInterceptor
                                          (new CriticalSectionTestClassProxy())))
            {
                // Act
                Thread threadProtected = new Thread(() =>
                {
                    criticalSectionTestClass.ThreadsMethod(criticalSectionTestClass.InterceptedMethod);
                });

                threadProtected.Start();
                threadProtected.Join();

                // Test
                Assert.IsTrue(criticalSectionTestClass.checkValidity);
            }
        }

        public class CriticalSectionTestClassProxy : HoareMonitorImplementation
        {
            [MonitorIntercept]
            public virtual void InterceptedMethod()
            {
                TestMethod();
            }

            public virtual void ThreadsMethod(ThreadStart start)
            {
                if (start == null)
                    throw new ArgumentNullException(nameof(start));
                Thread[] threadsArray = new Thread[2];
                for (int i = 0; i < threadsArray.Length; i++)
                    threadsArray[i] = new Thread(start);
                foreach (Thread _thread in threadsArray)
                    _thread.Start();
                foreach (Thread _thread in threadsArray)
                    _thread.Join();
            }

            private int intA = 0;
            private int intB = 0;
            internal virtual void TestMethod()
            {
                for (int i = 0; i < 1000000; i++)
                {
                    int randomInt = new Random().Next(0, 1000000);
                    intA = randomInt;
                    intB = randomInt;
                    if (intA - intB != 0)
                    {
                        checkValidity = false;
                    }
                }
            }

            public bool checkValidity = true;
        }
    }
}