using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Licensing.Base;
using iText.Licensing.Base.Reporting;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class ExtractStreamsSampleTest : WrappedSamplesRunner
    {
        public ExtractStreamsSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Sandbox.Parse.ExtractStreams");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            FontCache.ClearSavedFonts();
            FontProgramFactory.ClearRegisteredFonts();
            LicenseKeyReportingConfigurer.UseLocalReporting("./target/test/com/itextpdf/samples/report/");
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            RunSamples();
            LicenseKey.UnloadLicenses();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            for (int i = 1; i < 3; i++)
            {
                String currentDest = String.Format(dest + "/extract_streams{0}.dat", i);
                String currentCmp = String.Format(cmp + "/cmp_extract_streams{0}.dat", i);

                AddError(CompareFiles(currentDest, currentCmp));
            }
        }

        protected override String GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }

            int j = dest.LastIndexOf("/results", StringComparison.Ordinal) + 9;
            return "../../../cmpfiles/" + dest.Substring(j);
        }

        private String CompareFiles(String dest, String cmp)
        {
            String errorMessage = null;

            FileStream raf = new FileStream(dest, FileMode.Open, FileAccess.Read);
            byte[] destBytes = new byte[(int) raf.Length];
            raf.Read(destBytes, 0, destBytes.Length);
            raf.Close();

            raf = new FileStream(cmp, FileMode.Open, FileAccess.Read);
            byte[] cmpBytes = new byte[(int) raf.Length];
            raf.Read(cmpBytes, 0, cmpBytes.Length);
            raf.Close();

            try
            {
                CollectionAssert.AreEqual(cmpBytes, destBytes);
            }
            catch (AssertionException exc)
            {
                errorMessage = "Files are not equal:\n " + exc;
            }

            return errorMessage;
        }
    }
}
