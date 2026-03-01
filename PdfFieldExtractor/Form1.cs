using Newtonsoft.Json;
using PdfFieldExtractor.Models;
using PdfFieldExtractor.Services;

namespace PdfFieldExtractor;

public partial class Form1 : Form
{
    private readonly List<string> _selectedPdfPaths = new();
    private string _currentJsonFile = string.Empty;

    public Form1()
    {
        InitializeComponent();
    }

    // ── 1. Select PDF Files ──────────────────────────────────────────────────
    private void btnSelectPdf_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Select PDF Files",
            Filter = "PDF Files (*.pdf)|*.pdf",
            Multiselect = true
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        _selectedPdfPaths.Clear();
        _selectedPdfPaths.AddRange(dlg.FileNames);

        lstPdfFiles.Items.Clear();
        foreach (var path in _selectedPdfPaths)
            lstPdfFiles.Items.Add(Path.GetFileName(path));

        lblStatus.Text = $"{_selectedPdfPaths.Count} file(s) selected.";
    }

    // ── 2. Get X/Y Coordinator ───────────────────────────────────────────────
    private void btnGetCoords_Click(object sender, EventArgs e)
    {
        if (_selectedPdfPaths.Count == 0)
        {
            MessageBox.Show("Please select at least one PDF file first.", "No Files",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            lblStatus.Text = "Extracting fields…";
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            var output = PdfExtractorService.ExtractFromFiles(_selectedPdfPaths);

            var fileName = $"{DateTime.Now:yyyyMMddHHmmss}.json";
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var json = JsonConvert.SerializeObject(output, Formatting.Indented);
            File.WriteAllText(fullPath, json);

            _currentJsonFile = fullPath;
            lblJsonFile.Text = fileName;
            lblStatus.Text = $"Done. Fields extracted and saved to {fileName}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error during extraction:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "Extraction failed.";
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    // ── Select existing JSON ─────────────────────────────────────────────────
    private void btnSelectJson_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Select JSON File",
            Filter = "JSON Files (*.json)|*.json",
            Multiselect = false
        };

        if (dlg.ShowDialog() != DialogResult.OK) return;

        _currentJsonFile = dlg.FileName;
        lblJsonFile.Text = Path.GetFileName(_currentJsonFile);
        lblStatus.Text = $"JSON file selected: {lblJsonFile.Text}";
    }

    // ── 3. Generate Insert SQL (stub) ────────────────────────────────────────
    private void btnGenSql_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_currentJsonFile) || !File.Exists(_currentJsonFile))
        {
            MessageBox.Show("Please extract fields or select a JSON file first.", "No JSON",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var json = File.ReadAllText(_currentJsonFile);
            var output = JsonConvert.DeserializeObject<ExtractionOutput>(json);
            if (output == null) throw new InvalidOperationException("Could not parse JSON file.");

            var sql = SqlGeneratorService.GenerateSql(output);

            var sqlFileName = Path.ChangeExtension(_currentJsonFile, ".sql");
            File.WriteAllText(sqlFileName, sql);

            lblStatus.Text = $"SQL generated: {Path.GetFileName(sqlFileName)}";
            MessageBox.Show($"SQL file written to:\n{sqlFileName}", "SQL Generated",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating SQL:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "SQL generation failed.";
        }
    }
}
