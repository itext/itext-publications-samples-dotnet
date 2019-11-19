/*

This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV

*/
/*
* This class is part of the white paper entitled
* "Digital Signatures for PDF documents"
* written by Bruno Lowagie
*
* For more info, go to: http://itextpdf.com/learn
*/

using System;
using System.Collections.Generic;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter03
{
    public class C3_05_SignWithCRLOffline
    {
        public static readonly string DEST = "results/signatures/chapter03/";

        public static readonly string CRLURL = "../../resources/encryption/revoke.crl";
        public static readonly string SRC = "../../resources/pdfs/hello.pdf";

        public static readonly String[] RESULT_FILES =
        {
            "hello_cacert_crl_offline.pdf"
        };

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            Properties properties = new Properties();

            // Specify the correct path to the certificate
            properties.Load(new FileStream("c:/home/blowagie/key.properties", FileMode.Open, FileAccess.Read));
            String path = properties.GetProperty("PRIVATE");
            char[] pass = properties.GetProperty("PASSWORD").ToCharArray();

            Pkcs12Store pk12 = new Pkcs12Store(new FileStream(path, FileMode.Open, FileAccess.Read), pass);
            string alias = null;
            foreach (var a in pk12.Aliases)
            {
                alias = ((string) a);
                if (pk12.IsKeyEntry(alias))
                    break;
            }

            ICipherParameters pk = pk12.GetKey(alias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            X509Certificate[] chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = ce[k].Certificate;
            }

            FileStream fileStream = new FileStream(CRLURL, FileMode.Open, FileAccess.Read);
            MemoryStream baos = new MemoryStream();
            byte[] buf = new byte[1024];
            while (fileStream.Read(buf, 0, buf.Length) != 0)
            {
                baos.Write(buf, 0, buf.Length);
            }

            /* Create a CrlClientOffline instance with the read CRL file's data.
             * Given CRL file is specific to the CAcert provider and was downloaded long time ago.
             * Make sure that you have the CRL specific for your certificate and CRL is up to date
             * (by checking NextUpdate properties as seen below).
             */
            ICrlClient crlClient = new CrlClientOffline(baos.ToArray());
            X509Crl crl = new X509CrlParser().ReadCrl(new FileStream(CRLURL, FileMode.Open, FileAccess.Read));
            Console.WriteLine("CRL valid until: " + crl.NextUpdate);
            Console.WriteLine("Certificate revoked: " + crl.IsRevoked(chain[0]));
            IList<ICrlClient> crlList = new List<ICrlClient>();
            crlList.Add(crlClient);

            new C3_05_SignWithCRLOffline().Sign(SRC, DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS, "Test", "Ghent",
                crlList, null, null, 0);
        }

        public void Sign(String src, String dest, X509Certificate[] chain, ICipherParameters pk,
            String digestAlgorithm, PdfSigner.CryptoStandard subfilter, String reason, String location,
            ICollection<ICrlClient> crlList, IOcspClient ocspClient, ITSAClient tsaClient, int estimatedSize)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            Rectangle rect = new Rectangle(36, 648, 200, 100);
            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance
                .SetReason(reason)
                .SetLocation(location)

                // Specify if the appearance before field is signed will be used
                // as a background for the signed field. The "false" value is the default value.
                .SetReuseAppearance(false)
                .SetPageRect(rect)
                .SetPageNumber(1);
            signer.SetFieldName("sig");

            IExternalSignature pks = new PrivateKeySignature(pk, digestAlgorithm);

            // Sign the document using the detached mode, CMS or CAdES equivalent.
            // Pass the created CRL to the signing method.
            signer.SignDetached(pks, chain, crlList, ocspClient, tsaClient, estimatedSize, subfilter);
        }
    }
}