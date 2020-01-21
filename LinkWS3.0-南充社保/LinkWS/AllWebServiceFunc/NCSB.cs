using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Collections;

namespace LinkWS.AllWebServiceFunc
{
    class NCSB
    {

        static LinkWS.WebR_NCSB.yhjypt webService;
        static private string jybm,yhbm,jylsh,yhwdh,yhwdmc,jymm;

        static private Cfg cfg_NCSB;

        static public bool getConfigInfo()
        {
            try
            {
                INIClass inicls = new INIClass(Service_NCSB.MyConfigFile);
                if (!inicls.ExistINIFile())
                {
                    Service_NCSB.WriteLog("inicls错误");
                    return false;
                }
                cfg_NCSB = new Cfg();
                cfg_NCSB.WSip = inicls.IniReadValue("NCSB", "WS_IP");
                cfg_NCSB.WSport = inicls.IniReadValue("NCSB", "WS_PORT");
                cfg_NCSB.WStimeout = inicls.IniReadValue("NCSB", "WS_TIMEOUT");
                cfg_NCSB.ftp_ip = inicls.IniReadValue("NCSB", "FTP_IP");
                cfg_NCSB.ftp_user = inicls.IniReadValue("NCSB", "FTP_USER");
                cfg_NCSB.ftp_passwd = inicls.IniReadValue("NCSB", "FTP_PASSWD");
                cfg_NCSB.ftp_localpath = inicls.IniReadValue("NCSB", "FTP_LOCALPATH");
                if (cfg_NCSB.ftp_ip == "" || cfg_NCSB.ftp_user == "" || cfg_NCSB.ftp_passwd == "")
                {
                    Service_NCSB.WriteLog("ftp配置错误");
                    return false;
                }
                if (cfg_NCSB.WSip == "" || cfg_NCSB.WSport == "")
                {
                    Service_NCSB.WriteLog("WS配置错误");
                    return false;
                }
                if (cfg_NCSB.WStimeout == "")
                    cfg_NCSB.WStimeout = "200000";
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("getConfigInfo错误" + err.Message);
                return false;
            }

            return true;

        }

        static private bool init()
        {
            webService = new LinkWS.WebR_NCSB.yhjypt();

            INIClass inicls = new INIClass(Service_NCSB.MyConfigFile);
            if (!inicls.ExistINIFile())
            {
                return false;
            }
            string WSip = inicls.IniReadValue("NCSB", "WS_IP");
            string WSport = inicls.IniReadValue("NCSB", "WS_PORT");
            string WStimeout = inicls.IniReadValue("NCSB", "WS_TIMEOUT");  

            if (WSip == "" || WSport == "" )
            {
                return false;
            }
            if (WStimeout == "")
            {
                WStimeout = "200000";
            }
            webService.Timeout = int.Parse(WStimeout);

            webService.Url = "http://" + WSip + ":" + WSport + "/yhjypt/yhjypt.asmx";
            return true;
        }

