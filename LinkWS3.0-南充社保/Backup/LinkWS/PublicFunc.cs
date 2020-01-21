using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using System.IO.Compression;


namespace LinkWS
{
    class Func
    {
        public Func()
        {
        }

        public static string Encrypt3DES(string a_strString, string a_strKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(a_strKey);
            DES.Mode = CipherMode.ECB;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(a_strString);
            return BitConverter.ToString(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length)).Replace("-","");
            //return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
        public static string Decrypt3DES(string a_strString, string a_strKey)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(a_strKey);
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            //DES.Padding = System.Security.Cryptography.PaddingMode.Zeros;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            string result = "";
            try
            {
                byte[] Buffer = GetBytes(a_strString);
                result = ASCIIEncoding.ASCII.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception )
            {
                result = "";
            }
            return result;
        }

        public static byte[] GetBytes(string str)
        {
            byte[] tempBytes = new byte[] { };

            //string tempStr = str.Trim();

            //两个一组
            if (str.Length % 2 != 0)
            {
                // //throw new Exception(string.Format("字符串长度错误"));
                return null;
            }

            int index = 0;
            while (index < str.Length)
            {
                string subStr = str.Substring(index, 2);
                Append(ref tempBytes, Convert.ToByte(subStr, 16));
                index += 2;
            }
            return tempBytes;

        }

        /// <summary>
        /// 往byte数组中添加一个byte元素
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <param name="bvalue"></param>
        /// <returns></returns>
        public static byte[] Append(ref byte[] srcBytes, byte bvalue)
        {
            Array.Resize<byte>(ref srcBytes, srcBytes.Length + 1);

            srcBytes[srcBytes.Length - 1] = bvalue;

            return srcBytes;
        }

        /// <summary>
        /// 取字符串长度(1个中文字符长度为2)
        /// </summary>
        /// <param name="strinput"></param>
        /// <returns></returns>    
        public static int GetLength_Chinese(string strinput)
        {
            char[] char_input = strinput.ToCharArray();
            int ilength = strinput.Length;
            for (int i = 0; i < strinput.Length; i++)
            {
                if (char_input[i] > 255)
                {
                    ilength++;
                }
            }
            return ilength;
        }

        /// <summary>
        /// 根据返回码取返回信息
        /// </summary>
        /// <param name="retcode">返回码</param>
        /// <returns>返回信息</returns>
        public static string GetRetMsg(string retcode)
        {
            string strTmp = "";
            INIClass inicls = new INIClass(Service_NCSB.MyConfigFile);
            if (!inicls.ExistINIFile())
            {
                return "未知错误";
            }
            strTmp = inicls.IniReadValue("RETCODE", retcode);
            if (strTmp=="")
                strTmp = "未知错误码";
            return strTmp;
        }

        /// <summary>
        /// 插入或修改xml节点值
        /// </summary>
        /// <param name="xmlDoc">xml文档</param>
        /// <param name="XmlNode">新增或修改的xmlnode</param>
        /// <param name="XmlValue">新增或修改的xmlvalue</param>
        /// <param name="iFlag">增改标志 0:修改</param>
        /// <returns></returns>
        public static bool InsertXmlNode(ref  XmlDocument xmlDoc,string XmlNode,string XmlValue,int iFlag)
        {
            //xitem = nRoot.OwnerDocument.CreateElement("RetInfo1");
            //xitem.InnerXml = arrRetInfos[0].ToString();                
            //nRoot.AppendChild(xitem);
            
            string[] AllXmlNodes = XmlNode.Split('/');
            XmlNode xmlCurrNode,XmlNodeTmp;
            XmlNodeTmp = xmlDoc.CreateNode(XmlNodeType.Element, AllXmlNodes[0], null);
            try
            {
                xmlDoc.AppendChild(XmlNodeTmp);
            }
            catch (Exception err)
            {
                //Service_NCSB.WriteLog("err=[" + err.Message + "]");
                //return false;
            }
            
            for (int i = 1; i < AllXmlNodes.Length-1; i++)
            {                
                xmlCurrNode = xmlDoc.SelectSingleNode(AllXmlNodes[i-1]);
                XmlNodeTmp = xmlDoc.CreateNode(XmlNodeType.Element, AllXmlNodes[i], null);
                try
                {
                    xmlCurrNode.AppendChild(XmlNodeTmp);
                }
                catch (Exception )
                {
                    //return false;
                }
                
            }
            xmlCurrNode = xmlDoc.SelectSingleNode(AllXmlNodes[AllXmlNodes.Length-2]);
            XmlNodeTmp = xmlDoc.CreateNode(XmlNodeType.Element, AllXmlNodes[AllXmlNodes.Length-1], null);
            try
            {
                if (XmlValue.IndexOf('/') != -1)
                    XmlNodeTmp.InnerXml = XmlValue;
                else
                    XmlNodeTmp.InnerText = XmlValue;
            }
            catch (Exception)
            {

                return false;
            }
            
            if (iFlag == 0)
            {
                try
                {
                    XmlNode xmlNode=xmlDoc.SelectSingleNode(XmlNode);
                    xmlNode.ParentNode.RemoveChild(xmlNode);                    
                }
                catch (Exception )
                {
                    //return false;
                }
            }    
            xmlCurrNode.AppendChild(XmlNodeTmp);
            
            return true;
        }

