using iText.Commons.Utils;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Utils;
using iText.Licensing.Base;
using iText.Licensing.Base.Reporting;
using iText.Samples.Sandbox.Fonts;
using iText.Test;
using iText.Test.Pdfa;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace iText.Samples
{
    /// <summary>
    /// These tests are separated as they require a longer timeout
    /// </summary>
    [TestFixtureSource("Data")]
    public class PdfOcrOnnxTest : WrappedSamplesRunner
    {

        /**
         * List of samples, which require txt files comparison
         */
        private static readonly List<string> txtCompareList = new List<string>(
            new[]
            {
                "iText.Samples.Sandbox.Pdfocr.Onnx.PdfOcrOnnxTxtFileExample"
            });

        public PdfOcrOnnxTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Sandbox.Pdfocr.Onnx");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(90000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            FontCache.ClearSavedFonts();
            FontProgramFactory.ClearRegisteredFonts();
            LicenseKeyReportingConfigurer.UseLocalReporting("results/test/com/itextpdf/samples/report/");
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
            
            if (txtCompareList.Contains(sampleClass.FullName))
            {
                AddError(CompareTxt(dest, cmp));
            }
            else if (txtCompareList.Contains(sampleClass.FullName))
            {
                AddError(CompareTxt(dest, cmp));
            }
            else
            {
                AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            }
        }

        private String CompareTxt(String dest, String cmp)
        {
            String errorMessage = null;
            Console.Out.WriteLine("Out txt: " + UrlUtil.GetNormalizedFileUriString(dest));
            Console.Out.WriteLine("Cmp txt: " + UrlUtil.GetNormalizedFileUriString(cmp) + "\n");

            using (
                StreamReader destReader = new StreamReader(dest),
                cmpReader = new StreamReader(cmp))
            {
                int lineNumber = 1;
                String destLine = destReader.ReadLine();
                String cmpLine = cmpReader.ReadLine();
                while (destLine != null || cmpLine != null)
                {
                    if (destLine == null || cmpLine == null)
                    {
                        errorMessage = "The number of lines is different\n";
                        break;
                    }

                    if (!destLine.Equals(cmpLine))
                    {
                        errorMessage = "Txt files differ at line " + lineNumber
                                                                   + "\n See difference: cmp file: \""
                                                                   + cmpLine + "\"\n"
                                                                   + "target file: \"" + destLine + "\n";
                    }

                    destLine = destReader.ReadLine();
                    cmpLine = cmpReader.ReadLine();
                    lineNumber++;
                }
            }

            return errorMessage;
        }
    }
}
