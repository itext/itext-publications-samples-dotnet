/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Events
{
    public class GenericFields
    {
        public static readonly String DEST = "results/sandbox/events/generic_fields.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new GenericFields().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph p = new Paragraph();
            p.Add("The Effective Date is ");

            Text day = new Text("     ");
            day.SetNextRenderer(new FieldTextRenderer(day, "day"));
            p.Add(day);
            p.Add(" day of ");

            Text month = new Text("     ");
            month.SetNextRenderer(new FieldTextRenderer(month, "month"));
            p.Add(month);
            p.Add(", ");

            Text year = new Text("            ");
            year.SetNextRenderer(new FieldTextRenderer(year, "year"));
            p.Add(year);
            p.Add(" that this will begin.");

            doc.Add(p);

            doc.Close();
        }

        private class FieldTextRenderer : TextRenderer
        {
            protected String fieldName;

            public FieldTextRenderer(Text textElement, String fieldName) : base(textElement)
            {
                this.fieldName = fieldName;
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new FieldTextRenderer((Text) modelElement, fieldName);
            }

            public override void Draw(DrawContext drawContext)
            {
                PdfTextFormField field = PdfTextFormField.CreateText(drawContext.GetDocument(),
                    GetOccupiedAreaBBox(), fieldName);
                PdfAcroForm.GetAcroForm(drawContext.GetDocument(), true)
                    .AddField(field);
            }
        }
    }
}