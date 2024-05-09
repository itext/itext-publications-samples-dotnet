using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Contexts;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Security;
using iText.Commons.Utils;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Signatures.Clients;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Signatures;
using iText.Signatures.Validation.V1;
using iText.Signatures.Validation.V1.Context;
using iText.Signatures.Validation.V1.Report;

namespace iText.Samples.Sandbox.Signatures.Validation
{
    /// <summary>Basic example of the existing signature validation.</summary>
    public class ValidateSignatureExample
    {
        public static readonly String SRC = "../../../resources/pdfs/validDocWithTimestamp.pdf";
        public static readonly String DEST = "results/sandbox/signatures/validation/validateSignatureExample.txt";

        private const String SIGNATURE_VERIFICATION = "Signature verification check.";
        private const String CANNOT_VERIFY_SIGNATURE = "Signature {0} cannot be mathematically verified.";
        private const String DOCUMENT_IS_NOT_COVERED = "Signature {0} doesn't cover entire document.";
        private const String TIMESTAMP_VERIFICATION = "Timestamp verification check.";
        private const String CANNOT_VERIFY_TIMESTAMP = "Signature timestamp attribute cannot be verified";

        private IX509Certificate[] certificateChain;
        private IPrivateKey privateKey;

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ValidateSignatureExample().ValidateExistingSignature(SRC, DEST);
        }

        /// <summary>Basic example of the existing signature validation.</summary>
        /// <param name="src">path to the signed document</param>
        /// <param name="dest">path to the parsed validation report</param>
        public virtual void ValidateExistingSignature(String src, String dest)
        {
            // Set up the validator.
            SignatureValidationProperties properties = GetSignatureValidationProperties();
            IssuingCertificateRetriever certificateRetriever = new IssuingCertificateRetriever();
            IX509Certificate rootCert = (IX509Certificate)GetCertificateChain()[2];
            certificateRetriever.SetTrustedCertificates(JavaCollectionsUtil.SingletonList(rootCert));
            ValidatorChainBuilder validatorChainBuilder = new ValidatorChainBuilder().WithIssuingCertificateRetriever(
                certificateRetriever).WithSignatureValidationProperties(properties);
            ValidationReport report = new ValidationReport();
            using (PdfDocument document = new PdfDocument(new PdfReader(src)))
            {
                CertificateChainValidator validator = validatorChainBuilder.BuildCertificateChainValidator();
                validator.AddOcspClient(GetOcspClient());
                // Validate the signature.
                SignatureUtil signatureUtil = new SignatureUtil(document);
                IList<String> signatures = signatureUtil.GetSignatureNames();
                String latestSignatureName = signatures[signatures.Count - 1];
                PdfPKCS7 pkcs7 = signatureUtil.ReadSignatureData(latestSignatureName);
                ValidateSignature(report, signatureUtil, latestSignatureName, pkcs7);
                DateTime signingDate = DateTimeUtil.GetCurrentUtcTime();
                if (pkcs7.GetTimeStampTokenInfo() != null)
                {
                    // Validate timestamp.
                    ValidateTimestamp(certificateRetriever, report, validator, pkcs7);
                    signingDate = pkcs7.GetTimeStampDate().ToUniversalTime();
                }

                IX509Certificate[] certificates = pkcs7.GetCertificates();
                certificateRetriever.AddKnownCertificates(JavaUtil.ArraysAsList(certificates));
                IX509Certificate signingCertificate = pkcs7.GetSigningCertificate();
                // Set up validation context related to CertificateChainValidator for the historical validation.
                ValidationContext baseContext = new ValidationContext(ValidatorContext.CERTIFICATE_CHAIN_VALIDATOR,
                    CertificateSource
                        .SIGNER_CERT, TimeBasedContext.HISTORICAL);
                // Validate the signing chain. ValidationReport will contain all the validation report messages.
                report = validator.Validate(report, baseContext, signingCertificate, signingDate);
            }

            NUnit.Framework.Assert.AreEqual(report.GetValidationResult(), ValidationReport.ValidationResult.VALID);

            // Write validation report to the file.
            using (FileStream fos = new FileStream(dest, FileMode.Create))
            {
                byte[] reportBytes = GetBytes(report.ToString());
                fos.Write(reportBytes, 0, reportBytes.Length);
            }
        }

