using System;
using System.Collections.Generic;
using System.Reflection;
using iText.Kernel.Utils;
using iText.Licensing.Base;
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
#if !NETSTANDARD2_0
            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
#else
            return GenerateTestsList(typeof(C06E09_FillOutFlattenAndMergeFormsWrapperTest).GetTypeInfo().Assembly, searchConfig);
#endif
        }
        
        [NUnit.Framework.Test]
        public virtual void Test() {
            LicenseKey.UnloadLicenses();
            RunSamples();
        }

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
        
        protected override string GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }
            int i = dest.LastIndexOf("/");
            int j = dest.IndexOf("results") + 8;
            return "../../../cmpfiles/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
        }
    }
}
