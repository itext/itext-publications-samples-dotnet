/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class SimpleTable7
    {
        public static readonly string DEST = "../../results/sandbox/tables/simple_table7.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SimpleTable7().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont titleFont = PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD);
            Paragraph docTitle = new Paragraph("UCSC Direct - Direct Payment Form").SetMarginRight(1);
            docTitle.SetFont(titleFont).SetFontSize(11);
            doc.Add(docTitle);

            PdfFont subtitleFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            Paragraph subTitle = new Paragraph("(not to be used for reimbursement of services)");
            subTitle.SetFont(subtitleFont).SetFontSize(9);
            doc.Add(subTitle);

            PdfFont importantNoticeFont = PdfFontFactory.CreateFont(StandardFonts.COURIER);
            Paragraph importantNotice = new Paragraph(
                "Important: Form must be filled out in Adobe Reader or Acrobat Professional 8.1 or above. " +
                "To save completed forms, Acrobat Professional is required. For technical and accessibility assistance, " +
                "contact the Campus Controller's Office.");

            importantNotice.SetFont(importantNoticeFont).SetFontSize(9);
            importantNotice.SetFontColor(ColorConstants.RED);
            doc.Add(importantNotice);

            Table table = new Table(UnitValue.CreatePercentArray(10))
                .UseAllAvailableWidth().SetFixedLayout().SetWidth(UnitValue.CreatePercentValue(80));

            Cell cell = new Cell(1, 3).Add(docTitle);
            cell.SetBorder(Border.NO_BORDER);
            cell.SetHorizontalAlignment(HorizontalAlignment.LEFT);
            table.AddCell(cell);

            Cell cellCaveat = new Cell(1, 2).Add(subTitle);
            cellCaveat.SetBorder(Border.NO_BORDER);
            table.AddCell(cellCaveat);

            Cell cellImportantNote = new Cell(1, 5).Add(importantNotice);
            cellImportantNote.SetBorder(Border.NO_BORDER);

            table.AddCell(cellImportantNote);

            doc.Add(table.SetHorizontalAlignment(HorizontalAlignment.CENTER));
            doc.Add(new Paragraph("This is the same table, created differently").SetFont(subtitleFont).SetFontSize(9)
                .SetMarginBottom(10));

            table = new Table(UnitValue.CreatePercentArray(new float[] {30, 20, 50})).SetFixedLayout()
                .SetWidth(UnitValue.CreatePercentValue(80));
            table.AddCell(new Cell().Add(docTitle).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(subTitle).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(importantNotice).SetBorder(Border.NO_BORDER));

            doc.Add(table.SetHorizontalAlignment(HorizontalAlignment.CENTER));

            doc.Close();
        }
    }
}