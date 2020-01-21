using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Collections;

namespace LinkWS.AllWebServiceFunc
{
    /*�ϳ��籣���ն���_��ũ��*/
    class NCDC
    {
        /*TODO:�����籣WEBSERVICE����*/
        //static LinkWS.WebR_NCSB.yhjypt webService;
        static LinkWS.javaEncrypt.serviceEncryptService javaEncrypt;
        static LinkWS.WebR_NCDC.SYConnWebService webNCDC;
        static private Cfg cfg_NCDC;
        static public bool getConfigInfo()
        {
            try
            {
                INIClass inicls = new INIClass(Service_NCSB.MyConfigFile);
                if (!inicls.ExistINIFile())
                {
                    Service_NCSB.WriteLog("inicls����");
                    return false;
                }
                cfg_NCDC = new Cfg();
                cfg_NCDC.WSip = inicls.IniReadValue("NCSB", "WS_IP");
                cfg_NCDC.WSport = inicls.IniReadValue("NCSB", "WS_PORT");
                cfg_NCDC.WStimeout = inicls.IniReadValue("NCSB", "WS_TIMEOUT");
                cfg_NCDC.ftp_ip = inicls.IniReadValue("NCSB", "FTP_IP");
                cfg_NCDC.ftp_user = inicls.IniReadValue("NCSB", "FTP_USER");
                cfg_NCDC.ftp_passwd = inicls.IniReadValue("NCSB", "FTP_PASSWD");
                cfg_NCDC.ftp_localpath = inicls.IniReadValue("NCSB", "FTP_LOCALPATH");
                if (cfg_NCDC.ftp_ip == "" || cfg_NCDC.ftp_user == "" || cfg_NCDC.ftp_passwd == "")
                {
                    Service_NCSB.WriteLog("ftp���ô���");
                    return false;
                }
                if (cfg_NCDC.WSip == "" || cfg_NCDC.WSport == "")
                {
                    Service_NCSB.WriteLog("WS���ô���");
                    return false;
                }
                if (cfg_NCDC.WStimeout == "")
                    cfg_NCDC.WStimeout = "200000";
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("getConfigInfo����" + err.Message);
                return false;
            }
            return true;
        }
        static private bool init()
        {
            /*����webService����*/
            javaEncrypt = new LinkWS.javaEncrypt.serviceEncryptService();
            webNCDC = new LinkWS.WebR_NCDC.SYConnWebService();
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
            webNCDC.Timeout = int.Parse(WStimeout);

            webNCDC.Url = "http://" + WSip + ":" + WSport + "/ysptsz/services/szWebService?wsdl";
            return true;
        }
        /*ǩ������*/
        static private bool sign()
        {

            return true;
        }
        public static string szCall(string XmlPara01, string XmlPkg,ref string retcode)
        {
            /*aaz400:�籣��������511300 tradeId:���׺� aae008:���б��(���Զ�Ϊ103) inputXml:���뱨��*/
            string aaz400="511300", tradeId, aae008="103", reqseq, reqdate, inputXml, errinfo="", strRecvXml = "";
            //byte[]inputXml=new byte[128];
            /*���ܷ�ʽ*/
            int encryptKey = 2 | 4;
            INIClass iniKey = new INIClass(Service_NCSB.MyConfigFile);
            if (!iniKey.ExistINIFile())
            {
                Service_NCSB.WriteLog("init ����");
                strRecvXml = "";
                retcode = "0998";
                return strRecvXml;

            }
            /*������Կ*/
            string originalKey = iniKey.IniReadValue("KEY", "INIT_KEY");
            string key = iniKey.IniReadValue("KEY", "NEW_KEY");
            Service_NCSB.WriteLog("original key:["+originalKey+"]");
            Service_NCSB.WriteLog("new key:[" + key + "]");

            if (key == "")
            {
                strRecvXml = "";
                retcode = "9978";
                return strRecvXml;
            }
            if (!init())
            {
                Service_NCSB.WriteLog("init ����");
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
                //aaz400 = xReqPara.SelectSingleNode("Para01/aaz400").InnerText;                
                tradeId = xReqPara.SelectSingleNode("Para01/tradeId").InnerText;             
                reqseq = xReqPara.SelectSingleNode("Para01/reqseq").InnerText;
                reqdate = xReqPara.SelectSingleNode("Para01/reqdate").InnerText;

                /*����inputxml����*/
                XmlNode inputXmlNode = xReqPara.SelectSingleNode("Para01/inputxml");
                XmlElement inputXmlElem = (XmlElement)inputXmlNode;
                inputXmlElem.SetAttribute("reqseq",reqseq);
                inputXmlElem.SetAttribute("reqdate", reqdate);
                inputXml = xReqPara.SelectSingleNode("Para01/inputxml").OuterXml;

                //Service_NCSB.WriteLog("aaz400[" + aaz400 + "]");
                Service_NCSB.WriteLog("tradeId[" + tradeId + "]");
                Service_NCSB.WriteLog("inputXml[" + inputXml + "]");
            }
            catch(Exception err)
            {
                /*��׽xml�쳣*/
                Service_NCSB.WriteLog("Exception:[" + err.Message + "]");
                strRecvXml = "";
                retcode = "9996";
                return strRecvXml;
            }
            /*TODO:���ʴ���*/
            if(tradeId=="NC39006")
            {
                XmlDocument PkgXml = new XmlDocument();
                PkgXml.InnerXml = XmlPkg;
                XmlNodeList fcNode = PkgXml.SelectNodes("ap/fc");
                int fcLengh = fcNode.Count;

                XmlNode inputXmlNode = xReqPara.SelectSingleNode("Para01/inputxml");
                //XmlElement inputXmlElem = (XmlElement)inputXmlNode;

                XmlElement rows = xReqPara.CreateElement("rows");//����rows
                inputXmlNode.AppendChild(rows);

                for (int i = 0; i < fcLengh; i++)
                {
                    XmlElement row = xReqPara.CreateElement("row");
                    string tmpfcXml = fcNode.Item(i).InnerXml;
                    row.InnerXml = tmpfcXml;
                    rows.AppendChild(row);
                }
                inputXml = xReqPara.SelectSingleNode("Para01/inputxml").OuterXml;
                Service_NCSB.WriteLog("inputXml[" + inputXml + "]");
                //xReqPara.AppendChild(fcNode);

                //Service_NCSB.WriteLog("fc[")
            }
            /*ǩ��*/
            if (tradeId == "T0102")
            {

                /*����Կ*/
                string newKey = "";
                /*�籣����xml*/
                string recvxmlDe = "";
                
                try
                {
                    /*���ܷ�ʽ*/
                    //int encryptKey = 2 | 4;
                    /*����xml*/
                    string encryptXmlKey = javaEncrypt.encrypt(inputXml, originalKey, encryptKey);
                    /*����*/
                    string recvxmlEn = webNCDC.szCall(aaz400, tradeId, encryptXmlKey, encryptKey.ToString(), aae008);
                    Service_NCSB.WriteLog("ǩ�����ؼ���[" + recvxmlEn + "]");
                    /*����*/
                    recvxmlDe = javaEncrypt.decrypt(recvxmlEn, originalKey, encryptKey);
                    Service_NCSB.WriteLog("ǩ�����ؽ���[" + recvxmlDe + "]");

                    //recvxmlDe = "<result><code>1</code><message>�ɹ�</message><newkey>3333333311111111</newkey><sysdt>20161206161433</sysdt></result>";
                    
                }
                catch (Exception err)
                {
                    /*��׽ǩ�����ü��ӽ����쳣*/
                    Service_NCSB.WriteLog("err=[" + err.Message + "]");
                    retcode = "0998";
                    strRecvXml = "";
                    return strRecvXml;
                }
                xReqPara.LoadXml(recvxmlDe);
                /*�ж��籣���ؽ��*/
                string returnCode = xReqPara.SelectSingleNode("result/code").InnerText;
                if (returnCode == "1")
                {
                    newKey = xReqPara.SelectSingleNode("result/newkey").InnerText;
                    Service_NCSB.WriteLog("newKey[" + newKey + "]");
                    iniKey.IniWriteValue("KEY", "NEW_KEY", newKey);
                }
                retcode = "0000";
                recvxmlDe = xReqPara.SelectSingleNode("result").OuterXml;
                Service_NCSB.WriteLog("�����籣���ر���[" + recvxmlDe + "]");
                return recvxmlDe;

            }


            try
            {
                /*TODO:�����籣�ӿ�*/
                //strRecvXml="<result><code>1</code><message></message><output><rows><row><aae001>2016</aae001><aae041>201601</aae041><aae042>201612</aae042></row></rows></output></result>";
                //string keyEncrypt="1111111133333333";
                //int entype = 2 | 4;//���ܷ�ʽ

                //���Ե���ƾ֤
                //strRecvXml = "<result><message></message><code>1</code><output><aae009_sb/><aae013/><aac003>����</aac003><aae072>6466463</aae072><aaa121>01</aaa121><aae010_sb>1234567890000000001</aae010_sb><aaa027>511302</aaa027><aae008_sb>103</aae008_sb><aae019>5263.2</aae019><aac001>0003994991</aac001></output></result>";
                //Service_NCSB.WriteLog("���Ա���:" + strRecvXml);
                
                string inputXmlEncrypt = javaEncrypt.encrypt(inputXml, key, encryptKey);//��������
                Service_NCSB.WriteLog("����inputXml["+inputXmlEncrypt+"]");
                string strTmpRecv = webNCDC.szCall(aaz400, tradeId, inputXmlEncrypt, encryptKey.ToString(), aae008);//�õ��籣ԭʼ����
                Service_NCSB.WriteLog("�籣���ر��ļ���recvXml[" + strTmpRecv + "]");
                strRecvXml = javaEncrypt.decrypt(strTmpRecv, key, encryptKey);
                Service_NCSB.WriteLog("�籣���ر��Ľ���[" + strRecvXml + "]");
                
            }
            catch(Exception err)
            {
                /*��׽webservice���ü��ӽ����쳣*/
                Service_NCSB.WriteLog("err=[" + err.Message + "]");
                retcode = "0998";
                strRecvXml = "";
                return strRecvXml;
            }
            retcode = "0000";
            xReqPara.LoadXml(strRecvXml);
            strRecvXml = xReqPara.SelectSingleNode("result").OuterXml;
            return strRecvXml;
        }

        
    }
}
