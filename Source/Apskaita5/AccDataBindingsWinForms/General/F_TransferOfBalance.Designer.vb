<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Friend Class F_TransferOfBalance
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
        Me.components = New System.ComponentModel.Container()
        Dim CreditSumLabel As System.Windows.Forms.Label
        Dim DateLabel As System.Windows.Forms.Label
        Dim DebetSumLabel As System.Windows.Forms.Label
        Dim IDLabel As System.Windows.Forms.Label
        Dim InsertDateLabel As System.Windows.Forms.Label
        Dim UpdateDateLabel As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(F_TransferOfBalance))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.DebetListDataListView = New BrightIdeasSoftware.DataListView()
        Me.OlvColumn1 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.OlvColumn2 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.DebetListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.TransferOfBalanceBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.CreditListDataListView = New BrightIdeasSoftware.DataListView()
        Me.OlvColumn3 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.OlvColumn4 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.CreditListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PasteAccButton = New AccControlsWinForms.AccButton()
        Me.nCancelButton = New System.Windows.Forms.Button()
        Me.ApplyButton = New System.Windows.Forms.Button()
        Me.nOkButton = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.UpdateDateTextBox = New System.Windows.Forms.TextBox()
        Me.CreditSumAccTextBox = New AccControlsWinForms.AccTextBox()
        Me.DebetSumAccTextBox = New AccControlsWinForms.AccTextBox()
        Me.IDTextBox = New System.Windows.Forms.TextBox()
        Me.InsertDateTextBox = New System.Windows.Forms.TextBox()
        Me.DateAccDatePicker = New AccControlsWinForms.AccDatePicker()
        Me.AnalyticListDataListView = New BrightIdeasSoftware.DataListView()
        Me.OlvColumn5 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.OlvColumn6 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.OlvColumn7 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.OlvColumn8 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.AnalyticsListSortedBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ProgressFiller1 = New AccControlsWinForms.ProgressFiller()
        Me.ProgressFiller2 = New AccControlsWinForms.ProgressFiller()
        Me.ErrorWarnInfoProvider1 = New AccControlsWinForms.ErrorWarnInfoProvider(Me.components)
        CreditSumLabel = New System.Windows.Forms.Label()
        DateLabel = New System.Windows.Forms.Label()
        DebetSumLabel = New System.Windows.Forms.Label()
        IDLabel = New System.Windows.Forms.Label()
        InsertDateLabel = New System.Windows.Forms.Label()
        UpdateDateLabel = New System.Windows.Forms.Label()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.DebetListDataListView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DebetListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TransferOfBalanceBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CreditListDataListView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CreditListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.AnalyticListDataListView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AnalyticsListSortedBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorWarnInfoProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CreditSumLabel
        '
        CreditSumLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.AnalyticListDataListView)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(5)
        Me.SplitContainer1.Size = New System.Drawing.Size(723, 539)
        Me.SplitContainer1.SplitterDistance = 307
        Me.SplitContainer1.TabIndex = 0
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.IsSplitterFixed = True
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 65)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.AutoScroll = True
        Me.SplitContainer2.Panel1.Controls.Add(Me.DebetListDataListView)
        Me.SplitContainer2.Panel1.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.AutoScroll = True
        Me.SplitContainer2.Panel2.Controls.Add(Me.CreditListDataListView)
        Me.SplitContainer2.Panel2.Padding = New System.Windows.Forms.Padding(0, 0, 5, 0)
        Me.SplitContainer2.Size = New System.Drawing.Size(620, 242)
        Me.SplitContainer2.SplitterDistance = 307
        Me.SplitContainer2.SplitterWidth = 10
        Me.SplitContainer2.TabIndex = 2
        '
        'DebetListDataListView
        '
        Me.DebetListDataListView.AllColumns.Add(Me.OlvColumn1)
        Me.DebetListDataListView.AllColumns.Add(Me.OlvColumn2)
        Me.DebetListDataListView.AllowColumnReorder = True
        Me.DebetListDataListView.AutoGenerateColumns = False
        Me.DebetListDataListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways
        Me.DebetListDataListView.CellEditEnterChangesRows = True
        Me.DebetListDataListView.CellEditTabChangesRows = True
        Me.DebetListDataListView.CellEditUseWholeCell = False
        Me.DebetListDataListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.OlvColumn1, Me.OlvColumn2})
        Me.DebetListDataListView.Cursor = System.Windows.Forms.Cursors.Default
        Me.DebetListDataListView.DataSource = Me.DebetListBindingSource
        Me.DebetListDataListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DebetListDataListView.FullRowSelect = True
        Me.DebetListDataListView.HasCollapsibleGroups = False
        Me.DebetListDataListView.HeaderWordWrap = True
        Me.DebetListDataListView.HideSelection = False
        Me.DebetListDataListView.HighlightBackgroundColor = System.Drawing.Color.PaleGreen
        Me.DebetListDataListView.HighlightForegroundColor = System.Drawing.Color.Black
        Me.DebetListDataListView.IncludeColumnHeadersInCopy = True
        Me.DebetListDataListView.Location = New System.Drawing.Point(5, 0)
        Me.DebetListDataListView.Name = "DebetListDataListView"
        Me.DebetListDataListView.RenderNonEditableCheckboxesAsDisabled = True
        Me.DebetListDataListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu
        Me.DebetListDataListView.SelectedBackColor = System.Drawing.Color.PaleGreen
        Me.DebetListDataListView.SelectedForeColor = System.Drawing.Color.Black
        Me.DebetListDataListView.ShowCommandMenuOnRightClick = True
        Me.DebetListDataListView.ShowGroups = False
        Me.DebetListDataListView.ShowImagesOnSubItems = True
        Me.DebetListDataListView.ShowItemCountOnGroups = True
        Me.DebetListDataListView.ShowItemToolTips = True
        Me.DebetListDataListView.Size = New System.Drawing.Size(302, 242)
        Me.DebetListDataListView.TabIndex = 3
        Me.DebetListDataListView.UnfocusedSelectedBackColor = System.Drawing.Color.PaleGreen
        Me.DebetListDataListView.UnfocusedSelectedForeColor = System.Drawing.Color.Black
        Me.DebetListDataListView.UseCellFormatEvents = True
        Me.DebetListDataListView.UseCompatibleStateImageBehavior = False
        Me.DebetListDataListView.UseFilterIndicator = True
        Me.DebetListDataListView.UseFiltering = True
        Me.DebetListDataListView.UseHotItem = True
        Me.DebetListDataListView.UseNotifyPropertyChanged = True
        Me.DebetListDataListView.View = System.Windows.Forms.View.Details
        '
        'OlvColumn1
        '
        Me.OlvColumn1.AspectName = "Account"
        Me.OlvColumn1.CellEditUseWholeCell = True
        Me.OlvColumn1.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn1.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn1.Text = "Sąskaita"
        Me.OlvColumn1.ToolTipText = ""
        Me.OlvColumn1.Width = 119
        '
        'OlvColumn2
        '
        Me.OlvColumn2.AspectName = "Amount"
        Me.OlvColumn2.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn2.CellEditUseWholeCell = True
        Me.OlvColumn2.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn2.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn2.Text = "Suma"
        Me.OlvColumn2.ToolTipText = ""
        Me.OlvColumn2.Width = 218
        '
        'DebetListBindingSource
        '
        Me.DebetListBindingSource.DataMember = "DebetList"
        Me.DebetListBindingSource.DataSource = Me.TransferOfBalanceBindingSource
        '
        'TransferOfBalanceBindingSource
        '
        Me.TransferOfBalanceBindingSource.DataSource = GetType(ApskaitaObjects.General.TransferOfBalance)
        '
        'CreditListDataListView
        '
        Me.CreditListDataListView.AllColumns.Add(Me.OlvColumn3)
        Me.CreditListDataListView.AllColumns.Add(Me.OlvColumn4)
        Me.CreditListDataListView.AllowColumnReorder = True
        Me.CreditListDataListView.AutoGenerateColumns = False
        Me.CreditListDataListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways
        Me.CreditListDataListView.CellEditEnterChangesRows = True
        Me.CreditListDataListView.CellEditTabChangesRows = True
        Me.CreditListDataListView.CellEditUseWholeCell = False
        Me.CreditListDataListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.OlvColumn3, Me.OlvColumn4})
        Me.CreditListDataListView.Cursor = System.Windows.Forms.Cursors.Default
        Me.CreditListDataListView.DataSource = Me.CreditListBindingSource
        Me.CreditListDataListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CreditListDataListView.FullRowSelect = True
        Me.CreditListDataListView.HasCollapsibleGroups = False
        Me.CreditListDataListView.HeaderWordWrap = True
        Me.CreditListDataListView.HideSelection = False
        Me.CreditListDataListView.HighlightBackgroundColor = System.Drawing.Color.PaleGreen
        Me.CreditListDataListView.HighlightForegroundColor = System.Drawing.Color.Black
        Me.CreditListDataListView.IncludeColumnHeadersInCopy = True
        Me.CreditListDataListView.Location = New System.Drawing.Point(0, 0)
        Me.CreditListDataListView.Name = "CreditListDataListView"
        Me.CreditListDataListView.RenderNonEditableCheckboxesAsDisabled = True
        Me.CreditListDataListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu
        Me.CreditListDataListView.SelectedBackColor = System.Drawing.Color.PaleGreen
        Me.CreditListDataListView.SelectedForeColor = System.Drawing.Color.Black
        Me.CreditListDataListView.ShowCommandMenuOnRightClick = True
        Me.CreditListDataListView.ShowGroups = False
        Me.CreditListDataListView.ShowImagesOnSubItems = True
        Me.CreditListDataListView.ShowItemCountOnGroups = True
        Me.CreditListDataListView.ShowItemToolTips = True
        Me.CreditListDataListView.Size = New System.Drawing.Size(298, 242)
        Me.CreditListDataListView.TabIndex = 3
        Me.CreditListDataListView.UnfocusedSelectedBackColor = System.Drawing.Color.PaleGreen
        Me.CreditListDataListView.UnfocusedSelectedForeColor = System.Drawing.Color.Black
        Me.CreditListDataListView.UseCellFormatEvents = True
        Me.CreditListDataListView.UseCompatibleStateImageBehavior = False
        Me.CreditListDataListView.UseFilterIndicator = True
        Me.CreditListDataListView.UseFiltering = True
        Me.CreditListDataListView.UseHotItem = True
        Me.CreditListDataListView.UseNotifyPropertyChanged = True
        Me.CreditListDataListView.View = System.Windows.Forms.View.Details
        '
        'OlvColumn3
        '
        Me.OlvColumn3.AspectName = "Account"
        Me.OlvColumn3.CellEditUseWholeCell = True
        Me.OlvColumn3.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn3.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn3.Text = "Sąskaita"
        Me.OlvColumn3.ToolTipText = ""
        Me.OlvColumn3.Width = 124
        '
        'OlvColumn4
        '
        Me.OlvColumn4.AspectName = "Amount"
        Me.OlvColumn4.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn4.CellEditUseWholeCell = True
        Me.OlvColumn4.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn4.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn4.Text = "Suma"
        Me.OlvColumn4.ToolTipText = ""
        Me.OlvColumn4.Width = 214
        '
        'CreditListBindingSource
        '
        Me.CreditListBindingSource.DataMember = "CreditList"
        Me.CreditListBindingSource.DataSource = Me.TransferOfBalanceBindingSource
        '
        'Panel1
        '
        Me.Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel1.Controls.Add(Me.PasteAccButton)
        Me.Panel1.Controls.Add(Me.nCancelButton)
        Me.Panel1.Controls.Add(Me.ApplyButton)
        Me.Panel1.Controls.Add(Me.nOkButton)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel1.Location = New System.Drawing.Point(620, 65)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(103, 242)
        Me.Panel1.TabIndex = 2
        '
        'PasteAccButton
        '
        Me.PasteAccButton.BorderStyleDown = System.Windows.Forms.Border3DStyle.Sunken
        Me.PasteAccButton.BorderStyleNormal = System.Windows.Forms.Border3DStyle.Raised
        Me.PasteAccButton.BorderStyleUp = System.Windows.Forms.Border3DStyle.Raised
        Me.PasteAccButton.ButtonStyle = AccControlsWinForms.rsButtonStyle.DropDownWithSep
        Me.PasteAccButton.Checked = False
        Me.PasteAccButton.DropDownSepWidth = 12
        Me.PasteAccButton.FocusRectangle = False
        Me.PasteAccButton.Image = Global.AccDataBindingsWinForms.My.Resources.Resources.Paste_icon_24p
        Me.PasteAccButton.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.PasteAccButton.ImagePadding = 2
        Me.PasteAccButton.Location = New System.Drawing.Point(35, 107)
        Me.PasteAccButton.Name = "PasteAccButton"
        Me.PasteAccButton.Size = New System.Drawing.Size(45, 30)
        Me.PasteAccButton.TabIndex = 71
        Me.PasteAccButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.PasteAccButton.TextPadding = 2
        '
        'nCancelButton
        '
        Me.nCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.Panel2.AutoScroll = True
        Me.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Padding = New System.Windows.Forms.Padding(0, 0, 0, 5)
        Me.Panel2.Size = New System.Drawing.Size(723, 65)
        Me.Panel2.TabIndex = 1
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoScroll = True
        Me.TableLayoutPanel1.ColumnCount = 9
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.UpdateDateTextBox, 7, 0)
        Me.TableLayoutPanel1.Controls.Add(DateLabel, 6, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.CreditSumAccTextBox, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(UpdateDateLabel, 6, 0)
        Me.TableLayoutPanel1.Controls.Add(CreditSumLabel, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.DebetSumAccTextBox, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(IDLabel, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.IDTextBox, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.InsertDateTextBox, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(InsertDateLabel, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(DebetSumLabel, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.DateAccDatePicker, 7, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(723, 60)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'UpdateDateTextBox
        '
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
        Me.CreditSumAccTextBox.ButtonVisible = False
        Me.CreditSumAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.TransferOfBalanceBindingSource, "CreditSum", True))
        Me.CreditSumAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CreditSumAccTextBox.Location = New System.Drawing.Point(324, 33)
        Me.CreditSumAccTextBox.Name = "CreditSumAccTextBox"
        Me.CreditSumAccTextBox.ReadOnly = True
        Me.CreditSumAccTextBox.Size = New System.Drawing.Size(142, 20)
        Me.CreditSumAccTextBox.TabIndex = 7
        Me.CreditSumAccTextBox.TabStop = False
        Me.CreditSumAccTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DebetSumAccTextBox
        '
        Me.DebetSumAccTextBox.ButtonVisible = False
        Me.DebetSumAccTextBox.DataBindings.Add(New System.Windows.Forms.Binding("DecimalValue", Me.TransferOfBalanceBindingSource, "DebetSum", True))
        Me.DebetSumAccTextBox.Dock = System.Windows.Forms.DockStyle.Fill
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
        'DateAccDatePicker
        '
        Me.DateAccDatePicker.BoldedDates = Nothing
        Me.DateAccDatePicker.DataBindings.Add(New System.Windows.Forms.Binding("Value", Me.TransferOfBalanceBindingSource, "Date", True))
        Me.DateAccDatePicker.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DateAccDatePicker.Location = New System.Drawing.Point(558, 33)
        Me.DateAccDatePicker.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.DateAccDatePicker.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.DateAccDatePicker.Name = "DateAccDatePicker"
        Me.DateAccDatePicker.ShowWeekNumbers = True
        Me.DateAccDatePicker.Size = New System.Drawing.Size(142, 20)
        Me.DateAccDatePicker.TabIndex = 0
        '
        'AnalyticListDataListView
        '
        Me.AnalyticListDataListView.AllColumns.Add(Me.OlvColumn5)
        Me.AnalyticListDataListView.AllColumns.Add(Me.OlvColumn6)
        Me.AnalyticListDataListView.AllColumns.Add(Me.OlvColumn7)
        Me.AnalyticListDataListView.AllColumns.Add(Me.OlvColumn8)
        Me.AnalyticListDataListView.AllowColumnReorder = True
        Me.AnalyticListDataListView.AutoGenerateColumns = False
        Me.AnalyticListDataListView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways
        Me.AnalyticListDataListView.CellEditEnterChangesRows = True
        Me.AnalyticListDataListView.CellEditTabChangesRows = True
        Me.AnalyticListDataListView.CellEditUseWholeCell = False
        Me.AnalyticListDataListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.OlvColumn5, Me.OlvColumn6, Me.OlvColumn7, Me.OlvColumn8})
        Me.AnalyticListDataListView.Cursor = System.Windows.Forms.Cursors.Default
        Me.AnalyticListDataListView.DataSource = Me.AnalyticsListSortedBindingSource
        Me.AnalyticListDataListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AnalyticListDataListView.FullRowSelect = True
        Me.AnalyticListDataListView.HasCollapsibleGroups = False
        Me.AnalyticListDataListView.HeaderWordWrap = True
        Me.AnalyticListDataListView.HideSelection = False
        Me.AnalyticListDataListView.HighlightBackgroundColor = System.Drawing.Color.PaleGreen
        Me.AnalyticListDataListView.HighlightForegroundColor = System.Drawing.Color.Black
        Me.AnalyticListDataListView.IncludeColumnHeadersInCopy = True
        Me.AnalyticListDataListView.Location = New System.Drawing.Point(5, 5)
        Me.AnalyticListDataListView.Name = "AnalyticListDataListView"
        Me.AnalyticListDataListView.RenderNonEditableCheckboxesAsDisabled = True
        Me.AnalyticListDataListView.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu
        Me.AnalyticListDataListView.SelectedBackColor = System.Drawing.Color.PaleGreen
        Me.AnalyticListDataListView.SelectedForeColor = System.Drawing.Color.Black
        Me.AnalyticListDataListView.ShowCommandMenuOnRightClick = True
        Me.AnalyticListDataListView.ShowGroups = False
        Me.AnalyticListDataListView.ShowImagesOnSubItems = True
        Me.AnalyticListDataListView.ShowItemCountOnGroups = True
        Me.AnalyticListDataListView.ShowItemToolTips = True
        Me.AnalyticListDataListView.Size = New System.Drawing.Size(713, 218)
        Me.AnalyticListDataListView.TabIndex = 3
        Me.AnalyticListDataListView.UnfocusedSelectedBackColor = System.Drawing.Color.PaleGreen
        Me.AnalyticListDataListView.UnfocusedSelectedForeColor = System.Drawing.Color.Black
        Me.AnalyticListDataListView.UseCellFormatEvents = True
        Me.AnalyticListDataListView.UseCompatibleStateImageBehavior = False
        Me.AnalyticListDataListView.UseFilterIndicator = True
        Me.AnalyticListDataListView.UseFiltering = True
        Me.AnalyticListDataListView.UseHotItem = True
        Me.AnalyticListDataListView.UseNotifyPropertyChanged = True
        Me.AnalyticListDataListView.View = System.Windows.Forms.View.Details
        '
        'OlvColumn5
        '
        Me.OlvColumn5.AspectName = "EntryTypeHumanReadable"
        Me.OlvColumn5.CellEditUseWholeCell = True
        Me.OlvColumn5.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn5.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn5.Text = "Tipas"
        Me.OlvColumn5.ToolTipText = ""
        Me.OlvColumn5.Width = 70
        '
        'OlvColumn6
        '
        Me.OlvColumn6.AspectName = "Account"
        Me.OlvColumn6.CellEditUseWholeCell = True
        Me.OlvColumn6.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn6.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn6.Text = "Sąskaita"
        Me.OlvColumn6.ToolTipText = ""
        Me.OlvColumn6.Width = 119
        '
        'OlvColumn7
        '
        Me.OlvColumn7.AspectName = "Ammount"
        Me.OlvColumn7.AspectToStringFormat = "{0:##,0.00}"
        Me.OlvColumn7.CellEditUseWholeCell = True
        Me.OlvColumn7.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn7.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn7.Text = "Suma"
        Me.OlvColumn7.ToolTipText = ""
        Me.OlvColumn7.Width = 77
        '
        'OlvColumn8
        '
        Me.OlvColumn8.AspectName = "Person"
        Me.OlvColumn8.CellEditUseWholeCell = True
        Me.OlvColumn8.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OlvColumn8.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.OlvColumn8.Text = "Kontrahentas"
        Me.OlvColumn8.ToolTipText = ""
        Me.OlvColumn8.Width = 423
        '
        'AnalyticsListSortedBindingSource
        '
        Me.AnalyticsListSortedBindingSource.DataMember = "AnalyticsList"
        Me.AnalyticsListSortedBindingSource.DataSource = Me.TransferOfBalanceBindingSource
        '
        'ProgressFiller1
        '
        Me.ProgressFiller1.Location = New System.Drawing.Point(149, 41)
        Me.ProgressFiller1.Name = "ProgressFiller1"
        Me.ProgressFiller1.Size = New System.Drawing.Size(177, 81)
        Me.ProgressFiller1.TabIndex = 1
        Me.ProgressFiller1.Visible = False
        '
        'ProgressFiller2
        '
        Me.ProgressFiller2.Location = New System.Drawing.Point(342, 42)
        Me.ProgressFiller2.Name = "ProgressFiller2"
        Me.ProgressFiller2.Size = New System.Drawing.Size(156, 80)
        Me.ProgressFiller2.TabIndex = 2
        Me.ProgressFiller2.Visible = False
        '
        'ErrorWarnInfoProvider1
        '
        Me.ErrorWarnInfoProvider1.ContainerControl = Me
        Me.ErrorWarnInfoProvider1.DataSource = Me.TransferOfBalanceBindingSource
        '
        'F_TransferOfBalance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(723, 539)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.ProgressFiller2)
        Me.Controls.Add(Me.ProgressFiller1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "F_TransferOfBalance"
        Me.Text = "Likučių perkėlimas"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.DebetListDataListView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DebetListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TransferOfBalanceBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CreditListDataListView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CreditListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.AnalyticListDataListView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AnalyticsListSortedBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorWarnInfoProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents nOkButton As System.Windows.Forms.Button
    Friend WithEvents ApplyButton As System.Windows.Forms.Button
    Friend WithEvents nCancelButton As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents TransferOfBalanceBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents DebetListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CreditListBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DataGridViewTextBoxColumn14 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AnalyticsListSortedBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CreditSumAccTextBox As AccControlsWinForms.AccTextBox
    Friend WithEvents DebetSumAccTextBox As AccControlsWinForms.AccTextBox
    Friend WithEvents UpdateDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents InsertDateTextBox As System.Windows.Forms.TextBox
    Friend WithEvents IDTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents PasteAccButton As AccControlsWinForms.AccButton
    Friend WithEvents DebetListDataListView As BrightIdeasSoftware.DataListView
    Friend WithEvents CreditListDataListView As BrightIdeasSoftware.DataListView
    Friend WithEvents AnalyticListDataListView As BrightIdeasSoftware.DataListView
    Friend WithEvents OlvColumn1 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn2 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn3 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn4 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn5 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn6 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn7 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn8 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents ProgressFiller2 As AccControlsWinForms.ProgressFiller
    Friend WithEvents ProgressFiller1 As AccControlsWinForms.ProgressFiller
    Friend WithEvents ErrorWarnInfoProvider1 As AccControlsWinForms.ErrorWarnInfoProvider
    Friend WithEvents DateAccDatePicker As AccControlsWinForms.AccDatePicker
End Class