        /// <summary>
        /// 获取xml指点节点的值
        /// </summary>
        /// <param name="XmlPkg">xml字符串</param>
        /// <param name="XmlNodeName">xml节点名称</param>
        /// <returns>xml节点值</returns>
        public static bool GetXmlNodeValue(string XmlPkg, string XmlNodeName, out string XmlNodeValue)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(XmlPkg);

                XmlNode root = xmlDoc.SelectSingleNode(XmlNodeName);
                if (root == null)
                {
                    Console.Write("xml节点未找到[{0}][{1}]......\n", XmlPkg, XmlNodeName);
                    XmlNodeValue = "";
                    return false;
                }
                if (root.FirstChild==null)
                {
                    XmlNodeValue = "";
                    return false;
                }
                
                XmlNodeValue = root.FirstChild.Value;
                return true;
            }
            catch (Exception)
            {
                Console.Write("xml报文异常[{0}]......\n", XmlPkg);
                XmlNodeValue = "";
                return false;
            }
        }

        /// <summary>
        /// 根据返回码组返回XML数据包
        /// </summary>
        /// <param name="retcode">返回码</param>
        /// <returns>XML数据包</returns>
        public static string GetXmlPkgFromCode(string retcode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            InsertXmlNode(ref xmlDoc, "ap/RetCode", retcode, 0);
            InsertXmlNode(ref xmlDoc, "ap/RetMsg", GetRetMsg(retcode), 0);
            return xmlDoc.InnerXml;

        }


        public static string CreateControlXml(string brno)
        {
            XmlElement xmlelem;
            XmlDocument xmlDoc = new XmlDocument();

            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmlDoc.AppendChild(xmldecl);

            //加入一个根元素
            xmlelem = xmlDoc.CreateElement("", "control", "");
            xmlDoc.AppendChild(xmlelem);

            //加入另外一个元素
            for (int i = 1; i < 2; i++)
            {
                XmlNode root = xmlDoc.SelectSingleNode("control");//查找<Employees>                 
                XmlElement xesub1 = xmlDoc.CreateElement("yab003");
                xesub1.InnerText = "01";//设置文本节点 
                root.AppendChild(xesub1);//添加到<Node>节点中 
                XmlElement xesub2 = xmlDoc.CreateElement("yke189");
                xesub2.InnerText = "";
                root.AppendChild(xesub2);
                XmlElement xesub3 = xmlDoc.CreateElement("akb020");
                xesub3.InnerText = brno;
                root.AppendChild(xesub3); 
            }
            return xmlDoc.InnerXml;

        }

        public static string CreateDataXml(string brno)
        {
            XmlElement xmlelem;
            XmlDocument xmlDoc = new XmlDocument();

            //加入XML的声明段落,<?xml version="1.0" encoding="gb2312"?>
            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "gb2312", null);
            xmlDoc.AppendChild(xmldecl);

            //加入一个根元素
            xmlelem = xmlDoc.CreateElement("", "data", "");
            xmlDoc.AppendChild(xmlelem);

            //加入另外一个元素
            //for (int i = 1; i < 2; i++)
            //{
            //    XmlNode root = xmlDoc.SelectSingleNode("control");//查找<Employees>                 
            //    XmlElement xesub1 = xmlDoc.CreateElement("yab003");
            //    xesub1.InnerText = "01";//设置文本节点 
            //    root.AppendChild(xesub1);//添加到<Node>节点中 
            //    XmlElement xesub2 = xmlDoc.CreateElement("yke189");
            //    xesub2.InnerText = "";
            //    root.AppendChild(xesub2);
            //    XmlElement xesub3 = xmlDoc.CreateElement("akb020");
            //    xesub3.InnerText = brno;
            //    root.AppendChild(xesub3);
            //}
            return xmlDoc.InnerXml;

        }

        public static int GetRetInfoFromWB(string XmlPkg, ref string retcode,ref string retmsg)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(XmlPkg);

                XmlNode root = xmlDoc.SelectSingleNode("result/code");
                if (root == null)
                {
                    retcode = "";
                    retmsg = "";
                    return 1;
                }
                if (root.FirstChild == null)
                {
                    retcode = "";
                    retmsg = "";
                    return 3;
                }

                retcode = root.FirstChild.Value;
                root = xmlDoc.SelectSingleNode("result/message");
                retmsg = (root.FirstChild == null)?"":root.FirstChild.Value;
                return 0;
            }
            catch (Exception)
            {
                retcode = "";
                retmsg = "";
                return 2;
            }
        }


        public static void InsertXmlNode(ref XmlDocument xmldoc,string rootnode,string xmlname,string xmlvalue)
        {
            XmlElement xeAdd;
            XmlElement nRoot = xmldoc.CreateElement(rootnode);
            xeAdd = xmldoc.CreateElement(xmlname);
            xeAdd.InnerText = xmlvalue;
            nRoot.AppendChild(xeAdd);
        }

        /// <summary>
        /// 通过WebService下载文件
        /// </summary>
        /// <param name="ServiceFileName">服务器文件名称</param>
        /// <param name="DownloadFolderPath">当地保存文件路径</param>
        /// <param name="LocalFileName">当地保存文件名称</param>
        /// <returns>成功标志</returns>
        public static bool DownloadWSF(byte[] bWebServiceFile, string sLocalFolderPath, string sLocalFileName, out string sRetMsg)
        {
            if (bWebServiceFile.Length==0)
            {
                sRetMsg = "文件为空";
                return false;
            }
            sRetMsg = "执行成功";
            if (sLocalFolderPath == "")
                sLocalFolderPath = @"c:\WSdownload";
            if (sLocalFileName == "")
                sLocalFileName = "WSF.default";
            try
            {
                string sLocalFullFileName = sLocalFolderPath + "\\" + sLocalFileName;

                ////////////// 调用webservice的下载文件函数 ////////////////
                /// bWebServiceFile = web.DownloadFile(sServiceFileName);
                ///////////////////////////////////////////////////////////

                if (bWebServiceFile != null)
                {
                    if (!Directory.Exists(sLocalFolderPath))
                    {
                        Directory.CreateDirectory(sLocalFolderPath);
                    }
                    if (!File.Exists(sLocalFullFileName))
                    {
                        File.Create(sLocalFullFileName).Dispose();
                    }
                    //如果不存在完整的上传路径就创建
                    FileInfo downloadInfo = new FileInfo(sLocalFullFileName);
                    if (downloadInfo.IsReadOnly) { downloadInfo.IsReadOnly = false; }
                    //定义并实例化一个内存流，以存放提交上来的字节数组。
                    MemoryStream ms = new MemoryStream(bWebServiceFile);
                    //定义实际文件对象，保存上载的文件。
                    FileStream fs = new FileStream(sLocalFullFileName, FileMode.Create);
                    ///把内内存里的数据写入物理文件
                    ms.WriteTo(fs);
                    fs.Flush();
                    ms.Flush();
                    ms.Close();
                    fs.Close();
                    fs = null;
                    ms = null;
                }

            }
            catch (Exception err)
            {
                sRetMsg = err.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 给任意长度字符串加密(AES)
        /// </summary>
        /// <param name="toDecrypt">待加密数据</param>
        /// <param name="key">字符串形式的密钥</param>
        /// <param name="strOut">大写的16进制字符串</param>
        /// <returns>成功与否</returns>
        public static bool Encrypt_AES_H(string toEncrypt, string key, out string strOut)
        {
            strOut = "";
            if (key=="")
            {
                strOut = toEncrypt;
                return true;
            }
            try
            {
                byte[] keyArray = new byte[16];
                byte[] bkey = UTF8Encoding.UTF8.GetBytes(key);

                for (int i = 0; i < 16; i++)
                {
                    if (i < bkey.Length)
                    {
                        keyArray[i] = bkey[i];
                    }
                }

                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                RijndaelManaged rDel = new RijndaelManaged();

                rDel.Key = keyArray;

                rDel.Mode = CipherMode.ECB;

                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                for (int i = 0; i < resultArray.Length; i++)
                {
                    strOut += String.Format("{0:X2}", resultArray[i]);
                }
                
                //return UTF8Encoding.UTF8.GetString(resultArray);

                //return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 给任意长度字符串解密(AES)
        /// </summary>
        /// <param name="toDecrypt">待解密数据</param>
        /// <param name="key">密钥</param>
        /// <param name="strOut">解密后数据</param>
        /// <returns>成功与否</returns>
        public static bool Decrypt_AES_H(string toDecrypt, string key,out string strOut)
        {
            strOut = "";
            if (key == "")
            {
                strOut = toDecrypt;
                return true;
            }
            try
            {
                byte[] keyArray = new byte[16];
                byte[] bkey = UTF8Encoding.UTF8.GetBytes(key);

                for (int i = 0; i < 16; i++)
                {
                    if (i < bkey.Length)
                    {
                        keyArray[i] = bkey[i];
                    }
                }

                if (toDecrypt.Length % 2 != 0) return false;

                string strTmp = "";
                byte bTmp = new byte();
                int iTotal = toDecrypt.Length / 2;

                byte[] toDecryptArray = new byte[toDecrypt.Length / 2];
                for (int i = 0; i < toDecrypt.Length; i = i + 2)
                {
                    strTmp = toDecrypt[i].ToString() + toDecrypt[i + 1].ToString();
                    bTmp = Convert.ToByte(strTmp, 16);
                    toDecryptArray[i / 2] = bTmp;
                }

                RijndaelManaged rDel = new RijndaelManaged();

                rDel.Key = keyArray;

                rDel.Mode = CipherMode.ECB;

                rDel.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                                
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

                strOut = UTF8Encoding.UTF8.GetString(resultArray);
                //strOut = Encoding.Default.GetString(resultArray);
                
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// AES解密文件
        /// </summary>
        /// <param name="strInFileName">待解密文件名</param>
        /// <param name="strKey">解密密钥</param>
        /// <param name="strOutFileName">解密后文件名</param>
        /// <returns></returns>
        public static bool Decrypt_AES_File(string strInFileName, string strKey, string strOutFileName)
        {
            Service_NCSB.WriteLog("strInFileName=[" + strInFileName + "]");
            Service_NCSB.WriteLog("strKey=[" + strKey + "]");
            Service_NCSB.WriteLog("strOutFileName=[" + strOutFileName + "]");
            if (strInFileName == strOutFileName)
            {//待解密文件名与解密后文件名不能重复
                return false;
            }
            try
            {
                StreamReader sr = File.OpenText(Service_NCSB.MyPath + strInFileName);
                File.Delete(Service_NCSB.MyPath + strOutFileName);

                StreamWriter sw = new StreamWriter(Service_NCSB.MyPath + strOutFileName,false,Encoding.GetEncoding("GB2312"));
                string strLine;
                strLine = sr.ReadLine();
                //strLine = "120140000001|银海住房公积金核心系统|20140101|2000.00|文件内容加解密测试|萧刘|yinhai";
                string strOutLine = "";
                while (strLine != null)
                {
                    Service_NCSB.WriteLog("strLine=[" + strLine + "]");
                    if (!Decrypt_AES_H(strLine, strKey, out strOutLine))
                    {
                        sr.Close();
                        sw.Close();
                        return false;
                    }
                    //if (!U2G(strTmp, out strOutLine))
                    //{
                    //    Service_PZHSB.WriteLog("U2G 错误");
                    //    sr.Close();
                    //    sw.Close();
                    //    return false;
                    //}
                    Service_NCSB.WriteLog("strOutLine=[" + strOutLine + "]");
                    sw.WriteLine(strOutLine);
                    strLine = sr.ReadLine();
                }

                sr.Close();
                sw.Close();
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("err=[" + err.Message + "]");
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// AES解密文件 省社保养老金代发专用
        /// </summary>
        /// <param name="strInFileName">待解密文件名</param>
        /// <param name="strUserPassword">用户密码</param>
        /// <param name="strBankCode">银行编号</param>
        /// <param name="strOutFileName">解密后文件名</param>
        /// <returns></returns>
        public static bool DecFile_SCSBYLJ(string strInFileName, string strUserPassword, string strBankCode, string strOutFileName)
        {
            Service_NCSB.WriteLog("strInFileName=[" + strInFileName + "]");
            Service_NCSB.WriteLog("strUserPassword=[" + strUserPassword + "]");
            Service_NCSB.WriteLog("strBankCode=[" + strBankCode + "]");
            Service_NCSB.WriteLog("strOutFileName=[" + strOutFileName + "]");
            string strYHPass = "yinhai_2014";
            string strKey ;     //解密密钥
            string strPassword; //密码字符串
            strPassword = strUserPassword + strYHPass + strBankCode;
            Service_NCSB.WriteLog("strPassword=[" + strPassword + "]");
            if (strInFileName == strOutFileName)
            {//待解密文件名与解密后文件名不能重复
                return false;
            }
            try
            {
                //StreamReader sr = File.OpenText(Service_PZHSB.MyPath + strInFileName);
                StreamReader sr = File.OpenText("C:\\ftp\\" + strInFileName);
                File.Delete(Service_NCSB.MyPath + strOutFileName);

                StreamWriter sw = new StreamWriter("C:\\ftp\\" + strOutFileName, false, Encoding.GetEncoding("GB2312"));
                string strLine;
                strLine = sr.ReadLine();
                //strLine = "120140000001|银海住房公积金核心系统|20140101|2000.00|文件内容加解密测试|萧刘|yinhai";
                string strOutLine = "";
                int iCount = 0;
                ArrayList arrLine=new ArrayList();
                MD5 MyMD5 = new MD5CryptoServiceProvider();
                while (strLine != null)
                {                    
                    Service_NCSB.WriteLog("strLine=[" + strLine + "]");
                    string strMD5 = strPassword + iCount.ToString();
                    //Service_PZHSB.WriteLog("MD5=[" + strMD5 + "]");                    
                    string strMD5Out =BitConverter.ToString(( MyMD5.ComputeHash(Encoding.Default.GetBytes(strMD5)))).Replace("-","");                    
                    //Service_PZHSB.WriteLog("strMD5Out=[" + strMD5Out + "]");                                  
                    strKey = strMD5Out.Substring(8,16);
                    strKey = strKey.ToLower();
                    //Service_PZHSB.WriteLog("strKey=[" + strKey + "]");                    
                    if (!Decrypt_AES_B64(strLine, strKey, out strOutLine))
                    {
                        Service_NCSB.WriteLog("Decrypt_AES_B64错误[" + iCount.ToString() + "]");
                        sr.Close();
                        sw.Close();
                        return false;
                    }
                    //if (!U2G(strTmp, out strOutLine))
                    //{
                    //    Service_PZHSB.WriteLog("U2G 错误");
                    //    sr.Close();
                    //    sw.Close();
                    //    return false;
                    //}
                    
                    Service_NCSB.WriteLog("strOutLine=[" + strOutLine + "]");
                    arrLine.Add(strOutLine);
                    //sw.WriteLine(strOutLine);
                    strLine = sr.ReadLine();
                    iCount++;
                }
                //数据校验
                for (int i = 0; i < arrLine.Count; i++)
                {
                    string strHash = ((string)arrLine[i]).Substring(0, 32);
                    string strTmp = i.ToString() + ((string)arrLine[i]).Substring(32) + arrLine.Count.ToString();
                    string strMD5 = BitConverter.ToString((MyMD5.ComputeHash(Encoding.Default.GetBytes(strTmp)))).Replace("-", "");
                    Service_NCSB.WriteLog("strHash=[" + strHash + "]");
                    Service_NCSB.WriteLog("strTmp=[" + strTmp + "]");
                    Service_NCSB.WriteLog("strMD5=[" + strMD5 + "]");
                    if (strHash != strMD5.ToLower())
                    {
                        Service_NCSB.WriteLog("数据校验失败,行数[" + i.ToString() + "]");
                        //return false;
                    }
                    sw.WriteLine(((string)arrLine[i]).Substring(32));
                }

                sr.Close();
                sw.Close();
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("err=[" + err.Message + "]");
                return false;
            }

            return true;
        }


        /// <summary>
        /// AES解密Base64文件
        /// </summary>
        /// <param name="strInFileName">待解密文件名</param>
        /// <param name="strKey">解密密钥</param>
        /// <param name="strOutFileName">解密后文件名</param>
        /// <returns></returns>
        public static bool Decrypt_AES_File(string strInFileName, string strKey, string strOutFileName,string strFileType)
        {
            Service_NCSB.WriteLog("strInFileName=[" + strInFileName + "]");
            Service_NCSB.WriteLog("strKey=[" + strKey + "]");
            Service_NCSB.WriteLog("strOutFileName=[" + strOutFileName + "]");
            Service_NCSB.WriteLog("strFileType=[" + strFileType + "]");
            if (strInFileName == strOutFileName)
            {//待解密文件名与解密后文件名不能重复
                return false;
            }
            try
            {
                StreamReader sr = File.OpenText(Service_NCSB.MyPath + strInFileName);
                File.Delete(Service_NCSB.MyPath + strOutFileName);

                StreamWriter sw = new StreamWriter(Service_NCSB.MyPath + strOutFileName, false, Encoding.GetEncoding("GB2312"));
                string strLine;
                strLine = sr.ReadLine();
                //strLine = "120140000001|银海住房公积金核心系统|20140101|2000.00|文件内容加解密测试|萧刘|yinhai";
                string strOutLine = "";
                while (strLine != null)
                {
                    Service_NCSB.WriteLog("strLine=[" + strLine + "]");                    
                    switch (strFileType)
                    {
                        case "B64"://Base64文件
                            if (!Decrypt_AES_B64(strLine, strKey, out strOutLine))
                            {
                                sr.Close();
                                sw.Close();
                                return false;
                            }
                            break;
                        case "H"://16进制文件
                            if (!Decrypt_AES_H(strLine, strKey, out strOutLine))
                            {
                                sr.Close();
                                sw.Close();
                                return false;
                            }
                            break;
                        default:
                            Service_NCSB.WriteLog("FileType错误！[" + strFileType + "]");
                            return false;                            
                    }                 
                    Service_NCSB.WriteLog("strOutLine=[" + strOutLine + "]");
                    sw.WriteLine(strOutLine);
                    strLine = sr.ReadLine();
                }

                sr.Close();
                sw.Close();
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("err=[" + err.Message + "]");
                return false;
            }

            return true;
        }

        /// <summary>
        /// AES加密文件
        /// </summary>
        /// <param name="strInFileName">待加密文件名</param>
        /// <param name="strKey">加密密钥</param>
        /// <param name="strOutFileName">加密后文件名</param>
        /// <returns></returns>
        public static bool Encrypt_AES_File(string strInFileName, string strKey, string strOutFileName)
        {
            //Service_PZHSB.WriteLog("strInFileName=[" + strInFileName + "]");
            //Service_PZHSB.WriteLog("strKey=[" + strKey + "]");
            //Service_PZHSB.WriteLog("strOutFileName=[" + strOutFileName + "]");
            if (strInFileName==strOutFileName)
            {//待加密文件名与加密后文件名不能重复
                return false;
            }
            try
            {                
                StreamReader sr = new StreamReader(Service_NCSB.MyPath + strInFileName, Encoding.Default);
                File.Delete(Service_NCSB.MyPath + strOutFileName);
                //StreamWriter sw = File.CreateText(Service_PZHSB.MyPath + strOutFileName);
                System.Text.UTF8Encoding utf8 = new UTF8Encoding(false);//去除utf8文件开头的BOM
                StreamWriter sw = new StreamWriter(Service_NCSB.MyPath + strOutFileName, false, utf8);
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                string strLine = "",strOutLine = "";

                while ((strLine = sr.ReadLine()) != null)
                {
                    //Service_PZHSB.WriteLog("strLine=[" + strLine + "]");
                    if (!Encrypt_AES_H(strLine, strKey, out strOutLine))
                    {
                        sr.Close();
                        sw.Close();
                        return false;
                    }
                    //Service_PZHSB.WriteLog("strOutLine=[" + strOutLine + "]");
                    sw.WriteLine(strOutLine);
                }
                sw.Flush();
                sr.Close();
                sw.Close();
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("err=[" + err.Message + "]");
                return false;
            }
            
            return true;
        }

        public static bool U2G(string strUTF8,out string strGB2312)
        {
            strGB2312 = "";
            try
            {
                byte[] bUTF8 = UTF8Encoding.UTF8.GetBytes(strUTF8);
                strGB2312 = Encoding.GetEncoding("gb2312").GetString(bUTF8);
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("err=[" + err.Message + "]");
                return false;
            }
            

            return true;
        }

        /// <summary>        
        /// RSA加密        
        /// </summary>        
        /// <param name="publickey"></param>        
        /// <param name="content"></param>        
        /// <returns></returns>        
        public static string RSAEncrypt(string publickey, string content)
        {
            string strResult = "eyJtZXRob2QiOiJzZWFyY2hiaWxsIiwicGFyYW1ldGVyIjp7InVuaXRpZCI6IjAwMDA3NDc5In19";
            
            publickey = @"<RSAKeyValue><Modulus>AJ7FqII2daSG8AeHjP96ZMEaXRNYpKCiW33GzQ2BYTaler3tMWVOum2InQtcX2AD/qYvwyvfHZ/eHi7oeDsilqk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            Service_NCSB.WriteLog(content);
            content = "http://127.0.0.1:8080/wems/interface/3rdpart";            
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            Service_NCSB.WriteLog("Encrypt over!");
            byte[] arr = new byte[cipherbytes.Length - 1];

            if (cipherbytes[0] == 0)
            {
                Array.Copy(cipherbytes, 1, arr, 0, cipherbytes.Length - 1);
                strResult = Convert.ToBase64String(arr);
            }
            else
                strResult = Convert.ToBase64String(cipherbytes);

            Service_NCSB.WriteLog(strResult);
            return strResult;
        }

        /// <summary>        
        /// RSA解密        
        /// </summary>        
        /// <param name="privatekey"></param>        
        /// <param name="content"></param>        
        /// <returns></returns>        
        public static string RSADecrypt(string privatekey, string content)
        {
            //privatekey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e+BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=</Modulus><Exponent>AQAB</Exponent><P>/hf2dnK7rNfl3lbqghWcpFdu778hUpIEBixCDL5WiBtpkZdpSw90aERmHJYaW2RGvGRi6zSftLh00KHsPcNUMw==</P><Q>6Cn/jOLrPapDTEp1Fkq+uz++1Do0eeX7HYqi9rY29CqShzCeI7LEYOoSwYuAJ3xA/DuCdQENPSoJ9KFbO4Wsow==</Q><DP>ga1rHIJro8e/yhxjrKYo/nqc5ICQGhrpMNlPkD9n3CjZVPOISkWF7FzUHEzDANeJfkZhcZa21z24aG3rKo5Qnw==</DP><DQ>MNGsCB8rYlMsRZ2ek2pyQwO7h/sZT8y5ilO9wu08Dwnot/7UMiOEQfDWstY3w5XQQHnvC9WFyCfP4h4QBissyw==</DQ><InverseQ>EG02S7SADhH1EVT9DD0Z62Y0uY7gIYvxX/uq+IzKSCwB8M2G7Qv9xgZQaQlLpCaeKbux3Y59hHM+KpamGL19Kg==</InverseQ><D>vmaYHEbPAgOJvaEXQl+t8DQKFT1fudEysTy31LTyXjGu6XiltXXHUuZaa2IPyHgBz0Nd7znwsW/S44iql0Fen1kzKioEL3svANui63O3o5xdDeExVM6zOf1wUUh/oldovPweChyoAdMtUzgvCbJk1sYDJf++Nr0FeNW1RB1XG30=</D></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
            return Encoding.UTF8.GetString(cipherbytes);
        }

        /// <summary>
        /// 给任意长度字符串加密(AES)
        /// </summary>
        /// <param name="toDecrypt">待加密数据</param>
        /// <param name="key">字符串形式的密钥</param>
        /// <param name="strOut">base64字符串</param>
        /// <returns>成功与否</returns>
        public static bool Encrypt_AES_B64(string toEncrypt, string key, out string strOut)
        {
            strOut = "";
            if (key == "")
            {
                strOut = toEncrypt;
                return true;
            }
            try
            {
                byte[] keyArray = new byte[16];
                byte[] bkey = UTF8Encoding.UTF8.GetBytes(key);

                for (int i = 0; i < 16; i++)
                {
                    if (i < bkey.Length)
                    {
                        keyArray[i] = bkey[i];
                    }
                }

                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                RijndaelManaged rDel = new RijndaelManaged();

                rDel.Key = keyArray;

                rDel.Mode = CipherMode.ECB;

                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                
                //for (int i = 0; i < resultArray.Length; i++)
                //{
                //    strOut += String.Format("{0:X2}", resultArray[i]);
                //}

                //return UTF8Encoding.UTF8.GetString(resultArray);

                strOut= Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 给任意长度字符串解密(AES)
        /// </summary>
        /// <param name="toDecrypt">待解密数据base64字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="strOut">解密后数据</param>
        /// <returns>成功与否</returns>
        public static bool Decrypt_AES_B64(string toDecrypt, string key, out string strOut)
        {
            strOut = "";
            if (key == "")
            {
                strOut = toDecrypt;
                return true;
            }
            try
            {
                byte[] keyArray = new byte[16];
                byte[] bkey = UTF8Encoding.UTF8.GetBytes(key);

                for (int i = 0; i < 16; i++)
                {
                    if (i < bkey.Length)
                    {
                        keyArray[i] = bkey[i];
                    }
                }

                if (toDecrypt.Length % 2 != 0) return false;

                
                int iTotal = toDecrypt.Length / 2;
                byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);
                Service_NCSB.WriteLog("FromBase64String OK!");
                //byte[] toDecryptArray = new byte[toDecrypt.Length / 2];
                //for (int i = 0; i < toDecrypt.Length; i = i + 2)
                //{
                //    strTmp = toDecrypt[i].ToString() + toDecrypt[i + 1].ToString();
                //    bTmp = Convert.ToByte(strTmp, 16);
                //    toDecryptArray[i / 2] = bTmp;
                //}

                RijndaelManaged rDel = new RijndaelManaged();

                rDel.Key = keyArray;

                rDel.Mode = CipherMode.ECB;

                rDel.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                Service_NCSB.WriteLog("toDecryptArray.Length [" + toDecryptArray.Length.ToString() + "]");
                byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                Service_NCSB.WriteLog("TransformFinalBlock OK!");
                strOut = UTF8Encoding.UTF8.GetString(resultArray);
                //strOut = Encoding.Default.GetString(resultArray);

            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog(err.Message);
                return false;
            }

            return true;
        }


        /// <summary>
        /// 根据错误码生成错误返回报文
        /// </summary>
        /// <param name="iHead">包头长度</param>
        /// <param name="strErrCode">错误码</param>
        /// <returns></returns>
        public static string MakeErrPkg(int iHead,string strErrCode)
        {
            string strRetPkg = "";
            string strFormt = "{0:D" + iHead.ToString() + "}";

            strRetPkg = "<ap><RetCode>" + strErrCode + "</RetCode><RetMsg>" + GetRetMsg(strErrCode) + "</RetMsg></ap>";
            strRetPkg = String.Format(strFormt, GetLength_Chinese(strRetPkg)) + strRetPkg;

            return strRetPkg;

        }

        /// <summary>
        /// 生成文本文件
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="FileInfo"></param>
        public static void MakeTxtFile(string FileName, string FileInfo)
        {

            FileStream fs;
            StreamWriter sw;
            fs = new FileStream(FileName, FileMode.Create | FileMode.Append);
            sw = new StreamWriter(fs);
            sw.Write(FileInfo);
            sw.Flush();
            sw.Close();
            fs.Close();

        }

        public static void MakeBlackListFile(string FileName,string ListNo, string CardID, string Reason, string ChgType, string CardCnt)
        {
            //string ListNo = "229999" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string strBlackListInfo = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>\n";
            strBlackListInfo += "<etc>\n<head>\n<ListNo>";
            strBlackListInfo += ListNo;
            strBlackListInfo += "</ListNo>\n<blackListCount>1</blackListCount>\n</head>\n<body>\n<list>\n<CardNet>";
            strBlackListInfo += "5101";
            strBlackListInfo += "</CardNet>\n<CardID>";
            strBlackListInfo += CardID;
            strBlackListInfo += "</CardID>\n<Reason>";
            strBlackListInfo += Reason;
            strBlackListInfo += "</Reason>\n<ChgType>";
            strBlackListInfo += ChgType;
            strBlackListInfo += "</ChgType>\n<CardCnt>";
            strBlackListInfo +=CardCnt;
            strBlackListInfo +="</CardCnt>\n</list>\n</body>\n</etc>\n";

            MakeTxtFile(FileName, strBlackListInfo);

            return;

        }

        public static string GetMD5HashFromFile(string FileName)
        {
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                    
                }

                return sb.ToString();
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog(err.Message);
                return "";
            }


        }

        public static bool CompressFile(string SourceFile, string DestinationFile)
        {
            if (File.Exists(SourceFile)==false)//判断文件是否存在
            {
                Service_NCSB.WriteLog("文件[" + SourceFile + "]不存在!");
                return false;
            }
            //创建文件流和字节数组
            byte[] buffer = null;
            FileStream sourceStream = null;
            FileStream destinationStream = null;
            GZipStream compressStream = null;

            try
            {
                sourceStream = new FileStream(SourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[sourceStream.Length];
                //把文件流存放到字节数组中
                int checkCounter = sourceStream.Read(buffer, 0, buffer.Length);
                if (checkCounter != buffer.Length)
                {
                    Service_NCSB.WriteLog("文件[" + SourceFile + "]读错误!");
                    return false;
                }
                destinationStream = new FileStream(DestinationFile, FileMode.OpenOrCreate, FileAccess.Write);
                //创建GzipStream实例，写入压缩的文件流
                compressStream = new GZipStream(destinationStream, CompressionMode.Compress, true);
                compressStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception err)
            {
                Service_NCSB.WriteLog("压缩文件时发生错误[" + err.Message + "]");
                return false;
            }
            finally
            {
                //确保总是关闭所有的流
                if (compressStream != null) compressStream.Close();
                if (sourceStream != null) sourceStream.Close();
                if (destinationStream != null) destinationStream.Close();                
            }

            return true;
        }

        /// <summary>
        /// 字符串分隔符文件转xml文件
        /// </summary>
        /// <param name="XmlFileName"></param>
        /// <param name="StrFileName"></param>
        /// <param name="strRetMsg"></param>
        /// <returns></returns>
        public static bool StrFile2XmlFile(string StrFileName, string XmlFileName, out string strRetMsg)
        {
            strRetMsg = "交易成功";
            string XmlFileInfo = "";          

            //Service_NCSB.WriteLog("XmlFileName:" + XmlFileName);
            //Service_NCSB.WriteLog("StrFileName:" + StrFileName);            

            try
            {
                string[] strLines = File.ReadAllLines(StrFileName);
                int iLine = 0;
                while (iLine < strLines.Length)
                {
                    //Service_NCSB.WriteLog(strLines[iLine]);
                    string[] jy = strLines[iLine].Split('|');
                    iLine++;
                    if (iLine==1)//第一行
                    {
                        if (jy.Length<3)
                        {
                            strRetMsg = "第[" + iLine + "]行数据错误！";
                            Service_NCSB.WriteLog(strRetMsg);
                            return false;
                        }
                        XmlFileInfo = "<dz>\n\t<yhbh>"+jy[0]+"</yhbh>\n\t<dzsj>\n\t\t<jykssj>";
                        XmlFileInfo += jy[1] + "</jykssj>\n\t\t<jyjssj>";
                        XmlFileInfo += jy[2] + "</jyjssj>\n\t</dzsj>\n\t<dzjy>\n";
                        continue;
                    }
                    if (jy.Length < 14)
                    {
                        strRetMsg = "第[" + iLine + "]行数据错误！";
                        Service_NCSB.WriteLog(strRetMsg);
                        return false;
                    }
                    XmlFileInfo += "\t\t<jy>\n\t\t\t<jylsh>" + jy[0] + "</jylsh>\n";
                    XmlFileInfo += "\t\t\t<grbm>" + jy[1] + "</grbm>\n";
                    XmlFileInfo += "\t\t\t<jfxz>" + jy[2] + "</jfxz>\n";
                    XmlFileInfo += "\t\t\t<ksqh>" + jy[3] + "</ksqh>\n";
                    XmlFileInfo += "\t\t\t<jzqh>" + jy[4] + "</jzqh>\n";
                    XmlFileInfo += "\t\t\t<jfdc>" + jy[5] + "</jfdc>\n";
                    XmlFileInfo += "\t\t\t<sbjbh>" + jy[6] + "</sbjbh>\n";
                    XmlFileInfo += "\t\t\t<yjbj>" + jy[7] + "</yjbj>\n";
                    XmlFileInfo += "\t\t\t<lx>" + jy[8] + "</lx>\n";
                    XmlFileInfo += "\t\t\t<znj>" + jy[9] + "</znj>\n";
                    XmlFileInfo += "\t\t\t<cwsxh>" + jy[10] + "</cwsxh>\n";
                    XmlFileInfo += "\t\t\t<jysj>" + jy[11] + "</jysj>\n";
                    XmlFileInfo += "\t\t\t<yhhh>" + jy[12] + "</yhhh>\n";
                    XmlFileInfo += "\t\t\t<jylx>" + jy[13] + "</jylx>\n\t\t</jy>\n";
                }

                XmlFileInfo += "\t</dzjy>\n</dz>";
                
                File.WriteAllText(XmlFileName, XmlFileInfo);
                return true;

            }
            catch (Exception err)
            {                
                strRetMsg = err.Message;
                return false;
            }

            //return false;
        }


    }
}
