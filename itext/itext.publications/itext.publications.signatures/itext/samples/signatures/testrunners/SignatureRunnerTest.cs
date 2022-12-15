/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2022 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using iText.License;
using System.Reflection;
using iText.IO.Font;
using iText.Kernel.Geom;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples.Signatures.Testrunners
{
    [TestFixtureSource("Data")]
    public class SignatureRunnerTest : WrappedSamplesRunner
    {
        private static readonly IDictionary<String, IList<Rectangle>> classAreaMap;

        static SignatureRunnerTest()
        {
            classAreaMap = new Dictionary<string, IList<Rectangle>>();
            classAreaMap.Add("iText.Samples.Signatures.Chapter02.C2_01_SignHelloWorld",
                new List<Rectangle>(new[] {new Rectangle(36, 648, 200, 100)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter02.C2_02_SignHelloWorldWithTempFile",
                new List<Rectangle>(new[] {new Rectangle(36, 648, 200, 100)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter02.C2_03_SignEmptyField",
                new List<Rectangle>(new[] {new Rectangle(46, 472, 287, 255)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter02.C2_04_CreateEmptyField",
                new List<Rectangle>(new[] {new Rectangle(72, 632, 200, 100)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter02.C2_05_CustomAppearance",
                new List<Rectangle>(new[] {new Rectangle(46, 472, 287, 255)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter02.C2_06_SignatureAppearance",
                new List<Rectangle>(new[] {new Rectangle(46, 472, 287, 255)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter02.C2_07_SignatureAppearances",
                new List<Rectangle>(new[] {new Rectangle(46, 472, 287, 255)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter02.C2_08_SignatureMetadata",
                new List<Rectangle>(new[] {new Rectangle(46, 472, 287, 255)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter03.C3_01_SignWithCAcert",
                new List<Rectangle>(new[] {new Rectangle(36, 648, 200, 100)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter04.C4_09_DeferredSigning",
                new List<Rectangle>(new[] {new Rectangle(36, 748, 200, 100)}));
            classAreaMap.Add("iText.Samples.Signatures.Chapter04.C4_08_ServerClientSigning",
                new List<Rectangle>(new[] {new Rectangle(36, 748, 200, 100)}));
        }

        public SignatureRunnerTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Signatures.Chapter02");
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Signatures.Chapter03");
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Signatures.Chapter04");

            // Samples are run by separate samples runner
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter02.C2_12_LockFields");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter02.C2_10_SequentialSignatures");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter02.C2_09_SignatureTypes");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter02.C2_11_SignatureWorkflow");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter04.C4_07_ClientServerSigning");

            // Samples require a valid certificate which is issued by the service that provides CRL access point
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_02_GetCrlUrl");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_03_SignWithCRLDefaultImp");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_04_SignWithCRLOnline");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_05_SignWithCRLOffline");

            // Samples require a valid certificate which is issued by the service that provides OCSP
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_06_GetOcspUrl");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_07_SignWithOCSP");

            // Samples require a valid certificate which is issued by the service that provides TSA access point
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_08_GetTsaUrl");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_09_SignWithTSA");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_10_SignWithTSAEvent");
            searchConfig.IgnorePackageOrClass("iText.Samples.Signatures.Chapter03.C3_12_SignWithEstimatedSize");
            
            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }
        
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
            IList<Rectangle> ignoredAreas = classAreaMap[sampleClass.FullName];
            IDictionary<int, IList<Rectangle>> ignoredAreasMap = new Dictionary<int, IList<Rectangle>>();
            ignoredAreasMap.Add(1, ignoredAreas);

            String[] resultFiles = GetResultFiles(sampleClass);
            for (int i = 0; i < resultFiles.Length; i++)
            {
                String currentDest = dest + resultFiles[i];
                String currentCmp = cmp + resultFiles[i];
                try
                {
                    AddError(new SignatureTestHelper().CheckForErrors(currentDest, currentCmp, outPath, ignoredAreasMap));
                }
                catch (Exception exc)
                {
                    AddError("Exception has been thrown: " + exc.Message);
                }
            }
        }

        protected override String GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }

            String destRootText = "/results";
            int i = dest.LastIndexOf("/", StringComparison.Ordinal);
            int j = dest.LastIndexOf(destRootText, StringComparison.Ordinal) + destRootText.Length;
            return "../../../cmpfiles/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
        }

        private static String[] GetResultFiles(Type c)
        {
            try
            {
                FieldInfo field = c.GetField("RESULT_FILES");
                if (field == null)
                {
                    return null;
                }

                Object obj = field.GetValue(null);
                if (obj == null || !(obj is String[]))
                {
                    return null;
                }

                return (String[]) obj;
            }
            catch (Exception)
            {
                return null;
            }
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