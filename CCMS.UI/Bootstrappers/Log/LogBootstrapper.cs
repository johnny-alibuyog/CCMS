using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
//using log4net.Config;

namespace CCMS.UI.Bootstrappers.Log
{
    public static class LogBootstrapper
    {
        public static void Initialize()
        {
            //var fileInfo = new FileInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log4net.config");
            //XmlConfigurator.ConfigureAndWatch(fileInfo);

            //var d = AppDomain.CurrentDomain.BaseDirectory;
            //var x = Assembly.GetExecutingAssembly().Location;
            //var y = Path.GetDirectoryName(x);
            //var z = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            //var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var filename = @"C:\Program Files (x86)\Narasoft\log4net.config"; //Path.Combine(directory, "log4net.config");
            //var fileInfo = new FileInfo(filename);
            //XmlConfigurator.ConfigureAndWatch(fileInfo);
        }
    }
}
