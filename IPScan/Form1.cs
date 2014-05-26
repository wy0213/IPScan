// name： IPScan
// decription： scan active ip
// creator：wy yx
// creatTime：2014-5-17
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using SnmpSharpNet;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
namespace IPScan
{

    //定义委托????
    delegate void AddItemToListBoxDelegate(string str);
    delegate void DelegateChangeText();


    public partial class Form1 : Form
    {

        //用于存储由IP转化的整数
        long startIp;
        long endIp;
        //用于存储活动IP的List
        public List<IPMessage> ipList = new List<IPMessage>();
        //扫描主机的个数
        private int count = 0;
        //是否继续扫描,默认为继续
        private volatile int isScan = 1;
        //采用了多少种扫描方法
        int scanMethodNum = 1;
        public Form1()
        {
            InitializeComponent();

        }
        //委托函数—在listview中添加活动IP
        public void AddText(string text)
        {

            //判断是否能当前线程调用
            if (this.IPList.InvokeRequired)
            {
                AddItemToListBoxDelegate d = new AddItemToListBoxDelegate(AddText);
                this.Invoke(d, text);
            }
            else
            {
                MessageBox.Show(text);
                IPList.Items.Add(text);
            }
        }
        //检查IP地址是否匹配
        private bool IpCheck(string ip)
        {
            string regex = "(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)";
            return System.Text.RegularExpressions.Regex.IsMatch(ip, ("^" + regex + "\\." + regex + "\\." + regex + "\\." + regex + "$"));

        }
        //将地址解析为整数类型，
        private void ResolveIp(string ipstart, string ipend)
        {
            string[] ipArr = ipstart.Split('.');//解析起始地址           
            Int64 ipS1 = Int64.Parse(ipArr[0]);
            Int64 ipS2 = Int64.Parse(ipArr[1]);
            Int64 ipS3 = Int64.Parse(ipArr[2]);
            Int64 ipS4 = Int64.Parse(ipArr[3]);

            string[] ipEndArr = ipend.Split('.');//解析结束地址           
            Int64 ipE1 = Int64.Parse(ipEndArr[0]);
            Int64 ipE2 = Int64.Parse(ipEndArr[1]);
            Int64 ipE3 = Int64.Parse(ipEndArr[2]);
            Int64 ipE4 = Int64.Parse(ipEndArr[3]);

            startIp = ipS1 * 256 * 256 * 256 + ipS2 * 256 * 256 + ipS3 * 256 + ipS4;
            endIp = ipE1 * 256 * 256 * 256 + ipE2 * 256 * 256 + ipE3 * 256 + ipE4;
        }
        //将整数类型转换成IP地址形式
        private string IpTrs(long ipresolved)
        {
            long num = ipresolved;
            long i = (long)num / (long)(256 * 256 * 256);
            num -= i * (long)(256 * 256 * 256);
            long j = num / (long)(256 * 256);
            num -= j * (long)(256 * 256);
            long k = num / (long)256;
            num -= k * (long)256;
            long m = num;

            string strip = i.ToString() + "." + j.ToString() + "." + k.ToString() + "." + m.ToString();
            return strip;
        }
        //按钮扫描
        private void Scan_Click(object sender, EventArgs e)
        {
            //用于存储scan线程
            Thread scanThread;
            isScan = 1;
            count = 0;
            ScanProLab.Text = "扫描中";
            string startIP = StartIp.Text;
            string endIP = EndIp.Text;
            IPList.Items.Clear();
            progressBar1.Minimum = 0;
            //当此Value值等于Maximum值时，进度条将会被填满
            progressBar1.Value = 0;
            Scan.Enabled = false;
            Refresh();
            //将IP转换为整数数字
            ResolveIp(startIP, endIP);
            //表示需要扫描的范围上限的总数
            progressBar1.Maximum = Convert.ToInt32(endIp - startIp + 1) * scanMethodNum;
            if (startIP == "" || endIP == "")
            {
                MessageBox.Show(" 请输入起始ip和结束ip");
            }
            try
            {
                scanThread = new Thread(scan);
                scanThread.IsBackground = true;
                scanThread.Start();
                //IPList.Items.Add("活动主机个数:" + activecount);
            }
            catch { }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            this.isScan = 0;
            Scan.Enabled = true;
        }
        private void printActive_Click(object sender, EventArgs e)
        {
            if (ipList != null && ipList.Count > 0)
            {
                foreach (IPMessage ip in ipList)
                {
                    if (ip.state == 1)
                    {
                        IPList.Items.Add(ip.IpAds + "  活动");
                    }
                    else
                    {
                        IPList.Items.Add(ip.IpAds + "  下线");
                    }
                }
            }
            else
            {
                MessageBox.Show("还没有保存活动IP地址列表");
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            IPList.Items.Clear();
        }
        //根据此方法创建一个scan线程
        private void scan()
        {
            //重新扫描的时候假设其他人已经下线
            foreach (IPMessage ip in ipList)
            {
                ip.state = 0;
            }
            //创建扫描执行线程
            Thread creatPingThread;
            for (long x = startIp; x <= endIp && this.isScan == 1; x++)
            {
                count++;
                //进度条增加
                addProgressInvokeControl();
                //调用IpTrs（）函数，将X转换成IP地址
                string ip = IpTrs(x);

                #region 为每个IP扫描器创建一个线程
                IPMessage IP = new IPMessage();
                //使用线程会出现有的ip扫描了几遍，有的ip没有扫描 )
                creatPingThread = new Thread(IP.ScanIP);
                IP.IpAds = ip;
                IP.form = this;
                creatPingThread.IsBackground = true;
                creatPingThread.Start();
                Thread.Sleep(5);
                #endregion


            }
            Thread.Sleep(2000);
            //去除重复的扫描结果
            for (int i = 0; i < ipList.Count; i++)
            {
                for (int j = i + 1; j < ipList.Count; j++)
                {
                    if (ipList[i].IpAds.Equals(ipList[j].IpAds))
                    {
                        ipList.Remove(ipList[i]);
                        i--;
                        break;
                    }
                }
            }
            finishInvokeControl();

        }
        //跨线程控制控件需要用到委托的方法
        private void addProgressInvokeControl()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateChangeText(addProgress));
            }
            else
            {
                addProgress();
            }
        }
        //具体实现方法
        private void addProgress()
        {
            progressBar1.Value++;
        }
        //跨线程控制控件需要用到委托的方法
        private void finishInvokeControl()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateChangeText(finishScan));
            }
            else
            {
                finishScan();
            }
        }
        //具体实现方法
        private void finishScan()
        {
            ScanProLab.Text = "扫描结束";
            Scan.Enabled = true;
            //ipList = ipList.Distinct().ToList();//去掉list中的重复项

            IPList.Items.Add("共扫描主机个数：" + count);
            IPList.Items.Add("活动主机个数:" + ipList.Count);
        }

    }
    //此类用于线程调用时，存数据
    public class IPMessage
    {
        #region 导入ARP_API
        [DllImport("iphlpapi.dll")]
        static extern int SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen);
        [DllImport("Ws2_32.dll")]
        static extern Int32 inet_addr(string ipaddr);
        private const Int32 NUMBER_OF_PHYSICAL_ADDRESS_BYTES = 6;
        #endregion
        //IP地址
        public string IpAds;
        //物理地址
        public string macDest;
        //是否是活动主机（0-不是活动 1-活动主机）
        public int state = 0;
        //设备类型(0-未识别 1-电脑主机 2-路由器...)默认为主机
        public int equipType = 1;
        //引用控件类
        public Form1 form;

        public IPMessage() { }
        public void ScanIP() //设备地址扫描
        {
            arpScan();
            pingScan();
            if (this.state == 1)
            {
                judgeEquipType();
            }
        }
        //使用arp协议扫描IP
        public void arpScan()
        {
            try
            {
                StringBuilder macAddress = new StringBuilder();
                Int32 remote = inet_addr(this.IpAds);
                Int64 macInfo = new Int64();
                Int32 length = 6;
                int Result = SendARP(remote, 0, ref macInfo, ref length);
                string temp = Convert.ToString(macInfo, 16).PadLeft(12, '0').ToUpper();
                
                if (SendARP(remote, 0, ref macInfo, ref length) == 0)
                {
                    int x = 12;
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 5)
                        {
                            macAddress.Append(temp.Substring(x - 2, 2));
                        }
                        else
                        {
                            macAddress.Append(temp.Substring(x - 2, 2) + "-");
                        }
                        x -= 2;
                    }
                    macDest = macAddress.ToString();
                    this.state = 1;
                    form.ipList.Add(this);
                    //Console.WriteLine(this.IpAds + ":" + Result + ":" + macDest);
                }
                
            }
            catch{ }
        }
        //使用ping扫描IP
        public void pingScan()
        {
            try
            {
                Ping ping = new Ping();
                PingOptions op = new PingOptions();
                op.DontFragment = true;
                int timeout = 2000;
                string data = "aaa";
                byte[] buff = Encoding.ASCII.GetBytes(data);
                PingReply reply = ping.Send(IpAds, timeout, buff, op);
                if (reply.Status == IPStatus.Success)
                {
                    this.state = 1;
                    form.ipList.Add(this);
                }
            }
            catch { }
        }
        //判断设备类型
        public void judgeEquipType()
        {
            string hoststr = this.IpAds;
            string comstr = "public";
            byte[] resstr = new byte[1024];
            SNMP msg = new SNMP();
            //1.3.6.1.2.1.1.7.0……设备的网络功能（主机、路由器等）
            resstr = msg.getMessage("get", hoststr, comstr, "1.3.6.1.2.1.1.7.0");
            if (resstr[0] == 0xff)//没有收到来自host的消息，对方没有开启snmp服务
            {
                //MessageBox.Show("没有收到来自 " + hoststr + " 的消息。");
                return;
            }
            int comleng = Convert.ToInt16(resstr[6]);
            int mibleng = Convert.ToInt16(resstr[23 + comleng]);
            int datatype = Convert.ToInt16(resstr[24 + comleng + mibleng]);
            int dataleng = Convert.ToInt16(resstr[25 + comleng + mibleng]);
            int start = 26 + comleng + mibleng;
            //string outprint = Encoding.ASCII.GetString(resstr, start, dataleng);
            string outprint = BitConverter.ToString(resstr, start, dataleng);
            int num = Int32.Parse(outprint, System.Globalization.NumberStyles.HexNumber);
            if (num == 76 || num == 72)//主机
            {
                this.equipType = 1;
            }
            else if (num == 4)//路由器
            {
                this.equipType = 2;
            }
            else//无法识别
            {
                this.equipType = 0;
            }

        }
    }

    #region SNMP类
    class SNMP
    {
        public SNMP()
        {
        }
        public byte[] getMessage(string request, string hostString, string community, string mibStr)
        {
            //初始化发送的SNMP包数组
            byte[] sendPacket = new byte[1024];
            //初始化返回的SNMP包数组
            byte[] returnPacket = new byte[1024];
            //初始化存放MIB对象的数组
            byte[] mib = new byte[1024];
            //将mib字符串数组用"."分隔开 并存入mibvalue字符串数组
            string[] mibvalue = mibStr.Split('.');

            //获得mibvalue字符串的长度 作为mib消息的长度
            int miblength = mibvalue.Length;
            //当前的mib长度
            int orgmiblength = miblength;
            //获得团体名长度
            int comlength = community.Length;

            int current = 0, ps = 0, temp, i;
            //初始化SNMP包的长度
            int snmplength;

            //这段for循环 将mib字符串数组转换为byte类型 存入mib byte数组中
            for (i = 0; i < orgmiblength; i++)
            {
                //首先将字符串转换为16位Int型
                temp = Convert.ToInt16(mibvalue[i]);
                if (temp > 127)
                {
                    //若mib中对象的标识符大于128 则将其高位存在前面的数组中
                    mib[current] = Convert.ToByte(128 + (temp / 128));
                    mib[current + 1] = Convert.ToByte(temp - ((temp / 128) * 128));
                    //由于数字占了两位 下标加2
                    current = current + 2;
                    //mib长度只加1
                    miblength++;
                }
                else
                {
                    //若标识符小于128则直接转化为byte类型存入
                    mib[current] = Convert.ToByte(temp);
                    current++;
                }
            }

            //计算出SNMP包总长
            snmplength = 29 + comlength + miblength - 1;
            //开始填写SNMP包的版本
            //16进制数0x30代表传输开始
            sendPacket[ps++] = 0x30;
            //填写SNMP包的长度 为SNMP包的总长度减去传输开始和长度这2位
            sendPacket[ps++] = Convert.ToByte(snmplength - 2);

            //填写SNMP版本
            //0x02说明类型为整数型
            sendPacket[ps++] = 0x02;
            //0x01说明长度为1
            sendPacket[ps++] = 0x01;
            //0x00说明版本号为0
            sendPacket[ps++] = 0x00;

            //填写团体名
            //0x04说明类型为字符串型
            sendPacket[ps++] = 0x04;
            //将团体长度转换为byte类型并填入数组
            sendPacket[ps++] = Convert.ToByte(comlength);

            //将团体名转化为byte类型存入snmpData数组中
            byte[] snmpData = Encoding.ASCII.GetBytes(community);
            for (i = 0; i < snmpData.Length; i++)
            {
                //将byte类型 存入SNMP包中
                sendPacket[ps++] = snmpData[i];
            }

            //填写SNMP PDU
            //设置SNMP的PDU类型 若PDU信息为get 
            //填入A0表示 Request 格式
            if ("get" == request)
                sendPacket[ps++] = 0xA0;
            //填入A1表示 NextRequest 格式
            else
                sendPacket[ps++] = 0xA1;
            //填入PDU的数据长度
            sendPacket[ps++] = Convert.ToByte(20 + miblength - 1);

            //填写PDU请求ID
            //填写数据类型为整型
            sendPacket[ps++] = 0x02;
            //填写长度为4
            sendPacket[ps++] = 0x04;
            //填写请求ID
            sendPacket[ps++] = 0x00;
            sendPacket[ps++] = 0x00;
            sendPacket[ps++] = 0x00;
            sendPacket[ps++] = 0x01;

            //填写错误状态信息
            //填写整型类型
            sendPacket[ps++] = 0x02;
            //填写错误长度类型
            sendPacket[ps++] = 0x01;
            //填写错误状态类型
            sendPacket[ps++] = 0x00;

            //填写错误索引
            //填写整型类型
            sendPacket[ps++] = 0x02;
            //填写长度类型
            sendPacket[ps++] = 0x01;
            //填写错误状态类型
            sendPacket[ps++] = 0x00;

            //开始绑定变量
            //0x30说明变量绑定开始
            sendPacket[ps++] = 0x30;
            //填写变量长度
            sendPacket[ps++] = Convert.ToByte(6 + miblength - 1);
            //0x30开始填写第一个变量
            sendPacket[ps++] = 0x30;
            //填写变量长度
            sendPacket[ps++] = Convert.ToByte(6 + miblength - 1 - 2);

            //下面填写MIB类型
            //说明变量类型为对象 
            sendPacket[ps++] = 0x06;
            //填写对象类型
            sendPacket[ps++] = Convert.ToByte(miblength - 1);
            //MIB开始填写
            //用0x2b代表MIB对象的标识符1.3
            sendPacket[ps++] = 0x2B;
            //填写mib对象
            for (i = 2; i < miblength; i++)
                //根据长度开始填写MIB对象
                sendPacket[ps++] = Convert.ToByte(mib[i]);

            //填写最后结束类型为空
            sendPacket[ps++] = 0x05;
            //填写长度为0
            sendPacket[ps++] = 0x00;

            //下面编写发送程序
            //初始化Socket对象
            Socket sendSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //设置Socket的超时时间为5秒
            sendSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
            //使用传入的IP字符串来初始化为IPAddress 类型的IP地址
            IPAddress hostip = IPAddress.Parse(hostString);
            //从端口161发送信息
            IPEndPoint iep = new IPEndPoint(hostip, 161);
            EndPoint ep = (EndPoint)iep;

            try
            {
                //开始发送套接字
                sendSock.SendTo(sendPacket, snmplength, SocketFlags.None, iep);
            }
            catch (Exception e)
            {
                //若发送失败 弹出出错消息
                MessageBox.Show("与服务器连接失败");
                //把SNMP包的首字符改为 0xff 说明传送失败
                returnPacket[0] = 0xff;
                //传回SNMP包
                return returnPacket;
            }

            //若发送成功 
            try
            {
                //接收返回消息
                int recv = sendSock.ReceiveFrom(returnPacket, ref ep);
            }
            catch (SocketException)
            {
                //若接收失败 修改SNMP包首字符为0xff
                returnPacket[0] = 0xff;
            }
            //返回收到的SNMP包
            return returnPacket;
        }

        //将byte数组mibName转换为string类型
        public string nextMIBMessage(byte[] mibName)
        {
            //字符串首字符为"1.3"
            string outStr = "1.3";
            int cmutyleng = mibName[6];
            int start = 6 + cmutyleng + 17;
            int mibleng = mibName[start] - 1;
            //除去字符“1.3” 标志start加2
            start += 2;
            int mibVal;

            //使用for循环来将byte类型的mib标识符转化为字符串类型
            for (int i = start; i < start + mibleng; i++)
            {
                mibVal = Convert.ToInt16(mibName[i]);
                //若欲转换字符大于128 
                if (mibVal > 128)
                {
                    //则将高位先行放入字符串
                    mibVal = (mibVal / 128) * 128 + Convert.ToInt16(mibName[i + 1]);
                    i++;
                }
                //将每个字符加入MIB字符串outStr中
                outStr += "." + mibVal;
            }
            //将转换好的MIB字符串作为返回值返回
            return outStr;
        }
    }
    #endregion 
}
