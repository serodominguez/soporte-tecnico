using iTextSharp.text.pdf;
using Sistema.Api.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Api.Util
{
    public class GenerarPDF
    {
        Conversion conversion = new Conversion();
        public GenerarPDF()
        {

        }
        public string GenerateInvestorDocument(DocumentosModel model)
        {
            string fullName = model.Personal;
            string carnet = model.Carnet.ToString();
            string total = model.Total.ToString();
            string direccion = model.Direccion;
            string literal = conversion.enletras(total).ToLower();
            string filePath = @"D:\Template\";
            string fileNameExisting = @"DocumentoComodato.pdf";
            string fileNameNew = @"Comodato_" + fullName.Replace(" ", "").Trim() + ".pdf";
            string fullNewPath = filePath + fileNameNew;
            string fullExistingPath = filePath + fileNameExisting;
            using (var existingFileStream = new FileStream(fullExistingPath, FileMode.Open))

            using (var newFileStream = new FileStream(fullNewPath, FileMode.Create))
            {
                var pdfReader = new iTextSharp.text.pdf.PdfReader(existingFileStream);
                var stamper = new PdfStamper(pdfReader, newFileStream);
                AcroFields fields = stamper.AcroFields;
                fields.SetField("Trabajador", fullName);
                fields.SetField("Nombre", fullName);
                fields.SetField("Carnet", carnet);
                fields.SetField("Total", total);
                fields.SetField("Direccion", direccion);
                fields.SetField("Literal", literal);
                fields.SetField("Fecha", DateTime.Now.ToString("D",CultureInfo.CreateSpecificCulture("es-ES")));
                stamper.FormFlattening = true;

                stamper.Close();
                pdfReader.Close();

                return fullNewPath;
            }
        }
    }
}
