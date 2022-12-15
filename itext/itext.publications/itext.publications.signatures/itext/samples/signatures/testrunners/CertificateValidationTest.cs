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
using System.Reflection;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples.Signatures.Testrunners
{
    [TestFixtureSource("Data")]
    public class CertificateValidationTest : WrappedSamplesRunner
    {
        private TextWriter writer;

        public CertificateValidationTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter05.C5_03_CertificateValidation");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }
        
        [Test, Description("{0}")]
        public virtual void Test()
        {
            RunSamples();
        }

        protected override void InitClass()
        {
            base.InitClass();
            SetSampleOutStream(sampleClass);
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            String sysOut = writer.ToString().Replace("\r\n", "\n");

            String[] outputLines = sysOut.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

            String[] expectedLines = GetStringField(sampleClass, "EXPECTED_OUTPUT")
                .Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < outputLines.Length; ++i)
            {
                String line = outputLines[i];
                if (!line.Trim().Equals(expectedLines[i].Trim()))
                {
                    AddError(String.Format("Unexpected output at line {0}.\nExpected: {1}\ngot: {2}",
                        i + 1, expectedLines[i], outputLines[i]));
                }
            }
        }

        private void SetSampleOutStream(Type c)
        {
            try
            {
                FieldInfo field = c.GetField("OUT_STREAM");
                if (field == null)
                {
                    return;
                }

                Object obj = field.GetValue(null);
                if (obj == null || !(obj is TextWriter))
                {
                    return;
                }

                writer = new StringWriter();
                field.SetValue(c, writer);
            }
            catch (Exception e)
            {
                AddError(e.Message);
            }
        }
    }
}