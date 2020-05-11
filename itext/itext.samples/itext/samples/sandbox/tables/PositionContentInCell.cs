using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Tables
{
    public class PositionContentInCell
    {
        public static readonly string DEST = "results/sandbox/tables/position_content_in_cell.pdf";

        public static readonly string IMG = "../../../resources/img/info.png";

        private enum POSITION
        {
            TOP_LEFT,
            TOP_RIGHT,
            BOTTOM_LEFT,
            BOTTOM_RIGHT
        }

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PositionContentInCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            // 1. Create a Document which contains a table:
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            Cell cell1 = new Cell();
            Cell cell2 = new Cell();
            Cell cell3 = new Cell();
            Cell cell4 = new Cell();

            // 2. Inside that table, make each cell with specific height:
            cell1.SetHeight(50);
            cell2.SetHeight(50);
            cell3.SetHeight(50);
            cell4.SetHeight(50);

            // 3. Each cell has the same background image
            // 4. Add text in front of the image at specific position
            cell1.SetNextRenderer(new ImageAndPositionRenderer(cell1, new Image(ImageDataFactory.Create(IMG)), 
                "Top left", POSITION.TOP_LEFT));
            cell2.SetNextRenderer(new ImageAndPositionRenderer(cell2, new Image(ImageDataFactory.Create(IMG)),
                "Top right", POSITION.TOP_RIGHT));
            cell3.SetNextRenderer(new ImageAndPositionRenderer(cell3, new Image(ImageDataFactory.Create(IMG)),
                "Bottom left", POSITION.BOTTOM_LEFT));
            cell4.SetNextRenderer(new ImageAndPositionRenderer(cell4, new Image(ImageDataFactory.Create(IMG)),
                "Bottom right", POSITION.BOTTOM_RIGHT));

            // Wrap it all up!
            table.AddCell(cell1);
            table.AddCell(cell2);
            table.AddCell(cell3);
            table.AddCell(cell4);

            doc.Add(table);

            doc.Close();
        }

        private class ImageAndPositionRenderer : CellRenderer
        {
            private Image img;
            private String content;
            private POSITION position;

            public ImageAndPositionRenderer(Cell modelElement, Image img, String content, POSITION position)
                : base(modelElement)
            {
                this.img = img;
                this.content = content;
                this.position = position;
            }            
            
            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new ImageAndPositionRenderer((Cell) modelElement, img, content, position);
            }

            public override void Draw(DrawContext drawContext)
            {
                base.Draw(drawContext);
                
                Rectangle area = GetOccupiedAreaBBox();
                img.ScaleToFit(area.GetWidth(), area.GetHeight());

                drawContext.GetCanvas().AddXObject(img.GetXObject(),
                    area.GetX() + (area.GetWidth() - img.GetImageWidth() *
                                                    img.GetProperty<float>(Property.HORIZONTAL_SCALING)) / 2,
                    area.GetY() + (area.GetHeight() - img.GetImageHeight() *
                                                    img.GetProperty<float>(Property.VERTICAL_SCALING)) / 2,
                    img.GetImageWidth() * img.GetProperty<float>(Property.HORIZONTAL_SCALING));

                drawContext.GetCanvas().Stroke();

                Paragraph p = new Paragraph(content);
                Leading leading = p.GetDefaultProperty<Leading>(Property.LEADING);

                UnitValue defaultFontSizeUv = new DocumentRenderer(new Document(drawContext.GetDocument()))
                    .GetPropertyAsUnitValue(Property.FONT_SIZE);

                float defaultFontSize = defaultFontSizeUv.IsPointValue() ? defaultFontSizeUv.GetValue() : 12f;
                float x;
                float y;
                TextAlignment? alignment;
                switch (position)
                {
                    case POSITION.TOP_LEFT:
                    {
                        x = area.GetLeft() + 3;
                        y = area.GetTop() - defaultFontSize * leading.GetValue();
                        alignment = TextAlignment.LEFT;
                        break;
                    }

                    case POSITION.TOP_RIGHT:
                    {
                        x = area.GetRight() - 3;
                        y = area.GetTop() - defaultFontSize * leading.GetValue();
                        alignment = TextAlignment.RIGHT;
                        break;
                    }

                    case POSITION.BOTTOM_LEFT:
                    {
                        x = area.GetLeft() + 3;
                        y = area.GetBottom() + 3;
                        alignment = TextAlignment.LEFT;
                        break;
                    }

                    case POSITION.BOTTOM_RIGHT:
                    {
                        x = area.GetRight() - 3;
                        y = area.GetBottom() + 3;
                        alignment = TextAlignment.RIGHT;
                        break;
                    }

                    default:
                    {
                        x = 0;
                        y = 0;
                        alignment = TextAlignment.CENTER;
                        break;
                    }
                }

                new Canvas(drawContext.GetCanvas(), area).ShowTextAligned(p, x, y, alignment);
            }
        }
    }
}