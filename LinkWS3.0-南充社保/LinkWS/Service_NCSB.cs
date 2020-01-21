using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Collections;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using LinkWS.AllWebServiceFunc;

namespace LinkWS
{
    public partial class Service_NCSB : ServiceBase
    {
        public static string MyPath;
        public static string MyConfigFile;
        private static int iPort, iSendLen, iRecvLen;
        private static string strLogFile;
        private static INIClass MyIni;

        static EventLog log = new EventLog();
        public static string MyVersion = "3.0.1";
        static Socket clientsocket;
        const int MAXCONNECT = 20;
        static string logName;
        static FileStream myFs;
        static StreamWriter sw;
        static string logDate;

        public Service_NCSB()
        {
            
            InitializeComponent();         
            // 设置日志的名字            
            log.Source = "LinkWS-南充社保二期服务";// 设置日志的来源
            log.WriteEntry("启动LinkWS-南充社保二期服务" + MyVersion + "......", EventLogEntryType.Information);
            if (!init())
            {
                log.WriteEntry("系统初始化错误，请联系技术人员解决！", EventLogEntryType.Information);
                return;
            }            
            WriteLog("System(" + MyVersion + ") init is completed......");
        }

        /// <summary>
        /// 程序初始化
        /// </summary>
        /// <returns></returns>
        private static bool init()
        {
            string strTmp;

            MyPath = System.Environment.CurrentDirectory;
            MyPath = System.IO.Directory.GetCurrentDirectory();
            MyPath = System.AppDomain.CurrentDomain.BaseDirectory;
            MyConfigFile = MyPath + "Config.ini";
            log.WriteEntry(MyConfigFile, EventLogEntryType.Information);
            MyIni = new INIClass(MyConfigFile);

            if (!MyIni.ExistINIFile())
            {
                log.WriteEntry("Config.ini文件不存在", EventLogEntryType.Information);
                return false;
            }
            strTmp = MyIni.IniReadValue("SYSTEM", "LOG");
            logDate = DateTime.Now.ToString("yyyyMMdd");
            if (strTmp != "")
                logName = strTmp;
            else
                logName = "LWS"; 
            strLogFile = Service_NCSB.MyPath + "\\" + logName + "." + logDate + ".log";
            //if (!File.Exists(strLogFile))
            //{
            //    File.Create(strLogFile);
            //}
            myFs = new FileStream(strLogFile, FileMode.OpenOrCreate | FileMode.Append);
            sw = new StreamWriter(myFs);
            WriteLog("WriteLog start...");
            strTmp = MyIni.IniReadValue("SYSTEM", "Port");
            try
            {
                iPort = int.Parse(strTmp.Trim());
            }
            catch (Exception)
            {
                log.WriteEntry("端口配置错误！", EventLogEntryType.Information);
                WriteLog("端口配置错误！");
                return false;
            }

            strTmp = MyIni.IniReadValue("Comm", "SendLen");
            try
            {
                iSendLen = int.Parse(strTmp.Trim());
            }
            catch (Exception)
            {
                iSendLen = 0;
            }
            strTmp = MyIni.IniReadValue("Comm", "RecvLen");
            try
            {
                iRecvLen = int.Parse(strTmp.Trim());
            }
            catch (Exception)
            {
                iRecvLen = 0;
            }

            return true;
        }

        public static void WriteLog(string strLog)
        {
            string strLog_File, strLog_Cosole;

            if (logDate != DateTime.Now.ToString("yyyyMMdd"))
            {
                myFs.Close();
                logDate = DateTime.Now.ToString("yyyyMMdd");
                strLogFile = Service_NCSB.MyPath + "\\" + logName + "." + logDate + ".log";
                myFs = new FileStream(strLogFile, FileMode.OpenOrCreate | FileMode.Append);
                sw = new StreamWriter(myFs);
            }
            strLog_File = "[" + DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "]" + strLog;
            strLog_Cosole = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]\n" + strLog + "\n";
            sw.WriteLine(strLog_File);
            sw.Flush();
        }


        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            log.WriteEntry("启动服务......", EventLogEntryType.Information);
            
