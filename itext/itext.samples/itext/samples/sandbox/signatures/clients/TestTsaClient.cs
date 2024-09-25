using System;
using System.Collections;
using System.Collections.Generic;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Math;
using iText.Commons.Bouncycastle.Tsp;
using iText.Commons.Digest;
using iText.Commons.Utils;
using iText.Kernel.Crypto;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Clients {
    public class TestTsaClient : ITSAClient
    {
        private static readonly IBouncyCastleFactory BOUNCY_CASTLE_FACTORY = BouncyCastleFactoryCreator.GetFactory
            ();

        private const String DIGEST_ALG = "SHA256";

        private readonly IPrivateKey tsaPrivateKey;

        private IList<IX509Certificate> tsaCertificateChain;

        public TestTsaClient(IList<IX509Certificate> tsaCertificateChain, IPrivateKey tsaPrivateKey)
        {
            this.tsaCertificateChain = tsaCertificateChain;
            this.tsaPrivateKey = tsaPrivateKey;
        }

        public virtual int GetTokenSizeEstimate()
        {
            return 4096;
        }

        public virtual IMessageDigest GetMessageDigest()
        {
            return BOUNCY_CASTLE_FACTORY.CreateIDigest(DIGEST_ALG);
        }

        public virtual byte[] GetTimeStampToken(byte[] imprint)
        {
            ITimeStampRequestGenerator tsqGenerator = BOUNCY_CASTLE_FACTORY.CreateTimeStampRequestGenerator();
            tsqGenerator.SetCertReq(true);
            IBigInteger nonce = iText.Bouncycastleconnector.BouncyCastleFactoryCreator.GetFactory().CreateBigInteger()
                .ValueOf
                    (SystemUtil.GetTimeBasedSeed());
            ITimeStampRequest request = tsqGenerator.Generate(BOUNCY_CASTLE_FACTORY.CreateASN1ObjectIdentifier(
                DigestAlgorithms
                    .GetAllowedDigest(DIGEST_ALG)), imprint, nonce);
            return new TestTimestampTokenBuilder(tsaCertificateChain, tsaPrivateKey).CreateTimeStampToken(request);
        }

        private class TestTimestampTokenBuilder
        {
            private static readonly IBouncyCastleFactory FACTORY = BouncyCastleFactoryCreator.GetFactory();
            private IList<IX509Certificate> tsaCertificateChain;

            // just a more or less random oid of timestamp policy
            private static readonly String POLICY_OID = "1.3.6.1.4.1.45794.1.1";

            private IPrivateKey tsaPrivateKey;

            public TestTimestampTokenBuilder(IList<IX509Certificate> tsaCertificateChain, IPrivateKey tsaPrivateKey
            )
            {
                if (tsaCertificateChain.Count == 0)
                {
                    throw new ArgumentException("tsaCertificateChain shall not be empty");
                }

                this.tsaCertificateChain = tsaCertificateChain;
                this.tsaPrivateKey = tsaPrivateKey;
            }

            public virtual byte[] CreateTimeStampToken(ITimeStampRequest request)
            {
                ITimeStampTokenGenerator tsTokGen = CreateTimeStampTokenGenerator(tsaPrivateKey, tsaCertificateChain[0],
                    "SHA1", POLICY_OID);
                tsTokGen.SetAccuracySeconds(1);

                tsTokGen.SetCertificates(tsaCertificateChain);
                // should be unique for every timestamp
                IBigInteger serialNumber = FACTORY.CreateBigInteger(SystemUtil.GetTimeBasedSeed().ToString());
                DateTime genTime = DateTimeUtil.GetCurrentUtcTime();
                ITimeStampToken tsToken = tsTokGen.Generate(request, serialNumber, genTime);
                return tsToken.GetEncoded();
            }

            public virtual byte[] CreateTSAResponse(byte[] requestBytes, String signatureAlgorithm,
                String allowedDigest)
            {
                try
                {
                    String digestForTsSigningCert = DigestAlgorithms.GetAllowedDigest(allowedDigest);
                    ITimeStampTokenGenerator tokenGenerator = CreateTimeStampTokenGenerator(tsaPrivateKey,
                        tsaCertificateChain[0], allowedDigest, POLICY_OID);

                    IList<String> algorithms = new List<string>();
                    algorithms.Add(digestForTsSigningCert);
                    ITimeStampResponseGenerator generator =
                        FACTORY.CreateTimeStampResponseGenerator(tokenGenerator, (IList)algorithms);
                    ITimeStampRequest request = FACTORY.CreateTimeStampRequest(requestBytes);
                    return generator.Generate(request, request.GetNonce(), new DateTime()).GetEncoded();
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            private static ITimeStampTokenGenerator CreateTimeStampTokenGenerator(IPrivateKey pk, IX509Certificate cert,
                String allowedDigest, String policyOid)
            {
                return FACTORY.CreateTimeStampTokenGenerator(pk, cert,
                    DigestAlgorithms.GetDigest(DigestAlgorithms.GetAllowedDigest(allowedDigest)), policyOid);
            }

        }
    }
}
