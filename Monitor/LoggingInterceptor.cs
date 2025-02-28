using Castle.DynamicProxy;
using MonitorImplementation.HoareMonitor;

public class LoggingInterceptor(HoareMonitorImplementation monitor) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        bool intercepted = false;

        if (invocation.MethodInvocationTarget.GetCustomAttributes(typeof(MonitorInterceptAttribute), true).Any())
        {
            intercepted = true;
        }
        if (invocation.Method.GetCustomAttributes(typeof(MonitorInterceptAttribute), true).Any())
        {
            intercepted = true;
        }

        if (intercepted)
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