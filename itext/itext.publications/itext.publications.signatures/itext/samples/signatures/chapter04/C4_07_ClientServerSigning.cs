using System;
using System.IO;
using System.Net;
using iText.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel;
using iText.Kernel.Exceptions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.X509;

namespace iText.Samples.Signatures.Chapter04
{
    public class C4_07_ClientServerSigning
    {
        public static readonly string DEST = "results/signatures/chapter04/";

        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";
        public static readonly string CERT = "https://demo.itextsupport.com/SigningApp/itextpdf.cer";

        public static readonly String[] RESULT_FILES =
        {
            "hello_server.pdf"
        };

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            Uri certUrl = new Uri(CERT);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(certUrl);
            request.Method = WebRequestMethods.Http.Get;
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            X509CertificateParser parser = new X509CertificateParser();
            X509Certificate[] chain = new X509Certificate[1];
            using (Stream stream = response.GetResponseStream())
            {
                chain[0] = parser.ReadCertificate(stream);
            }

            new C4_07_ClientServerSigning().Sign(SRC, DEST + RESULT_FILES[0], chain, PdfSigner.CryptoStandard.CMS,
                "Test", "Ghent");
        }

        public void Sign(String src, String dest, X509Certificate[] chain, PdfSigner.CryptoStandard subfilter,
            String reason, String location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            Rectangle rect = new Rectangle(36, 648, 200, 100);
            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance
                .SetReason(reason)
                .SetLocation(location)
                .SetPageRect(rect)
                .SetPageNumber(1);
            signer.SetFieldName("sig");

            IExternalSignature pks = new ServerSignature();

            IX509Certificate[] certificateWrappers = new IX509Certificate[chain.Length];
            for (int i = 0; i < certificateWrappers.Length; ++i) {
                certificateWrappers[i] = new X509CertificateBC(chain[i]);
            }
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, certificateWrappers, null, null, null, 0, subfilter);
        }

        public class ServerSignature : IExternalSignature
        {
            public static readonly String SIGN = "http://demo.itextsupport.com/SigningApp/signbytes";

            public String GetDigestAlgorithmName()
            {
                return DigestAlgorithms.SHA256;
            }

            public String GetSignatureAlgorithmName()
            {
                return "RSA";
            }

            public ISignatureMechanismParams GetSignatureMechanismParameters()
            {
                return null;
            }

            public byte[] Sign(byte[] message)
            {
                try
                {
                    Uri url = new Uri(SIGN);
                    var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
                    httpWebRequest.Method = "POST";

                    using (Stream stream = httpWebRequest.GetRequestStream())
                    {
                        stream.Write(message, 0, message.Length);
                    }

                    var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        Stream stream = httpResponse.GetResponseStream();
                        stream.CopyTo(memoryStream);
                        stream.Close();
                        
                        return memoryStream.ToArray();
                    }
                }
                catch (IOException e)
                {
                    throw new PdfException(e);
                }
            }
        }
    }
}