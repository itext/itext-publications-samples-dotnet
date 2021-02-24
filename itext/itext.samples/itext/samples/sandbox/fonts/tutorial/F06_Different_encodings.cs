using System;
using System.IO;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts.Tutorial
{
    public class F06_Different_encodings
    {
        public static readonly String DEST = "results/sandbox/fonts/tutorial/f06_different_encodings.pdf";

        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new F06_Different_encodings().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont french = PdfFontFactory.CreateFont(FONT, "WINANSI",
                PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            PdfFont czech = PdfFontFactory.CreateFont(FONT, "Cp1250", 
                PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            PdfFont russian = PdfFontFactory.CreateFont(FONT, "Cp1251", 
                PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            // The text line is "Vous êtes d'où?"
            doc.Add(new Paragraph("Vous \u00eates d'o\u00f9?").SetFont(french));

            // The text line is "À tout à l'heure. À bientôt."
            doc.Add(new Paragraph("\u00c0 tout \u00e0 l'heure. \u00c0 bient\u00f4t.").SetFont(french));

            // The text line is "Je me présente."
            doc.Add(new Paragraph("Je me pr\u00e9sente.").SetFont(french));

            // The text line is "C'est un étudiant."
            doc.Add(new Paragraph("C'est un \u00e9tudiant.").SetFont(french));

            // The text line is "Ça va?"
            doc.Add(new Paragraph("\u00c7a va?").SetFont(french));

            // The text line is "Il est ingénieur. Elle est médecin."
            doc.Add(new Paragraph("Il est ing\u00e9nieur. Elle est m\u00e9decin.").SetFont(french));

            // The text line is "C'est une fenêtre."
            doc.Add(new Paragraph("C'est une fen\u00eatre.").SetFont(french));

            // The text line is "Répétez, s'il vous plaît."
            doc.Add(new Paragraph("R\u00e9p\u00e9tez, s'il vous pla\u00eet.").SetFont(french));
            doc.Add(new Paragraph("Odkud jste?").SetFont(czech));

            // The text line is "Uvidíme se za chvilku. Měj se."
            doc.Add(new Paragraph("Uvid\u00edme se za chvilku. M\u011bj se.").SetFont(czech));

            // The text line is "Dovolte, abych se představil."
            doc.Add(new Paragraph("Dovolte, abych se p\u0159edstavil.").SetFont(czech));
            doc.Add(new Paragraph("To je studentka.").SetFont(czech));

            // The text line is "Všechno v pořádku?"
            doc.Add(new Paragraph("V\u0161echno v po\u0159\u00e1dku?").SetFont(czech));

            // The text line is "On je inženýr. Ona je lékař."
            doc.Add(new Paragraph("On je in\u017een\u00fdr. Ona je l\u00e9ka\u0159.").SetFont(czech));
            doc.Add(new Paragraph("Toto je okno.").SetFont(czech));

            // The text line is "Zopakujte to prosím"
            doc.Add(new Paragraph("Zopakujte to pros\u00edm.").SetFont(czech));

            // The text line is "Откуда ты?"
            doc.Add(new Paragraph("\u041e\u0442\u043a\u0443\u0434\u0430 \u0442\u044b?")
                .SetFont(russian));

            // The text line is "Увидимся позже. Увидимся."
            doc.Add(new Paragraph("\u0423\u0432\u0438\u0434\u0438\u043c\u0441\u044f "
                                  + "\u043f\u043E\u0437\u0436\u0435. \u0423\u0432\u0438\u0434\u0438\u043c\u0441\u044f.")
                .SetFont(russian));

            // The text line is "Позвольте мне представиться."
            doc.Add(new Paragraph("\u041f\u043e\u0437\u0432\u043e\u043b\u044c\u0442\u0435 \u043c\u043d\u0435 "
                                  + "\u043f\u0440\u0435\u0434\u0441\u0442\u0430\u0432\u0438\u0442\u044c\u0441\u044f.")
                .SetFont(russian));

            // The text line is "Это студент."
            doc.Add(new Paragraph("\u042d\u0442\u043e \u0441\u0442\u0443\u0434\u0435\u043d\u0442.")
                .SetFont(russian));

            // The text line is "Хорошо?"
            doc.Add(new Paragraph("\u0425\u043e\u0440\u043e\u0448\u043e?")
                .SetFont(russian));

            // The text line is "Он инженер. Она доктор."
            doc.Add(new Paragraph("\u041e\u043d \u0438\u043d\u0436\u0435\u043d\u0435\u0440. "
                                  + "\u041e\u043d\u0430 \u0434\u043e\u043a\u0442\u043e\u0440.")
                .SetFont(russian));

            // The text line is "Это окно."
            doc.Add(new Paragraph("\u042d\u0442\u043e \u043e\u043a\u043d\u043e.")
                .SetFont(russian));

            // The text line is "Повторите, пожалуйста."
            doc.Add(new Paragraph("\u041f\u043e\u0432\u0442\u043e\u0440\u0438\u0442\u0435, "
                                  + "\u043f\u043e\u0436\u0430\u043b\u0443\u0439\u0441\u0442\u0430.")
                .SetFont(russian));

            doc.Close();
        }
    }
}