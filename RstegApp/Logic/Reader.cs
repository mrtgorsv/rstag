using System;
using System.Linq;
using System.Text;
using PacketDotNet;
using RstegApp.Properties;
using SharpPcap;
using SharpPcap.WinPcap;

namespace RstegApp.Logic
{
    class Reader
    {
        private byte[] _keyWord;
        private string _stegWord;
        private int maxBuff = 0xFFFF;
        private readonly int _port;
        private ICaptureDevice _device;

        private bool _isServer;

        public Reader(short port)
        {
            _port = port;
        }

        private CaptureDeviceList DeviceList
        {
            get { return CaptureDeviceList.Instance; }
        }

        public void StartCapturing(bool isServer)
        {
            _isServer = isServer;

            _stegWord = Resources.StegWord;

            var device  = DeviceList.FirstOrDefault(d => d is WinPcapDevice);

            if (device == null)
            {
                return;
            }

            _device = device;

            _keyWord = Encoding.Unicode.GetBytes(Resources.KeyWord);

            _device.OnPacketArrival += device_OnPaketArrival;

            _device.Open();
            _device.Filter = "tcp";
            _device.StartCapture();
        }

        private void device_OnPaketArrival(object sender, CaptureEventArgs e)
        {
            Packet packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            TcpPacket tcpPacket = packet.Extract(typeof(TcpPacket)) as TcpPacket;

            if (tcpPacket != null)
            {
                string msg = Encoding.ASCII.GetString(tcpPacket.PayloadData);

                if (tcpPacket.DestinationPort == _port || tcpPacket.SourcePort == _port)
                {
                    if (OptionTcp.UnsafeCompareByte(tcpPacket.PayloadData, _keyWord))
                    {
                        if (_isServer)
                        {
                            Drop();
                        }
                        else
                        {
                            Steg();
                        }
                    }
                }
            }
        }

        private void Drop()
        {
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

                    if (!WinDivertMethods.WinDivertRecv(handle, pack, (uint)pack.Length, ref addr, ref packetLen))
                    {
                        return;
                    }
                    WinDivertMethods.WinDivertHelperParsePacket(pack, packetLen, null, null, null, null, packHeader, null, null, null);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(Resources.ExceptionMessage, exp.Message);
            }
        }

        private void Steg()
        {
            try
            {
                byte[] pack = new byte[maxBuff];
                unsafe
                {
                    WINDIVERT_ADDRESS addr = new WINDIVERT_ADDRESS();
                    WINDIVERT_TCPHDR** packHeader = default(WINDIVERT_TCPHDR**);
                    uint packetLen = 0;
                    IntPtr handle = WinDivertMethods.WinDivertOpen(string.Format("tcp.SrcPort == {0}", _port),
                        WINDIVERT_LAYER.WINDIVERT_LAYER_NETWORK, 0, 0);

                    if (!WinDivertMethods.WinDivertRecv(handle, pack, (uint)pack.Length, ref addr, ref packetLen))
                    {
                        return;
                    }
                    WinDivertMethods.WinDivertHelperParsePacket(pack, packetLen, null, null, null, null, packHeader, null, null, null);

                    byte[] backPack = new byte[pack.Length];
                    Array.Copy(pack, backPack, pack.Length);
                    for (int i = 0; i < _stegWord.Length; i++)
                    {
                        backPack[41 + i] = Convert.ToByte(_stegWord[i]);
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(Resources.ExceptionMessage, exp.Message);
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