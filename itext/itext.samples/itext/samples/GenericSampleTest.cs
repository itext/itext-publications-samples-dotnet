using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.IO.Font;
using iText.Commons.Utils;
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Kernel.Utils;
using iText.Licensing.Base;
using iText.Licensing.Base.Reporting;
using iText.Test;
using iText.Test.Pdfa;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class GenericSampleTest : WrappedSamplesRunner
    {
        
        /**
         * List of samples, which should be validated visually and by links annotations on corresponding pages
         */
        private static readonly List<string> renderCompareList = new List<string>()
        {
            "iText.Samples.Sandbox.Signatures.SignatureExample"
        };

        /**
         * List of samples, which require xml files comparison
         */
        private static readonly List<string> xmlCompareList = new List<string>(new[]
            {
                    "iText.Samples.Sandbox.Acroforms.ReadXFA", 
                    "iText.Samples.Sandbox.Acroforms.CreateXfdf",
                    "iText.Samples.Sandbox.Stamper.AddNamedDestinations"

            });

        /**
         * List of samples, which require txt files comparison
         */
        private static readonly List<string> txtCompareList = new List<string>(
            new[]
            {
                "iText.Samples.Sandbox.Interactive.FetchBookmarkTitles",
                "iText.Samples.Sandbox.Parse.ParseCustom",
                "iText.Samples.Sandbox.Parse.ParseCzech",
                "iText.Samples.Sandbox.Logging.CounterDemo",
                "iText.Samples.Sandbox.Tagging.WalkTheTree",
                "iText.Samples.Sandbox.Signatures.Validation.ValidateChainBeforeSigningExample",
                "iText.Samples.Sandbox.Signatures.Validation.ValidateSignatureExample"
            });

        /**
         * List of samples, which require VeraPDF file comparison
         */
        private static readonly List<string> veraPdfValidateList = new List<string>(
            new[]
            {
                "iText.Samples.Sandbox.Pdfa.HelloPdfA2a",
                "iText.Samples.Sandbox.Pdfa.PdfA1a",
                "iText.Samples.Sandbox.Pdfa.PdfA1a_images",
                "iText.Samples.Sandbox.Pdfa.PdfA3"
            });

        /**
         * Global map of classes with ignored areas
         */
        private static readonly IDictionary<String, IDictionary<int, IList<Rectangle>>> ignoredClassesMap;

        static GenericSampleTest()
        {
            Rectangle latinClassIgnoredArea = new Rectangle(30, 539, 250, 13);
            IList<Rectangle> rectangles = JavaUtil.ArraysAsList(latinClassIgnoredArea);
            IDictionary<int, IList<Rectangle>> ignoredAreasMap = new Dictionary<int, IList<Rectangle>>();
            ignoredAreasMap.Add(1, rectangles);
            ignoredClassesMap = new Dictionary<String, IDictionary<int, IList<Rectangle>>>();
            ignoredClassesMap.Add("iText.Samples.Sandbox.Typography.Latin.LatinSignature", ignoredAreasMap);
        }

        public GenericSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Sandbox");

            // Samples are run by separate samples runner
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Fonts.MergeAndAddFont");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Pdfhtml.PdfHtmlResponsiveDesign");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Security.DecryptPdf");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Security.DecryptPdf2");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Security.EncryptPdf");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Security.EncryptWithCertificate");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Parse.ExtractStreams");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Annotations.RemoteGoto");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Annotations.RemoteGoToPage");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Split.SplitAndCount");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Signatures.Pades");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Signatures.Appearance");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Signatures.TwoPhase");

            // Not a sample classes
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Signatures.Utils");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Signatures.Clients");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Pdfhtml.Colorblindness");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Pdfhtml.Qrcodetag");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Pdfhtml.Headertagging");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Merge.Densemerger.PageVerticalAnalyzer");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Merge.Densemerger.PdfDenseMerger");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Pdfhtml.Formtagging");


            // TODO DEVSIX-3189
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.TableBorder");

            // TODO DEVSIX-3188
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.SplitRowAtEndOfPage");

            // TODO DEVSIX-3188
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.SplitRowAtSpecificRow");

            // TODO DEVSIX-3187
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.RepeatLastRows");

            // TODO DEVSIX-3187
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.RepeatLastRows2");

            // TODO DEVSIX-3326
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.SplittingNestedTable2");
            
            //TODO DEVSIX-6508 remove unnecessary makeFormField calls
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Acroforms.RemoveXFA");


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
            CompareTool compareTool = new CompareTool();

            if (xmlCompareList.Contains(sampleClass.FullName))
            {
                if (!compareTool.CompareXmls(dest, cmp))
                {
                    AddError("The XML structures are different.");
                }
            }
            else if (txtCompareList.Contains(sampleClass.FullName))
            {
                AddError(CompareTxt(dest, cmp));
            }
            else if (renderCompareList.Contains(sampleClass.FullName))
            {
                AddError(compareTool.CompareVisually(dest, cmp, outPath, "diff_"));
                AddError(compareTool.CompareLinkAnnotations(dest, cmp));
                AddError(compareTool.CompareDocumentInfo(dest, cmp));
            }
            else if (ignoredClassesMap.Keys.Contains(sampleClass.FullName))
            {
                AddError(compareTool.CompareVisually(dest, cmp, outPath, "diff_",
                    ignoredClassesMap[sampleClass.FullName]));
            }
            else
            {
                AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            }

            if (veraPdfValidateList.Contains(sampleClass.FullName))
            {
                AddError(new VeraPdfValidator().Validate(dest));
            }
        }

        private String CompareTxt(String dest, String cmp)
        {
            String errorMessage = null;
            Console.Out.WriteLine("Out txt: " + UrlUtil.GetNormalizedFileUriString(dest));
            Console.Out.WriteLine("Cmp txt: " + UrlUtil.GetNormalizedFileUriString(cmp)+ "\n");

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
