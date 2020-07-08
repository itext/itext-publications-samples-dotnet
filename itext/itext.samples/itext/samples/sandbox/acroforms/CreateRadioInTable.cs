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
    public class CreateRadioInTable
    {
        public static readonly String DEST = "results/sandbox/acroforms/create_radio_in_table.pdf";
        
        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new CreateRadioInTable().ManipulatePdf(DEST);
        }
        
        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            
            // Radio buttons will be added to this radio group
            PdfButtonFormField group = PdfFormField.CreateRadioGroup(pdfDoc, "Language", "");
            
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            Cell cell = new Cell().Add(new Paragraph("English"));
            table.AddCell(cell);
            
            cell = new Cell();
            
            // The renderer creates radio button for the current radio group in the current cell
            cell.SetNextRenderer(new AddRadioButtonRenderer(cell, group, "english"));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("French"));
            table.AddCell(cell);
            
            cell = new Cell();
            cell.SetNextRenderer(new AddRadioButtonRenderer(cell, group, "french"));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("Dutch"));
            table.AddCell(cell);
            
            cell = new Cell();
            cell.SetNextRenderer(new AddRadioButtonRenderer(cell, group, "dutch"));
            table.AddCell(cell);
            
            doc.Add(table);
            
            form.AddField(group);
            
            doc.Close();
        }

        private class AddRadioButtonRenderer : CellRenderer
        {
            protected String value;
            protected PdfButtonFormField radioGroup;

            public AddRadioButtonRenderer(Cell modelElement, PdfButtonFormField radioGroup,
                String fieldName) : base(modelElement)
            {
                this.radioGroup = radioGroup;
                this.value = fieldName;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new AddRadioButtonRenderer((Cell) modelElement, radioGroup, value);
            }

            public override void Draw(DrawContext drawContext)
            {
                // Create a radio button that is added to a radio group.
                PdfFormField.CreateRadioButton(drawContext.GetDocument(), GetOccupiedAreaBBox(), radioGroup,
                    value);
            }
        }
    }
}