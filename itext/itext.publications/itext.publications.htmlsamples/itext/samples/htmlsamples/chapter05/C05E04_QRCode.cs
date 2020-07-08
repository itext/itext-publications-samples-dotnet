using System;
using System.Collections.Generic;
using System.IO;
using iText.Barcodes;
using iText.Barcodes.Qrcode;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Css.Apply;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Layout;
using iText.Layout.Element;
using iText.License;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Htmlsamples.Chapter05
{
    /// <summary>
    /// Converts an HTML file to a PDF document, introducing a custom tag to create
    /// a QR Code involving a custom TagWorker and a custom CssApplier.
    /// </summary>
    public class C05E04_QRCode
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch05/qrcode.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/qrcode.html";

        /// <summary>
        /// The main method of this example.
        /// </summary>
        /// <param name="args">no arguments are needed to run this example.</param>
        public static void Main(String[] args)
        {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-html2pdf_typography.xml");
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            C05E04_QRCode app = new C05E04_QRCode();
            app.CreatePdf(SRC, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            properties
                .SetCssApplierFactory(new QRCodeTagCssApplierFactory())
                .SetTagWorkerFactory(new QRCodeTagWorkerFactory());
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), properties);
        }

        /// <summary>
        /// A factory for creating QRCodeTagCssApplier objects.
        /// </summary>
        class QRCodeTagCssApplierFactory : DefaultCssApplierFactory
        {
            /* (non-Javadoc)
             * @see iText.Html2pdf.Css.Apply.Impl.DefaultCssApplierFactory#GetCustomCssApplier(iText.StyledXmlParser.Node.IElementNode)
             */
            public override ICssApplier GetCustomCssApplier(IElementNode tag)
            {
                if (tag.Name().Equals("qr"))
                {
                    return new BlockCssApplier();
                }

                return null;
            }
        }

        /// <summary>
        /// A factory for creating QRCodeTagWorker objects.
        /// </summary>
        class QRCodeTagWorkerFactory : DefaultTagWorkerFactory
        {
            /* (non-Javadoc)
             * @see iText.Html2pdf.Attach.Impl.DefaultTagWorkerFactory#GetCustomTagWorker(iText.StyledXmlParser.Node.IElementNode,
             * iText.Html2pdf.Attach.ProcessorContext)
             */
            public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context)
            {
                if (tag.Name().Equals("qr"))
                {
                    return new QRCodeTagWorker(tag, context);
                }

                return null;
            }
        }

        /// <summary>
        /// The custom ITagWorker implementation for the qr-tag.
        /// </summary>
        class QRCodeTagWorker : ITagWorker
        {
            /// <summary>
            /// The different error corrections that are allowed.
            /// </summary>
            private static String[] allowedErrorCorrection = {"L", "M", "Q", "H"};

            /// <summary>
            /// The different characters sets that are allowed.
            /// </summary>
            private static String[] allowedCharset = {"Cp437", "Shift_JIS", "ISO-8859-1", "ISO-8859-16"};

            /// <summary>
            /// The QR code object.
            /// </summary>
            private BarcodeQRCode qrCode;

            /// <summary>
            /// The QR code as an Image object.
            /// </summary>
            private Image qrCodeAsImage;

            /// <summary>
            /// Instantiates a new QR code tag worker.
            /// </summary>
            /// <param name="element">the element node</param>
            /// <param name="context">the processor context</param>
            public QRCodeTagWorker(IElementNode element, ProcessorContext context)
            {
                //Retrieve all necessary properties to create the barcode
                Dictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();
                //Character set
                String charset = element.GetAttribute("charset");
                if (CheckCharacterSet(charset))
                {
                    hints.Add(EncodeHintType.CHARACTER_SET, charset);
                }

                //Error-correction level
                String errorCorrection = element.GetAttribute("errorcorrection");
                if (CheckErrorCorrectionAllowed(errorCorrection))
                {
                    ErrorCorrectionLevel errorCorrectionLevel = GetErrorCorrectionLevel(errorCorrection);
                    hints.Add(EncodeHintType.ERROR_CORRECTION, errorCorrectionLevel);
                }

                //Create the QR-code
                qrCode = new BarcodeQRCode("placeholder", hints);
            }

            /* (non-Javadoc)
             * @see iText.Html2pdf.Attach.ITagWorker#ProcessContent(System.String, iText.Html2pdf.Attach.ProcessorContext)
             */
            public bool ProcessContent(String content, ProcessorContext context)
            {
                //Add content to the barcode
                qrCode.SetCode(content);
                return true;
            }

            /* (non-Javadoc)
             * @see iText.Html2pdf.Attach.ITagWorker#ProcessTagChild(iText.Html2pdf.Attach.ITagWorker, iText.Html2pdf.Attach.ProcessorContext)
             */
            public bool ProcessTagChild(ITagWorker childTagWorker, ProcessorContext context)
            {
                return false;
            }

            /* (non-Javadoc)
             * @see iText.Html2pdf.Attach.ITagWorker#ProcessEnd(iText.StyledXmlParser.Node.IElementNode, iText.Html2pdf.Attach.ProcessorContext)
             */
            public void ProcessEnd(IElementNode element, ProcessorContext context)
            {
                //Transform barcode into image
                qrCodeAsImage = new Image(qrCode.CreateFormXObject(context.GetPdfDocument()));
            }

            /* (non-Javadoc)
             * @see iText.Html2pdf.Attach.ITagWorker#GetElementResult()
             */
            public IPropertyContainer GetElementResult()
            {
                return qrCodeAsImage;
            }

            /// <summary>
            /// Checks if a type of error correction is allowed.
            /// </summary>
            /// <param name="toCheck">the error correction type to check</param>
            /// <returns>true, if successful</returns>
            private static bool CheckErrorCorrectionAllowed(String toCheck)
            {
                for (int i = 0; i < allowedErrorCorrection.Length; i++)
                {
                    if (toCheck.ToUpper().Equals(allowedErrorCorrection[i]))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Check if a certain character set is allowed.
            /// </summary>
            /// <param name="toCheck">the character set to check</param>
            /// <returns>true, if successful</returns>
            private static bool CheckCharacterSet(String toCheck)
            {
                for (int i = 0; i < allowedCharset.Length; i++)
                {
                    if (toCheck.Equals(allowedCharset[i]))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Gets the error correction level.
            /// </summary>
            /// <param name="level">the error correction level as a String</param>
            /// <returns>the error correction level</returns>
            private static ErrorCorrectionLevel GetErrorCorrectionLevel(String level)
            {
                switch (level)
                {
                    case "L":
                        return ErrorCorrectionLevel.L;
                    case "M":
                        return ErrorCorrectionLevel.M;
                    case "Q":
                        return ErrorCorrectionLevel.Q;
                    case "H":
                        return ErrorCorrectionLevel.H;
                }

                return null;
            }
        }
    }
}