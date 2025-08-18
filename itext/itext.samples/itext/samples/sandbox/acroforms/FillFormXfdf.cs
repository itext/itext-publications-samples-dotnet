using System;
using System.IO;
using iText.Forms.Xfdf;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // FillFormXfdf.cs
    // 
    // This example demonstrates how to fill a PDF form with data from an XFDF file.
    // It reads both the PDF form and the XFDF data file, and merges the form data into the PDF.
 
    public class FillFormXfdf
    {
        public static readonly String sourceFolder = "../../../resources/pdfs/";

        public static readonly String DEST = "results/sandbox/acroforms/setFields.pdf";


        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillFormXfdf().setFields(DEST);
        }

        // Currently iText xfdf implementation works in the following way:
        // the XFDF file is used to insert data from it directly into the PDF.
        public void setFields(String dest)
        {
            String pdfForm = sourceFolder + "simpleRegistrationForm.pdf";
            String xfdf = sourceFolder + "register.xfdf";
            PdfDocument pdfDocument = new PdfDocument(
                new PdfReader(new FileStream(pdfForm, FileMode.Open, FileAccess.Read)),
                new PdfWriter(new FileStream(dest, FileMode.Create)));
            XfdfObjectFactory factory = new XfdfObjectFactory();
            XfdfObject xfdfObject = factory.CreateXfdfObject(new FileStream(xfdf, FileMode.Open, FileAccess.Read));
            xfdfObject.MergeToPdf(pdfDocument, pdfForm);
            pdfDocument.Close();
        }
    }
}