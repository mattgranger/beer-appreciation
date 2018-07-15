namespace Core.Shared.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Embedded Resource Helper
    /// </summary>
    public static class ResourceHelper
    {
        /// <summary>
        /// Reads a manifest resource by name and returns the contents as a string
        /// </summary>
        /// <param name="resourceName">The name of the resource</param>
        /// <returns>The string representation of the resource content</returns>
        public static string GetManifestResourceAsString(string resourceName)
        {
            return GetManifestResourceAsString(Assembly.GetCallingAssembly(), resourceName);
        }

        /// <summary>
        /// Reads a manifest resource by name from a specified assembly and returns the contents as a string
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>The string representation of the resource content</returns>
        public static string GetManifestResourceAsString(string assemblyName, string resourceName)
        {
            Assembly assembly = GetAssemblyByName(assemblyName);

            if (assembly == null)
            {
                throw new InvalidOperationException($"The assembly {assemblyName} is not loaded in the current app domain");
            }

            return GetManifestResourceAsString(assembly, resourceName);
        }

        /// <summary>
        /// Reads a manifest resource by name and returns the contents as a byte array
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>The resource as a byte array</returns>
        public static byte[] GetManifestResourceAsByteArray(string resourceName)
        {
            using (Stream stream = GetManifestResourceAsStream(Assembly.GetCallingAssembly(), resourceName))
            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                return binaryReader.ReadBytes((int)stream.Length);
            }
        }

        /// <summary>
        /// Reads a manifest resource by name from a specified assembly and returns the contents as a string
        /// </summary>
        /// <param name="assembly">The assembly in which the resource resides.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>
        /// The string representation of the resource content
        /// </returns>
        private static string GetManifestResourceAsString(Assembly assembly, string resourceName)
        {
            using (Stream stream = GetManifestResourceAsStream(assembly, resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                StringBuilder stringBuilder = new StringBuilder();
                string line = reader.ReadLine();
                while (line != null)
                {
                    stringBuilder.AppendLine(line);
                    line = reader.ReadLine();
                }

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Reads a manifest resource by name from a specified assembly and returns the contents as a stream
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>The Stream for the specified manifest resource</returns>
        private static Stream GetManifestResourceAsStream(Assembly assembly, string resourceName)
        {
            Stream stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new InvalidOperationException($"Unable to locate the resource at '{resourceName}', make sure it is marked as an Embedded Resource");
            }

            return stream;
        }

        /// <summary>
        /// Gets an assembly currently loaded into the app domain by name
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>The assembly or null if not loaded in the current app domain</returns>
        private static Assembly GetAssemblyByName(string assemblyName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == assemblyName);   
        }
    }
}
