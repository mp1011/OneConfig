using System.IO;
using System.Linq;
using System.Reflection;

namespace OneConfig.Services
{
    public static class FileHelper
    {
        /// <summary>
        /// Unless changed at runtime, this will refer to the location of the executing assembly
        /// </summary>
        public static DirectoryInfo ApplicationDirectory { get; set; }

        static FileHelper()
        {
            var assemblyLocation = new FileInfo(Assembly.GetExecutingAssembly().Location);
            ApplicationDirectory = new DirectoryInfo(assemblyLocation.Directory.FullName);
        }

        public static bool IsPathRelative(string path)
        {
            var isRooted = Path.IsPathRooted(path);

            //a path that leads with a slash also counts as rooted but isn't actually absolute for our purposes
            if (Path.GetPathRoot(path).FirstOrDefault() == Path.DirectorySeparatorChar)
                return true;

            return !isRooted;
        }

        /// <summary>
        /// If the given path is relative, prepends ApplicationDirectory to it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToAbsolutePath(string path)
        {
            if (path == null)
                return ApplicationDirectory.FullName;
            else if (IsPathRelative(path))
                return Path.Combine(ApplicationDirectory.FullName, path);
            else
                return path;
        }
    }
}
