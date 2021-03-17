using System;
using System.Linq;
using System.Reflection;

namespace Beblue.Desafio.Cashback.Generico.Helpers
{
    public static class AssemblyHelper
    {
        public static Assembly GetAssemblyByName(string assemblyName)
        {
            Assembly.Load(assemblyName);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            return (from assembly in assemblies let name = assembly.GetName().Name.ToLower() where assemblyName.ToLower() == name select assembly).FirstOrDefault();
        }
    }
}
