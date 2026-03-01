using System;
using System.Collections.Generic;
using System.Text;
using PdfFieldExtractor.Models;

namespace PdfFieldExtractor.Services;

public static class SqlGeneratorService
{
    public static string GenerateSql(ExtractionOutput output)
    {
        var sb = new StringBuilder();
        var settings = AppConfig.Settings;
        var table = string.IsNullOrWhiteSpace(settings.TableName) ? "pdf_fields" : settings.TableName;
        var fieldColumn = string.IsNullOrWhiteSpace(settings.FieldName) ? "field_name" : settings.FieldName;

        sb.AppendLine($"-- Generated SQL for table: {table}");
        sb.AppendLine($"-- Mode: {settings.SqlMode}");
        sb.AppendLine($"-- GeneratedAt: {output.GeneratedAt:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine();

        bool isUpdate = string.Equals(settings.SqlMode, "Update", StringComparison.OrdinalIgnoreCase);

        foreach (var template in output.Results)
        {
            foreach (var field in template.Fields)
            {
                if (!isUpdate)
                {
                    sb.AppendLine(
                        $"INSERT INTO {EscapeIdentifier(table)} (template_name, {EscapeIdentifier(fieldColumn)}, field_token, page_no, x0, y0, x1, y1) VALUES (" +
                        $"'{EscapeSql(template.TemplateName)}', " +
                        $"'{EscapeSql(field.FieldName)}', " +
                        $"'{EscapeSql(field.FieldToken)}', " +
                        $"{field.PageNo}, " +
                        $"{field.X0}, {field.Y0}, {field.X1}, {field.Y1}" +
                        $");");
                }
                else
                {
                    // Build SET clause
                    var setClause =
                        $"field_token = '{EscapeSql(field.FieldToken)}', page_no = {field.PageNo}, x0 = {field.X0}, y0 = {field.Y0}, x1 = {field.X1}, y1 = {field.Y1}";

                    // Build WHERE clause from configured unique keys
                    var whereParts = new List<string>();
                    foreach (var key in settings.UpdateUniqueKeys ?? Array.Empty<string>())
                    {
                        if (string.Equals(key, "TemplateName", StringComparison.OrdinalIgnoreCase))
                            whereParts.Add($"template_name = '{EscapeSql(template.TemplateName)}'");
                        else if (string.Equals(key, "FieldName", StringComparison.OrdinalIgnoreCase))
                            whereParts.Add($"{EscapeIdentifier(fieldColumn)} = '{EscapeSql(field.FieldName)}'");
                        else if (string.Equals(key, "FilePath", StringComparison.OrdinalIgnoreCase))
                            whereParts.Add($"file_path = '{EscapeSql(template.FilePath)}'");
                        else if (string.Equals(key, "PageNo", StringComparison.OrdinalIgnoreCase))
                            whereParts.Add($"page_no = {field.PageNo}");
                        // unknown keys are ignored
                    }

                    if (whereParts.Count == 0)
                    {
                        sb.AppendLine($"-- Skipped UPDATE for template '{EscapeSql(template.TemplateName)}', field '{EscapeSql(field.FieldName)}' because no UpdateUniqueKeys matched available data.");
                    }
                    else
                    {
                        var where = string.Join(" AND ", whereParts);
                        sb.AppendLine($"UPDATE {EscapeIdentifier(table)} SET {setClause} WHERE {where};");
                    }
                }
            }
        }

        return sb.ToString();
    }

    private static string EscapeSql(string value) => value.Replace("'", "''");

    private static string EscapeIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier)) return identifier;
        // Simple escape for identifiers: wrap with double quotes if needed (works for many SQL dialects)
        if (identifier.StartsWith("\"") && identifier.EndsWith("\"")) return identifier;
        return identifier.Contains(" ") ? $"\"{identifier}\"" : identifier;
    }
}
