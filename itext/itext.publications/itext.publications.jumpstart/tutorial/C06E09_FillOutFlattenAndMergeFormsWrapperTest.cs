using System;
using System.Collections.Generic;
using NUnit.Framework.Runners;
using iText.Kernel.Utils;
using iText.Test;

namespace Tutorial {
    /// <summary>
    /// C06E09_FillOutFlattenAndMergeForms sample has irregular DEST files: it has two of them,
    /// so we need process it in a specific way.
    /// </summary>
    public class C06E09_FillOutFlattenAndMergeFormsWrapperTest : WrappedSamplesRunner {
        [Parameterized.Parameters(Name = "{index}: {0}")]
        public static ICollection<Object[]> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("tutorial.chapter06.C06E09_FillOutFlattenAndMergeForms");
            return GenerateTestsList(searchConfig);
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Timeout(60000)]
        [NUnit.Framework.Test]
        public virtual void Test() {
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
    }
}
