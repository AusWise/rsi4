using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Contract
{
    public class Service1 : IService1
    {
        public string[] getFilenames()
        {
            string[] filePaths = Directory.GetFiles("Images");
            return filePaths;
        }

        public System.IO.Stream GetStream(string filepath)
        {
            //zakladamy, ze obraz jest w tym samym folderze
            //string filePath = Path.Combine(System.Environment.CurrentDirectory, ".\\image.jpg");

            //wyjatek na wypadek bledu otwarcia pliku
            try
            {
                FileStream imageFile = File.OpenRead(filepath);
                return imageFile;
            }
            catch (IOException ex)
            {
                Console.WriteLine("Wyjatek: ");
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public long getFileLength(string filepath)
        {
            long length = new System.IO.FileInfo(filepath).Length;
            return length;
        }
    }
}
