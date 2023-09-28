using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using iText.Kernel.Geom;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples.Signatures.Testrunners
{
    [TestFixtureSource("Data")]
    public class SignatureTypesTest : WrappedSamplesRunner
    {
        private static readonly IDictionary<int, IList<Rectangle>> ignoredAreaMap;

        private static readonly String EXPECTED_ERROR_TEXT =
            "\nresults/signatures/chapter02/hello_level_1_annotated_wrong.pdf:"
            + "\n\"sig\" signature integrity is invalid\n\n";

        static SignatureTypesTest()
        {
            ignoredAreaMap = new Dictionary<int, IList<Rectangle>>();
            ignoredAreaMap.Add(1, new List<Rectangle>(new[]
            {
                new Rectangle(72, 675, 170, 20),
                new Rectangle(72, 725, 170, 20)
            }));
        }

        public SignatureTypesTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Signatures.Chapter02.C2_09_SignatureTypes");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }
        
        [Test, Description("{0}")]
        public virtual void Test()
        {
            RunSamples();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            String[] resultFiles = GetResultFiles(sampleClass);
            StringBuilder errorTemp = new StringBuilder();
            for (int i = 0; i < resultFiles.Length; i++)
            {
                String currentDest = dest + resultFiles[i];
                String currentCmp = cmp + resultFiles[i];
                try
                {
                    /* iText doesn't recognize invalidated signatures in "hello_level_3_annotated.pdf",
                     * "hello_level_4_annotated.pdf", "hello_level_1_text.pdf", "hello_level_4_double.pdf"
                     * files, because we don't check changes in new revisions against old signatures
                     * (permissions, certifications, content changes),
                     *  however signatures themselves are not broken.
                     */
                    String result = new SignatureTestHelper()
                        .CheckForErrors(currentDest, currentCmp, outPath, ignoredAreaMap);

                    if (result != null)
                    {
                        errorTemp.Append(result);
                    }
                }
                catch (Exception exc)
                {
                    AddError("Exception has been thrown: " + exc.Message);
                }
            }

            String errorText = errorTemp.ToString();
            if (errorText.Equals("") || !errorText.Contains(EXPECTED_ERROR_TEXT))
            {
                errorText += "\n'hello_level_1_annotated_wrong.pdf' file's signature "
                             + "was expected to be invalid.\n\n";
            }
            else
            {
                
                // Expected error should be ignored
                errorText = errorText.Replace(EXPECTED_ERROR_TEXT, "");
            }

            AddError(errorText);
        }

        protected override String GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }

            int i = dest.LastIndexOf("/", StringComparison.Ordinal);
            int j = dest.LastIndexOf("/results", StringComparison.Ordinal) + 9;
            return "../../../resources/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
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
    }
}