            System.Timers.Timer t = new System.Timers.Timer(1000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
            t.AutoReset = false;
            t.Enabled = true;
        }

        void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int iMaxWorkThreads, iMinWorkThreads, iWorkThreads, iPortThreads;
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, iPort);
            Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mySocket.Bind(ipEnd);
            mySocket.Listen(MAXCONNECT);//最大连接数
            WriteLog("Begin to listen[" + iPort.ToString() + "][" + MAXCONNECT + "]......");
            if (!NCDC.getConfigInfo())
            {
                WriteLog("getConfigInfo错误！！！");
                return;
            }

            ThreadPool.SetMinThreads(5, 5);
            ThreadPool.GetMaxThreads(out iMaxWorkThreads, out iPortThreads);
            ThreadPool.GetMinThreads(out iMinWorkThreads, out iPortThreads);
            WriteLog("线程池最大线程[" + iMaxWorkThreads.ToString() + "]最小线程[" + iMinWorkThreads.ToString() + "]");

            while (true)
            {
                try
                {
                    Socket s = mySocket.Accept();
                    clientsocket = s;
                    IPEndPoint ipEndClient = (IPEndPoint)clientsocket.RemoteEndPoint;
                    ThreadPool.GetAvailableThreads(out iWorkThreads, out iPortThreads);
                    WriteLog("线程池中新增新线程[" + ipEndClient.Address + "]当前可用线程数[" + iWorkThreads.ToString() + "]......");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ServiceClient), s);                    
                }
                catch (Exception err)
                {
                    WriteLog("守护进程异常退出！！！");
                    WriteLog(err.Message);
                }
            }

        }

        private static void ServiceClient(object thisSocket)
        {
            int recv;
            byte[] data = new byte[1024 * 8];
            string sProject;       //项目编号
            string sFuncName;      //函数名称
            ArrayList arrParas;       //变量组

            string sRecvPkg;          //接收数据包
            XmlDocument xRecvPkg;          //接收数据XML文档

            string sRetPkg;           //返回数据包            

            string sRetCode = "";          //返回码

            string sFuncValue = "";     //webservice函数返回值
            ArrayList arrRetInfos;       //返回信息组

            string strFormt;

            //Socket client = clientsocket;
            Socket client = (Socket)thisSocket;
            IPEndPoint ipEndClient = (IPEndPoint)client.RemoteEndPoint;

            string strTmp = "";
            WriteLog("Connect with [" + ipEndClient.Address + "]......");
            recv = client.Receive(data, iRecvLen, SocketFlags.None);
            strTmp = Encoding.GetEncoding("GB2312").GetString(data, 0, recv);
            WriteLog("报文长度:[" + strTmp + "]");
            try
            {
                recv = client.Receive(data, int.Parse(strTmp), SocketFlags.None);
            }
            catch (Exception err)
            {
                WriteLog("Exception:[" + err.Message + "]");
                WriteLog("报文长度错误:[" + strTmp + "]");
                sRetPkg = Func.MakeErrPkg(iRecvLen, "9981");
                client.Send(Encoding.Default.GetBytes(sRetPkg));
                WriteLog("应答报文:[" + sRetPkg + "]");
                WriteLog("Disconnect from [" + ipEndClient.Address + "]");
                client.Close();
                return;
            }

            sRecvPkg = Encoding.GetEncoding("GB2312").GetString(data, 0, recv);
            WriteLog("报文内容:[" + sRecvPkg + "]");

            xRecvPkg = new XmlDocument();
            int Index = 1;
            XmlNode xmlnd = null;
            string sPath;
            arrParas = new ArrayList();
            try
            {
                xRecvPkg.InnerXml = sRecvPkg;
                sProject = xRecvPkg.SelectSingleNode("ap/Project").InnerText;
                sFuncName = xRecvPkg.SelectSingleNode("ap/FuncName").InnerText;
                sPath = "ap/Para" + String.Format("{0:D2}", Index);
                xmlnd = xRecvPkg.SelectSingleNode(sPath);//获取<ap>/<Para01>的值
            }
            catch (Exception err)
            {
                WriteLog("Exception:[" + err.Message + "]");
                sRetPkg = Func.MakeErrPkg(iRecvLen, "9980");
                client.Send(Encoding.Default.GetBytes(sRetPkg));
                WriteLog("应答报文:[" + sRetPkg + "]");
                WriteLog("Disconnect from [" + ipEndClient.Address + "]");
                client.Close();
                return;
            }

            while (xmlnd != null)
            {
                arrParas.Insert(Index - 1, xmlnd.InnerXml);
                Index++;
                xmlnd = xRecvPkg.SelectSingleNode("ap/Para" + String.Format("{0:D2}", Index));
            } 

            if (arrParas.Count != 1)
            {
                WriteLog("参数个数错误！");
                sRetCode = "9982";
                sRetPkg = Func.MakeErrPkg(iRecvLen, sRetCode);
                client.Send(Encoding.Default.GetBytes(sRetPkg));
                WriteLog("应答报文:[" + sRetPkg + "]");
                WriteLog("Disconnect from [" + ipEndClient.Address + "]");
                client.Close();
                return;
            }

            //初始化返回变量数组
            arrRetInfos = new ArrayList();
            WriteLog("sProject=[" + sProject + "]");
            switch (sProject)
            {
                case "NCSB":
                    switch (sFuncName)
                    {
                        case "yhcall":
                            sFuncValue = NCSB.yhcall(arrParas[0].ToString(), ref sRetCode);
                            arrRetInfos.Insert(0, sFuncValue);
                            break;
                        case "yhconfirm":
                            sFuncValue = NCSB.yhconfirm(arrParas[0].ToString(), ref sRetCode);
                            arrRetInfos.Insert(0, sFuncValue);
                            break;
                        case "yhcancel":
                            sFuncValue = NCSB.yhcancel(arrParas[0].ToString(), ref sRetCode);
                            arrRetInfos.Insert(0, sFuncValue);
                            break;
                        default: sRetCode = "9998"; break;
                    }
                    break;
                case "NCDC":
                    sFuncValue = NCDC.szCall(arrParas[0].ToString(),sRecvPkg, ref sRetCode);
                    arrRetInfos.Insert(0, sFuncValue);
                    break;
                default:
                    sRetCode = "9984";
                    break;
            }
            //WriteLog("sFuncValue:[" + sFuncValue + "]");
            //WriteLog("arrRetInfos.count:[" + arrRetInfos.Count.ToString() + "]");
            XmlDocument doc = new XmlDocument();
            Func.InsertXmlNode(ref doc, "ap/RetCode", sRetCode, 1);
            Func.InsertXmlNode(ref doc, "ap/RetMsg", Func.GetRetMsg(sRetCode), 1);
            //WriteLog("arrRetInfos.Count:[" + arrRetInfos.Count.ToString() + "]");
            for (int i = 0; i < arrRetInfos.Count; i++)
            {
                //WriteLog("arrRetInfos[i].ToString():[" + arrRetInfos[i].ToString() + "]");
                Func.InsertXmlNode(ref doc, "ap/RetInfo" + String.Format("{0:D2}", i + 1), arrRetInfos[i].ToString(), 1);
            }

            sRetPkg = doc.OuterXml;
            //WriteLog("sRetPkg:[" + sRetPkg + "]");
            strFormt = "{0:D" + iSendLen.ToString() + "}";
            sRetPkg = String.Format(strFormt, Func.GetLength_Chinese(sRetPkg)) + sRetPkg;
            client.Send(Encoding.Default.GetBytes(sRetPkg));
            WriteLog("应答报文:[" + sRetPkg + "]");
            WriteLog("Disconnect from [" + ipEndClient.Address + "]");
            //client.Close();
         
        }     

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            log.WriteEntry("停止服务......", EventLogEntryType.Information);
        }
  
    }
}
