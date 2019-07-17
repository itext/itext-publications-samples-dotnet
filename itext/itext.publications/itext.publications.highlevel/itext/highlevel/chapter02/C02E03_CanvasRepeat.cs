/*
* To change this license header, choose License Headers in Project Properties.
* To change this template file, choose Tools | Templates
* and open the template in the editor.
*/

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Highlevel.Chapter02 {
    /// <author>iText</author>
    public class C02E03_CanvasRepeat {
        internal class MyCanvasRenderer : CanvasRenderer {
            protected internal bool full = false;

            public MyCanvasRenderer(Canvas canvas)
                : base(canvas) {
            }

            public override void AddChild(IRenderer renderer) {
                base.AddChild(renderer);
                this.full = true.Equals(this.GetPropertyAsBoolean(Property.FULL));
            }

            public virtual bool IsFull() {
                return this.full;
            }
        }

        public const String DEST = "../../results/chapter02/canvas_repeat.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E03_CanvasRepeat().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfPage page = pdf.AddNewPage();
            PdfCanvas pdfCanvas = new PdfCanvas(page);
            Rectangle rectangle = new Rectangle(36, 500, 100, 250);
            pdfCanvas.Rectangle(rectangle);
            pdfCanvas.Stroke();
            iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas, pdf, rectangle);
            MyCanvasRenderer renderer = new MyCanvasRenderer(canvas);
            canvas.SetRenderer(renderer);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            Text title = new Text("The Strange Case of Dr. Jekyll and Mr. Hyde").SetFont(bold);
            Text author = new Text("Robert Louis Stevenson").SetFont(font);
            Paragraph p = new Paragraph().Add(title).Add(" by ").Add(author);
            while (!renderer.IsFull()) {
                canvas.Add(p);
            }
            //Close document
            pdf.Close();
        }
    }
}
