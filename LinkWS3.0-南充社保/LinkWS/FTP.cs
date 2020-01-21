using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace LinkWS
{
    /// <summary>
    /// FTP��
    /// �ṩ����FTP�ϴ����ص����ݴ�����
    /// </summary>
    class FTP
    {
        string ftpServerIP;         //������ip��ַ
        string ftpUserID;           //�û���
        string ftpPassword;         //����
        long ftpTimeOut;            //��ʱ����
        FtpWebRequest reqFTP;

        //����ftp
        private void Connect(String path)
        {
            // ����uri����FtpWebRequest����
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            // ָ�����ݴ�������
            reqFTP.UseBinary = true;
            // ftp�û���������
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            //reqFTP.Timeout = ftpTimeOut;
        }

        public FTP(string ftpserverip, string ftpuserid, string ftppassword)
        {
            this.ftpServerIP = ftpserverip;
            this.ftpUserID = ftpuserid;
            this.ftpPassword = ftppassword;
        }

        //��ftp�������ϻ���ļ��б�
        private string[] Dir(string path, string wrmethods)
        {
            string[] downloadfiles;
            StringBuilder result = new StringBuilder();
            try
            {
                Connect(path);
                reqFTP.Method = wrmethods;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                string line = reader.ReadLine();

                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                downloadfiles = result.ToString().Split('\n');
            }
            catch (Exception)
            {
                downloadfiles = null;
            }
            return downloadfiles;
        }

        //��ftp�������ϻ���ļ��б�
        public string[] Dir(string path)
        {
            return Dir("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectory);
        }

        //��ftp�������ϻ���ļ��б�
        public string[] Dir()
        {
            return Dir("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectory);
        }

        //��ftp�����������ļ��Ĺ���
        public bool Put(string path,string filename, out string errinfo)
        {
            errinfo = "ִ�гɹ�";
            FileInfo fileInf = new FileInfo(filename);
            string uri = "ftp://" + ftpServerIP + "/" + path + "/" + fileInf.Name;
            if (path=="") uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
            
            Service_NCSB.WriteLog("uri=[" + uri + "]");
            try
            {
                Connect(uri);//����
                // Ĭ��Ϊtrue�����Ӳ��ᱻ�ر�
                // ��һ������֮��ִ��
                reqFTP.KeepAlive = false;
                // ָ��ִ��ʲô����
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                // �ϴ��ļ�ʱ֪ͨ�������ļ��Ĵ�С
                reqFTP.ContentLength = fileInf.Length;
                // �����С����Ϊkb 
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int iReadLen;
                // ��һ���ļ���(System.IO.FileStream) ȥ���ϴ����ļ�
                FileStream fs = fileInf.OpenRead();
                // ���ϴ����ļ�д����
                Stream stmFtpPut = reqFTP.GetRequestStream();
                // ÿ�ζ��ļ��������ݴ浽������buff
                iReadLen = fs.Read(buff, 0, buffLength);
                // ������û�н���
                while (iReadLen != 0)
                {
                    // �ѻ�����buff������д��stmFtpPut 
                    stmFtpPut.Write(buff, 0, iReadLen);
                    iReadLen = fs.Read(buff, 0, buffLength);
                }
                // �ر�������
                stmFtpPut.Close();
                fs.Close();
            }
            catch (Exception err)
            {
                errinfo = err.Message;
                return false;
            }
            return true;
        }

        //��ftp�����������ļ��Ĺ���
        public bool Get(string filePath,string remotepath, string fileName, out string errorinfo)
        {
            errorinfo = "ִ�гɹ�";
            try
            {
                String onlyFileName = Path.GetFileName(fileName);
                string newFileName = filePath + "\\" + onlyFileName;
                //if (File.Exists(newFileName))
                //{
                //    errorinfo = string.Format("�����ļ�{0}�Ѵ���,�޷�����", newFileName);
                //    return false;
                //}
                string url = "ftp://" + ftpServerIP + "/" + remotepath + "/" + fileName;
                Service_NCSB.WriteLog("url=[" + url + "]");
                Connect(url);//���� 
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];
                int readCount = ftpStream.Read(buffer, 0, bufferSize);
                //����ļ������򱻸���
                FileStream outputStream = new FileStream(newFileName, FileMode.Create);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }

            catch (Exception err)
            {
                errorinfo = err.Message;
                return false;
            }

            return true;

        }

        //ɾ���ļ�
        public bool Rm(string filename, out string errinfo)
        {
            errinfo = "ִ�гɹ�";
            try
            {
                FileInfo fileInf = new FileInfo(filename);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(uri);//����
                // Ĭ��Ϊtrue�����Ӳ��ᱻ�ر�
                // ��һ������֮��ִ��
                reqFTP.KeepAlive = false;
                // ָ��ִ��ʲô����
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception err)
            {
                errinfo = err.Message;
                return false;
            }
            return true;
        }

        //����Ŀ¼
        public bool Mkdir(string dirname, out string errinfo)
        {
            errinfo = "ִ�гɹ�";
            try
            {
                string uri = "ftp://" + ftpServerIP + "/" + dirname;
                Connect(uri);//����
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception err)
            {
                errinfo = err.Message;
                return false;
            }
            return true;
        }

        //ɾ��Ŀ¼
        public bool Rmdir(string dirname, out string errinfo)
        {
            errinfo = "ִ�гɹ�";
            try
            {
                string uri = "ftp://" + ftpServerIP + "/" + dirname;
                Connect(uri);//����
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception err)
            {
                errinfo = err.Message;
                return false;
            }
            return true;
        }

        //����ļ���С
        public long GetFileSize(string filename)
        {
            long fileSize = 0;
            try
            {
                FileInfo fileInf = new FileInfo(filename);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(uri);//����
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                fileSize = response.ContentLength;
                response.Close();
            }
            catch (Exception)
            {
            }

            return fileSize;

        }

        //�ļ�����
        public bool Rename(string currentFilename, string newFilename, out string errinfo)
        {
            errinfo = "ִ�гɹ�";
            try
            {
                FileInfo fileInf = new FileInfo(currentFilename);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(uri);//����
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                //Stream ftpStream = response.GetResponseStream();
                //ftpStream.Close();
                response.Close();
            }
            catch (Exception err)
            {
                errinfo = err.Message;
                return false;
            }
            return true;

        }

        //����ļ�����
        public string[] Ls()
        {
            return Dir("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectoryDetails);
        }

        //����ļ�����
        public string[] Ls(string path)
        {
            return Dir("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectoryDetails);
        }

    }
}
