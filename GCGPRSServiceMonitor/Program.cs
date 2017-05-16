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
                    try
                    {
                        Process proc = GetProcess(ConstValue.MSMQServiceName);// RunningInstance();
                        if (proc == null)
                        {
                            StartService();
                        }
                        else
                        {
                            using (var p1 = new PerformanceCounter("Process", "Working Set - Private", proc.ProcessName))
                            {
                                if (p1.NextValue() / 1024 / 1024 > 400) //>500MB ,GCGPRSService内存问题未解决,运行时间长后，内存会一直增加
                                {
                                    proc.Kill();
                                    Thread.Sleep(3 * 1000);
                                    StartService();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Thread.Sleep(2 * 60 * 1000);
                }
            }
        }

        static void StartService()
        {
            try
            {
                ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                if (serviceController1 != null && serviceController1.Status == ServiceControllerStatus.Stopped)
                {
                    serviceController1.Start();
                    serviceController1.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0,0,10));  // 等待服务达到指定状态
                }

            }
            catch (Exception ex)
            {
            }
        }

        static void StopService()
        {
            try
            {
                ServiceController serviceController1 = ServiceManager.GetService(ConstValue.MSMQServiceName);
                if (serviceController1 != null && serviceController1.Status != ServiceControllerStatus.Stopped)
                {
                    serviceController1.Stop();
                    serviceController1.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 10));  // 等待服务达到指定状态
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

        static float GetMemory(string programename)
        {
            using (var process = GetProcess(programename))
            {
                if (process != null)
                {
                    using (var p1 = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName))
                    {
                        return p1.NextValue() / 1024 / 1024;
                    }
                }
            }
            return 0;
        }

        static Process GetProcess(string programename)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Trim().ToLower() == programename.ToLower())
                {
                    return process;
                }
            }
            return null;
        }

        /// <summary>
        /// 停止进程
        /// </summary>
        /// <param name="programename"></param>
        /// <returns></returns>
        static bool Stop(string programename)
        {
            try
            {
                Process proc = GetProcess(programename);
                if (proc != null)
                    proc.Kill();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
