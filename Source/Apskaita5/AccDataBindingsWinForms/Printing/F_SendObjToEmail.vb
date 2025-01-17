Imports AccControlsWinForms
Imports AccDataBindingsWinForms.Printing

Friend Class F_SendObjToEmail

    Private Obj As Object
    Private _Version As Integer

    Public Sub New(ByVal ObjToSend As Object, ByVal Version As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Obj = ObjToSend
        _Version = Version

    End Sub

    Private Sub F_SendObjToEmail_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If TypeOf Obj Is Documents.InvoiceMade Then

            SubjectTextBox.Text = "Saskaita - faktura (invoice) " & _
                DirectCast(Obj, Documents.InvoiceMade).Date.ToShortDateString & " Nr. " _
                & DirectCast(Obj, Documents.InvoiceMade).Serial _
                & DirectCast(Obj, Documents.InvoiceMade).FullNumber

            If Not DirectCast(Obj, Documents.InvoiceMade).Payer Is Nothing AndAlso _
                DirectCast(Obj, Documents.InvoiceMade).Payer.ID > 0 AndAlso _
                Not String.IsNullOrEmpty(DirectCast(Obj, Documents.InvoiceMade).Payer.Email.Trim) Then

                EmailTextBox.Text = DirectCast(Obj, Documents.InvoiceMade).Payer.Email.Trim

            End If

            MessageTextBox.Text = MyCustomSettings.EmailMessageText

        ElseIf TypeOf Obj Is Documents.AccumulativeCosts Then
            SubjectTextBox.Text = "Sukauptu sanaudu/pajamu operacija"
        ElseIf TypeOf Obj Is Documents.AdvanceReport Then
            SubjectTextBox.Text = "Avanso apyskaita"
        ElseIf TypeOf Obj Is Documents.BankOperation Then
            SubjectTextBox.Text = "Banko operacija"
        ElseIf TypeOf Obj Is Documents.CashAccountList Then
            SubjectTextBox.Text = "Lesu saskaitu suvestine"
        ElseIf TypeOf Obj Is Documents.Offset Then
            SubjectTextBox.Text = "Uzskaita"
        ElseIf TypeOf Obj Is Documents.TillIncomeOrder Then
            SubjectTextBox.Text = "Kasos pajamu orderis"
        ElseIf TypeOf Obj Is Documents.TillSpendingsOrder Then
            SubjectTextBox.Text = "Kasos islaidu orderis"
        ElseIf TypeOf Obj Is General.AccountList Then
            SubjectTextBox.Text = "Saskaitu planas"
        ElseIf TypeOf Obj Is General.JournalEntry Then
            SubjectTextBox.Text = "BZ operacija"
        ElseIf TypeOf Obj Is ActiveReports.AccountTurnoverInfoList Then
            SubjectTextBox.Text = "Saskaitu apyvartos ziniarastis"
        ElseIf TypeOf Obj Is ActiveReports.AdvanceReportInfoList Then
            SubjectTextBox.Text = "Avanso apyskaitu suvestine"
        ElseIf TypeOf Obj Is ActiveReports.BookEntryInfoListParent Then
            SubjectTextBox.Text = "Saskaitos apyvartos ziniarastis"
        ElseIf TypeOf Obj Is ActiveReports.CashOperationInfoList Then
            SubjectTextBox.Text = "Lėšų apyvartos ziniarastis"
        ElseIf TypeOf Obj Is ActiveReports.ContractInfoList Then
            SubjectTextBox.Text = "Darbo sutarciu suvestine"
        ElseIf TypeOf Obj Is ActiveReports.DebtInfoList Then
            SubjectTextBox.Text = "Skolu ziniarastis"
        ElseIf TypeOf Obj Is ActiveReports.GoodsOperationInfoListParent Then
            SubjectTextBox.Text = "Operaciju su prekemis suvestine"
        ElseIf TypeOf Obj Is ActiveReports.GoodsTurnoverInfoList Then
            SubjectTextBox.Text = "Prekiu apyvartos ataskaita"
        ElseIf TypeOf Obj Is ActiveReports.ImprestSheetInfoList Then
            SubjectTextBox.Text = "Darbo uzmokescio avanso ziniarasciu suvestine"
        ElseIf TypeOf Obj Is ActiveReports.InvoiceInfoItemList Then
            SubjectTextBox.Text = "Saskaitu fakturu registras"
        ElseIf TypeOf Obj Is ActiveReports.JournalEntryInfoList Then
            SubjectTextBox.Text = "Bendrasis zurnalas"
        ElseIf TypeOf Obj Is ActiveReports.LabourContractUpdateInfoList Then
            SubjectTextBox.Text = "Darbo sutarciu pakeitimu suvestine"
        ElseIf TypeOf Obj Is ActiveReports.PayOutNaturalPersonInfoList Then
            SubjectTextBox.Text = "Ismoku gyventojams suvestine"
        ElseIf TypeOf Obj Is ActiveReports.PersonInfoItemList Then
            SubjectTextBox.Text = "Kontrahentu sarasas"
        ElseIf TypeOf Obj Is ActiveReports.ProductionCalculationItemList Then
            SubjectTextBox.Text = "Gamybos kalkuliaciju suvestine"
        ElseIf TypeOf Obj Is ActiveReports.WageSheetInfoList Then
            SubjectTextBox.Text = "Darbo uzmokescio ziniarasciu suvestine"
        ElseIf TypeOf Obj Is ActiveReports.WorkerWageInfoList Then
            SubjectTextBox.Text = "Darbuotojo kortele"
        ElseIf TypeOf Obj Is ActiveReports.WorkerWageInfoReport Then
            SubjectTextBox.Text = "Darbuotojo kortele"
        ElseIf TypeOf Obj Is ActiveReports.WorkTimeSheetInfoList Then
            SubjectTextBox.Text = "Darbo laiko apskaitos ziniarasciu suvestine"
        ElseIf TypeOf Obj Is ActiveReports.LongTermAssetComplexDocumentInfoList Then
            SubjectTextBox.Text = "Operaciju su IT dokumentu suvestine"
        ElseIf TypeOf Obj Is ActiveReports.LongTermAssetOperationInfoListParent Then
            SubjectTextBox.Text = "Operaciju su IT suvestine"
        ElseIf TypeOf Obj Is ActiveReports.LongTermAssetInfoList Then
            SubjectTextBox.Text = "Ilgalaikio turto suvestine"
        ElseIf TypeOf Obj Is Assets.OperationAccountChange Then
            SubjectTextBox.Text = "Ilgalaikio turto apskaitos saskaitos pakeitimo pazyma"
        ElseIf TypeOf Obj Is Assets.OperationAcquisitionValueIncrease Then
            SubjectTextBox.Text = "Ilgalaikio turto isig. savikainos padidinimo pazyma"
        ElseIf TypeOf Obj Is Assets.OperationAmortization Then
            SubjectTextBox.Text = "Ilgalaikio turto amortizacijos dokumentas"
        ElseIf TypeOf Obj Is Assets.OperationAmortizationPeriodChange Then
            SubjectTextBox.Text = "Ilgalaikio turto amortizacijos laikotarpio pakeitimo pazyma"
        ElseIf TypeOf Obj Is Assets.OperationDiscard Then
            SubjectTextBox.Text = "Ilgalaikio turto nurasymo aktas"
        ElseIf TypeOf Obj Is Assets.OperationOperationalStatusChange Then
            SubjectTextBox.Text = "Ilgalaikio turto eksploatacijos aktas"
        ElseIf TypeOf Obj Is Assets.OperationTransfer Then
            SubjectTextBox.Text = "Ilgalaikio turto nefakturinio perleidimo aktas"
        ElseIf TypeOf Obj Is Assets.OperationValueChange Then
            SubjectTextBox.Text = "Ilgalaikio turto perkainojimo pazyma"
        ElseIf TypeOf Obj Is Assets.ComplexOperationAmortization Then
            SubjectTextBox.Text = "Ilgalaikio turto amortizacijos dokumentas"
        ElseIf TypeOf Obj Is Assets.ComplexOperationDiscard Then
            SubjectTextBox.Text = "Ilgalaikio turto nurasymo aktas"
        ElseIf TypeOf Obj Is Assets.ComplexOperationOperationalStatusChange Then
            SubjectTextBox.Text = "Ilgalaikio turto eksploatacijos aktas"
        ElseIf TypeOf Obj Is Assets.ComplexOperationValueChange Then
            SubjectTextBox.Text = "Ilgalaikio turto perkainojimo pazyma"
        ElseIf TypeOf Obj Is Assets.LongTermAssetsTransferOfBalance Then
            SubjectTextBox.Text = "Ilgalaikio turto likuciu perkelimo pazyma"
        ElseIf TypeOf Obj Is Goods.GoodsComplexOperationDiscard Then
            SubjectTextBox.Text = "Prekiu nurasymo aktas"
        ElseIf TypeOf Obj Is Goods.GoodsComplexOperationInternalTransfer Then
            SubjectTextBox.Text = "Prekiu vidinio judejimo aktas"
        ElseIf TypeOf Obj Is Goods.GoodsComplexOperationInventorization Then
            SubjectTextBox.Text = "Prekiu inventorizacijos aktas"
        ElseIf TypeOf Obj Is Goods.GoodsComplexOperationPriceCut Then
            SubjectTextBox.Text = "Prekiu nukainojimo aktas"
        ElseIf TypeOf Obj Is Goods.GoodsComplexOperationProduction Then
            SubjectTextBox.Text = "Prekiu gamybos aktas"
        ElseIf TypeOf Obj Is Goods.GoodsComplexOperationTransferOfBalance Then
            SubjectTextBox.Text = "Prekiu likuciu perkelimas"
        ElseIf TypeOf Obj Is Goods.GoodsOperationAccountChange Then
            SubjectTextBox.Text = "Prekes apskaitos saskaitos pakeitimas"
        ElseIf TypeOf Obj Is Goods.GoodsOperationAcquisition Then
            SubjectTextBox.Text = "Prekes pajamavimas"
        ElseIf TypeOf Obj Is Goods.GoodsOperationAdditionalCosts Then
            SubjectTextBox.Text = "Prekes savikainos padidinimas"
        ElseIf TypeOf Obj Is Goods.GoodsOperationDiscard Then
            SubjectTextBox.Text = "Prekes nurasymas"
        ElseIf TypeOf Obj Is Goods.GoodsOperationDiscount Then
            SubjectTextBox.Text = "Gauta nuolaida prekems"
        ElseIf TypeOf Obj Is Goods.GoodsOperationPriceCut Then
            SubjectTextBox.Text = "Prekes nukainojimas"
        ElseIf TypeOf Obj Is Goods.GoodsOperationTransfer Then
            SubjectTextBox.Text = "Prekes vidinis judejimas"
        ElseIf TypeOf Obj Is Goods.GoodsOperationValuationMethod Then
            SubjectTextBox.Text = "Prekes vertinimo metodo pakeitimas"
        ElseIf TypeOf Obj Is Goods.ProductionCalculation Then
            SubjectTextBox.Text = "Gamybos kalkuliacija"
        ElseIf TypeOf Obj Is Goods.WarehouseList Then
            SubjectTextBox.Text = "Prekiu sandeliu suvestine"
        ElseIf TypeOf Obj Is Workers.Contract Then
            SubjectTextBox.Text = "Darbo sutartis"
        ElseIf TypeOf Obj Is Workers.ContractUpdate Then
            SubjectTextBox.Text = "Darbo sutarties pakeitimas"
        ElseIf TypeOf Obj Is Workers.ImprestSheet Then
            SubjectTextBox.Text = "Darbo uzmokescio avanso ziniarastis"
        ElseIf TypeOf Obj Is Workers.PayOutNaturalPerson Then
            SubjectTextBox.Text = "Ismoka gyventojui"
        ElseIf TypeOf Obj Is Workers.WageSheet Then
            SubjectTextBox.Text = "Darbo uzmokescio ziniarastis"
        ElseIf TypeOf Obj Is Workers.WorkTimeSheet Then
            SubjectTextBox.Text = "Darbo laiko apskaitos ziniarastis"
        End If

    End Sub

    Private Sub SendButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles SendButton.Click

        If StringIsNullOrEmpty(EmailTextBox.Text) Then
            MsgBox("Klaida. Nenurodytas gavėjo e-paštas.", MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        ElseIf StringIsNullOrEmpty(SubjectTextBox.Text) Then
            MsgBox("Klaida. Nenurodytas pranešimo dalykas.", MsgBoxStyle.Exclamation, "Klaida")
            Exit Sub
        End If

        Dim FileName As String = ""
        If TypeOf Obj Is Documents.InvoiceMade Then FileName = _
            DirectCast(Obj, Documents.InvoiceMade).GetFileName

        Try
            Using busy As New StatusBusy
                SendObjectToEmail(Obj, EmailTextBox.Text.Trim, SubjectTextBox.Text.Trim, _
                    MessageTextBox.Text.Trim, _Version, FileName, "", Nothing)
            End Using
        Catch ex As Exception
            ShowError(ex, Obj)
            Exit Sub
        End Try

        If MyCustomSettings.UseDefaultEmailClient AndAlso MyCustomSettings.ShowDefaultMailClientWindow Then
            MsgBox("Dokumentas sėkmingai perduotas e-pašto programai.", MsgBoxStyle.Information, "Info")
        Else
            MsgBox("Dokumentas sėkmingai išsiųstas.", MsgBoxStyle.Information, "Info")
        End If

        Me.Close()

    End Sub

    Private Shared Function IsSubclassOfRawGeneric(ByVal generic As Type, ByVal toCheck As Type) As Boolean
        While toCheck IsNot Nothing AndAlso Not toCheck Is GetType(Object)
            Dim cur As Type
            If toCheck.IsGenericType Then
                cur = toCheck.GetGenericTypeDefinition()
            Else
                cur = toCheck
            End If
            If generic Is cur Then Return True
            toCheck = toCheck.BaseType
        End While
        Return False
    End Function

End Class