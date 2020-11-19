using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConfigPrinter
{
    public class PrinterCMD : IPrinter
    {

        public List<PrinterModel> GetListPrinters()
        {
            var listPrinters = new List<PrinterModel>();

            var arguments = "WMIC PRINTER GET Default, DriverName, Name, PortName, ServerName, ShareName";
            var resultCommand = Utils.RunCMD(arguments);

            var linhas = resultCommand.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 1; i < linhas.Length; i++)
            {
                if (String.IsNullOrEmpty(linhas[i]) || linhas[i] == "\r") continue;

                var printer = new PrinterModel();
                var cols = Regex.Split(linhas[i], @"\s{2,}");

                printer.Print = cols[2].Trim();
                printer.HostPrintServer = cols[4].Trim();
                printer.Port = cols[3].Trim();
                printer.Driver = cols[1].Trim();

                listPrinters.Add(printer);
            }

            return listPrinters;
        }


        public PrinterModel GetPrintProperties(String print)
        {
            var printer = new PrinterModel();

            var arguments = String.Format("WMIC PRINTER where Name='{0}' GET Default, DriverName, Name, PortName, ServerName", print);
            var resultCommand = Utils.RunCMD(arguments);

            var linhas = resultCommand.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var cols = Regex.Split(linhas[1], @"\s{2,}");

            printer.Print = cols[2].Trim();
            printer.HostPrintServer = cols[4].Trim();
            printer.Port = cols[3].Trim();
            printer.Driver = cols[1].Trim();

            return printer;
        }


        public String GetPrinterDefault()
        {
            var arguments = "WMIC PRINTER where Default='TRUE' GET Name";
            var resultCommand = Utils.RunCMD(arguments);

            var linhas = resultCommand.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            string defaultPrinter = linhas[1].Replace("\r", string.Empty).Trim();
            return defaultPrinter;
        }


        public String SetPrinterDefault(String print)
        {
            var arguments = String.Format("WMIC PRINTER where Name='{0}' call setdefaultprinter", print);
            var resultCommand = Utils.RunCMD(arguments);

            return resultCommand;
        }


        public String SetPrinterShared(String print)
        {
            var arguments = String.Format("WMIC PRINTER where Name='{0}' set shared=true", print);
            var resultCommand = Utils.RunCMD(arguments);

            return resultCommand;
        }


        public String InstallPrinter(String print)
        {
            string printName = String.Format("/b \"{0}\"", print);
            string printDir = "/f \"c:\\Printers_Drivers\\Lexmark\\LMUD1o40.inf\"";
            string printPort = "/r FILE:"; // "/r 0.0.0.0"
            string printModel = "/m \"Lexmark Universal v2\"";  // Modelo existente no .INF


            var arguments = String.Format("printui.dll,PrintUIEntry /ga /if {0} {1} {2} {3}", printName, printDir, printPort, printModel);
            var resultCommand = Utils.RunDLL32(arguments);

            return resultCommand;
        }




    }
}
