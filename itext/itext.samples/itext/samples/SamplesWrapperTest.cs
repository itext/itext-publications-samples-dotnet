/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */
using System;
using System.Collections.Generic;
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
    public class SamplesWrapperTest : WrappedSamplesRunner
    {
        private string errorMessage;

        /**
         * List of samples, which should be validated visually and by links annotations on corresponding pages
         */
        private List<string> renderCompareList = new List<string>()
        {
            "iText.Samples.Sandbox.Signatures.SignatureExample"
        };

        /**
         * List of samples, which require xml files comparison
         */
        private List<string> xmlCompareList = new List<string>(new[] {"iText.Samples.Sandbox.Acroforms.ReadXFA", "iText.Samples.Sandbox.Acroforms.CreateXfdf"});

        /**
         * List of samples, which require VeraPDF file comparison
         */
        private List<string> veraPdfValidateList = new List<string>(
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
        private static IDictionary<String, IDictionary<int, IList<Rectangle>>> ignoredClassesMap;
        
        static  SamplesWrapperTest() {
            Rectangle latinClassIgnoredArea = new Rectangle(30, 539, 250, 13);
            IList<Rectangle> rectangles = JavaUtil.ArraysAsList(latinClassIgnoredArea);
            IDictionary<int, IList<Rectangle>> ignoredAreasMap = new Dictionary<int, IList<Rectangle>>();
            ignoredAreasMap.Add(1, rectangles);
            ignoredClassesMap = new Dictionary<String, IDictionary<int, IList<Rectangle>>>();
            ignoredClassesMap.Add("iText.Samples.Sandbox.Typography.Latin.LatinSignature", ignoredAreasMap);
        }
        
        public SamplesWrapperTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Sandbox");

            // TODO DEVSIX-3189
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.TableBorder");
            
            // TODO DEVSIX-3188
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.SplitRowAtEndOfPage");
            
            // TODO DEVSIX-3188
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.SplitRowAtSpecificRow");
            
            // TODO DEVSIX-3189
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.RepeatLastRows");
            
            // TODO DEVSIX-3187
            searchConfig.IgnorePackageOrClass("iText.Samples.Sandbox.Tables.RepeatLastRows2");

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