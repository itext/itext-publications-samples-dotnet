/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
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

namespace Tutorial {
    [TestFixtureSource("Data")]
    public class JumpStartWrapperTest : WrappedSamplesRunner {

        public JumpStartWrapperTest(RunnerParams runnerParams) : base(runnerParams) {
        }

        public static ICollection<TestFixtureData> Data() {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("Tutorial");
            searchConfig.IgnorePackageOrClass("Tutorial.Chapter06.C06E09_FillOutFlattenAndMergeForms");
            searchConfig.IgnorePackageOrClass("Tutorial.JumpStartWrapperTest");
            searchConfig.IgnorePackageOrClass("Tutorial.C06E09_FillOutFlattenAndMergeFormsWrapperTest");
#if !NETSTANDARD2_0
            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
#else
            return GenerateTestsList(typeof(JumpStartWrapperTest).GetTypeInfo().Assembly, searchConfig);
#endif
        }
        
        [NUnit.Framework.Test]
        public virtual void Test() {
            ResetLicense();
            RunSamples();
        }

        protected override void ComparePdf(String outPath, String dest, String cmp) {
            CompareTool compareTool = new CompareTool();
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            AddError(compareTool.CompareDocumentInfo(dest, cmp));
        }
        
        protected override string GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }
            int i = dest.LastIndexOf("/");
            int j = dest.IndexOf("results") + 8;
            return "../../../cmpfiles/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
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
