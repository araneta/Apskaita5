Public Class F_TransferOfBalance

    Private Obj As General.TransferOfBalance
    Public Loading As Boolean = True


    Private Sub F_TransferOfBalance_Activated(ByVal sender As Object, _
            ByVal e As System.EventArgs) Handles Me.Activated

        If Me.WindowState = FormWindowState.Maximized AndAlso MyCustomSettings.AutoSizeForm Then _
            Me.WindowState = FormWindowState.Normal

        If Loading Then
            Loading = False
            Exit Sub
        End If

        If Not PrepareCache(Me, GetType(HelperLists.PersonInfoList), _
            GetType(HelperLists.AccountInfoList)) Then Exit Sub

    End Sub

    Private Sub F_TransferOfBalance_FormClosing(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not Obj Is Nothing AndAlso TypeOf Obj Is IIsDirtyEnough AndAlso _
                    DirectCast(Obj, IIsDirtyEnough).IsDirtyEnough Then
            Dim answ As String = Ask("Išsaugoti duomenis?", New ButtonStructure("Taip"), _
                New ButtonStructure("Ne"), New ButtonStructure("Atšaukti"))
            If answ <> "Taip" AndAlso answ <> "Ne" Then
                e.Cancel = True
                Exit Sub
            End If
            If answ = "Taip" Then
                If Not SaveObj() Then
                    e.Cancel = True
                    Exit Sub
                End If
            End If
        End If

        GetDataGridViewLayOut(DebetListDataGridView)
        GetDataGridViewLayOut(CreditListDataGridView)
        GetDataGridViewLayOut(AnalyticListDataGridView)
        GetFormLayout(Me)

    End Sub

    Private Sub F_TransferOfBalance_Load(ByVal sender As System.Object, _
            ByVal e As System.EventArgs) Handles MyBase.Load

        If Not SetDataSources() Then Exit Sub

        Try
            Obj = LoadObject(Of General.TransferOfBalance)(Nothing, "GetTransferOfBalance", False)
        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Exit Sub
        End Try

        TransferOfBalanceBindingSource.DataSource = Obj

        Dim CM2 As New ContextMenu()
        Dim CMItem1 As New MenuItem("Debeto likučius", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem1)
        Dim CMItem2 As New MenuItem("Debeto likučius overwrite", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem2)
        Dim CMItem3 As New MenuItem("Kredito likučius", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem3)
        Dim CMItem4 As New MenuItem("Kredito likučius overwrite", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem4)
        Dim CMItem5 As New MenuItem("Analitinius likučius", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem5)
        Dim CMItem6 As New MenuItem("Analitinius likučius overwrite", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem6)
        Dim CMItem7 As New MenuItem("Informacija apie formatą", AddressOf PasteAccButton_Click)
        CM2.MenuItems.Add(CMItem7)
        PasteAccButton.ContextMenu = CM2

        ReadWriteAuthorization1.ResetControlAuthorization()
        ConfigureButtons()

        SetDataGridViewLayOut(DebetListDataGridView)
        SetDataGridViewLayOut(CreditListDataGridView)
        SetDataGridViewLayOut(AnalyticListDataGridView)
        SetFormLayout(Me)

        Dim h As New EditableDataGridViewHelper(DebetListDataGridView)
        Dim g As New EditableDataGridViewHelper(CreditListDataGridView)
        Dim f As New EditableDataGridViewHelper(AnalyticListDataGridView)

    End Sub


    Private Sub OkButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles nOkButton.Click
        If Not SaveObj() Then Exit Sub
        Me.Hide()
        Me.Close()
    End Sub

    Private Sub ApplyButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ApplyButton.Click
        If Not SaveObj() Then Exit Sub
        nCancelButton.Enabled = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles nCancelButton.Click
        CancelObj()
    End Sub


    Private Sub LimitationsButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles LimitationsButton.Click
        If Obj Is Nothing Then Exit Sub
        MsgBox(Obj.ChronologicValidator.LimitsExplanation, MsgBoxStyle.Information, "")
    End Sub

    Private Sub PasteAccButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles PasteAccButton.Click

        If Obj Is Nothing OrElse Not General.TransferOfBalance.CanEditObject Then Exit Sub

        If GetSenderText(sender).Trim.ToLower.Contains("informacija") Then
            MsgBox(General.BookEntry.GetPasteStringColumnsDescription & vbCrLf _
                & General.TransferOfBalanceAnalytics.GetPasteStringColumnsDescription, _
                MsgBoxStyle.Information, "Info")
            Clipboard.SetText(String.Join(vbTab, General.BookEntry.GetPasteStringColumns) _
                & vbCrLf & vbCrLf & vbCrLf _
                & String.Join(vbTab, General.TransferOfBalanceAnalytics.GetPasteStringColumns))
            Exit Sub
        End If

        If GetSenderText(sender).Trim.ToLower.Contains("overwrite") Then
            If GetSenderText(sender).Trim.ToLower.Contains("debeto") AndAlso Obj.DebetList.Count > 0 Then
                If Not YesOrNo("Ar tikrai norite perrašyti visus debetinius likučius?") Then Exit Sub
            ElseIf GetSenderText(sender).Trim.ToLower.Contains("kredito") AndAlso Obj.CreditList.Count > 0 Then
                If Not YesOrNo("Ar tikrai norite perrašyti visus kreditinius likučius?") Then Exit Sub
            ElseIf GetSenderText(sender).Trim.ToLower.Contains("analitinius") AndAlso Obj.AnalyticsList.Count > 0 Then
                If Not YesOrNo("Ar tikrai norite perrašyti visus analitinius likučius?") Then Exit Sub
            End If
        End If

        Try
            Using busy As New StatusBusy
                If GetSenderText(sender).Trim.ToLower.Contains("kredito") Then
                    Obj.PasteCreditList(Clipboard.GetText, GetSenderText(sender).Trim.ToLower.Contains("overwrite"))
                ElseIf GetSenderText(sender).Trim.ToLower.Contains("analitinius") Then
                    Obj.PasteAnalyticsList(Clipboard.GetText, GetSenderText(sender).Trim.ToLower.Contains("overwrite"))
                Else ' let default be debet list
                    Obj.PasteDebetList(Clipboard.GetText, GetSenderText(sender).Trim.ToLower.Contains("overwrite"))
                End If
            End Using
        Catch ex As Exception
            ShowError(ex)
            Exit Sub
        End Try

    End Sub


    Private Function SaveObj() As Boolean

        If Not Obj.IsDirty Then Return True

        If Not Obj.IsValid Then
            MsgBox("Formoje yra klaidų:" & vbCrLf & Obj.BrokenRulesCollection.ToString( _
                Csla.Validation.RuleSeverity.Error), MsgBoxStyle.Exclamation, "Klaida.")
            Return False
        End If

        Dim Question, Answer As String
        If Obj.BrokenRulesCollection.WarningCount > 0 Then
            Question = "DĖMESIO. Duomenyse gali būti klaidų: " & vbCrLf _
                & Obj.BrokenRulesCollection.ToString(Csla.Validation.RuleSeverity.Warning) & vbCrLf
        Else
            Question = ""
        End If
        If Obj.IsNew Then
            Question = Question & "Ar tikrai norite įtraukti naujus duomenis?"
            Answer = "Nauji duomenys sėkmingai įtraukti."
        Else
            Question = Question & "Ar tikrai norite pakeisti duomenis?"
            Answer = "Duomenys sėkmingai pakeisti."
        End If

        If Not YesOrNo(Question) Then Return False

        Using busy As New StatusBusy

            AnalyticsListSortedBindingSource.RaiseListChangedEvents = False
            CreditListBindingSource.RaiseListChangedEvents = False
            DebetListBindingSource.RaiseListChangedEvents = False
            TransferOfBalanceBindingSource.RaiseListChangedEvents = False

            AnalyticsListSortedBindingSource.EndEdit()
            CreditListBindingSource.EndEdit()
            DebetListBindingSource.EndEdit()
            TransferOfBalanceBindingSource.EndEdit()

            Try
                Obj = LoadObject(Of General.TransferOfBalance)(Obj, "Save", False)
            Catch ex As Exception
                ShowError(ex)
                AnalyticsListSortedBindingSource.RaiseListChangedEvents = True
                CreditListBindingSource.RaiseListChangedEvents = True
                DebetListBindingSource.RaiseListChangedEvents = True
                TransferOfBalanceBindingSource.RaiseListChangedEvents = True
                Return False
            End Try

            TransferOfBalanceBindingSource.DataSource = Nothing
            AnalyticsListSortedBindingSource.DataSource = TransferOfBalanceBindingSource
            CreditListBindingSource.DataSource = TransferOfBalanceBindingSource
            DebetListBindingSource.DataSource = TransferOfBalanceBindingSource
            TransferOfBalanceBindingSource.DataSource = Obj

            AnalyticsListSortedBindingSource.RaiseListChangedEvents = True
            CreditListBindingSource.RaiseListChangedEvents = True
            DebetListBindingSource.RaiseListChangedEvents = True
            TransferOfBalanceBindingSource.RaiseListChangedEvents = True

            TransferOfBalanceBindingSource.ResetBindings(False)
            DebetListBindingSource.ResetBindings(False)
            CreditListBindingSource.ResetBindings(False)
            AnalyticsListSortedBindingSource.ResetBindings(False)

        End Using

        ConfigureButtons()

        MsgBox(Answer, MsgBoxStyle.Information, "Info")

        Return True

    End Function

    Private Sub CancelObj()

        If Obj Is Nothing OrElse Obj.IsNew OrElse Not Obj.IsDirty Then Exit Sub

        AnalyticsListSortedBindingSource.RaiseListChangedEvents = False
        CreditListBindingSource.RaiseListChangedEvents = False
        DebetListBindingSource.RaiseListChangedEvents = False
        TransferOfBalanceBindingSource.RaiseListChangedEvents = False

        AnalyticsListSortedBindingSource.CancelEdit()
        CreditListBindingSource.CancelEdit()
        DebetListBindingSource.CancelEdit()
        TransferOfBalanceBindingSource.CancelEdit()

        AnalyticsListSortedBindingSource.RaiseListChangedEvents = True
        CreditListBindingSource.RaiseListChangedEvents = True
        DebetListBindingSource.RaiseListChangedEvents = True
        TransferOfBalanceBindingSource.RaiseListChangedEvents = True

        TransferOfBalanceBindingSource.ResetBindings(False)
        DebetListBindingSource.ResetBindings(False)
        CreditListBindingSource.ResetBindings(False)
        AnalyticsListSortedBindingSource.ResetBindings(False)

    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, GetType(HelperLists.PersonInfoList), _
            GetType(HelperLists.AccountInfoList)) Then Return False

        Try

            LoadAccountInfoListToGridCombo(DataGridViewTextBoxColumn3, True)
            LoadAccountInfoListToGridCombo(DataGridViewTextBoxColumn8, True)
            LoadAccountInfoListToGridCombo(AccountDataGridViewTextBoxColumn, True)
            LoadPersonInfoListToGridCombo(PersonDataGridViewTextBoxColumn, True, True, True, True)

            EntryTypeHumanReadableDataGridViewTextBoxColumn.DataSource = _
                ApskaitaObjects.GetEnumValuesHumanReadableList(GetType(BookEntryType), False)

        Catch ex As Exception
            ShowError(ex)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function

    Private Sub ConfigureButtons()

        If Obj Is Nothing Then Exit Sub

        nCancelButton.Enabled = Not (Obj.IsNew)
        nOkButton.Enabled = General.TransferOfBalance.CanEditObject
        ApplyButton.Enabled = General.TransferOfBalance.CanEditObject
        PasteAccButton.Enabled = General.TransferOfBalance.CanEditObject

        AnalyticListDataGridView.AllowUserToAddRows = Obj.ChronologicValidator.FinancialDataCanChange
        AnalyticListDataGridView.AllowUserToDeleteRows = Obj.ChronologicValidator.FinancialDataCanChange
        AnalyticListDataGridView.ReadOnly = Not Obj.ChronologicValidator.FinancialDataCanChange

        DebetListDataGridView.AllowUserToAddRows = Obj.ChronologicValidator.FinancialDataCanChange
        DebetListDataGridView.AllowUserToDeleteRows = Obj.ChronologicValidator.FinancialDataCanChange
        DebetListDataGridView.ReadOnly = Not Obj.ChronologicValidator.FinancialDataCanChange

        CreditListDataGridView.AllowUserToAddRows = Obj.ChronologicValidator.FinancialDataCanChange
        CreditListDataGridView.AllowUserToDeleteRows = Obj.ChronologicValidator.FinancialDataCanChange
        CreditListDataGridView.ReadOnly = Not Obj.ChronologicValidator.FinancialDataCanChange

        LimitationsButton.Visible = Not String.IsNullOrEmpty(Obj.ChronologicValidator.LimitsExplanation.Trim)

    End Sub

End Class