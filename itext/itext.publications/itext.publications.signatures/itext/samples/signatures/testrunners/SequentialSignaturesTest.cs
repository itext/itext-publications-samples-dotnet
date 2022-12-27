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
using iText.Kernel.Geom;
using iText.Samples.Signatures.Chapter02;
using iText.Test;
using NUnit.Framework;
using Org.BouncyCastle.X509;

namespace iText.Samples.Signatures.Testrunners
{
    [TestFixtureSource("Data")]
    public class SequentialSignaturesTest : WrappedSamplesRunner
    {
        private static readonly IDictionary<int, IList<Rectangle>> ignoredAreaMap;

        static SequentialSignaturesTest()
        {
            ignoredAreaMap = new Dictionary<int, IList<Rectangle>>();
            ignoredAreaMap.Add(1, new List<Rectangle>(new[]
            {
                new Rectangle(55, 550, 287, 255)
            }));
        }

        public SequentialSignaturesTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter02.C2_10_SequentialSignatures");

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

                /* 'signed_by_carol2.pdf' file should be invalid from dig sig point of view,
                 * however both Acrobat and iText doesn't recognize it (certification signatures shall be
                 * the first signatures in the document, still signatures themselves are not broken in it).
                 */
                /* iText doesn't recognize invalidated signatures in "signed_by_carol3.pdf",
                 * "signed_by_carol4.pdf" files, because we don't check that document shall
                 * have only one certification signature and it shall be the first one.
                 * However signatures themselves are not broken.
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
            protected internal override void InitKeyStoreForVerification(List<X509Certificate> ks)
            {
                base.InitKeyStoreForVerification(ks);
                ks.Add(LoadCertificateFromKeyStore(C2_10_SequentialSignatures.ALICE,
                    C2_10_SequentialSignatures.PASSWORD));
                ks.Add(LoadCertificateFromKeyStore(C2_10_SequentialSignatures.BOB,
                    C2_10_SequentialSignatures.PASSWORD));
                ks.Add(LoadCertificateFromKeyStore(C2_10_SequentialSignatures.CAROL,
                    C2_10_SequentialSignatures.PASSWORD));
            }
        }
    }
}
