using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigPrinter
{
    public class PrinterSnmpSharpNet
    {
        public static int Timeout { get; } = 1000;

        private readonly List<PrinterModel> _listPrint = new List<PrinterModel>();
        private readonly ManualResetEvent _doneEvent = new ManualResetEvent(false);
        private readonly object listLock = new object();
        private int inProcess = 0;

        public PrinterModel[] GetPrinters(String[] ips)
        {
            _doneEvent.Reset();
            _listPrint.Clear();

            foreach (var ip in ips)
            {
                Interlocked.Increment(ref inProcess);
                ThreadPool.QueueUserWorkItem(GetPrinter, ip);
            }
            _doneEvent.WaitOne();

            return _listPrint.ToArray();
        }

        private void GetPrinter(object ipObj)
        {
            try
            {
                var ip = (string)ipObj;
                IpAddress agent = new IpAddress(ip);

                using(UdpTarget target = new UdpTarget((IPAddress)agent, 161, Timeout, 2))
                {
                    Pdu pdu = new Pdu(PduType.Get);
                    pdu.VbList.Add("1.3.6.1.2.1.1.0");

                    OctetString community = new OctetString("public");
                    SnmpPacket result = null;
                    foreach (SnmpVersion ver in Enum.GetValues(typeof(SnmpVersion)))
                    {
                        result = target.Request(pdu, new AgentParameters(community) { Version = ver });
                        if (result != null)
                            break;
                    }

                    if(result != null)
                    {
                        if(result.Pdu.ErrorStatus == 0)
                        {
                            var name = result.Pdu.VbList[0].Value.ToString();
                            var imp = GetNameIp(name,ip);
                            if(imp != null)
                            {
                                lock (listLock)
                                    _listPrint.Add(imp);
                            }
                        }
                    }
                }
            }
            catch (SnmpException) { }
            finally
            {
                if (Interlocked.Decrement(ref inProcess) == 0)
                    _doneEvent.Set();
            }
            
        }


        private PrinterModel GetNameIp(String name, String ipv4)
        {
            var printer = new PrinterModel
            {
                Print = name,
                Ipv4 = ipv4
            };

            return printer;
        }



    }
}
