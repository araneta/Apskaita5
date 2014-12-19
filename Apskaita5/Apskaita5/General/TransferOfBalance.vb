Namespace General

    <Serializable()> _
    Public Class TransferOfBalance
        Inherits BusinessBase(Of TransferOfBalance)
        Implements IIsDirtyEnough

#Region " Business Methods "

        Private _ID As Integer = 0
        Private _ChronologicValidator As TransferOfBalanceChronologicValidator
        Private _Date As Date = Today
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now
        Private WithEvents _DebetList As BookEntryList
        Private WithEvents _CreditList As BookEntryList
        Private WithEvents _AnalyticsList As TransferOfBalanceAnalyticsList
        Private _DebetSum As Double = 0
        Private _CreditSum As Double = 0


        Private SuspendChildListChangedEvents As Boolean = False
        ' used to implement automatic sort in datagridview
        <NotUndoable()> _
        <NonSerialized()> _
        Private _AnalyticsListSortedList As Csla.SortedBindingList(Of TransferOfBalanceAnalytics) = Nothing
        ' used to implement automatic sort in datagridview
        <NotUndoable()> _
        <NonSerialized()> _
        Private _DebetListSortedList As Csla.SortedBindingList(Of BookEntry) = Nothing
        ' used to implement automatic sort in datagridview
        <NotUndoable()> _
        <NonSerialized()> _
        Private _CreditListSortedList As Csla.SortedBindingList(Of BookEntry) = Nothing


        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property ChronologicValidator() As TransferOfBalanceChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ChronologicValidator
            End Get
        End Property

        Public ReadOnly Property InsertDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _InsertDate
            End Get
        End Property

        Public ReadOnly Property UpdateDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UpdateDate
            End Get
        End Property

        Public Property [Date]() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Date
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Date)
                CanWriteProperty(True)
                If _Date.Date <> value.Date Then
                    _Date = value.Date
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public ReadOnly Property DebetList() As BookEntryList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DebetList
            End Get
        End Property

        Public ReadOnly Property CreditList() As BookEntryList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CreditList
            End Get
        End Property

        Public ReadOnly Property AnalyticsList() As TransferOfBalanceAnalyticsList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AnalyticsList
            End Get
        End Property

        Public ReadOnly Property DebetListSorted() As Csla.SortedBindingList(Of BookEntry)
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _DebetListSortedList Is Nothing Then _DebetListSortedList = _
                    New Csla.SortedBindingList(Of BookEntry)(_DebetList)
                Return _DebetListSortedList
            End Get
        End Property

        Public ReadOnly Property CreditListSorted() As Csla.SortedBindingList(Of BookEntry)
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _CreditListSortedList Is Nothing Then _CreditListSortedList = _
                    New Csla.SortedBindingList(Of BookEntry)(_CreditList)
                Return _CreditListSortedList
            End Get
        End Property

        Public ReadOnly Property AnalyticsListSorted() As Csla.SortedBindingList(Of TransferOfBalanceAnalytics)
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _AnalyticsListSortedList Is Nothing Then _AnalyticsListSortedList = _
                    New Csla.SortedBindingList(Of TransferOfBalanceAnalytics)(_AnalyticsList)
                Return _AnalyticsListSortedList
            End Get
        End Property

        Public ReadOnly Property DebetSum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_DebetSum)
            End Get
        End Property

        Public ReadOnly Property CreditSum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CreditSum)
            End Get
        End Property


        Public ReadOnly Property IsDirtyEnough() As Boolean _
        Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return _DebetList.Count > 0 OrElse _CreditList.Count > 0 _
                    OrElse _AnalyticsList.Count > 0
            End Get
        End Property

        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _AnalyticsList.IsDirty _
                    OrElse _DebetList.IsDirty OrElse _CreditList.IsDirty
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return MyBase.IsValid AndAlso _AnalyticsList.IsValid _
                    AndAlso _DebetList.IsValid AndAlso _CreditList.IsValid
            End Get
        End Property


        Public Overrides Function Save() As TransferOfBalance

            Me.ValidationRules.CheckRules()
            If Not IsValid Then Throw New Exception("Duomenyse yra klaidų: " _
                & vbCrLf & Me.GetAllBrokenRules)

            Return MyBase.Save

        End Function


        Private Sub AnalyticsList_Changed(ByVal sender As Object, _
            ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _AnalyticsList.ListChanged

            If SuspendChildListChangedEvents Then Exit Sub

        End Sub

        Private Sub DebetList_Changed(ByVal sender As Object, _
        ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _DebetList.ListChanged

            If SuspendChildListChangedEvents Then Exit Sub

            _DebetSum = _DebetList.GetSum
            PropertyHasChanged("DebetSum")

        End Sub

        Private Sub CreditList_Changed(ByVal sender As Object, _
            ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _CreditList.ListChanged

            If SuspendChildListChangedEvents Then Exit Sub

            _CreditSum = _CreditList.GetSum
            PropertyHasChanged("CreditSum")

        End Sub

        ''' <summary>
        ''' Helper method. Takes care of child lists loosing their handlers.
        ''' </summary>
        Protected Overrides Function GetClone() As Object
            Dim result As TransferOfBalance = DirectCast(MyBase.GetClone(), TransferOfBalance)
            result.RestoreChildListsHandles()
            Return result
        End Function

        Protected Overrides Sub OnDeserialized(ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            RestoreChildListsHandles()
        End Sub

        Protected Overrides Sub UndoChangesComplete()
            MyBase.UndoChangesComplete()
            RestoreChildListsHandles()
        End Sub

        ''' <summary>
        ''' Helper method. Takes care of TaskTimeSpans loosing its handler. See GetClone method.
        ''' </summary>
        Friend Sub RestoreChildListsHandles()

            Try
                RemoveHandler _AnalyticsList.ListChanged, AddressOf AnalyticsList_Changed
                RemoveHandler _DebetList.ListChanged, AddressOf DebetList_Changed
                RemoveHandler _CreditList.ListChanged, AddressOf CreditList_Changed
            Catch ex As Exception
            End Try
            AddHandler _AnalyticsList.ListChanged, AddressOf AnalyticsList_Changed
            AddHandler _DebetList.ListChanged, AddressOf DebetList_Changed
            AddHandler _CreditList.ListChanged, AddressOf CreditList_Changed

        End Sub


        Public Function GetAllBrokenRules() As String
            Dim result As String = ""
            If Not MyBase.IsValid Then result = AddWithNewLine(result, _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error), False)
            If Not _DebetList.IsValid Then result = AddWithNewLine(result, _
                _DebetList.GetAllBrokenRules, False)
            If Not _CreditList.IsValid Then result = AddWithNewLine(result, _
                _CreditList.GetAllBrokenRules, False)
            If Not _AnalyticsList.IsValid Then result = AddWithNewLine(result, _
                _AnalyticsList.GetAllBrokenRules, False)
            Return result
        End Function

        Public Function GetAllWarnings() As String
            Dim result As String = ""
            If Not MyBase.BrokenRulesCollection.WarningCount > 0 Then result = AddWithNewLine(result, _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning), False)
            result = AddWithNewLine(result, _DebetList.GetAllWarnings, False)
            result = AddWithNewLine(result, _CreditList.GetAllWarnings, False)
            result = AddWithNewLine(result, _AnalyticsList.GetAllWarnings, False)
            Return result
        End Function


        Public Sub PasteDebetList(ByVal pasteString As String, ByVal accountsInfo As AccountInfoList, _
            ByVal overwrite As Boolean)

            If Not _ChronologicValidator.FinancialDataCanChange Then Throw New Exception( _
                "Klaida. Likučių perkėlimo duomenys negali būti keičiami:" _
                & vbCrLf & _ChronologicValidator.FinancialDataCanChangeExplanation)

            _DebetList.Paste(pasteString, Nothing, accountsInfo, overwrite)

        End Sub

        Public Sub PasteCreditList(ByVal pasteString As String, ByVal accountsInfo As AccountInfoList, _
            ByVal overwrite As Boolean)

            If Not _ChronologicValidator.FinancialDataCanChange Then Throw New Exception( _
                "Klaida. Likučių perkėlimo duomenys negali būti keičiami:" _
                & vbCrLf & _ChronologicValidator.FinancialDataCanChangeExplanation)

            _CreditList.Paste(pasteString, Nothing, accountsInfo, overwrite)

        End Sub

        Public Sub PasteAnalyticsList(ByVal pasteString As String, ByVal personsInfo As PersonInfoList, _
            ByVal accountsInfo As AccountInfoList, ByVal overwrite As Boolean)

            If Not _ChronologicValidator.FinancialDataCanChange Then Throw New Exception( _
                "Klaida. Likučių perkėlimo duomenys negali būti keičiami:" _
                & vbCrLf & _ChronologicValidator.FinancialDataCanChangeExplanation)

            _AnalyticsList.Paste(pasteString, personsInfo, accountsInfo, overwrite)

        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _ID
        End Function

        Public Overrides Function ToString() As String
            Return _Date.ToShortDateString & " likučių perkėlimas"
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("DebetSum", "perkeliama debeto korespondencijų suma"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("CreditSum", "perkeliama kredito korespondencijų suma"))
            ValidationRules.AddRule(AddressOf CommonValidation.ChronologyValidation, _
                New CommonValidation.ChronologyRuleArgs("Date", "ChronologicValidator"))

            ValidationRules.AddRule(AddressOf DebetEqualsCreditValidation, _
                New Validation.RuleArgs("DebetSum"))

            ValidationRules.AddDependantProperty("CreditSum", "DebetSum", False)

        End Sub

        ''' <summary>
        ''' Rule ensuring that Debet BookEntries.GetSum = Credit BookEntries.GetSum.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function DebetEqualsCreditValidation(ByVal target As Object, _
          ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As TransferOfBalance = DirectCast(target, TransferOfBalance)

            If Not CRound(ValObj._DebetSum) > 0 OrElse Not CRound(ValObj._CreditSum) > 0 Then Return True

            If CRound(ValObj._DebetSum) <> CRound(ValObj._CreditSum) Then
                e.Description = "Debeto korespondencijų suma nelygi kredito korespondencijų sumai."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return True

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite("General.TransferOfBalance3")
        End Sub

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.TransferOfBalance1")
        End Function

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.TransferOfBalance3")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.TransferOfBalance3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.TransferOfBalance3")
        End Function

#End Region

#Region " Factory Methods "

        Public Shared Function GetTransferOfBalance() As TransferOfBalance

            Return DataPortal.Fetch(Of TransferOfBalance)(New Criteria())

        End Function

        Public Shared Sub DeleteTransferOfBalance()
            DataPortal.Delete(New Criteria())
        End Sub

        Private Sub New()
            ' require use of factory methods
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Public Sub New()
            End Sub
        End Class

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims gauti.")

            _ChronologicValidator = TransferOfBalanceChronologicValidator. _
                GetTransferOfBalanceChronologicValidator

            If Not _ChronologicValidator.CurrentOperationID > 0 AndAlso _
                Not _ChronologicValidator.FinancialDataCanChange Then Throw New Exception( _
                "Klaida. Įtraukti naujos likučių perkėlimo operacijos neleidžiama. " _
                & _ChronologicValidator.FinancialDataCanChangeExplanation)

            _ID = _ChronologicValidator.CurrentOperationID

            If Not _ID > 0 Then

                _DebetList = BookEntryList.NewBookEntryList(BookEntryType.Debetas)
                _CreditList = BookEntryList.NewBookEntryList(BookEntryType.Kreditas)
                _AnalyticsList = TransferOfBalanceAnalyticsList. _
                    NewTransferOfBalanceAnalyticsList

                ValidationRules.CheckRules()

                Exit Sub

            End If

            Dim JE As JournalEntry = JournalEntry.GetJournalEntryChild(_ID, DocumentType.TransferOfBalance)

            _Date = JE.Date
            _InsertDate = JE.InsertDate
            _UpdateDate = JE.UpdateDate

            Dim CommonBookEntryList As BookEntryInternalList = _
                BookEntryInternalList.NewBookEntryInternalList(BookEntryType.Debetas)

            CommonBookEntryList.AddBookEntryLists(JE.DebetList, JE.CreditList)

            _AnalyticsList = TransferOfBalanceAnalyticsList.GetTransferOfBalanceAnalyticsList( _
                CommonBookEntryList, _ChronologicValidator.FinancialDataCanChange)

            CommonBookEntryList.Aggregate()

            _DebetList = BookEntryList.GetBookEntryList(CommonBookEntryList, BookEntryType.Debetas, _
                True, _ChronologicValidator.FinancialDataCanChange, _ChronologicValidator.FinancialDataCanChangeExplanation)
            _CreditList = BookEntryList.GetBookEntryList(CommonBookEntryList, BookEntryType.Kreditas, _
                True, _ChronologicValidator.FinancialDataCanChange, _ChronologicValidator.FinancialDataCanChangeExplanation)

            _DebetSum = _DebetList.GetSum
            _CreditSum = _CreditList.GetSum

            MarkOld()

        End Sub

        Protected Overrides Sub DataPortal_Insert()

            If Not CanEditObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims pakeisti.")

            DoSave()
            MarkOld()

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanEditObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims pakeisti.")

            DoSave()
            MarkOld()

        End Sub

        Private Sub DoSave()

            _ChronologicValidator = TransferOfBalanceChronologicValidator. _
                GetTransferOfBalanceChronologicValidator
            ValidationRules.CheckRules()
            If Not IsValid Then Throw New Exception("Dokumente yra klaidų:" _
                & vbCrLf & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error))

            If _ChronologicValidator.CurrentOperationID <> _ID Then Throw New Exception( _
                "Klaida. Pasikeitė likučių perkėlimo operacijos ID.")

            If Not _ID > 0 AndAlso Not _ChronologicValidator.FinancialDataCanChange Then _
                Throw New Exception("Likučių perkėlimo įrašas negali būti įtraukiamas. " _
                    & Me._ChronologicValidator.FinancialDataCanChangeExplanation)

            Dim result As JournalEntry
            If Not _ID > 0 Then
                result = JournalEntry.NewJournalEntryChild(DocumentType.TransferOfBalance)
                result.Content = "Apskaitos likučių perkėlimas"
                result.DocNumber = "Lik. Perk."
            Else
                result = JournalEntry.GetJournalEntryChild(_ID, DocumentType.TransferOfBalance)
                If result.UpdateDate <> _UpdateDate Then Throw New Exception( _
                    "Klaida. Dokumento atnaujinimo data pasikeitė. Teigtina, kad kitas " _
                    & "vartotojas redagavo šį objektą.")
            End If
            result.Date = _Date.Date

            If _ChronologicValidator.FinancialDataCanChange Then

                Dim CommonBookEntryList As BookEntryInternalList = _
                BookEntryInternalList.NewBookEntryInternalList(BookEntryType.Debetas)
                CommonBookEntryList.AddBookEntryLists(_DebetList, _CreditList)
                For Each i As BookEntryInternal In CommonBookEntryList
                    i.Person = Nothing
                Next
                _AnalyticsList.Update(CommonBookEntryList)
                CommonBookEntryList.Aggregate()

                result.DebetList.LoadBookEntryListFromInternalList(CommonBookEntryList, False)
                result.CreditList.LoadBookEntryListFromInternalList(CommonBookEntryList, False)

            End If

            If Not result.IsValid Then Throw New Exception( _
                "Klaida. Nepavyko generuoti bendrojo žurnalo įrašo: " & result.GetAllBrokenRules)

            result = result.SaveServerSide()

            _ID = result.ID
            _InsertDate = result.InsertDate
            _UpdateDate = result.UpdateDate

        End Sub

        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria())
        End Sub

        Protected Overrides Sub DataPortal_Delete(ByVal criteria As Object)

            If Not CanDeleteObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims pašalinti.")

            Dim validator As TransferOfBalanceChronologicValidator = _
                TransferOfBalanceChronologicValidator.GetTransferOfBalanceChronologicValidator

            If Not validator.CurrentOperationID > 0 Then
                Throw New Exception("Klaida. Likučių perkėlimo įrašas nerastas duomenų bazėje.")
            ElseIf Not validator.FinancialDataCanChange Then
                Throw New Exception("Klaida. Likučių perkėlimo operacijos pašalinti neleidžiama. " _
                    & validator.FinancialDataCanChangeExplanation)
            End If

            IndirectRelationInfoList.CheckIfJournalEntryCanBeDeleted(validator.CurrentOperationID, _
                DocumentType.TransferOfBalance)

            General.JournalEntry.DoDelete(validator.CurrentOperationID)

            MarkNew()

        End Sub

#End Region

    End Class

End Namespace