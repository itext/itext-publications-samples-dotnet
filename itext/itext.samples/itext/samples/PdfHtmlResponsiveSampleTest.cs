using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Utils;
using iText.Licensing.Base;
using iText.Licensing.Base.Reporting;
using iText.Samples.Sandbox.Pdfhtml;
using iText.StyledXmlParser.Css.Util;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class PdfHtmlResponsiveSampleTest : WrappedSamplesRunner
    {
        public PdfHtmlResponsiveSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Sandbox.Pdfhtml.PdfHtmlResponsiveDesign");

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
            FontProgramFactory.ClearRegisteredFonts();

            RunSamples();
            LicenseKey.UnloadLicenses();
        }

        protected override void ComparePdf(String outPath, String dest, String cmp)
        {
            CompareTool compareTool = new CompareTool();
            for (int i = 0; i < PdfHtmlResponsiveDesign.pageSizes.Length; i++)
            {
                float width = CssDimensionParsingUtils.ParseAbsoluteLength(PdfHtmlResponsiveDesign.pageSizes[i].GetWidth().ToString());
                String currentDest = dest.Replace("<filename>",
                    "responsive_" + width.ToString("0.0", CultureInfo.InvariantCulture) + ".pdf");
                String currentCmp = cmp.Replace("<filename>",
                    "responsive_" + width.ToString("0.0", CultureInfo.InvariantCulture) + ".pdf");

                AddError(compareTool.CompareByContent(currentDest, currentCmp, outPath, "diff_"));
                AddError(compareTool.CompareDocumentInfo(currentDest, currentCmp));
            }
        }

        protected override String GetOutPath(String dest)
        {
            return "./target/" + (Path.GetDirectoryName(dest.Replace("<filename>", "")));
        }
    }
}
