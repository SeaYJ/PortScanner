using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace PortScanner.VM
{
    internal class ControlValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String info = "默认值")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private const int MIN_IP_VALUE = 0;             // IP 最大值
        private const int MAX_IP_VALUE = 255;           // IP 最小值

        public int[] IPStart { get; private set; }
            = new int[4];                               // IP 地址起始值

        public int[] IPEnd { get; private set; }
            = new int[4];                               // IP 地址终点值

        private bool[] IPStartInitFlag                  // IP 地址起始值页面初始化标志
            = new bool[4] { true, true, true, true };

        private bool[] IPEndInitFlag
            = new bool[4] { true, true, true, true };   // IP 地址终点值页面初始化标志

        public string IPS0
        {
            get
            {
                if (IPStartInitFlag[0])                         // 页面初始化时不提供具体值
                {
                    IPStartInitFlag[0] = false;
                    return "";
                }
                else                                            // 常规输出
                {
                    return string.Format($"{IPStart[0]}");
                }
            }
            set
            {
                if (!int.TryParse(value, out IPStart[0])
                    || int.Parse(value) < MIN_IP_VALUE)         // 输入非数字、小于最小值
                {
                    IPStart[0] = MIN_IP_VALUE;
                }
                else if (value.Equals(string.Empty)
                    || int.Parse(value) > MAX_IP_VALUE)         // 输入为空、大于最大值
                {
                    IPStart[0] = MAX_IP_VALUE;
                }
                else                                            // 常规输入
                {
                    IPStart[0] = int.Parse(value);
                }
            }
        }

        public string IPS1
        {
            get
            {
                if (IPStartInitFlag[1])                         // 页面初始化时不提供具体值
                {
                    IPStartInitFlag[1] = false;
                    return "";
                }
                else                                            // 常规输出
                {
                    return string.Format($"{IPStart[1]}");
                }
            }
            set
            {
                if (!int.TryParse(value, out IPStart[1])
                    || int.Parse(value) < MIN_IP_VALUE)         // 输入非数字、小于最小值
                {
                    IPStart[1] = MIN_IP_VALUE;
                }
                else if (value.Equals(string.Empty)
                    || int.Parse(value) > MAX_IP_VALUE)         // 输入为空、大于最大值
                {
                    IPStart[1] = MAX_IP_VALUE;
                }
                else                                            // 常规输入
                {
                    IPStart[1] = int.Parse(value);
                }
            }
        }

        public string IPS2
        {
            get
            {
                if (IPStartInitFlag[2])                         // 页面初始化时不提供具体值
                {
                    IPStartInitFlag[2] = false;
                    return "";
                }
                else                                            // 常规输出
                {
                    return string.Format($"{IPStart[2]}");
                }
            }
            set
            {
                if (!int.TryParse(value, out IPStart[2])
                    || int.Parse(value) < MIN_IP_VALUE)         // 输入非数字、小于最小值
                {
                    IPStart[2] = MIN_IP_VALUE;
                }
                else if (value.Equals(string.Empty)
                    || int.Parse(value) > MAX_IP_VALUE)         // 输入为空、大于最大值
                {
                    IPStart[2] = MAX_IP_VALUE;
                }
                else                                            // 常规输入
                {
                    IPStart[2] = int.Parse(value);
                }
            }
        }

        public string IPS3
        {
            get
            {
                if (IPStartInitFlag[3])                         // 页面初始化时不提供具体值
                {
                    IPStartInitFlag[3] = false;
                    return "";
                }
                else                                            // 常规输出
                {
                    return string.Format($"{IPStart[3]}");
                }
            }
            set
            {
                if (!int.TryParse(value, out IPStart[3])
                    || int.Parse(value) < MIN_IP_VALUE)         // 输入非数字、小于最小值
                {
                    IPStart[3] = MIN_IP_VALUE;
                }
                else if (value.Equals(string.Empty)
                    || int.Parse(value) > MAX_IP_VALUE)         // 输入为空、大于最大值
                {
                    IPStart[3] = MAX_IP_VALUE;
                }
                else                                            // 常规输入
                {
                    IPStart[3] = int.Parse(value);
                }
            }
        }

        public string IPE0
        {
            get
            {
                if (IPEndInitFlag[0])                           // 页面初始化时不提供具体值
                {
                    IPEndInitFlag[0] = false;
                    return "";
                }
                else                                            // 常规输出
                {
                    return string.Format($"{IPEnd[0]}");
                }
            }
            set
            {
                if (!int.TryParse(value, out IPEnd[0])
                    || int.Parse(value) < MIN_IP_VALUE)         // 输入非数字、小于最小值
                {
                    IPEnd[0] = MIN_IP_VALUE;
                }
                else if (value.Equals(string.Empty)
                    || int.Parse(value) > MAX_IP_VALUE)         // 输入为空、大于最大值
                {
                    IPEnd[0] = MAX_IP_VALUE;
                }
                else                                            // 常规输入
                {
                    IPEnd[0] = int.Parse(value);
                }
            }
        }

        public string IPE1
        {
            get
            {
                if (IPEndInitFlag[1])                           // 页面初始化时不提供具体值
                {
                    IPEndInitFlag[1] = false;
                    return "";
                }
                else                                            // 常规输出
                {
                    return string.Format($"{IPEnd[1]}");
                }
            }
            set
            {
                if (!int.TryParse(value, out IPEnd[1])
                    || int.Parse(value) < MIN_IP_VALUE)         // 输入非数字、小于最小值
                {
                    IPEnd[1] = MIN_IP_VALUE;
                }
                else if (value.Equals(string.Empty)
                    || int.Parse(value) > MAX_IP_VALUE)         // 输入为空、大于最大值
                {
                    IPEnd[1] = MAX_IP_VALUE;
                }
                else                                            // 常规输入
                {
                    IPEnd[1] = int.Parse(value);
                }
            }
        }

        public string IPE2
        {
            get
            {
                if (IPEndInitFlag[2])                           // 页面初始化时不提供具体值
                {
                    IPEndInitFlag[2] = false;
                    return "";
                }
                else                                            // 常规输出
                {
                    return string.Format($"{IPEnd[2]}");
                }
            }
            set
            {
                if (!int.TryParse(value, out IPEnd[2])
                    || int.Parse(value) < MIN_IP_VALUE)         // 输入非数字、小于最小值
                {
                    IPEnd[2] = MIN_IP_VALUE;
                }
                else if (value.Equals(string.Empty)
                    || int.Parse(value) > MAX_IP_VALUE)         // 输入为空、大于最大值
                {
                    IPEnd[2] = MAX_IP_VALUE;
                }
                else                                            // 常规输入
                {
                    IPEnd[2] = int.Parse(value);
                }
            }
        }

        public string IPE3
        {
            get
            {
                if (IPEndInitFlag[3])                           // 页面初始化时不提供具体值
                {
                    IPEndInitFlag[3] = false;
                    return "";
                }
                else                                            // 常规输出
                {
                    return string.Format($"{IPEnd[3]}");
                }
            }
            set
            {
                if (!int.TryParse(value, out IPEnd[3])
                    || int.Parse(value) < MIN_IP_VALUE)         // 输入非数字、小于最小值
                {
                    IPEnd[3] = MIN_IP_VALUE;
                }
                else if (value.Equals(string.Empty)
                    || int.Parse(value) > MAX_IP_VALUE)         // 输入为空、大于最大值
                {
                    IPEnd[3] = MAX_IP_VALUE;
                }
                else                                            // 常规输入
                {
                    IPEnd[3] = int.Parse(value);
                }
            }
        }

        private const int MIN_PORT_VALUE = 1;               // 端口最小值
        private const int MAX_PORT_VALUE = 65535;           // 端口最大值
        private int _PortStart;                             // 端口起始值
        private int _PortEnd;                               // 端口终点值
        private bool PortStartInitFlag = true;              // 端口起始值页面初始化标志
        private bool PortEndInitFlag = true;                // 端口终点值页面初始化标志

        public string PortStart
        {
            get
            {
                if (PortStartInitFlag)                      // 初始化页面不显示值
                {
                    PortStartInitFlag = false;
                    return "";
                }
                else                                        // 常规输出
                {
                    return string.Format($"{_PortStart}");
                }
            }
            set
            {
                if (!int.TryParse(value, out _PortStart)
                    || value.Equals(string.Empty)
                    || int.Parse(value) < MIN_PORT_VALUE)   // 输入非数字、为空、小于最小值
                {
                    _PortStart = MIN_PORT_VALUE;
                }
                else if (int.Parse(value) > MAX_PORT_VALUE) // 输入大于最大值
                {
                    _PortStart = MAX_PORT_VALUE;
                }
                else                                        // 常规输入
                {
                    _PortStart = int.Parse(value);
                }
            }
        }

        public string PortEnd
        {
            get
            {
                if (PortEndInitFlag)                        // 初始化显示值
                {
                    PortEndInitFlag = false;
                    return "";
                }
                else                                        // 常规输出
                {
                    return string.Format($"{_PortEnd}");
                }
            }
            set
            {
                if (!int.TryParse(value, out _PortEnd)
                    || value.Equals(string.Empty)
                    || int.Parse(value) < MIN_PORT_VALUE)   // 输入非数字、为空、小于最小值
                {
                    _PortEnd = MIN_PORT_VALUE;
                }
                else if (int.Parse(value) > MAX_PORT_VALUE) // 输入大于最大值
                {
                    _PortEnd = MAX_PORT_VALUE;
                }
                else                                        // 常规输入
                {
                    _PortEnd = int.Parse(value);
                }
            }
        }

        private uint _ThreadNum = 1;

        public uint ThreadNum
        {
            get { return _ThreadNum; }
            set { _ThreadNum = value; NotifyPropertyChanged(); }
        }

        private int _OverTime = 30;                             // 超时
        private const int MIN_OVERTIME_VALUE = 10;              // 超时最小值
        private const int MAX_OVERTIME_VALUE = 1000;              // 超时最大值

        public string OverTime
        {
            get
            {
                return string.Format($"{_OverTime}");
            }
            set
            {
                if (!int.TryParse(value, out _OverTime)
                    || value.Equals(string.Empty)
                    || int.Parse(value) > MAX_OVERTIME_VALUE)   // 输入非数字、为空、大于最大值
                {
                    _OverTime = MAX_OVERTIME_VALUE;
                }
                else if (int.Parse(value) < MIN_OVERTIME_VALUE) // 输入小于最小值
                {
                    _OverTime = MIN_OVERTIME_VALUE;
                }
                else                                            // 常规输入
                {
                    _OverTime = int.Parse(value);
                }
            }
        }

        private double _ProcessBarInfo = 0;

        public double ProcessBarInfo
        {
            get
            {
                return _ProcessBarInfo;
            }
            set
            {
                _ProcessBarInfo = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ScanningResultInfo> _ScanningResults = new ObservableCollection<ScanningResultInfo>();

        // 数据过滤函数

        private bool OnlyShowOpenFilterMethod(object DataSource)
        {
            // 注意数据过滤开启条件
            // 如果不需要数据过滤就一直返回 true
            return (_OnlyShowOpen ? (((DataSource as ScanningResultInfo).OUT_STATUS.IndexOf("Opening", StringComparison.OrdinalIgnoreCase) >= 0)) : (true));
        }

        public ObservableCollection<ScanningResultInfo> ScanningResults
        {
            get
            {
                // 设置数据过滤器
                CollectionView OnlyShowOpenFilter = (CollectionView)CollectionViewSource.GetDefaultView(_ScanningResults);
                OnlyShowOpenFilter.Filter = OnlyShowOpenFilterMethod;

                // 返回过滤后的数据
                return _ScanningResults;
            }
            set { _ScanningResults = value; NotifyPropertyChanged(); }
        }

        private string _ProcessDetails = "";

        public string ProcessDetails
        {
            get { return _ProcessDetails; }
            set { _ProcessDetails = value; NotifyPropertyChanged(); }
        }

        private bool _OnlyShowOpen = false;

        public bool OnlyShowOpen
        {
            get { return _OnlyShowOpen; }
            set
            {
                _OnlyShowOpen = value;
                NotifyPropertyChanged();

                // 更新列表过滤器状态（非常重要）
                CollectionViewSource.GetDefaultView(ScanningResults).Refresh();
            }
        }

        public ControlValue()
        {
        }
    }

    internal class ScanningResultInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String info = "默认值")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private string _OUT_IP;
        private string _OUT_PORT;
        private string _OUT_STATUS;
        private string _OUT_SERVER;

        public string OUT_IP
        {
            get { return _OUT_IP; }
            set { _OUT_IP = value; NotifyPropertyChanged(); }
        }

        public string OUT_PORT
        {
            get { return _OUT_PORT; }
            set { _OUT_PORT = value; NotifyPropertyChanged(); }
        }

        public string OUT_STATUS
        {
            get { return _OUT_STATUS; }
            set { _OUT_STATUS = value; NotifyPropertyChanged(); }
        }

        public string OUT_SERVER
        {
            get { return _OUT_SERVER; }
            set { _OUT_SERVER = value; NotifyPropertyChanged(); }
        }

        public ScanningResultInfo()
        {
            _OUT_IP = OUT_IP;
            _OUT_PORT = OUT_PORT;
            _OUT_STATUS = OUT_STATUS;
            _OUT_SERVER = OUT_SERVER;
        }

        public override string ToString()
        {
            return $"{{OUT_IP={OUT_IP}\tOUT_PORT={OUT_PORT}\tOUT_STAUTS={OUT_STATUS}\tOUT_SERVER={OUT_SERVER}}}";
        }
    }
}