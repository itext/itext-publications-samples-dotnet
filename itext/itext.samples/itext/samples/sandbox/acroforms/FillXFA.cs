using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Forms.Xfa;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // FillXFA.cs
    // 
    // This example demonstrates how to fill an XFA (XML Forms Architecture) form in a PDF document.
    // It uses an external XML file as the data source to populate the form fields.
    // 
    // Requires pdfXFA addon
 
    public class FillXFA
    {
        public static readonly String DEST = "results/sandbox/acroforms/purchase_order_filled.pdf";

        public static readonly String SRC = "../../../resources/pdfs/purchase_order.pdf";
        public static readonly String XML = "../../../resources/xml/data.xml";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillXFA().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfdoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfdoc, true);

            XfaForm xfa = form.GetXfaForm();

            // Method fills this object with XFA data under datasets/data.
            xfa.FillXfaForm(new FileStream(XML, FileMode.Open, FileAccess.Read));
            xfa.Write(pdfdoc);

            pdfdoc.Close();
        }
    }
}