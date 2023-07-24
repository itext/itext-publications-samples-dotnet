using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Acroforms
{
    public class CreateFormInTable
    {
        public static readonly String DEST = "results/sandbox/acroforms/create_form_in_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CreateFormInTable().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();

            Cell cell = new Cell().Add(new Paragraph("Name:"));
            table.AddCell(cell);

            cell = new Cell();
            cell.SetNextRenderer(new CreateFormFieldRenderer(cell, "name"));
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("Address"));
            table.AddCell(cell);

            cell = new Cell();
            cell.SetNextRenderer(new CreateFormFieldRenderer(cell, "address"));
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }

        private class CreateFormFieldRenderer : CellRenderer
        {
            protected String fieldName;

            public CreateFormFieldRenderer(Cell modelElement, String fieldName)
                : base(modelElement)
            {
                this.fieldName = fieldName;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new CreateFormFieldRenderer((Cell) modelElement, fieldName);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);

                PdfTextFormField field = new TextFormFieldBuilder(drawContext.GetDocument(), fieldName)
                    .SetWidgetRectangle(GetOccupiedAreaBBox()).CreateText();
                field.SetValue("");
                PdfAcroForm form = PdfFormCreator.GetAcroForm(drawContext.GetDocument(), true);
                form.AddField(field);
            }
        }
    }
}