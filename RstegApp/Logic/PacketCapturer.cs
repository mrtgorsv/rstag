using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using PacketDotNet;
using RstegApp.Properties;
using SharpPcap;
using SharpPcap.WinPcap;

namespace RstegApp.Logic
{
    class PacketCapturer : MessageBus
    {
        private byte[] _keyWord;
        private string _stegWord;
        private int maxBuff = 0xFFFF;
        private readonly int _port;
        private int _dropPacketCount;
        private short _stegPacketCount;
        private readonly Random _random = new Random();
        private bool _dropPacket;
        private bool _stegPacket;
        private ICaptureDevice _device;

        private bool _isServer;

        public PacketCapturer(short port)
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
            if (_dropPacket || _stegPacket)
            {
                return;
            }
            Packet packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            TcpPacket tcpPacket = packet.Extract(typeof(TcpPacket)) as TcpPacket;

            if (tcpPacket != null)
            {

                if (tcpPacket.DestinationPort == _port)
                {
                    if (OptionTcp.UnsafeCompareByte(tcpPacket.PayloadData, _keyWord))
                    {
                        if (_isServer)
                        {
                            if (_dropPacketCount > 0)
                            {
                                return;
                            }
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
                _dropPacket = true;
                _dropPacketCount = _random.Next(2 , 4);
                IntPtr handle = WinDivertMethods.WinDivertOpen(string.Format("tcp.SrcPort == {0}", _port),
                    WINDIVERT_LAYER.WINDIVERT_LAYER_NETWORK, 0, 0);

                OnMessage(string.Format("Key found , start drop packets. Drop remains : {0}", _dropPacketCount));

                while (_dropPacketCount > 0)
                {
                    unsafe
                    {
                        uint packetLen = 0;
                        byte[] pack = new byte[maxBuff];
                        WINDIVERT_ADDRESS addr = new WINDIVERT_ADDRESS();
                        WINDIVERT_TCPHDR** packHeader = default(WINDIVERT_TCPHDR**);

                        if (HasError())
                        {
                            continue;
                        }

                        if (!WinDivertMethods.WinDivertRecv(handle, pack, (uint)pack.Length, ref addr, ref packetLen))
                        {
                            continue;
                        }

                        WinDivertMethods.WinDivertHelperParsePacket(pack, packetLen, null, null, null, null, packHeader, null, null, null);

                        _dropPacketCount--;

                        OnMessage(string.Format("Drop packet: length '{0}'. Remains: {1}", packetLen,  _dropPacketCount));
                    }
                }
                WinDivertMethods.WinDivertClose(handle);
                _dropPacket = false;
            }
            catch (Exception exp)
            {
                Console.WriteLine(Resources.ExceptionMessage, exp.Message);
            }
        }

        private bool HasError()
        {
            int errorCode = Marshal.GetLastWin32Error();

            switch (errorCode)
            {
                case 2:
                    OnMessage("The driver files WinDivert32.sys or WinDivert64.sys were not found.");
                    break;
                case 5:
                    OnMessage("The calling application does not have Administrator privileges");
                    break;
                case 87:
                    OnMessage("This indicates an invalid packet filter string, layer, priority, or flags");
                    break;
                case 577:
                    OnMessage("The WinDivert32.sys or WinDivert64.sys driver does not have a valid digital signature");
                    break;
                case 1275:
                    OnMessage("The WinDivert32.sys or WinDivert64.sys driver does not have a valid digital signature");
                    break;
                default:
                    return false;
            }
            return true;
        }

        public void Steg()
        {
            try
            {
                _stegPacket = true;
                int breakCount = 1;
                _stegPacketCount = 1;
                OnMessage(string.Format("Key found , add stegonography to next packets.Remains : {0}", _stegPacketCount));
                unsafe
                {
                    IntPtr handle = WinDivertMethods.WinDivertOpen(string.Format("tcp.DstPort == {0}", _port), WINDIVERT_LAYER.WINDIVERT_LAYER_NETWORK, 0, 0);

                    while (_stegPacketCount > 0)
                    {
                        byte[] pack = new byte[maxBuff];
                        WINDIVERT_ADDRESS addr = new WINDIVERT_ADDRESS();
                        WINDIVERT_TCPHDR** packHeader = default(WINDIVERT_TCPHDR**);
                        uint packetLen = 0;

                        if (!WinDivertMethods.WinDivertRecv(handle, pack, (uint)pack.Length, ref addr, ref packetLen))
                        {
                            continue;
                        }


                        WinDivertMethods.WinDivertHelperParsePacket(pack, packetLen, null, null, null, null, packHeader, null, null, null);

                        
                        if (breakCount > 0)
                        {
                            OnMessage("Send legal data...");
                        }
                        else
                        {
                            OnMessage("Stegonography addet to packet.");
                            for (int i = 0; i < _stegWord.Length; i++)
                            {
                                pack[41 + i] = Convert.ToByte(_stegWord[i]);
                            }
                        }

                        if (!WinDivertMethods.WinDivertSend(handle, pack, packetLen, ref addr, IntPtr.Zero))
                        {
                            OnMessage("Can\'t send packet");
                        }
                        else
                        {
                            if (breakCount == 0)
                            {
                                _stegPacketCount--;
                            }
                            else
                            {
                                breakCount--;
                            }
                        }
                    }
                    WinDivertMethods.WinDivertClose(handle);
                }
                _stegPacket = false;
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