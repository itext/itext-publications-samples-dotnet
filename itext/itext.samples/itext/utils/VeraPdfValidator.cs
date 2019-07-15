/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using iText.IO.Util;

namespace iText.Utils
{
    public class VeraPdfValidator
    {
        private String cliCommand = "java -classpath \"<projectPath>\\etc;<libPath>\\*\" -Dfile.encoding=UTF8 " +
                                    "-XX:+IgnoreUnrecognizedVMOptions -Dapp.name=\"VeraPDF validation GUI\" " +
                                    "-Dapp.repo=\"<libPath>\" -Dapp.home=\"<projectPath>\\\" " +
                                    "-Dbasedir=\"<projectPath>\\\" org.verapdf.apps.GreenfieldCliWrapper ";
        
        public String Validate(String dest)
        {
            Process p = new Process();
            String currentCommand = cliCommand.Replace("<projectPath>", "..\\..\\")
                              .Replace("<libPath>", "..\\..\\lib\\VeraPdf");
            
            String currentDest = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "\\" + dest);
            
            p.StartInfo = new ProcessStartInfo("cmd", "/c" + currentCommand + currentDest);
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            
            String result = HandleVeraPdfOutput(p, currentDest);
            p.WaitForExit();

            return result;
        }
        
        private string HandleVeraPdfOutput(Process p, String dest)
        {
            StringBuilder standardOutput = new StringBuilder();
            StringBuilder standardError = new StringBuilder();

            while (!p.HasExited)
            {
                standardOutput.Append(p.StandardOutput.ReadToEnd());
                standardError.Append(p.StandardError.ReadToEnd());
            }

            if (!String.IsNullOrEmpty(standardError.ToString()))
            {
                return "VeraPDF execution failed: " + standardError;
            }

            return GenerateReport(standardOutput.ToString(), dest, true);
        }

        private String GenerateReport(String output, String dest, bool toReportSuccess)
        {
            XmlDocument document = new XmlDocument();

            try
            {
                document.LoadXml(output.Trim());
            }
            catch (XmlException exc)
            {
                return "VeraPDF verification results parsing failed: " + exc.Message;
            }

            String reportDest = dest.Replace(".pdf", ".xml");
            
            XmlAttributeCollection detailsAttributes = document.GetElementsByTagName("details")[0].Attributes;
            
            if (!detailsAttributes["failedRules"].Value.Equals("0") ||
                !detailsAttributes["failedChecks"].Value.Equals("0"))
            {
                WriteToFile(output, reportDest);
                return "VeraPDF verification failed. See verification results: file:///"
                       + UrlUtil.ToNormalizedURI(reportDest).AbsolutePath;
            }
            
            if (toReportSuccess)
            {
                WriteToFile(output, reportDest);
                Console.WriteLine("VeraPDF verification finished. See verification report: file:///" 
                                  + UrlUtil.ToNormalizedURI(reportDest).AbsolutePath);
            }

            return null;
        }

        private void WriteToFile(String output, String reportDest)
        {
            using (FileStream stream = File.Create(reportDest))
            {
                stream.Write(new UTF8Encoding(true).GetBytes(output),
                    0, output.Length);
            }
        }
    }
}