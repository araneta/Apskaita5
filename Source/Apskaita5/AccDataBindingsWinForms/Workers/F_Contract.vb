Imports ApskaitaObjects.Workers
Imports AccControlsWinForms
Imports ApskaitaObjects.Settings
Imports AccDataBindingsWinForms.CachedInfoLists
Imports AccDataBindingsWinForms.Printing

Friend Class F_Contract
    Implements ISupportsPrinting, IObjectEditForm, ISupportsChronologicValidator

    Private ReadOnly _RequiredCachedLists As Type() = New Type() _
        {GetType(HelperLists.DocumentSerialInfoList)}

    Private WithEvents _FormManager As CslaActionExtenderEditForm(Of Contract)
    Private _QueryManager As CslaActionExtenderQueryObject
    Private _ContractToEdit As Contract = Nothing


    Public ReadOnly Property ObjectID() As Integer Implements IObjectEditForm.ObjectID
        Get
            If _FormManager Is Nothing OrElse _FormManager.DataSource Is Nothing Then
                If _ContractToEdit Is Nothing OrElse _ContractToEdit.IsNew Then
                    Return Integer.MinValue
                Else
                    Return _ContractToEdit.ID
                End If
            ElseIf _FormManager.DataSource.IsNew Then
                Return Integer.MinValue
            End If
            Return _FormManager.DataSource.ID
        End Get
    End Property

    Public ReadOnly Property ObjectType() As System.Type Implements IObjectEditForm.ObjectType
        Get
            Return GetType(Contract)
        End Get
    End Property


    Public Sub New(ByVal contractToEdit As Contract)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        _ContractToEdit = contractToEdit

    End Sub


    Private Sub F_LabourContract_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If _ContractToEdit Is Nothing Then
            MsgBox("Klaida. Nenurodyta darbo sutartis.", MsgBoxStyle.Exclamation, "Klaida")
            DisableAllControls(Me)
            Exit Sub
        End If

        If Not SetDataSources() Then Exit Sub

        Try
            _FormManager = New CslaActionExtenderEditForm(Of Contract)(Me, _
                ContractBindingSource, _ContractToEdit, _RequiredCachedLists, _
                IOkButton, IApplyButton, ICancelButton, Nothing, ProgressFiller1)
        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Exit Sub
        End Try

        ConfigureButtons()

    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, _RequiredCachedLists) Then Return False

        Try

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            SetupDefaultControls(Of Contract)(Me, ContractBindingSource, _ContractToEdit)

        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function


    Private Sub RefreshNumberButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles RefreshNumberButton.Click

        If _FormManager.DataSource Is Nothing OrElse Not _FormManager.DataSource.IsNew Then Exit Sub

        '        Obj.Number = Settings.CommandLastDocumentNumber.TheCommand( _
        '            Settings.DocumentSerialType.LabourContract, Obj.Serial.Trim, Obj.Date, False) + 1
        _QueryManager.InvokeQuery(Of CommandLastDocumentNumber)(Nothing, _
            "TheCommand", True, AddressOf OnContractNumberFetched, DocumentSerialType.LabourContract, _
            _FormManager.DataSource.Serial.Trim, _FormManager.DataSource.Date, False)

    End Sub

    Private Sub OnContractNumberFetched(ByVal result As Object, ByVal exceptionHandled As Boolean)
        If result Is Nothing Then Exit Sub
        Try
            _FormManager.DataSource.Number = DirectCast(result, Integer) + 1
        Catch ex As Exception
        End Try
    End Sub

    Private Sub IsTerminatedCheckBox_CheckedChanged(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles IsTerminatedCheckBox.CheckedChanged
        TerminationDateAccDatePicker.ReadOnly = (_FormManager.DataSource Is Nothing _
            OrElse Not DirectCast(_FormManager.DataSource.ChronologicValidator, ContractChronologicValidator). _
            TerminationCanBeCanceled OrElse Not IsTerminatedCheckBox.Checked)
    End Sub


    Public Function GetMailDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetMailDropDownItems
        Return Nothing
    End Function

    Public Function GetPrintDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetPrintDropDownItems
        Return Nothing
    End Function

    Public Function GetPrintPreviewDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetPrintPreviewDropDownItems
        Return Nothing
    End Function

    Public Sub OnMailClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnMailClick
        If _FormManager.DataSource Is Nothing Then Exit Sub

        Using frm As New F_SendObjToEmail(_FormManager.DataSource, 0)
            frm.ShowDialog()
        End Using

    End Sub

    Public Sub OnPrintClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintClick
        If _FormManager.DataSource Is Nothing Then Exit Sub
        Try
            PrintObject(_FormManager.DataSource, False, 0, "", Me, "")
        Catch ex As Exception
            ShowError(ex, _FormManager.DataSource)
        End Try
    End Sub

    Public Sub OnPrintPreviewClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintPreviewClick
        If _FormManager.DataSource Is Nothing Then Exit Sub
        Try
            PrintObject(_FormManager.DataSource, True, 0, "", Me, "")
        Catch ex As Exception
            ShowError(ex, _FormManager.DataSource)
        End Try
    End Sub

    Public Function SupportsEmailing() As Boolean _
        Implements ISupportsPrinting.SupportsEmailing
        Return True
    End Function


    Public Function ChronologicContent() As String _
            Implements ISupportsChronologicValidator.ChronologicContent
        If _FormManager.DataSource Is Nothing Then Return ""
        Return _FormManager.DataSource.ChronologicValidator.LimitsExplanation
    End Function

    Public Function HasChronologicContent() As Boolean _
        Implements ISupportsChronologicValidator.HasChronologicContent

        Return Not _FormManager.DataSource Is Nothing AndAlso _
            Not StringIsNullOrEmpty(_FormManager.DataSource.ChronologicValidator.LimitsExplanation)

    End Function


    Private Sub _FormManager_DataSourceStateHasChanged(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles _FormManager.DataSourceStateHasChanged
        ConfigureButtons()
    End Sub

    Private Sub ConfigureButtons()

        If _FormManager.DataSource Is Nothing Then Exit Sub

        SerialComboBox.Enabled = _FormManager.DataSource.IsNew
        NumberAccTextBox.ReadOnly = Not _FormManager.DataSource.IsNew
        RefreshNumberButton.Enabled = _FormManager.DataSource.IsNew
        WageAccTextBox.ReadOnly = Not _FormManager.DataSource.ChronologicValidator.FinancialDataCanChange
        HumanReadableWageTypeComboBox.Enabled = _FormManager.DataSource.ChronologicValidator.FinancialDataCanChange
        ExtraPayAccTextBox.ReadOnly = Not _FormManager.DataSource.ChronologicValidator.FinancialDataCanChange
        NPDAccTextBox.ReadOnly = Not _FormManager.DataSource.ChronologicValidator.FinancialDataCanChange
        PNPDAccTextBox.ReadOnly = Not _FormManager.DataSource.ChronologicValidator.FinancialDataCanChange
        IsTerminatedCheckBox.Enabled = DirectCast(_FormManager.DataSource.ChronologicValidator, ContractChronologicValidator).TerminationCanBeCanceled
        TerminationDateAccDatePicker.ReadOnly = Not DirectCast(_FormManager.DataSource.ChronologicValidator, ContractChronologicValidator).TerminationCanBeCanceled OrElse Not IsTerminatedCheckBox.Checked

    End Sub

End Class