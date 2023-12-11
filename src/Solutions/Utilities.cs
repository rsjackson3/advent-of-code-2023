using System.Reflection;

namespace AdventOfCode
{
    public static class Utilities
    {
        public static string GetResourceData(string resourceName, string assemblyName = "")
        {
            var assembly = string.IsNullOrEmpty(assemblyName) ? Assembly.GetExecutingAssembly() : Assembly.Load(assemblyName);
            string fullName = assembly.GetManifestResourceNames().Where(r => r.EndsWith(resourceName)).First();
            string input;
            using (Stream? stream = assembly.GetManifestResourceStream(fullName))
            {
                if (stream == null)
                    throw new Exception($"cannot get manifest resource stream. Resource Name: {resourceName}");

                using (StreamReader reader = new StreamReader(stream))
                {
                    input = reader.ReadToEnd();
                }
            }

            return input;
        }
    }
}