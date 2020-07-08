using System;
using System.Collections.Generic;
using iText.Barcodes;
using iText.Barcodes.Qrcode;
using iText.Html2pdf.Attach;
using iText.Layout;
using iText.Layout.Element;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Qrcodetag
{
    /// <summary>
    /// Example of a custom tagworker implementation for pdfHTML.
    /// </summary>
    /// <remarks>
    /// The tagworker processes a <bold>qr</bold> tag using iText Barcode functionality
    /// </remarks>
    public class QRCodeTagWorker : ITagWorker
    {
        private static string[] allowedErrorCorrection = {"L", "M", "Q", "H"};
        private static string[] allowedCharset = {"Cp437", "Shift_JIS", "ISO-8859-1", "ISO-8859-16"};
        
        private BarcodeQRCode qrCode;
        private Image qrCodeAsImage;

        public QRCodeTagWorker(IElementNode element, ProcessorContext context)
        {
            // Retrieve all necessary properties to create the barcode
            IDictionary<EncodeHintType, Object> hints = new Dictionary<EncodeHintType, object>();

            // Character set
            string charset = element.GetAttribute("charset");
            if (CheckCharacterSet(charset))
            {
                hints[EncodeHintType.CHARACTER_SET] = charset;
            }

            // Error-correction level
            string errorCorrection = element.GetAttribute("errorcorrection");
            if (CheckErrorCorrectionAllowed(errorCorrection))
            {
                ErrorCorrectionLevel errorCorrectionLevel = GetErrorCorrectionLevel(errorCorrection);
                hints[EncodeHintType.ERROR_CORRECTION] = errorCorrectionLevel;
            }

            // Create the QR-code
            qrCode = new BarcodeQRCode("placeholder", hints);
        }
        
        public virtual void ProcessEnd(IElementNode element, ProcessorContext context)
        {
            // Transform barcode into image
            qrCodeAsImage = new Image(qrCode.CreateFormXObject(context.GetPdfDocument()));
        }
        
        public virtual bool ProcessContent(string content, ProcessorContext context)
        {
            // Add content to the barcode
            qrCode.SetCode(content);
            return true;
        }

        public virtual bool ProcessTagChild(ITagWorker childTagWorker, ProcessorContext context)
        {
            return false;
        }

        public virtual IPropertyContainer GetElementResult()
        {
            return qrCodeAsImage;
        }

        private static bool CheckErrorCorrectionAllowed(string toCheck)
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

        private static bool CheckCharacterSet(string toCheck)
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

        private static ErrorCorrectionLevel GetErrorCorrectionLevel(string level)
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