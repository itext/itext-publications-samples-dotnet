using System;
using System.Collections.Generic;
using System.Reflection;
using iText.Kernel.Utils;
using iText.Licensing.Base;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class SplitAndCountSampleTest : WrappedSamplesRunner
    {
        public SplitAndCountSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Sandbox.Split.SplitAndCount");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            LicenseKey.UnloadLicenses();
            RunSamples();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            CompareTool compareTool = new CompareTool();

            for (int i = 1; i < 8; i++)
            {
                String currentDest = String.Format(dest, i);
                String currentCmp = String.Format(cmp, i);

                AddError(compareTool.CompareByContent(currentDest, currentCmp, outPath, "diff_"));
                AddError(compareTool.CompareDocumentInfo(currentDest, currentCmp));
            }
        }
    }
}
