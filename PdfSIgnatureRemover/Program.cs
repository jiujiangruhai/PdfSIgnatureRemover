using iText.Forms;
using iText.Kernel.Pdf;

class Program
{
    static void Main()
    {
        //Hardcoded pdf path for pdf document.
        string inputPdfPath = @"C:\Users\ASUS\Desktop\Pdf sign\sign.pdf";

        //Hardcoded output pdf document with removed signature.
        string outputPdfPath = @"C:\Users\ASUS\Desktop\Pdf sign\removedsign.pdf";

        try
        {
            RemoveSignature(inputPdfPath, outputPdfPath);
            Console.WriteLine("Digital signature removed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }
    }

    //Method for removing signatures
    static void RemoveSignature(string inputPdfPath, string outputPdfPath)
    {
        using (PdfReader reader = new PdfReader(inputPdfPath))
        using (PdfWriter writer = new PdfWriter(outputPdfPath))
        {
            using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
            {
                PdfAcroForm acroForm = PdfAcroForm.GetAcroForm(pdfDoc, true);

                foreach (var field in acroForm.GetAllFormFields().Values)
                {
                    if (field.GetWidgets() != null && field.GetWidgets().Count > 0)
                    {
                        foreach (var widget in field.GetWidgets())
                        {
                            widget.GetPdfObject().Remove(PdfName.V);
                            widget.GetPdfObject().Remove(PdfName.AP);
                        }
                    }
                }

                 //Optionally remove other signature-related information from the document
                //pdfDoc.GetCatalog().GetPdfObject().Remove(PdfName.Perms);
                //pdfDoc.GetCatalog().GetPdfObject().Remove(PdfName.Signed);
                //pdfDoc.GetCatalog().GetPdfObject().Remove(PdfName.AA);
                //pdfDoc.GetCatalog().GetPdfObject().Remove(PdfName.V);

                pdfDoc.Close();
            }
        }
    }
}