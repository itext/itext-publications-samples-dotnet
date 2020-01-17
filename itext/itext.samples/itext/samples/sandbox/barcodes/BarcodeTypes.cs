/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this Address:
sales@itextpdf.com
*/

using System;
using System.IO;
using System.Text;
using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Barcodes
{
    public class BarcodeTypes
    {
        public static readonly String DEST = "results/sandbox/barcodes/barcodeLayout.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BarcodeTypes().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(340, 842));

            // The default barcode EAN 13 type
            doc.Add(new Paragraph("Barcode EAN.UCC-13"));
            BarcodeEAN codeEAN = new BarcodeEAN(pdfDoc);
            codeEAN.SetCode("4512345678906");
            doc.Add(new Paragraph("default:"));
            codeEAN.FitWidth(250);
            doc.Add(new Image(codeEAN.CreateFormXObject(pdfDoc)));
            codeEAN.SetGuardBars(false);
            doc.Add(new Paragraph("without guard bars:"));
            doc.Add(new Image(codeEAN.CreateFormXObject(pdfDoc)));
            codeEAN.SetBaseline(-1);
            codeEAN.SetGuardBars(true);
            doc.Add(new Paragraph("text above:"));
            doc.Add(new Image(codeEAN.CreateFormXObject(pdfDoc)));
            codeEAN.SetBaseline(codeEAN.GetSize());

            // Barcode EAN UPC A type
            doc.Add(new Paragraph("Barcode UCC-12 (UPC-A)"));
            codeEAN.SetCodeType(BarcodeEAN.UPCA);
            codeEAN.SetCode("785342304749");
            doc.Add(new Image(codeEAN.CreateFormXObject(pdfDoc)));

            // Barcode EAN 8 type
            doc.Add(new Paragraph("Barcode EAN.UCC-8"));
            codeEAN.SetCodeType(BarcodeEAN.EAN8);
            codeEAN.SetBarHeight(codeEAN.GetSize() * 1.5f);
            codeEAN.SetCode("34569870");
            codeEAN.FitWidth(250);
            doc.Add(new Image(codeEAN.CreateFormXObject(pdfDoc)));

            // Barcode UPC E type
            doc.Add(new Paragraph("Barcode UPC-E"));
            codeEAN.SetCodeType(BarcodeEAN.UPCE);
            codeEAN.SetCode("03456781");
            codeEAN.FitWidth(250);
            doc.Add(new Image(codeEAN.CreateFormXObject(pdfDoc)));
            codeEAN.SetBarHeight(codeEAN.GetSize() * 3);

            // Barcode EANSUPP type
            doc.Add(new Paragraph("Bookland - BarcodeEANSUPP"));
            doc.Add(new Paragraph("ISBN 0-321-30474-8"));
            codeEAN = new BarcodeEAN(pdfDoc);
            codeEAN.SetCodeType(BarcodeEAN.EAN13);
            codeEAN.SetCode("9781935182610");
            BarcodeEAN codeSUPP = new BarcodeEAN(pdfDoc);
            codeSUPP.SetCodeType(BarcodeEAN.SUPP5);
            codeSUPP.SetCode("55999");
            codeSUPP.SetBaseline(-2);
            BarcodeEANSUPP eanSupp = new BarcodeEANSUPP(codeEAN, codeSUPP);
            doc.Add(new Image(eanSupp.CreateFormXObject(null, ColorConstants.BLUE, pdfDoc)));

            // Barcode CODE 128 type
            doc.Add(new Paragraph("Barcode 128"));
            Barcode128 code128 = new Barcode128(pdfDoc);
            code128.SetCode("0123456789 hello");
            code128.FitWidth(250);
            doc.Add(new Image(code128.CreateFormXObject(pdfDoc))
                .SetRotationAngle(Math.PI / 2)
                .SetMargins(10, 10, 10, 10));
            code128.SetCode("0123456789\uffffMy Raw Barcode (0 - 9)");
            code128.SetCodeType(Barcode128.CODE128_RAW);
            code128.FitWidth(250);
            doc.Add(new Image(code128.CreateFormXObject(pdfDoc)));

            // Data for the barcode
            String code402 = "24132399420058289";
            String code90 = "3700000050";
            String code421 = "422356";
            StringBuilder data = new StringBuilder(code402);
            data.Append(Barcode128.FNC1);
            data.Append(code90);
            data.Append(Barcode128.FNC1);
            data.Append(code421);

            Barcode128 shipBarCode = new Barcode128(pdfDoc);
            shipBarCode.SetX(0.75f);
            shipBarCode.SetN(1.5f);
            shipBarCode.SetSize(10f);
            shipBarCode.SetTextAlignment(Barcode1D.ALIGN_CENTER);
            shipBarCode.SetBaseline(10f);
            shipBarCode.SetBarHeight(50f);
            shipBarCode.SetCode(data.ToString());
            shipBarCode.FitWidth(250);
            doc.Add(new Image(shipBarCode.CreateFormXObject(ColorConstants.BLACK, ColorConstants.BLUE, pdfDoc)));

            // CODE 128 type barcode, which is composed of 3 blocks with AI 01, 3101 and 10
            Barcode128 uccEan128 = new Barcode128(pdfDoc);
            uccEan128.SetCodeType(Barcode128.CODE128_UCC);
            uccEan128.SetCode("(01)00000090311314(10)ABC123(15)060916");
            uccEan128.FitWidth(250);
            doc.Add(new Image(uccEan128.CreateFormXObject(ColorConstants.BLUE, ColorConstants.BLACK, pdfDoc)));
            uccEan128.SetCode("0191234567890121310100035510ABC123");
            uccEan128.FitWidth(250);
            doc.Add(new Image(uccEan128.CreateFormXObject(ColorConstants.BLUE, ColorConstants.RED, pdfDoc)));
            uccEan128.SetCode("(01)28880123456788");
            uccEan128.FitWidth(250);
            doc.Add(new Image(uccEan128.CreateFormXObject(ColorConstants.BLUE, ColorConstants.BLACK, pdfDoc)));

            // Barcode INTER25 type
            doc.Add(new Paragraph("Barcode Interrevealed 2 of 5"));
            BarcodeInter25 code25 = new BarcodeInter25(pdfDoc);
            code25.SetGenerateChecksum(true);
            code25.SetCode("41-1200076041-001");
            code25.FitWidth(250);
            doc.Add(new Image(code25.CreateFormXObject(pdfDoc)));
            code25.SetCode("411200076041001");
            code25.FitWidth(250);
            doc.Add(new Image(code25.CreateFormXObject(pdfDoc)));
            code25.SetCode("0611012345678");
            code25.SetChecksumText(true);
            code25.FitWidth(250);
            doc.Add(new Image(code25.CreateFormXObject(pdfDoc)));

            // Barcode POSTNET type
            doc.Add(new Paragraph("Barcode Postnet"));
            BarcodePostnet codePost = new BarcodePostnet(pdfDoc);
            doc.Add(new Paragraph("ZIP"));
            codePost.SetCode("01234");
            codePost.FitWidth(250);
            doc.Add(new Image(codePost.CreateFormXObject(pdfDoc)));
            doc.Add(new Paragraph("ZIP+4"));
            codePost.SetCode("012345678");
            codePost.FitWidth(250);
            doc.Add(new Image(codePost.CreateFormXObject(pdfDoc)));
            doc.Add(new Paragraph("ZIP+4 and dp"));
            codePost.SetCode("01234567890");
            codePost.FitWidth(250);
            doc.Add(new Image(codePost.CreateFormXObject(pdfDoc)));

            // Barcode PLANET type
            doc.Add(new Paragraph("Barcode Planet"));
            BarcodePostnet codePlanet = new BarcodePostnet(pdfDoc);
            codePlanet.SetCode("01234567890");
            codePlanet.SetCodeType(BarcodePostnet.TYPE_PLANET);
            codePlanet.FitWidth(250);
            doc.Add(new Image(codePlanet.CreateFormXObject(pdfDoc)));

            // Barcode CODE 39 type
            doc.Add(new Paragraph("Barcode 3 of 9"));
            Barcode39 code39 = new Barcode39(pdfDoc);
            code39.SetCode("ITEXT IN ACTION");
            code39.FitWidth(250);
            doc.Add(new Image(code39.CreateFormXObject(pdfDoc)));

            doc.Add(new Paragraph("Barcode 3 of 9 extended"));
            Barcode39 code39ext = new Barcode39(pdfDoc);
            code39ext.SetCode("iText in Action");
            code39ext.SetStartStopText(false);
            code39ext.SetExtended(true);
            code39ext.FitWidth(250);
            doc.Add(new Image(code39ext.CreateFormXObject(pdfDoc)));

            // Barcode CODABAR type
            doc.Add(new Paragraph("Codabar"));
            BarcodeCodabar codabar = new BarcodeCodabar(pdfDoc);
            codabar.SetCode("A123A");
            codabar.SetStartStopText(true);
            codabar.FitWidth(250);
            doc.Add(new Image(codabar.CreateFormXObject(pdfDoc)));

            doc.Add(new AreaBreak());

            // Barcode PDF417 type
            doc.Add(new Paragraph("Barcode PDF417"));
            BarcodePDF417 pdf417 = new BarcodePDF417();
            String text = "Call me Ishmael. Some years ago--never mind how long "
                          + "precisely --having little or no money in my purse, and nothing "
                          + "particular to interest me on shore, I thought I would sail about "
                          + "a little and see the watery part of the world.";
            pdf417.SetCode(text);

            PdfFormXObject xObject = pdf417.CreateFormXObject(pdfDoc);
            Image img = new Image(xObject);
            doc.Add(img.SetAutoScale(true));

            doc.Add(new Paragraph("Barcode Datamatrix"));
            BarcodeDataMatrix datamatrix = new BarcodeDataMatrix();
            datamatrix.SetCode(text);
            Image imgDM = new Image(datamatrix.CreateFormXObject(pdfDoc));
            doc.Add(imgDM.ScaleToFit(250, 250));

            // Barcode QRCode type
            doc.Add(new Paragraph("Barcode QRCode"));
            BarcodeQRCode qrcode = new BarcodeQRCode("Moby Dick by Herman Melville");
            img = new Image(qrcode.CreateFormXObject(pdfDoc));
            doc.Add(img.ScaleToFit(250, 250));

            doc.Close();
        }
    }
}