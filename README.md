# PDF Field Extractor

A .NET 8 WinForms desktop application that scans PDF files for placeholder fields
(tokens of the form `<<FieldName>>`), extracts their bounding-box coordinates, and
exports the results as JSON and SQL INSERT statements.

---

## Requirements

| Component | Version |
|-----------|---------|
| .NET SDK  | 8.0+    |
| OS        | Windows (WinForms) |

---

## Building

```bash
dotnet restore PdfFieldExtractor.sln
dotnet build   PdfFieldExtractor.sln -c Release
```

---

## Running

```bash
dotnet run --project PdfFieldExtractor/PdfFieldExtractor.csproj
```

Or open `PdfFieldExtractor.sln` in Visual Studio 2022 and press **F5**.

---

## Usage

1. **1. Select PDF Files** – opens a multi-select file dialog; selected filenames
   are shown in the list box.
2. **2. Get X/Y Coordinator** – scans every selected PDF for `<<Token>>` placeholders,
   extracts bounding-box coordinates (X0, Y0, X1, Y1) and page numbers, then writes
   a JSON file to the application folder.
3. **JSON Output File** group – shows the name of the generated (or previously chosen)
   JSON file. Use **Browse…** to pick an existing JSON file.
4. **3. Gen Insert SQL** – reads the current JSON file and generates a `.sql` file
   containing `INSERT` statements for the `pdf_fields` table.

---

## JSON Output Format

```json
{
  "GeneratedAt": "2024-01-15T10:30:00",
  "TotalFiles": 2,
  "Results": [
    {
      "TemplateName": "invoice_template.pdf",
      "FilePath": "C:\\Templates\\invoice_template.pdf",
      "TotalFields": 3,
      "Fields": [
        {
          "FieldToken": "<<InvoiceNumber>>",
          "FieldName": "InvoiceNumber",
          "PageNo": 1,
          "X0": 72.0,
          "Y0": 680.0,
          "X1": 210.0,
          "Y1": 695.0
        }
      ]
    }
  ]
}
```

### Field descriptions

| Property | Description |
|----------|-------------|
| `FieldToken` | Full placeholder text, e.g. `<<InvoiceNumber>>` |
| `FieldName`  | Token without angle-bracket delimiters, e.g. `InvoiceNumber` |
| `PageNo`     | 1-based page number where the placeholder appears |
| `X0`, `Y0`   | Bottom-left corner of the bounding box (PDF coordinate space) |
| `X1`, `Y1`   | Top-right corner of the bounding box (PDF coordinate space) |

---

## SQL Output Format

```sql
INSERT INTO pdf_fields (template_name, field_name, field_token, page_no, x0, y0, x1, y1)
VALUES ('invoice_template.pdf', 'InvoiceNumber', '<<InvoiceNumber>>', 1, 72.0, 680.0, 210.0, 695.0);
```

---

## Project Structure

```
PdfFieldExtractor.sln
PdfFieldExtractor/
├── PdfFieldExtractor.csproj
├── Program.cs
├── Form1.cs
├── Form1.Designer.cs
├── Models/
│   └── PdfFieldModel.cs          # ExtractionOutput, TemplateResult, PdfFieldModel
└── Services/
    ├── PdfExtractorService.cs     # PDF scanning & field extraction logic
    └── SqlGeneratorService.cs     # SQL INSERT statement generator
```

---

## Placeholder Format

Placeholders must match the regex `<<[A-Za-z0-9_]+>>`.  
Examples: `<<CustomerName>>`, `<<Order_ID>>`, `<<InvoiceDate>>`

Placeholders split across multiple text spans on the same line are handled by
stitching spans together before matching.
