using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace _2lab
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        private static byte[] GetIV(string ivSecret)
        {
            using MD5 md5 = MD5.Create();
            return md5.ComputeHash(Encoding.UTF8.GetBytes(ivSecret));
        }
        private static byte[] GetKey(string key)
        {
            using SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        static string sKey;
        static string ivSecret = "вектор";
        static string values = "";
        private void button1_Click(object sender, EventArgs e)
        {

            sKey = textBox1.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string source = openFileDialog1.FileName;
                if (values == "AES") { saveFileDialog1.Filter = "enc files |*.enc"; }
                else { saveFileDialog1.Filter = "des files |*.des"; }

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string destination = saveFileDialog1.FileName; // Переменная в которую сохроняем зашифрованный файл
                    if (values == "AES")
                    {
                        EncryptFileAes(source, destination, ivSecret, sKey);
                    }
                    else { EncryptFile(source, destination, sKey); }

                }
            }
        }
        private void EncryptFileAes(string source, string destination, string ivSecret, string sKey)
        {
            using Aes aes = Aes.Create();
            aes.IV = GetIV(ivSecret);
            aes.Key = GetKey(sKey);
            using FileStream inStream = new FileStream(source, FileMode.Open); //создаем файловый поток на чтение
            using FileStream outStream = new FileStream(destination, FileMode.Create);//создаем файловый поток на запись
                                                                                      //поток для шифрования данных
            CryptoStream encStream = new CryptoStream(outStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write);
            long readTotal = 0;

            int len;
            int tempSize = 100; //размер временного хранилища
            byte[] bin = new byte[tempSize];    //временное Хранилище для зашифрованной информации
            while (readTotal < inStream.Length)
            {
                len = inStream.Read(bin, 0, tempSize);
                encStream.Write(bin, 0, len);
                readTotal = readTotal + len;
                MessageBox.Show($"{readTotal} байт обработано");
            }
            encStream.Close();
            outStream.Close();
            inStream.Close();
        }
        private void EncryptFile(string source, string destination, string sKey)
        {
            FileStream fsInput = new FileStream(source, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypted = new FileStream(destination, FileMode.Create, FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            try
            {
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = DES.CreateEncryptor();
                CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
                byte[] bytearrayinput = new byte[fsInput.Length - 0];
                fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка");
                return;
            }
            fsInput.Close();
            fsEncrypted.Close();

        }

        private void DecryptFile(string source, string destination, string sKey)
        {
            FileStream fsInput = new FileStream(source, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypted = new FileStream(destination, FileMode.Create, FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            try
            {
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = DES.CreateDecryptor();
                CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
                byte[] bytearrayinput = new byte[fsInput.Length - 0];
                fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
            fsInput.Close();
            fsEncrypted.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            sKey = textBox1.Text;
            if (values == "AES") { openFileDialog1.Filter = "enc files |*.enc"; }
            else { openFileDialog1.Filter = "des files|*.des"; }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string source = openFileDialog1.FileName;
                saveFileDialog1.Filter = "txt files |*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string destination = saveFileDialog1.FileName; // Переменная в которую сохроняем зашифрованный файл
                    if (values == "AES")
                    {
                        DecryptFileAes(source, destination, GetKey(sKey), GetKey(ivSecret));
                    }
                    else { DecryptFile(source, destination, sKey); }

                }
            }
        }
        private void DecryptFileAes(string source, string destination, byte[] key, byte[] iv)
        {
            using FileStream fileStream = new(source, FileMode.Open);
            using Aes aes = Aes.Create();

            aes.IV = GetIV(ivSecret);

            using CryptoStream cryptoStream = new(fileStream,
                                       aes.CreateDecryptor(key, aes.IV),
                                                  CryptoStreamMode.Read); //создаем поток для чтения (расшифровки) данных
            using FileStream outStream = new FileStream(destination, FileMode.Create);//создаем поток для расшифрованных данных

            using BinaryReader decryptReader = new(cryptoStream);
            int tempSize = 10;  //размер временного хранилища
            byte[] data;        //временное хранилище для зашифрованной информации
            while (true)
            {
                data = decryptReader.ReadBytes(tempSize);
                if (data.Length == 0)
                    break;
                outStream.Write(data, 0, data.Length);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            values = comboBox1.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var rijndael = System.Security.Cryptography.Rijndael.Create())
            {
                rijndael.GenerateKey();
                sKey = Convert.ToBase64String(rijndael.Key);
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string source = openFileDialog1.FileName;
                saveFileDialog1.Filter = "enc files |*.enc";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string destination = saveFileDialog1.FileName; // Переменная в которую сохроняем зашифрованный файл

                    EncryptFileAes(source, destination, ivSecret, sKey);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
             openFileDialog1.Filter = "enc files |*.enc"; 
          

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string source = openFileDialog1.FileName;
                saveFileDialog1.Filter = "txt files |*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string destination = saveFileDialog1.FileName; // Переменная в которую сохроняем зашифрованный файл
                    
                        DecryptFileAes(source, destination, GetKey(sKey), GetKey(ivSecret));
                   

                }
            }
        }
    }
}