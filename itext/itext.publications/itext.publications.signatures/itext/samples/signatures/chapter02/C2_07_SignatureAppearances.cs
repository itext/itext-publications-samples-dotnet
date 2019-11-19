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
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;

namespace iText.Samples.Signatures.Chapter02
{
    public class C2_07_SignatureAppearances
    {
        public static readonly string DEST = "results/signatures/chapter02/";

        public static readonly string KEYSTORE = "../../resources/encryption/ks";
        public static readonly string SRC = "../../resources/pdfs/hello_to_sign.pdf";
        public static readonly string IMG = "../../resources/img/1t3xt.gif";

        public static readonly char[] PASSWORD = "password".ToCharArray();

        public static readonly String[] RESULT_FILES =
        {
            "signature_appearance_1.pdf",
            "signature_appearance_2.pdf",
            "signature_appearance_3.pdf",
            "signature_appearance_4.pdf"
        };

        public void Sign(String src, String name, String dest, X509Certificate[] chain,
            ICipherParameters pk, String digestAlgorithm, PdfSigner.CryptoStandard subfilter,
            String reason, String location, PdfSignatureAppearance.RenderingMode renderingMode, ImageData image)
        {
            PdfReader reader = new PdfReader(src);
            PdfSigner signer = new PdfSigner(reader, new FileStream(dest, FileMode.Create), new StampingProperties());

            // Create the signature appearance
            PdfSignatureAppearance appearance = signer.GetSignatureAppearance();
            appearance.SetReason(reason);
            appearance.SetLocation(location);

            // This name corresponds to the name of the field that already exists in the document.
            signer.SetFieldName(name);

            appearance.SetLayer2Text("Signed on " + DateTime.Now);

            // Set the rendering mode for this signature.
            appearance.SetRenderingMode(renderingMode);

            // Set the Image object to render when the rendering mode is set to RenderingMode.GRAPHIC
            // or RenderingMode.GRAPHIC_AND_DESCRIPTION.
            appearance.SetSignatureGraphic(image);

            PrivateKeySignature pks = new PrivateKeySignature(pk, digestAlgorithm);

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

            ImageData image = ImageDataFactory.Create(IMG);

            C2_07_SignatureAppearances app = new C2_07_SignatureAppearances();
            String signatureName = "Signature1";
            String location = "Ghent";
            app.Sign(SRC, signatureName, DEST + RESULT_FILES[0], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Appearance 1", location, PdfSignatureAppearance.RenderingMode.DESCRIPTION, null);

            app.Sign(SRC, signatureName, DEST + RESULT_FILES[1], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Appearance 2", location, PdfSignatureAppearance.RenderingMode.NAME_AND_DESCRIPTION, null);

            app.Sign(SRC, signatureName, DEST + RESULT_FILES[2], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Appearance 3", location, PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION, image);

            app.Sign(SRC, signatureName, DEST + RESULT_FILES[3], chain, pk,
                DigestAlgorithms.SHA256, PdfSigner.CryptoStandard.CMS,
                "Appearance 4", location, PdfSignatureAppearance.RenderingMode.GRAPHIC, image);
        }
    }
}