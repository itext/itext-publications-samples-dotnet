/*
This file is part of the iText (R) project.
Copyright (c) 1998-2021 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.Kernel.Utils;
using iText.License;
using iText.Samples.Sandbox.Security;
using iText.Test;
using NUnit.Framework;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class EncryptWithCertificateSampleTest : WrappedSamplesRunner
    {
        public static readonly String PRIVATE = "../../../resources/encryption/test.p12";

        public EncryptWithCertificateSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Sandbox.Security.EncryptWithCertificate");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            LicenseKey.UnloadLicenses();
            RunSamples();
        }

        protected override void ComparePdf(String outPath, String dest, String cmp)
        {
            CompareTool compareTool = new CompareTool();
            AsymmetricKeyParameter privateKey = GetPrivateKey();

            compareTool.GetOutReaderProperties().SetPublicKeySecurityParams(
                GetPublicCertificate(EncryptWithCertificate.PUBLIC), privateKey);
            compareTool.GetCmpReaderProperties().SetPublicKeySecurityParams(
                GetPublicCertificate(EncryptWithCertificate.PUBLIC), privateKey);
            compareTool.EnableEncryptionCompare();
            
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            AddError(compareTool.CompareDocumentInfo(dest, cmp));
        }

        private AsymmetricKeyParameter GetPrivateKey()
        {
            using (FileStream stream = new FileStream(PRIVATE, FileMode.Open, FileAccess.Read))
            {
                Pkcs12Store keyStore = new Pkcs12Store(stream, "kspass".ToCharArray());
                return keyStore.GetKey("sandbox").Key;
            }
        }

        public X509Certificate GetPublicCertificate(String path)
        {
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                X509CertificateParser parser = new X509CertificateParser();
                X509Certificate readCertificate = parser.ReadCertificate(stream);
                return readCertificate;
            }
        }
    }
}