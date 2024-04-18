using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using iText.IO.Font;
using iText.Commons.Utils;
using iText.Licensing.Base;
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
            
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            RunSamples();
            LicenseKey.UnloadLicenses();

            ServicePointManager.SecurityProtocol = defaultSecurityProtocolType;
        }

        protected override void ComparePdf(string outPath, string dest, string cmp) {
        }

    }
}

public class TimedWebClient : WebClient
{
    protected override WebRequest GetWebRequest(Uri address)
    {
        var webRequest = base.GetWebRequest(address);
        webRequest.Timeout = 15000;
        return webRequest;
    }
}