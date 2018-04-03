using System;
using System.Collections.Generic;
using System.Reflection;
using iText.IO.Util;
using iText.Kernel.Utils;
using iText.License;
using iText.Test;
using NUnit.Framework;

namespace iText.Highlevel {
    [TestFixtureSource("Data")]
    public class HighLevelWrapperTest : WrappedSamplesRunner {
        public HighLevelWrapperTest(RunnerParams runnerParams) : base(runnerParams) {
        }
        public static ICollection<TestFixtureData> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Highlevel");
            
            searchConfig.IgnorePackageOrClass("iText.Highlevel.Chapter02.C02E15_ShowTextAlignedKerned");
            searchConfig.IgnorePackageOrClass("iText.Highlevel.Chapter07.C07E14_Encrypted");
            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Timeout(60000)]
        [NUnit.Framework.Test, Description("{0}")]
        public virtual void Test() {
            ResetLicense();
            RunSamples();
        }

        /// <exception cref="System.Exception"/>
        protected override void ComparePdf(String outPath, String dest, String cmp) {
            CompareTool compareTool = new CompareTool();
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            AddError(compareTool.CompareDocumentInfo(dest, cmp));
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
