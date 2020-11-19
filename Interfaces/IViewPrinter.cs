using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigPrinter.Interfaces
{
    public interface IViewPrinter
    {
        void ListPrinters(List<PrinterModel> listPrinters);

        void PrinterDefault(String defaultPrinter);

    }
}
