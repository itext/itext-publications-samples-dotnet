using System;
using System.Collections.Generic;
using NUnit.Framework.Runners;
using iText.Kernel.Utils;
using iText.Test;

namespace Tutorial {
    public class JumpStartWrapperTest : WrappedSamplesRunner {
        [Parameterized.Parameters(Name = "{index}: {0}")]
        public static ICollection<Object[]> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("tutorial");
            searchConfig.IgnorePackageOrClass("tutorial.chapter06.C06E09_FillOutFlattenAndMergeForms");
            return GenerateTestsList(searchConfig);
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Timeout(120000)]
        [NUnit.Framework.Test]
        public virtual void Test() {
            RunSamples();
        }

        /// <exception cref="System.Exception"/>
        protected override void ComparePdf(String outPath, String dest, String cmp) {
            CompareTool compareTool = new CompareTool();
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            AddError(compareTool.CompareDocumentInfo(dest, cmp));
        }
    }
}
