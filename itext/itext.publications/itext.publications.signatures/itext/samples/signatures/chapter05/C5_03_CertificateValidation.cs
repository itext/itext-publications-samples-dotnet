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
using Common.Logging;
using Common.Logging.Simple;
using iText.Kernel.Pdf;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.X509;
using iText.Signatures;
using Org.BouncyCastle.Security.Certificates;

namespace iText.Samples.Signatures.Chapter05
{
    public class C5_03_CertificateValidation
    {
        public static readonly string DEST = "signatures/chapter05/";

        public static readonly string ROOT = "../../../resources/encryption/rootRsa.cer";

        public static readonly string EXAMPLE = "../../../resources/pdfs/signedPAdES-LT.pdf";

        public const String EXPECTED_OUTPUT = "../../../resources/pdfs/signedPAdES-LT.pdf\n"
                                              + "===== Signature1 =====\n"
                                              + "Signature covers whole document: False\n"
                                              + "Document revision: 1 of 2\n"
                                              + "Integrity check OK? True\n"
                                              + "Certificates verified against the KeyStore\n"
                                              + "=== Certificate 0 ===\n"
                                              + "Issuer: C=BY,L=Minsk,O=iText,OU=test,CN=iTextTestRoot\n"
                                              + "Subject: C=BY,L=Minsk,O=iText,OU=test,CN=iTextTestRsaCert01\n"
                                              + "Valid from: 2017-04-07\n"
                                              + "Valid to: 2117-04-07\n"
                                              + "The certificate was valid at the time of signing.\n"
                                              + "The certificate is still valid.\n"
                                              + "=== Certificate 1 ===\n"
                                              + "Issuer: C=BY,L=Minsk,O=iText,OU=test,CN=iTextTestRoot\n"
                                              + "Subject: C=BY,L=Minsk,O=iText,OU=test,CN=iTextTestRoot\n"
                                              + "Valid from: 2017-04-07\n"
                                              + "Valid to: 2117-04-07\n"
                                              + "The certificate was valid at the time of signing.\n"
                                              + "The certificate is still valid.\n"
                                              + "=== Checking validity of the document at the time of signing ===\n"
                                              + "iText.Signatures.OCSPVerifier: Valid OCSPs found: 0\n"
                                              + "iText.Signatures.CRLVerifier: Valid CRLs found: 0\n"
                                              + "The signing certificate couldn't be verified\n"
                                              + "=== Checking validity of the document today ===\n"
                                              + "iText.Signatures.OCSPVerifier: Valid OCSPs found: 0\n"
                                              + "iText.Signatures.CRLVerifier: Valid CRLs found: 0\n"
                                              + "The signing certificate couldn't be verified"
                                              +"\n";

        public static TextWriter OUT_STREAM = Console.Out;
        private static ILoggerFactoryAdapter defaultLogAdapter;
        private List<X509Certificate> ks;

        public void VerifySignatures(String path)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
            SignatureUtil signUtil = new SignatureUtil(pdfDoc);
            IList<String> names = signUtil.GetSignatureNames();

            OUT_STREAM.WriteLine(path);
            foreach (String name in names)
            {
                OUT_STREAM.WriteLine("===== " + name + " =====");
                VerifySignature(signUtil, name);
            }
        }

        public PdfPKCS7 VerifySignature(SignatureUtil signUtil, String name)
        {
            PdfPKCS7 pkcs7 = GetSignatureData(signUtil, name);
            X509Certificate[] certs = pkcs7.GetSignCertificateChain();
            
            // Timestamp is a secure source of signature creation time,
            // because it's based on Time Stamping Authority service.
            DateTime cal = pkcs7.GetTimeStampDate();
            
            // The value, which is returned if there is no timestamp
            DateTime noTimestampValue = DateTime.MaxValue;
            
            // If there is no timestamp, use the current date
            if (cal.Equals(noTimestampValue)) {
                cal = new DateTime();
            }

            // Check if the certificate chain, presented in the PDF, can be verified against
            // the created key store.
            IList<VerificationException> errors = CertificateVerification.VerifyCertificates(certs, ks, cal);
            if (errors.Count == 0)
            {
                OUT_STREAM.WriteLine("Certificates verified against the KeyStore");
            }
            else
            {
                OUT_STREAM.WriteLine(errors);
            }

            // Find out if certificates were valid on the signing date, and if they are still valid today
            for (int i = 0; i < certs.Length; i++)
            {
                X509Certificate cert = (X509Certificate) certs[i];
                OUT_STREAM.WriteLine("=== Certificate " + i + " ===");
                ShowCertificateInfo(cert, cal.ToUniversalTime());
            }

            // Take the signing certificate
            X509Certificate signCert = (X509Certificate) certs[0];

            // Take the certificate of the issuer of that certificate (or null if it was self-signed).
            X509Certificate issuerCert = (certs.Length > 1 ? (X509Certificate) certs[1] : null);

            OUT_STREAM.WriteLine("=== Checking validity of the document at the time of signing ===");
            CheckRevocation(pkcs7, signCert, issuerCert, cal.ToUniversalTime());

            OUT_STREAM.WriteLine("=== Checking validity of the document today ===");
            CheckRevocation(pkcs7, signCert, issuerCert, new DateTime());

            return pkcs7;
        }

