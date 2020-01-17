/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples.Signatures.Testrunners
{
    [TestFixtureSource("Data")]
    public class OutputTest : WrappedSamplesRunner
    {
        private TextWriter oldSysOut;
        private TextWriter newSysOut;

        public OutputTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter01.C1_01_DigestDefault");
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter01.C1_02_DigestBC");
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter05.C5_01_SignatureIntegrity");
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter05.C5_02_SignatureInfo");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            SetupSystemOutput();

            RunSamples();

            ResetSystemOutput();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            Console.Out.Flush();
            String sysOut = newSysOut.ToString().Replace("\r\n", "\n");

            // The 1st and the last output lines are created by samples runner, so they should be removed
            String[] temp = sysOut.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
            String[] outputLines = new String[temp.Length - 2];

            outputLines = temp.Where((value, index) =>
                index != 0 && index != temp.Length - 1).ToArray();

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

        private void SetupSystemOutput()
        {
            oldSysOut = Console.Out;
            newSysOut = new StringWriter();
            Console.SetOut(newSysOut);
        }

        private void ResetSystemOutput()
        {
            Console.SetOut(oldSysOut);
        }
    }
}