using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddNamedDestinations
    {
        public static readonly String PDF = "results/sandbox/stamper/add_named_destinations.pdf";
        public static readonly String SRC = "../../../resources/pdfs/primes.pdf";
        public static readonly String DEST = "results/xml/primes_with_destination.xml";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddNamedDestinations().ManipulatePdf(DEST);
        }

        public static IList<int> GetFactors(int n) 
        {
            IList<int> factors = new List<int>();
            for (int i = 2; i <= n; i++) {
                while (n % i == 0) {
                    factors.Add(i);
                    n /= i;
                }
            }
            return factors;
        }

        /// <summary>Create an XML file with named destinations</summary>
        /// <param name="src">The path to the PDF with the destinations</param>
        /// <param name="dest">The path to the XML file</param>
        public void CreateXml(String src, String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src));
            
            XmlDocument doc = new XmlDocument();
            
            XmlElement root = doc.CreateElement("Destination");
            doc.AppendChild(root);
            
            IDictionary<String, PdfObject> names = pdfDoc.GetCatalog().GetNameTree(PdfName.Dests).GetNames();
            foreach (KeyValuePair<String, PdfObject> name in names) 
            {
                XmlElement el = doc.CreateElement("Name");
                el.SetAttribute("Page", name.Value.ToString());
                el.InnerText = name.Key;
                root.AppendChild(el);
            }
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            
            XmlWriter writer = XmlWriter.Create(dest, settings);
            root.WriteTo(writer);
            writer.Close();
            
            pdfDoc.Close();
        }

        protected void ManipulatePdf(String dest) 
        {
            // Creates directory and new pdf file by content of the read pdf
            FileInfo file = new FileInfo(PDF);
            file.Directory.Create();
            
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(PDF));
            
            for (int i = 1; i < pdfDoc.GetNumberOfPages(); ) 
            {
                if (GetFactors(++i).Count > 1) 
                {
                    continue;
                }
                
                // Adding named destinations for further usage depending on the needs
                PdfPage pdfPage = pdfDoc.GetPage(i);
                Rectangle pageRect = pdfPage.GetPageSize();
                float getLeft = pageRect.GetLeft();
                float getTop = pageRect.GetTop();
                PdfExplicitDestination destObj = PdfExplicitDestination.CreateXYZ(pdfPage, getLeft, getTop, 1);
                pdfDoc.AddNamedDestination("prime" + i, destObj.GetPdfObject());
            }
            
            pdfDoc.Close();
            
            CreateXml(PDF, dest);
        }
    }
}
