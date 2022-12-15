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
using NUnit.Framework;
using iText.Kernel.Utils;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.License;
using iText.Test;

namespace iText.Highlevel {
    /// <summary>This class expects samples with KEY field in which path to the license file is set.</summary>
    [TestFixtureSource("Data")]
    public class HighLevelWrapperWithLicenseTest : WrappedSamplesRunner {

        /*[Parameterized.Parameters(Name = "{index}: {0}")]*/

        public HighLevelWrapperWithLicenseTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Highlevel.Chapter02.C02E15_ShowTextAlignedKerned");
            searchConfig.AddClassToRunnerSearchPath("iText.Highlevel.Chapter01.C01E05_Czech_Russian_Korean_Right");
            searchConfig.AddClassToRunnerSearchPath("iText.Highlevel.Chapter01.C01E06_Czech_Russian_Korean_Unicode");
            return GenerateTestsList(Assembly.GetExecutingAssembly(),searchConfig);
        }

        [NUnit.Framework.Timeout(60000)]
        [NUnit.Framework.Test]
        public virtual void Test() {
            ResetLicense();
            this.InitClass();
            sampleClass.GetField("KEY").SetValue(null, Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-typography.xml");
            RunSamples();
        }
		
		protected override string GetCmpPdf(String dest) {
            if (dest == null) {
                return null;
            }
            int i = dest.LastIndexOf("/");
            int j = dest.IndexOf("results") + 8;
            return "../../../cmpfiles/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
        }

        protected override void ComparePdf(String outPath, String dest, String cmp) {
            CompareTool compareTool = new CompareTool();
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            AddError(compareTool.CompareDocumentInfo(dest, cmp));
        }

        protected void InitClass()
        {
            if (sampleClass == null)
            {
                try
                {
                    String sampleClassName = sampleClassParams.ToString();
                    this.sampleClass = Type.GetType(sampleClassName, true);
                }
                catch (Exception e)
                {
                    throw new TypeLoadException(sampleClassParams.GetType().ToString());

                }
            }
        }

        private void ResetLicense() {
            try {
                FieldInfo validatorsField = typeof(LicenseKey).GetField("validators", BindingFlags.NonPublic | BindingFlags.Static);
                validatorsField.SetValue(null, null);
                FieldInfo versionField = typeof(iText.Kernel.Version).GetField("version", BindingFlags.NonPublic | BindingFlags.Static);
                versionField.SetValue(null, null);
            } catch {
            }
        }
    }
}
