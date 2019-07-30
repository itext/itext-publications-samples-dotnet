using System;
using System.IO;
using iText.Forms;
using iText.Forms.Xfa;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FillXFA2
    {
        public static readonly String DEST = "../../results/sandbox/acroforms/xfa_form_poland_filled.pdf";

        public static readonly String SRC = "../../resources/pdfs/xfa_form_poland.pdf";
        public static readonly String XML = "../../resources/xml/xfa_form_poland.xml";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new FillXFA2().ManipulatePdf(DEST);
        }
        
        protected void ManipulatePdf(string dest) {
            PdfReader reader = new PdfReader(SRC);
            reader.SetUnethicalReading(true);
            PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(DEST));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            XfaForm xfa = form.GetXfaForm();
            xfa.FillXfaForm(new FileStream(XML, FileMode.Open, FileAccess.Read));
            xfa.Write(pdfDoc);
            pdfDoc.Close();
        }
    }
}