        /// <summary>
        /// Use
        /// <see cref="SignatureValidationProperties"/>
        /// to configure different properties for the validators, e.g. whether to
        /// continue validation after failure, whether to allow online revocation data fetching, set freshness value for the
        /// revocation data.
        /// </summary>
        /// <remarks>
        /// Use
        /// <see cref="SignatureValidationProperties"/>
        /// to configure different properties for the validators, e.g. whether to
        /// continue validation after failure, whether to allow online revocation data fetching, set freshness value for the
        /// revocation data. It is possible to specify properties for any
        /// <see cref="Context.ValidatorContexts"/>
        /// ,
        /// <see cref="Context.CertificateSources"/>
        /// and
        /// <see cref="Context.TimeBasedContext"/>.
        /// </remarks>
        /// <returns>
        /// created
        /// <see cref="SignatureValidationProperties"/>
        /// instance.
        /// </returns>
        protected internal virtual SignatureValidationProperties GetSignatureValidationProperties()
        {
            SignatureValidationProperties properties = new SignatureValidationProperties();
            properties.SetRevocationOnlineFetching(ValidatorContexts.All(), CertificateSources.All(), TimeBasedContexts
                .All(), SignatureValidationProperties.OnlineFetching.NEVER_FETCH);
            properties.SetFreshness(ValidatorContexts.All(), CertificateSources.All(), TimeBasedContexts.Of(
                TimeBasedContext.HISTORICAL), TimeSpan.FromDays(-5));
            properties.SetContinueAfterFailure(
                ValidatorContexts.Of(ValidatorContext.OCSP_VALIDATOR, ValidatorContext.CRL_VALIDATOR),
                CertificateSources.Of(CertificateSource.CRL_ISSUER, CertificateSource.OCSP_ISSUER, CertificateSource
                    .CERT_ISSUER), false);
            return properties;
        }

        /// <summary>Check if the signature covers the entire document and verify signature integrity and authenticity.
        ///     </summary>
        /// <param name="report">validation report</param>
        /// <param name="signatureUtil">signature util instance created for the signed document</param>
        /// <param name="latestSignatureName">signature name</param>
        /// <param name="pkcs7">
        /// created
        /// <see cref="iText.Signatures.PdfPKCS7"/>
        /// instance
        /// </param>
        protected internal virtual void ValidateSignature(ValidationReport report, SignatureUtil signatureUtil, String
            latestSignatureName, PdfPKCS7 pkcs7)
        {
            if (!signatureUtil.SignatureCoversWholeDocument(latestSignatureName))
            {
                report.AddReportItem(new ReportItem(SIGNATURE_VERIFICATION, MessageFormatUtil.Format(
                    DOCUMENT_IS_NOT_COVERED
                    , latestSignatureName), ReportItem.ReportItemStatus.INVALID));
            }

            try
            {
                if (!pkcs7.VerifySignatureIntegrityAndAuthenticity())
                {
                    report.AddReportItem(new ReportItem(SIGNATURE_VERIFICATION, MessageFormatUtil.Format(
                        CANNOT_VERIFY_SIGNATURE
                        , latestSignatureName), ReportItem.ReportItemStatus.INVALID));
                }
            }
            catch (AbstractGeneralSecurityException e)
            {
                report.AddReportItem(new ReportItem(SIGNATURE_VERIFICATION, MessageFormatUtil.Format(
                    CANNOT_VERIFY_SIGNATURE
                    , latestSignatureName), e, ReportItem.ReportItemStatus.INVALID));
            }
        }

        /// <summary>Verify timestamp imprint and validate timestamp certificates chain.</summary>
        /// <param name="certificateRetriever">certificate retriever</param>
        /// <param name="report">validation report</param>
        /// <param name="validator">certificate chain validator instance</param>
        /// <param name="pkcs7">
        /// created
        /// <see cref="iText.Signatures.PdfPKCS7"/>
        /// instance
        /// </param>
        protected internal virtual void ValidateTimestamp(IssuingCertificateRetriever certificateRetriever,
            ValidationReport report, CertificateChainValidator validator, PdfPKCS7 pkcs7)
        {
            try
            {
                if (!pkcs7.VerifyTimestampImprint())
                {
                    report.AddReportItem(new ReportItem(TIMESTAMP_VERIFICATION, CANNOT_VERIFY_TIMESTAMP, ReportItem
                        .ReportItemStatus
                        .INVALID));
                }
            }
            catch (AbstractGeneralSecurityException e)
            {
                report.AddReportItem(new ReportItem(TIMESTAMP_VERIFICATION, CANNOT_VERIFY_TIMESTAMP, e, ReportItem
                    .ReportItemStatus
                    .INVALID));
            }

            // Validate timestamp certificates chain.
            IX509Certificate[] timestampCertificates = pkcs7.GetTimestampCertificates();
            certificateRetriever.AddKnownCertificates(JavaUtil.ArraysAsList(timestampCertificates));
            ValidationContext baseContext = new ValidationContext(ValidatorContext.CERTIFICATE_CHAIN_VALIDATOR,
                CertificateSource.TIMESTAMP, TimeBasedContext.PRESENT);
            validator.Validate(report, baseContext, (IX509Certificate)timestampCertificates[0],
                DateTimeUtil.GetCurrentUtcTime
                    ());
        }

