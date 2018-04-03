using System;
using System.Collections.Generic;
using iText.Kernel.Utils;
using iText.Test;
using iText.Kernel.XMP.Impl;
using System.Reflection;
using iText.License;
using NUnit.Framework;

namespace iText.Highlevel
{
    [TestFixtureSource("Data")]

    public class HighLevelWrapperWithEncryptionTest : WrappedSamplesRunner {
        public HighLevelWrapperWithEncryptionTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        /*    [Parameterized.Parameters(QName = "{index}: {0}")]*/
        public static ICollection<TestFixtureData> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Highlevel.Chapter07.C07E14_Encrypted");
            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Timeout(60000)]
        [NUnit.Framework.Test]
        public virtual void Test() {
            ResetLicense();
            RunSamples();
        }

        /// <exception cref="System.Exception"/>
        protected override void ComparePdf(String outPath, String dest, String cmp) {
            CompareTool compareTool = new CompareTool();
            byte[] ownerPass = GetBytes("abcdefg");
            compareTool.EnableEncryptionCompare();
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_", ownerPass, ownerPass));
            AddError(compareTool.CompareDocumentInfo(dest, cmp, ownerPass, ownerPass));
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = System.Buffer.GetByte(str.ToCharArray(), 2*i);
            }
            //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void ResetLicense() {
            try {
                FieldInfo validatorsField = typeof(LicenseKey).GetField("validators", BindingFlags.NonPublic | BindingFlags.Static);
                validatorsField.SetValue(null, null);
                FieldInfo versionField = typeof(iText.Kernel.Version).GetField("version", BindingFlags.NonPublic | BindingFlags.Static);
                versionField.SetValue(null, null);
            } catch {
            }
        }


    }
}
