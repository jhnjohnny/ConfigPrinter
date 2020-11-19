using System;
using System.Collections.Generic;

namespace ConfigPrinter
{
    public interface IPrinter
    {
        List<PrinterModel> GetListPrinters();

        PrinterModel GetPrintProperties(String print);

        String GetPrinterDefault();

        String InstallPrinter(String print);
    }
}
