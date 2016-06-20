using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Load : ILoad
    {

        ILoadCallback callback = null;
        public Load()
        {
            callback = OperationContext.Current.GetCallbackChannel<ILoadCallback>();
        }

        public void Dodaj(System.IO.Stream stream)
        {

            
            //Console.WriteLine(filename);
            ZapiszPlik(stream, "Images\\" + RandomString(9)); 
            callback.updateFileList();
        }

        private void ZapiszPlik(System.IO.Stream stream, string filePath)
        {
            Console.WriteLine("Zapisz plik {0}", filePath);
            FileStream outstream = File.Open(filePath, FileMode.Create, FileAccess.Write);
            KopiujStrumien(stream, outstream);
            outstream.Close();
            stream.Close();

            Console.WriteLine();
            Console.WriteLine("Plik {0} zapisany", filePath);
        }

        private void KopiujStrumien(System.IO.Stream instream, System.IO.Stream outstream)
        {
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            int bytecount = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
                Console.WriteLine(".{0}", count);
                bytecount += count;

            }
            Console.WriteLine();
            Console.WriteLine("Zapisano {0} bajtow", bytecount);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
