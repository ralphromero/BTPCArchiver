using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BTPCArchiver
{
    class FileArchiver : System.IDisposable
    {
        string propertyName = null;
        string fileName = null;
        string propertyNS = null;
        string filenameExtension = null;
        FileStream fs;
        private string finalFilePath;
        public string FinalFilePath
        {
            get { return finalFilePath; }
        }

        public long Position
        {
            get { return fs.Position; }
            set { fs.Position = value; }
        }

        public FileArchiver(string propertyNS, string propertyName, string filenameExtension, string filePath, Guid messageID)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");
            if (propertyNS == null)
                throw new ArgumentNullException("propertyNS");
            if (propertyNS == null)
                this.filenameExtension = "xml";
            if (filePath == null)
                throw new ArgumentNullException("filepath");

            this.propertyName = propertyName;
            fileName = messageID.ToString();
            this.propertyNS = propertyNS;
            this.filenameExtension = filenameExtension;

            finalFilePath = CheckPath(filePath) + fileName + "." + filenameExtension.Replace(".", "");
            fs = new FileStream(finalFilePath, FileMode.Create);
        }

        public void write(byte[] buffer, int offset, int count)
        {
            fs.Write(buffer, offset, count);
        }

        public void Dispose()
        {
            fs.Close();
            fs.Dispose();
        }

        public string CheckPath(string path)
        {
            string correctedPath = path;

            if (path.Substring(path.Length - 1) == @"\")
            {
                if (!new DirectoryInfo(path).Exists)
                    Directory.CreateDirectory(path.Substring(0, path.Length - 1));
            }
            else
            {
                if (!new DirectoryInfo(path).Exists)
                    Directory.CreateDirectory(path);
                correctedPath = path + @"\";
            }

            return correctedPath;
        }
    }
}
