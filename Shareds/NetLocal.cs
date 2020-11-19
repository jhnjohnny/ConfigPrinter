using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConfigPrinter
{
    public static class NetLocal
    {

        public static string[] ListAddressIPv4()
        {
            var listIps = new List<string>();
            GetIpMask(out String ipv4, out String mask);
            var addressLan = GetAddressLan(ipv4, mask);

            var ipLan = addressLan.Split('/')[0];
            var ipLanBytes = IPAddress.Parse(ipLan).GetAddressBytes();
            Array.Reverse(ipLanBytes);
            var ipInt = BitConverter.ToInt32(ipLanBytes, 0);

            var bitsHosts = 32 - int.Parse(addressLan.Split('/')[1]);
            var hosts = Math.Pow(2, bitsHosts) - 2;

            for (int i = 0; i < hosts; i++)
            {
                var ipBytes = BitConverter.GetBytes(ipInt + i);
                Array.Reverse(ipBytes);
                listIps.Add(new IPAddress(ipBytes).ToString());
            }

            return listIps.ToArray();
        }


        private static void GetIpMask(out String ipv4, out String mask)
        {
            NetworkInterface[] netInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var netInterface in netInterfaces)
            {
                if(netInterface.OperationalStatus == OperationalStatus.Up)
                {
                    if(netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet || 
                        netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    {
                        UnicastIPAddressInformationCollection unicastIPInfoCol = netInterface
                            .GetIPProperties().UnicastAddresses;

                        foreach (var unicastIPInfo in unicastIPInfoCol)
                        {
                            if(unicastIPInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ipv4 = unicastIPInfo.Address.ToString();
                                mask = unicastIPInfo.IPv4Mask.ToString();
                                return;
                            }
                        }
                    }
                }
            }
            throw new Exception("IP not found");
        }

        private static String GetAddressLan(String ipv4, String mask)
        {
            int CIDR = 0;
            byte[] ipAddressBytes = IPAddress.Parse(ipv4).GetAddressBytes();
            byte[] subNetMaskBytes = IPAddress.Parse(mask).GetAddressBytes();

            if (ipAddressBytes.Length != subNetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAddressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAddressBytes[i] & (subNetMaskBytes[i]));
            }

            BitArray bits = new BitArray(subNetMaskBytes);
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i]) CIDR++;
            }

            return $"{new IPAddress(broadcastAddress).ToString()}/{CIDR}";
        }


    }
}