        public PdfPKCS7 GetSignatureData(SignatureUtil signUtil, String name)
        {
            PdfPKCS7 pkcs7 = signUtil.ReadSignatureData(name);

            OUT_STREAM.WriteLine("Signature covers whole document: " + signUtil.SignatureCoversWholeDocument(name));
            OUT_STREAM.WriteLine("Document revision: " + signUtil.GetRevision(name) + " of "
                                 + signUtil.GetTotalRevisions());
            OUT_STREAM.WriteLine("Integrity check OK? " + pkcs7.VerifySignatureIntegrityAndAuthenticity());
            return pkcs7;
        }

        public void ShowCertificateInfo(X509Certificate cert, DateTime signDate)
        {
            OUT_STREAM.WriteLine("Issuer: " + cert.IssuerDN);
            OUT_STREAM.WriteLine("Subject: " + cert.SubjectDN);
            OUT_STREAM.WriteLine("Valid from: " + (cert.NotBefore.ToUniversalTime().ToString("yyyy-MM-dd")));
            OUT_STREAM.WriteLine("Valid to: " + cert.NotAfter.ToUniversalTime().ToString("yyyy-MM-dd"));

            // Check if a certificate was valid on the signing date
            try
            {
                cert.CheckValidity(signDate);
                OUT_STREAM.WriteLine("The certificate was valid at the time of signing.");
            }
            catch (CertificateExpiredException)
            {
                OUT_STREAM.WriteLine("The certificate was expired at the time of signing.");
            }
            catch (CertificateNotYetValidException)
            {
                OUT_STREAM.WriteLine("The certificate wasn't valid yet at the time of signing.");
            }

            // Check if a certificate is still valid now
            try
            {
                cert.CheckValidity();
                OUT_STREAM.WriteLine("The certificate is still valid.");
            }
            catch (CertificateExpiredException)
            {
                OUT_STREAM.WriteLine("The certificate has expired.");
            }
            catch (CertificateNotYetValidException)
            {
                OUT_STREAM.WriteLine("The certificate isn't valid yet.");
            }
        }

        private static void CheckRevocation(PdfPKCS7 pkcs7, X509Certificate signCert, X509Certificate issuerCert,
            DateTime date)
        {
            IList<BasicOcspResp> ocsps = new List<BasicOcspResp>();
            if (pkcs7.GetOcsp() != null)
            {
                ocsps.Add(pkcs7.GetOcsp());
            }

            // Check if the OCSP responses in the list were valid for the certificate on a specific date.
            OCSPVerifier ocspVerifier = new OCSPVerifier(null, ocsps);
            IList<VerificationOK> verification = ocspVerifier.Verify(signCert, issuerCert, date);

            // If that list is empty, we can’t verify using OCSP, and we need to look for CRLs.
            if (verification.Count == 0)
            {
                IList<X509Crl> crls = new List<X509Crl>();
                if (pkcs7.GetCRLs() != null)
                {
                    foreach (X509Crl crl in pkcs7.GetCRLs())
                    {
                        crls.Add((X509Crl) crl);
                    }
                }

                // Check if the CRLs in the list were valid on a specific date.
                CRLVerifier crlVerifier = new CRLVerifier(null, crls);
                IList<VerificationOK> verificationOks = crlVerifier.Verify(signCert, issuerCert, date);
                foreach (VerificationOK verOK in verificationOks)
                {
                    verification.Add(verOK);
                }
            }

            if (verification.Count == 0)
            {
                OUT_STREAM.WriteLine("The signing certificate couldn't be verified");
            }
            else
            {
                foreach (VerificationOK v in verification)
                {
                    OUT_STREAM.WriteLine(v);
                }
            }
        }

        public static void Main(String[] args)
        {
            C5_03_CertificateValidation app = new C5_03_CertificateValidation();

            // Set up logger to show log messages from OCSPVerifier and CLRVerifier classes 
            SetUpLogger();

            // Create your own root certificate store and add certificates
            List<X509Certificate> ks = new List<X509Certificate>();
            var parser = new X509CertificateParser();
            X509Certificate rootCert;
            using (FileStream stream = new FileStream(ROOT, FileMode.Open, FileAccess.Read))
            {
                rootCert = parser.ReadCertificate(stream);
            }

            ks.Add(rootCert);
            app.SetKeyStore(ks);

            app.VerifySignatures(EXAMPLE);

            // Reset logger to the default value
            ResetLogger();
        }

        private void SetKeyStore(List<X509Certificate> ks)
        {
            this.ks = ks;
        }

        private static void SetUpLogger()
        {
            defaultLogAdapter = LogManager.Adapter;
            LogManager.Adapter = new CustomMemoryAdapter(OUT_STREAM);
        }

        private static void ResetLogger()
        {
            LogManager.Adapter = defaultLogAdapter;
        }

        // Custom log adapter to write log messages to the specific text writer
        private class CustomMemoryAdapter : CapturingLoggerFactoryAdapter
        {
            private TextWriter writer;

            public CustomMemoryAdapter(TextWriter writer)
            {
                this.writer = writer;
            }

            public override void AddEvent(CapturingLoggerEvent le)
            {
                writer.WriteLine(le.Source.Name + ": " + le.RenderedMessage);
                if (le.Level >= LogLevel.Warn)
                {
                    base.AddEvent(le);
                }
            }
        }
    }
}