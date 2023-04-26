using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddFieldAndKids {
        public static readonly String DEST = "results/sandbox/stamper/add_field_and_kids.pdf";
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddFieldAndKids().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            PdfFormField personal = new NonTerminalFormFieldBuilder(pdfDoc, "personal")
                .CreateNonTerminalFormField();
            PdfTextFormField name = new TextFormFieldBuilder(pdfDoc, "name")
                .SetWidgetRectangle(new Rectangle(36, 760, 108, 30)).CreateText();
            name.SetValue("");
            personal.AddKid(name);
            PdfTextFormField password = new TextFormFieldBuilder(pdfDoc, "password")
                .SetWidgetRectangle(new Rectangle(150, 760, 300, 30)).CreateText();
            password.SetValue("");
            personal.AddKid(password);
            PdfAcroForm.GetAcroForm(pdfDoc, true).AddField(personal, pdfDoc.GetFirstPage());
            
            pdfDoc.Close();
        }
    }
}
