using System;
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

namespace iText.Samples.Sandbox.Signatures.Utils
{
    public class TestOcspResponseBuilder
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
        
        private DateTime producedAt = TEST_DATE_TIME;

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

        public void SetProducedAt(DateTime producedAt)
        {
            this.producedAt = producedAt;
        }

        public void SetThisUpdate(DateTime thisUpdate)
        {
            this.thisUpdate = thisUpdate;
        }

        public void SetNextUpdate(DateTime nextUpdate)
        {
            this.nextUpdate = nextUpdate;
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