using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using iText.Kernel.Utils;
using iText.Licensing.Base;
using iText.Test;

namespace iText.Highlevel {
    [TestFixtureSource("Data")]

    public class HighLevelWrapperC01E02Cardo2Test: WrappedSamplesRunner {
        public HighLevelWrapperC01E02Cardo2Test(RunnerParams runnerParams) : base(runnerParams)
        {
        }
        
        /*    [Parameterized.Parameters(QName = "{index}: {0}")]*/
        public static ICollection<TestFixtureData> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Highlevel.Chapter01.C01E02_Text_Paragraph_Cardo2");
            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }
        
        [NUnit.Framework.Timeout(60000)]
        [NUnit.Framework.Test]
        public virtual void Test() {
            RunSamples();
        }
        
        protected override string GetCmpPdf(String dest) {
            if (dest == null) {
                return null;
            }
            int i = dest.LastIndexOf("/");
            int j = dest.IndexOf("results") + 8;
            return "../../../cmpfiles/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
        }

        protected override void ComparePdf(String outPath, String dest, String cmp) {

            for (int i = 1; i < 4; i++)
            {
                String currentDest = String.Format(dest, i);
                String currentOutPath = String.Format(outPath, i);
                String currentCmp = String.Format(cmp, i);
                CompareTool compareTool = new CompareTool();
                AddError(compareTool.CompareByContent(currentDest, currentCmp, currentOutPath, "diff_"));
                AddError(compareTool.CompareDocumentInfo(currentDest, currentCmp));
            }
        }
    }
}