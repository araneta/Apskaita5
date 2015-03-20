Namespace Documents

    <Serializable()> _
    Public Class AccumulativeCostsItem
        Inherits BusinessBase(Of AccumulativeCostsItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private _Guid As Guid = Guid.NewGuid
        Private _ID As Integer = 0
        Private _Date As Date = Today
        Private _Sum As Double = 0
        Private _ChronologyValidator As SimpleChronologicValidator


        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property ChronologyValidator() As SimpleChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ChronologyValidator
            End Get
        End Property

        Public ReadOnly Property SumIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _ChronologyValidator Is Nothing AndAlso Not _ChronologyValidator.FinancialDataCanChange
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

        Public Property Sum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Sum)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If SumIsReadOnly Then Exit Property
                If CRound(_Sum) <> CRound(value) Then
                    _Sum = CRound(value)
                    PropertyHasChanged()
                End If
            End Set
        End Property



        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If IsValid Then Return ""
            Return "Klaida (-os) eilutėje '" & Me.ToString & "': " & _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error)
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If BrokenRulesCollection.WarningCount < 1 Then Return ""
            Return "Sąskaitos eilutėje '" & Me.ToString & "' gali būti klaida: " & _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning)
        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return _Date.ToString("yyyy-MM-dd") & " - " & Sum.ToString
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.ChronologyValidation, _
                New CommonValidation.ChronologyRuleArgs("Date", "ChronologyValidator"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("Sum", "laikotarpiui tenkanti suma"))

        End Sub

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function NewAccumulativeCostsItem() As AccumulativeCostsItem
            Dim result As New AccumulativeCostsItem
            result._ChronologyValidator = SimpleChronologicValidator. _
                NewSimpleChronologicValidator("sukauptų sąnaudų paskirstymo eilutė")
            result.ValidationRules.CheckRules()
            Return result
        End Function

        Friend Shared Function GetAccumulativeCostsItem(ByVal dr As DataRow, _
            ByVal closingsDataSource As DataTable) As AccumulativeCostsItem
            Return New AccumulativeCostsItem(dr, closingsDataSource)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal closingsDataSource As DataTable)
            MarkAsChild()
            Fetch(dr, closingsDataSource)
        End Sub

#End Region

#Region " Data Access "

        <NotUndoable()> _
        <NonSerialized()> _
        Private childJournalEntry As General.JournalEntry = Nothing


        Private Sub Fetch(ByVal dr As DataRow, ByVal closingsDataSource As DataTable)

            _ID = CIntSafe(dr.Item(0), 0)
            _Date = CDateSafe(dr.Item(1), Today)
            _Sum = CDblSafe(dr.Item(2), 2, 0)

            _ChronologyValidator = SimpleChronologicValidator.GetSimpleChronologicValidator( _
                closingsDataSource, _ID, _Date, "sukauptų sąnaudų paskirstymo eilutė")

            MarkOld()

        End Sub

        Friend Sub Insert(ByVal parent As AccumulativeCosts)

            childJournalEntry = childJournalEntry.SaveChild

            _ID = childJournalEntry.ID

            Dim myComm As New SQLCommand("InsertAccumulativeCostsItem")
            AddWithParams(myComm)
            myComm.AddParam("?PD", parent.ID)
            myComm.AddParam("?BD", _ID)

            myComm.Execute()

            MarkOld()

        End Sub

        Friend Sub Update(ByVal parent As AccumulativeCosts)

            childJournalEntry = childJournalEntry.SaveChild

            Dim myComm As New SQLCommand("UpdateAccumulativeCostsItem")
            myComm.AddParam("?CD", _ID)
            AddWithParams(myComm)

            myComm.Execute()

            MarkOld()

        End Sub

        Friend Sub DeleteSelf()

            General.JournalEntry.DeleteJournalEntryChild(_ID)

            Dim myComm As New SQLCommand("DeleteAccumulativeCostsItem")
            myComm.AddParam("?CD", _ID)

            myComm.Execute()

            MarkNew()

        End Sub


        Friend Sub PrepareChildJournalEntry(ByVal parent As AccumulativeCosts)

            Dim result As General.JournalEntry
            If IsNew Then
                result = General.JournalEntry.NewJournalEntryChild(DocumentType.AccumulatedCosts)
            Else
                result = General.JournalEntry.GetJournalEntryChild(_ID, DocumentType.AccumulatedCosts)
            End If

            result.Content = parent.Content
            result.Date = _Date
            result.DocNumber = parent.DocumentNumber

            If _ChronologyValidator.FinancialDataCanChange Then

                If result.DebetList.Count <> 1 Then
                    result.DebetList.Clear()
                    result.DebetList.Add(General.BookEntry.NewBookEntry())
                End If

                result.DebetList(0).Amount = CRound(_Sum)

                If result.CreditList.Count <> 1 Then
                    result.CreditList.Clear()
                    result.CreditList.Add(General.BookEntry.NewBookEntry())
                End If

                result.CreditList(0).Amount = CRound(_Sum)

                If parent.ApplicableIsAccumulatedIncome Then
                    result.DebetList(0).Account = parent.AccountAccumulatedCosts
                    result.CreditList(0).Account = parent.AccountDistributedCosts
                Else
                    result.DebetList(0).Account = parent.AccountDistributedCosts
                    result.CreditList(0).Account = parent.AccountAccumulatedCosts
                End If

            End If

            childJournalEntry = result

        End Sub

        Private Sub AddWithParams(ByRef myComm As SQLCommand)
            myComm.AddParam("?AA", _Date.Date)
            myComm.AddParam("?AB", CRound(_Sum))
        End Sub

#End Region

    End Class

End Namespace