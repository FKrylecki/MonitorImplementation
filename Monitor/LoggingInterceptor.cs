using Castle.DynamicProxy;
using MonitorImplementation.HoareMonitor;

public class LoggingInterceptor(HoareMonitorImplementation monitor) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        bool isIntercepted = false;

        if (invocation.MethodInvocationTarget.GetCustomAttributes(typeof(MonitorInterceptAttribute), true).Any())
        {
            isIntercepted = true;
        }
        if (invocation.Method.GetCustomAttributes(typeof(MonitorInterceptAttribute), true).Any())
        {
            isIntercepted = true;
        }

        if (isIntercepted)
        {
            monitor.enterTheMonitor();
            try
            {
                invocation.Proceed();
            }
            finally
            {
                monitor.exitTheMonitor();
            }
        }
        else
        {
            invocation.Proceed();
        }
    }
}