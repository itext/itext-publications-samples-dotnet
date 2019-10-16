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
using System.Xml.Linq;
using iText.Forms;
using iText.Forms.Xfa;
using iText.Kernel.Pdf;
using iText.License;

namespace iText.Samples.Sandbox.Xfa
{
    public class XFACalculate
    {
        public const String INPUT_PDF = "../../../resources/xfa/invoice.pdf";
        public const String DEST = "results/sandbox/xfa/XFACalculate.pdf";

        public static void Main(String[] args)
        {
            // Load the license file to use XFA features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/all-products.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new XFACalculate().ManipulatePdf();
        }

        public void ManipulatePdf()
        {

            // Create a pdf document instance
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(INPUT_PDF), new PdfWriter(DEST));

            // Load the DOM Document
            XfaForm xfa = PdfAcroForm.GetAcroForm(pdfDoc, false).GetXfaForm();
            XDocument domDoc = xfa.GetDomDocument();

            // This works for the specific document
            // Generate the list of calculate amount Nodes
            IEnumerable<XElement> calcElements = findCalc(domDoc);

            // Update calculate value
            foreach (XElement element in calcElements)
            {
                XElement calc = element.Descendants().First();
                String curVal = calc.Value;
                calc.Value = "2*" + curVal;
                Console.WriteLine(calc.Value);
            }

            // Write XFA back to PDF Document
            xfa.SetDomDocument(domDoc);
            xfa.Write(pdfDoc);
            pdfDoc.Close();
        }

        // Searches the Dom Document to access the calculate amount Nodes
        public IEnumerable<XElement> findCalc(XDocument doc)
        {
            IEnumerable<XElement> calc = doc.Root.Descendants().Where(t =>
                t.Name.LocalName.Equals("calculate") && t.Parent.Attribute("name").Value.Equals("Amount")).ToList();

            return calc;
        }
    }
}
