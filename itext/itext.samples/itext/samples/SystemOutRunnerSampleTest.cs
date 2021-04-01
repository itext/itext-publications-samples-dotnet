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
using System.Text;
using iText.IO.Util;
using iText.License;
using iText.Test;

using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class SystemOutRunnerSampleTest : WrappedSamplesRunner
    {
        private TextWriter storedSystemOut;
        private FileStream stream;

        public SystemOutRunnerSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Sandbox.Logging.CounterDemoSystemOut");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
            
            storedSystemOut = Console.Out;
            
            // By default target dir is created during runSamples method. In this particular case we need this
            // action before due to overriding system out stream
            new FileInfo(GetDest()).Directory.Create();

            using (stream = File.Open((GetDest().Replace(".pdf", ".txt")), 
                    FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Console.SetOut(new StreamWriter(stream, Encoding.UTF8));
                RunSamples();
            }
            
            ResetLicense();
        }

        protected override void ComparePdf(String outPath, String dest, String cmp)
        {
            // Make sure that the stream's content is flushed
            Console.Out.Flush();
            
            Console.SetOut(storedSystemOut);
            String dest_txt = dest.Replace(".pdf", ".txt");
            String cmp_txt = cmp.Replace(".pdf", ".txt");
            
            Console.WriteLine("Out txt file: " + UrlUtil.GetNormalizedFileUriString(dest_txt));
            Console.WriteLine("Cmp txt file: " + UrlUtil.GetNormalizedFileUriString(cmp_txt)+ "\n");
            
            AddError(CompareTxt(dest_txt, cmp_txt));
        }

        private String CompareTxt(String dest, String cmp)
        {
            stream.Flush();
            
            String errorMessage = null;
            
            using (
                    StreamReader destReader = new StreamReader(new FileStream(dest, 
                            FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.UTF8),
                    cmpReader = new StreamReader(cmp, Encoding.UTF8))
            {
                int lineNumber = 1;
                String destLine = destReader.ReadLine();
                String cmpLine = cmpReader.ReadLine();
                while (destLine != null || cmpLine != null)
                {
                    if (destLine == null || cmpLine == null)
                    {
                        errorMessage = "The number of lines is different\n";
                        break;
                    }

                    if (!destLine.Equals(cmpLine))
                    {
                        errorMessage = "Txt files differ at line " + lineNumber
                                                                   + "\n See difference: cmp file: \""
                                                                   + cmpLine + "\"\n"
                                                                   + "target file: \"" + destLine + "\n";
                    }

                    destLine = destReader.ReadLine();
                    cmpLine = cmpReader.ReadLine();
                    lineNumber++;
                }
            }

            return errorMessage;
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