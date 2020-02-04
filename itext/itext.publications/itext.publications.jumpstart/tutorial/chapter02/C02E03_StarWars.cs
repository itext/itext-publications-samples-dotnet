/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace Tutorial.Chapter02 {
    /// <summary>Simple drawing text example.</summary>
    public class C02E03_StarWars {
        public const String DEST = "../../../results/chapter02/star_wars.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E03_StarWars().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            //Add new page
            PageSize ps = PageSize.A4;
            PdfPage page = pdf.AddNewPage(ps);
            PdfCanvas canvas = new PdfCanvas(page);
            IList<String> text = new List<String>();
            text.Add("         Episode V         ");
            text.Add("  THE EMPIRE STRIKES BACK  ");
            text.Add("It is a dark time for the");
            text.Add("Rebellion. Although the Death");
            text.Add("Star has been destroyed,");
            text.Add("Imperial troops have driven the");
            text.Add("Rebel forces from their hidden");
            text.Add("base and pursued them across");
            text.Add("the galaxy.");
            text.Add("Evading the dreaded Imperial");
            text.Add("Starfleet, a group of freedom");
            text.Add("fighters led by Luke Skywalker");
            text.Add("has established a new secret");
            text.Add("base on the remote ice world");
            text.Add("of Hoth...");
            //Replace the origin of the coordinate system to the top left corner
            canvas.ConcatMatrix(1, 0, 0, 1, 0, ps.GetHeight());
            canvas.BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD), 14).SetLeading(14
                 * 1.2f).MoveText(70, -40);
            foreach (String s in text) {
                //Add text and move to the next line
                canvas.NewlineShowText(s);
            }
            canvas.EndText();
            //Close document
            pdf.Close();
        }
    }
}
