using System;
using System.Text;
using System.Configuration.Install;
using System.ServiceProcess;

namespace GPRSServiceUI
{
    public class ServiceManager
    {
        /// <summary>
        /// 使用Windows Service对应的exe文件 安装Service
        /// 和 installutil xxx.exe 效果相同
        /// </summary>
        /// <param name="installFile">exe文件（包含路径）</param>
        /// <returns>是否安装成功</returns>
        public static bool InstallServie(string installFile)
        {
            string[] args = { installFile };
            try
            {
                ManagedInstallerClass.InstallHelper(args);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 使用Windows Service对应的exe文件 卸载Service
        /// 和 installutil /u xxx.exe 效果相同
        /// </summary>
        /// <param name="installFile">exe文件（包含路径）</param>
        /// <returns>是否卸载成功</returns>
        public static bool UninstallService(string installFile)
        {
            string[] args = { "/u", installFile };
            try
            {
                // 根据文件获得服务名，假设exe文件名和服务名相同
                string tmp = installFile;
                if (tmp.IndexOf('\\') != -1)
                {
                    tmp = tmp.Substring(tmp.LastIndexOf('\\') + 1);
                }
                string svcName = tmp.Substring(0, tmp.LastIndexOf('.'));
                // 在卸载服务之前 要先停止windows服务
                StopService(svcName);

                ManagedInstallerClass.InstallHelper(args);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得service对应的ServiceController对象
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>ServiceController对象，若没有该服务，则返回null</returns>
        public static ServiceController GetService(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName == serviceName)
                {
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// 获得Service的详细信息
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>Service信息，保存在string中</returns>
        public static string GetServiceInfo(string serviceName)
        {
            StringBuilder details = new StringBuilder();

            ServiceController sc = GetService(serviceName);

            if (sc == null)
            {
                return string.Format("{0} 不存在！", serviceName);
            }

            details.AppendLine(string.Format("服务标识的名称: {0}", sc.ServiceName));
            details.AppendLine(string.Format("服务友好名称：{0}", sc.DisplayName));
            details.AppendLine(string.Format("服务在启动后是否可以停止: {0}", sc.CanStop));
            details.AppendLine(string.Format("服务所驻留的计算机的名称: {0}", sc.MachineName)); // "." 表示本地计算机
            details.AppendLine(string.Format("服务类型： {0}", sc.ServiceType.ToString()));
            details.AppendLine(string.Format("服务状态： {0}", sc.Status.ToString()));

            // DependentServices 获取依赖于与此 ServiceController 实例关联的服务的服务集。
            StringBuilder dependentServices = new StringBuilder();
            foreach (ServiceController s in sc.DependentServices)
            {
                dependentServices.Append(s.ServiceName + ", ");
            }
            details.AppendLine(string.Format("依赖于与此 ServiceController 实例关联的服务的服务: {0}", dependentServices.ToString()));

            // ServicesDependedOn 此服务所依赖的服务集。
            StringBuilder serviceDependedOn = new StringBuilder();
            foreach (ServiceController s in sc.ServicesDependedOn)
            {
                serviceDependedOn.Append(s.ServiceName + ", ");
            }
            details.AppendLine(string.Format("此服务所依赖的服务: {0}", serviceDependedOn.ToString()));

            return details.ToString();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>是否启动成功</returns>
        public static bool StartService(string serviceName)
        {
            ServiceController sc = GetService(serviceName);

            if (sc.Status != ServiceControllerStatus.Running)
            {
                try
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);  // 等待服务达到指定状态
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            return true;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>是否停止服务成功，如果服务启动后不可以停止，则抛异常</returns>
        public static bool StopService(string serviceName)
        {
            ServiceController sc = GetService(serviceName);

            //if (!sc.CanStop)
            //{
            //    throw new Exception(string.Format("服务{0}启动后不可以停止.", serviceName));
            //}

            if (sc.Status != ServiceControllerStatus.Stopped)
            {
                try
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);  // 等待服务达到指定状态
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return true;
        }
    }
}
