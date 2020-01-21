using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace LinkWS
{
    /// <summary>
    /// FTP类
    /// 提供常用FTP上传下载等数据处理函数
    /// </summary>
    class FTP
    {
        string ftpServerIP;         //服务器ip地址
        string ftpUserID;           //用户名
        string ftpPassword;         //密码
        long ftpTimeOut;            //超时设置
        FtpWebRequest reqFTP;

        //连接ftp
        private void Connect(String path)
        {
            // 根据uri创建FtpWebRequest对象
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
            // 指定数据传输类型
            reqFTP.UseBinary = true;
            // ftp用户名和密码
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            //reqFTP.Timeout = ftpTimeOut;
        }

        public FTP(string ftpserverip, string ftpuserid, string ftppassword)
        {
            this.ftpServerIP = ftpserverip;
            this.ftpUserID = ftpuserid;
            this.ftpPassword = ftppassword;
        }

        //从ftp服务器上获得文件列表
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

        //从ftp服务器上获得文件列表
        public string[] Dir(string path)
        {
            return Dir("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectory);
        }

        //从ftp服务器上获得文件列表
        public string[] Dir()
        {
            return Dir("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectory);
        }

        //从ftp服务器上载文件的功能
        public bool Put(string path,string filename, out string errinfo)
        {
            errinfo = "执行成功";
            FileInfo fileInf = new FileInfo(filename);
            string uri = "ftp://" + ftpServerIP + "/" + path + "/" + fileInf.Name;
            if (path=="") uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
            
            Service_NCSB.WriteLog("uri=[" + uri + "]");
            try
            {
                Connect(uri);//连接
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                // 上传文件时通知服务器文件的大小
                reqFTP.ContentLength = fileInf.Length;
                // 缓冲大小设置为kb 
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int iReadLen;
                // 打开一个文件流(System.IO.FileStream) 去读上传的文件
                FileStream fs = fileInf.OpenRead();
                // 把上传的文件写入流
                Stream stmFtpPut = reqFTP.GetRequestStream();
                // 每次读文件流的内容存到缓冲区buff
                iReadLen = fs.Read(buff, 0, buffLength);
                // 流内容没有结束
                while (iReadLen != 0)
                {
                    // 把缓冲区buff的内容写入stmFtpPut 
                    stmFtpPut.Write(buff, 0, iReadLen);
                    iReadLen = fs.Read(buff, 0, buffLength);
                }
                // 关闭两个流
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

        //从ftp服务器下载文件的功能
        public bool Get(string filePath,string remotepath, string fileName, out string errorinfo)
        {
            errorinfo = "执行成功";
            try
            {
                String onlyFileName = Path.GetFileName(fileName);
                string newFileName = filePath + "\\" + onlyFileName;
                //if (File.Exists(newFileName))
                //{
                //    errorinfo = string.Format("本地文件{0}已存在,无法下载", newFileName);
                //    return false;
                //}
                string url = "ftp://" + ftpServerIP + "/" + remotepath + "/" + fileName;
                Service_NCSB.WriteLog("url=[" + url + "]");
                Connect(url);//连接 
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];
                int readCount = ftpStream.Read(buffer, 0, bufferSize);
                //如果文件存在则被覆盖
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

        //删除文件
        public bool Rm(string filename, out string errinfo)
        {
            errinfo = "执行成功";
            try
            {
                FileInfo fileInf = new FileInfo(filename);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(uri);//连接
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;
                // 指定执行什么命令
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

        //创建目录
        public bool Mkdir(string dirname, out string errinfo)
        {
            errinfo = "执行成功";
            try
            {
                string uri = "ftp://" + ftpServerIP + "/" + dirname;
                Connect(uri);//连接
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

        //删除目录
        public bool Rmdir(string dirname, out string errinfo)
        {
            errinfo = "执行成功";
            try
            {
                string uri = "ftp://" + ftpServerIP + "/" + dirname;
                Connect(uri);//连接
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

        //获得文件大小
        public long GetFileSize(string filename)
        {
            long fileSize = 0;
            try
            {
                FileInfo fileInf = new FileInfo(filename);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(uri);//连接
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

        //文件改名
        public bool Rename(string currentFilename, string newFilename, out string errinfo)
        {
            errinfo = "执行成功";
            try
            {
                FileInfo fileInf = new FileInfo(currentFilename);
                string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;
                Connect(uri);//连接
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

        //获得文件明晰
        public string[] Ls()
        {
            return Dir("ftp://" + ftpServerIP + "/", WebRequestMethods.Ftp.ListDirectoryDetails);
        }

        //获得文件明晰
        public string[] Ls(string path)
        {
            return Dir("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectoryDetails);
        }

    }
}
