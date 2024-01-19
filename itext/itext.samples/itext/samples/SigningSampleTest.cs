using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.Commons.Utils;
using iText.Kernel.Geom;
using iText.Kernel.Utils;
using iText.Licensing.Base;
using iText.Licensing.Base.Reporting;
using iText.Samples.Sandbox.Signatures.Utils;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class SigningSampleTest : WrappedSamplesRunner
    {

        /**
         * Global map of classes with ignored areas
         */
        private static readonly IDictionary<String, IDictionary<int, IList<Rectangle>>> ignoredClassesMap;

        static SigningSampleTest()
        {
            Rectangle signatureIgnoredArea = new Rectangle(150, 660, 100, 80);
            IList<Rectangle> rectangles = JavaUtil.ArraysAsList(signatureIgnoredArea);
            IDictionary<int, IList<Rectangle>> ignoredAreasMap = new Dictionary<int, IList<Rectangle>>();
            ignoredAreasMap.Add(1, rectangles);
            ignoredClassesMap = new Dictionary<String, IDictionary<int, IList<Rectangle>>>();
            ignoredClassesMap.Add("iText.Samples.Sandbox.Signatures.Appearance.PadesSignatureAppearanceExample", 
                ignoredAreasMap);
        }

        public SigningSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Sandbox.Signatures.Pades");
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Sandbox.Signatures.Appearance");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            LicenseKeyReportingConfigurer.UseLocalReporting("./target/test/com/itextpdf/samples/report/");
            using (Stream license = FileUtil.GetInputStreamForFile(
                       Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            RunSamples();
            LicenseKey.UnloadLicenses();
        }

        protected override void ComparePdf(String outPath, String dest, String cmp)
        {
            CompareTool compareTool = new CompareTool();
            if (ignoredClassesMap.Keys.Contains(sampleClass.FullName))
            {
                AddError(compareTool.CompareVisually(dest, cmp, outPath, "diff_",
                    ignoredClassesMap[sampleClass.FullName]));
            }
            else
            {
                AddError(compareTool.CompareVisually(dest, cmp, outPath, "diff_"));
            }
            AddError(SignaturesCompareTool.CompareSignatures(dest, cmp));
        }
    }
}
