using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketDotNet;
using RstegApp.Logic.Utils;
using RstegApp.Properties;
using SharpPcap;
using SharpPcap.WinPcap;

namespace RstegApp.Logic.Capture
{
    class PacketCapturer : MessageBus, IDisposable
    {
        private byte[] _keyWord;
        private string _stegWord;
        private const int MaxBuff = 0xFFFF;
        private readonly int _port;
        private int _dropPacketCount;
        private short _stegPacketCount;
        private readonly Random _random = new Random();
        private bool _dropPacket;
        private bool _stegPacket;
        private ICaptureDevice _device;
        private IntPtr _dropHandler;
        private IntPtr _stegHandler;
        private Task _dropTask;
        private Task _stegTask;
        private readonly CancellationTokenSource _dropTokenSource;
        private readonly CancellationTokenSource _stegTokenSource;

        private bool _isServer;

        public PacketCapturer(short port)
        {
            _port = port;
            _dropTokenSource = new CancellationTokenSource();
            _stegTokenSource = new CancellationTokenSource();
        }

        private CaptureDeviceList DeviceList
        {
            get { return CaptureDeviceList.Instance; }
        }

        public void StartCapturing(bool isServer)
        {
            _isServer = isServer;
            _stegWord = Resources.StegWord;

            var device = DeviceList.FirstOrDefault(d => d is WinPcapDevice);

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

                            _dropTask = Task.Factory.StartNew(Drop, _dropTokenSource.Token);
                        }
                        else
                        {
                            _stegTask = Task.Factory.StartNew(Steg, _stegTokenSource.Token);
                        }
                    }
                }
            }
        }

        private void Drop()
        {
            _dropTokenSource.Token.ThrowIfCancellationRequested();
            try
            {
                _dropPacket = true;
                _dropPacketCount = _random.Next(2, 4);
                _dropHandler = WinDivertMethods.WinDivertOpen(string.Format(Resources.Template_PacketFilter, _port),
                    WINDIVERT_LAYER.WINDIVERT_LAYER_NETWORK, 0, 0);

                OnMessage(Resources.DropPacketInitializeMessage);

                while (_dropPacketCount > 0)
                {
                    unsafe
                    {
                        uint packetLen = 0;
                        byte[] pack = new byte[MaxBuff];
                        WINDIVERT_ADDRESS addr = new WINDIVERT_ADDRESS();
                        WINDIVERT_TCPHDR** packHeader = default(WINDIVERT_TCPHDR**);

                        if (HasError())
                        {
                            continue;
                        }

                        if (
                            !WinDivertMethods.WinDivertRecv(_dropHandler, pack, (uint) pack.Length, ref addr,
                                ref packetLen))
                        {
                            continue;
                        }

                        WinDivertMethods.WinDivertHelperParsePacket(pack, packetLen, null, null, null, null, packHeader,
                            null, null, null);

                        _dropPacketCount--;

                        OnMessage(string.Format(Resources.Template_DropPacket, _dropPacketCount));
                    }
                }
                WinDivertMethods.WinDivertClose(_dropHandler);
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
                    OnMessage(Resources.WinDivertSysNotFoundMessage);
                    break;
                case 5:
                    OnMessage(Resources.ApplicationNotHavePrivilegesMessage);
                    break;
                case 87:
                    OnMessage(Resources.InvalidWinDivertPropertiesMessage);
                    break;
                case 577:
                    OnMessage(Resources.WinDivertDriverSignatureNotValidMessage);
                    break;
                case 1275:
                    OnMessage(Resources.WinDivertDriverBlockedMessage);
                    break;
                default:
                    return false;
            }
            return true;
        }

        private void Steg()
        {
            try
            {
                _stegPacket = true;
                int breakCount = 1;
                _stegPacketCount = 1;
                _stegTokenSource.Token.ThrowIfCancellationRequested();

                OnMessage(Resources.StegonographyInitializeMessage);
                unsafe
                {
                    _stegHandler = WinDivertMethods.WinDivertOpen(string.Format(Resources.Template_DestinationFilter, _port),
                        WINDIVERT_LAYER.WINDIVERT_LAYER_NETWORK, 0, 0);

                    while (_stegPacketCount > 0)
                    {
                        byte[] pack = new byte[MaxBuff];
                        WINDIVERT_ADDRESS addr = new WINDIVERT_ADDRESS();
                        WINDIVERT_TCPHDR** packHeader = default(WINDIVERT_TCPHDR**);
                        uint packetLen = 0;

                        if (
                            !WinDivertMethods.WinDivertRecv(_stegHandler, pack, (uint) pack.Length, ref addr,
                                ref packetLen))
                        {
                            continue;
                        }


                        WinDivertMethods.WinDivertHelperParsePacket(pack, packetLen, null, null, null, null, packHeader,
                            null, null, null);


                        if (breakCount > 0)
                        {
                            OnMessage(Resources.SendLegalDataMessage);
                        }
                        else
                        {
                            OnMessage(Resources.StegonographyAddedMessage);
                            for (int i = 0; i < _stegWord.Length; i++)
                            {
                                pack[41 + i] = Convert.ToByte(_stegWord[i]);
                            }
                        }

                        if (!WinDivertMethods.WinDivertSend(_stegHandler, pack, packetLen, ref addr, IntPtr.Zero))
                        {
                            OnMessage(Resources.SendPacketError);
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
                    WinDivertMethods.WinDivertClose(_stegHandler);
                }
                _stegPacket = false;
            }
            catch (Exception exp)
            {
                Console.WriteLine(Resources.ExceptionMessage, exp.Message);
            }
        }

        public void Dispose()
        {
            try
            {
                if (_device != null)
                {
                    _device.OnPacketArrival -= device_OnPaketArrival;
                    _device.Close();
                }
                if (_dropTask != null)
                {
                    _dropTokenSource.Cancel();
                }
                if (_stegTask != null)
                {
                    _stegTokenSource.Cancel();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}