namespace PdfFieldExtractor.Models;

public class PdfFieldModel
{
    public string FieldToken { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public int PageNo { get; set; }
    public double X0 { get; set; }
    public double Y0 { get; set; }
    public double X1 { get; set; }
    public double Y1 { get; set; }
}

public class TemplateResult
{
    public string TemplateName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public int TotalFields { get; set; }
    public List<PdfFieldModel> Fields { get; set; } = new();
}

public class ExtractionOutput
{
    public DateTime GeneratedAt { get; set; }
    public int TotalFiles { get; set; }
    public List<TemplateResult> Results { get; set; } = new();
}
