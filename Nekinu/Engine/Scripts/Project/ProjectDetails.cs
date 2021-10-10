using System;
using System.IO;
using System.Reflection;

namespace Nekinu
{
    public class ProjectDetails
    {
        public static string rootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string projectName = "";
        public static string projectDeveloper = "";

        public static float projectVersion = 0f;
    }
}