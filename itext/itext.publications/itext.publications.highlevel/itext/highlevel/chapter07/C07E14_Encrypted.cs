using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter07 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C07E14_Encrypted {
        public const String DEST = "../../../results/chapter07/encrypted.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E14_Encrypted().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            byte[] user = GetBytes("It's Hyde");
            string test = "test";
           
            byte[] owner = GetBytes("abcdefg");
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest, new WriterProperties().SetStandardEncryption(user, owner
                , EncryptionConstants.ALLOW_PRINTING | EncryptionConstants.ALLOW_ASSEMBLY, EncryptionConstants.ENCRYPTION_AES_256
                )));
            Document document = new Document(pdf);
            document.Add(new Paragraph("Mr. Jekyll has a secret: he changes into Mr. Hyde."));
            document.Close();
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                bytes[i] = System.Buffer.GetByte(str.ToCharArray(), 2 * i);
            }
            //System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


    }
}