        /// <summary>
        ///南充社保接口
        /// </summary>
        /// <param name="XmlPara01">交易编号</param>      
        /// <returns>WS返回字符串</returns>
        public static string yhcall(string XmlPara01, ref string retcode)
        {
            string Astr_jyh, Astr_jylsh, Astr_lydz, Astr_ydz,errinfo="", strRecvXml = "";
            byte[] Astr_jyjg=new byte[128], Astr_clxx=new byte[128], Astr_jysj=new byte[128];
            int Aint_clbz=0;

            if (!init())
            {
                Service_NCSB.WriteLog("init 错误");
                strRecvXml = "";
                retcode = "0998";
                return strRecvXml;
            }

            XmlPara01 = "<Para01>" + XmlPara01 + "</Para01>";
            Service_NCSB.WriteLog("XmlPara01[" + XmlPara01 + "]");
            XmlDocument xReqPara = new XmlDocument();
            try
            {
                xReqPara.InnerXml = XmlPara01;
                Astr_jyh = xReqPara.SelectSingleNode("Para01/Astr_jyh").InnerText;
                //Service_NCSB.WriteLog("Astr_jyh[" + Astr_jyh + "]");
                Astr_jylsh = xReqPara.SelectSingleNode("Para01/Astr_jylsh").InnerText;
                //Service_NCSB.WriteLog("Astr_jylsh[" + Astr_jylsh + "]");
                Astr_lydz = xReqPara.SelectSingleNode("Para01/Astr_lydz").InnerText;
                //Service_NCSB.WriteLog("Astr_lydz[" + Astr_lydz + "]");
                Astr_ydz = xReqPara.SelectSingleNode("Para01/Astr_ydz").InnerText;
                //Service_NCSB.WriteLog("Astr_ydz[" + Astr_ydz + "]");
                if (Astr_jyh == "003" || Astr_jyh == "004")
                {//处理性交易，需要申请流水号
                    //Astr_jylsh = getjylsh(Astr_lydz, Astr_ydz);
                    if (Astr_jylsh == "")
                    {
                        Service_NCSB.WriteLog("申请交易流水号不能为空:[" + Astr_lydz + "|" + Astr_ydz + "]");
                        strRecvXml = "";
                        retcode = "0998";
                        return strRecvXml;
                    }
                    Service_NCSB.WriteLog("jylsh=[" + jylsh + "]");
                }
                Astr_jysj = Encoding.Default.GetBytes(xReqPara.SelectSingleNode("Para01/Astr_jysj").InnerXml);
                             
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("Exception:[" + err.Message + "]");
                strRecvXml = "";
                retcode = "0998";
                return strRecvXml;
            }
            if (Astr_jyh=="998")//对帐文件上传成功，可以对帐
            {
                string jysj,wjdx;
                string wjmc = xReqPara.SelectSingleNode("Para01/Astr_jysj/jysj/wjmc").InnerText;
                
                
                if (!File.Exists(cfg_NCSB.ftp_localpath + "\\" + wjmc))
                {
                    Service_NCSB.WriteLog("ftp文件不存在!");
                    strRecvXml = "";
                    retcode = "9989";
                    return strRecvXml;
                }

                if (!Func.StrFile2XmlFile(cfg_NCSB.ftp_localpath + "/" + wjmc, cfg_NCSB.ftp_localpath + "\\NCSB\\" + wjmc + ".txt" , out strRecvXml))
                {
                    Service_NCSB.WriteLog(strRecvXml);
                    strRecvXml = "";
                    retcode = "9989";
                    return strRecvXml;
                }
                Service_NCSB.WriteLog("StrFile2XmlFile end.");
                FileInfo fi = new FileInfo(cfg_NCSB.ftp_localpath + "\\NCSB\\" + wjmc + ".txt");
                Astr_jysj = Encoding.Default.GetBytes("<jysj><wjmc>" + wjmc + "</wjmc><wjdx>" + fi.Length + "</wjdx></jysj>");

                try
                {
                    FTP ftpObj;

                    Service_NCSB.WriteLog("ftp_ip=[" + cfg_NCSB.ftp_ip + "]");
                    ftpObj = new FTP(cfg_NCSB.ftp_ip, cfg_NCSB.ftp_user, cfg_NCSB.ftp_passwd);
                    Service_NCSB.WriteLog("FTP Create !");

                    if (!ftpObj.Put("", cfg_NCSB.ftp_localpath + "\\NCSB\\" + wjmc + ".txt", out errinfo))
                    {
                        Service_NCSB.WriteLog("Exception:[" + errinfo + "]");
                        strRecvXml = "";
                        retcode = "0998";
                        return strRecvXml;
                    }
                }
                catch (Exception err)
                {
                    Service_NCSB.WriteLog(err.Message);
                    strRecvXml = "";
                    retcode = "0998";
                    return strRecvXml;
                }
            }
            Service_NCSB.WriteLog("Astr_jyh[" + Astr_jyh + "]");
            Service_NCSB.WriteLog("Astr_jylsh[" + Astr_jylsh + "]");
            Service_NCSB.WriteLog("Astr_jysj[" + Encoding.Default.GetString(Astr_jysj) + "]");
            Service_NCSB.WriteLog("Astr_lydz[" + Astr_lydz + "]");
            Service_NCSB.WriteLog("Astr_ydz[" + Astr_ydz + "]");

            try
            {
                webService.yhcall(Astr_jyh, Astr_jylsh, Astr_jysj, Astr_lydz, Astr_ydz, ref Astr_jyjg, ref Aint_clbz, ref Astr_clxx);
                strRecvXml = "\r\n<Astr_jyjg>" + Encoding.Default.GetString(Astr_jyjg) + "</Astr_jyjg>\r\n";
                strRecvXml += "<Aint_clbz>" + Aint_clbz + "</Aint_clbz>\r\n";                
                string strTmp = Encoding.Default.GetString(Astr_clxx);
                Service_NCSB.WriteLog("Astr_clxx=[" + strTmp + "]");
                if (strTmp.Length >= 32) strTmp = strTmp.Substring(0, 32);
                strRecvXml += "<Astr_clxx>" + strTmp + "</Astr_clxx>\r\n";
                //int iTmp = strTmp.IndexOf("jysj:<jysj>");
                //if (iTmp>0)
                //    strRecvXml += "<Astr_clxx>" + strTmp.Substring(0, strTmp.IndexOf("jysj:<jysj>") + 4) + "</Astr_clxx>\r\n";
                //else
                //    strRecvXml += "<Astr_clxx>" + strTmp + "</Astr_clxx>\r\n";
                //Service_NCSB.WriteLog("strRecvXml:[" + strRecvXml + "]");
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("err=[" + err.Message + "]");
                retcode = "9989";
                strRecvXml = "";
                return strRecvXml;
            }
            
            //if (Aint_clbz <= 0 )
            //{
            //    Service_NCSB.WriteLog("yhcall失败:[" + Astr_jyh + "|" + Astr_jylsh + "]");
            //    Service_NCSB.WriteLog(Encoding.Default.GetString(Astr_clxx));
            //    strRecvXml = "";
            //    retcode = "0998";
            //    return strRecvXml;                
            //}
                   
            //strRecvXml = Encoding.Default.GetString(Astr_jyjg);
            retcode = "0000";

            return strRecvXml;

        }

