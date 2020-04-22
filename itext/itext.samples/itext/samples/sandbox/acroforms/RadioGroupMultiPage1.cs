using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Acroforms
{
    public class RadioGroupMultiPage1
    {
        public static readonly String DEST = "results/sandbox/acroforms/radio_group_multi_page1.pdf";

        public static readonly String[] LANGUAGES = {"English", "German", "French", "Spanish", "Dutch"};

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RadioGroupMultiPage1().ManipulatePdf(DEST);
        }
        
        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            Rectangle rect = new Rectangle(40, 788, 20, 18);
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            
            // Radio buttons will be added to this radio group
            PdfButtonFormField radioGroup = PdfFormField.CreateRadioGroup(pdfDoc, "Language", "");
            
            for (int page = 1; page <= LANGUAGES.Length; page++)
            {
                pdfDoc.AddNewPage();
                
                // Create a radio button that is added to a radio group.
                PdfFormField field = PdfFormField.CreateRadioButton(pdfDoc, rect, radioGroup, LANGUAGES[page - 1]);
                field.SetBorderWidth(1);
                field.SetBorderColor(ColorConstants.BLACK);
                
                // Method specifies on which page the form field's widget must be shown.
                field.SetPage(page);
                doc.ShowTextAligned(new Paragraph(LANGUAGES[page - 1]).SetFont(font).SetFontSize(18),
                    70, 786, page, TextAlignment.LEFT, VerticalAlignment.BOTTOM, 0);
            }

            form.AddField(radioGroup);
            
            doc.Close();
        }
    }
}