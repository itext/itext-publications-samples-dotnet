using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Geom;
using iText.Samples.Signatures.Chapter02;
using iText.Signatures;
using iText.Test;
using NUnit.Framework;
using Org.BouncyCastle.X509;

namespace iText.Samples.Signatures.Testrunners
{
    [TestFixtureSource("Data")]
    public class SignatureWorkflowTest : WrappedSamplesRunner
    {
        private static readonly IDictionary<int, IList<Rectangle>> ignoredAreaMap;

        private static readonly String ALICE = "../../../resources/encryption/alice.crt";
        private static readonly String BOB = "../../../resources/encryption/bob.crt";
        private static readonly String CAROL = "../../../resources/encryption/carol.crt";
        private static readonly String DAVE = "../../../resources/encryption/dave.crt";

        static SignatureWorkflowTest()
        {
            ignoredAreaMap = new Dictionary<int, IList<Rectangle>>();
            ignoredAreaMap.Add(1, new List<Rectangle>(new[]
            {
                new Rectangle(55, 440, 287, 365)
            }));
        }

        public SignatureWorkflowTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter02.C2_11_SignatureWorkflow");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }
        
        [Test, Description("{0}")]
        public virtual void Test()
        {
            RunSamples();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            String[] resultFiles = GetResultFiles(sampleClass);
            for (int i = 0; i < resultFiles.Length; i++)
            {
                String currentDest = dest + resultFiles[i];
                String currentCmp = cmp + resultFiles[i];

                AddError(new CustomSignatureTest().CheckForErrors(currentDest, currentCmp,
                    outPath, ignoredAreaMap));
            }
        }

        protected override String GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }

            int i = dest.LastIndexOf("/", StringComparison.Ordinal);
            int j = dest.LastIndexOf("/results", StringComparison.Ordinal) + 9;
            return "../../../resources/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
        }

        private static String[] GetResultFiles(Type c)
        {
            try
            {
                FieldInfo field = c.GetField("RESULT_FILES");
                if (field == null)
                {
                    return null;
                }

                Object obj = field.GetValue(null);
                if (obj == null || !(obj is String[]))
                {
                    return null;
                }

                return (String[]) obj;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private class CustomSignatureTest : SignatureTestHelper
        {
            protected internal override void AddTrustedCertificates(IssuingCertificateRetriever certificateRetriever,
                ICollection<IX509Certificate> certificates)
            {
                base.AddTrustedCertificates(certificateRetriever, certificates);
                var parser = new X509CertificateParser();
                IX509Certificate aliceCert;
                IX509Certificate bobCert;
                IX509Certificate carolCert;
                IX509Certificate daveCert;
                using (FileStream aliceStream = new FileStream(ALICE, FileMode.Open, FileAccess.Read),
                       bobStream = new FileStream(BOB, FileMode.Open, FileAccess.Read),
                       carolStream = new FileStream(CAROL, FileMode.Open, FileAccess.Read),
                       daveStream = new FileStream(DAVE, FileMode.Open, FileAccess.Read))
                {
                    aliceCert = new X509CertificateBC(parser.ReadCertificate(aliceStream));
                    bobCert = new X509CertificateBC(parser.ReadCertificate(bobStream));
                    carolCert = new X509CertificateBC(parser.ReadCertificate(carolStream));
                    daveCert = new X509CertificateBC(parser.ReadCertificate(daveStream));
                }
                certificateRetriever.AddTrustedCertificates(new[] { aliceCert, bobCert, carolCert, daveCert });
            }
        }
    }
}
