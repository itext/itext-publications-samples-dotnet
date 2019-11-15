using System;
using System.IO;
using iText.Forms.Xfdf;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FillFormListXfdf
    {
        public static readonly String sourceFolder = "../../resources/pdfs/";

        public static readonly String DEST = "../../results/sandbox/acroforms/listInSetField.pdf";


        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillFormListXfdf().listInSetField(DEST);
        }

        // Currently iText xfdf implementation works with the fields in the following way: when the <value> tag with text
        // contents is found, it is considered to be the value of the field. All other <value> tags are ignored.
        public void listInSetField(String dest)
        {
            String pdfForm = sourceFolder + "simpleRegistrationForm.pdf";
            String xfdf = sourceFolder + "list_register.xfdf";
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