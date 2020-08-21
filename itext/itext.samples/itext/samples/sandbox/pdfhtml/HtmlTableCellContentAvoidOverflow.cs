using System;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Css.Apply;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml
{
    public class HtmlTableCellContentAvoidOverflow
    {
        public static readonly String DEST = "results/sandbox/pdfhtml/htmlTableCellContentAvoidOverflow.pdf";
        public static readonly String HTML = "<table style='width: 100%; table-layout:fixed;'>\n"
            + "    <tbody>\n"
            + "    <tr>\n"
            + "        <td><b>Hamlet</b><br/>\n"
            + "            <table style='width: 100%; border: 0.1mm solid; border-collapse:collapse;table-layout:fixed;'>\n"
            + "                <tbody>\n"
            + "                <tr style='text-align:center;font-weight:bold; border: 0.5px solid black;'>\n"
            + "                    <td style='border: 0.5px solid black;width:13%;'>To be, or not to be</td>\n"
            + "                    <td style='border: 0.5px solid black;width:13%; '> that is the queeeeeeeeeeeestion</td>\n"
            + "                    <td style='border: 0.5px solid black;width:16%'>Whether 'tis nobler</td>\n"
            + "                    <td style='border: 0.5px solid black;width:16%'>in the mind to suffer</td>\n"
            + "                    <td style='border: 0.5px solid black;width:16%'>Or to take aaaaaaaaarms</td>\n"
            + "                    <td style='border: 0.5px solid black;width:13%'>agaaaaaaaaaaainst a sea of troubles,</td>\n"
            + "                    <td style='border: 0.5px solid black;width:10%'>And by opposing end them<br>?</td>\n"
            + "                </tr>\n"
            + "                <tr style='border: 0.5px solid black;'>\n"
            + "                    <td style='border: 0.5px solid black;'>To die: </td>\n"
            + "                    <td style='border: 0.5px solid black;'>to sleeeeeeeeeeeeeep</td>\n"
            + "                    <td style='border: 0.5px solid black;'>No more; and by a sleep</td>\n"
            + "                    <td style='border: 0.5px solid black;'>to say we end</td>\n"
            + "                    <td style='border: 0.5px solid black;'>The heart-ache</td>\n"
            + "                    <td style='border: 0.5px solid black;'>and the thousand naaaaaaaaaaaaaaaaatural shocks</td>\n"
            + "                    <td style='border: 0.5px solid black;text-align:right;'>That flesh is heir to</td>\n"
            + "                </tr>\n"
            + "                </tbody>\n"
            + "            </table>\n"
            + "            <br></td>\n"
            + "    </tr>\n"
            + "    </tbody>\n"
            + "</table>";
        
        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HtmlTableCellContentAvoidOverflow().ManipulatePdf(HTML, DEST);
        }
        
        public void ManipulatePdf(string html, string pdfDest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(pdfDest));
            pdfDoc.AddNewPage();

            DefaultCssApplierFactory cssApplierFactory = new CellTagCssApplierFactory();

            ConverterProperties converterProperties = new ConverterProperties();
            // Using custom css applier with OverflowPropertyValue.FIT set,
            // we can achieve that text content of table's cell suits their width
            converterProperties.SetCssApplierFactory(cssApplierFactory);
            
            HtmlConverter.ConvertToPdf(html, pdfDoc, converterProperties);

            pdfDoc.Close();
        }
        
        private class CellTagCssApplierFactory : DefaultCssApplierFactory
        {
            public override ICssApplier GetCustomCssApplier(IElementNode tag)
            {
                // Custom css applier works only for 'td' html element
                if (tag.Name().Equals("td"))
                {
                    return new CellCssApplier();
                }

                return null;
            }
        }

        private class CellCssApplier : TdTagCssApplier
        {
            public override void Apply(ProcessorContext context, IStylesContainer stylesContainer, ITagWorker tagWorker)
            {
                base.Apply(context, stylesContainer, tagWorker);
                IPropertyContainer container = tagWorker.GetElementResult();
                if (container != null && container is Cell) 
                {
                    Cell cell = (Cell) container;
                    foreach (IElement element in cell.GetChildren())
                    {
                        element.SetProperty(Property.OVERFLOW_X, OverflowPropertyValue.FIT);
                        element.SetProperty(Property.OVERFLOW_Y, OverflowPropertyValue.FIT);
                    }
                }
            }
        }
    }
}