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
using iText.Kernel.Utils;
using iText.License;
using iText.Test;
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
        private List<string> renderCompareList = new List<string>();

        /**
         * List of samples, which require xml files comparison
         */
        private List<string> xmlCompareList = new List<string>(new[] {"iText.Samples.Sandbox.Acroforms.ReadXFA"});

        public SamplesWrapperTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Sandbox.Acroforms");
            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-typography.xml");
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
            else
            {
                AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
                AddError(compareTool.CompareDocumentInfo(dest, cmp));
            }
        }

        protected override String GetCmpPdf(String dest) 
        {
            if (dest == null) {
                return null;
            }
            int i = dest.LastIndexOf("/", StringComparison.Ordinal);
            int j = dest.LastIndexOf("/results", StringComparison.Ordinal) + 9;
            return "../../resources/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
        }

        private void ResetLicense()
        {
            try {
                FieldInfo validatorsField = typeof(LicenseKey).GetField("validators", 
                    BindingFlags.NonPublic | BindingFlags.Static);
                validatorsField.SetValue(null, null);
                FieldInfo versionField = typeof(Kernel.Version).GetField("version",
                    BindingFlags.NonPublic | BindingFlags.Static);
                versionField.SetValue(null, null);
            } 
            catch
            {
                
                // It does nothing, because there can be no loaded license,
                // so no exception handling required
            }
        }
    }
}