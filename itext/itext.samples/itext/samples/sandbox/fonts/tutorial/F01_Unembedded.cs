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

namespace iText.Samples.Sandbox.Fonts.Tutorial
{
    public class F01_Unembedded
    {
        public static readonly String DEST = "../../results/sandbox/fonts/tutorial/f01_unembedded.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new F01_Unembedded().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // The text line is "Vous êtes d'où?"
            doc.Add(new Paragraph("Vous \u00EAtes d\'o\u00F9?"));

            // The text line is "À tout à l'heure. À bientôt."
            doc.Add(new Paragraph("\u00C0 tout \u00E0 l\'heure. \u00C0 bient\u00F4t."));

            // The text line is "Je me présente."
            doc.Add(new Paragraph("Je me pr\u00E9sente."));

            // The text line is "C'est un étudiant."
            doc.Add(new Paragraph("C\'est un \u00E9tudiant."));

            // The text line is "Ça va?"
            doc.Add(new Paragraph("\u00C7a va?"));

            // The text line is "Il est ingénieur. Elle est médecin."
            doc.Add(new Paragraph("Il est ing\u00E9nieur. Elle est m\u00E9decin."));

            // The text line is "C'est une fenêtre."
            doc.Add(new Paragraph("C\'est une fen\u00EAtre."));

            // The text line is "Répétez, s'il vous plaît."
            doc.Add(new Paragraph("R\u00E9p\u00E9tez, s\'il vous pla\u00EEt."));

            doc.Close();
        }
    }
}