using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;

namespace iText.Samples.Sandbox.Annotations
{
   
    // SetCustomFontInDefaultAppearance.cs
    // 
    // This class demonstrates how to create a free text annotation using a custom font
    // in a PDF document. The code loads a custom TrueType font (Vollkorn-Regular),
    // configures it for full embedding (no subsetting), adds it to the document's resources,
    // and then creates a free text annotation with this font specified in its default appearance.
    // The annotation is displayed with red text at 24pt size. This example shows how to
    // customize the visual appearance of text annotations beyond the default fonts.
 
    public class SetCustomFontInDefaultAppearance
    {
        public static readonly String DEST = "results/sandbox/annotations/customFontInDA.pdf";
        public static readonly String FONT = "../../../resources/font/Vollkorn-Regular.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SetCustomFontInDefaultAppearance().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfFont font = PdfFontFactory.CreateFont(FONT, null, 
                PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            // Set the full font to be included and all subset ranges to be removed.
            font.SetSubset(false);

            PdfResources acroResources = new PdfResources();
            PdfName fontResourceName = acroResources.AddFont(pdf, font);
            PdfFormCreator.GetAcroForm(pdf, true).SetDefaultResources(acroResources.GetPdfObject());

            Rectangle rect = new Rectangle(100, 700, 200, 120);
            String annotationText = "Annotation text";

            /* Set a default appearance string:
             * Tf - a text font operator
             * 24 - a font size (zero value meas that the font shall be auto-sized)
             * fontResourceName - a font value (shall match a resource name in the Font entry
             * of the default resource dictionary)
             * 1 0 0 rg - a color value (red)
             */
            PdfString daString = new PdfString(fontResourceName + " 24 Tf 1 0 0 rg");
            PdfAnnotation annotation = new PdfFreeTextAnnotation(rect, new PdfString(annotationText, PdfEncodings.UNICODE_BIG))
                .SetDefaultAppearance(daString);
            pdf
                .AddNewPage()
                .AddAnnotation(annotation);

            pdf.Close();
        }
    }
}