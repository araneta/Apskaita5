Imports ApskaitaObjects.Goods
Imports AccControlsWinForms
Imports AccDataBindingsWinForms.Printing
Imports AccDataBindingsWinForms.CachedInfoLists
Imports ApskaitaObjects.Attributes

Friend Class F_GoodsComplexOperationTransferOfBalance
    Implements ISingleInstanceForm, ISupportsPrinting, ISupportsChronologicValidator

    Private ReadOnly _RequiredCachedLists As Type() = New Type() _
        {GetType(HelperLists.GoodsInfoList), GetType(HelperLists.WarehouseInfoList)}

    Private WithEvents _FormManager As CslaActionExtenderEditForm(Of GoodsComplexOperationTransferOfBalance)
    Private _ListViewManager As DataListViewEditControlManager(Of GoodsTransferOfBalanceItem)
    Private _QueryManager As CslaActionExtenderQueryObject


    Private Sub F_GoodsComplexOperationTransferOfBalance_Load(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles Me.Load

        Try

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            'GoodsComplexOperationTransferOfBalance.GetGoodsComplexOperationTransferOfBalance()
            _QueryManager.InvokeQuery(Of GoodsComplexOperationTransferOfBalance)(Nothing, _
                "GetGoodsComplexOperationTransferOfBalance", True, AddressOf OnDataSourceFetched)

        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Exit Sub
        End Try

    End Sub

    Private Function SetDataSources(ByVal source As GoodsComplexOperationTransferOfBalance) As Boolean

        If Not PrepareCache(Me, _RequiredCachedLists) Then Return False

        Try

            _ListViewManager = New DataListViewEditControlManager(Of GoodsTransferOfBalanceItem) _
                (ItemsDataListView, Nothing, AddressOf OnItemsDelete,
                 Nothing, Nothing, source)

            SetupDefaultControls(Of GoodsComplexOperationTransferOfBalance) _
                (Me, GoodsComplexOperationTransferOfBalanceBindingSource, source)

            PrepareControl(WarehouseInfoListAccGridComboBox,
                New WarehouseFieldAttribute(ValueRequiredLevel.Optional))

        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function

    Private Sub OnDataSourceFetched(ByVal result As Object, ByVal exceptionHandled As Boolean)

        If exceptionHandled Then
            DisableAllControls(Me)
            Exit Sub
        ElseIf result Is Nothing Then
            MsgBox("Klaida. Dėl nežinomų priežasčių nepavyko gauti prekių likučių perkėlimo operacijos duomenų.", _
                MsgBoxStyle.Exclamation, "Klaida")
            DisableAllControls(Me)
            Exit Sub
        End If

        If Not SetDataSources(DirectCast(result, GoodsComplexOperationTransferOfBalance)) Then Exit Sub

        Try

            _FormManager = New CslaActionExtenderEditForm(Of GoodsComplexOperationTransferOfBalance) _
                (Me, GoodsComplexOperationTransferOfBalanceBindingSource, _
                DirectCast(result, GoodsComplexOperationTransferOfBalance), _
                _RequiredCachedLists, nOkButton, ApplyButton, nCancelButton, _
                Nothing, ProgressFiller1)

            _FormManager.ManageDataListViewStates(ItemsDataListView)

        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Exit Sub
        End Try

        ConfigureButtons()

    End Sub


    Private Sub OnItemsDelete(ByVal items As GoodsTransferOfBalanceItem())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As GoodsTransferOfBalanceItem In items
            If Not item.OperationLimitations.FinancialDataCanChange Then
                MsgBox(String.Format("Klaida. Eilutės {0} pašalinti neleidžiama:{1}{2}", _
                item.GoodsName, vbCrLf, item.OperationLimitations.FinancialDataCanChangeExplanation), _
                MsgBoxStyle.Exclamation, "Klaida")
                Exit Sub
            End If
        Next
        For Each item As GoodsTransferOfBalanceItem In items
            _FormManager.DataSource.Items.Remove(item)
        Next
    End Sub

    Private Sub AddNewItemButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles AddNewItemButton.Click

        Dim newWarehouseInfo As HelperLists.WarehouseInfo = Nothing
        Try
            newWarehouseInfo = DirectCast(WarehouseInfoListAccGridComboBox.SelectedValue, HelperLists.WarehouseInfo)
        Catch ex As Exception
        End Try

        If newWarehouseInfo Is Nothing OrElse newWarehouseInfo.IsEmpty Then
            MsgBox("Klaida. Nepasirinktas sandėlys.", MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If

        Dim ids As Integer() = GoodsOperationManager.RequestUserToChooseGoods()

        If ids Is Nothing OrElse ids.Length < 1 Then Exit Sub

        'GoodsTransferOfBalanceItemList.NewGoodsDiscardItemList(ids, newWarehouseInfo.ID)
        _QueryManager.InvokeQuery(Of GoodsTransferOfBalanceItemList)(Nothing, _
            "NewGoodsDiscardItemList", True, AddressOf OnNewItemsFetched, ids, newWarehouseInfo.ID)

    End Sub

    Private Sub PasteAccButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles PasteAccButton.Click

        Dim data As DataTable = F_DataImport.GetImportedData(GoodsTransferOfBalanceItem.GetDataTableSpecification)
        If data Is Nothing Then Exit Sub

        'GoodsTransferOfBalanceItemList.NewGoodsDiscardItemList(data)
        _QueryManager.InvokeQuery(Of GoodsTransferOfBalanceItemList)(Nothing,
            "NewGoodsDiscardItemList", True, AddressOf OnNewItemsFetched, data)

    End Sub

    Private Sub OnNewItemsFetched(ByVal result As Object, ByVal exceptionHandled As Boolean)

        If result Is Nothing Then Exit Sub

        Try
            _FormManager.DataSource.AddRange(DirectCast(result, GoodsTransferOfBalanceItemList))
        Catch ex As Exception
            ShowError(ex, New Object() {_FormManager.DataSource, result})
            Exit Sub
        End Try

        If Not StringIsNullOrEmpty(DirectCast(result, GoodsTransferOfBalanceItemList).DataImportWarnings) Then
            MsgBox(DirectCast(result, GoodsTransferOfBalanceItemList).DataImportWarnings, MsgBoxStyle.Exclamation, "Įspėjimas")
        End If

    End Sub

    Private Sub ViewJournalEntryButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ViewJournalEntryButton.Click
        If _FormManager.DataSource Is Nothing OrElse Not _FormManager.DataSource.JournalEntryID > 0 Then Exit Sub
        OpenJournalEntryEditForm(_QueryManager, _FormManager.DataSource.JournalEntryID)
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
            PrintObject(_FormManager.DataSource, False, 0, "PrekiuLikuciuPerkelimas", Me, "")
        Catch ex As Exception
            ShowError(ex, _FormManager.DataSource)
        End Try
    End Sub

    Public Sub OnPrintPreviewClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintPreviewClick
        If _FormManager.DataSource Is Nothing Then Exit Sub
        Try
            PrintObject(_FormManager.DataSource, True, 0, "PrekiuLikuciuPerkelimas", Me, "")
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
        Return _FormManager.DataSource.OperationalLimit.LimitsExplanation
    End Function

    Public Function HasChronologicContent() As Boolean _
        Implements ISupportsChronologicValidator.HasChronologicContent

        Return Not _FormManager.DataSource Is Nothing AndAlso _
            Not StringIsNullOrEmpty(_FormManager.DataSource.OperationalLimit.LimitsExplanation)

    End Function


    Private Sub _FormManager_DataSourceStateHasChanged(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles _FormManager.DataSourceStateHasChanged
        ConfigureButtons()
    End Sub

    Private Sub ConfigureButtons()

        If _FormManager.DataSource Is Nothing Then Exit Sub

        AddNewItemButton.Enabled = Not _FormManager.DataSource Is Nothing

        nCancelButton.Enabled = Not _FormManager.DataSource Is Nothing AndAlso Not _FormManager.DataSource.IsNew
        nOkButton.Enabled = Not _FormManager.DataSource Is Nothing
        ApplyButton.Enabled = Not _FormManager.DataSource Is Nothing

        EditedBaner.Visible = Not _FormManager.DataSource Is Nothing AndAlso Not _FormManager.DataSource.IsNew

    End Sub

End Class