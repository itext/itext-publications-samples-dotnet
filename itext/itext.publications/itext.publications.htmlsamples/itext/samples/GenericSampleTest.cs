using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Utils;
using iText.Licensing.Base;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class GenericSampleTest : WrappedSamplesRunner
    {
        public GenericSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Htmlsamples");

            // Samples are run by separate samples runner
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter05.C05E03_Invitations");
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter07.C07E04_CreateFromURL");
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter07.C07E05_CreateFromURL2");
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter07.C07E06_CreateFromURL3");
            
            // Should not be run due to falling on different systems with different system fonts
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter06.C06E03_SystemFonts");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            FontCache.ClearSavedFonts();
            FontProgramFactory.ClearRegisteredFonts();
            
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT_LICENSE_FILE_LOCAL_STORAGE") + "/all-products.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            RunSamples();
            LicenseKey.UnloadLicenses();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            CompareTool compareTool = new CompareTool();

            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
        }
    }
}
