using System;
using System.Collections.Generic;
using System.Linq;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Asn1.X500;
using iText.Commons.Bouncycastle.Cert;
using iText.Commons.Bouncycastle.Crypto;
using iText.Kernel.Exceptions;
using iText.Signatures;

namespace iText.Samples.Sandbox.Signatures.Clients
{
    public class TestCrlClient : ICrlClient
    {
        private readonly IList<TestCrlBuilder> crlBuilders;

        public TestCrlClient()
        {
            crlBuilders = new List<TestCrlBuilder>();
        }

        public virtual TestCrlClient AddBuilderForCertIssuer(IX509Certificate issuerCert
            , IPrivateKey issuerPrivateKey)
        {
            DateTime yesterday = TestCrlBuilder.TEST_DATE_TIME.AddDays(-1);
            crlBuilders.Add(new TestCrlBuilder(issuerCert, issuerPrivateKey, yesterday));
            return this;
        }

        public virtual ICollection<byte[]> GetEncoded(IX509Certificate checkCert, String url)
        {
            return crlBuilders.Select((testCrlBuilder) =>
                {
                    try
                    {
                        return testCrlBuilder.MakeCrl();
                    }
                    catch (Exception ignore)
                    {
                        throw new PdfException(ignore);
                    }
                }
            ).ToList();
        }

        private class TestCrlBuilder
        {
            internal static readonly DateTime TEST_DATE_TIME = new DateTime(2000, 2, 14, 14, 14, 2, DateTimeKind.Utc);

            private static readonly IBouncyCastleFactory FACTORY = BouncyCastleFactoryCreator.GetFactory();
            private const String SIGN_ALG = "SHA256withRSA";

            private readonly IPrivateKey issuerPrivateKey;
            private readonly IX509V2CrlGenerator crlBuilder;

            private DateTime nextUpdate = TEST_DATE_TIME.AddDays(30);

            public TestCrlBuilder(IX509Certificate issuerCert, IPrivateKey issuerPrivateKey, DateTime thisUpdate)
            {
                IX500Name issuerCertSubjectDn = issuerCert.GetSubjectDN();
                this.crlBuilder = FACTORY.CreateX509v2CRLBuilder(issuerCertSubjectDn, thisUpdate);
                this.issuerPrivateKey = issuerPrivateKey;
            }

            public virtual byte[] MakeCrl()
            {
                crlBuilder.SetNextUpdate(nextUpdate);
                IX509Crl crl = crlBuilder.Build(FACTORY.CreateContentSigner(SIGN_ALG, issuerPrivateKey));
                return crl.GetEncoded();
            }
        }
    }
}
