using System.ServiceProcess;

namespace GCGPRSService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
               {
                            new Service1()
               };
            ServiceBase.Run(ServicesToRun);
            //Service1 service = new Service1();
            //service.OnStart(null);
        }
    }
}
