# Copilot Instructions (short)

Purpose: give Copilot quick repo-level rules to follow when writing or editing code here.

Rules:
- Target framework: .NET 8 / WinForms on Windows.
- Keep methods small, testable, and null-safe.
- Prefer managed C# libraries (avoid native interop). For PDFs prefer iText7 or PdfSharp.
- SQL generation: keep mappings simple (text -> VARCHAR(255), number -> DECIMAL).

How to respond:
- Return small, focused functions with clear signatures and return types.
- Add unit-testable pure helpers in `Services/` and keep UI code in `Form1` minimal.

Examples for prompts:
- "Implement `List<PdfFieldModel> ExtractFields(Stream pdf)` reading AcroForm fields."
- "Create `string GenerateCreateTableSql(string name, IEnumerable<PdfFieldModel> fields)` using simple mappings."
