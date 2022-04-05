using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;

namespace iText.Samples.Sandbox.Acroforms
{
    public class ChangeFieldPosition
    {
        public static readonly String DEST = "results/sandbox/acroforms/change_field_position.pdf";

        public static readonly String SRC = "../../../resources/pdfs/state.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ChangeFieldPosition().ManipulatePdf(DEST);
        }
        
        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            
            IDictionary<String, PdfFormField> fields = form.GetAllFormFields();
            PdfFormField field = fields["timezone2"];
            PdfWidgetAnnotation widgetAnnotation = field.GetWidgets()[0];
            PdfArray annotationRect = widgetAnnotation.GetRectangle();
            
            // Change value of the right coordinate (index 2 corresponds with right coordinate)
            annotationRect.Set(2, new PdfNumber(annotationRect.GetAsNumber(2).FloatValue() - 10f));
            
            pdfDoc.Close();
        }
    }
}