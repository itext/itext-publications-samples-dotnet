using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.IO.Font;
using iText.License;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class ExtractStreamsTest : WrappedSamplesRunner
    {
        public ExtractStreamsTest(RunnerParams runnerParams) : base(runnerParams)
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
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.xml");
            RunSamples();
            ResetLicense();
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
            return "../../resources/" + dest.Substring(j);
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

        private void ResetLicense()
        {
            try
            {
                FieldInfo validatorsField = typeof(LicenseKey).GetField("validators",
                    BindingFlags.NonPublic | BindingFlags.Static);
                validatorsField.SetValue(null, null);
                FieldInfo versionField = typeof(Kernel.Version).GetField("version",
                    BindingFlags.NonPublic | BindingFlags.Static);
                versionField.SetValue(null, null);
            }
            catch
            {
                
                // No exception handling required, because there can be no license loaded
            }
        }
    }
}