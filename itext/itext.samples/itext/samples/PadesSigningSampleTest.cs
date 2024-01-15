using System;
using System.Collections.Generic;
using System.Reflection;
using iText.Kernel.Utils;
using iText.Licensing.Base;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class PadesSigningSampleTest : WrappedSamplesRunner
    {

        public PadesSigningSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Sandbox.Signatures.Pades");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            LicenseKey.UnloadLicenses();
            RunSamples();
        }

        protected override void ComparePdf(String outPath, String dest, String cmp)
        {
            CompareTool compareTool = new CompareTool();
            AddError(compareTool.CompareVisually(dest, cmp, outPath, "diff_"));
            AddError(SignaturesCompareTool.CompareSignatures(dest, cmp));
        }
    }
}
