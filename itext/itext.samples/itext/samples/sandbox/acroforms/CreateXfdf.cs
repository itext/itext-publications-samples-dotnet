using System;
using System.IO;
using iText.Forms.Xfdf;
using iText.Kernel.Pdf;


namespace iText.Samples.Sandbox.Acroforms
{
   
    // CreateXfdf.cs
    // 
    // This example demonstrates how to extract form data from a PDF document and save it as an XFDF file.
    // XFDF (XML Forms Data Format) is used to represent form data in a structured XML format.
 
    public class CreateXfdf
    {
        public static readonly String sourceFolder = "../../../resources/pdfs/";
        
        public static readonly String DEST = "results/sandbox/acroforms/createXfdf.xfdf";


        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CreateXfdf().createXfdf(DEST);
        }

        // Currently iText xfdf implementation works in the following way:
        // data from Pdf form could be received as file with the XFDF file extension.
        public void createXfdf(String dest)
        {

            String pdfDocumentName = "createXfdf.pdf";

            PdfDocument pdfDocument = new PdfDocument(new PdfReader(new FileStream
                (sourceFolder + pdfDocumentName, FileMode.Open, FileAccess.Read)));

            XfdfObjectFactory factory = new XfdfObjectFactory();
            XfdfObject xfdfObject = factory.CreateXfdfObject(pdfDocument, pdfDocumentName);
            xfdfObject.WriteToFile(dest);

            pdfDocument.Close();
        }
    }
}