using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigPrinter
{
    public static class Utils
    {
        public static String RunCMD(String arguments)
        {
            //https://ss64.com/nt/wmic.html
            var output = string.Empty;

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/c " + arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using(Process cmd = new Process())
            {
                cmd.StartInfo = startInfo;
                cmd.Start();
                output = cmd.StandardOutput.ReadToEnd();
                cmd.WaitForExit();
            }

            return output;
        }

        public static String RunDLL32(String arguments)
        {
            string system32 = Environment.GetEnvironmentVariable("SYSTEMROOT") + @"\system32\";

            var output = string.Empty;

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = system32 + @"/RUNDLL32",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (Process cmd = new Process())
            {
                cmd.StartInfo = startInfo;
                cmd.Start();
                output = cmd.StandardOutput.ReadToEnd();
                cmd.WaitForExit();
            }

            return output;
        }
    }
}
