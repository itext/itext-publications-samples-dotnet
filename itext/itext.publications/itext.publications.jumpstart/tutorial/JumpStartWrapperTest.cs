using System;
using System.Collections.Generic;
using System.Reflection;
using iText.Kernel.Utils;
using iText.Test;
using NUnit.Framework;

namespace Tutorial {
    [TestFixtureSource("Data")]
    public class JumpStartWrapperTest : WrappedSamplesRunner {

        public JumpStartWrapperTest(RunnerParams runnerParams) : base(runnerParams) {
        }

        public static ICollection<TestFixtureData> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("Tutorial");
            searchConfig.IgnorePackageOrClass("Tutorial.Chapter06.C06E09_FillOutFlattenAndMergeForms");
            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
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
