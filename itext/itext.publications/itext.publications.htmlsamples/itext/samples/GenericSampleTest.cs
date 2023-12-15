/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2022 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using iText.IO.Font;
using iText.Kernel.Utils;
using iText.License;
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
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.xml");
            RunSamples();
            ResetLicense();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            CompareTool compareTool = new CompareTool();

            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            AddError(compareTool.CompareDocumentInfo(dest, cmp));
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