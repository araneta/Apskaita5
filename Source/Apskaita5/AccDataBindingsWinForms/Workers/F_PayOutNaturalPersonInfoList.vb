Imports ApskaitaObjects.Workers
Imports ApskaitaObjects.ActiveReports
Imports AccControlsWinForms
Imports AccDataBindingsWinForms.Printing

Friend Class F_PayOutNaturalPersonInfoList
    Implements ISupportsPrinting

    Private _FormManager As CslaActionExtenderReportForm(Of PayOutNaturalPersonInfoList)
    Private _ListViewManager As DataListViewEditControlManager(Of PayOutNaturalPersonInfo)
    Private _QueryManager As CslaActionExtenderQueryObject


    Private Sub F_PayOutNaturalPersonList_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If Not SetDataSources() Then Exit Sub

    End Sub

    Private Function SetDataSources() As Boolean

        Try

            _ListViewManager = New DataListViewEditControlManager(Of PayOutNaturalPersonInfo) _
                (PayOutNaturalPersonItemListDataListView, ContextMenuStrip1, Nothing, _
                 Nothing, Nothing, Nothing)

            _ListViewManager.AddCancelButton = True
            _ListViewManager.AddButtonHandler("Keisti", "Keisti išmokos fiziniam asmeniui duomenis.", _
                AddressOf ChangeItem)
            _ListViewManager.AddButtonHandler("Ištrinti", "Pašalinti išmokos fiziniam asmeniui duomenis iš duomenų bazės.", _
                AddressOf DeleteItem)

            _ListViewManager.AddMenuItemHandler(ChangeItem_MenuItem, AddressOf ChangeItem)
            _ListViewManager.AddMenuItemHandler(DeleteItem_MenuItem, AddressOf DeleteItem)
            _ListViewManager.AddMenuItemHandler(NewItem_MenuItem, AddressOf NewItem)

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            ' PayOutNaturalPersonInfoList.GetPayOutNaturalPersonInfoList(dateFrom, dateTo)
            _FormManager = New CslaActionExtenderReportForm(Of PayOutNaturalPersonInfoList) _
                (Me, PayOutNaturalPersonItemListBindingSource, Nothing, Nothing, RefreshButton, _
                 ProgressFiller1, "GetPayOutNaturalPersonInfoList", AddressOf GetReportParams)

            _FormManager.ManageDataListViewStates(PayOutNaturalPersonItemListDataListView)

        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Return False
        End Try

        RefreshButton.Enabled = PayOutNaturalPersonInfoList.CanGetObject
        NewItemButton.Enabled = PayOutNaturalPerson.CanAddObject

        DateFromAccDatePicker.Value = Today.AddMonths(-3)

        Return True

    End Function


    Private Function GetReportParams() As Object()
        ' PayOutNaturalPersonInfoList.GetPayOutNaturalPersonInfoList(DateFromDateTimePicker.Value, DateToDateTimePicker.Value)
        Return New Object() {DateFromAccDatePicker.Value, DateToAccDatePicker.Value}
    End Function

    Private Sub NewItemButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles NewItemButton.Click
        NewItem(Nothing)
    End Sub

    Private Sub ChangeItem(ByVal item As PayOutNaturalPersonInfo)
        If item Is Nothing Then Exit Sub
        ' PayOutNaturalPerson.GetPayOutNaturalPerson(item.ID)
        _QueryManager.InvokeQuery(Of PayOutNaturalPerson)(Nothing, "GetPayOutNaturalPerson", True, _
            AddressOf OpenObjectEditForm, item.ID)
    End Sub

    Private Sub DeleteItem(ByVal item As PayOutNaturalPersonInfo)

        If item Is Nothing Then Exit Sub

        If CheckIfObjectEditFormOpen(Of PayOutNaturalPerson)(item.ID, True, True) Then Exit Sub

        If Not YesOrNo("Ar tikrai norite pašalinti išmokos duomenis iš duomenų bazės?") Then Exit Sub

        ' PayOutNaturalPerson.DeletePayOutNaturalPerson(item.ID)
        _QueryManager.InvokeQuery(Of PayOutNaturalPerson)(Nothing, "DeletePayOutNaturalPerson", False, _
            AddressOf OnItemDeleted, item.ID)

    End Sub

    Private Sub OnItemDeleted(ByVal result As Object, ByVal exceptionHandled As Boolean)
        If exceptionHandled Then Exit Sub
        If Not YesOrNo("Išmokos duomenys sėkmingai pašalinti iš duomenų bazės. Atnaujinti sąrašą?") Then Exit Sub
        RefreshButton.PerformClick()
    End Sub

    Private Sub NewItem(ByVal item As PayOutNaturalPersonInfo)
        OpenNewForm(Of PayOutNaturalPerson)()
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
            PrintObject(_FormManager.DataSource, False, 0, "IsmokosFiziniams", Me, _
                _ListViewManager.GetCurrentFilterDescription(), _
                _ListViewManager.GetDisplayOrderIndexes())
        Catch ex As Exception
            ShowError(ex, _FormManager.DataSource)
        End Try
    End Sub

    Public Sub OnPrintPreviewClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintPreviewClick
        If _FormManager.DataSource Is Nothing Then Exit Sub
        Try
            PrintObject(_FormManager.DataSource, True, 0, "IsmokosFiziniams", Me, _
                _ListViewManager.GetCurrentFilterDescription(), _
                _ListViewManager.GetDisplayOrderIndexes())
        Catch ex As Exception
            ShowError(ex, _FormManager.DataSource)
        End Try
    End Sub

    Public Function SupportsEmailing() As Boolean _
        Implements ISupportsPrinting.SupportsEmailing
        Return True
    End Function

End Class