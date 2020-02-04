/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class CellMethod
    {
        public static readonly string DEST = "results/sandbox/tables/cell_method.pdf";
        public static readonly string FONT = "../../../resources/font/FreeSans.ttf";

        private static PdfFont czechFont;
        private static PdfFont defaultFont;
        private static PdfFont greekFont;

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CellMethod().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            czechFont = PdfFontFactory.CreateFont(FONT, "Cp1250", true);
            greekFont = PdfFontFactory.CreateFont(FONT, "Cp1253", true);
            defaultFont = PdfFontFactory.CreateFont(FONT, null, true);

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method makes table use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            
            table.AddCell("Winansi");
            table.AddCell(GetNormalCell("Test", null, 12));
            table.AddCell("Winansi");
            table.AddCell(GetNormalCell("Test", null, -12));
            table.AddCell("Greek");
            table.AddCell(GetNormalCell("\u039d\u03cd\u03c6\u03b5\u03c2", "greek", 12));
            table.AddCell("Czech");
            table.AddCell(GetNormalCell("\u010c,\u0106,\u0160,\u017d,\u0110", "czech", 12));
            table.AddCell("Test");
            table.AddCell(GetNormalCell(" ", null, 12));
            table.AddCell("Test");
            table.AddCell(GetNormalCell(" ", "greek", 12));
            table.AddCell("Test");
            table.AddCell(GetNormalCell(" ", "czech", 12));

            doc.Add(table);

            doc.Close();
        }

        private static Cell GetNormalCell(String line, String language, float size)
        {
            if (line != null && "".Equals(line))
            {
                return new Cell();
            }

            PdfFont f = GetFontForThisLanguage(language);
            Paragraph paragraph = new Paragraph(line).SetFont(f);

            Cell cell = new Cell().Add(paragraph);
            cell.SetHorizontalAlignment(HorizontalAlignment.LEFT);

            if (size < 0)
            {
                size = -size;
                cell.SetFontSize(size);
                cell.SetFontColor(ColorConstants.RED);
            }

            return cell;
        }

        private static PdfFont GetFontForThisLanguage(String language)
        {
            if (language == null) 
            {
                return defaultFont;
            }
            
            switch (language)
            {
                case "czech":
                    return czechFont;
                case "greek":
                    return greekFont;
                default:
                    return defaultFont;
            }
        }
    }
}