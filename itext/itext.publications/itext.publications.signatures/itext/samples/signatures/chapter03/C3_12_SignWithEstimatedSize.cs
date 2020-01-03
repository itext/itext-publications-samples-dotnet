/*

This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV

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
    public class C3_12_SignWithEstimatedSize
    {
        public static readonly string DEST = "results/signatures/chapter03/";

        public static readonly string SRC = "../../resources/pdfs/hello.pdf";

        public static readonly String[] RESULT_FILES =
        {
            "hello_estimated.pdf"
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
            String tsaUrl = properties.GetProperty("TSAURL");
            String tsaUser = properties.GetProperty("TSAUSERNAME");
            String tsaPass = properties.GetProperty("TSAPASSWORD");

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

            IOcspClient ocspClient = new OcspClientBouncyCastle(null);
            ITSAClient tsaClient = new TSAClientBouncyCastle(tsaUrl, tsaUser, tsaPass);
            C3_12_SignWithEstimatedSize app = new C3_12_SignWithEstimatedSize();

            bool succeeded = false;
            int estimatedSize = 1000;
            while (!succeeded)
            {
                try
                {
                    Console.WriteLine("Attempt: " + estimatedSize + " bytes");

                    app.Sign(SRC, DEST, chain, pk, DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                        "Test", "Ghent", null, ocspClient, tsaClient, estimatedSize);

                    succeeded = true;
                    Console.WriteLine("Succeeded!");
                }
                catch (IOException ioe)
                {
                    Console.WriteLine("Not succeeded: " + ioe.Message);
                    estimatedSize += 50;
                }
            }
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
            signer.SignDetached(pks, chain, crlList, ocspClient, tsaClient, estimatedSize, subfilter);
        }
    }
}