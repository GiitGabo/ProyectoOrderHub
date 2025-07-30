using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using JarredsOrderHub.Models;
using System.Reflection.Metadata;

namespace JarredsOrderHub.Controllers.Service
{
    public class ExportacionService : IExportacionService
    {
        public async Task<byte[]> GenerarReporteVentasPDF(List<Reporte> reportes)
        {
            using (var ms = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Título del documento
                document.Add(new Paragraph("Reporte de Ventas"));

                // Tabla de contenido
                var table = new PdfPTable(3); // Definir 3 columnas: Producto, Cantidad Vendida, Total Vendido
                table.AddCell("Producto");
                table.AddCell("Cantidad Vendida");
                table.AddCell("Total Vendido");

                foreach (var reporte in reportes)
                {
                    table.AddCell(reporte.DescripcionReporte); // Producto
                    table.AddCell("Cantidad Vendida"); // Aquí puedes poner el dato real de cantidad
                    table.AddCell("Total Vendido"); // Aquí puedes poner el dato real de total vendido
                }

                document.Add(table);
                document.Close();
                return ms.ToArray(); // Devuelve el contenido PDF como byte array
            }
        }

        public async Task<byte[]> GenerarReporteVentasExcel(List<Reporte> reportes)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Ventas");

                // Encabezados
                worksheet.Cells[1, 1].Value = "Producto";
                worksheet.Cells[1, 2].Value = "Cantidad Vendida";
                worksheet.Cells[1, 3].Value = "Total Vendido";

                // Agregar filas con los datos de reportes
                int row = 2;
                foreach (var reporte in reportes)
                {
                    worksheet.Cells[row, 1].Value = reporte.DescripcionReporte; // Producto
                    worksheet.Cells[row, 2].Value = "Cantidad Vendida"; // Aquí puedes poner el dato real de cantidad
                    worksheet.Cells[row, 3].Value = "Total Vendido"; // Aquí puedes poner el dato real de total vendido
                    row++;
                }

                return await Task.FromResult(package.GetAsByteArray()); // Devuelve el archivo Excel como byte array
            }
        }
    }
}
