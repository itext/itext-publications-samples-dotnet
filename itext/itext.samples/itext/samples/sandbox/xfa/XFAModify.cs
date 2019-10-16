/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using iText.Forms;
using iText.Forms.Xfa;
using iText.Kernel.Pdf;
using iText.License;

namespace iText.Samples.Sandbox.Xfa
{
    public class XFAModify
    {
        public const String INPUT_PDF = "../../../resources/xfa/invoice.pdf";
        public const String DEST = "results/sandbox/xfa/XFAModify.pdf";

        public static void Main(String[] args)
        {
            // Load the license file to use XFA features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/all-products.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new XFAModify().ManipulatePdf();
        }

        public void ManipulatePdf()
        {

            // Create a PDF document instance
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(INPUT_PDF), new PdfWriter(DEST));

            // Load the DOM Document
            XfaForm xfa = PdfAcroForm.GetAcroForm(pdfDoc, false).GetXfaForm();
            XDocument domDoc = xfa.GetDomDocument();

            // The follwing 2 lines of code only work for the specific document
            // Access the Script Node of the DOM Document
            XElement template = domDoc.Descendants().First().Descendants().First().ElementsAfterSelf().First();
            XElement script = template.Descendants().First().Descendants().First().ElementsAfterSelf().First()
                .Descendants().First().ElementsAfterSelf().First().ElementsAfterSelf().First().ElementsAfterSelf()
                .First().ElementsAfterSelf().First().ElementsAfterSelf().First().Descendants().First();

            // Update the script message
            String message = "xfa.host.messageBox(\"XFA SCRIPT Message!!!\")";
            script.SetValue(message);

            // Write XFA back to PDF Document
            xfa.SetDomDocument(domDoc);
            xfa.Write(pdfDoc);
            pdfDoc.Close();
        }
    }
}
