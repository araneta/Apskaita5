Imports ApskaitaObjects.Documents
Imports ApskaitaObjects.Documents.BankDataExchangeProviders
Imports ApskaitaObjects.HelperLists
Imports System.Drawing
Imports AccControlsWinForms
Imports AccDataBindingsWinForms.CachedInfoLists
Imports ApskaitaObjects.Attributes
Imports BrightIdeasSoftware
Imports AccControlsWinForms.WebControls

Friend Class F_BankOperationItemList

    Private ReadOnly _RequiredCachedLists As Type() = New Type() _
        {GetType(PersonInfoList), GetType(AccountInfoList), _
        GetType(CashAccountInfoList)}

    Private WithEvents _FormManager As CslaActionExtenderEditForm(Of BankOperationItemList)
    Private _ListViewManager As DataListViewEditControlManager(Of BankOperationItem)
    Private _QueryManager As CslaActionExtenderQueryObject

    Private _CurrentItem As BankOperationItem = Nothing


    Private Sub F_BankOperationItemList_FormClosed(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Try
            MyCustomSettings.BankDocumentPrefix = DocumentNumberPrefixTextBox.Text.Trim
            MyCustomSettings.IgnoreWrongIBAN = IgnoreIBANCheckBox.Checked
            MyCustomSettings.Save()
        Catch ex As Exception
            ShowError(New Exception("Nepavyko išsaugoti programos nustatymų: " & ex.Message, ex), Nothing)
        End Try

    End Sub

    Private Sub F_BankOperationItemList_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If Not SetDataSources() Then Exit Sub

        Try

            _FormManager = New CslaActionExtenderEditForm(Of BankOperationItemList) _
                (Me, BankOperationItemListBindingSource, BankOperationItemList.NewBankOperationItemList, _
                _RequiredCachedLists, IOkButton, IApplyButton, ICancelButton, Nothing, ProgressFiller1)

            _FormManager.ManageDataListViewStates(BankOperationItemListDataListView)

        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Exit Sub
        End Try

    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, _RequiredCachedLists) Then Exit Function

        Try

            _ListViewManager = New DataListViewEditControlManager(Of BankOperationItem) _
                (BankOperationItemListDataListView, ContextMenuStrip1, AddressOf OnItemsRemove, _
                 Nothing, AddressOf ItemActionIsAvailable, Nothing)

            _ListViewManager.AddCancelButton = True
            _ListViewManager.AddButtonHandler("Koreguoti", _
                "Koreguoti banko operacijos duomenis banko operacijos formoje.", AddressOf OnEditItem)
            _ListViewManager.AddButtonHandler("Ištrinti", _
                "Pašalinti banko operacijos duomenis iš duomenų bazės.", AddressOf OnDeleteItem)
            _ListViewManager.AddButtonHandler("Naujas Kontrahentas", _
                "Įtraukti naujo kontrahento duomenis į duomenų bazę.", AddressOf OnAddPerson)

            _ListViewManager.AddMenuItemHandler(EditItem_MenuItem, AddressOf OnEditItem)
            _ListViewManager.AddMenuItemHandler(DeleteItem_MenuItem, AddressOf OnDeleteItem)
            _ListViewManager.AddMenuItemHandler(AddPerson_MenuItem, AddressOf OnAddPerson)

            _QueryManager = New CslaActionExtenderQueryObject(Me, ProgressFiller2)

            PrepareControl(AccountAccGridComboBox, New CashAccountFieldAttribute( _
                ValueRequiredLevel.Mandatory, True, CashAccountType.BankAccount, _
                CashAccountType.PseudoBankAccount))

        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Return False
        End Try

        DocumentNumberPrefixTextBox.Text = MyCustomSettings.BankDocumentPrefix.Trim
        IgnoreIBANCheckBox.Checked = MyCustomSettings.IgnoreWrongIBAN

        Dim cm As New ContextMenu()
        cm.MenuItems.Add(New MenuItem("LITAS-ESIS", AddressOf LoadDataButton_Click))
        cm.MenuItems.Add(New MenuItem("Kitas formatas", AddressOf LoadDataButton_Click))
        OpenFileAccButton.ContextMenu = cm

        Return True

    End Function


    Private Sub OnItemsRemove(ByVal items As BankOperationItem())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As BankOperationItem In items
            _FormManager.DataSource.Remove(item)
        Next
    End Sub

    Private Function ItemActionIsAvailable(ByVal item As BankOperationItem, _
        ByVal action As String) As Boolean

        If item Is Nothing OrElse action Is Nothing Then Return False

        If action.Trim.ToLower = "OnDeleteItem".ToLower Then

            Return item.ExistsInDatabase

        ElseIf action.Trim.ToLower = "OnAddPerson".ToLower Then

            Return Not String.IsNullOrEmpty(item.PersonName.Trim) OrElse _
                Not String.IsNullOrEmpty(item.PersonCode.Trim)

        End If

        Return True

    End Function

    Private Sub OnEditItem(ByVal item As BankOperationItem)
        If item Is Nothing OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        If item.ExistsInDatabase Then
            ' BankOperation.GetBankOperation(item.OperationDatabaseID)
            _QueryManager.InvokeQuery(Of BankOperation)(Nothing, "GetBankOperation", True, _
                AddressOf OpenObjectEditForm, item.OperationDatabaseID)
        Else
            OpenObjectEditForm(New ImportedBankOperation(item, _FormManager.DataSource.Account))
        End If
    End Sub

    Private Sub OnDeleteItem(ByVal item As BankOperationItem)

        If item Is Nothing OrElse Not item.ExistsInDatabase Then Exit Sub

        If CheckIfObjectEditFormOpen(Of BankOperation)(item.OperationDatabaseID, True, True) Then Exit Sub

        If Not YesOrNo("Ar tikrai norite pašalinti dokumento duomenis iš duomenų bazės?") Then Exit Sub

        _CurrentItem = item

        ' BankOperation.DeleteBankOperation(item.OperationDatabaseID)
        _QueryManager.InvokeQuery(Of BankOperation)(Nothing, "DeleteBankOperation", False, _
            AddressOf OnItemDeleted, item.OperationDatabaseID)

    End Sub

    Private Sub OnItemDeleted(ByVal nullResult As Object, ByVal exceptionHandled As Boolean)
        If exceptionHandled Then
            _CurrentItem = Nothing
            Exit Sub
        End If
        _FormManager.DataSource.Remove(_CurrentItem)
        _CurrentItem = Nothing
    End Sub

    Private Sub OnAddPerson(ByVal item As BankOperationItem)

        If String.IsNullOrEmpty(item.PersonName.Trim) AndAlso _
            String.IsNullOrEmpty(item.PersonCode.Trim) Then Exit Sub

        _CurrentItem = item

        ' General.Person.NewPerson()
        _QueryManager.InvokeQuery(Of General.Person)(Nothing, "NewPerson", True, AddressOf OnNewPersonFetched)

    End Sub

    Private Sub OnNewPersonFetched(ByVal result As Object, ByVal exceptionHandled As Boolean)

        If result Is Nothing Then
            _CurrentItem = Nothing
            Exit Sub
        End If

        Dim person As General.Person = DirectCast(result, General.Person)

        person.BankAccount = _CurrentItem.PersonBankAccount
        person.Bank = _CurrentItem.PersonBankName
        person.Name = _CurrentItem.PersonName
        person.Code = _CurrentItem.PersonCode

        _CurrentItem = Nothing

        OpenObjectEditForm(person)

    End Sub

    Private Sub DataListView_FormatRow(ByVal sender As Object, _
        ByVal e As FormatRowEventArgs) Handles BankOperationItemListDataListView.FormatRow

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Dim currentItem As BankOperationItem = Nothing
        Try
            currentItem = DirectCast(e.Model, BankOperationItem)
        Catch ex As Exception
        End Try
        If currentItem Is Nothing Then Exit Sub

        If currentItem.ExistsInDatabase Then
            e.Item.BackColor = Color.Gray
        ElseIf currentItem.ProbablyExistsInDatabase Then
            e.Item.BackColor = Color.LightSteelBlue
        ElseIf currentItem.Person = HelperLists.PersonInfo.Empty AndAlso Not currentItem.IsBankCosts Then
            e.Item.BackColor = Color.MistyRose
        Else
            e.Item.BackColor = BankOperationItemListDataListView.BackColor
        End If

    End Sub


    Private Sub LoadDataButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles PasteButton.Click, OpenFileAccButton.Click

        If sender Is Nothing Then Exit Sub

        Dim currentAccount As CashAccountInfo = Nothing
        Try
            currentAccount = DirectCast(AccountAccGridComboBox.SelectedValue, CashAccountInfo)
        Catch ex As Exception
        End Try
        If currentAccount Is Nothing OrElse currentAccount.IsEmpty Then
            MsgBox("Klaida. Nepasirinkta sąskaita, į kurią importuojami duomenys.", _
                MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If

        If Not currentAccount.EnforceUniqueOperationID Then
            MsgBox("Dėmesio. Pasirinkta sąskaita pagal jos nustatymus neužtikrina operacijų unikalumo. Rekomenduotina sąskaitoms, kuriose naudojamas importas, nustatyti privalomą unikalų kodą.", MsgBoxStyle.Information, "Įspėjimas")
        End If

        Dim needsFile As Boolean = True
        Dim accountStatement As IBankAccountStatement
        If GetSenderText(sender).Trim.ToLower.Contains("litas-esis") Then
            accountStatement = New LitasEsisBankAccountStatement
        ElseIf GetSenderText(sender).Trim.ToLower.Contains("kitas formatas") Then
            accountStatement = New ProprietaryBankAccountStatement()
        ElseIf sender Is OpenFileAccButton Then
            accountStatement = New ISO20022v053BankAccountStatement()
        Else
            accountStatement = New ProprietaryBankAccountStatement()
            needsFile = False
        End If

        Dim fileName As String = ""
        If needsFile Then

            Using ofd As New OpenFileDialog
                ofd.Multiselect = False
                ofd.Filter = String.Format("{0}|*.{1}|All Files (*.*)|*.*", _
                    accountStatement.FileFormatDescription, accountStatement.FileExtension)
                If ofd.ShowDialog(Me) <> System.Windows.Forms.DialogResult.OK Then Exit Sub
                fileName = ofd.FileName
            End Using
            If StringIsNullOrEmpty(fileName) OrElse Not IO.File.Exists(fileName) Then Exit Sub

        End If

        Try

            If TypeOf accountStatement Is ProprietaryBankAccountStatement Then

                Dim data As DataTable
                If needsFile Then
                    data = F_DataImport.GetImportedData(ProprietaryBankAccountStatement.GetDataTableSpecification, fileName)
                Else
                    data = F_DataImport.GetImportedData(ProprietaryBankAccountStatement.GetDataTableSpecification)
                End If

                If data Is Nothing Then Exit Sub

                DirectCast(accountStatement, ProprietaryBankAccountStatement).LoadDataFromTable(data)

            Else

                Using busy As New StatusBusy
                    If needsFile Then
                        accountStatement.LoadDataFromFile(fileName)
                    Else
                        accountStatement.LoadDataFromString(Clipboard.GetText())
                    End If
                End Using

            End If

            'BankOperationItemList.GetBankOperationItemList(accountStatement, currentAccount, _
            '    DocumentNumberPrefixTextBox.Text.Trim, IgnoreIBANCheckBox.Checked)
            _QueryManager.InvokeQuery(Of BankOperationItemList)(Nothing, "GetBankOperationItemList", True, _
                AddressOf OnDataLoaded, accountStatement, currentAccount, _
                DocumentNumberPrefixTextBox.Text.Trim, IgnoreIBANCheckBox.Checked)

        Catch ex As Exception
            ShowError(ex, accountStatement)
            Exit Sub
        End Try

    End Sub

    Private Sub OnDataLoaded(ByVal result As Object, ByVal exceptionHandled As Boolean)

        If result Is Nothing Then Exit Sub

        _FormManager.AddNewDataSource(DirectCast(result, BankOperationItemList))

    End Sub

    Private Sub nHelpButton_Click(ByVal sender As System.Object,
        ByVal e As System.EventArgs)

        MsgBox("Importuojant duomenys copy paste būdu, duomenys turi atitikti šiuos reikalavimus:" _
            & vbCrLf & "Duomenų eilutės turi būti atskirtos naudojant cr lf simbolius." _
            & vbCrLf & "Duomenų stulpeliai turi būti atskirti naudojant tab simbolį." _
            & vbCrLf & "Duomenyse negali būti simbolių, naudojamų kaip skirtukai, t.y. cr lf arba tab." _
            & vbCrLf & ProprietaryBankAccountStatement.GetPasteStringColumnsDescription, MsgBoxStyle.Information, "Info")
        Clipboard.SetText(String.Join(vbTab, ProprietaryBankAccountStatement.GetPasteStringColumns), TextDataFormat.UnicodeText)

    End Sub

    Private Sub GetCurrencyRatesButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles GetCurrencyRatesButton.Click

        If _FormManager.DataSource Is Nothing Then Exit Sub

        Dim paramList As New List(Of CurrencyRate.CurrencyRateParam)

        For Each b As BankOperationItem In _FormManager.DataSource
            paramList.Add(New CurrencyRate.CurrencyRateParam(b.Date, b.Currency))
            paramList.Add(New CurrencyRate.CurrencyRateParam(b.Date, _FormManager.DataSource.Account.CurrencyCode))
        Next

        If Not YesOrNo("Gauti valiutų kursus?") Then Exit Sub

        Dim factory As CurrencyRateFactoryBase = Nothing
        Dim result As List(Of CurrencyRate) = Nothing
        Dim baseCurrency As String = GetCurrentCompany.BaseCurrency
        Try
            factory = WebControls.GetCurrencyRateFactory(baseCurrency)
            result = WebControls.GetCurrencyRateListWithProgress(paramList.ToArray, factory)
        Catch ex As Exception
            ShowError(ex, paramList)
            Exit Sub
        End Try

        If Not result Is Nothing AndAlso result.Count > 0 Then

            For Each b As BankOperationItem In _FormManager.DataSource
                b.CurrencyRate = CurrencyRate.GetRate(result, b.Date, b.Currency, baseCurrency)
                b.CurrencyRateInAccount = CurrencyRate.GetRate(result, b.Date, _
                    _FormManager.DataSource.Account.CurrencyCode, baseCurrency)
            Next

        End If

    End Sub


    Public Sub MarkAsExistingInDatabase(ByVal updatedBankOperation As BankOperation)
        If _FormManager.DataSource Is Nothing Then Exit Sub
        _FormManager.DataSource.UpdateWithBankOperation(updatedBankOperation)
    End Sub

    Private Sub _FormManager_DataSourceStateHasChanged(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles _FormManager.DataSourceStateHasChanged

        If _FormManager.DataSource Is Nothing Then Exit Sub

        AccountTextBox.Text = _FormManager.DataSource.Account.ToString
        ReportDescriptionTextBox.Text = _FormManager.DataSource.GetDescription

        BankOperationItemListDataListView.Select()

    End Sub

End Class