<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class F_TransferOfBalance
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
        Dim CreditSumLabel As System.Windows.Forms.Label
        Dim DateLabel As System.Windows.Forms.Label
        Dim DebetSumLabel As System.Windows.Forms.Label
        Dim IDLabel As System.Windows.Forms.Label
        Dim InsertDateLabel As System.Windows.Forms.Label
        Dim UpdateDateLabel As System.Windows.Forms.Label
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(F_TransferOfBalance))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.DebetListDataGridView = New System.Windows.Forms.DataGridView
        Me.DebetListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TransferOfBalanceBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.CreditListDataGridView = New System.Windows.Forms.DataGridView
        Me.CreditListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PasteAccButton = New AccControls.AccButton
        Me.LimitationsButton = New System.Windows.Forms.Button
        Me.nCancelButton = New System.Windows.Forms.Button
        Me.ApplyButton = New System.Windows.Forms.Button
        Me.nOkButton = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.UpdateDateTextBox = New System.Windows.Forms.TextBox
        Me.CreditSumAccTextBox = New AccControls.AccTextBox
        Me.DateDateTimePicker = New System.Windows.Forms.DateTimePicker
        Me.DebetSumAccTextBox = New AccControls.AccTextBox
        Me.IDTextBox = New System.Windows.Forms.TextBox
        Me.InsertDateTextBox = New System.Windows.Forms.TextBox
        Me.AnalyticListDataGridView = New System.Windows.Forms.DataGridView
        Me.EntryTypeHumanReadableDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.AccountDataGridViewTextBoxColumn = New AccControls.DataGridViewAccGridComboBoxColumn
        Me.AmmountDataGridViewTextBoxColumn = New AccControls.DataGridViewAccTextBoxColumn
        Me.PersonDataGridViewTextBoxColumn = New AccControls.DataGridViewAccGridComboBoxColumn
        Me.AnalyticsListSortedBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ReadWriteAuthorization1 = New Csla.Windows.ReadWriteAuthorization(Me.components)
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.DataGridViewTextBoxColumn3 = New AccControls.DataGridViewAccGridComboBoxColumn
        Me.DataGridViewTextBoxColumn4 = New AccControls.DataGridViewAccTextBoxColumn
        Me.DataGridViewTextBoxColumn8 = New AccControls.DataGridViewAccGridComboBoxColumn
        Me.DataGridViewTextBoxColumn9 = New AccControls.DataGridViewAccTextBoxColumn
        CreditSumLabel = New System.Windows.Forms.Label
        DateLabel = New System.Windows.Forms.Label
        DebetSumLabel = New System.Windows.Forms.Label
        IDLabel = New System.Windows.Forms.Label
        InsertDateLabel = New System.Windows.Forms.Label
        UpdateDateLabel = New System.Windows.Forms.Label
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.DebetListDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DebetListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TransferOfBalanceBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CreditListDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CreditListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.AnalyticListDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AnalyticsListSortedBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CreditSumLabel
        '
        CreditSumLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(CreditSumLabel, False)
        CreditSumLabel.AutoSize = True
        CreditSumLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        CreditSumLabel.Location = New System.Drawing.Point(245, 35)
        CreditSumLabel.Margin = New System.Windows.Forms.Padding(3, 5, 3, 0)
        CreditSumLabel.Name = "CreditSumLabel"
        CreditSumLabel.Size = New System.Drawing.Size(73, 13)
        CreditSumLabel.TabIndex = 1
        CreditSumLabel.Text = "KREDITAS:"
        '
        'DateLabel
        '
        DateLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(DateLabel, False)
        DateLabel.AutoSize = True
        DateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DateLabel.Location = New System.Drawing.Point(514, 35)
        DateLabel.Margin = New System.Windows.Forms.Padding(3, 5, 3, 0)
        DateLabel.Name = "DateLabel"
        DateLabel.Size = New System.Drawing.Size(38, 13)
        DateLabel.TabIndex = 3
        DateLabel.Text = "Data:"
        '
        'DebetSumLabel
        '
        DebetSumLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(DebetSumLabel, False)
        DebetSumLabel.AutoSize = True
        DebetSumLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DebetSumLabel.Location = New System.Drawing.Point(3, 35)
        DebetSumLabel.Margin = New System.Windows.Forms.Padding(3, 5, 3, 0)
        DebetSumLabel.Name = "DebetSumLabel"
        DebetSumLabel.Size = New System.Drawing.Size(68, 13)
        DebetSumLabel.TabIndex = 5
        DebetSumLabel.Text = "DEBETAS:"
        '
        'IDLabel
        '
        IDLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(IDLabel, False)
        IDLabel.AutoSize = True
        IDLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        IDLabel.Location = New System.Drawing.Point(47, 5)
        IDLabel.Margin = New System.Windows.Forms.Padding(3, 5, 3, 0)
        IDLabel.Name = "IDLabel"
        IDLabel.Size = New System.Drawing.Size(24, 13)
        IDLabel.TabIndex = 8
        IDLabel.Text = "ID:"
        '
        'InsertDateLabel
        '
        InsertDateLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(InsertDateLabel, False)
        InsertDateLabel.AutoSize = True
        InsertDateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        InsertDateLabel.Location = New System.Drawing.Point(263, 5)
        InsertDateLabel.Margin = New System.Windows.Forms.Padding(3, 5, 3, 0)
        InsertDateLabel.Name = "InsertDateLabel"
        InsertDateLabel.Size = New System.Drawing.Size(55, 13)
        InsertDateLabel.TabIndex = 9
        InsertDateLabel.Text = "Įtraukta:"
        '
        'UpdateDateLabel
        '
        UpdateDateLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(UpdateDateLabel, False)
        UpdateDateLabel.AutoSize = True
        UpdateDateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        UpdateDateLabel.Location = New System.Drawing.Point(492, 5)
        UpdateDateLabel.Margin = New System.Windows.Forms.Padding(3, 5, 3, 0)
        UpdateDateLabel.Name = "UpdateDateLabel"
        UpdateDateLabel.Size = New System.Drawing.Size(60, 13)
        UpdateDateLabel.TabIndex = 11
        UpdateDateLabel.Text = "Pakeista:"
        '
        'SplitContainer1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainer1, False)
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainer1.Panel1, False)
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel2)
        '
        'SplitContainer1.Panel2
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainer1.Panel2, False)
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.AnalyticListDataGridView)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(5)
        Me.SplitContainer1.Size = New System.Drawing.Size(723, 539)
        Me.SplitContainer1.SplitterDistance = 307
        Me.SplitContainer1.TabIndex = 0
        '
        'SplitContainer2
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainer2, False)
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.IsSplitterFixed = True
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 60)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainer2.Panel1, False)
        Me.SplitContainer2.Panel1.AutoScroll = True
        Me.SplitContainer2.Panel1.Controls.Add(Me.DebetListDataGridView)
        Me.SplitContainer2.Panel1.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        '
        'SplitContainer2.Panel2
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.SplitContainer2.Panel2, False)
        Me.SplitContainer2.Panel2.AutoScroll = True
        Me.SplitContainer2.Panel2.Controls.Add(Me.CreditListDataGridView)
        Me.SplitContainer2.Panel2.Padding = New System.Windows.Forms.Padding(0, 0, 5, 0)
        Me.SplitContainer2.Size = New System.Drawing.Size(620, 247)
        Me.SplitContainer2.SplitterDistance = 307
        Me.SplitContainer2.SplitterWidth = 10
        Me.SplitContainer2.TabIndex = 2
        '
        'DebetListDataGridView
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.DebetListDataGridView, True)
        Me.DebetListDataGridView.AutoGenerateColumns = False
        Me.DebetListDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DebetListDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DebetListDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DebetListDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4})
        Me.DebetListDataGridView.DataSource = Me.DebetListBindingSource
        Me.DebetListDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DebetListDataGridView.Location = New System.Drawing.Point(5, 0)
        Me.DebetListDataGridView.Name = "DebetListDataGridView"
        Me.DebetListDataGridView.RowHeadersWidth = 20
        Me.DebetListDataGridView.Size = New System.Drawing.Size(302, 247)
        Me.DebetListDataGridView.TabIndex = 0
        '
        'DebetListBindingSource
        '
        Me.DebetListBindingSource.DataMember = "DebetListSorted"
        Me.DebetListBindingSource.DataSource = Me.TransferOfBalanceBindingSource
        '
        'TransferOfBalanceBindingSource
        '
        Me.TransferOfBalanceBindingSource.DataSource = GetType(ApskaitaObjects.General.TransferOfBalance)
        '
        'CreditListDataGridView
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.CreditListDataGridView, True)
        Me.CreditListDataGridView.AutoGenerateColumns = False
        Me.CreditListDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.CreditListDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CreditListDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.CreditListDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn8, Me.DataGridViewTextBoxColumn9})
        Me.CreditListDataGridView.DataSource = Me.CreditListBindingSource
        Me.CreditListDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CreditListDataGridView.Location = New System.Drawing.Point(0, 0)
        Me.CreditListDataGridView.Name = "CreditListDataGridView"
        Me.CreditListDataGridView.RowHeadersWidth = 20
        Me.CreditListDataGridView.Size = New System.Drawing.Size(298, 247)
        Me.CreditListDataGridView.TabIndex = 0
        '
        'CreditListBindingSource
        '
        Me.CreditListBindingSource.DataMember = "CreditListSorted"
        Me.CreditListBindingSource.DataSource = Me.TransferOfBalanceBindingSource
        '
        'Panel1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.Panel1, False)
        Me.Panel1.AutoSize = True
        Me.Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel1.Controls.Add(Me.PasteAccButton)
        Me.Panel1.Controls.Add(Me.LimitationsButton)
        Me.Panel1.Controls.Add(Me.nCancelButton)
        Me.Panel1.Controls.Add(Me.ApplyButton)
        Me.Panel1.Controls.Add(Me.nOkButton)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(620, 60)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(103, 247)
        Me.Panel1.TabIndex = 2
        '
        'PasteAccButton
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.PasteAccButton, False)
        Me.PasteAccButton.BorderStyleDown = System.Windows.Forms.Border3DStyle.Sunken
        Me.PasteAccButton.BorderStyleNormal = System.Windows.Forms.Border3DStyle.Raised
        Me.PasteAccButton.BorderStyleUp = System.Windows.Forms.Border3DStyle.Raised
        Me.PasteAccButton.ButtonStyle = AccControls.rsButtonStyle.DropDownWithSep
        Me.PasteAccButton.Checked = False
        Me.PasteAccButton.DropDownSepWidth = 12
        Me.PasteAccButton.FocusRectangle = False
        Me.PasteAccButton.Image = Global.ApskaitaWUI.My.Resources.Resources.Paste_icon_24p
        Me.PasteAccButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.PasteAccButton.ImagePadding = 2
        Me.PasteAccButton.Location = New System.Drawing.Point(35, 107)
        Me.PasteAccButton.Name = "PasteAccButton"
        Me.PasteAccButton.Size = New System.Drawing.Size(45, 30)
        Me.PasteAccButton.TabIndex = 71
        Me.PasteAccButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.PasteAccButton.TextPadding = 2
        '
        'LimitationsButton
        '
        Me.LimitationsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.LimitationsButton, False)
        Me.LimitationsButton.Image = Global.ApskaitaWUI.My.Resources.Resources.Action_lock_icon_32p
        Me.LimitationsButton.Location = New System.Drawing.Point(30, 156)
        Me.LimitationsButton.Name = "LimitationsButton"
        Me.LimitationsButton.Size = New System.Drawing.Size(50, 40)
        Me.LimitationsButton.TabIndex = 3
        Me.LimitationsButton.UseVisualStyleBackColor = True
        '
        'nCancelButton
        '
        Me.nCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.nCancelButton, False)
        Me.nCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.nCancelButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nCancelButton.Location = New System.Drawing.Point(16, 67)
        Me.nCancelButton.Name = "nCancelButton"
        Me.nCancelButton.Size = New System.Drawing.Size(75, 23)
        Me.nCancelButton.TabIndex = 2
        Me.nCancelButton.Text = "Atšaukti"
        Me.nCancelButton.UseVisualStyleBackColor = True
        '
        'ApplyButton
        '
        Me.ApplyButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.ApplyButton, False)
        Me.ApplyButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ApplyButton.Location = New System.Drawing.Point(16, 38)
        Me.ApplyButton.Name = "ApplyButton"
        Me.ApplyButton.Size = New System.Drawing.Size(75, 23)
        Me.ApplyButton.TabIndex = 1
        Me.ApplyButton.Text = "Taikyti"
        Me.ApplyButton.UseVisualStyleBackColor = True
        '
        'nOkButton
        '
        Me.nOkButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.nOkButton, False)
        Me.nOkButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nOkButton.Location = New System.Drawing.Point(16, 9)
        Me.nOkButton.Name = "nOkButton"
        Me.nOkButton.Size = New System.Drawing.Size(75, 23)
        Me.nOkButton.TabIndex = 0
        Me.nOkButton.Text = "OK"
        Me.nOkButton.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.Panel2, False)
        Me.Panel2.AutoScroll = True
        Me.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Padding = New System.Windows.Forms.Padding(0, 0, 0, 5)
        Me.Panel2.Size = New System.Drawing.Size(723, 60)
        Me.Panel2.TabIndex = 1
        '
        'TableLayoutPanel1
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.TableLayoutPanel1, False)
        Me.TableLayoutPanel1.ColumnCount = 9
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.UpdateDateTextBox, 7, 0)
        Me.TableLayoutPanel1.Controls.Add(DateLabel, 6, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.CreditSumAccTextBox, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.DateDateTimePicker, 7, 1)
        Me.TableLayoutPanel1.Controls.Add(UpdateDateLabel, 6, 0)
        Me.TableLayoutPanel1.Controls.Add(CreditSumLabel, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.DebetSumAccTextBox, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(IDLabel, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.IDTextBox, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.InsertDateTextBox, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(InsertDateLabel, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(DebetSumLabel, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(723, 55)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'UpdateDateTextBox
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.UpdateDateTextBox, False)
        Me.UpdateDateTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TransferOfBalanceBindingSource, "UpdateDate", True))
        Me.UpdateDateTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UpdateDateTextBox.Location = New System.Drawing.Point(558, 3)
        Me.UpdateDateTextBox.Name = "UpdateDateTextBox"
        Me.UpdateDateTextBox.ReadOnly = True
        Me.UpdateDateTextBox.Size = New System.Drawing.Size(142, 20)
        Me.UpdateDateTextBox.TabIndex = 12
        Me.UpdateDateTextBox.TabStop = False
        Me.UpdateDateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CreditSumAccTextBox
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.CreditSumAccTextBox, False)
        Me.CreditSumAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.TransferOfBalanceBindingSource, "CreditSum", True))
        Me.CreditSumAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CreditSumAccTextBox.KeepBackColorWhenReadOnly = False
        Me.CreditSumAccTextBox.Location = New System.Drawing.Point(324, 33)
        Me.CreditSumAccTextBox.Name = "CreditSumAccTextBox"
        Me.CreditSumAccTextBox.ReadOnly = True
        Me.CreditSumAccTextBox.Size = New System.Drawing.Size(142, 20)
        Me.CreditSumAccTextBox.TabIndex = 7
        Me.CreditSumAccTextBox.TabStop = False
        Me.CreditSumAccTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DateDateTimePicker
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.DateDateTimePicker, True)
        Me.DateDateTimePicker.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.TransferOfBalanceBindingSource, "Date", True))
        Me.DateDateTimePicker.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DateDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateDateTimePicker.Location = New System.Drawing.Point(558, 33)
        Me.DateDateTimePicker.Name = "DateDateTimePicker"
        Me.DateDateTimePicker.Size = New System.Drawing.Size(142, 20)
        Me.DateDateTimePicker.TabIndex = 0
        '
        'DebetSumAccTextBox
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.DebetSumAccTextBox, False)
        Me.DebetSumAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.TransferOfBalanceBindingSource, "DebetSum", True))
        Me.DebetSumAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DebetSumAccTextBox.KeepBackColorWhenReadOnly = False
        Me.DebetSumAccTextBox.Location = New System.Drawing.Point(77, 33)
        Me.DebetSumAccTextBox.Name = "DebetSumAccTextBox"
        Me.DebetSumAccTextBox.ReadOnly = True
        Me.DebetSumAccTextBox.Size = New System.Drawing.Size(142, 20)
        Me.DebetSumAccTextBox.TabIndex = 6
        Me.DebetSumAccTextBox.TabStop = False
        Me.DebetSumAccTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'IDTextBox
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.IDTextBox, False)
        Me.IDTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TransferOfBalanceBindingSource, "ID", True))
        Me.IDTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.IDTextBox.Location = New System.Drawing.Point(77, 3)
        Me.IDTextBox.Name = "IDTextBox"
        Me.IDTextBox.ReadOnly = True
        Me.IDTextBox.Size = New System.Drawing.Size(142, 20)
        Me.IDTextBox.TabIndex = 9
        Me.IDTextBox.TabStop = False
        Me.IDTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'InsertDateTextBox
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.InsertDateTextBox, False)
        Me.InsertDateTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.TransferOfBalanceBindingSource, "InsertDate", True))
        Me.InsertDateTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InsertDateTextBox.Location = New System.Drawing.Point(324, 3)
        Me.InsertDateTextBox.Name = "InsertDateTextBox"
        Me.InsertDateTextBox.ReadOnly = True
        Me.InsertDateTextBox.Size = New System.Drawing.Size(142, 20)
        Me.InsertDateTextBox.TabIndex = 10
        Me.InsertDateTextBox.TabStop = False
        Me.InsertDateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'AnalyticListDataGridView
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me.AnalyticListDataGridView, True)
        Me.AnalyticListDataGridView.AutoGenerateColumns = False
        Me.AnalyticListDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.AnalyticListDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AnalyticListDataGridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.AnalyticListDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.EntryTypeHumanReadableDataGridViewTextBoxColumn, Me.AccountDataGridViewTextBoxColumn, Me.AmmountDataGridViewTextBoxColumn, Me.PersonDataGridViewTextBoxColumn})
        Me.AnalyticListDataGridView.DataSource = Me.AnalyticsListSortedBindingSource
        Me.AnalyticListDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AnalyticListDataGridView.Location = New System.Drawing.Point(5, 5)
        Me.AnalyticListDataGridView.Name = "AnalyticListDataGridView"
        Me.AnalyticListDataGridView.RowHeadersWidth = 20
        Me.AnalyticListDataGridView.Size = New System.Drawing.Size(713, 218)
        Me.AnalyticListDataGridView.TabIndex = 0
        '
        'EntryTypeHumanReadableDataGridViewTextBoxColumn
        '
        Me.EntryTypeHumanReadableDataGridViewTextBoxColumn.DataPropertyName = "EntryTypeHumanReadable"
        Me.EntryTypeHumanReadableDataGridViewTextBoxColumn.HeaderText = "Tipas"
        Me.EntryTypeHumanReadableDataGridViewTextBoxColumn.Name = "EntryTypeHumanReadableDataGridViewTextBoxColumn"
        Me.EntryTypeHumanReadableDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.EntryTypeHumanReadableDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.EntryTypeHumanReadableDataGridViewTextBoxColumn.Width = 63
        '
        'AccountDataGridViewTextBoxColumn
        '
        Me.AccountDataGridViewTextBoxColumn.CloseOnSingleClick = True
        Me.AccountDataGridViewTextBoxColumn.ComboDataGridView = Nothing
        Me.AccountDataGridViewTextBoxColumn.DataPropertyName = "Account"
        Me.AccountDataGridViewTextBoxColumn.EmptyValueString = ""
        Me.AccountDataGridViewTextBoxColumn.FilterPropertyName = ""
        Me.AccountDataGridViewTextBoxColumn.HeaderText = "Sąskaita"
        Me.AccountDataGridViewTextBoxColumn.InstantBinding = True
        Me.AccountDataGridViewTextBoxColumn.Name = "AccountDataGridViewTextBoxColumn"
        Me.AccountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AccountDataGridViewTextBoxColumn.ValueMember = ""
        Me.AccountDataGridViewTextBoxColumn.Width = 81
        '
        'AmmountDataGridViewTextBoxColumn
        '
        Me.AmmountDataGridViewTextBoxColumn.AllowNegative = False
        Me.AmmountDataGridViewTextBoxColumn.DataPropertyName = "Ammount"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.Format = "##,0.00"
        Me.AmmountDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle6
        Me.AmmountDataGridViewTextBoxColumn.HeaderText = "Suma"
        Me.AmmountDataGridViewTextBoxColumn.MaxInputLength = 20
        Me.AmmountDataGridViewTextBoxColumn.Name = "AmmountDataGridViewTextBoxColumn"
        Me.AmmountDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AmmountDataGridViewTextBoxColumn.Width = 63
        '
        'PersonDataGridViewTextBoxColumn
        '
        Me.PersonDataGridViewTextBoxColumn.CloseOnSingleClick = True
        Me.PersonDataGridViewTextBoxColumn.ComboDataGridView = Nothing
        Me.PersonDataGridViewTextBoxColumn.DataPropertyName = "Person"
        Me.PersonDataGridViewTextBoxColumn.EmptyValueString = ""
        Me.PersonDataGridViewTextBoxColumn.FilterPropertyName = ""
        Me.PersonDataGridViewTextBoxColumn.HeaderText = "Kontrahentas"
        Me.PersonDataGridViewTextBoxColumn.InstantBinding = True
        Me.PersonDataGridViewTextBoxColumn.Name = "PersonDataGridViewTextBoxColumn"
        Me.PersonDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.PersonDataGridViewTextBoxColumn.ValueMember = ""
        Me.PersonDataGridViewTextBoxColumn.Width = 107
        '
        'AnalyticsListSortedBindingSource
        '
        Me.AnalyticsListSortedBindingSource.DataMember = "AnalyticsListSorted"
        Me.AnalyticsListSortedBindingSource.DataSource = Me.TransferOfBalanceBindingSource
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ErrorProvider1.ContainerControl = Me
        Me.ErrorProvider1.DataSource = Me.TransferOfBalanceBindingSource
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn3.CloseOnSingleClick = True
        Me.DataGridViewTextBoxColumn3.ComboDataGridView = Nothing
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "Account"
        Me.DataGridViewTextBoxColumn3.EmptyValueString = ""
        Me.DataGridViewTextBoxColumn3.FilterPropertyName = ""
        Me.DataGridViewTextBoxColumn3.HeaderText = "Sąskaita"
        Me.DataGridViewTextBoxColumn3.InstantBinding = True
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn3.ValueMember = ""
        Me.DataGridViewTextBoxColumn3.Width = 62
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.AllowNegative = False
        Me.DataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "Amount"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.Format = "##,0.00"
        DataGridViewCellStyle2.NullValue = "0.00"
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn4.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn4.HeaderText = "Suma"
        Me.DataGridViewTextBoxColumn4.MaxInputLength = 20
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumn8.CloseOnSingleClick = True
        Me.DataGridViewTextBoxColumn8.ComboDataGridView = Nothing
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "Account"
        Me.DataGridViewTextBoxColumn8.EmptyValueString = ""
        Me.DataGridViewTextBoxColumn8.FilterPropertyName = ""
        Me.DataGridViewTextBoxColumn8.HeaderText = "Sąskaita"
        Me.DataGridViewTextBoxColumn8.InstantBinding = True
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn8.ValueMember = ""
        Me.DataGridViewTextBoxColumn8.Width = 62
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.AllowNegative = False
        Me.DataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "Amount"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Format = "##,0.00"
        DataGridViewCellStyle4.NullValue = "0.00"
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumn9.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewTextBoxColumn9.HeaderText = "Suma"
        Me.DataGridViewTextBoxColumn9.MaxInputLength = 20
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'F_TransferOfBalance
        '
        Me.ReadWriteAuthorization1.SetApplyAuthorization(Me, False)
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(723, 539)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "F_TransferOfBalance"
        Me.Text = "Likučių perkėlimas"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.DebetListDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DebetListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TransferOfBalanceBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CreditListDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CreditListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.AnalyticListDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AnalyticsListSortedBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents nOkButton As System.Windows.Forms.Button
    Friend WithEvents ApplyButton As System.Windows.Forms.Button
    Friend WithEvents nCancelButton As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents TransferOfBalanceBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DateDateTimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents DebetListDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents DebetListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CreditListDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents CreditListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents AnalyticListDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents ReadWriteAuthorization1 As Csla.Windows.ReadWriteAuthorization
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents DataGridViewTextBoxColumn14 As AccControls.DataGridViewAccBoxColumn
    Friend WithEvents AnalyticsListSortedBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents EntryTypeHumanReadableDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents AccountDataGridViewTextBoxColumn As AccControls.DataGridViewAccGridComboBoxColumn
    Friend WithEvents AmmountDataGridViewTextBoxColumn As AccControls.DataGridViewAccTextBoxColumn
    Friend WithEvents PersonDataGridViewTextBoxColumn As AccControls.DataGridViewAccGridComboBoxColumn
    Friend WithEvents CreditSumAccTextBox As AccControls.AccTextBox
    Friend WithEvents DebetSumAccTextBox As AccControls.AccTextBox
    Friend WithEvents LimitationsButton As System.Windows.Forms.Button
    Friend WithEvents UpdateDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents InsertDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents IDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents PasteAccButton As AccControls.AccButton
    Friend WithEvents DataGridViewTextBoxColumn3 As AccControls.DataGridViewAccGridComboBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As AccControls.DataGridViewAccTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As AccControls.DataGridViewAccGridComboBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As AccControls.DataGridViewAccTextBoxColumn
End Class
