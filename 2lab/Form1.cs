using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace _2lab
{
    public partial class Form1 : Form
    {
        private const int sizeOfBlock = 128; //в DES размер блока 64 бит, но поскольку в unicode символ в два раза длинее, то увеличим блок тоже в два раза
        private const int sizeOfChar = 16; //размер одного символа (in Unicode 16 bit)

        private const int shiftKey = 2; //сдвиг ключа 

        private const int quantityOfRounds = 16; //количество раундов

        string[] Blocks; //сами блоки в двоичном формате
        DES des = DES.Create();
        Aes aes = Aes.Create();
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
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
        static string valuesMethod = "";
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

            if (values == "AES")
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
            else
            {
                openFileDialog1.Filter = "txt files |*.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string source = openFileDialog1.FileName;
                    saveFileDialog1.Filter = "des files |*.des";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string destination = saveFileDialog1.FileName; // Переменная в которую сохроняем зашифрованный файл

                        EncryptFileDesRandKey(source, destination);
                    }
                }

            }

        }
        private void EncryptFileDesRandKey(string source, string destination)
        {
            FileStream inStream = new FileStream(source, FileMode.Open, FileAccess.Read);
            FileStream outStream = new FileStream(destination, FileMode.Create, FileAccess.Write);
            CryptoStream encStream = new CryptoStream(outStream, des.CreateEncryptor(des.Key, des.IV), CryptoStreamMode.Write);
            long readTotal = 0;

            int len;
            int tempSize = 100; //размер временного хранилища
            byte[] bin = new byte[tempSize];    //временное Хранилище для зашифрованной информации
            while (readTotal < inStream.Length)
            {
                len = inStream.Read(bin, 0, tempSize);
                encStream.Write(bin, 0, len);
                readTotal = readTotal + len;
            }
            encStream.Close();
            outStream.Close();
            inStream.Close();

            ////

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (values == "AES")
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
            else
            {
                openFileDialog1.Filter = "des files|*.des";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string source = openFileDialog1.FileName;
                    saveFileDialog1.Filter = "txt files |*.txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string destination = saveFileDialog1.FileName; // Переменная в которую сохроняем зашифрованный файл

                        DecryptFileDesRandKey(source, destination);


                    }
                }
            }

        }
        private void DecryptFileDesRandKey(string source, string destination)
        {
            FileStream fileStream = new FileStream(source, FileMode.Open);
            FileStream outStream = new FileStream(destination, FileMode.Create);

            CryptoStream cryptoStream = new CryptoStream(fileStream, des.CreateDecryptor(des.Key, des.IV), CryptoStreamMode.Read); //создаем поток для чтения (расшифровки) данных
            BinaryReader decryptReader = new BinaryReader(cryptoStream);
            int tempSize = 10;  //размер временного хранилища
            byte[] data;        //временное хранилище для зашифрованной информации
            while (true)
            {
                data = decryptReader.ReadBytes(tempSize);
                if (data.Length == 0)
                    break;
                outStream.Write(data, 0, data.Length);
            }
            cryptoStream.Close();
            outStream.Close();
            fileStream.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Generate a set of symmetric key matrices for 8 network subscribers
            byte[][] symmetricKeys = new byte[8][];
            for (int i = 0; i < 8; i++)
            {
                symmetricKeys[i] = GenerateSymmetricKey();
                SaveSymmetricKeyToFile($"5task/symmetrickey{i + 1}.txt", symmetricKeys[i]);
            }

            // Read the symmetric key matrices from the files
            Console.WriteLine("Symmetric Key Matrices:");
            for (int i = 0; i < 8; i++)
            {
                byte[] keyFromFile = ReadSymmetricKeyFromFile($"5task/symmetrickey{i + 1}.txt");
                Console.WriteLine($"Subscriber {i + 1}: {BitConverter.ToString(keyFromFile).Replace("-", "")}");
            }
        }

        static byte[] GenerateSymmetricKey()
        {
            byte[] symmetricKey = new byte[32]; // 256 bits
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(symmetricKey);
            }
            return symmetricKey;
        }

        static void SaveSymmetricKeyToFile(string fileName, byte[] symmetricKey)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(symmetricKey, 0, symmetricKey.Length);
            }
        }

        static byte[] ReadSymmetricKeyFromFile(string fileName)
        {
            byte[] symmetricKey = new byte[32]; // 256 bits
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                fileStream.Read(symmetricKey, 0, symmetricKey.Length);
            }
            return symmetricKey;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            byte[][] symmetricKeys = new byte[8][];
            for (int i = 0; i < 8; i++)
            {
                symmetricKeys[i] = GenerateSymmetricKey();
            }

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                // Save the public key to a file for distribution to subscribers
                SavePublicKeyToFile("6task/public_key.xml", rsa.ExportParameters(false));

                // Create a key carrier for each subscriber
                for (int i = 0; i < 8; i++)
                {
                    byte[] encryptedSymmetricKey = rsa.Encrypt(symmetricKeys[i], true);
                    SaveKeyCarrierToFile($"6task/keycarrier{i + 1}.bin", encryptedSymmetricKey);
                }
            }
        }
        static void SavePublicKeyToFile(string fileName, RSAParameters publicKey)
        {
            using (var sw = new StreamWriter(fileName))
            {
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, publicKey);
            }
        }

        static void SaveKeyCarrierToFile(string fileName, byte[] encryptedSymmetricKey)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(encryptedSymmetricKey, 0, encryptedSymmetricKey.Length);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Load the public key for the key carrier encryption
            RSAParameters publicKey = LoadPublicKeyFromFile("public_key.xml");

            // Load the symmetric key matrices for the 8 network subscribers
            byte[][] symmetricKeys = new byte[8][];
            for (int i = 0; i < 8; i++)
            {
                symmetricKeys[i] = LoadSymmetricKeyFromFile($"symmetrickey{i + 1}.bin");
            }

            // Simulate a message exchange between two subscribers
            int sender1 = 1;
            int recipient = 2;
            string message = "This is a secret message!";

            byte[] encryptedMessage = EncryptMessage(message, symmetricKeys[sender1 - 1]);
            byte[] encryptedSymmetricKey = LoadKeyCarrierFromFile($"keycarrier{recipient}.bin");
            byte[] decryptedSymmetricKey = DecryptSymmetricKey(encryptedSymmetricKey, publicKey);
            //string decryptedMessage = DecryptMessage(encryptedMessage, decryptedSymmetricKey);

            Console.WriteLine($"Sender: {sender1}");
            Console.WriteLine($"Recipient: {recipient}");
            Console.WriteLine($"Message: {message}");
            Console.WriteLine($"Encrypted message: {Convert.ToBase64String(encryptedMessage)}");
            Console.WriteLine($"Encrypted symmetric key: {Convert.ToBase64String(encryptedSymmetricKey)}");
            Console.WriteLine($"Decrypted symmetric key: {Convert.ToBase64String(decryptedSymmetricKey)}");
            //Console.WriteLine($"Decrypted message: {decryptedMessage}");
        }
        static RSAParameters LoadPublicKeyFromFile(string fileName)
        {
            using (var sr = new StreamReader(fileName))
            {
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                return (RSAParameters)xs.Deserialize(sr);
            }
        }
        static byte[] LoadSymmetricKeyFromFile(string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        static byte[] LoadKeyCarrierFromFile(string fileName)
        {
            return LoadSymmetricKeyFromFile(fileName);
        }

        static byte[] EncryptMessage(string message, byte[] symmetricKey)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = symmetricKey;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(message);
                    cs.FlushFinalBlock();
                    return aes.IV.Concat(ms.ToArray()).ToArray();
                }
            }
        }
        /*
        static byte[] DecryptMessage(byte[] encryptedMessage, byte[] symmetricKey)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = symmetricKey;
                byte[] iv = encryptedMessage.Take(16).ToArray();
                byte[] encryptedData = encryptedMessage.Skip(16).ToArray();

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                using (var ms = new MemoryStream(encryptedData))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                
                
                {
                    return sr.ReadToEnd();
                }
            }
        } */
        static byte[] DecryptSymmetricKey(byte[] encryptedKey, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.Decrypt(encryptedKey, false);
            }
        }
        string destinations;
        string sources;
        private void button8_Click(object sender, EventArgs e)
        {
            // Load the image file into a byte array
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.Filter = "jpg files |*.jpg";
                sources = openFileDialog1.FileName;
                saveFileDialog1.Filter = "jpg files |*.jpg";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    destinations = saveFileDialog1.FileName; // Переменная в которую сохроняем зашифрованный файл
                }
            }
            byte[] imageBytes = File.ReadAllBytes(sources);

            // Generate random encryption keys for each algorithm
            byte[] aesKey = new byte[16];
            byte[] tripleDesKey = new byte[24];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(aesKey);
                rng.GetBytes(tripleDesKey);
            }

            // Choose the encryption modes and set up the CryptoTransform objects
            var aesEncryptor = Aes.Create().CreateEncryptor(aesKey, aesKey);
            var tripleDesEncryptor = TripleDES.Create().CreateEncryptor(aesKey, tripleDesKey);

            // Encrypt the image using the first algorithm (AES with ECB mode)
            byte[] encryptedAesEcb = EncryptImage(imageBytes, aesEncryptor, CipherMode.ECB);

            // Encrypt the image using the second algorithm (TripleDES with CBC mode)
            byte[] encryptedTripleDesCbc = EncryptImage(imageBytes, tripleDesEncryptor, CipherMode.CBC);

            // Save the encrypted images to files
            File.WriteAllBytes("picture_aes_ecb.jpg", encryptedAesEcb);
            File.WriteAllBytes("picture_triple_des_cbc.jpg", encryptedTripleDesCbc);

        }
        static byte[] EncryptImage(byte[] imageBytes, ICryptoTransform encryptor, CipherMode mode)
        {
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                if (mode == CipherMode.CBC)
                {
                    // For CBC mode, generate a random IV and write it to the beginning of the output stream
                    byte[] iv = new byte[encryptor.InputBlockSize];
                    using (var rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(iv);
                    }
                    cs.Write(iv, 0, iv.Length);
                }

                // Encrypt the image data and write it to the output stream
                cs.Write(imageBytes, 0, imageBytes.Length);

                // Return the encrypted bytes
                return ms.ToArray();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            values = comboBox2.Text;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            values = comboBox3.Text;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            valuesMethod = comboBox4.Text;
        }
        private string strIV = "abcdefghijklmnmo"; //The initialization vector.
        private string strIVD = "abcdefgh"; //The initialization vector.
        private string strKey = "abcdefghijklmnmoabcdefghijklmnmo"; //The key
        private string strKeyD = "abcdefgh"; //The key
        public byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (values == "AES")
            {
                if (valuesMethod == "ECB")
                {
                    openFileDialog1.Filter = "bmp files|*.bmp"; //Что читаем
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string source = openFileDialog1.FileName;
                        saveFileDialog1.Filter = "bmp files |*.bmp";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string destination = saveFileDialog1.FileName; // Куда записываем


                            EncryptAecECB(source, destination);

                        }
                    }
                }
                else
                {
                    openFileDialog1.Filter = "bmp files|*.bmp"; //Что читаем
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string source = openFileDialog1.FileName;
                        saveFileDialog1.Filter = "bmp files |*.bmp";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string destination = saveFileDialog1.FileName; // Куда записываем


                            EncryptAecCBCPhoto(source, destination);

                        }
                    }
                }
            }
            else
            {
                if (valuesMethod == "ECB")
                {
                    openFileDialog1.Filter = "bmp files|*.bmp"; //Что читаем
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string source = openFileDialog1.FileName;
                        saveFileDialog1.Filter = "bmp files |*.bmp";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string destination = saveFileDialog1.FileName; // Куда записываем


                            EncryptDECECB(source, destination);

                        }
                    }
                }
                else
                {
                    openFileDialog1.Filter = "bmp files|*.bmp"; //Что читаем
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string source = openFileDialog1.FileName;
                        saveFileDialog1.Filter = "bmp files |*.bmp";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string destination = saveFileDialog1.FileName; // Куда записываем


                            EncriptDecCbcPhoto(source, destination);

                        }
                    }
                }
            }
        }
        void EncriptDecCbcPhoto(string inFile, string outFile)
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            desProvider.BlockSize = 64;
            desProvider.KeySize = 64;

            desProvider.Key = System.Text.Encoding.ASCII.GetBytes(strKeyD);

            desProvider.IV = System.Text.Encoding.ASCII.GetBytes(strIVD);
            desProvider.Padding = PaddingMode.None;
            //  aesProvider.Mode = CipherMode.CBC;
            desProvider.Mode = CipherMode.CBC;
            //Read
            FileStream fileStream = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            fileStream.CopyTo(ms);
            //Store header in byte array (we will used this after encryption)
            var header = ms.ToArray().Take(54).ToArray();
            //Take rest from stream
            var imageArray = ms.ToArray().Skip(54).ToArray();
            //Create encryptor
            fileStream.Close();
            var enc = desProvider.CreateEncryptor();
            //Encrypt image
            var encimg = enc.TransformFinalBlock(imageArray, 0, imageArray.Length);
            //Combine header and encrypted image
            var image = Combine(header, encimg);
            //Write encrypted image to disk
            File.WriteAllBytes(outFile, image);
            desProvider.Clear();
        }
        public void DecryptAesCBCPhoto(string inFile, string outFile) //cbc
        {
            AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();
            aesProvider.BlockSize = 128;
            aesProvider.KeySize = 256;
            aesProvider.Padding = PaddingMode.None;
            aesProvider.Mode = CipherMode.CBC;
            aesProvider.Key = System.Text.Encoding.ASCII.GetBytes(strKey);
            aesProvider.IV = System.Text.Encoding.ASCII.GetBytes(strIV);


            //Read
            FileStream fileStream = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            fileStream.CopyTo(ms);
            //Store header in byte array (we will used this after encryption)
            var header = ms.ToArray().Take(54).ToArray();
            //Take rest from stream
            var imageArray = ms.ToArray().Skip(54).ToArray();
            //Create encryptor
            var enc = aesProvider.CreateDecryptor();
            //Encrypt image
            var encimg = enc.TransformFinalBlock(imageArray, 0, imageArray.Length);
            //Combine header and encrypted image
            var image = Combine(header, encimg);
            //Write encrypted image to disk
            File.WriteAllBytes(outFile, image);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (values == "AES")
            {
                if (valuesMethod == "ECB")
                {
                    openFileDialog1.Filter = "bmp files|*.bmp"; //Что читаем
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string source = openFileDialog1.FileName;
                        saveFileDialog1.Filter = "bmp files |*.bmp";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string destination = saveFileDialog1.FileName; // Куда записываем


                            DecryptAesECBPhoto(source, destination);

                        }
                    }
                }
                else
                {
                    openFileDialog1.Filter = "bmp files|*.bmp"; //Что читаем
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string source = openFileDialog1.FileName;
                        saveFileDialog1.Filter = "bmp files |*.bmp";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string destination = saveFileDialog1.FileName; // Куда записываем


                            DecryptAesCBCPhoto(source, destination);

                        }
                    }
                }
            }
            else
            {
                if (valuesMethod == "ECB")
                {
                    openFileDialog1.Filter = "bmp files|*.bmp"; //Что читаем
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string source = openFileDialog1.FileName;
                        saveFileDialog1.Filter = "bmp files |*.bmp";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string destination = saveFileDialog1.FileName; // Куда записываем


                            DecryptDesECB(source, destination);

                        }
                    }
                }
                else
                {
                    openFileDialog1.Filter = "bmp files|*.bmp"; //Что читаем
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string source = openFileDialog1.FileName;
                        saveFileDialog1.Filter = "bmp files |*.bmp";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string destination = saveFileDialog1.FileName; // Куда записываем


                            DecryptDesCBCPhoto(source, destination);

                        }
                    }
                }
            }
        }

        void EncryptAecCBCPhoto(string inFile, string outFile)//aesCBC
        {
            AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();

            aesProvider.BlockSize = 128;
            aesProvider.KeySize = 256;
            aesProvider.Key = System.Text.Encoding.ASCII.GetBytes(strKey);
            aesProvider.IV = System.Text.Encoding.ASCII.GetBytes(strIV);
            aesProvider.Padding = PaddingMode.None;
            //  aesProvider.Mode = CipherMode.CBC;
            aesProvider.Mode = CipherMode.CBC;
            //Read
            FileStream fileStream = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            fileStream.CopyTo(ms);
            //Store header in byte array (we will used this after encryption)
            var header = ms.ToArray().Take(54).ToArray();
            //Take rest from stream
            var imageArray = ms.ToArray().Skip(54).ToArray();
            //Create encryptor
            fileStream.Close();
            var enc = aesProvider.CreateEncryptor();
            //Encrypt image
            var encimg = enc.TransformFinalBlock(imageArray, 0, imageArray.Length);
            //Combine header and encrypted image
            var image = Combine(header, encimg);
            //Write encrypted image to disk
            File.WriteAllBytes(outFile, image);
            aesProvider.Clear();

        }

        public void DecryptDesCBCPhoto(string inFile, string outFile) //DES CBC
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

            desProvider.BlockSize = 64;
            desProvider.KeySize = 64;
            desProvider.Padding = PaddingMode.None;
            desProvider.Mode = CipherMode.CBC;
            desProvider.Key = System.Text.Encoding.ASCII.GetBytes(strKeyD);
            desProvider.IV = System.Text.Encoding.ASCII.GetBytes(strIVD);


            //Read
            FileStream fileStream = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            fileStream.CopyTo(ms);
            //Store header in byte array (we will used this after encryption)
            var header = ms.ToArray().Take(54).ToArray();
            //Take rest from stream
            var imageArray = ms.ToArray().Skip(54).ToArray();
            //Create encryptor
            var enc = desProvider.CreateDecryptor();
            //Encrypt image
            var encimg = enc.TransformFinalBlock(imageArray, 0, imageArray.Length);
            //Combine header and encrypted image
            var image = Combine(header, encimg);
            //Write encrypted image to disk
            File.WriteAllBytes(outFile, image);
        }
        void EncryptDECECB(string inFile, string outFile)//DECECB
        {

            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            desProvider.BlockSize = 64;
            desProvider.KeySize = 64;

            desProvider.Key = System.Text.Encoding.ASCII.GetBytes(strKeyD);

            desProvider.IV = System.Text.Encoding.ASCII.GetBytes(strIVD);
            desProvider.Padding = PaddingMode.None;
            //  aesProvider.Mode = CipherMode.CBC;
            desProvider.Mode = CipherMode.ECB;
            //Read
            FileStream fileStream = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            fileStream.CopyTo(ms);
            //Store header in byte array (we will used this after encryption)
            var header = ms.ToArray().Take(54).ToArray();
            //Take rest from stream
            var imageArray = ms.ToArray().Skip(54).ToArray();
            //Create encryptor
            fileStream.Close();
            var enc = desProvider.CreateEncryptor();
            //Encrypt image
            var encimg = enc.TransformFinalBlock(imageArray, 0, imageArray.Length);
            //Combine header and encrypted image
            var image = Combine(header, encimg);
            //Write encrypted image to disk
            File.WriteAllBytes(outFile, image);
            desProvider.Clear();

        }
        public void DecryptDesECB(string inFile, string outFile) //DES ECB
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

            desProvider.BlockSize = 64;
            desProvider.KeySize = 64;
            desProvider.Padding = PaddingMode.None;
            desProvider.Mode = CipherMode.ECB;
            desProvider.Key = System.Text.Encoding.ASCII.GetBytes(strKeyD);
            desProvider.IV = System.Text.Encoding.ASCII.GetBytes(strIVD);


            //Read
            FileStream fileStream = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            fileStream.CopyTo(ms);
            //Store header in byte array (we will used this after encryption)
            var header = ms.ToArray().Take(54).ToArray();
            //Take rest from stream
            var imageArray = ms.ToArray().Skip(54).ToArray();
            //Create encryptor
            var enc = desProvider.CreateDecryptor();
            //Encrypt image
            var encimg = enc.TransformFinalBlock(imageArray, 0, imageArray.Length);
            //Combine header and encrypted image
            var image = Combine(header, encimg);
            //Write encrypted image to disk
            File.WriteAllBytes(outFile, image);
        }
        void EncryptAecECB(string inFile, string outFile)//aesECB
        {
            AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();

            aesProvider.BlockSize = 128;
            aesProvider.KeySize = 256;
            aesProvider.Key = System.Text.Encoding.ASCII.GetBytes(strKey);
            aesProvider.IV = System.Text.Encoding.ASCII.GetBytes(strIV);
            aesProvider.Padding = PaddingMode.None;
            //  aesProvider.Mode = CipherMode.CBC;
            aesProvider.Mode = CipherMode.ECB;
            //Read
            FileStream fileStream = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            fileStream.CopyTo(ms);
            //Store header in byte array (we will used this after encryption)
            var header = ms.ToArray().Take(54).ToArray();
            //Take rest from stream
            var imageArray = ms.ToArray().Skip(54).ToArray();
            //Create encryptor
            fileStream.Close();
            var enc = aesProvider.CreateEncryptor();
            //Encrypt image
            var encimg = enc.TransformFinalBlock(imageArray, 0, imageArray.Length);
            //Combine header and encrypted image
            var image = Combine(header, encimg);
            //Write encrypted image to disk
            File.WriteAllBytes(outFile, image);
            aesProvider.Clear();

        }
        public void DecryptAesECBPhoto(string inFile, string outFile) //ECB
        {
            AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider();
            aesProvider.BlockSize = 128;
            aesProvider.KeySize = 256;
            aesProvider.Padding = PaddingMode.None;
            aesProvider.Mode = CipherMode.ECB;
            aesProvider.Key = System.Text.Encoding.ASCII.GetBytes(strKey);
            aesProvider.IV = System.Text.Encoding.ASCII.GetBytes(strIV);


            //Read
            FileStream fileStream = new FileStream(inFile, FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            fileStream.CopyTo(ms);
            //Store header in byte array (we will used this after encryption)
            var header = ms.ToArray().Take(54).ToArray();
            //Take rest from stream
            var imageArray = ms.ToArray().Skip(54).ToArray();
            //Create encryptor
            var enc = aesProvider.CreateDecryptor();
            //Encrypt image
            var encimg = enc.TransformFinalBlock(imageArray, 0, imageArray.Length);
            //Combine header and encrypted image
            var image = Combine(header, encimg);
            //Write encrypted image to disk
            File.WriteAllBytes(outFile, image);
        }
    }
}