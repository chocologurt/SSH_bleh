using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ASPJ.Admin
{
    public partial class Change_Key : System.Web.UI.Page
    {
   //     AsymmetricCipherKeyPair keyPair;
        public string challenge { get; set; }
        public string privKey { get; set; }
        string userID = "Administrator";
        protected void Page_Load(object sender, EventArgs e)
        {
   /*         challenge = "HELLO";
            keyPair = getKeyPair();
            PrivateKeyInfo pkInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("-----BEGIN PRIVATE KEY-----");
            builder.AppendLine(Convert.ToBase64String(pkInfo.GetDerEncoded()));
            builder.AppendLine("-----END PRIVATE KEY-----");

    //        var encrypted = RsaEncryptWithPrivate(challenge, builder.ToString());
            privKey = "-----BEGIN PRIVATE KEY----- "+ Convert.ToBase64String(pkInfo.GetDerEncoded())+ " -----END PRIVATE KEY-----";
            SubjectPublicKeyInfo info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public);
            builder = new StringBuilder();
            builder.AppendLine("-----BEGIN PUBLIC KEY-----");
            builder.AppendLine(Convert.ToBase64String(info.GetDerEncoded()));
            builder.AppendLine("-----END PUBLIC KEY-----");
            pubKey = builder.ToString();


            
   //         string plain = RsaDecryptWithPublic(encrypted, pubKey);
  //          Debug.WriteLine(plain);
            Debug.WriteLine(privKey);
            Debug.WriteLine(pubKey);*/
        }

        public AsymmetricCipherKeyPair getKeyPair()
        {
            CryptoApiRandomGenerator randomGenerator = new CryptoApiRandomGenerator();
            SecureRandom secureRandom = new SecureRandom(randomGenerator);
            var keyGenerationParameters = new KeyGenerationParameters(secureRandom, 1024);

            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            return keyPairGenerator.GenerateKeyPair();
        }
        public OpenSSL.Crypto.RSA getKeyPair2()
        {
            OpenSSL.Crypto.RSA keypair = new OpenSSL.Crypto.RSA();
            
            keypair.GenerateKeys(1024, 65537, null, null);
            return keypair;
        }
        public void changeKey(object sender, EventArgs e)
        {
            //     AsymmetricCipherKeyPair keyPair = getKeyPair();
            //     PrivateKeyInfo pkInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
            //       StringBuilder builder=new StringBuilder();
            //       builder.AppendLine("-----BEGIN PRIVATE KEY-----");
            //       builder.AppendLine(Convert.ToBase64String(pkInfo.GetDerEncoded()));
            //       builder.AppendLine("-----END PRIVATE KEY-----");
            OpenSSL.Crypto.RSA keyPair = getKeyPair2();

            string privateKey = keyPair.PrivateKeyAsPEM;

            MemoryStream ms = new MemoryStream();
            TextWriter tw = new StreamWriter(ms);
            tw.Write(privateKey);
            tw.Flush();
            byte[] bytes = ms.ToArray();
            ms.Close();
            Debug.WriteLine("I Am HERE");
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment;    filename=key.txt");
            Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            //    Response.End();

            //        var encrypted = RsaEncryptWithPrivate(challenge, builder.ToString());
    /*        SubjectPublicKeyInfo info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public);
            builder = new StringBuilder();
            builder.AppendLine("-----BEGIN PUBLIC KEY-----");
            builder.AppendLine(Convert.ToBase64String(info.GetDerEncoded()));
            builder.AppendLine("-----END PUBLIC KEY-----");*/

            string pubKey = keyPair.PublicKeyAsPEM;
            KeyManager.changeKey(userID, pubKey);

            string eText = RsaEncryptWithPrivate("Hello", privateKey);

            Debug.WriteLine(RsaDecryptWithPublic(eText, pubKey));
        }
        public string RsaDecryptWithPublic(string base64Input, string publicKey)
        {
            var bytesToDecrypt = Convert.FromBase64String(base64Input);

            var decryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(publicKey))
            {
                var keyParameter = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();

                decryptEngine.Init(false, keyParameter);
            }

            var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));
            return decrypted;
        }
        public string RsaEncryptWithPrivate(string clearText, string privateKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());

            using (var txtreader = new StringReader(privateKey))
            {
                var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();

                encryptEngine.Init(true, keyPair.Private);
            }

            var encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
            return encrypted;
        }
    }
}