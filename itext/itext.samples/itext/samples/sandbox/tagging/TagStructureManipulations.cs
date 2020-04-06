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
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.Pdf.Tagutils;

namespace iText.Samples.Sandbox.Tagging
{
    public class TagStructureManipulations
    {
        public static readonly String DEST = "results/sandbox/tagging/88th_Academy_Awards_with_stars.pdf";
        public static readonly String SRC = "../../../resources/tagging/88th_Academy_Awards.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TagStructureManipulations().ManipulatePdf(SRC, DEST);
        }

        protected void ManipulatePdf(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdfDocument = new PdfDocument(reader, writer);

            PdfPage firstPage = pdfDocument.GetFirstPage();
            PdfCanvas canvas = new PdfCanvas(firstPage);

            // The blue star would be something like logo of our document.
            // So for example we don't want it to be read out loud on every page. To achieve it, we mark it as an Artifact.
            canvas.OpenTag(new CanvasArtifact());
            DrawStar(canvas, 30, 745, ColorConstants.BLUE);
            canvas.CloseTag();

            // The green star we want to be a part of actual content and logical structure of the document.
            // To modify tag structure manually we create TagTreePointer. After creation it points at the root tag.
            TagTreePointer tagPointer = new TagTreePointer(pdfDocument);
            tagPointer.AddTag(StandardRoles.FIGURE);
            tagPointer.GetProperties().SetAlternateDescription("The green star.");

            // It is important to set the page at which new content will be tagged
            tagPointer.SetPageForTagging(firstPage);

            canvas.OpenTag(tagPointer.GetTagReference());
            DrawStar(canvas, 450, 745, ColorConstants.GREEN);
            canvas.CloseTag();

            // We can change the position of the existing tags in the tag structure.
            tagPointer.MoveToParent();
            TagTreePointer newPositionOfStar = new TagTreePointer(pdfDocument);
            newPositionOfStar.MoveToKid(StandardRoles.SECT);
            int indexOfTheGreenStarTag = 2;

            // tagPointer points at the parent of the green star tag
            tagPointer.RelocateKid(indexOfTheGreenStarTag, newPositionOfStar);

            // Using the relocateKid method, we can even change the order of the same parent's kids.
            // This could be used to change for example reading order.

            // Make both tagPointer and newPositionOfStar to point at the same tag
            tagPointer.MoveToRoot().MoveToKid(StandardRoles.SECT);


            // Next added tag to this tag pointer will be added at the 0 position
            newPositionOfStar.SetNextNewKidIndex(0);
            indexOfTheGreenStarTag = 2;
            tagPointer.RelocateKid(indexOfTheGreenStarTag, newPositionOfStar);

            pdfDocument.Close();
        }

        private static void DrawStar(PdfCanvas canvas, int x, int y, Color color)
        {
            canvas.SetFillColor(color);
            canvas
                .MoveTo(x + 10, y)
                .LineTo(x + 80, y + 60)
                .LineTo(x, y + 60)
                .LineTo(x + 70, y)
                .LineTo(x + 40, y + 90)
                .ClosePathFillStroke();
        }
    }
}