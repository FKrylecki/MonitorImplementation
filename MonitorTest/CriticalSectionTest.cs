using MonitorImplementation.HoareMonitor;

namespace MonitorTest
{
    [TestClass]
    public class CriticalSectionTest
    {
        [TestMethod]
        public void TestProtectedMethod()
        {
            // Prepare
            using (CriticalSectionTestClass criticalSectionTestClass = new CriticalSectionTestClass()) {

                // Act
                Thread threadProtected = new Thread(() =>
                {
                    criticalSectionTestClass.ThreadsMethod(criticalSectionTestClass.HoareMonitorMethod);
                });

                threadProtected.Start();
                threadProtected.Join();

                // Test
                Assert.IsTrue(criticalSectionTestClass.checkValidity);
            }
        }

        [TestMethod]
        public void TestNotProtectedMethod()
        {
            // Prepare
            using (CriticalSectionTestClass criticalSectionTestClass = new CriticalSectionTestClass())
            {
                // Act
                Thread threadProtected = new Thread(() =>
                {
                    criticalSectionTestClass.ThreadsMethod(criticalSectionTestClass.NonMonitorMethod);
                });

                threadProtected.Start();
                threadProtected.Join();

                // Test
                Assert.IsFalse(criticalSectionTestClass.checkValidity);
            }
        }

        private class CriticalSectionTestClass : HoareMonitorImplementation
        {
            internal void HoareMonitorMethod()
            {
                enterTheMonitor();
                try
                {
                    TestMethod();
                }
                finally
                {
                    exitTheMonitor();
                }
            }

            internal void NonMonitorMethod()
            {
                TestMethod();
            }

            public void ThreadsMethod(ThreadStart start)
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
