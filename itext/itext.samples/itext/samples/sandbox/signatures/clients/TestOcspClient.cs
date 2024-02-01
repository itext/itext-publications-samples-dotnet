using System;
using System.Collections;
using System.Collections.Generic;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Asn1;
using iText.Commons.Bouncycastle.Asn1.Ocsp;
using iText.Commons.Bouncycastle.Asn1.X500;
using iText.Commons.Bouncycastle.Asn1.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Cert.Ocsp;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Utils;
using iText.Kernel.Pdf;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Clients
{
    public class TestOcspClient : IOcspClient
    {
        private static readonly IBouncyCastleFactory BOUNCY_CASTLE_FACTORY = BouncyCastleFactoryCreator.GetFactory
            ();

        private readonly IDictionary<String, TestOcspResponseBuilder> certDNToResponseBuilder = new LinkedDictionary
            <String, TestOcspResponseBuilder>();

        public virtual TestOcspClient AddBuilderForCertificate(IX509Certificate cert, IPrivateKey privateKey)
        {
            certDNToResponseBuilder.Add(cert.GetSubjectDN().ToString(), new TestOcspResponseBuilder(cert, privateKey
            ));
            return this;
        }

        public virtual byte[] GetEncoded(IX509Certificate checkCert, IX509Certificate issuerCert, String url)
        {
            byte[] bytes = null;
            try
            {
                ICertID id = BOUNCY_CASTLE_FACTORY.CreateCertificateID(BOUNCY_CASTLE_FACTORY
                    .CreateCertificateID().GetHashSha1(), issuerCert, checkCert.GetSerialNumber());
                try
                {
                    TestOcspResponseBuilder builder = certDNToResponseBuilder[checkCert.GetSubjectDN().ToString()];
                    if (builder == null)
                    {
                        return null;
                    }

                    bytes = builder.MakeOcspResponse(GenerateOcspRequestWithNonce(id).GetEncoded());
                }
                catch (KeyNotFoundException)
                {
                    return null;
                }
            }
            catch (Exception ignored)
            {
                if (ignored != null)
                {
                    throw;
                }
            }

            return bytes;
        }

        private IOcspRequest GenerateOcspRequestWithNonce(ICertID id)
        {
            IOcspReqGenerator gen = BOUNCY_CASTLE_FACTORY.CreateOCSPReqBuilder();
            gen.AddRequest(id);

            // create details for nonce extension
            IDictionary extensions = new Hashtable();

            extensions[BOUNCY_CASTLE_FACTORY.CreateOCSPObjectIdentifiers().GetIdPkixOcspNonce()] =
                BOUNCY_CASTLE_FACTORY.CreateExtension(false,
                    BOUNCY_CASTLE_FACTORY.CreateDEROctetString(BOUNCY_CASTLE_FACTORY
                        .CreateDEROctetString(PdfEncryption.GenerateNewDocumentId()).GetEncoded()));

            gen.SetRequestExtensions(BOUNCY_CASTLE_FACTORY.CreateExtensions(extensions));
            return gen.Build();
        }

        private class TestOcspResponseBuilder
        {
            private static readonly IBouncyCastleFactory FACTORY = BouncyCastleFactoryCreator.GetFactory();
            private const String SIGN_ALG = "SHA256withRSA";
            private static readonly DateTime TEST_DATE_TIME = new DateTime(2000, 2, 14, 14, 14, 2, DateTimeKind.Utc);

            private IBasicOcspRespGenerator responseBuilder;

            private IX509Certificate issuerCert;
            private IPrivateKey issuerPrivateKey;

            private ICertStatus certificateStatus;

            private DateTime thisUpdate = TEST_DATE_TIME.AddDays(-1);

            private DateTime nextUpdate = TEST_DATE_TIME.AddDays(30);

            public TestOcspResponseBuilder(IX509Certificate issuerCert, IPrivateKey issuerPrivateKey,
                ICertStatus certificateStatus)
            {
                this.issuerCert = issuerCert;
                this.issuerPrivateKey = issuerPrivateKey;
                this.certificateStatus = certificateStatus;
                IX500Name subjectDN = issuerCert.GetSubjectDN();
                responseBuilder = FACTORY.CreateBasicOCSPRespBuilder(FACTORY.CreateRespID(subjectDN));
            }

            public TestOcspResponseBuilder(IX509Certificate issuerCert, IPrivateKey issuerPrivateKey)
                : this(issuerCert, issuerPrivateKey, FACTORY.CreateCertificateStatus().GetGood())
            {
            }

            public virtual byte[] MakeOcspResponse(byte[] requestBytes)
            {
                IBasicOcspResponse ocspResponse = MakeOcspResponseObject(requestBytes);
                return ocspResponse.GetEncoded();
            }

            public virtual IBasicOcspResponse MakeOcspResponseObject(byte[] requestBytes)
            {
                IOcspRequest ocspRequest = FACTORY.CreateOCSPReq(requestBytes);
                IReq[] requestList = ocspRequest.GetRequestList();

                IX509Extension extNonce = ocspRequest.GetExtension(FACTORY.CreateOCSPObjectIdentifiers()
                    .GetIdPkixOcspNonce());
                if (!FACTORY.IsNullExtension(extNonce))
                {
                    // TODO ensure
                    IX509Extensions responseExtensions = FACTORY.CreateExtensions(
                        new Dictionary<IDerObjectIdentifier, IX509Extension>()
                        {
                            {
                                FACTORY.CreateOCSPObjectIdentifiers().GetIdPkixOcspNonce(), extNonce
                            }
                        });
                    responseBuilder.SetResponseExtensions(responseExtensions);
                }

                foreach (IReq req in requestList)
                {
                    responseBuilder.AddResponse(req.GetCertID(), certificateStatus, thisUpdate.ToUniversalTime(),
                        nextUpdate.ToUniversalTime(),
                        FACTORY.CreateExtensions());
                }

                return responseBuilder.Build(FACTORY.CreateContentSigner(SIGN_ALG, issuerPrivateKey),
                    new[] { issuerCert }, TEST_DATE_TIME);
            }
        }
    }
}
