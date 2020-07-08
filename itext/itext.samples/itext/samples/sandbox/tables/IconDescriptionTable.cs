using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class IconDescriptionTable
    {
        public static readonly string DEST = "results/sandbox/tables/icon_description_table.pdf";

        public static readonly string IMG = "../../../resources/img/bulb.gif";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new IconDescriptionTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] {10, 90}));
            Image img = new Image(ImageDataFactory.Create(IMG));

            // Width and height of image are set to autoscale
            table.AddCell(img.SetAutoScale(true));
            table.AddCell("A light bulb icon");

            doc.Add(table);

            doc.Close();
        }
    }
}