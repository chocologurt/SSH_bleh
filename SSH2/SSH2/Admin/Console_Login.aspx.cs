using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPJ.Admin
{
    public partial class Console_Login : System.Web.UI.Page
    {
        string userID = "Administrator";
        public string challenge { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            challenge = "HELLO";
        }

        public void validate(object sender, EventArgs e)
        {
            string privateKey = privKey.Value;
            
            string encrypted = RsaEncryptWithPrivate(challenge, privateKey);
            if (encrypted != null)
            {
                try
                {
                    Debug.WriteLine(encrypted);
                    string pubKey = KeyManager.retrievePublicKey(userID);
                    var bytesToDecrypt = Convert.FromBase64String(encrypted);
                    var decryptEngine = new Pkcs1Encoding(new RsaEngine());
                    var pKey = new StringReader(pubKey);
                    Debug.WriteLine(pubKey);
                    var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(pKey);
                    var KeyParameter = (Org.BouncyCastle.Crypto.AsymmetricKeyParameter)pemReader.ReadObject();

                    Debug.WriteLine(bytesToDecrypt.ToString());

                    decryptEngine.Init(false, KeyParameter);
                    Debug.WriteLine(bytesToDecrypt.Length);
                    var decrypted = Encoding.UTF8.GetString(decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length));

                    Debug.WriteLine(decrypted);

                    if (decrypted.Equals(challenge))
                    {
                        Response.Redirect("/Admin/Console", false);
                        FormsAuthentication.RedirectFromLoginPage
                            ("Administrator", false);
                        KeyManager.setLastLogin(userID);
                        KeyManager.setSessionCookie(Context);
                        Logger.authLogging(DateTime.Now+","+userID+",login,success");
                    }
                }
                catch (Exception ex)
                {
                    error.Text = "Invalid Key";
                    Logger.authLogging(DateTime.Now + "," + userID + ",login,failed");
                }
            }
            else
            {
                error.Text = "Wrong Format";
                Logger.authLogging(DateTime.Now + "," + userID + ",login,failed");
            }
        }
        public string RsaEncryptWithPrivate(string clearText, string privateKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(clearText);

            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            string encrypted = null;
            using (var txtreader = new StringReader(privateKey))
            {
                var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();
                if (keyPair != null)
                {
                    encryptEngine.Init(true, keyPair.Private);
                    encrypted = Convert.ToBase64String(encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length));
                }
            }

            
            return encrypted;
        }
    }
}