# PdfFieldExtractor — GitHub Copilot Friendly Instructions

## Purpose
- Short: a small WinForms tool that extracts PDF form fields and generates SQL.

## Quick prerequisites
- .NET 8 SDK installed (Windows recommended for WinForms UI).
- Any PDF library used by the project (check `PdfFieldExtractor.csproj`).

## Build and run
Run from the repo root:

```powershell
dotnet build PdfFieldExtractor/PdfFieldExtractor.csproj
dotnet run --project PdfFieldExtractor/PdfFieldExtractor.csproj
```

Or launch the built EXE under `PdfFieldExtractor/bin/Debug/net8.0-windows/`.

## Project overview (key files)
- `Program.cs` — app entry, calls `Application.Run(new Form1())`.
- `Form1.cs` / `Form1.Designer.cs` — UI and event handlers for user actions.
- `Models/PdfFieldModel.cs` — model representing a PDF field.
- `Services/PdfExtractorService.cs` — responsible for reading PDFs and extracting fields.
- `Services/SqlGeneratorService.cs` — builds SQL statements from extracted fields.

## Where to start when editing
- Add or update extraction logic in `PdfExtractorService`. Keep methods small and return `List<PdfFieldModel>` or a well-defined DTO.
- Centralize SQL formatting in `SqlGeneratorService` so UI code only passes data and receives SQL strings.

## Copilot usage tips (how to prompt Copilot effectively in this repo)
- Give a short method signature and desired return type. Example comment above a stub in `PdfExtractorService`:

```csharp
// TODO: implement ExtractFields(Stream pdfStream) -> List<PdfFieldModel>
// Input: PDF stream; Output: list of fields with name, type, and sample value.
public List<PdfFieldModel> ExtractFields(Stream pdfStream) { /* ... */ }
```

- When asking Copilot to generate code, include constraints: target framework `.NET 8`, avoid native interop, prefer a known PDF library (iText7, PdfSharp, etc.).
- For SQL generation helpers, give an example input and expected SQL output in a comment so Copilot produces deterministic code.

## Example prompts to leave as comments for Copilot
- "Create a method `ExtractFields(Stream pdf)` that returns `List<PdfFieldModel>` by reading AcroForm fields and mapping their names, values, and field types. Add null-safety and unit-testable behavior."
- "Add a method `GenerateCreateTableSql(string tableName, IEnumerable<PdfFieldModel> fields)` that returns a single SQL `CREATE TABLE` statement using simple mappings (text->VARCHAR(255), number->DECIMAL)."

## Testing and debug tips
- Add small unit tests for `SqlGeneratorService` mapping rules (pure functions).
- For `PdfExtractorService`, use small sample PDFs checked into a `tests/samples/` folder to validate behavior.

## Extension ideas (good first issues)
- Add file import support (drop multiple PDFs) in `Form1` and batch SQL generation.
- Add config for SQL dialect (SQLite, SQL Server, MySQL) in `SqlGeneratorService`.

## Where I edited / added this file
See the file: PdfFieldExtractor/README_Copilot.md

---
If you want, I can: run a `dotnet build`, add unit-test scaffolding, or open a few TODO stubs in `PdfExtractorService` for Copilot to complete.
