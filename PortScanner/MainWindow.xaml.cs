using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;
using PortScanner.VM;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.IO.Ports;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PortScanner
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        // 用于接受控件值

        private uint IPStart;
        private uint IPEnd;

        private uint PortStart;
        private uint PortEnd;

        // 特殊标记

        private const int MODE_IP = 0;                          // 按照 IP 进行分配线程
        private const int MODE_PORT = 1;                        // 按照端口进行分配线程

        private const string PORT_STATUS_OPENING = "Opening";   // 标记端口状态【开放】
        private const string PORT_STATUS_CLOSED = "Closed";     // 标记端口状态【关闭】

        // 控件交互变量
        private ControlValue CValue = new ControlValue();

        // 同于控制线程取消（结束）
        private static CancellationTokenSource cancellationTokenSource;

        private uint WaitScanTaskNum;
        private double ProcessBarStep;

        /// <summary>
        /// 扫描单个 IP 地址的某个端口
        /// </summary>
        /// <param name="IPAddress">IP 地址(使用标准格式)</param>
        /// <param name="Port">端口号</param>
        /// <param name="OverTime">超时时间(ms)</param>
        /// <returns>该 IP 的某个端口是否开启</returns>
        private bool SingleAddressSinglePortScanning(string IPAddress, uint Port, int OverTime)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                return tcpClient.ConnectAsync(IPAddress, (int)Port).Wait(OverTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取某个端口的服务名称（只收录固定的、已注册的，未注册但是默认的不被收录）
        /// </summary>
        /// <param name="Port">端口号</param>
        /// <returns>服务名称</returns>
        private string GetPortServerName(uint Port)
        {
            // 共计收录 188 个端口信息
            switch (Port)
            {
                case 1: return "TCPMUX";
                case 5: return "RJE";
                case 7: return "ECHO";
                case 9: return "DISCARD";
                case 11: return "SYSTAT";
                case 13: return "DAYTIME";
                case 17: return "QOTD";
                case 18: return "MSP";
                case 19: return "CHARGEN";
                case 20: return "FTP-DATA";
                case 21: return "FTP";
                case 22: return "SSH";
                case 23: return "TELNET";
                case 25: return "SMTP";
                case 37: return "TIME";
                case 39: return "RLP";
                case 42: return "NAMESERVER";
                case 43: return "NICNAME";
                case 49: return "TACACS";
                case 50: return "RE-MAIL-CK";
                case 53: return "DOMAIN";
                case 63: return "WHOIS++";
                case 67: return "BOOTPS";
                case 68: return "BOOTPC";
                case 69: return "TFTP";
                case 70: return "GOPHER";
                case 71: return "NETRJS-1";
                case 72: return "NETRJS-2";
                case 73: return "NETRJS-3";
                case 74: return "NETRJS-4";
                case 79: return "FINGER";
                case 80: return "HTTP";
                case 88: return "KERBEROS";
                case 95: return "SUPDUP";
                case 101: return "HOSTNAME";
                case 102: return "ISO-TSAP";
                case 105: return "CSNET-NS";
                case 107: return "RTELNET";
                case 109: return "POP2";
                case 110: return "POP3";
                case 111: return "SUNRPC";
                case 113: return "AUTH";
                case 115: return "SFTP";
                case 117: return "UUCP-PATH";
                case 119: return "NNTP";
                case 123: return "NTP";
                case 137: return "NETBIOS-NS";
                case 138: return "NETBIOS-DGM";
                case 139: return "NETBIOS-SSN";
                case 143: return "IMAP";
                case 161: return "SNMP";
                case 162: return "SNMPTRAP";
                case 163: return "CMIP-MAN";
                case 164: return "CMIP-AGENT";
                case 174: return "MAILQ";
                case 177: return "XDMCP";
                case 178: return "NEXTSTEP";
                case 179: return "BGP";
                case 191: return "PROSPERO";
                case 194: return "IRC";
                case 199: return "SMUX";
                case 201: return "AT-RTMP";
                case 202: return "AT-NBP";
                case 204: return "AT-ECHO";
                case 206: return "AT-ZIS";
                case 209: return "QMTP";
                case 210: return "Z39.50";
                case 213: return "IPX";
                case 220: return "IMAP3";
                case 245: return "LINK";
                case 347: return "FATSERV";
                case 363: return "RSVP_TUNNEL";
                case 369: return "RPC2PORTMAP";
                case 370: return "CODAAUTH2";
                case 372: return "ULISTPROC";
                case 389: return "LDAP";
                case 427: return "SVRLOC";
                case 434: return "MOBILEIP-AGENT";
                case 435: return "MOBILIP-MN";
                case 443: return "HTTPS";
                case 444: return "SNPP";
                case 445: return "MICROSOFT-DS";
                case 464: return "KPASSWD";
                case 468: return "PHOTURIS";
                case 487: return "SAFT";
                case 488: return "GSS-HTTP";
                case 496: return "PIM-RP-DISC";
                case 500: return "ISAKMP";
                case 535: return "IIOP";
                case 538: return "GDOMAP";
                case 546: return "DHCPV6-CLIENT";
                case 547: return "DHCPV6-SERVER";
                case 554: return "RTSP";
                case 563: return "NNTPS";
                case 565: return "WHOAMI";
                case 587: return "SUBMISSION";
                case 610: return "NPMP-LOCAL";
                case 611: return "NPMP-GUI";
                case 612: return "HMMP-IND";
                case 631: return "IPP";
                case 636: return "LDAPS";
                case 674: return "ACAP";
                case 694: return "HA-CLUSTER";
                case 749: return "KERBEROS-ADM";
                case 750: return "KERBEROS-IV";
                case 765: return "WEBSTER";
                case 767: return "PHONEBOOK";
                case 873: return "RSYNC";
                case 992: return "TELNETS";
                case 993: return "IMAPS";
                case 994: return "IRCS";
                case 995: return "POP3S";
                case 1080: return "SOCKS";
                case 1236: return "BVCONTROL [RMTCFG]";
                case 1300: return "H323HOSTCALLSC";
                case 1433: return "MS-SQL-S";
                case 1434: return "MS-SQL-M";
                case 1494: return "ICA";
                case 1512: return "WINS";
                case 1524: return "INGRESLOCK";
                case 1525: return "PROSPERO-NP";
                case 1645: return "DATAMETRICS [OLD-RADIUS]";
                case 1646: return "SA-MSG-PORT [OLDRADACCT]";
                case 1649: return "KERMIT";
                case 1701: return "L2TP [L2F]";
                case 1718: return "H323GATEDISC";
                case 1719: return "H323GATESTAT";
                case 1720: return "H323HOSTCALL";
                case 1758: return "TFTP-MCAST";
                case 1759: return "MTFTP";
                case 1789: return "HELLO";
                case 1812: return "RADIUS";
                case 1813: return "RADIUS-ACCT";
                case 1911: return "MTP";
                case 1985: return "HSRP";
                case 1986: return "LICENSEDAEMON";
                case 1997: return "GDP-PORT";
                case 2049: return "NFS [NFSD]";
                case 2102: return "ZEPHYR-SRV";
                case 2103: return "ZEPHYR-CLT";
                case 2104: return "ZEPHYR-HM";
                case 2401: return "CVSPSERVER";
                case 2430: return "VENUS";
                case 2431: return "VENUS-SE";
                case 2432: return "CODASRV";
                case 2433: return "CODASRV-SE";
                case 2600: return "HPSTGMGR [ZEBRASRV]";
                case 2601: return "DISCP-CLIENT [ZEBRA]";
                case 2602: return "DISCP-SERVER [RIPD]";
                case 2603: return "SERVICEMETER [RIPNGD]";
                case 2604: return "NSC-CCS [OSPFD]";
                case 2605: return "NSC-POSA";
                case 2606: return "NETMON [OSPF6D]";
                case 2809: return "CORBALOC";
                case 3130: return "ICPV2";
                case 3306: return "MYSQL";
                case 3346: return "TRNSPRNTPROXY";
                case 3389: return "MS-WBT-SERVER";
                case 4011: return "PXE";
                case 4321: return "RWHOIS";
                case 4444: return "KRB524";
                case 5002: return "RFE";
                case 5308: return "CFENGINE";
                case 5999: return "CVSUP [CVSUP]";
                case 6000: return "X11 [X]";
                case 7000: return "AFS3-FILESERVER";
                case 7001: return "AFS3-CALLBACK";
                case 7002: return "AFS3-PRSERVER";
                case 7003: return "AFS3-VLSERVER";
                case 7004: return "AFS3-KASERVER";
                case 7005: return "AFS3-VOLSER";
                case 7006: return "AFS3-ERRORS";
                case 7007: return "AFS3-BOS";
                case 7008: return "AFS3-UPDATE";
                case 7009: return "AFS3-RMTSYS";
                case 9876: return "SD";
                case 10080: return "AMANDA";
                case 11371: return "PGPKEYSERVER";
                case 11720: return "H323CALLSIGALT";
                case 13720: return "BPRD";
                case 13721: return "BPDBM";
                case 13722: return "BPJAVA-MSVC";
                case 13724: return "VNETD";
                case 13782: return "BPCD";
                case 13783: return "VOPIED";
                case 22273: return "WNN6 [WNN4]";
                case 26000: return "QUAKE";
                case 26208: return "WNN6-DS";
                case 33434: return "TRACEROUTE";
                default: return "-";
            }
        }

        /// <summary>
        /// 将 IP 地址转换成 IP 长整值
        /// </summary>
        /// <param name="IPAddress">标准格式的 IP 地址</param>
        /// <returns>对应 IP 地址的长整值</returns>
        private uint IPToInt(string IPAddress)
        {
            char[] separator = new char[] { '.' };
            string[] items = IPAddress.Split(separator);
            return uint.Parse(items[0]) << 24
                    | uint.Parse(items[1]) << 16
                    | uint.Parse(items[2]) << 8
                    | uint.Parse(items[3]);
        }

        /// <summary>
        /// 将 IP 长整值转换成 IP 地址
        /// </summary>
        /// <param name="IPAddressInt">长整值格式的 IP 地址</param>
        /// <returns>标准格式的 IP 地址</returns>
        private string IntToIP(uint IPAddressInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((IPAddressInt >> 24) & 0xFF).Append(".");
            sb.Append((IPAddressInt >> 16) & 0xFF).Append(".");
            sb.Append((IPAddressInt >> 8) & 0xFF).Append(".");
            sb.Append(IPAddressInt & 0xFF);
            return sb.ToString();
        }

        /// <summary>
        /// 检查 IP 地址合法性（完整检测）
        /// </summary>
        /// <param name="ip">标准格式的 IP 地址</param>
        /// <returns>是否合法</returns>
        private bool Check_IP_Legality(string ip)
        {
            Regex IP_REGEX = new Regex(@"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");
            return IP_REGEX.IsMatch(ip);
        }

        /// <summary>
        /// 检测端口合法性
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns>是否合法</returns>
        private bool Check_Port_Legality(uint port)
        {
            return (port >= 1 && port <= 65535);
        }

        public MainWindow()
        {
            InitializeComponent();
            AllowDragWindow();

            this.DataContext = CValue;
        }

        // 允许窗口拖拽
        private void AllowDragWindow()
        {
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };
        }

        // 窗口最小化按钮
        private void MinClose_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // 窗口关闭按钮
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // 打开主页
        private void HomePage_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://www.seayj.cn/");
        }

        private void SetBtnShowRel(int Status)
        {
            switch (Status)
            {
                case 0b1000:
                    this.START_BTN.IsEnabled = true;
                    this.END_BTN.IsEnabled = false;
                    this.SORT_BTN.IsEnabled = false;
                    this.EXPORT_BTN.IsEnabled = false;
                    break;

                case 0b0100:
                    this.START_BTN.IsEnabled = false;
                    this.END_BTN.IsEnabled = true;
                    this.SORT_BTN.IsEnabled = false;
                    this.EXPORT_BTN.IsEnabled = false;
                    break;

                case 0b1011:
                    this.START_BTN.IsEnabled = true;
                    this.END_BTN.IsEnabled = false;
                    this.SORT_BTN.IsEnabled = true;
                    this.EXPORT_BTN.IsEnabled = true;
                    break;

                case 0b1001:
                    this.START_BTN.IsEnabled = true;
                    this.END_BTN.IsEnabled = false;
                    this.SORT_BTN.IsEnabled = false;
                    this.EXPORT_BTN.IsEnabled = true;
                    break;

                default:
                    this.START_BTN.IsEnabled = true;
                    this.END_BTN.IsEnabled = false;
                    this.SORT_BTN.IsEnabled = false;
                    this.EXPORT_BTN.IsEnabled = false;
                    break;
            }
        }

        private void START_BTN_Click(object sender, RoutedEventArgs e)
        {
            // 新赋予一个值
            cancellationTokenSource = new CancellationTokenSource();

            // 控制按钮显示关系
            SetBtnShowRel(0b0100);

            // 清除【过程细节】的内容
            CValue.ProcessDetails = "";

            // 清除【扫描结果】的内容
            CValue.ScanningResults.Clear();

            // 进度条清0
            CValue.ProcessBarInfo = 0;

            // 获取正确的IP地址范围
            // 当扫描单个IP地址时，使 IPSart=IPEnd 即可！
            string tIPStart; string tIPEnd;
            if (CValue.IPS0 == string.Empty
                || CValue.IPS1 == string.Empty
                || CValue.IPS2 == string.Empty
                || CValue.IPS3 == string.Empty)    // 检查输入是否为空
            {
                MessageBox.Show("请输入正确的始IP地址！", "IP ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                SetBtnShowRel(0b1000);
                return;
            }
            tIPStart = string.Format($"{CValue.IPS0}.{CValue.IPS1}.{CValue.IPS2}.{CValue.IPS3}");// 获取输入
            if (!(bool)this.MULTIPLE_IP_SCANNING.IsChecked)// 扫描单个IP地址
            {
                tIPEnd = tIPStart;
            }
            else                                           // 扫描IP地址区间
            {
                if (CValue.IPE0 == string.Empty
                || CValue.IPE1 == string.Empty
                || CValue.IPE2 == string.Empty
                || CValue.IPE3 == string.Empty)    // 检查输入是否为空
                {
                    MessageBox.Show("请输入正确的末IP地址！", "IP ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    SetBtnShowRel(0b1000);
                    return;
                }
                tIPEnd = string.Format($"{CValue.IPE0}.{CValue.IPE1}.{CValue.IPE2}.{CValue.IPE3}");
            }
            if (!Check_IP_Legality(tIPStart)
                || !Check_IP_Legality(tIPEnd))   // 检查IP地址是否合法(虽然输入已经有检查，但是依然可能为空值)
            {
                MessageBox.Show("请输入正确的IP地址或IP地址区间！", "IP ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                SetBtnShowRel(0b1000);
                return;
            }
            uint t1 = IPToInt(tIPStart);
            uint t2 = IPToInt(tIPEnd);
            (IPStart, IPEnd) = ((t1 <= t2) ? (t1, t2) : (t2, t1));// IP地址起始关系修正

            // 获取正确的端口范围
            // 当扫描单个端口时，使 PortSart=PortEnd 即可！
            if (CValue.PortStart == string.Empty)   // 检查是否为空输入
            {
                MessageBox.Show($"请输入正确的始端口号！", "PORT ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                SetBtnShowRel(0b1000);
                return;
            }
            PortStart = uint.Parse(CValue.PortStart);
            if (!(bool)this.MULTIPORT_SCANNING.IsChecked)   // 扫描单个端口
            {
                PortEnd = PortStart;
            }
            else                                            // 扫描端口区间
            {
                if (CValue.PortEnd == string.Empty)
                {
                    MessageBox.Show($"请输入正确的终端口号！", "PORT ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    SetBtnShowRel(0b1000);
                    return;
                }
                PortEnd = uint.Parse(CValue.PortEnd);
            }
            if (!Check_Port_Legality(PortStart)
                || !Check_Port_Legality(PortEnd))   // 检查IP地址是否合法(虽然输入已经有检查，但是依然可能为空值)
            {
                MessageBox.Show("请输入正确的端口号或端口区间！", "PORT ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                SetBtnShowRel(0b1000);
                return;
            }
            if (PortStart > PortEnd)// 端口起始关系修正
            {
                uint t = PortEnd;
                PortEnd = PortStart;
                PortStart = t;
            }

            // 防止界面假死
            // 目前推测是过多启动线程进行大数据扫描时就会造成界面假死
            // 所以，当线程数过大时应该给予警告
            if (CValue.ThreadNum > 5)
            {
                MessageBox.Show("请根据自己的电脑性能合理决定多线程数哦~\n否则可能出现界面卡死现象！\n不过不用担心，界面卡死后本程序会一直在后台进行扫描工作。\n所以，只要耐心等待即可！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // 添加【过程细节】提示，准备开始扫描
            CValue.ProcessDetails += "-----开始扫描-----\n";

            // 计算出总共需要进行的扫描次数
            WaitScanTaskNum = (IPEnd - IPStart + 1) * (PortEnd - PortStart + 1);

            // 计算进度条每次扫描需要前进的步长
            ProcessBarStep = 100.0 / WaitScanTaskNum;

            // 开始扫描
            List<Task> Tasks = new List<Task>();            // 线程列表
            if (IPStart == IPEnd && PortStart == PortEnd)   // 只需启动一个线程扫描
            {
                // 新建一个线程进行扫描
                Tasks.Add(Task.Run(() => ScanningTaskMethod(0, 1, MODE_IP), cancellationTokenSource.Token));
            }
            // 为了增强代码可读性，就不进行合并了
            else if (IPStart == IPEnd && PortStart < PortEnd)// 按照端口启动多线程
            {
                // 计算实际需要启动的线程数
                int NumOfThreadRequired = (int)Math.Min(CValue.ThreadNum, (PortEnd - PortStart + 1));

                // 注意：使用 Math.Min() 控制线程最大数，避免启动不必要的线程
                // 说明：当允许使用的线程数大于实际扫描的端口总数时，就会有多余的线程资源不需要干活，所以他们是没必要开启的
                for (int i = 0; i < NumOfThreadRequired; i++)
                {
                    var TaskID = i;// 这个变量是必须的，否则启动线程时会出现BUG（值得注意/暂时不清楚原因）
                    Tasks.Add(Task.Run(() => ScanningTaskMethod(TaskID, NumOfThreadRequired, MODE_PORT), cancellationTokenSource.Token));
                }
            }
            else if (IPStart < IPEnd && PortStart <= PortEnd)// 按照IP地址启动多线程
            {
                // 计算实际需要启动的线程数
                int NumOfThreadRequired = (int)Math.Min(CValue.ThreadNum, (IPEnd - IPStart + 1));

                // 注意：使用 Math.Min() 控制线程最大数，避免启动不必要的线程
                // 说明：当允许使用的线程数大于实际扫描的IP地址总数时，就会有多余的线程资源不需要干活，所以他们是没必要开启的
                for (int i = 0; i < NumOfThreadRequired; i++)
                {
                    var TaskID = i;// 这个变量是必须的，否则启动线程时会出现BUG（值得注意/暂时不清楚原因）
                    Tasks.Add(Task.Run(() => ScanningTaskMethod(TaskID, NumOfThreadRequired, MODE_IP), cancellationTokenSource.Token));
                }
            }

            Task.WhenAll(Tasks).ContinueWith(t =>
            {
                // 通知完成扫描
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    MessageBox.Show("扫描取消！", "通知", MessageBoxButton.OK, MessageBoxImage.Information);
                    CValue.ProcessDetails += "-----取消扫描-----\n";
                }
                else
                {
                    MessageBox.Show("扫描完成！", "通知", MessageBoxButton.OK, MessageBoxImage.Information);
                    CValue.ProcessDetails += "-----扫描完成-----\n";
                }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    SetBtnShowRel(0b1011);
                }));
            });
        }

        private void ScanningTaskMethod(int TaskID, int ThreadNum, int Mode)
        {
            // 第一层 For 循环为 IP 控制层， 第二层 For 循环为 Port 控制层
            // 注意 TaskID 是从 0 开始的
            // 注意分配线程是按照线程数依次分配任务（IP 模式和 Port 模式是互斥的/只能存在一个）
            for (int i = ((Mode == MODE_IP) ? (TaskID) : (0)); i <= (int)(IPEnd - IPStart); i += ((Mode == MODE_IP) ? (ThreadNum) : (1)))
            {
                // 两个变量是为了避免溢出
                // 因为 IP 全地址总共有 4,294,967,295 个，而 UInt32 的范围为 [0, 4,294,967,295]，所以二者是双射关系（单射+满射）
                // 如果使用 uint i = IPStart 开始扫描，那当 IPStart 为 255.255.255.255 对应的长整值 4,294,967,295 时，i++就会溢出。
                // 所以，这里采用 int i = 0; i < IPEnd - IPStart 的方式进行循环控制
                string ip = IntToIP((uint)(i + IPStart));

                // 添加【过程细节】信息
                // 注意这里的判断：MODE_PORT 模式下多个线程扫描一个 IP 会多次输出同样的信息
                if (Mode == MODE_IP || (Mode == MODE_PORT && TaskID == 0))

                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        CValue.ProcessDetails += $"正在扫描 IP 地址\t{ip}\t...\n";
                    }));
                }

                for (int j = ((Mode == MODE_PORT) ? (TaskID) : (0)); j <= (int)(PortEnd - PortStart); j += ((Mode == MODE_PORT) ? (ThreadNum) : (1)))
                {
                    // 扫描任务被取消
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        Console.WriteLine($"线程 {Task.CurrentId} 退出！");
                        return;
                    }

                    // 获取端口号
                    uint port = (uint)(j + PortStart);

                    // 获取端口状态
                    string port_status =
                        SingleAddressSinglePortScanning(ip, port, int.Parse(CValue.OverTime)) ? (PORT_STATUS_OPENING) : (PORT_STATUS_CLOSED);

                    // 生成一个扫描结果
                    ScanningResultInfo scanningResultInfo = new ScanningResultInfo()
                    {
                        OUT_IP = $"{ip}",                  // 扫描的IP地址
                        OUT_PORT = $"{port}",                       // 扫描的端口号
                        OUT_STATUS = $"{port_status}",
                        OUT_SERVER = $"{GetPortServerName(port)}"   // 扫描端口的服务名称
                    };

                    // 牺牲一点时间，给UI线程一点缓冲时间，防止数据量过大时卡死UI线程
                    //Thread.Sleep(ThreadNum * 5);

                    // 委托 UI 线程添加一条扫描结果
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        CValue.ProcessBarInfo += ProcessBarStep;
                        CValue.ScanningResults.Add(scanningResultInfo);
                    }));
                }
            }
        }

        private void END_BTN_Click(object sender, RoutedEventArgs e)
        {
            // 控制按钮显示关系
            SetBtnShowRel(0b1011);

            cancellationTokenSource.Cancel();
        }

        private void EXPORT_BTN_Click(object sender, RoutedEventArgs e)
        {
        }

        private void START_IP_0_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter
                || e.Key == System.Windows.Input.Key.OemPeriod
                || e.Key == System.Windows.Input.Key.Decimal)
            {
                this.START_IP_1.Focus();
            }
        }

        private void START_IP_1_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter
                || e.Key == System.Windows.Input.Key.OemPeriod
                || e.Key == System.Windows.Input.Key.Decimal)
            {
                this.START_IP_2.Focus();
            }
            else if (this.START_IP_1.Text == string.Empty && e.Key == System.Windows.Input.Key.Back && e.IsToggled)
            {
                this.START_IP_0.Focus();
            }
        }

        private void START_IP_2_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter
                || e.Key == System.Windows.Input.Key.OemPeriod
                || e.Key == System.Windows.Input.Key.Decimal)
            {
                this.START_IP_3.Focus();
            }
            else if (this.START_IP_2.Text == string.Empty && e.Key == System.Windows.Input.Key.Back && e.IsToggled)
            {
                this.START_IP_1.Focus();
            }
        }

        private void START_IP_3_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (this.END_IP_0.IsEnabled)
                {
                    this.END_IP_0.Focus();
                }
                else
                {
                    this.START_PORT.Focus();
                }
            }
            else if (this.START_IP_3.Text == string.Empty && e.Key == System.Windows.Input.Key.Back && e.IsToggled)
            {
                this.START_IP_2.Focus();
            }
        }

        private void END_IP_0_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter
                || e.Key == System.Windows.Input.Key.OemPeriod
                || e.Key == System.Windows.Input.Key.Decimal)
            {
                this.END_IP_1.Focus();
            }
        }

        private void END_IP_1_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter
                || e.Key == System.Windows.Input.Key.OemPeriod
                || e.Key == System.Windows.Input.Key.Decimal)
            {
                this.END_IP_2.Focus();
            }
            else if (this.END_IP_1.Text == string.Empty && e.Key == System.Windows.Input.Key.Back && e.IsToggled)
            {
                this.END_IP_0.Focus();
            }
        }

        private void END_IP_2_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter
                || e.Key == System.Windows.Input.Key.OemPeriod
                || e.Key == System.Windows.Input.Key.Decimal)
            {
                this.END_IP_3.Focus();
            }
            else if (this.END_IP_2.Text == string.Empty && e.Key == System.Windows.Input.Key.Back && e.IsToggled)
            {
                this.END_IP_1.Focus();
            }
        }

        private void END_IP_3_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.START_PORT.Focus();
            }
            else if (this.END_IP_3.Text == string.Empty && e.Key == System.Windows.Input.Key.Back && e.IsToggled)
            {
                this.END_IP_2.Focus();
            }
        }

        private void START_PORT_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (this.END_PORT.IsEnabled)
                {
                    this.END_PORT.Focus();
                }
                else
                {
                    this.START_BTN.Focus();
                }
            }
        }

        private void END_PORT_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.START_BTN.Focus();
            }
        }

        private void SORT_BTN_Click(object sender, RoutedEventArgs e)
        {
            SetBtnShowRel(0b1001);
            Task.WhenAll(Task.Run(() =>
            {
                CValue.ScanningResults = new ObservableCollection<ScanningResultInfo>(
                        CValue.ScanningResults.OrderBy(s => IPToInt(s.OUT_IP)).ThenBy(s => uint.Parse(s.OUT_PORT))
                    );
            })).ContinueWith(t =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MessageBox.Show("已将结果进行排序！", "通知", MessageBoxButton.OK, MessageBoxImage.Information);
                    SetBtnShowRel(0b1011);
                }));
            });
        }
    }

    /// <summary>
    /// 当 TextBoxBase获得焦点的时候，自动全部选择文字。附加属性为SelectAllWhenGotFocus，类型为bool.
    /// </summary>
    public class TextBoxAutoSelectHelper
    {
        public static readonly DependencyProperty SelectAllWhenGotFocusProperty = DependencyProperty.RegisterAttached("SelectAllWhenGotFocus",
                            typeof(bool), typeof(TextBoxAutoSelectHelper),
                            new FrameworkPropertyMetadata((bool)false, new PropertyChangedCallback(OnSelectAllWhenGotFocusChanged)));

        public static bool GetSelectAllWhenGotFocus(TextBoxBase d)
        {
            return (bool)d.GetValue(SelectAllWhenGotFocusProperty);
        }

        public static void SetSelectAllWhenGotFocus(TextBoxBase d, bool value)
        {
            d.SetValue(SelectAllWhenGotFocusProperty, value);
        }

        private static void OnSelectAllWhenGotFocusChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs e)
        {
            if (dependency is TextBoxBase tBox)
            {
                var isSelectedAllWhenGotFocus = (bool)e.NewValue;
                if (isSelectedAllWhenGotFocus)
                {
                    tBox.PreviewMouseDown += TextBoxPreviewMouseDown;
                    tBox.GotFocus += TextBoxOnGotFocus;
                    tBox.LostFocus += TextBoxOnLostFocus;
                }
                else
                {
                    tBox.PreviewMouseDown -= TextBoxPreviewMouseDown;
                    tBox.GotFocus -= TextBoxOnGotFocus;
                    tBox.LostFocus -= TextBoxOnLostFocus;
                }
            }
        }

        private static void TextBoxOnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBoxBase tBox)
            {
                tBox.SelectAll();
                tBox.PreviewMouseDown -= TextBoxPreviewMouseDown;
            }
        }

        private static void TextBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBoxBase tBox)
            {
                tBox.Focus();
                e.Handled = true;
            }
        }

        private static void TextBoxOnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBoxBase tBox)
            {
                tBox.PreviewMouseDown += TextBoxPreviewMouseDown;
            }
        }
    }
}