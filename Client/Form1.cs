using Client.ServiceReference1;
using Client.ServiceReference2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        InstanceContext instanceContext;

        Service1Client client;
        LoadClient client2;


        private static Form1 INSTANCE;
         
        public Form1()
        {
            INSTANCE = this;
            InitializeComponent();
            instanceContext = new InstanceContext(new CallbackHandler());
            client = new Service1Client();
            client2 = new LoadClient(instanceContext);
        }

        public static Form1 getInstance(){
            return INSTANCE;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updateListBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //string filename;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //filename = openFileDialog1.FileName.Split('\\').Last();
                

                FileStream imageFile = File.OpenRead(openFileDialog1.FileName);

                //MessageBox.Show(imageFile.Name.Split('\\').Last());
                client2.Dodaj(imageFile);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                }
                long length = client.getFileLength(listBox1.Items[index].ToString());
                progressBar1.Minimum = 0;
                progressBar1.Maximum = (int)length;
                
                Console.WriteLine("Wywoluje GetStream()");
                System.IO.Stream stream2 = client.GetStream(listBox1.Items[index].ToString());
                
                ZapiszPlik(stream2, "image.jpg");

                Console.WriteLine();

                Image image = Image.FromFile("image.jpg");
                pictureBox1.Image = image;
                pictureBox1.Height = image.Height;
                pictureBox1.Width = image.Width;
            }
        }

        void ZapiszPlik(System.IO.Stream stream, string filePath)
        {
            Console.WriteLine("Zapisz plik {0}", filePath);
            FileStream outstream = File.Open(filePath, FileMode.Create, FileAccess.Write);
            KopiujStrumien(stream, outstream);
            outstream.Close();
            stream.Close();

            Console.WriteLine();
            Console.WriteLine("Plik {0} zapisany", filePath);
        }

        void KopiujStrumien(System.IO.Stream instream, System.IO.Stream outstream)
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

                progressBar1.Value = bytecount;
                
            }
            Console.WriteLine();
            Console.WriteLine("Zapisano {0} bajtow", bytecount);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Close();
        }

        private void updateListBox(){
            string[] filenames = client.getFilenames();
            listBox1.Items.Clear();
            foreach (string filename in filenames)
            {
                listBox1.Items.Add(filename);
            }
        }

        private class CallbackHandler : ILoadCallback
        {
            public void updateFileList()
            {
                INSTANCE.updateListBox();
            }
        }   

    }

    
}
