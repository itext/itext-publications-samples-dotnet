/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;

namespace iText.Samples.Sandbox.Annotations
{
    public class AddInReplyTo
    {
        public static readonly String DEST = "results/sandbox/annotations/add_in_reply_to.pdf";

        public static readonly String SRC = "../../resources/pdfs/hello_sticky_note.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddInReplyTo().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfPage firstPage = pdfDoc.GetFirstPage();
            IList<PdfAnnotation> annots = firstPage.GetAnnotations();

            Rectangle stickyRectangle = annots[0].GetRectangle().ToRectangle();
            PdfAnnotation replySticky = new PdfTextAnnotation(stickyRectangle)

                // This method specifies whether the annotation will initially be displayed open.
                .SetOpen(true)
                .SetIconName(new PdfName("Comment"))

                // This method sets an annotation to which the current annotation is "in reply".
                // Both annotations shall be on the same page of the document.
                .SetInReplyTo(annots[0])

                // This method sets the text label that will be displayed in the title bar of the annotation's pop-up window
                // when open and active. This entry shall identify the user who added the annotation.
                .SetText(new PdfString("Reply"))

                // This method sets the text that will be displayed for the annotation or the alternate description,
                // if this type of annotation does not display text.
                .SetContents("Hello PDF");
            firstPage.AddAnnotation(replySticky);

            pdfDoc.Close();
        }
    }
}