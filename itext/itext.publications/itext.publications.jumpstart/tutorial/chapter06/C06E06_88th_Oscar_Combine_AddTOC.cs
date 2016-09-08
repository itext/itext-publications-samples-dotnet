/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test.Attributes;

namespace Tutorial.Chapter06 {
    [WrapToTest]
    public class C06E06_88th_Oscar_Combine_AddTOC {
        public const String SRC1 = "../../resources/pdf/88th_noms_announcement.pdf";

        public const String SRC2 = "../../resources/pdf/oscars_movies_checklist_2016.pdf";

        public const String DEST = "../../results/chapter06/88th_oscar_the_revenant_nominations_TOC.pdf";
    
        public static readonly IDictionary<String, int> TheRevenantNominations = new SortedDictionary<String, int
            >();

        static C06E06_88th_Oscar_Combine_AddTOC() {
            TheRevenantNominations["Performance by an actor in a leading role"] = 4;
            TheRevenantNominations["Performance by an actor in a supporting role"] = 4;
            TheRevenantNominations["Achievement in cinematography"] = 4;
            TheRevenantNominations["Achievement in costume design"] = 5;
            TheRevenantNominations["Achievement in directing"] = 5;
            TheRevenantNominations["Achievement in film editing"] = 6;
            TheRevenantNominations["Achievement in makeup and hairstyling"] = 7;
            TheRevenantNominations["Best motion picture of the year"] = 8;
            TheRevenantNominations["Achievement in production design"] = 8;
            TheRevenantNominations["Achievement in sound editing"] = 9;
            TheRevenantNominations["Achievement in sound mixing"] = 9;
            TheRevenantNominations["Achievement in visual effects"] = 10;
        }

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E06_88th_Oscar_Combine_AddTOC().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDoc);
            document.Add(new Paragraph(new Text("The Revenant nominations list")).SetTextAlignment(TextAlignment.CENTER
                ));
            PdfDocument firstSourcePdf = new PdfDocument(new PdfReader(SRC1));
            foreach (KeyValuePair<String, int> entry in TheRevenantNominations) {
                //Copy page
                PdfPage page = firstSourcePdf.GetPage(entry.Value).CopyTo(pdfDoc);
                pdfDoc.AddPage(page);
                //Overwrite page number
                Text text = new Text(String.Format("Page %d", pdfDoc.GetNumberOfPages() - 1));
                text.SetBackgroundColor(Color.WHITE);
                document.Add(new Paragraph(text).SetFixedPosition(pdfDoc.GetNumberOfPages(), 549, 742, 100));
                //Add destination
                String destinationKey = "p" + (pdfDoc.GetNumberOfPages() - 1);
                PdfArray destinationArray = new PdfArray();
                destinationArray.Add(page.GetPdfObject());
                destinationArray.Add(PdfName.XYZ);
                destinationArray.Add(new PdfNumber(0));
                destinationArray.Add(new PdfNumber(page.GetMediaBox().GetHeight()));
                destinationArray.Add(new PdfNumber(1));
                pdfDoc.AddNamedDestination(destinationKey, destinationArray);
                //Add TOC line with bookmark
                Paragraph p = new Paragraph();
                p.AddTabStops(new TabStop(540, TabAlignment.RIGHT, new DottedLine()));
                p.Add(entry.Key);
                p.Add(new Tab());
                p.Add((pdfDoc.GetNumberOfPages() - 1).ToString());
                p.SetProperty(Property.ACTION, PdfAction.CreateGoTo(destinationKey));
                document.Add(p);
            }
            firstSourcePdf.Close();
            //Add the last page
            PdfDocument secondSourcePdf = new PdfDocument(new PdfReader(SRC2));
            PdfPage page_1 = secondSourcePdf.GetPage(1).CopyTo(pdfDoc);
            pdfDoc.AddPage(page_1);
            //Add destination
            PdfArray destinationArray_1 = new PdfArray();
            destinationArray_1.Add(page_1.GetPdfObject());
            destinationArray_1.Add(PdfName.XYZ);
            destinationArray_1.Add(new PdfNumber(0));
            destinationArray_1.Add(new PdfNumber(page_1.GetMediaBox().GetHeight()));
            destinationArray_1.Add(new PdfNumber(1));
            pdfDoc.AddNamedDestination("checklist", destinationArray_1);
            //Add TOC line with bookmark
            Paragraph p_1 = new Paragraph();
            p_1.AddTabStops(new TabStop(540, TabAlignment.RIGHT, new DottedLine()));
            p_1.Add("Oscars\u00ae 2016 Movie Checklist");
            p_1.Add(new Tab());
            p_1.Add((pdfDoc.GetNumberOfPages() - 1).ToString());
            p_1.SetProperty(Property.ACTION, PdfAction.CreateGoTo("checklist"));
            document.Add(p_1);
            secondSourcePdf.Close();
            // close the document
            document.Close();
        }
    }
}
