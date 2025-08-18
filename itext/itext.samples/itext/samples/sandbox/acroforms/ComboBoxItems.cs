using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font.Constants;
using iText.Kernel;
using iText.Kernel.Colors;
using iText.Kernel.Exceptions;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // ComboBoxItems.cs
    // 
    // This example demonstrates how to create a PDF with a combobox form field.
    // The combobox allows users to select one option from a predefined list of choices.
 
    public class ComboBoxItems
    {
        public static readonly String DEST = "results/sandbox/acroforms/combo_box_items.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ComboBoxItems().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(612, 792));

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.AddCell(new Cell().Add(new Paragraph("Combobox:")));
            Cell cell = new Cell();

            // Add rows with selectors
            String[] options = {"Choose first option", "Choose second option", "Choose third option"};
            String[] exports = {"option1", "option2", "option3"};
            
            // The renderer creates combobox in the current cell
            cell.SetNextRenderer(new SelectCellRenderer(cell, "Choose first option", exports, options));
            cell.SetHeight(20);
            table.AddCell(cell);
            doc.Add(table);

            doc.Close();
        }

        private class SelectCellRenderer : CellRenderer
        {
            protected String name;
            protected String[] exports;
            protected String[] options;

            public SelectCellRenderer(Cell modelElement, String name, String[] exports, String
                [] options)
                : base(modelElement)
            {
                this.name = name;
                this.exports = exports;
                this.options = options;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new SelectCellRenderer((Cell) modelElement, name, exports, options);
            }

            public override void Draw(DrawContext drawContext)
            {
                PdfFont font;
                try
                {
                    font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                }
                catch (IOException e)
                {
                    throw new PdfException(e);
                }

                String[][] optionsArray = new String[options.Length][];
                for (int i = 0; i < options.Length; i++)
                {
                    optionsArray[i] = new String[2];
                    optionsArray[i][0] = exports[i];
                    optionsArray[i][1] = options[i];
                }

                PdfAcroForm form = PdfFormCreator.GetAcroForm(drawContext.GetDocument(), true);

                // The 3rd parameter is the combobox name, the 4th parameter is the combobox's initial value
                PdfChoiceFormField choice = new ChoiceFormFieldBuilder(drawContext.GetDocument(), name)
                    .SetWidgetRectangle(GetOccupiedAreaBBox()).SetOptions(optionsArray).CreateComboBox();
                choice.SetValue(name);
                choice.SetFont(font);
                choice.GetWidgets()[0].SetBorderStyle(PdfAnnotation.STYLE_BEVELED);
                choice.GetFirstFormAnnotation().SetVisibility(PdfFormAnnotation.VISIBLE_BUT_DOES_NOT_PRINT);
                choice.GetFirstFormAnnotation().SetBorderColor(ColorConstants.GRAY);
                choice.SetJustification(TextAlignment.CENTER);
                form.AddField(choice);
            }
        }
    }
}