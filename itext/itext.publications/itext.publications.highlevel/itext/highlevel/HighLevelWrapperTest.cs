using System;
using System.Collections.Generic;
using System.Reflection;
using iText.IO.Util;
using iText.Kernel.Utils;
using iText.Test;
using NUnit.Framework;

namespace itext.publications.highlevel.itext.highlevel {
    [TestFixtureSource("Data")]
    public class HighLevelWrapperTest : WrappedSamplesRunner {
        public HighLevelWrapperTest(RunnerParams runnerParams) : base(runnerParams) {
            ResourceUtil.AddToResourceSearch(TestContext.CurrentContext.TestDirectory + "/itext.hyph.dll");
        }
        public static ICollection<TestFixtureData> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("itext.publications.highlevel.itext.highlevel");
            
            searchConfig.IgnorePackageOrClass("itext.publications.highlevel.itext.highlevel.chapter02.C02E15_ShowTextAlignedKerned");
            searchConfig.IgnorePackageOrClass("itext.publications.highlevel.itext.highlevel.chapter07.C07E14_Encrypted");
            return GenerateTestsList(Assembly.GetExecutingAssembly(),searchConfig);
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Timeout(60000)]
        [NUnit.Framework.Test, Description("{0}")]
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
