/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2023 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Geom;
using iText.Samples.Signatures.Chapter02;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples.Signatures.Testrunners
{
    [TestFixtureSource("Data")]
    public class LockFieldsTest : WrappedSamplesRunner
    {
        private static readonly IDictionary<int, IList<Rectangle>> ignoredAreaMap;

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
                /* iText doesn't recognize invalidated signatures in those files,
                 * because we don't check changes in new revisions against old signatures (permissions,
                 * certifications, content changes), however signatures themselves are not broken.
                 */
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
            protected internal override void InitKeyStoreForVerification(List<IX509Certificate> ks)
            {
                base.InitKeyStoreForVerification(ks);
                ks.Add(LoadCertificateFromKeyStore(C2_12_LockFields.ALICE,
                    C2_11_SignatureWorkflow.PASSWORD));
                ks.Add(LoadCertificateFromKeyStore(C2_12_LockFields.BOB,
                    C2_11_SignatureWorkflow.PASSWORD));
                ks.Add(LoadCertificateFromKeyStore(C2_12_LockFields.CAROL,
                    C2_11_SignatureWorkflow.PASSWORD));
                ks.Add(LoadCertificateFromKeyStore(C2_12_LockFields.DAVE,
                    C2_11_SignatureWorkflow.PASSWORD));
            }
        }
    }
}
