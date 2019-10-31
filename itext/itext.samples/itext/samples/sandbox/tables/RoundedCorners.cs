/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class RoundedCorners
    {
        public static readonly string DEST = "../../results/sandbox/tables/rounded_corners.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RoundedCorners().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();

            // By default iText collapses borders and draws them on table level.
            // In this sample, however, we want each cell to draw its borders separately,
            // that's why we need to override border collapse.
            table.SetBorderCollapse(BorderCollapsePropertyValue.SEPARATE);

            // Sets horizontal spacing between all the table's cells. See css's border-spacing for more information.
            table.SetHorizontalBorderSpacing(5);

            Cell cell = GetCell("These cells have rounded borders at the top.");
            table.AddCell(cell);

            cell = GetCell("These cells aren't rounded at the bottom.");
            table.AddCell(cell);

            cell = GetCell("A custom cell event was used to achieve this.");
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }

        private static Cell GetCell(String content)
        {
            Cell cell = new Cell().Add(new Paragraph(content));
            cell.SetBorderTopRightRadius(new BorderRadius(4));
            cell.SetBorderTopLeftRadius(new BorderRadius(4));
            return cell;
        }
    }
}