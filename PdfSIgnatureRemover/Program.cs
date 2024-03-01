using iText.Forms;
using iText.Kernel.Pdf;
using System;

class Program
{
    static void Main()
    {
        string inputPdfPath = @"C:\Users\ASUS\Desktop\Pdf sign\sign.pdf";
        string outputPdfPath = @"C:\Users\ASUS\Desktop\Pdf sign\removedsign.pdf";

        try
        {
            RemoveSignature(inputPdfPath, outputPdfPath);
            Console.WriteLine("Digital signature removed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

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

                // Optionally remove other signature-related information from the document
                // pdfDoc.GetCatalog().GetPdfObject().Remove(PdfName.Perms);
                // pdfDoc.GetCatalog().GetPdfObject().Remove(PdfName.Signatures);
                // pdfDoc.GetCatalog().GetPdfObject().Remove(PdfName.AA);
                // pdfDoc.GetCatalog().GetPdfObject().Remove(PdfName.V);

                pdfDoc.Close();
            }
        }
    }
}