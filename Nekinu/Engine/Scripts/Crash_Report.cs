using System;
using System.IO;

namespace Nekinu
{
    public class Crash_Report
    {
        public static void generate_crash_report(object error)
        {
            DateTime time = DateTime.Now;

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Crash Reports"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Crash Reports");
            }

            string[] n = time.ToString().Replace(":", " ").Split('/');

            string name = "";

            for (int i = 0; i < n.Length; i++)
            {
                name += n[i] + " ";
            }
            
            FileStream stream = File.Create($"{Directory.GetCurrentDirectory()}/Crash Reports/{name}.crsh");

            StreamWriter writer = new StreamWriter(stream);
            
            writer.WriteLine(error.ToString());
            
            writer.Close();
        }
    }
}