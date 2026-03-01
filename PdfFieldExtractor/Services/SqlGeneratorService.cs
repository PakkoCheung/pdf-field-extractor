using System.Text;
using PdfFieldExtractor.Models;

namespace PdfFieldExtractor.Services;

public static class SqlGeneratorService
{
    public static string GenerateSql(ExtractionOutput output)
    {
        var sb = new StringBuilder();
        sb.AppendLine("-- Generated INSERT statements for pdf_fields table");
        sb.AppendLine($"-- GeneratedAt: {output.GeneratedAt:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine();

        foreach (var template in output.Results)
        {
            foreach (var field in template.Fields)
            {
                sb.AppendLine(
                    $"INSERT INTO pdf_fields (template_name, field_name, field_token, page_no, x0, y0, x1, y1) VALUES (" +
                    $"'{EscapeSql(template.TemplateName)}', " +
                    $"'{EscapeSql(field.FieldName)}', " +
                    $"'{EscapeSql(field.FieldToken)}', " +
                    $"{field.PageNo}, " +
                    $"{field.X0}, {field.Y0}, {field.X1}, {field.Y1}" +
                    $");");
            }
        }

        return sb.ToString();
    }

    private static string EscapeSql(string value) => value.Replace("'", "''");
}