        /// <summary>Creates an OCSP client for the sample.</summary>
        /// <remarks>
        /// Creates an OCSP client for the sample.
        /// <para />
        /// NOTE: for the real validation you should use real revocation data clients
        /// (such as
        /// <see cref="iText.Signatures.OcspClientBouncyCastle"/>
        /// ).
        /// </remarks>
        /// <returns>the OCSP client to be used for the validation.</returns>
        protected internal virtual IOcspClient GetOcspClient()
        {
            try
            {
                IX509Certificate[] certificateChain = GetCertificateChain();
                IX509Certificate signCert = (IX509Certificate)certificateChain[0];
                IX509Certificate intermediateCert = (IX509Certificate)certificateChain[1];
                IX509Certificate rootCert = (IX509Certificate)certificateChain[2];
                String timestamp = "../../../resources/cert/timestamp.pem";
                IX509Certificate timestampCert = (IX509Certificate)PemFileHelper.ReadFirstChain(timestamp)[0];
                IPrivateKey rootPrivateKey = GetPrivateKey();
                DateTime currentDate = DateTimeUtil.GetCurrentUtcTime();
                TestOcspResponseBuilder builder1 = new TestOcspResponseBuilder(rootCert, rootPrivateKey);
                builder1.SetProducedAt(currentDate);
                builder1.SetThisUpdate(DateTimeUtil.GetCalendar(currentDate));
                builder1.SetNextUpdate(DateTimeUtil.GetCalendar(currentDate.AddDays(30)));
                TestOcspResponseBuilder builder2 = new TestOcspResponseBuilder(rootCert, rootPrivateKey);
                builder2.SetProducedAt(currentDate);
                builder2.SetThisUpdate(DateTimeUtil.GetCalendar(currentDate));
                builder2.SetNextUpdate(DateTimeUtil.GetCalendar(currentDate.AddDays(30)));
                TestOcspResponseBuilder builder3 = new TestOcspResponseBuilder(rootCert, rootPrivateKey);
                builder3.SetProducedAt(currentDate);
                builder3.SetThisUpdate(DateTimeUtil.GetCalendar(currentDate));
                builder3.SetNextUpdate(DateTimeUtil.GetCalendar(currentDate.AddDays(30)));
                return new TestOcspClient().AddBuilderForCertificate(intermediateCert, builder1)
                    .AddBuilderForCertificate(signCert, builder2)
                    .AddBuilderForCertificate(timestampCert, builder3);
                ;
            }
            catch (Exception e)
            {
                throw new PdfException(e);
            }
        }

        /// <summary>Creates signing chain for the revocation data clients.</summary>
        /// <remarks>Creates signing chain for the revocation data clients. This chain shouldn't be used for the real validation.
        ///     </remarks>
        /// <returns>the chain of certificates to be used for the validation.</returns>
        private IX509Certificate[] GetCertificateChain()
        {
            if (certificateChain == null)
            {
                try
                {
                    String CHAIN = "../../../resources/cert/validCertsChain.pem";
                    certificateChain = PemFileHelper.ReadFirstChain(CHAIN);
                }
                catch (Exception)
                {
                    // Ignore.
                }
            }

            return certificateChain;
        }

        /// <summary>Creates private key for the sample.</summary>
        /// <remarks>Creates private key for the sample. This key shouldn't be used for the real validation.</remarks>
        /// <returns>
        /// 
        /// <see cref="iText.Commons.Bouncycastle.Crypto.IPrivateKey"/>
        /// instance to be used for the validation.
        /// </returns>
        private IPrivateKey GetPrivateKey()
        {
            if (privateKey == null)
            {
                try
                {
                    String rootKey = "../../../resources/cert/rootCertKey.pem";
                    char[] password = "testpassphrase".ToCharArray();
                    privateKey = PemFileHelper.ReadFirstKey(rootKey, password);
                }
                catch (Exception)
                {
                    // Ignore.
                }
            }

            return privateKey;
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = System.Buffer.GetByte(str.ToCharArray(), 2 * i);
            }

            return bytes;
        }
    }
}