using System;
using System.Collections.Generic;
using System.Management;
using System.Printing;

namespace ConfigPrinter
{
    public class PrinterPrintServer : IPrinter
    {

        public List<PrinterModel> GetListPrinters()
        {
            var listPrinters = new List<PrinterModel>();

            using(var printServer = new PrintServer())
            {
                foreach (var print in printServer.GetPrintQueues())
                {
                    //if (!print.IsShared) continue;

                    listPrinters.Add(new PrinterModel 
                    { 
                        Print = print.Name,
                        HostPrintServer = print.HostingPrintServer.Name,
                        Port = print.QueuePort.Name,
                        Driver = print.QueueDriver.Name
                    });
                }
            }

            return listPrinters;
        }


        public PrinterModel GetPrintProperties(String print)
        {
            PrinterModel printer = new PrinterModel();

            string query = string.Format("SELECT * FROM Win32_Printer WHERE Name LIKE '%{0}'", print);

            using(ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            using(ManagementObjectCollection coll = searcher.Get())
            {
                try
                {
                    foreach (var printerObj in coll)
                    {
                        foreach (PropertyData property in printerObj.Properties)
                        {
                            //Console.WriteLine(string.Format("{0}: {1}", property.Name,property.Value));

                            switch (property.Name)
                            {
                                case "DeviceID":
                                    printer.Print = property.Value.ToString();
                                    break;

                                case "ServerName":
                                    printer.HostPrintServer = (property.Value == null) ? "" : property.Value.ToString();
                                    break;

                                case "PortName":
                                    printer.Port = property.Value.ToString();
                                    break;

                                case "DriverName":
                                    printer.Driver = property.Value.ToString();
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
                catch (ManagementException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return printer;
        }


        public String GetPrinterDefault()
        {
            string defaultPrinter = "";

            using (var printServer = new LocalPrintServer())
            {
                defaultPrinter = printServer.DefaultPrintQueue.Name;
            }

            return defaultPrinter;
        }

        public string InstallPrinter(string print)
        {
            throw new NotImplementedException();
        }
    }
}
