<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Friend Class F_AdvanceReport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim AccountLabel As System.Windows.Forms.Label
        Dim CommentsLabel As System.Windows.Forms.Label
        Dim CommentsInternalLabel As System.Windows.Forms.Label
        Dim ContentLabel As System.Windows.Forms.Label
        Dim CurrencyCodeLabel As System.Windows.Forms.Label
        Dim CurrencyRateLabel As System.Windows.Forms.Label
        Dim DateLabel As System.Windows.Forms.Label
        Dim DocumentNumberLabel As System.Windows.Forms.Label
        Dim IDLabel As System.Windows.Forms.Label
        Dim PersonLabel As System.Windows.Forms.Label
        Dim SumLabel As System.Windows.Forms.Label
        Dim SumTotalLabel As System.Windows.Forms.Label
        Dim SumVatLabel As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(F_AdvanceReport))
        Me.AccountAccGridComboBox = New AccControlsWinForms.AccListComboBox
        Me.AdvanceReportBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.CommentsTextBox = New System.Windows.Forms.TextBox
        Me.CommentsInternalTextBox = New System.Windows.Forms.TextBox
        Me.ContentTextBox = New System.Windows.Forms.TextBox
        Me.CurrencyCodeComboBox = New System.Windows.Forms.ComboBox
        Me.CurrencyRateAccTextBox = New AccControlsWinForms.AccTextBox
        Me.DocumentNumberTextBox = New System.Windows.Forms.TextBox
        Me.IDTextBox = New System.Windows.Forms.TextBox
        Me.PersonAccGridComboBox = New AccControlsWinForms.AccListComboBox
        Me.SumAccTextBox = New AccControlsWinForms.AccTextBox
        Me.SumTotalAccTextBox = New AccControlsWinForms.AccTextBox
        Me.SumVatAccTextBox = New AccControlsWinForms.AccTextBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.CurrencyCodeLabel2 = New System.Windows.Forms.Label
        Me.GetCurrencyRatesButton = New System.Windows.Forms.Button
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel11 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel9 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel10 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel8 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.ViewJournalEntryButton = New System.Windows.Forms.Button
        Me.DateAccDatePicker = New AccControlsWinForms.AccDatePicker
        Me.AddAdvanceReportItemButton = New System.Windows.Forms.Button
        Me.ReportItemsSortedBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ICancelButton = New System.Windows.Forms.Button
        Me.IOkButton = New System.Windows.Forms.Button
        Me.IApplyButton = New System.Windows.Forms.Button
        Me.ReportItemsDataListView = New BrightIdeasSoftware.DataListView
        Me.OlvColumn2 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn1 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn3 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn4 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn5 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn6 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn7 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn8 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn9 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn10 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn34 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn11 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn12 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn13 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn14 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn15 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn16 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn17 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn18 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn19 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn20 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn21 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn22 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn23 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn24 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn25 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn26 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn27 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn28 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn29 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn30 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn31 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn32 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn33 = New BrightIdeasSoftware.OLVColumn
        Me.ProgressFiller1 = New AccControlsWinForms.ProgressFiller
        Me.ProgressFiller2 = New AccControlsWinForms.ProgressFiller
        Me.ErrorWarnInfoProvider1 = New AccControlsWinForms.ErrorWarnInfoProvider(Me.components)
        AccountLabel = New System.Windows.Forms.Label
        CommentsLabel = New System.Windows.Forms.Label
        CommentsInternalLabel = New System.Windows.Forms.Label
        ContentLabel = New System.Windows.Forms.Label
        CurrencyCodeLabel = New System.Windows.Forms.Label
        CurrencyRateLabel = New System.Windows.Forms.Label
        DateLabel = New System.Windows.Forms.Label
        DocumentNumberLabel = New System.Windows.Forms.Label
        IDLabel = New System.Windows.Forms.Label
        PersonLabel = New System.Windows.Forms.Label
        SumLabel = New System.Windows.Forms.Label
        SumTotalLabel = New System.Windows.Forms.Label
        SumVatLabel = New System.Windows.Forms.Label
        CType(Me.AdvanceReportBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel11.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.TableLayoutPanel9.SuspendLayout()
        Me.TableLayoutPanel10.SuspendLayout()
        Me.TableLayoutPanel8.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        CType(Me.ReportItemsSortedBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.ReportItemsDataListView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorWarnInfoProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AccountLabel
        '
        AccountLabel.AutoSize = True
        AccountLabel.Dock = System.Windows.Forms.DockStyle.Fill
        AccountLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        AccountLabel.Location = New System.Drawing.Point(3, 60)
        AccountLabel.Name = "AccountLabel"
        AccountLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        AccountLabel.Size = New System.Drawing.Size(120, 30)
        AccountLabel.TabIndex = 1
        AccountLabel.Text = "Atskaitingo sąsk.:"
        AccountLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'CommentsLabel
        '
        CommentsLabel.AutoSize = True
        CommentsLabel.Dock = System.Windows.Forms.DockStyle.Fill
        CommentsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CommentsLabel.Location = New System.Drawing.Point(3, 120)
        CommentsLabel.Name = "CommentsLabel"
        CommentsLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        CommentsLabel.Size = New System.Drawing.Size(120, 30)
        CommentsLabel.TabIndex = 3
        CommentsLabel.Text = "Komentarai:"
        CommentsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'CommentsInternalLabel
        '
        CommentsInternalLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        CommentsInternalLabel.AutoSize = True
        CommentsInternalLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CommentsInternalLabel.Location = New System.Drawing.Point(5, 150)
        CommentsInternalLabel.Name = "CommentsInternalLabel"
        CommentsInternalLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        CommentsInternalLabel.Size = New System.Drawing.Size(118, 19)
        CommentsInternalLabel.TabIndex = 5
        CommentsInternalLabel.Text = "Vidiniai komentarai:"
        CommentsInternalLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ContentLabel
        '
        ContentLabel.AutoSize = True
        ContentLabel.Dock = System.Windows.Forms.DockStyle.Fill
        ContentLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ContentLabel.Location = New System.Drawing.Point(3, 90)
        ContentLabel.Name = "ContentLabel"
        ContentLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        ContentLabel.Size = New System.Drawing.Size(120, 30)
        ContentLabel.TabIndex = 7
        ContentLabel.Text = "Turinys:"
        ContentLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'CurrencyCodeLabel
        '
        CurrencyCodeLabel.AutoSize = True
        CurrencyCodeLabel.Dock = System.Windows.Forms.DockStyle.Fill
        CurrencyCodeLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CurrencyCodeLabel.Location = New System.Drawing.Point(163, 0)
        CurrencyCodeLabel.Name = "CurrencyCodeLabel"
        CurrencyCodeLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        CurrencyCodeLabel.Size = New System.Drawing.Size(50, 30)
        CurrencyCodeLabel.TabIndex = 9
        CurrencyCodeLabel.Text = "Valiuta:"
        CurrencyCodeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'CurrencyRateLabel
        '
        CurrencyRateLabel.AutoSize = True
        CurrencyRateLabel.Dock = System.Windows.Forms.DockStyle.Fill
        CurrencyRateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CurrencyRateLabel.Location = New System.Drawing.Point(339, 0)
        CurrencyRateLabel.Name = "CurrencyRateLabel"
        CurrencyRateLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        CurrencyRateLabel.Size = New System.Drawing.Size(49, 30)
        CurrencyRateLabel.TabIndex = 11
        CurrencyRateLabel.Text = "Kursas:"
        CurrencyRateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'DateLabel
        '
        DateLabel.AutoSize = True
        DateLabel.Dock = System.Windows.Forms.DockStyle.Fill
        DateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DateLabel.Location = New System.Drawing.Point(132, 0)
        DateLabel.Name = "DateLabel"
        DateLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        DateLabel.Size = New System.Drawing.Size(38, 30)
        DateLabel.TabIndex = 13
        DateLabel.Text = "Data:"
        DateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'DocumentNumberLabel
        '
        DocumentNumberLabel.AutoSize = True
        DocumentNumberLabel.Dock = System.Windows.Forms.DockStyle.Fill
        DocumentNumberLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DocumentNumberLabel.Location = New System.Drawing.Point(344, 0)
        DocumentNumberLabel.Name = "DocumentNumberLabel"
        DocumentNumberLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        DocumentNumberLabel.Size = New System.Drawing.Size(28, 30)
        DocumentNumberLabel.TabIndex = 15
        DocumentNumberLabel.Text = "Nr.:"
        DocumentNumberLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'IDLabel
        '
        IDLabel.AutoSize = True
        IDLabel.Dock = System.Windows.Forms.DockStyle.Fill
        IDLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        IDLabel.Location = New System.Drawing.Point(3, 0)
        IDLabel.Name = "IDLabel"
        IDLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        IDLabel.Size = New System.Drawing.Size(120, 30)
        IDLabel.TabIndex = 17
        IDLabel.Text = "ID:"
        IDLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'PersonLabel
        '
        PersonLabel.AutoSize = True
        PersonLabel.Dock = System.Windows.Forms.DockStyle.Fill
        PersonLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        PersonLabel.Location = New System.Drawing.Point(3, 30)
        PersonLabel.Name = "PersonLabel"
        PersonLabel.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
        PersonLabel.Size = New System.Drawing.Size(120, 30)
        PersonLabel.TabIndex = 19
        PersonLabel.Text = "Atskaitingas asmuo:"
        PersonLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'SumLabel
        '
        SumLabel.AutoSize = True
        SumLabel.Dock = System.Windows.Forms.DockStyle.Fill
        SumLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        SumLabel.Location = New System.Drawing.Point(11, 33)
        SumLabel.Name = "SumLabel"
        SumLabel.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        SumLabel.Size = New System.Drawing.Size(42, 27)
        SumLabel.TabIndex = 21
        SumLabel.Text = "Suma:"
        SumLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'SumTotalLabel
        '
        SumTotalLabel.AutoSize = True
        SumTotalLabel.Dock = System.Windows.Forms.DockStyle.Fill
        SumTotalLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        SumTotalLabel.Location = New System.Drawing.Point(11, 87)
        SumTotalLabel.Name = "SumTotalLabel"
        SumTotalLabel.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        SumTotalLabel.Size = New System.Drawing.Size(42, 27)
        SumTotalLabel.TabIndex = 25
        SumTotalLabel.Text = "Viso:"
        SumTotalLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'SumVatLabel
        '
        SumVatLabel.AutoSize = True
        SumVatLabel.Dock = System.Windows.Forms.DockStyle.Fill
        SumVatLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        SumVatLabel.Location = New System.Drawing.Point(11, 60)
        SumVatLabel.Name = "SumVatLabel"
        SumVatLabel.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        SumVatLabel.Size = New System.Drawing.Size(42, 27)
        SumVatLabel.TabIndex = 29
        SumVatLabel.Text = "PVM:"
        SumVatLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'AccountAccGridComboBox
        '
        Me.AccountAccGridComboBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.AdvanceReportBindingSource, "Account", True))
        Me.AccountAccGridComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccountAccGridComboBox.EmptyValueString = ""
        Me.AccountAccGridComboBox.Location = New System.Drawing.Point(0, 3)
        Me.AccountAccGridComboBox.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
        Me.AccountAccGridComboBox.Name = "AccountAccGridComboBox"
        Me.AccountAccGridComboBox.Size = New System.Drawing.Size(137, 20)
        Me.AccountAccGridComboBox.TabIndex = 0
        '
        'AdvanceReportBindingSource
        '
        Me.AdvanceReportBindingSource.DataSource = GetType(ApskaitaObjects.Documents.AdvanceReport)
        '
        'CommentsTextBox
        '
        Me.CommentsTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AdvanceReportBindingSource, "Comments", True))
        Me.CommentsTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CommentsTextBox.Location = New System.Drawing.Point(3, 3)
        Me.CommentsTextBox.MaxLength = 255
        Me.CommentsTextBox.Name = "CommentsTextBox"
        Me.CommentsTextBox.Size = New System.Drawing.Size(547, 20)
        Me.CommentsTextBox.TabIndex = 0
        '
        'CommentsInternalTextBox
        '
        Me.CommentsInternalTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AdvanceReportBindingSource, "CommentsInternal", True))
        Me.CommentsInternalTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CommentsInternalTextBox.Location = New System.Drawing.Point(3, 3)
        Me.CommentsInternalTextBox.MaxLength = 255
        Me.CommentsInternalTextBox.Multiline = True
        Me.CommentsInternalTextBox.Name = "CommentsInternalTextBox"
        Me.CommentsInternalTextBox.Size = New System.Drawing.Size(547, 24)
        Me.CommentsInternalTextBox.TabIndex = 0
        '
        'ContentTextBox
        '
        Me.ContentTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AdvanceReportBindingSource, "Content", True))
        Me.ContentTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ContentTextBox.Location = New System.Drawing.Point(3, 3)
        Me.ContentTextBox.MaxLength = 255
        Me.ContentTextBox.Name = "ContentTextBox"
        Me.ContentTextBox.Size = New System.Drawing.Size(547, 20)
        Me.ContentTextBox.TabIndex = 0
        '
        'CurrencyCodeComboBox
        '
        Me.CurrencyCodeComboBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AdvanceReportBindingSource, "CurrencyCode", True))
        Me.CurrencyCodeComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CurrencyCodeComboBox.FormattingEnabled = True
        Me.CurrencyCodeComboBox.Location = New System.Drawing.Point(219, 3)
        Me.CurrencyCodeComboBox.Name = "CurrencyCodeComboBox"
        Me.CurrencyCodeComboBox.Size = New System.Drawing.Size(94, 21)
        Me.CurrencyCodeComboBox.TabIndex = 1
        '
        'CurrencyRateAccTextBox
        '
        Me.CurrencyRateAccTextBox.ButtonVisible = False
        Me.CurrencyRateAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.AdvanceReportBindingSource, "CurrencyRate", True))
        Me.CurrencyRateAccTextBox.DecimalLength = 6
        Me.CurrencyRateAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CurrencyRateAccTextBox.Location = New System.Drawing.Point(394, 3)
        Me.CurrencyRateAccTextBox.Name = "CurrencyRateAccTextBox"
        Me.CurrencyRateAccTextBox.NegativeValue = False
        Me.CurrencyRateAccTextBox.Size = New System.Drawing.Size(154, 20)
        Me.CurrencyRateAccTextBox.TabIndex = 2
        Me.CurrencyRateAccTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DocumentNumberTextBox
        '
        Me.DocumentNumberTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AdvanceReportBindingSource, "DocumentNumber", True))
        Me.DocumentNumberTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DocumentNumberTextBox.Location = New System.Drawing.Point(378, 3)
        Me.DocumentNumberTextBox.MaxLength = 50
        Me.DocumentNumberTextBox.Name = "DocumentNumberTextBox"
        Me.DocumentNumberTextBox.Size = New System.Drawing.Size(163, 20)
        Me.DocumentNumberTextBox.TabIndex = 1
        Me.DocumentNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'IDTextBox
        '
        Me.IDTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AdvanceReportBindingSource, "ID", True))
        Me.IDTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IDTextBox.Location = New System.Drawing.Point(3, 3)
        Me.IDTextBox.Name = "IDTextBox"
        Me.IDTextBox.ReadOnly = True
        Me.IDTextBox.Size = New System.Drawing.Size(99, 20)
        Me.IDTextBox.TabIndex = 18
        Me.IDTextBox.TabStop = False
        Me.IDTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PersonAccGridComboBox
        '
        Me.PersonAccGridComboBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.AdvanceReportBindingSource, "Person", True))
        Me.PersonAccGridComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PersonAccGridComboBox.EmptyValueString = ""
        Me.PersonAccGridComboBox.Location = New System.Drawing.Point(3, 3)
        Me.PersonAccGridComboBox.Name = "PersonAccGridComboBox"
        Me.PersonAccGridComboBox.Size = New System.Drawing.Size(547, 20)
        Me.PersonAccGridComboBox.TabIndex = 0
        '
        'SumAccTextBox
        '
        Me.SumAccTextBox.ButtonVisible = False
        Me.SumAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.AdvanceReportBindingSource, "Sum", True))
        Me.SumAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SumAccTextBox.Location = New System.Drawing.Point(59, 36)
        Me.SumAccTextBox.Name = "SumAccTextBox"
        Me.SumAccTextBox.ReadOnly = True
        Me.SumAccTextBox.Size = New System.Drawing.Size(107, 20)
        Me.SumAccTextBox.TabIndex = 22
        Me.SumAccTextBox.TabStop = False
        Me.SumAccTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'SumTotalAccTextBox
        '
        Me.SumTotalAccTextBox.ButtonVisible = False
        Me.SumTotalAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.AdvanceReportBindingSource, "SumTotal", True))
        Me.SumTotalAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SumTotalAccTextBox.Location = New System.Drawing.Point(59, 90)
        Me.SumTotalAccTextBox.Name = "SumTotalAccTextBox"
        Me.SumTotalAccTextBox.ReadOnly = True
        Me.SumTotalAccTextBox.Size = New System.Drawing.Size(107, 20)
        Me.SumTotalAccTextBox.TabIndex = 26
        Me.SumTotalAccTextBox.TabStop = False
        Me.SumTotalAccTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'SumVatAccTextBox
        '
        Me.SumVatAccTextBox.ButtonVisible = False
        Me.SumVatAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.AdvanceReportBindingSource, "SumVat", True))
        Me.SumVatAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SumVatAccTextBox.Location = New System.Drawing.Point(59, 63)
        Me.SumVatAccTextBox.Name = "SumVatAccTextBox"
        Me.SumVatAccTextBox.ReadOnly = True
        Me.SumVatAccTextBox.Size = New System.Drawing.Size(107, 20)
        Me.SumVatAccTextBox.TabIndex = 30
        Me.SumVatAccTextBox.TabStop = False
        Me.SumVatAccTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17.0!))
        Me.TableLayoutPanel1.Controls.Add(SumLabel, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(SumVatLabel, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(SumTotalLabel, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.CurrencyCodeLabel2, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.SumAccTextBox, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.SumTotalAccTextBox, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.SumVatAccTextBox, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.GetCurrencyRatesButton, 1, 6)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(708, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.TableLayoutPanel1.RowCount = 7
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(186, 163)
        Me.TableLayoutPanel1.TabIndex = 33
        '
        'CurrencyCodeLabel2
        '
        Me.CurrencyCodeLabel2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AdvanceReportBindingSource, "CurrencyCode", True))
        Me.CurrencyCodeLabel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CurrencyCodeLabel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrencyCodeLabel2.Location = New System.Drawing.Point(59, 0)
        Me.CurrencyCodeLabel2.Name = "CurrencyCodeLabel2"
        Me.CurrencyCodeLabel2.Size = New System.Drawing.Size(107, 23)
        Me.CurrencyCodeLabel2.TabIndex = 35
        Me.CurrencyCodeLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GetCurrencyRatesButton
        '
        Me.GetCurrencyRatesButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GetCurrencyRatesButton.Location = New System.Drawing.Point(59, 137)
        Me.GetCurrencyRatesButton.Name = "GetCurrencyRatesButton"
        Me.GetCurrencyRatesButton.Size = New System.Drawing.Size(49, 23)
        Me.GetCurrencyRatesButton.TabIndex = 0
        Me.GetCurrencyRatesButton.Text = "$->€"
        Me.GetCurrencyRatesButton.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.AutoSize = True
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.63145!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.36855!))
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel3, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.AddAdvanceReportItemButton, 1, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(897, 217)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.AutoSize = True
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel11, 1, 5)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel5, 1, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel9, 1, 4)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel10, 1, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel8, 1, 1)
        Me.TableLayoutPanel3.Controls.Add(IDLabel, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(AccountLabel, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(PersonLabel, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(CommentsInternalLabel, 0, 5)
        Me.TableLayoutPanel3.Controls.Add(CommentsLabel, 0, 4)
        Me.TableLayoutPanel3.Controls.Add(ContentLabel, 0, 3)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel4, 1, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 0)
        Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 6
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(699, 180)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'TableLayoutPanel11
        '
        Me.TableLayoutPanel11.ColumnCount = 2
        Me.TableLayoutPanel11.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel11.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel11.Controls.Add(Me.CommentsInternalTextBox, 0, 0)
        Me.TableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel11.Location = New System.Drawing.Point(126, 150)
        Me.TableLayoutPanel11.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel11.Name = "TableLayoutPanel11"
        Me.TableLayoutPanel11.RowCount = 1
        Me.TableLayoutPanel11.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel11.Size = New System.Drawing.Size(573, 30)
        Me.TableLayoutPanel11.TabIndex = 5
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 8
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.AccountAccGridComboBox, 0, 0)
        Me.TableLayoutPanel5.Controls.Add(CurrencyCodeLabel, 2, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.CurrencyCodeComboBox, 3, 0)
        Me.TableLayoutPanel5.Controls.Add(CurrencyRateLabel, 5, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.CurrencyRateAccTextBox, 6, 0)
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(126, 60)
        Me.TableLayoutPanel5.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 1
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(573, 30)
        Me.TableLayoutPanel5.TabIndex = 2
        '
        'TableLayoutPanel9
        '
        Me.TableLayoutPanel9.ColumnCount = 2
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel9.Controls.Add(Me.CommentsTextBox, 0, 0)
        Me.TableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel9.Location = New System.Drawing.Point(126, 120)
        Me.TableLayoutPanel9.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel9.Name = "TableLayoutPanel9"
        Me.TableLayoutPanel9.RowCount = 1
        Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel9.Size = New System.Drawing.Size(573, 30)
        Me.TableLayoutPanel9.TabIndex = 4
        '
        'TableLayoutPanel10
        '
        Me.TableLayoutPanel10.ColumnCount = 2
        Me.TableLayoutPanel10.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel10.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel10.Controls.Add(Me.ContentTextBox, 0, 0)
        Me.TableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel10.Location = New System.Drawing.Point(126, 90)
        Me.TableLayoutPanel10.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel10.Name = "TableLayoutPanel10"
        Me.TableLayoutPanel10.RowCount = 1
        Me.TableLayoutPanel10.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel10.Size = New System.Drawing.Size(573, 30)
        Me.TableLayoutPanel10.TabIndex = 3
        '
        'TableLayoutPanel8
        '
        Me.TableLayoutPanel8.ColumnCount = 2
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel8.Controls.Add(Me.PersonAccGridComboBox, 0, 0)
        Me.TableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel8.Location = New System.Drawing.Point(126, 30)
        Me.TableLayoutPanel8.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
        Me.TableLayoutPanel8.RowCount = 1
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel8.Size = New System.Drawing.Size(573, 30)
        Me.TableLayoutPanel8.TabIndex = 1
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.AutoScroll = True
        Me.TableLayoutPanel4.ColumnCount = 8
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.ViewJournalEntryButton, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.IDTextBox, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(DateLabel, 2, 0)
        Me.TableLayoutPanel4.Controls.Add(DocumentNumberLabel, 5, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.DocumentNumberTextBox, 6, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.DateAccDatePicker, 3, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(126, 0)
        Me.TableLayoutPanel4.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(573, 30)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'ViewJournalEntryButton
        '
        Me.ViewJournalEntryButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ViewJournalEntryButton.Image = Global.AccDataBindingsWinForms.My.Resources.Resources.lektuvelis_16
        Me.ViewJournalEntryButton.Location = New System.Drawing.Point(105, 0)
        Me.ViewJournalEntryButton.Margin = New System.Windows.Forms.Padding(0)
        Me.ViewJournalEntryButton.Name = "ViewJournalEntryButton"
        Me.ViewJournalEntryButton.Size = New System.Drawing.Size(24, 24)
        Me.ViewJournalEntryButton.TabIndex = 89
        Me.ViewJournalEntryButton.UseVisualStyleBackColor = True
        '
        'DateAccDatePicker
        '
        Me.DateAccDatePicker.BoldedDates = Nothing
        Me.DateAccDatePicker.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.AdvanceReportBindingSource, "Date", True))
        Me.DateAccDatePicker.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DateAccDatePicker.Location = New System.Drawing.Point(176, 3)
        Me.DateAccDatePicker.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.DateAccDatePicker.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.DateAccDatePicker.Name = "DateAccDatePicker"
        Me.DateAccDatePicker.ShowWeekNumbers = True
        Me.DateAccDatePicker.Size = New System.Drawing.Size(142, 20)
        Me.DateAccDatePicker.TabIndex = 0
        '
        'AddAdvanceReportItemButton
        '
        Me.AddAdvanceReportItemButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddAdvanceReportItemButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AddAdvanceReportItemButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.AddAdvanceReportItemButton.Image = Global.AccDataBindingsWinForms.My.Resources.Resources.attach_icon_24x24
        Me.AddAdvanceReportItemButton.Location = New System.Drawing.Point(795, 183)
        Me.AddAdvanceReportItemButton.Name = "AddAdvanceReportItemButton"
        Me.AddAdvanceReportItemButton.Size = New System.Drawing.Size(99, 31)
        Me.AddAdvanceReportItemButton.TabIndex = 2
        Me.AddAdvanceReportItemButton.Text = "Faktūra"
        Me.AddAdvanceReportItemButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.AddAdvanceReportItemButton.UseVisualStyleBackColor = True
        '
        'ReportItemsSortedBindingSource
        '
        Me.ReportItemsSortedBindingSource.DataMember = "ReportItems"
        Me.ReportItemsSortedBindingSource.DataSource = Me.AdvanceReportBindingSource
        '
        'Panel2
        '
        Me.Panel2.AutoSize = True
        Me.Panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel2.Controls.Add(Me.ICancelButton)
        Me.Panel2.Controls.Add(Me.IOkButton)
        Me.Panel2.Controls.Add(Me.IApplyButton)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 504)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(897, 32)
        Me.Panel2.TabIndex = 2
        '
        'ICancelButton
        '
        Me.ICancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ICancelButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ICancelButton.Location = New System.Drawing.Point(796, 6)
        Me.ICancelButton.Name = "ICancelButton"
        Me.ICancelButton.Size = New System.Drawing.Size(89, 23)
        Me.ICancelButton.TabIndex = 14
        Me.ICancelButton.Text = "Atšaukti"
        Me.ICancelButton.UseVisualStyleBackColor = True
        '
        'IOkButton
        '
        Me.IOkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.IOkButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IOkButton.Location = New System.Drawing.Point(590, 6)
        Me.IOkButton.Name = "IOkButton"
        Me.IOkButton.Size = New System.Drawing.Size(89, 23)
        Me.IOkButton.TabIndex = 13
        Me.IOkButton.Text = "Ok"
        Me.IOkButton.UseVisualStyleBackColor = True
        '
        'IApplyButton
        '
        Me.IApplyButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.IApplyButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IApplyButton.Location = New System.Drawing.Point(694, 6)
        Me.IApplyButton.Name = "IApplyButton"
        Me.IApplyButton.Size = New System.Drawing.Size(89, 23)
        Me.IApplyButton.TabIndex = 12
        Me.IApplyButton.Text = "Išsaugoti"
        Me.IApplyButton.UseVisualStyleBackColor = True
        '
        'ReportItemsDataListView
        '
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn2)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn1)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn3)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn4)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn5)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn6)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn7)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn8)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn9)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn10)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn34)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn11)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn12)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn13)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn14)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn15)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn16)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn17)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn18)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn19)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn20)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn21)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn22)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn23)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn24)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn25)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn26)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn27)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn28)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn29)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn30)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn31)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn32)
        Me.ReportItemsDataListView.AllColumns.Add(Me.OlvColumn33)
        Me.ReportItemsDataListView.AllowColumnReorder = True
        Me.ReportItemsDataListView.AutoGenerateColumns = False
        Me.ReportItemsDataListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways
        Me.ReportItemsDataListView.CellEditEnterChangesRows = True
        Me.ReportItemsDataListView.CellEditTabChangesRows = True
        Me.ReportItemsDataListView.CellEditUseWholeCell = False
        Me.ReportItemsDataListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.OlvColumn2, Me.OlvColumn3, Me.OlvColumn4, Me.OlvColumn5, Me.OlvColumn6, Me.OlvColumn7, Me.OlvColumn8, Me.OlvColumn9, Me.OlvColumn10, Me.OlvColumn34, Me.OlvColumn11, Me.OlvColumn12, Me.OlvColumn14, Me.OlvColumn23, Me.OlvColumn25, Me.OlvColumn33})
        Me.ReportItemsDataListView.Cursor = System.Windows.Forms.Cursors.Default
        Me.ReportItemsDataListView.DataSource = Me.ReportItemsSortedBindingSource
        Me.ReportItemsDataListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ReportItemsDataListView.FullRowSelect = True
        Me.ReportItemsDataListView.HasCollapsibleGroups = False
        Me.ReportItemsDataListView.HeaderWordWrap = True
        Me.ReportItemsDataListView.HideSelection = False
        Me.ReportItemsDataListView.IncludeColumnHeadersInCopy = True
        Me.ReportItemsDataListView.Location = New System.Drawing.Point(0, 217)
        Me.ReportItemsDataListView.Name = "ReportItemsDataListView"
        Me.ReportItemsDataListView.RenderNonEditableCheckboxesAsDisabled = True
        Me.ReportItemsDataListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu
        Me.ReportItemsDataListView.SelectedBackColor = System.Drawing.Color.PaleGreen
        Me.ReportItemsDataListView.SelectedForeColor = System.Drawing.Color.Black
        Me.ReportItemsDataListView.ShowCommandMenuOnRightClick = True
        Me.ReportItemsDataListView.ShowGroups = False
        Me.ReportItemsDataListView.ShowImagesOnSubItems = True
        Me.ReportItemsDataListView.ShowItemCountOnGroups = True
        Me.ReportItemsDataListView.ShowItemToolTips = True
        Me.ReportItemsDataListView.Size = New System.Drawing.Size(897, 287)
        Me.ReportItemsDataListView.TabIndex = 3
        Me.ReportItemsDataListView.UnfocusedSelectedBackColor = System.Drawing.Color.PaleGreen
        Me.ReportItemsDataListView.UnfocusedSelectedForeColor = System.Drawing.Color.Black
        Me.ReportItemsDataListView.UseCellFormatEvents = True
        Me.ReportItemsDataListView.UseCompatibleStateImageBehavior = False
        Me.ReportItemsDataListView.UseFilterIndicator = True
        Me.ReportItemsDataListView.UseFiltering = True
        Me.ReportItemsDataListView.UseHotItem = True
        Me.ReportItemsDataListView.UseNotifyPropertyChanged = True
        Me.ReportItemsDataListView.View = System.Windows.Forms.View.Details
        '
        'OlvColumn2
        '
        Me.OlvColumn2.AspectName = "Date"
        Me.OlvColumn2.AspectToStringFormat = "{0:d}"
        Me.OlvColumn2.CellEditUseWholeCell = True
        Me.OlvColumn2.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn2.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn2.Text = "Data"
        Me.OlvColumn2.ToolTipText = ""
        Me.OlvColumn2.Width = 85
        '
        'OlvColumn1
        '
        Me.OlvColumn1.AspectName = "ID"
        Me.OlvColumn1.CellEditUseWholeCell = True
        Me.OlvColumn1.DisplayIndex = 1
        Me.OlvColumn1.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn1.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn1.IsEditable = False
        Me.OlvColumn1.IsVisible = False
        Me.OlvColumn1.Text = "ID"
        Me.OlvColumn1.ToolTipText = ""
        Me.OlvColumn1.Width = 100
        '
        'OlvColumn3
        '
        Me.OlvColumn3.AspectName = "DocumentNumber"
        Me.OlvColumn3.CellEditUseWholeCell = True
        Me.OlvColumn3.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn3.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn3.Text = "Dok. Nr."
        Me.OlvColumn3.ToolTipText = ""
        Me.OlvColumn3.Width = 68
        '
        'OlvColumn4
        '
        Me.OlvColumn4.AspectName = "Person"
        Me.OlvColumn4.CellEditUseWholeCell = True
        Me.OlvColumn4.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn4.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn4.Text = "Kontrahentas"
        Me.OlvColumn4.ToolTipText = ""
        Me.OlvColumn4.Width = 187
        '
        'OlvColumn5
        '
        Me.OlvColumn5.AspectName = "Content"
        Me.OlvColumn5.CellEditUseWholeCell = True
        Me.OlvColumn5.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn5.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn5.Text = "Turinys"
        Me.OlvColumn5.ToolTipText = ""
        Me.OlvColumn5.Width = 178
        '
        'OlvColumn6
        '
        Me.OlvColumn6.AspectName = "Income"
        Me.OlvColumn6.CellEditUseWholeCell = True
        Me.OlvColumn6.CheckBoxes = True
        Me.OlvColumn6.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn6.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn6.IsHeaderVertical = True
        Me.OlvColumn6.Text = "Pajamos"
        Me.OlvColumn6.ToolTipText = ""
        Me.OlvColumn6.Width = 82
        '
        'OlvColumn7
        '
        Me.OlvColumn7.AspectName = "Expenses"
        Me.OlvColumn7.CellEditUseWholeCell = True
        Me.OlvColumn7.CheckBoxes = True
        Me.OlvColumn7.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn7.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn7.IsHeaderVertical = True
        Me.OlvColumn7.Text = "Išlaidos"
        Me.OlvColumn7.ToolTipText = ""
        Me.OlvColumn7.Width = 86
        '
        'OlvColumn8
        '
        Me.OlvColumn8.AspectName = "Account"
        Me.OlvColumn8.CellEditUseWholeCell = True
        Me.OlvColumn8.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn8.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn8.Text = "Koresp. sąsk."
        Me.OlvColumn8.ToolTipText = ""
        Me.OlvColumn8.Width = 100
        '
        'OlvColumn9
        '
        Me.OlvColumn9.AspectName = "AccountVat"
        Me.OlvColumn9.CellEditUseWholeCell = True
        Me.OlvColumn9.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn9.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn9.Text = "Koresp. PVM sąsk."
        Me.OlvColumn9.ToolTipText = ""
        Me.OlvColumn9.Width = 100
        '
        'OlvColumn10
        '
        Me.OlvColumn10.AspectName = "Sum"
        Me.OlvColumn10.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn10.CellEditUseWholeCell = True
        Me.OlvColumn10.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn10.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn10.Text = "Suma"
        Me.OlvColumn10.ToolTipText = ""
        Me.OlvColumn10.Width = 100
        '
        'OlvColumn34
        '
        Me.OlvColumn34.AspectName = "DeclarationSchema"
        Me.OlvColumn34.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.OlvColumn34.Text = "PVM Deklaravimo Schema"
        '
        'OlvColumn11
        '
        Me.OlvColumn11.AspectName = "VatRate"
        Me.OlvColumn11.CellEditUseWholeCell = True
        Me.OlvColumn11.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn11.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn11.Text = "PVM tarifas"
        Me.OlvColumn11.ToolTipText = ""
        Me.OlvColumn11.Width = 100
        '
        'OlvColumn12
        '
        Me.OlvColumn12.AspectName = "SumVat"
        Me.OlvColumn12.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn12.CellEditUseWholeCell = True
        Me.OlvColumn12.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn12.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn12.IsEditable = False
        Me.OlvColumn12.Text = "Suma PVM"
        Me.OlvColumn12.ToolTipText = ""
        Me.OlvColumn12.Width = 100
        '
        'OlvColumn13
        '
        Me.OlvColumn13.AspectName = "SumVatCorrection"
        Me.OlvColumn13.CellEditUseWholeCell = True
        Me.OlvColumn13.DisplayIndex = 12
        Me.OlvColumn13.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn13.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn13.IsVisible = False
        Me.OlvColumn13.Text = "Sumos PVM korekc."
        Me.OlvColumn13.ToolTipText = ""
        Me.OlvColumn13.Width = 100
        '
        'OlvColumn14
        '
        Me.OlvColumn14.AspectName = "SumTotal"
        Me.OlvColumn14.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn14.CellEditUseWholeCell = True
        Me.OlvColumn14.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn14.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn14.IsEditable = False
        Me.OlvColumn14.Text = "Suma viso"
        Me.OlvColumn14.ToolTipText = ""
        Me.OlvColumn14.Width = 100
        '
        'OlvColumn15
        '
        Me.OlvColumn15.AspectName = "SumLTL"
        Me.OlvColumn15.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn15.CellEditUseWholeCell = True
        Me.OlvColumn15.DisplayIndex = 14
        Me.OlvColumn15.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn15.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn15.IsEditable = False
        Me.OlvColumn15.IsVisible = False
        Me.OlvColumn15.Text = "Suma LTL"
        Me.OlvColumn15.ToolTipText = ""
        Me.OlvColumn15.Width = 100
        '
        'OlvColumn16
        '
        Me.OlvColumn16.AspectName = "SumCorrectionLTL"
        Me.OlvColumn16.CellEditUseWholeCell = True
        Me.OlvColumn16.DisplayIndex = 15
        Me.OlvColumn16.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn16.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn16.IsVisible = False
        Me.OlvColumn16.Text = "Sumos LTL korekc."
        Me.OlvColumn16.ToolTipText = ""
        Me.OlvColumn16.Width = 100
        '
        'OlvColumn17
        '
        Me.OlvColumn17.AspectName = "SumVatLTL"
        Me.OlvColumn17.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn17.CellEditUseWholeCell = True
        Me.OlvColumn17.DisplayIndex = 16
        Me.OlvColumn17.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn17.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn17.IsEditable = False
        Me.OlvColumn17.IsVisible = False
        Me.OlvColumn17.Text = "Suma PVM LTL"
        Me.OlvColumn17.ToolTipText = ""
        Me.OlvColumn17.Width = 100
        '
        'OlvColumn18
        '
        Me.OlvColumn18.AspectName = "SumVatCorrectionLTL"
        Me.OlvColumn18.CellEditUseWholeCell = True
        Me.OlvColumn18.DisplayIndex = 17
        Me.OlvColumn18.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn18.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn18.IsVisible = False
        Me.OlvColumn18.Text = "Sumos PVM LTL korekc."
        Me.OlvColumn18.ToolTipText = ""
        Me.OlvColumn18.Width = 100
        '
        'OlvColumn19
        '
        Me.OlvColumn19.AspectName = "SumTotalLTL"
        Me.OlvColumn19.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn19.CellEditUseWholeCell = True
        Me.OlvColumn19.DisplayIndex = 18
        Me.OlvColumn19.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn19.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn19.IsEditable = False
        Me.OlvColumn19.IsVisible = False
        Me.OlvColumn19.Text = "Suma viso LTL"
        Me.OlvColumn19.ToolTipText = ""
        Me.OlvColumn19.Width = 100
        '
        'OlvColumn20
        '
        Me.OlvColumn20.AspectName = "CurrencyRateChangeEffect"
        Me.OlvColumn20.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn20.CellEditUseWholeCell = True
        Me.OlvColumn20.DisplayIndex = 19
        Me.OlvColumn20.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn20.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn20.IsVisible = False
        Me.OlvColumn20.Text = "Kurso pasik. įtaka"
        Me.OlvColumn20.ToolTipText = ""
        Me.OlvColumn20.Width = 100
        '
        'OlvColumn21
        '
        Me.OlvColumn21.AspectName = "AccountCurrencyRateChangeEffect"
        Me.OlvColumn21.CellEditUseWholeCell = True
        Me.OlvColumn21.DisplayIndex = 20
        Me.OlvColumn21.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn21.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn21.IsVisible = False
        Me.OlvColumn21.Text = "Kurso pasik. įtakos sąsk."
        Me.OlvColumn21.ToolTipText = ""
        Me.OlvColumn21.Width = 100
        '
        'OlvColumn22
        '
        Me.OlvColumn22.AspectName = "InvoiceID"
        Me.OlvColumn22.CellEditUseWholeCell = True
        Me.OlvColumn22.DisplayIndex = 21
        Me.OlvColumn22.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn22.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn22.IsEditable = False
        Me.OlvColumn22.IsVisible = False
        Me.OlvColumn22.Text = "Sąskaitos ID"
        Me.OlvColumn22.ToolTipText = ""
        Me.OlvColumn22.Width = 100
        '
        'OlvColumn23
        '
        Me.OlvColumn23.AspectName = "InvoiceDateAndNumber"
        Me.OlvColumn23.CellEditUseWholeCell = True
        Me.OlvColumn23.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn23.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn23.IsEditable = False
        Me.OlvColumn23.Text = "Sąskaitos data ir Nr."
        Me.OlvColumn23.ToolTipText = ""
        Me.OlvColumn23.Width = 100
        '
        'OlvColumn24
        '
        Me.OlvColumn24.AspectName = "InvoiceContent"
        Me.OlvColumn24.CellEditUseWholeCell = True
        Me.OlvColumn24.DisplayIndex = 23
        Me.OlvColumn24.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn24.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn24.IsEditable = False
        Me.OlvColumn24.IsVisible = False
        Me.OlvColumn24.Text = "Sąskaitos turinys"
        Me.OlvColumn24.ToolTipText = ""
        Me.OlvColumn24.Width = 100
        '
        'OlvColumn25
        '
        Me.OlvColumn25.AspectName = "InvoiceCurrencyCode"
        Me.OlvColumn25.CellEditUseWholeCell = True
        Me.OlvColumn25.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn25.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn25.IsEditable = False
        Me.OlvColumn25.Text = "Sąskaitos valiuta"
        Me.OlvColumn25.ToolTipText = ""
        Me.OlvColumn25.Width = 100
        '
        'OlvColumn26
        '
        Me.OlvColumn26.AspectName = "InvoiceCurrencyRate"
        Me.OlvColumn26.AspectToStringFormat = "{0:##,0.000000}"
        Me.OlvColumn26.CellEditUseWholeCell = True
        Me.OlvColumn26.DisplayIndex = 25
        Me.OlvColumn26.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn26.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn26.IsEditable = False
        Me.OlvColumn26.IsVisible = False
        Me.OlvColumn26.Text = "Sąskaitos kursas"
        Me.OlvColumn26.ToolTipText = ""
        Me.OlvColumn26.Width = 100
        '
        'OlvColumn27
        '
        Me.OlvColumn27.AspectName = "InvoiceSumOriginal"
        Me.OlvColumn27.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn27.CellEditUseWholeCell = True
        Me.OlvColumn27.DisplayIndex = 26
        Me.OlvColumn27.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn27.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn27.IsEditable = False
        Me.OlvColumn27.IsVisible = False
        Me.OlvColumn27.Text = "Sąskaitos suma"
        Me.OlvColumn27.ToolTipText = ""
        Me.OlvColumn27.Width = 100
        '
        'OlvColumn28
        '
        Me.OlvColumn28.AspectName = "InvoiceSumVatOriginal"
        Me.OlvColumn28.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn28.CellEditUseWholeCell = True
        Me.OlvColumn28.DisplayIndex = 27
        Me.OlvColumn28.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn28.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn28.IsEditable = False
        Me.OlvColumn28.IsVisible = False
        Me.OlvColumn28.Text = "Sąskaitos PVM suma"
        Me.OlvColumn28.ToolTipText = ""
        Me.OlvColumn28.Width = 100
        '
        'OlvColumn29
        '
        Me.OlvColumn29.AspectName = "InvoiceSumTotalOriginal"
        Me.OlvColumn29.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn29.CellEditUseWholeCell = True
        Me.OlvColumn29.DisplayIndex = 28
        Me.OlvColumn29.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn29.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn29.IsEditable = False
        Me.OlvColumn29.IsVisible = False
        Me.OlvColumn29.Text = "Sąskaitos suma viso"
        Me.OlvColumn29.ToolTipText = ""
        Me.OlvColumn29.Width = 100
        '
        'OlvColumn30
        '
        Me.OlvColumn30.AspectName = "InvoiceSumLTL"
        Me.OlvColumn30.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn30.CellEditUseWholeCell = True
        Me.OlvColumn30.DisplayIndex = 29
        Me.OlvColumn30.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn30.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn30.IsEditable = False
        Me.OlvColumn30.IsVisible = False
        Me.OlvColumn30.Text = "Sąskaitos suma LTL"
        Me.OlvColumn30.ToolTipText = ""
        Me.OlvColumn30.Width = 100
        '
        'OlvColumn31
        '
        Me.OlvColumn31.AspectName = "InvoiceSumVatLTL"
        Me.OlvColumn31.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn31.CellEditUseWholeCell = True
        Me.OlvColumn31.DisplayIndex = 30
        Me.OlvColumn31.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn31.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn31.IsEditable = False
        Me.OlvColumn31.IsVisible = False
        Me.OlvColumn31.Text = "Sąskaitos PVM suma LTL"
        Me.OlvColumn31.ToolTipText = ""
        Me.OlvColumn31.Width = 100
        '
        'OlvColumn32
        '
        Me.OlvColumn32.AspectName = "InvoiceSumTotalLTL"
        Me.OlvColumn32.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn32.CellEditUseWholeCell = True
        Me.OlvColumn32.DisplayIndex = 31
        Me.OlvColumn32.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn32.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn32.IsEditable = False
        Me.OlvColumn32.IsVisible = False
        Me.OlvColumn32.Text = "Sąskaitos suma viso LTL"
        Me.OlvColumn32.ToolTipText = ""
        Me.OlvColumn32.Width = 100
        '
        'OlvColumn33
        '
        Me.OlvColumn33.AspectName = "InvoiceSumTotal"
        Me.OlvColumn33.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn33.CellEditUseWholeCell = True
        Me.OlvColumn33.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn33.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn33.IsEditable = False
        Me.OlvColumn33.Text = "Sąskaitos suma apyskaitoje"
        Me.OlvColumn33.ToolTipText = ""
        Me.OlvColumn33.Width = 100
        '
        'ProgressFiller1
        '
        Me.ProgressFiller1.Location = New System.Drawing.Point(181, 326)
        Me.ProgressFiller1.Name = "ProgressFiller1"
        Me.ProgressFiller1.Size = New System.Drawing.Size(237, 62)
        Me.ProgressFiller1.TabIndex = 4
        Me.ProgressFiller1.Visible = False
        '
        'ProgressFiller2
        '
        Me.ProgressFiller2.Location = New System.Drawing.Point(438, 327)
        Me.ProgressFiller2.Name = "ProgressFiller2"
        Me.ProgressFiller2.Size = New System.Drawing.Size(264, 81)
        Me.ProgressFiller2.TabIndex = 5
        Me.ProgressFiller2.Visible = False
        '
        'ErrorWarnInfoProvider1
        '
        Me.ErrorWarnInfoProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorWarnInfoProvider1.BlinkStyleInformation = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorWarnInfoProvider1.BlinkStyleWarning = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorWarnInfoProvider1.ContainerControl = Me
        Me.ErrorWarnInfoProvider1.DataSource = Me.AdvanceReportBindingSource
        '
        'F_AdvanceReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(897, 536)
        Me.Controls.Add(Me.ReportItemsDataListView)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.ProgressFiller2)
        Me.Controls.Add(Me.ProgressFiller1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "F_AdvanceReport"
        Me.ShowInTaskbar = False
        Me.Text = "Avanso apyskaita"
        CType(Me.AdvanceReportBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel11.ResumeLayout(False)
        Me.TableLayoutPanel11.PerformLayout()
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TableLayoutPanel5.PerformLayout()
        Me.TableLayoutPanel9.ResumeLayout(False)
        Me.TableLayoutPanel9.PerformLayout()
        Me.TableLayoutPanel10.ResumeLayout(False)
        Me.TableLayoutPanel10.PerformLayout()
        Me.TableLayoutPanel8.ResumeLayout(False)
        Me.TableLayoutPanel8.PerformLayout()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        CType(Me.ReportItemsSortedBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        CType(Me.ReportItemsDataListView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorWarnInfoProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AdvanceReportBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents AccountAccGridComboBox As AccControlsWinForms.AccListComboBox
    Friend WithEvents CommentsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CommentsInternalTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ContentTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CurrencyCodeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents CurrencyRateAccTextBox As AccControlsWinForms.AccTextBox
    Friend WithEvents DocumentNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents IDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PersonAccGridComboBox As AccControlsWinForms.AccListComboBox
    Friend WithEvents SumAccTextBox As AccControlsWinForms.AccTextBox
    Friend WithEvents SumTotalAccTextBox As AccControlsWinForms.AccTextBox
    Friend WithEvents SumVatAccTextBox As AccControlsWinForms.AccTextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents CurrencyCodeLabel2 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents AddAdvanceReportItemButton As System.Windows.Forms.Button
    Friend WithEvents ReportItemsSortedBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ICancelButton As System.Windows.Forms.Button
    Friend WithEvents IOkButton As System.Windows.Forms.Button
    Friend WithEvents IApplyButton As System.Windows.Forms.Button
    Friend WithEvents GetCurrencyRatesButton As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel8 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel11 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel9 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel10 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ViewJournalEntryButton As System.Windows.Forms.Button
    Friend WithEvents ReportItemsDataListView As BrightIdeasSoftware.DataListView
    Friend WithEvents OlvColumn1 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn2 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn3 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn4 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn5 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn6 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn7 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn8 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn9 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn10 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn11 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn12 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn13 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn14 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn15 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn16 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn17 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn18 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn19 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn20 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn21 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn22 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn23 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn24 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn25 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn26 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn27 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn28 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn29 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn30 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn31 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn32 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn33 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents ProgressFiller2 As AccControlsWinForms.ProgressFiller
    Friend WithEvents ProgressFiller1 As AccControlsWinForms.ProgressFiller
    Friend WithEvents ErrorWarnInfoProvider1 As AccControlsWinForms.ErrorWarnInfoProvider
    Friend WithEvents OlvColumn34 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents DateAccDatePicker As AccControlsWinForms.AccDatePicker
End Class
