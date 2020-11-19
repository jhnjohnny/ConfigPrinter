using ConfigPrinter.Interfaces;
using System;
using System.Collections.Generic;

namespace ConfigPrinter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IViewPrinter viewPrinter;
                IPrinter printer;

                viewPrinter = new ViewConsolePrinter();

                printer = new PrinterCMD();
                //printer = new PrinterPrintServer()               
                

                // Exibe a Lista de Impressoras
                viewPrinter.ListPrinters(printer.GetListPrinters());

                // Exibe a Impressora Padrão
                string printDefault = printer.GetPrinterDefault();
                viewPrinter.PrinterDefault(printDefault);

                // Exibe as Propriedades da Impressora Padrão
                viewPrinter.ListPrinters(new List<PrinterModel>()
                {
                    printer.GetPrintProperties(printDefault)
                });

                var instPrint = printer.InstallPrinter("TesteInstall");
                Console.WriteLine(instPrint);

                // Exibe Range de IPs encontrados na Rede
                //var IPsLan = NetLocal.ListAddressIPv4();
                //Console.WriteLine(String.Format($"Range Lan count: {IPsLan.Length}"));
                //Console.WriteLine(String.Format($"IPs address: {IPsLan[0]} - {IPsLan[IPsLan.Length - 1]}"));

                //var printerSnmpSharpNet = new PrinterSnmpSharpNet();
                //var printers = printerSnmpSharpNet.GetPrinters(IPsLan);

                //Console.WriteLine(printers.Length);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }
    }
}
