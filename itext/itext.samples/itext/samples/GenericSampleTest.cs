/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Kernel.Utils;
using iText.License;
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
                "iText.Samples.Sandbox.Tagging.WalkTheTree"
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
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Htmlsamples");
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
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Logging.CounterDemoSystemOut");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Merge.MergeAndCount");
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter05.C05E03_Invitations");
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter07.C07E04_CreateFromURL");
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter07.C07E05_CreateFromURL2");
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter07.C07E06_CreateFromURL3");

            // Not a sample classes
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Pdfhtml.Colorblindness");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Pdfhtml.Qrcodetag");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Pdfhtml.Headertagging");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Merge.Densemerger.PageVerticalAnalyzer");
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Merge.Densemerger.PdfDenseMerger");
            
            // Should not be run due to falling on different systems with different system fonts
            searchConfig.IgnorePackageOrClass("iText.Samples.Htmlsamples.Chapter06.C06E03_SystemFonts");

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

        protected override String GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }

            int i = dest.LastIndexOf("/", StringComparison.Ordinal);
            int j = dest.LastIndexOf("/results", StringComparison.Ordinal) + 9;
            return "../../resources/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
        }

        private String CompareTxt(String dest, String cmp)
        {
            String errorMessage = null;

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