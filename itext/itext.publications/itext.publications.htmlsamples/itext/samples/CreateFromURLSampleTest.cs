/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2022 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.License;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class CreateFromURLSampleTest : WrappedSamplesRunner
    {
        public CreateFromURLSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Htmlsamples.Chapter07.C07E04_CreateFromURL");
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Htmlsamples.Chapter07.C07E05_CreateFromURL2");
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Htmlsamples.Chapter07.C07E06_CreateFromURL3");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }
        
        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            SecurityProtocolType defaultSecurityProtocolType = ServicePointManager.SecurityProtocol;
            
            // Set security protocol version to TLS 1.2 to avoid https connection issues
            ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072;
            
            FontCache.ClearSavedFonts();
            FontProgramFactory.ClearRegisteredFonts();
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.xml");
            RunSamples();
            ResetLicense();

            ServicePointManager.SecurityProtocol = defaultSecurityProtocolType;
        }

        protected override void ComparePdf(string outPath, string dest, string cmp) {
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