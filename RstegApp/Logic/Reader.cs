using System;
using System.Linq;
using System.Text;
using PacketDotNet;
using RstegApp.Properties;
using SharpPcap;

namespace RstegApp.Logic
{
    class Reader
    {
        private byte[] _keyWord;
        private string _stegWord;
        private int maxBuff = 0xFFFF;
        private int _port = 0;
        private ICaptureDevice _device;

        private CaptureDeviceList DeviceList
        {
            get { return CaptureDeviceList.Instance; }
        }

        public void Read(string keyWord, string stegWord = null)
        {
            _stegWord = stegWord;

            //Console.WriteLine("enter device number.. (see count of flag) ");

            _device = DeviceList.FirstOrDefault();

            if (_device == null)
            {
                return;
            }

            Encoding encodUnicode = Encoding.Unicode;
            _keyWord = encodUnicode.GetBytes(keyWord);

            _device.OnPacketArrival += device_OnPaketArrival;

            _device.Open(DeviceMode.Normal);

            //Console.WriteLine("Listein {0}", _device.Description);

            _device.StartCapture();
        }

        private void device_OnPaketArrival(object sender, CaptureEventArgs e)
        {
            Packet packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
            TcpPacket tcpPacket = TcpPacket.GetEncapsulated(packet);

            if (tcpPacket != null)
            {
                String SPort = tcpPacket.SourcePort.ToString();
                var DPort = tcpPacket.DestinationPort.ToString();
                var Data = Encoding.Unicode.GetString(tcpPacket.PayloadData);

                if (OptionTcp.UnsafeCompareByte(tcpPacket.PayloadData, _keyWord))
                {
                    //Console.WriteLine(Resources.ConfirmMessage);
                    try
                    {
                        unsafe
                        {
                            byte[] pack = new byte[maxBuff];
                            WINDIVERT_ADDRESS addr = new WINDIVERT_ADDRESS();
                            WINDIVERT_TCPHDR** packHeader = default(WINDIVERT_TCPHDR**);
                            uint packetLen = 0;
                            IntPtr handle = WinDivertMethods.WinDivertOpen(string.Format("tcp.SrcPort == {0}", _port),
                                WINDIVERT_LAYER.WINDIVERT_LAYER_NETWORK, 0, 0);

                            if (
                                !WinDivertMethods.WinDivertRecv(handle, pack, (uint) pack.Length, ref addr,
                                    ref packetLen))
                            {
                                return;
                            }
                            WinDivertMethods.WinDivertHelperParsePacket(pack, packetLen, null, null, null, null,
                                packHeader, null, null, null);
//                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
//                        proc.StartInfo.FileName = "dropTcpPacket.exe";
//                        proc.StartInfo.Arguments = _stegWord;
//                        proc.Start();
                        }
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(Resources.ExceptionMessage, exp.Message);
                    }
                }
                //else Console.WriteLine("nnnnnnnnoo: {0}", Encoding.Unicode.GetString(_keyWord));

                //Console.WriteLine("Sport: {0},  DPort: {1}, Data: {2}", SPort, DPort, Data);
                //Console.WriteLine("==========================================================");
            }
        }

        private void Close()
        {
            if (_device != null)
            {
                _device.StopCapture();
                _device.Close();
            }
        }
    }
}