        /// <summary>
        /// 取交易流水号
        /// </summary>
        /// <param name="Astr_lydz">目的地址,为社保局的编码</param>
        /// <param name="Astr_ydz">源地址,为银行的编码</param>
        /// <param name="jylsh"></param>
        /// <returns></returns>
        private static string getjylsh(string Astr_lydz, string Astr_ydz)
        {
            byte[] Astr_jyjg = new byte[256], Astr_clxx = new byte[256];
            int Aint_clbz = 0;

            webService.yhcall("002", "", Encoding.Default.GetBytes(""), "", "", ref Astr_jyjg, ref Aint_clbz, ref Astr_clxx);
            if (Aint_clbz <= 0)
            {
                Service_NCSB.WriteLog("申请交易流水号失败:[" + Astr_lydz + "|" + Astr_ydz + "]");
                jylsh = "";
                return jylsh;
            }

            XmlDocument doc= new XmlDocument();
            try
            {
                doc.InnerXml = Encoding.Default.GetString(Astr_jyjg);
                jylsh = doc.SelectSingleNode("jyjg/jylsh").InnerText;
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("Exception:[" + err.Message + "]");
                jylsh = "";                
            }

            return jylsh;

        }

        /// <summary>
        /// 确认流程
        /// </summary>
        /// <param name="XmlPara01"></param>
        /// <param name="retcode"></param>
        /// <returns></returns>
        public static string yhconfirm(string XmlPara01, ref string retcode)
        {
            string Astr_jylsh, Astr_lydz, Astr_ydz, strRecvXml = "";
            byte[] Astr_jyjg = new byte[128], Astr_clxx = new byte[128], Astr_jysj = new byte[128];
            int Aint_clbz = 0;

            if (!init())
            {
                Service_NCSB.WriteLog("init 错误");
                strRecvXml = "";
                retcode = "0998";
                return strRecvXml;
            }

            XmlPara01 = "<Para01>" + XmlPara01 + "</Para01>";
            Service_NCSB.WriteLog("XmlPara01[" + XmlPara01 + "]");
            XmlDocument xReqPara = new XmlDocument();
            try
            {
                xReqPara.InnerXml = XmlPara01;
                Astr_jylsh = xReqPara.SelectSingleNode("Para01/Astr_jylsh").InnerText;
                Astr_lydz = xReqPara.SelectSingleNode("Para01/Astr_lydz").InnerText;
                Astr_ydz = xReqPara.SelectSingleNode("Para01/Astr_ydz").InnerText;
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("Exception:[" + err.Message + "]");
                strRecvXml = "";
                retcode = "0998";
                return strRecvXml;
            }

            Service_NCSB.WriteLog("Astr_jylsh[" + Astr_jylsh + "]");
            Service_NCSB.WriteLog("Astr_lydz[" + Astr_lydz + "]");
            Service_NCSB.WriteLog("Astr_ydz[" + Astr_ydz + "]");

            webService.yhconfirm(Astr_jylsh,Astr_lydz, Astr_ydz,ref Aint_clbz, ref Astr_clxx);

            strRecvXml += "<Aint_clbz>" + Aint_clbz + "</Aint_clbz>\r\n";
            strRecvXml += "<Astr_clxx>" + Encoding.Default.GetString(Astr_clxx) + "</Astr_clxx>\r\n";
           
            retcode = "0000";

            return strRecvXml;

        }

