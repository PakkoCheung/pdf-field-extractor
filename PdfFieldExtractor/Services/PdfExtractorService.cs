using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using PdfFieldExtractor.Models;

namespace PdfFieldExtractor.Services;

public static class PdfExtractorService
{
    private static readonly Regex PlaceholderRegex = new(@"<<[A-Za-z0-9_]+>>", RegexOptions.Compiled);

    public static ExtractionOutput ExtractFromFiles(IEnumerable<string> filePaths)
    {
        var output = new ExtractionOutput
        {
            GeneratedAt = DateTime.Now,
            Results = new List<TemplateResult>()
        };

        foreach (var filePath in filePaths)
        {
            var result = ExtractFromFile(filePath);
            output.Results.Add(result);
        }

        output.TotalFiles = output.Results.Count;
        return output;
    }

    private static TemplateResult ExtractFromFile(string filePath)
    {
        var result = new TemplateResult
        {
            TemplateName = Path.GetFileName(filePath),
            FilePath = filePath,
            Fields = new List<PdfFieldModel>()
        };

        using var document = PdfDocument.Open(filePath);

        for (int pageIndex = 0; pageIndex < document.NumberOfPages; pageIndex++)
        {
            var page = document.GetPage(pageIndex + 1);
            ExtractFieldsFromPage(page, pageIndex + 1, result.Fields);
        }

        result.TotalFields = result.Fields.Count;
        return result;
    }

    private static void ExtractFieldsFromPage(Page page, int pageNo, List<PdfFieldModel> fields)
    {
        // Group words by their approximate Y coordinate (line grouping)
        var words = page.GetWords().ToList();

        // Group by rounded Y baseline to reconstruct lines
        var lineGroups = words
            .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 1))
            .OrderByDescending(g => g.Key);

        foreach (var lineGroup in lineGroups)
        {
            var lineWords = lineGroup.OrderBy(w => w.BoundingBox.Left).ToList();
            var lineText = string.Join(" ", lineWords.Select(w => w.Text));

            // Match placeholders in the stitched line text
            var matches = PlaceholderRegex.Matches(lineText);
            if (matches.Count == 0) continue;

            // Build a position map: character index -> word bounding boxes
            // We need to find which words contribute to each match
            foreach (Match match in matches)
            {
                var bbox = FindBoundingBoxForMatch(lineText, match, lineWords);
                if (bbox == null) continue;

                var token = match.Value;
                var fieldName = token.TrimStart('<').TrimEnd('>');

                fields.Add(new PdfFieldModel
                {
                    FieldToken = token,
                    FieldName = fieldName,
                    PageNo = pageNo,
                    X0 = Math.Round(bbox.Value.x0, 4),
                    Y0 = Math.Round(bbox.Value.y0, 4),
                    X1 = Math.Round(bbox.Value.x1, 4),
                    Y1 = Math.Round(bbox.Value.y1, 4)
                });
            }
        }
    }

    private static (double x0, double y0, double x1, double y1)? FindBoundingBoxForMatch(
        string lineText, Match match, List<Word> lineWords)
    {
        // Rebuild the line with word positions to map character index -> word
        double x0 = double.MaxValue, y0 = double.MaxValue;
        double x1 = double.MinValue, y1 = double.MinValue;
        bool found = false;

        int charPos = 0;
        foreach (var word in lineWords)
        {
            int wordStart = charPos;
            int wordEnd = charPos + word.Text.Length;

            // Check overlap between [wordStart, wordEnd) and [match.Index, match.Index+match.Length)
            bool overlaps = wordStart < match.Index + match.Length && wordEnd > match.Index;
            if (overlaps)
            {
                x0 = Math.Min(x0, word.BoundingBox.Left);
                y0 = Math.Min(y0, word.BoundingBox.Bottom);
                x1 = Math.Max(x1, word.BoundingBox.Right);
                y1 = Math.Max(y1, word.BoundingBox.Top);
                found = true;
            }

            charPos = wordEnd + 1; // +1 for the space separator
        }

        if (!found) return null;
        return (x0, y0, x1, y1);
    }
}
