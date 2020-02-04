/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tagging
{
    public class AddArtifactTable
    {
        public static readonly String DEST = "results/sandbox/tagging/88th_Academy_Awards_artifact_table.pdf";
        public static readonly String SRC = "../../../resources/tagging/88th_Academy_Awards.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddArtifactTable().ManipulatePdf(SRC, DEST);
        }

        protected void ManipulatePdf(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdfDocument = new PdfDocument(reader, writer);
            Document document = new Document(pdfDocument);

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.AddCell(new Cell().Add(new Paragraph("Created as a sample document.")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("30.03.2016")).SetBorder(Border.NO_BORDER));

            // Create area break to the end of the document in order to use the last page as the current area to draw.
            document.Add(new AreaBreak(AreaBreakType.LAST_PAGE));
            table.SetFixedPosition(40, 150, 500);

            // This marks the whole table contents as an Artifact.
            // NOTE: Only content that is already added before this call will be marked as Artifact. 
            // New content will be tagged, unless you make this call again.
            table.GetAccessibilityProperties().SetRole(StandardRoles.ARTIFACT);
            document.Add(table);

            document.Close();
        }
    }
}