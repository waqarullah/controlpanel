using Spring.Context.Support;
using Spring.Context;
using System.IO;
using System;
using System.Configuration;

namespace ControlPanel.Core
{
		
	public static class IoC
	{
        private static readonly IApplicationContext _appContext = new XmlApplicationContext(false, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.IsNullOrEmpty(ConfigurationManager.AppSettings["SpringFilePath"]) ? "spring.cfg.xml" : ConfigurationManager.AppSettings["SpringFilePath"]));

		public static T Resolve<T>(string name)
		{
			return (T)_appContext.GetObject(name);
		}

		public static bool Exists(string name)
		{
			return _appContext.ContainsObjectDefinition(name);
		}
	}
	
	
}
