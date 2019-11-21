/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Acroforms
{
    public class AddFieldAfterParagraph
    {
        public static readonly String DEST = "results/sandbox/acroforms/add_field_after_paragraph.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddFieldAfterParagraph().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            Document doc = new Document(pdfDoc);
            doc.Add(new Paragraph("This is a paragraph.\nForm field will be inserted after it"));

            float fieldHeight = 20;
            float fieldWidth = 100;

            // 1st method: calculate position and create form field, using document's root layout area
            Rectangle freeBBox = doc.GetRenderer().GetCurrentArea().GetBBox();
            float top = freeBBox.GetTop();
            PdfTextFormField field = PdfFormField.CreateText(pdfDoc,
                new Rectangle(freeBBox.GetLeft(), top - fieldHeight, fieldWidth, fieldHeight), "myField", "Value");
            form.AddField(field);

            doc.Add(new AreaBreak());

            // 2nd method: create field using custom renderer
            doc.Add(new Paragraph("This is another paragraph.\nForm field will be inserted right after it."));
            doc.Add(new TextFieldLayoutElement().SetWidth(fieldWidth).SetHeight(fieldHeight));
            doc.Add(new Paragraph("This paragraph follows the form field"));

            pdfDoc.Close();
        }

        private class TextFieldRenderer : DivRenderer
        {
            public TextFieldRenderer(TextFieldLayoutElement modelElement)
                : base(modelElement)
            {
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new TextFieldRenderer((TextFieldLayoutElement) modelElement);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);

                PdfAcroForm form = PdfAcroForm.GetAcroForm(drawContext.GetDocument(), true);
                PdfTextFormField field = PdfFormField.CreateText(drawContext.GetDocument(),
                    occupiedArea.GetBBox(), "myField2", "Another Value");
                form.AddField(field);
            }
        }

        private class TextFieldLayoutElement : Div
        {
            public override IRenderer GetRenderer()
            {
                return new TextFieldRenderer(this);
            }
        }
    }
}