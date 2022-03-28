/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2022 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using iText.Kernel.Geom;
using iText.Samples.Signatures.Chapter04;
using iText.Test;
using NUnit.Framework;
using Org.BouncyCastle.X509;

namespace iText.Samples.Signatures.Testrunners
{
    [TestFixtureSource("Data")]
    public class ClientServerSigningTest : WrappedSamplesRunner
    {
        private static readonly IDictionary<int, IList<Rectangle>> ignoredAreaMap;

        static ClientServerSigningTest()
        {
            ignoredAreaMap = new Dictionary<int, IList<Rectangle>>();
            ignoredAreaMap.Add(1, new List<Rectangle>(new[]
            {
                new Rectangle(36, 648, 200, 100)
            }));
        }

        public ClientServerSigningTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter04.C4_07_ClientServerSigning");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }
        
        [Test, Description("{0}")]
        public virtual void Test()
        {
            SecurityProtocolType defaultSecurityProtocolType = ServicePointManager.SecurityProtocol;
            
            // Set security protocol version to TLS 1.2 to avoid https connection issues
            ServicePointManager.SecurityProtocol = (SecurityProtocolType) 3072;
            
            RunSamples();
            
            ServicePointManager.SecurityProtocol = defaultSecurityProtocolType;
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            String[] resultFiles = GetResultFiles(sampleClass);
            for (int i = 0; i < resultFiles.Length; i++)
            {
                String currentDest = dest + resultFiles[i];
                String currentCmp = cmp + resultFiles[i];

                AddError(new CustomSignatureTest().CheckForErrors(currentDest, currentCmp,
                    outPath, ignoredAreaMap));
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

        private class CustomSignatureTest : SignatureTestHelper
        {
            protected internal override void InitKeyStoreForVerification(List<X509Certificate> ks)
            {
                base.InitKeyStoreForVerification(ks);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(C4_07_ClientServerSigning.CERT);
                request.Method = WebRequestMethods.Http.Get;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                X509Certificate chain = new X509CertificateParser().ReadCertificate(response.GetResponseStream());
                ks.Add(chain);
            }
        }
    }
}
