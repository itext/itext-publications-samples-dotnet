using System;
using System.IO;
using System.Text;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms 
{
    public class MultiLineFieldCount 
    {
        public static readonly string DEST = "results/sandbox/acroforms/multiLineFieldCount.pdf";
        public static readonly string SRC = "../../../resources/pdfs/multiline.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new MultiLineFieldCount().ManipulatePdf(SRC, DEST);
        }

        private void ManipulatePdf(String src, String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
            PdfAcroForm acroForm = PdfFormCreator.GetAcroForm(pdfDoc, false);
            
            PassData(acroForm);
            
            pdfDoc.Close();
        }

        private void PassData(PdfAcroForm acroForm) 
        {
            String character = " *";
            StringBuilder sb = new StringBuilder();
            
            foreach (String name in acroForm.GetAllFormFields().Keys) 
            {
                for (int i = 0; i < GetInfo(character, acroForm, name); i++) 
                {
                    sb.Append(character);
                }
                
                String filler = sb.ToString();
                PdfFormField formField = acroForm.GetField(name);
                formField.SetValue(name + filler);
            }
        }

        private int GetInfo(String character, PdfAcroForm form, String name) 
        {
            PdfFormField field = form.GetField(name);
            PdfFont font = field.GetFont();
            FontMetrics fontMetrics = font.GetFontProgram().GetFontMetrics();
            float fontSize = field.GetFontSize();
            
            if (fontSize == 0) 
            {
                return 1000;
            }
            
            Rectangle rectangle = field.GetWidgets()[0].GetRectangle().ToRectangle();
            
            // Factor here is a leading value. We calculate it by subtracting lower left corner value from
            // the upper right corner value of the glyph bounding box
            float factor = (fontMetrics.GetBbox()[3] - fontMetrics.GetBbox()[1]) / 1000f;
            
            int rows = (int) Math.Round(rectangle.GetHeight() / (fontSize * factor) + 0.5f);
            int columns = (int) Math.Round(rectangle.GetWidth() / font.GetWidth(character, fontSize) + 0.5f);
            
            return rows * columns;
        }
    }
}
