using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Geom;
using iText.Samples.Signatures.Chapter02;
using iText.Samples.Signatures.Chapter04;
using iText.Signatures;
using iText.Test;
using NUnit.Framework;
using Org.BouncyCastle.X509;

namespace iText.Samples.Signatures.Testrunners
{
    [TestFixtureSource("Data")]
    public class LockFieldsTest : WrappedSamplesRunner
    {
        private static readonly IDictionary<int, IList<Rectangle>> ignoredAreaMap;

        private static readonly String ALICE = "../../../resources/encryption/alice.crt";
        private static readonly String BOB = "../../../resources/encryption/bob.crt";
        private static readonly String CAROL = "../../../resources/encryption/carol.crt";
        private static readonly String DAVE = "../../../resources/encryption/dave.crt";

        private static readonly String EXPECTED_ERROR_TEXT =
            "\nresults/signatures/chapter02/step_5_signed_by_alice_and_bob_broken_by_chuck.pdf:\n" +
            "Document signatures validation failed!\n\n" +
            "ReportItem{checkName='FieldMDP check.', message='Locked form field \"approved_bob\" or " +
            "one of its widgets was modified.', cause=, status=INVALID}\n" +
            "\nresults/signatures/chapter02/step_6_signed_by_dave_broken_by_chuck.pdf:\n" +
            "Document signatures validation failed!\n\n" +
            "ReportItem{checkName='DocMDP check.', message='Form field approved_carol was removed or " +
            "unexpectedly modified.', cause=, status=INVALID}\n\n" +
            "ReportItem{checkName='DocMDP check.', message='PDF document AcroForm contains changes " +
            "other than document timestamp (docMDP level >= 1), form fill-in and digital signatures " +
            "(docMDP level >= 2), adding or editing annotations (docMDP level 3), which are not allowed.', " +
            "cause=, status=INVALID}\n";

        static LockFieldsTest()
        {
            ignoredAreaMap = new Dictionary<int, IList<Rectangle>>();
            ignoredAreaMap.Add(1, new List<Rectangle>(new[]
            {
                new Rectangle(55, 425, 287, 380)
            }));
        }

        public LockFieldsTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter02.C2_12_LockFields");

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
            StringBuilder errorTemp = new StringBuilder();
            for (int i = 0; i < resultFiles.Length; i++)
            {
                String currentDest = dest + resultFiles[i];
                String currentCmp = cmp + resultFiles[i];

                /* TODO: DEVSIX-1623
                 * For some reason Acrobat recognizes last signature in the
                 * 'step_4_signed_by_alice_bob_carol_and_dave.pdf' file as invalid.
                 * It happens only if LockPermissions is set to NO_CHANGES_ALLOWED for the last signature form field.
                 * It's still unclear, whether it's iText messes up the document or it's Acrobat bug.
                 */
                String result = new CustomSignatureTest().CheckForErrors(currentDest, currentCmp,
                    outPath, ignoredAreaMap);
                
                if (result != null)
                {
                    errorTemp.Append(result);
                }
            }
            
            String errorText = errorTemp.ToString();
            if (!errorText.Contains(EXPECTED_ERROR_TEXT))
            {
                errorText += "\n'step_5_signed_by_alice_and_bob_broken_by_chuck.pdf' and " +
                             "'step_6_signed_by_dave_broken_by_chuck.pdf' files' signatures are expected to be invalid.\n\n";
            }
            else
            {
                // Expected error should be ignored
                errorText = errorText.Replace(EXPECTED_ERROR_TEXT, "");
            }

            AddError(errorText);
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
