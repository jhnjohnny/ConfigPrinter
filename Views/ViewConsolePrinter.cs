using ConfigPrinter.Interfaces;
using System;
using System.Collections.Generic;

namespace ConfigPrinter
{
    public class ViewConsolePrinter : IViewPrinter
    {

        public void ListPrinters(List<PrinterModel> listPrinters)
        {
            foreach (var print in listPrinters)
            {
                Console.WriteLine("Print: " + print.Print);
                Console.WriteLine("Server: " + print.HostPrintServer);
                Console.WriteLine("Port: " + print.Port);
                Console.WriteLine("Driver: " + print.Driver);
                Console.WriteLine("**********************************");
                Console.WriteLine("");
            }
        }

        public void PrinterDefault(String defaultPrinter)
        {
            Console.WriteLine("Printer default: " + defaultPrinter);
            Console.WriteLine("");
        }

    }
}
