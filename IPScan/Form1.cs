// 名称： IPScan
// 描述： 扫描活跃的IP地址
// 创建人：wangying
// 创建时间：2014-5-17
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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
        public List<PingIp> ipList = new List<PingIp>();
        //扫描主机的个数
        private int count = 0;
        //是否继续扫描,默认为继续
        private volatile int isScan = 1;
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
            progressBar1.Maximum = Convert.ToInt32(endIp - startIp + 1);
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
                foreach (PingIp ip in ipList)
                {
                    if (ip.state==1)
                    {
                        IPList.Items.Add(ip.IpAds+"  活动");
                    }else{
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
            foreach (PingIp ip in ipList)
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
                //创建扫描IP的对象
                PingIp IP = new PingIp();
                //使用线程会出现有的ip扫描了几遍，有的ip没有扫描 )
                creatPingThread = new Thread(IP.pingIp);
                IP.IpAds = ip;
                IP.form = this;
                creatPingThread.IsBackground = true;
                creatPingThread.Start();
                Thread.Sleep(5);
            }
            Thread.Sleep(2000);
            //去除重复的扫描结果
            for (int i = 0; i < ipList.Count;i++ )
            {
                for (int j = i+1; j < ipList.Count; j++)
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
    public class PingIp
    {
        //IP地址
        public string IpAds;
        //是否是活动主机（默认为否）
        public int state = 0;
        //引用控件类
        public Form1 form;
        public PingIp() { }
        public void pingIp() //设备地址扫描
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
            catch (Exception ex) { }
        }

    }
}
