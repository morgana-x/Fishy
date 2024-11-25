using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fishy.Extensions
{
    static class ExtensionLoader
    {
        public static List<FishyExtension> GetExtensions()
        {
            List<FishyExtension> extensions = [];

            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            List<Assembly> assemblies = [];

            foreach(string file in Directory.GetFiles(folder).Where(f => f.EndsWith(".dll")))
                assemblies.Add(Assembly.LoadFrom(file));

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsClass && type.IsSubclassOf(typeof(FishyExtension)))
                    {
                        if (Activator.CreateInstance(type) is FishyExtension extension)
                        {
                            extensions.Add(extension);
                        }
                    }
                }
            }
            return extensions;
        }
    }
}
