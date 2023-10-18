﻿using System;
using System.Collections.Generic;
using System.IO;
using iText.Bouncycastle.Cert;
using iText.Bouncycastle.X509;
using iText.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Forms.Form.Element;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace iText.Samples.Sandbox.Signatures
{
    public class SignatureExample
    {
        public static readonly String DEST = "results/sandbox/signatures/signExample.pdf";

        public static readonly String SRC = "../../../resources/pdfs/signExample.pdf";
        public static readonly String CERT_PATH = "../../../resources/cert/signCertRsa01.p12";
        public static readonly String IMAGE_PATH = "../../../resources/img/sign.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SignatureExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            ElectronicSignatureInfoDTO signatureInfo = new ElectronicSignatureInfoDTO();
            signatureInfo.Bottom = 25;
            signatureInfo.Left = 25;
            signatureInfo.PageNumber = 1;

            SignDocumentSignature(dest, signatureInfo);
        }

        protected void SignDocumentSignature(string filePath, ElectronicSignatureInfoDTO signatureInfo)
        {
            PdfSigner pdfSigner = new PdfSigner(new PdfReader(SRC), new FileStream(filePath, FileMode.Create),
                new StampingProperties());
            pdfSigner.SetCertificationLevel(PdfSigner.CERTIFIED_NO_CHANGES_ALLOWED);

            // Set the name indicating the field to be signed.
            // The field can already be present in the document but shall not be signed
            pdfSigner.SetFieldName("signature");

            ImageData clientSignatureImage = ImageDataFactory.Create(IMAGE_PATH);

            // If you create new signature field (or use SetFieldName(System.String) with
            // the name that doesn't exist in the document or don't specify it at all) then
            // the signature is invisible by default.
            SignatureFieldAppearance appearance = new SignatureFieldAppearance(pdfSigner.GetFieldName())
                    .SetContent(clientSignatureImage);
            pdfSigner.SetPageNumber(signatureInfo.PageNumber)
                    .SetPageRect(new Rectangle(signatureInfo.Left, signatureInfo.Bottom,
                25, 25))
                    .SetSignatureAppearance(appearance);

            char[] password = "testpass".ToCharArray();
            IExternalSignature pks = GetPrivateKeySignature(CERT_PATH, password);
            IX509Certificate[] chain = GetCertificateChain(CERT_PATH, password);
            OCSPVerifier ocspVerifier = new OCSPVerifier(null, null);
            OcspClientBouncyCastle ocspClient = new OcspClientBouncyCastle(ocspVerifier);
            List<ICrlClient> crlClients = new List<ICrlClient>(new[] {new CrlClientOnline()});

            
            // Sign the document using the detached mode, CMS or CAdES equivalent.
            // This method closes the underlying pdf document, so the instance
            // of PdfSigner cannot be used after this method call
            pdfSigner.SignDetached(pks, chain, crlClients, ocspClient, null, 0,
                PdfSigner.CryptoStandard.CMS);
        }

        /// Method reads pkcs12 file's first private key and returns a
        /// <see cref="PrivateKeySignature"/> instance, which uses SHA-512 hash algorithm. 
        private PrivateKeySignature GetPrivateKeySignature(String certificatePath, char[] password)
        {
            String alias = null;
            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(certificatePath, FileMode.Open, FileAccess.Read), password);

            foreach (var a in pk12.Aliases)
            {
                alias = ((String) a);
                if (pk12.IsKeyEntry(alias))
                {
                    break;
                }
            }

            IPrivateKey pk = new PrivateKeyBC(pk12.GetKey(alias).Key);
            return new PrivateKeySignature(pk, DigestAlgorithms.SHA512);
        }

        /// Method reads first public certificate chain
        private IX509Certificate[] GetCertificateChain(String certificatePath, char[] password)
        {
            IX509Certificate[] chain;
            String alias = null;
            Pkcs12Store pk12 = new Pkcs12StoreBuilder().Build();
            pk12.Load(new FileStream(certificatePath, FileMode.Open, FileAccess.Read), password);

            foreach (var a in pk12.Aliases)
            {
                alias = ((String) a);
                if (pk12.IsKeyEntry(alias))
                {
                    break;
                }
            }

            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            chain = new IX509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = new X509CertificateBC(ce[k].Certificate);
            }

            return chain;
        }

        protected class ElectronicSignatureInfoDTO
        {
            public int PageNumber { get; set; }

            public float Left { get; set; }

            public float Bottom { get; set; }
        }
    }
}