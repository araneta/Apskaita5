<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Friend Class F_AccumulativeCosts
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
        Dim IDLabel As System.Windows.Forms.Label
        Dim UpdateDateLabel As System.Windows.Forms.Label
        Dim InsertDateLabel As System.Windows.Forms.Label
        Dim DateLabel As System.Windows.Forms.Label
        Dim DocumentNumberLabel As System.Windows.Forms.Label
        Dim AccountCostsLabel As System.Windows.Forms.Label
        Dim AccountAccumulatedCostsLabel As System.Windows.Forms.Label
        Dim AccountDistributedCostsLabel As System.Windows.Forms.Label
        Dim ContentLabel As System.Windows.Forms.Label
        Dim CommentsLabel As System.Windows.Forms.Label
        Dim SumLabel As System.Windows.Forms.Label
        Dim SumDistributedLabel As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(F_AccumulativeCosts))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.NewDistributionButton = New System.Windows.Forms.Button
        Me.PeriodCountNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.Label3 = New System.Windows.Forms.Label
        Me.PeriodLengthNumericUpDown = New System.Windows.Forms.NumericUpDown
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.FirstPeriodAccDatePicker = New AccControlsWinForms.AccDatePicker
        Me.TableLayoutPanel8 = New System.Windows.Forms.TableLayoutPanel
        Me.DistributeButton = New System.Windows.Forms.Button
        Me.SumDistributedAccTextBox = New AccControlsWinForms.AccTextBox
        Me.AccumulativeCostsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.SumAccTextBox = New AccControlsWinForms.AccTextBox
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel
        Me.AccountDistributedCostsAccGridComboBox = New AccControlsWinForms.AccListComboBox
        Me.AccountCostsAccGridComboBox = New AccControlsWinForms.AccListComboBox
        Me.AccountAccumulatedCostsAccGridComboBox = New AccControlsWinForms.AccListComboBox
        Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel
        Me.ContentTextBox = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel
        Me.IsAccumulatedIncomeCheckBox = New System.Windows.Forms.CheckBox
        Me.DocumentNumberTextBox = New System.Windows.Forms.TextBox
        Me.DateAccDatePicker = New AccControlsWinForms.AccDatePicker
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel
        Me.UpdateDateTextBox = New System.Windows.Forms.TextBox
        Me.InsertDateTextBox = New System.Windows.Forms.TextBox
        Me.IDTextBox = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel
        Me.CommentsTextBox = New System.Windows.Forms.TextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.EditedBaner = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.nCancelButton = New System.Windows.Forms.Button
        Me.ApplyButton = New System.Windows.Forms.Button
        Me.nOkButton = New System.Windows.Forms.Button
        Me.ItemsSortedBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ItemsDataListView = New BrightIdeasSoftware.DataListView
        Me.OlvColumn2 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn1 = New BrightIdeasSoftware.OLVColumn
        Me.OlvColumn3 = New BrightIdeasSoftware.OLVColumn
        Me.ProgressFiller1 = New AccControlsWinForms.ProgressFiller
        Me.ErrorWarnInfoProvider1 = New AccControlsWinForms.ErrorWarnInfoProvider(Me.components)
        IDLabel = New System.Windows.Forms.Label
        UpdateDateLabel = New System.Windows.Forms.Label
        InsertDateLabel = New System.Windows.Forms.Label
        DateLabel = New System.Windows.Forms.Label
        DocumentNumberLabel = New System.Windows.Forms.Label
        AccountCostsLabel = New System.Windows.Forms.Label
        AccountAccumulatedCostsLabel = New System.Windows.Forms.Label
        AccountDistributedCostsLabel = New System.Windows.Forms.Label
        ContentLabel = New System.Windows.Forms.Label
        CommentsLabel = New System.Windows.Forms.Label
        SumLabel = New System.Windows.Forms.Label
        SumDistributedLabel = New System.Windows.Forms.Label
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.PeriodCountNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PeriodLengthNumericUpDown, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel8.SuspendLayout()
        CType(Me.AccumulativeCostsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.TableLayoutPanel6.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel7.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.EditedBaner.SuspendLayout()
        CType(Me.ItemsSortedBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ItemsDataListView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorWarnInfoProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'IDLabel
        '
        IDLabel.AutoSize = True
        IDLabel.Dock = System.Windows.Forms.DockStyle.Fill
        IDLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        IDLabel.Location = New System.Drawing.Point(3, 6)
        IDLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        IDLabel.Name = "IDLabel"
        IDLabel.Size = New System.Drawing.Size(142, 24)
        IDLabel.TabIndex = 6
        IDLabel.Text = "ID:"
        IDLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'UpdateDateLabel
        '
        UpdateDateLabel.AutoSize = True
        UpdateDateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        UpdateDateLabel.Location = New System.Drawing.Point(442, 6)
        UpdateDateLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        UpdateDateLabel.Name = "UpdateDateLabel"
        UpdateDateLabel.Size = New System.Drawing.Size(60, 13)
        UpdateDateLabel.TabIndex = 7
        UpdateDateLabel.Text = "Pakeista:"
        '
        'InsertDateLabel
        '
        InsertDateLabel.AutoSize = True
        InsertDateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        InsertDateLabel.Location = New System.Drawing.Point(179, 6)
        InsertDateLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        InsertDateLabel.Name = "InsertDateLabel"
        InsertDateLabel.Size = New System.Drawing.Size(55, 13)
        InsertDateLabel.TabIndex = 9
        InsertDateLabel.Text = "Įtraukta:"
        '
        'DateLabel
        '
        DateLabel.AutoSize = True
        DateLabel.Dock = System.Windows.Forms.DockStyle.Fill
        DateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DateLabel.Location = New System.Drawing.Point(3, 36)
        DateLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        DateLabel.Name = "DateLabel"
        DateLabel.Size = New System.Drawing.Size(142, 24)
        DateLabel.TabIndex = 5
        DateLabel.Text = "Data:"
        DateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'DocumentNumberLabel
        '
        DocumentNumberLabel.AutoSize = True
        DocumentNumberLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DocumentNumberLabel.Location = New System.Drawing.Point(201, 6)
        DocumentNumberLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        DocumentNumberLabel.Name = "DocumentNumberLabel"
        DocumentNumberLabel.Size = New System.Drawing.Size(59, 13)
        DocumentNumberLabel.TabIndex = 7
        DocumentNumberLabel.Text = "Dok. Nr.:"
        '
        'AccountCostsLabel
        '
        AccountCostsLabel.AutoSize = True
        AccountCostsLabel.Dock = System.Windows.Forms.DockStyle.Fill
        AccountCostsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        AccountCostsLabel.Location = New System.Drawing.Point(3, 66)
        AccountCostsLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        AccountCostsLabel.Name = "AccountCostsLabel"
        AccountCostsLabel.Size = New System.Drawing.Size(142, 24)
        AccountCostsLabel.TabIndex = 5
        AccountCostsLabel.Text = "Sąnaudų sąsk.:"
        AccountCostsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'AccountAccumulatedCostsLabel
        '
        AccountAccumulatedCostsLabel.AutoSize = True
        AccountAccumulatedCostsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        AccountAccumulatedCostsLabel.Location = New System.Drawing.Point(150, 6)
        AccountAccumulatedCostsLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        AccountAccumulatedCostsLabel.Name = "AccountAccumulatedCostsLabel"
        AccountAccumulatedCostsLabel.Size = New System.Drawing.Size(129, 13)
        AccountAccumulatedCostsLabel.TabIndex = 7
        AccountAccumulatedCostsLabel.Text = "Sukauptų Sąn. sąsk.:"
        '
        'AccountDistributedCostsLabel
        '
        AccountDistributedCostsLabel.AutoSize = True
        AccountDistributedCostsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        AccountDistributedCostsLabel.Location = New System.Drawing.Point(432, 6)
        AccountDistributedCostsLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        AccountDistributedCostsLabel.Name = "AccountDistributedCostsLabel"
        AccountDistributedCostsLabel.Size = New System.Drawing.Size(130, 13)
        AccountDistributedCostsLabel.TabIndex = 9
        AccountDistributedCostsLabel.Text = "Skirstomų Sąn. sąsk.:"
        '
        'ContentLabel
        '
        ContentLabel.AutoSize = True
        ContentLabel.Dock = System.Windows.Forms.DockStyle.Fill
        ContentLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ContentLabel.Location = New System.Drawing.Point(3, 96)
        ContentLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        ContentLabel.Name = "ContentLabel"
        ContentLabel.Size = New System.Drawing.Size(142, 24)
        ContentLabel.TabIndex = 5
        ContentLabel.Text = "Turinys:"
        ContentLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'CommentsLabel
        '
        CommentsLabel.AutoSize = True
        CommentsLabel.Dock = System.Windows.Forms.DockStyle.Fill
        CommentsLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CommentsLabel.Location = New System.Drawing.Point(3, 156)
        CommentsLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        CommentsLabel.Name = "CommentsLabel"
        CommentsLabel.Size = New System.Drawing.Size(142, 54)
        CommentsLabel.TabIndex = 7
        CommentsLabel.Text = "Komentarai:"
        CommentsLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'SumLabel
        '
        SumLabel.AutoSize = True
        SumLabel.Dock = System.Windows.Forms.DockStyle.Fill
        SumLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        SumLabel.Location = New System.Drawing.Point(3, 126)
        SumLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        SumLabel.Name = "SumLabel"
        SumLabel.Size = New System.Drawing.Size(142, 24)
        SumLabel.TabIndex = 4
        SumLabel.Text = "Suma:"
        SumLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'SumDistributedLabel
        '
        SumDistributedLabel.AutoSize = True
        SumDistributedLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        SumDistributedLabel.Location = New System.Drawing.Point(227, 6)
        SumDistributedLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        SumDistributedLabel.Name = "SumDistributedLabel"
        SumDistributedLabel.Size = New System.Drawing.Size(108, 13)
        SumDistributedLabel.TabIndex = 5
        SumDistributedLabel.Text = "Paskirstyta Suma:"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel8, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel5, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel6, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(SumLabel, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(IDLabel, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(ContentLabel, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel4, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(DateLabel, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(AccountCostsLabel, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(CommentsLabel, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel7, 1, 5)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 7
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(861, 264)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Info
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanel2)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(148, 210)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(713, 54)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Naujas Paskirstymas"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 10
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.Controls.Add(Me.NewDistributionButton, 9, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.PeriodCountNumericUpDown, 7, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label3, 6, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.PeriodLengthNumericUpDown, 4, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 3, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.FirstPeriodAccDatePicker, 1, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 16)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(707, 35)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'NewDistributionButton
        '
        Me.NewDistributionButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NewDistributionButton.Location = New System.Drawing.Point(620, 3)
        Me.NewDistributionButton.Name = "NewDistributionButton"
        Me.NewDistributionButton.Size = New System.Drawing.Size(83, 23)
        Me.NewDistributionButton.TabIndex = 3
        Me.NewDistributionButton.Text = "Sukurti"
        Me.NewDistributionButton.UseVisualStyleBackColor = True
        '
        'PeriodCountNumericUpDown
        '
        Me.PeriodCountNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PeriodCountNumericUpDown.Location = New System.Drawing.Point(507, 3)
        Me.PeriodCountNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PeriodCountNumericUpDown.Name = "PeriodCountNumericUpDown"
        Me.PeriodCountNumericUpDown.Size = New System.Drawing.Size(87, 20)
        Me.PeriodCountNumericUpDown.TabIndex = 2
        Me.PeriodCountNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.PeriodCountNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(410, 3)
        Me.Label3.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Laikotarpių sk."
        '
        'PeriodLengthNumericUpDown
        '
        Me.PeriodLengthNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PeriodLengthNumericUpDown.Location = New System.Drawing.Point(297, 3)
        Me.PeriodLengthNumericUpDown.Maximum = New Decimal(New Integer() {12, 0, 0, 0})
        Me.PeriodLengthNumericUpDown.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PeriodLengthNumericUpDown.Name = "PeriodLengthNumericUpDown"
        Me.PeriodLengthNumericUpDown.Size = New System.Drawing.Size(87, 20)
        Me.PeriodLengthNumericUpDown.TabIndex = 1
        Me.PeriodLengthNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.PeriodLengthNumericUpDown.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(187, 3)
        Me.Label2.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(104, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Laikotarpis mėn.:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 3)
        Me.Label1.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Nuo:"
        '
        'FirstPeriodAccDatePicker
        '
        Me.FirstPeriodAccDatePicker.BoldedDates = Nothing
        Me.FirstPeriodAccDatePicker.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FirstPeriodAccDatePicker.Location = New System.Drawing.Point(43, 3)
        Me.FirstPeriodAccDatePicker.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.FirstPeriodAccDatePicker.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.FirstPeriodAccDatePicker.Name = "FirstPeriodAccDatePicker"
        Me.FirstPeriodAccDatePicker.ReadOnly = False
        Me.FirstPeriodAccDatePicker.ShowWeekNumbers = True
        Me.FirstPeriodAccDatePicker.Size = New System.Drawing.Size(118, 29)
        Me.FirstPeriodAccDatePicker.TabIndex = 0
        '
        'TableLayoutPanel8
        '
        Me.TableLayoutPanel8.ColumnCount = 7
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel8.Controls.Add(Me.DistributeButton, 5, 0)
        Me.TableLayoutPanel8.Controls.Add(Me.SumDistributedAccTextBox, 3, 0)
        Me.TableLayoutPanel8.Controls.Add(SumDistributedLabel, 2, 0)
        Me.TableLayoutPanel8.Controls.Add(Me.SumAccTextBox, 0, 0)
        Me.TableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel8.Location = New System.Drawing.Point(148, 120)
        Me.TableLayoutPanel8.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
        Me.TableLayoutPanel8.RowCount = 1
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel8.Size = New System.Drawing.Size(713, 30)
        Me.TableLayoutPanel8.TabIndex = 4
        '
        'DistributeButton
        '
        Me.DistributeButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DistributeButton.Location = New System.Drawing.Point(565, 3)
        Me.DistributeButton.Name = "DistributeButton"
        Me.DistributeButton.Size = New System.Drawing.Size(125, 23)
        Me.DistributeButton.TabIndex = 1
        Me.DistributeButton.Text = "Paskirstyti Sumą"
        Me.DistributeButton.UseVisualStyleBackColor = True
        '
        'SumDistributedAccTextBox
        '
        Me.SumDistributedAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.AccumulativeCostsBindingSource, "SumDistributed", True))
        Me.SumDistributedAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SumDistributedAccTextBox.Location = New System.Drawing.Point(341, 3)
        Me.SumDistributedAccTextBox.Name = "SumDistributedAccTextBox"
        Me.SumDistributedAccTextBox.ReadOnly = True
        Me.SumDistributedAccTextBox.Size = New System.Drawing.Size(198, 20)
        Me.SumDistributedAccTextBox.TabIndex = 6
        Me.SumDistributedAccTextBox.TabStop = False
        Me.SumDistributedAccTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'AccumulativeCostsBindingSource
        '
        Me.AccumulativeCostsBindingSource.DataSource = GetType(ApskaitaObjects.Documents.AccumulativeCosts)
        '
        'SumAccTextBox
        '
        Me.SumAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.AccumulativeCostsBindingSource, "Sum", True))
        Me.SumAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SumAccTextBox.Location = New System.Drawing.Point(3, 3)
        Me.SumAccTextBox.Name = "SumAccTextBox"
        Me.SumAccTextBox.NegativeValue = False
        Me.SumAccTextBox.Size = New System.Drawing.Size(198, 20)
        Me.SumAccTextBox.TabIndex = 0
        Me.SumAccTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 8
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.AccountDistributedCostsAccGridComboBox, 6, 0)
        Me.TableLayoutPanel5.Controls.Add(AccountDistributedCostsLabel, 5, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.AccountCostsAccGridComboBox, 0, 0)
        Me.TableLayoutPanel5.Controls.Add(AccountAccumulatedCostsLabel, 2, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.AccountAccumulatedCostsAccGridComboBox, 3, 0)
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(148, 60)
        Me.TableLayoutPanel5.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 1
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(713, 30)
        Me.TableLayoutPanel5.TabIndex = 2
        '
        'AccountDistributedCostsAccGridComboBox
        '
        Me.AccountDistributedCostsAccGridComboBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.AccumulativeCostsBindingSource, "AccountDistributedCosts", True))
        Me.AccountDistributedCostsAccGridComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccountDistributedCostsAccGridComboBox.EmptyValueString = ""
        Me.AccountDistributedCostsAccGridComboBox.InstantBinding = True
        Me.AccountDistributedCostsAccGridComboBox.Location = New System.Drawing.Point(568, 3)
        Me.AccountDistributedCostsAccGridComboBox.Name = "AccountDistributedCostsAccGridComboBox"
        Me.AccountDistributedCostsAccGridComboBox.Size = New System.Drawing.Size(121, 21)
        Me.AccountDistributedCostsAccGridComboBox.TabIndex = 2
        '
        'AccountCostsAccGridComboBox
        '
        Me.AccountCostsAccGridComboBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.AccumulativeCostsBindingSource, "AccountCosts", True))
        Me.AccountCostsAccGridComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccountCostsAccGridComboBox.EmptyValueString = ""
        Me.AccountCostsAccGridComboBox.InstantBinding = True
        Me.AccountCostsAccGridComboBox.Location = New System.Drawing.Point(3, 3)
        Me.AccountCostsAccGridComboBox.Name = "AccountCostsAccGridComboBox"
        Me.AccountCostsAccGridComboBox.Size = New System.Drawing.Size(121, 21)
        Me.AccountCostsAccGridComboBox.TabIndex = 0
        '
        'AccountAccumulatedCostsAccGridComboBox
        '
        Me.AccountAccumulatedCostsAccGridComboBox.DataBindings.Add(New System.Windows.Forms.Binding("SelectedValue", Me.AccumulativeCostsBindingSource, "AccountAccumulatedCosts", True))
        Me.AccountAccumulatedCostsAccGridComboBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AccountAccumulatedCostsAccGridComboBox.EmptyValueString = ""
        Me.AccountAccumulatedCostsAccGridComboBox.InstantBinding = True
        Me.AccountAccumulatedCostsAccGridComboBox.Location = New System.Drawing.Point(285, 3)
        Me.AccountAccumulatedCostsAccGridComboBox.Name = "AccountAccumulatedCostsAccGridComboBox"
        Me.AccountAccumulatedCostsAccGridComboBox.Size = New System.Drawing.Size(121, 21)
        Me.AccountAccumulatedCostsAccGridComboBox.TabIndex = 1
        '
        'TableLayoutPanel6
        '
        Me.TableLayoutPanel6.ColumnCount = 2
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel6.Controls.Add(Me.ContentTextBox, 0, 0)
        Me.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel6.Location = New System.Drawing.Point(148, 90)
        Me.TableLayoutPanel6.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
        Me.TableLayoutPanel6.RowCount = 1
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel6.Size = New System.Drawing.Size(713, 30)
        Me.TableLayoutPanel6.TabIndex = 3
        '
        'ContentTextBox
        '
        Me.ContentTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AccumulativeCostsBindingSource, "Content", True))
        Me.ContentTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ContentTextBox.Location = New System.Drawing.Point(3, 3)
        Me.ContentTextBox.MaxLength = 255
        Me.ContentTextBox.Name = "ContentTextBox"
        Me.ContentTextBox.Size = New System.Drawing.Size(687, 20)
        Me.ContentTextBox.TabIndex = 0
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.AutoScroll = True
        Me.TableLayoutPanel4.ColumnCount = 7
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.IsAccumulatedIncomeCheckBox, 5, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.DocumentNumberTextBox, 3, 0)
        Me.TableLayoutPanel4.Controls.Add(DocumentNumberLabel, 2, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.DateAccDatePicker, 0, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(148, 30)
        Me.TableLayoutPanel4.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(713, 30)
        Me.TableLayoutPanel4.TabIndex = 1
        '
        'IsAccumulatedIncomeCheckBox
        '
        Me.IsAccumulatedIncomeCheckBox.AutoSize = True
        Me.IsAccumulatedIncomeCheckBox.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.AccumulativeCostsBindingSource, "IsAccumulatedIncome", True))
        Me.IsAccumulatedIncomeCheckBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IsAccumulatedIncomeCheckBox.Location = New System.Drawing.Point(553, 6)
        Me.IsAccumulatedIncomeCheckBox.Margin = New System.Windows.Forms.Padding(3, 6, 3, 3)
        Me.IsAccumulatedIncomeCheckBox.Name = "IsAccumulatedIncomeCheckBox"
        Me.IsAccumulatedIncomeCheckBox.Size = New System.Drawing.Size(137, 17)
        Me.IsAccumulatedIncomeCheckBox.TabIndex = 2
        Me.IsAccumulatedIncomeCheckBox.Text = "Sukauptos Pajamos"
        Me.IsAccumulatedIncomeCheckBox.TextAlign = System.Drawing.ContentAlignment.TopLeft
        '
        'DocumentNumberTextBox
        '
        Me.DocumentNumberTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AccumulativeCostsBindingSource, "DocumentNumber", True))
        Me.DocumentNumberTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DocumentNumberTextBox.Location = New System.Drawing.Point(266, 3)
        Me.DocumentNumberTextBox.MaxLength = 30
        Me.DocumentNumberTextBox.Name = "DocumentNumberTextBox"
        Me.DocumentNumberTextBox.Size = New System.Drawing.Size(261, 20)
        Me.DocumentNumberTextBox.TabIndex = 1
        Me.DocumentNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DateAccDatePicker
        '
        Me.DateAccDatePicker.BoldedDates = Nothing
        Me.DateAccDatePicker.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.AccumulativeCostsBindingSource, "Date", True))
        Me.DateAccDatePicker.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DateAccDatePicker.Location = New System.Drawing.Point(3, 3)
        Me.DateAccDatePicker.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.DateAccDatePicker.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.DateAccDatePicker.Name = "DateAccDatePicker"
        Me.DateAccDatePicker.ReadOnly = False
        Me.DateAccDatePicker.ShowWeekNumbers = True
        Me.DateAccDatePicker.Size = New System.Drawing.Size(172, 24)
        Me.DateAccDatePicker.TabIndex = 0
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 8
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.UpdateDateTextBox, 6, 0)
        Me.TableLayoutPanel3.Controls.Add(UpdateDateLabel, 5, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.InsertDateTextBox, 3, 0)
        Me.TableLayoutPanel3.Controls.Add(InsertDateLabel, 2, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.IDTextBox, 0, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(148, 0)
        Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(713, 30)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'UpdateDateTextBox
        '
        Me.UpdateDateTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AccumulativeCostsBindingSource, "UpdateDate", True))
        Me.UpdateDateTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UpdateDateTextBox.Location = New System.Drawing.Point(508, 3)
        Me.UpdateDateTextBox.Name = "UpdateDateTextBox"
        Me.UpdateDateTextBox.ReadOnly = True
        Me.UpdateDateTextBox.Size = New System.Drawing.Size(176, 20)
        Me.UpdateDateTextBox.TabIndex = 8
        Me.UpdateDateTextBox.TabStop = False
        Me.UpdateDateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'InsertDateTextBox
        '
        Me.InsertDateTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AccumulativeCostsBindingSource, "InsertDate", True))
        Me.InsertDateTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InsertDateTextBox.Location = New System.Drawing.Point(240, 3)
        Me.InsertDateTextBox.Name = "InsertDateTextBox"
        Me.InsertDateTextBox.ReadOnly = True
        Me.InsertDateTextBox.Size = New System.Drawing.Size(176, 20)
        Me.InsertDateTextBox.TabIndex = 10
        Me.InsertDateTextBox.TabStop = False
        Me.InsertDateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'IDTextBox
        '
        Me.IDTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AccumulativeCostsBindingSource, "ID", True))
        Me.IDTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IDTextBox.Location = New System.Drawing.Point(3, 3)
        Me.IDTextBox.Name = "IDTextBox"
        Me.IDTextBox.ReadOnly = True
        Me.IDTextBox.Size = New System.Drawing.Size(150, 20)
        Me.IDTextBox.TabIndex = 7
        Me.IDTextBox.TabStop = False
        Me.IDTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.ColumnCount = 2
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel7.Controls.Add(Me.CommentsTextBox, 0, 0)
        Me.TableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel7.Location = New System.Drawing.Point(148, 150)
        Me.TableLayoutPanel7.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
        Me.TableLayoutPanel7.RowCount = 1
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.Size = New System.Drawing.Size(713, 60)
        Me.TableLayoutPanel7.TabIndex = 5
        '
        'CommentsTextBox
        '
        Me.CommentsTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.AccumulativeCostsBindingSource, "Comments", True))
        Me.CommentsTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CommentsTextBox.Location = New System.Drawing.Point(3, 3)
        Me.CommentsTextBox.MaxLength = 255
        Me.CommentsTextBox.Multiline = True
        Me.CommentsTextBox.Name = "CommentsTextBox"
        Me.CommentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.CommentsTextBox.Size = New System.Drawing.Size(687, 54)
        Me.CommentsTextBox.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.AutoSize = True
        Me.Panel2.Controls.Add(Me.EditedBaner)
        Me.Panel2.Controls.Add(Me.nCancelButton)
        Me.Panel2.Controls.Add(Me.ApplyButton)
        Me.Panel2.Controls.Add(Me.nOkButton)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 539)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Padding = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.Panel2.Size = New System.Drawing.Size(861, 37)
        Me.Panel2.TabIndex = 2
        '
        'EditedBaner
        '
        Me.EditedBaner.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EditedBaner.BackColor = System.Drawing.Color.Red
        Me.EditedBaner.Controls.Add(Me.Label4)
        Me.EditedBaner.Location = New System.Drawing.Point(510, 5)
        Me.EditedBaner.Name = "EditedBaner"
        Me.EditedBaner.Size = New System.Drawing.Size(83, 25)
        Me.EditedBaner.TabIndex = 51
        Me.EditedBaner.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "TAISOMA"
        '
        'nCancelButton
        '
        Me.nCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.nCancelButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nCancelButton.Location = New System.Drawing.Point(774, 7)
        Me.nCancelButton.Name = "nCancelButton"
        Me.nCancelButton.Size = New System.Drawing.Size(75, 23)
        Me.nCancelButton.TabIndex = 2
        Me.nCancelButton.Text = "Atšaukti"
        Me.nCancelButton.UseVisualStyleBackColor = True
        '
        'ApplyButton
        '
        Me.ApplyButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ApplyButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ApplyButton.Location = New System.Drawing.Point(693, 7)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(75, 23)
        Me.ApplyButton.TabIndex = 1
        Me.ApplyButton.Text = "Taikyti"
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'nOkButton
        '
        Me.nOkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nOkButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nOkButton.Location = New System.Drawing.Point(612, 7)
        Me.nOkButton.Name = "nOkButton"
        Me.nOkButton.Size = New System.Drawing.Size(75, 23)
        Me.nOkButton.TabIndex = 0
        Me.nOkButton.Text = "OK"
        Me.nOkButton.UseVisualStyleBackColor = True
        '
        'ItemsSortedBindingSource
        '
        Me.ItemsSortedBindingSource.DataMember = "Items"
        Me.ItemsSortedBindingSource.DataSource = Me.AccumulativeCostsBindingSource
        '
        'ItemsDataListView
        '
        Me.ItemsDataListView.AllColumns.Add(Me.OlvColumn2)
        Me.ItemsDataListView.AllColumns.Add(Me.OlvColumn1)
        Me.ItemsDataListView.AllColumns.Add(Me.OlvColumn3)
        Me.ItemsDataListView.AllowColumnReorder = True
        Me.ItemsDataListView.AutoGenerateColumns = False
        Me.ItemsDataListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways
        Me.ItemsDataListView.CellEditEnterChangesRows = True
        Me.ItemsDataListView.CellEditTabChangesRows = True
        Me.ItemsDataListView.CellEditUseWholeCell = False
        Me.ItemsDataListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.OlvColumn2, Me.OlvColumn3})
        Me.ItemsDataListView.Cursor = System.Windows.Forms.Cursors.Default
        Me.ItemsDataListView.DataSource = Me.ItemsSortedBindingSource
        Me.ItemsDataListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ItemsDataListView.FullRowSelect = True
        Me.ItemsDataListView.HasCollapsibleGroups = False
        Me.ItemsDataListView.HeaderWordWrap = True
        Me.ItemsDataListView.HideSelection = False
        Me.ItemsDataListView.IncludeColumnHeadersInCopy = True
        Me.ItemsDataListView.Location = New System.Drawing.Point(0, 264)
        Me.ItemsDataListView.Name = "ItemsDataListView"
        Me.ItemsDataListView.RenderNonEditableCheckboxesAsDisabled = True
        Me.ItemsDataListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu
        Me.ItemsDataListView.SelectedBackColor = System.Drawing.Color.PaleGreen
        Me.ItemsDataListView.SelectedForeColor = System.Drawing.Color.Black
        Me.ItemsDataListView.ShowCommandMenuOnRightClick = True
        Me.ItemsDataListView.ShowGroups = False
        Me.ItemsDataListView.ShowImagesOnSubItems = True
        Me.ItemsDataListView.ShowItemCountOnGroups = True
        Me.ItemsDataListView.ShowItemToolTips = True
        Me.ItemsDataListView.Size = New System.Drawing.Size(861, 275)
        Me.ItemsDataListView.TabIndex = 3
        Me.ItemsDataListView.UnfocusedSelectedBackColor = System.Drawing.Color.PaleGreen
        Me.ItemsDataListView.UnfocusedSelectedForeColor = System.Drawing.Color.Black
        Me.ItemsDataListView.UseCellFormatEvents = True
        Me.ItemsDataListView.UseCompatibleStateImageBehavior = False
        Me.ItemsDataListView.UseFilterIndicator = True
        Me.ItemsDataListView.UseFiltering = True
        Me.ItemsDataListView.UseHotItem = True
        Me.ItemsDataListView.UseNotifyPropertyChanged = True
        Me.ItemsDataListView.View = System.Windows.Forms.View.Details
        '
        'OlvColumn2
        '
        Me.OlvColumn2.AspectName = "Date"
        Me.OlvColumn2.CellEditUseWholeCell = True
        Me.OlvColumn2.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn2.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn2.Text = "Data"
        Me.OlvColumn2.ToolTipText = ""
        Me.OlvColumn2.Width = 100
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
        Me.OlvColumn3.AspectName = "Sum"
        Me.OlvColumn3.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn3.CellEditUseWholeCell = True
        Me.OlvColumn3.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn3.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn3.Text = "Suma"
        Me.OlvColumn3.ToolTipText = ""
        Me.OlvColumn3.Width = 100
        '
        'ProgressFiller1
        '
        Me.ProgressFiller1.Location = New System.Drawing.Point(237, 289)
        Me.ProgressFiller1.Name = "ProgressFiller1"
        Me.ProgressFiller1.Size = New System.Drawing.Size(246, 73)
        Me.ProgressFiller1.TabIndex = 4
        Me.ProgressFiller1.Visible = False
        '
        'ErrorWarnInfoProvider1
        '
        Me.ErrorWarnInfoProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorWarnInfoProvider1.BlinkStyleInformation = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorWarnInfoProvider1.BlinkStyleWarning = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorWarnInfoProvider1.ContainerControl = Me
        Me.ErrorWarnInfoProvider1.DataSource = Me.AccumulativeCostsBindingSource
        '
        'F_AccumulativeCosts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(861, 576)
        Me.Controls.Add(Me.ItemsDataListView)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.ProgressFiller1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "F_AccumulativeCosts"
        Me.ShowInTaskbar = False
        Me.Text = "Sukauptos sąnaudos"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.PeriodCountNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PeriodLengthNumericUpDown, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel8.ResumeLayout(False)
        Me.TableLayoutPanel8.PerformLayout()
        CType(Me.AccumulativeCostsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.TableLayoutPanel5.PerformLayout()
        Me.TableLayoutPanel6.ResumeLayout(False)
        Me.TableLayoutPanel6.PerformLayout()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel7.ResumeLayout(False)
        Me.TableLayoutPanel7.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.EditedBaner.ResumeLayout(False)
        Me.EditedBaner.PerformLayout()
        CType(Me.ItemsSortedBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ItemsDataListView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorWarnInfoProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents UpdateDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AccumulativeCostsBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents InsertDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents IDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DocumentNumberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AccountDistributedCostsAccGridComboBox As AccControlsWinForms.AccListComboBox
    Friend WithEvents AccountCostsAccGridComboBox As AccControlsWinForms.AccListComboBox
    Friend WithEvents AccountAccumulatedCostsAccGridComboBox As AccControlsWinForms.AccListComboBox
    Friend WithEvents TableLayoutPanel6 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel7 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel8 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DistributeButton As System.Windows.Forms.Button
    Friend WithEvents SumDistributedAccTextBox As AccControlsWinForms.AccTextBox
    Friend WithEvents SumAccTextBox As AccControlsWinForms.AccTextBox
    Friend WithEvents ContentTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CommentsTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PeriodLengthNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents PeriodCountNumericUpDown As System.Windows.Forms.NumericUpDown
    Friend WithEvents NewDistributionButton As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents EditedBaner As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents nCancelButton As System.Windows.Forms.Button
    Friend WithEvents ApplyButton As System.Windows.Forms.Button
    Friend WithEvents nOkButton As System.Windows.Forms.Button
    Friend WithEvents ItemsSortedBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents IsAccumulatedIncomeCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ItemsDataListView As BrightIdeasSoftware.DataListView
    Friend WithEvents OlvColumn1 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn2 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn3 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents ProgressFiller1 As AccControlsWinForms.ProgressFiller
    Friend WithEvents ErrorWarnInfoProvider1 As AccControlsWinForms.ErrorWarnInfoProvider
    Friend WithEvents DateAccDatePicker As AccControlsWinForms.AccDatePicker
    Friend WithEvents FirstPeriodAccDatePicker As AccControlsWinForms.AccDatePicker
End Class
