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
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_03_SignEmptyField
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string KEYSTORE = "../../resources/encryption/ks";
        public static readonly string SRC = "../../resources/pdfs/hello_to_sign.pdf";

        public static readonly char[] PASSWORD = "password".ToCharArray();

        public static readonly String[] RESULT_FILES =
        {
            "field_signed1.pdf",
            "field_signed2.pdf",
            "field_signed3.pdf",
            "field_signed4.pdf"
        };

        public void Sign(String src, String name, String dest, X509Certificate[] chain,
            ICipherParameters pk, String digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            String reason, String location)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            signer.GetSignatureAppearance()
                .SetReason(reason)
                .SetLocation(location)

                // Specify if the appearance before field is signed will be used
                // as a background for the signed field. The "false" value is the default value.
                .SetReuseAppearance(false);

            // This name corresponds to the name of the field that already exists in the document.
            signer.SetFieldName(name);

            IExternalSignature pks = new PrivateKeySignature(pk, digestAlgorithm);

            // Sign the document using the detached mode, CMS or CAdES equivalent.
            signer.SignDetached(pks, chain, null, null, null, 0, subfilter);
        }

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            Pkcs12Store pk12 = new Pkcs12Store(new FileStream(KEYSTORE, FileMode.Open, FileAccess.Read), PASSWORD);
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

            C2_03_SignEmptyField app = new C2_03_SignEmptyField();
            app.Sign(SRC, "Signature1", DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Test 1", "Ghent");
            app.Sign(SRC, "Signature1", DEST + RESULT_FILES[1], chain, pk,
                DigestAlgorithms.SHA512, PdfSigner.CryptoStandard.CMS,
                "Test 2", "Ghent");
            app.Sign(SRC, "Signature1", DEST + RESULT_FILES[2], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CADES,
                "Test 3", "Ghent");
            app.Sign(SRC, "Signature1", DEST + RESULT_FILES[3], chain, pk,
                DigestAlgorithms.RIPEMD160, PdfSigner.CryptoStandard.CADES,
                "Test 4", "Ghent");
        }
    }
}