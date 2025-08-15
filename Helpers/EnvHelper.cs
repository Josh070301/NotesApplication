using System;
using System.IO;
using System.Web;

namespace NotesApplication.Helpers
{
    public static class EnvHelper
    {
        private static bool _isInitialized = false;
        private static object _lock = new object();

        public static void Initialize()
        {
            if (_isInitialized)
                return;

            lock (_lock)
            {
                if (_isInitialized)
                    return;

                string basePath = HttpContext.Current != null 
                    ? HttpContext.Current.Server.MapPath("~") 
                    : AppDomain.CurrentDomain.BaseDirectory;
                
                string envPath = Path.Combine(basePath, ".env");
                
                if (File.Exists(envPath))
                {
                    DotNetEnv.Env.Load(envPath);
                }
                
                _isInitialized = true;
            }
        }

        public static string GetEnvironmentVariable(string name, string defaultValue = "")
        {
            Initialize();
            
            string value = Environment.GetEnvironmentVariable(name);
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }
}