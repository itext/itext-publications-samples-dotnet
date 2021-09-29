using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Licensing.Base;

namespace iText.Highlevel.Chapter02 {
    /// <author>iText</author>
    public class C02E15_ShowTextAlignedKerned {
        public static String KEY = "../../../resources/license/itextkey-typography.xml";

        public const String DEST = "../../../results/chapter02/showtextalignedkerned.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E15_ShowTextAlignedKerned().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            LicenseKey.LoadLicenseFile(new FileStream(KEY, FileMode.Open, FileAccess.Read));
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            document.ShowTextAligned("The Strange Case of Dr. Jekyll and Mr. Hyde", 36, 806, TextAlignment.LEFT);
            document.ShowTextAlignedKerned("The Strange Case of Dr. Jekyll and Mr. Hyde", 36, 790, TextAlignment.LEFT, 
                VerticalAlignment.BOTTOM, 0);
            document.ShowTextAligned("AWAY AGAIN", 36, 774, TextAlignment.LEFT);
            document.ShowTextAlignedKerned("AWAY AGAIN", 36, 758, TextAlignment.LEFT, VerticalAlignment.BOTTOM, 0);
            document.Close();
        }
    }
}