        public static string yhcancel(string XmlPara01, ref string retcode)
        {
            string Astr_jylsh, Astr_lydz, Astr_ydz, strRecvXml = "";
            byte[] Astr_jyjg = new byte[128], Astr_clxx = new byte[128], Astr_jysj = new byte[128];
            int Aint_clbz = 0;

            if (!init())
            {
                Service_NCSB.WriteLog("init 错误");
                strRecvXml = "";
                retcode = "0998";
                return strRecvXml;
            }

            XmlPara01 = "<Para01>" + XmlPara01 + "</Para01>";
            Service_NCSB.WriteLog("XmlPara01[" + XmlPara01 + "]");
            XmlDocument xReqPara = new XmlDocument();
            try
            {
                xReqPara.InnerXml = XmlPara01;
                Astr_jylsh = xReqPara.SelectSingleNode("Para01/Astr_jylsh").InnerText;
                Astr_lydz = xReqPara.SelectSingleNode("Para01/Astr_lydz").InnerText;
                Astr_ydz = xReqPara.SelectSingleNode("Para01/Astr_ydz").InnerText;
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("Exception:[" + err.Message + "]");
                strRecvXml = "";
                retcode = "0998";
                return strRecvXml;
            }

            Service_NCSB.WriteLog("Astr_jylsh[" + Astr_jylsh + "]");
            Service_NCSB.WriteLog("Astr_lydz[" + Astr_lydz + "]");
            Service_NCSB.WriteLog("Astr_ydz[" + Astr_ydz + "]");

            webService.yhcancel(Astr_jylsh, Astr_lydz, Astr_ydz, ref Aint_clbz, ref Astr_clxx);

            strRecvXml += "<Aint_clbz>" + Aint_clbz + "</Aint_clbz>\r\n";
            strRecvXml += "<Astr_clxx>" + Encoding.Default.GetString(Astr_clxx) + "</Astr_clxx>\r\n";

            retcode = "0000";

            return strRecvXml;

        }


    }
}
