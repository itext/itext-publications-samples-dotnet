using System;
using System.IO;
using System.Net;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace iText.Samples.Signatures.Chapter04
{
    public class C4_08_ServerClientSigning
    {
        public static readonly String DEST = "results/signatures/chapter04/";

        public static readonly String KEYSTORE = "../../../resources/encryption/ks";
        public static readonly String CERT = "../../../resources/encryption/bruno.crt";
        public static readonly String PRE = "http://demo.itextsupport.com/SigningApp/presign";
        public static readonly String POST = "http://demo.itextsupport.com/SigningApp/postsign";

        public static readonly String[] RESULT_FILES = new String[]
        {
            "hello_server2.pdf"
        };

        public static readonly char[] PASSWORD = "password".ToCharArray();

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            // Make a connection to a PreSign servlet to ask to create a document,
            // then calculate its hash and send it to us
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(PRE);
            request.Method = "POST";

            // Upload our self-signed certificate
            Stream os = request.GetRequestStream();
            byte[] data = new byte[256];
            int read;
            using (FileStream fis = new FileStream(CERT, FileMode.Open))
            {
                while ((read = fis.Read(data, 0, data.Length)) != 0)
                {
                    os.Write(data, 0, read);
                }
            }

            os.Flush();
            os.Close();

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            // Use cookies to maintain a session
            String cookies = response.Headers["Set-Cookie"];

            // Receive a hash that needs to be signed
            Stream istream = response.GetResponseStream();
            MemoryStream memoryStream = new MemoryStream();
            istream.CopyTo(memoryStream);
            istream.Close();
            byte[] hash = memoryStream.ToArray();

            // Load our private key from the key store
            Pkcs12Store store = new Pkcs12StoreBuilder().Build();
            store.Load(new FileStream(KEYSTORE, FileMode.Open, FileAccess.Read), PASSWORD);

            // Searching for private key
            String alias = null;
            foreach (string al in store.Aliases)
            {
                if (store.IsKeyEntry(al) && store.GetKey(al).Key.IsPrivate)
                {
                    alias = al;
                    break;
                }
            }

            AsymmetricKeyEntry pk = store.GetKey(alias);

            // Sign the hash received from the server
            ISigner sig = SignerUtilities.GetSigner("SHA256withRSA");
            sig.Init(true, pk.Key);
            sig.BlockUpdate(hash, 0, hash.Length);
            data = sig.GenerateSignature();

            // Make a connection to the PostSign Servlet
            request = (HttpWebRequest) WebRequest.Create(POST);
            request.Headers.Add(HttpRequestHeader.Cookie, cookies.Split(";".ToCharArray(), 2)[0]);
            request.Method = "POST";

            // Upload the signed bytes
            os = request.GetRequestStream();
            os.Write(data, 0, data.Length);
            os.Flush();
            os.Close();

            // Receive the signed document
            response = (HttpWebResponse) request.GetResponse();
            istream = response.GetResponseStream();
            using (FileStream fos = new FileStream(DEST + RESULT_FILES[0], FileMode.Create))
            {
                data = new byte[256];
                while ((read = istream.Read(data, 0, data.Length)) != 0)
                {
                    fos.Write(data, 0, read);
                }

                istream.Close();
            }
        }
    }
}