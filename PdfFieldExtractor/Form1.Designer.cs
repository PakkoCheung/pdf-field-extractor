namespace PdfFieldExtractor;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        btnSelectPdf = new Button();
        lstPdfFiles = new ListBox();
        btnGetCoords = new Button();
        grpJson = new GroupBox();
        lblJsonFile = new Label();
        btnSelectJson = new Button();
        btnGenSql = new Button();
        lblStatus = new Label();
        grpJson.SuspendLayout();
        SuspendLayout();

        // ── btnSelectPdf ─────────────────────────────────────────────────────
        btnSelectPdf.Location = new Point(12, 12);
        btnSelectPdf.Name = "btnSelectPdf";
        btnSelectPdf.Size = new Size(180, 32);
        btnSelectPdf.TabIndex = 0;
        btnSelectPdf.Text = "1. Select PDF Files";
        btnSelectPdf.UseVisualStyleBackColor = true;
        btnSelectPdf.Click += btnSelectPdf_Click;

        // ── lstPdfFiles ───────────────────────────────────────────────────────
        lstPdfFiles.FormattingEnabled = true;
        lstPdfFiles.ItemHeight = 15;
        lstPdfFiles.Location = new Point(12, 54);
        lstPdfFiles.Name = "lstPdfFiles";
        lstPdfFiles.AllowDrop = true;
        lstPdfFiles.DragEnter += lstPdfFiles_DragEnter;
        lstPdfFiles.DragDrop += lstPdfFiles_DragDrop;
        lstPdfFiles.Size = new Size(580, 139);
        lstPdfFiles.TabIndex = 1;

        // ── btnGetCoords ─────────────────────────────────────────────────────
        btnGetCoords.Location = new Point(12, 206);
        btnGetCoords.Name = "btnGetCoords";
        btnGetCoords.Size = new Size(180, 32);
        btnGetCoords.TabIndex = 2;
        btnGetCoords.Text = "2. Get X/Y Coordinator";
        btnGetCoords.UseVisualStyleBackColor = true;
        btnGetCoords.Click += btnGetCoords_Click;

        // ── grpJson ───────────────────────────────────────────────────────────
        grpJson.Controls.Add(lblJsonFile);
        grpJson.Controls.Add(btnSelectJson);
        grpJson.Location = new Point(12, 252);
        grpJson.Name = "grpJson";
        grpJson.Size = new Size(580, 60);
        grpJson.TabIndex = 3;
        grpJson.TabStop = false;
        grpJson.Text = "JSON Output File";

        // ── lblJsonFile ───────────────────────────────────────────────────────
        lblJsonFile.AutoSize = true;
        lblJsonFile.Location = new Point(12, 28);
        lblJsonFile.Name = "lblJsonFile";
        lblJsonFile.Size = new Size(46, 15);
        lblJsonFile.TabIndex = 0;
        lblJsonFile.Text = "(none)";

        // ── btnSelectJson ─────────────────────────────────────────────────────
        btnSelectJson.Location = new Point(480, 24);
        btnSelectJson.Name = "btnSelectJson";
        btnSelectJson.Size = new Size(90, 26);
        btnSelectJson.TabIndex = 1;
        btnSelectJson.Text = "Browse…";
        btnSelectJson.UseVisualStyleBackColor = true;
        btnSelectJson.Click += btnSelectJson_Click;

        // ── btnGenSql ─────────────────────────────────────────────────────────
        btnGenSql.Location = new Point(12, 328);
        btnGenSql.Name = "btnGenSql";
        btnGenSql.Size = new Size(180, 32);
        btnGenSql.TabIndex = 4;
        btnGenSql.Text = "3. Gen Insert SQL";
        btnGenSql.UseVisualStyleBackColor = true;
        btnGenSql.Click += btnGenSql_Click;

        // ── lblStatus ─────────────────────────────────────────────────────────
        lblStatus.AutoSize = false;
        lblStatus.Location = new Point(12, 380);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(580, 20);
        lblStatus.TabIndex = 5;
        lblStatus.Text = "Ready.";

        // ── Form1 ─────────────────────────────────────────────────────────────
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(618, 420);
        Controls.Add(lblStatus);
        Controls.Add(btnGenSql);
        Controls.Add(grpJson);
        Controls.Add(btnGetCoords);
        Controls.Add(lstPdfFiles);
        Controls.Add(btnSelectPdf);
        MinimumSize = new Size(634, 459);
        Name = "Form1";
        Text = "PDF Field Extractor";
        grpJson.ResumeLayout(false);
        grpJson.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Button btnSelectPdf;
    private ListBox lstPdfFiles;
    private Button btnGetCoords;
    private GroupBox grpJson;
    private Label lblJsonFile;
    private Button btnSelectJson;
    private Button btnGenSql;
    private Label lblStatus;
}
