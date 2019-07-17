/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace Tutorial.Chapter02 {
    /// <summary>Simple changing graphics state example.</summary>
    public class C02E02_GridLines {
        public const String DEST = "../../results/chapter02/grid_lines.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E02_GridLines().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PageSize ps = PageSize.A4.Rotate();
            PdfPage page = pdf.AddNewPage(ps);
            PdfCanvas canvas = new PdfCanvas(page);
            //Replace the origin of the coordinate system to the center of the page
            canvas.ConcatMatrix(1, 0, 0, 1, ps.GetWidth() / 2, ps.GetHeight() / 2);
            Color grayColor = new DeviceCmyk(0f, 0f, 0f, 0.875f);
            Color greenColor = new DeviceCmyk(1f, 0f, 1f, 0.176f);
            Color blueColor = new DeviceCmyk(1f, 0.156f, 0f, 0.118f);
            canvas.SetLineWidth(0.5f).SetStrokeColor(blueColor);
            //Draw horizontal grid lines
            for (int i = -((int)ps.GetHeight() / 2 - 57); i < ((int)ps.GetHeight() / 2 - 56); i += 40) {
                canvas.MoveTo(-(ps.GetWidth() / 2 - 15), i).LineTo(ps.GetWidth() / 2 - 15, i);
            }
            //Draw vertical grid lines
            for (int j = -((int)ps.GetWidth() / 2 - 61); j < ((int)ps.GetWidth() / 2 - 60); j += 40) {
                canvas.MoveTo(j, -(ps.GetHeight() / 2 - 15)).LineTo(j, ps.GetHeight() / 2 - 15);
            }
            canvas.Stroke();
            //Draw axes
            canvas.SetLineWidth(3).SetStrokeColor(grayColor);
            C02E01_Axes.DrawAxes(canvas, ps);
            //Draw plot
            canvas.SetLineWidth(2).SetStrokeColor(greenColor).SetLineDash(10, 10, 8).MoveTo(-(ps.GetWidth() / 2 - 15), 
                -(ps.GetHeight() / 2 - 15)).LineTo(ps.GetWidth() / 2 - 15, ps.GetHeight() / 2 - 15).Stroke();
            //Close document
            pdf.Close();
        }
    }
}
