using System;
using System.Collections.Generic;
using System.Reflection;
using iText.Kernel.Utils;
using iText.License;
using iText.Test;
using NUnit.Framework;

namespace Tutorial {
    /// <summary>
    /// C06E09_FillOutFlattenAndMergeForms sample has irregular DEST files: it has two of them,
    /// so we need process it in a specific way.
    /// </summary>
    [TestFixtureSource("Data")]
    public class C06E09_FillOutFlattenAndMergeFormsWrapperTest : WrappedSamplesRunner {

        public C06E09_FillOutFlattenAndMergeFormsWrapperTest(RunnerParams runnerParams) : base(runnerParams) {
        }

        public static ICollection<TestFixtureData> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("Tutorial.Chapter06.C06E09_FillOutFlattenAndMergeForms");
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
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            AddError(compareTool.CompareDocumentInfo(dest, cmp));
            // only DEST1 pdf will be checked, check additionally DEST2
            dest = GetStringField(sampleClass, "DEST2");
            cmp = GetCmpPdf(dest);
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            AddError(compareTool.CompareDocumentInfo(dest, cmp));
        }

        protected override String GetDest() {
            return GetStringField(sampleClass, "DEST1");
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
