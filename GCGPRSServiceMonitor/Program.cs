using System;
using System.ServiceProcess;
using Entity;
using Common;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace GCGPRSServiceMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Process instance = RunningInstance();
            if (instance == null)
            {
                while (true)
                {
                    CheckServiceStatus();
                }
            }
        }

        static void CheckServiceStatus()
        {
            try
            {
                Thread.Sleep(10 * 60 * 1000);
                ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                if (serviceController1 != null && serviceController1.Status == ServiceControllerStatus.Stopped)
                {
                    serviceController1.Start();
                    serviceController1.WaitForStatus(ServiceControllerStatus.Running);  // 等待服务达到指定状态
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //Loop through the running processes in with the same name   
            foreach (Process process in processes)
            {
                //Ignore   the   current   process  
                if (process.Id != current.Id)
                {
                    //Make sure that the process is running from the exe file.   
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\\\") == current.MainModule.FileName)
                    {
                        //Return   the   other   process   instance.  
                        return process;
                    }
                }
            }
            //No other instance was found, return null. 
            return null;
        }

    }
}
