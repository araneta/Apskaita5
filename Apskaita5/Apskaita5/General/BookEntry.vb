Namespace General

    <Serializable()> _
    Public Class BookEntry
        Inherits BusinessBase(Of BookEntry)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Friend Const ColumnCount As Integer = 3
        Public Const ColumnSequence As String = "Kontavimo duomenys privalo būti išdėstyti " _
            & "3 stulpeliais: sąskaita, suma ir asmens ar įmonės kodas (neprivalomas)."

        Private _Guid As Guid = Guid.NewGuid
        Private _ID As Integer = 0
        Private _FinancialDataCanChange As Boolean = True
        Private _Account As Long = 0
        Private _Amount As Double = 0
        Private _Person As PersonInfo = Nothing


        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property FinancialDataCanChange() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _FinancialDataCanChange
            End Get
        End Property

        Public Property Account() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Account
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If Not _FinancialDataCanChange Then Exit Property
                If value < 0 Then value = 0
                If _Account <> value Then
                    _Account = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property Amount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Amount)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If Not _FinancialDataCanChange Then Exit Property
                If CRound(value) < 0 Then value = 0
                If CRound(_Amount) <> CRound(value) Then
                    _Amount = CRound(value)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property Person() As PersonInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Person
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As PersonInfo)
                CanWriteProperty(True)
                If Not _FinancialDataCanChange Then Exit Property
                If Not (_Person Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _Person Is Nothing AndAlso Not value Is Nothing AndAlso _Person = value) Then
                    _Person = value
                    PropertyHasChanged()
                End If
            End Set
        End Property


        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If IsValid Then Return ""
            Return "Klaida (-os) eilutėje '" & Me.ToString & "': " _
                & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error)
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If BrokenRulesCollection.WarningCount < 1 Then Return ""
            Return "Eilutėje '" & Me.ToString & "' gali būti klaida: " _
                & Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning)
        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            If Not _ID > 0 Then Return ""
            If Not Parent Is Nothing Then Return ConvertEnumHumanReadable(DirectCast(Parent, BookEntryList).Type) _
                & " " & _Account.ToString & " - " & _Amount.ToString & " " & GetCurrentCompany.BaseCurrency
            Return _Account.ToString & " - " & _Amount.ToString & " " & GetCurrentCompany.BaseCurrency
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("Account", "sąskaitos numeris"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("Amount", "korespondencijos vertė"))

        End Sub

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function NewBookEntry() As BookEntry
            Dim result As New BookEntry
            result.ValidationRules.CheckRules()
            Return result
        End Function

        Friend Shared Function NewBookEntry(ByVal BookEntryInternalItem As BookEntryInternal) As BookEntry
            Return New BookEntry(BookEntryInternalItem, False)
        End Function

        Friend Shared Function NewBookEntry(ByVal pasteString As String, _
            ByVal personsInfo As PersonInfoList, ByVal accountsInfo As AccountInfoList) As BookEntry
            Return New BookEntry(pasteString, personsInfo, accountsInfo)
        End Function

        Friend Shared Function GetBookEntry(ByVal dr As DataRow, _
            ByVal nFinancialDataCanChange As Boolean) As BookEntry
            Return New BookEntry(dr, nFinancialDataCanChange)
        End Function

        ''' <summary>
        ''' Only for object TransferOfBalance.
        ''' </summary>
        ''' <remarks>Only for object TransferOfBalance.</remarks>
        Friend Shared Function GetBookEntry(ByVal nBookEntryInternal As BookEntryInternal, _
            ByVal MarkItemAsOld As Boolean) As BookEntry
            Return New BookEntry(nBookEntryInternal, MarkItemAsOld)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub


        Private Sub New(ByVal dr As DataRow, ByVal nFinancialDataCanChange As Boolean)
            MarkAsChild()
            Fetch(dr, nFinancialDataCanChange)
        End Sub

        Private Sub New(ByVal nBookEntryInternal As BookEntryInternal, ByVal MarkItemAsOld As Boolean)
            MarkAsChild()
            Create(nBookEntryInternal, MarkItemAsOld)
        End Sub

        Private Sub New(ByVal pasteString As String, ByVal personsInfo As PersonInfoList, _
            ByVal accountsInfo As AccountInfoList)
            MarkAsChild()
            Create(pasteString, personsInfo, accountsInfo)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Create(ByVal nBookEntryInternal As BookEntryInternal, ByVal MarkItemAsOld As Boolean)

            _Amount = nBookEntryInternal.Ammount
            _Account = nBookEntryInternal.Account
            _Person = nBookEntryInternal.Person

            If MarkItemAsOld Then MarkOld()

            ValidationRules.CheckRules()

        End Sub

        Private Sub Create(ByVal pasteString As String, ByVal personsInfo As PersonInfoList, _
            ByVal accountsInfo As AccountInfoList)

            Long.TryParse(GetField(pasteString, vbTab, 0), _Account)
            Double.TryParse(GetField(pasteString, vbTab, 1), _Amount)

            If accountsInfo.GetAccountByID(_Account) Is Nothing Then _Account = 0

            If Not personsInfo Is Nothing Then
                Dim PersonCode As String = GetField(pasteString, vbTab, 2).Trim.ToLower
                If Not String.IsNullOrEmpty(PersonCode.Trim) Then _
                    _Person = personsInfo.GetPersonInfo(PersonCode)
            End If

            ValidationRules.CheckRules()

        End Sub

        Private Sub Fetch(ByVal dr As DataRow, ByVal nFinancialDataCanChange As Boolean)

            _ID = CIntSafe(dr.Item(0), 0)
            _Account = CLongSafe(dr.Item(2), 0)
            _Amount = CDblSafe(dr.Item(3), 2, 0)
            _Person = HelperLists.PersonInfo.GetPersonInfo(dr, 4)
            _FinancialDataCanChange = nFinancialDataCanChange

            MarkOld()

        End Sub

        Friend Sub Insert(ByVal parentList As BookEntryList, ByVal parent As JournalEntry)

            Dim myComm As New SQLCommand("BookEntryInsert")
            myComm.AddParam("?AA", Parent.ID)
            myComm.AddParam("?AB", ConvertEnumDatabaseStringCode(parentList.Type))
            AddWithParamsGeneral(myComm)
            AddWithParamsFinancial(myComm)

            myComm.Execute()

            _ID = myComm.LastInsertID

            MarkOld()

        End Sub

        Friend Sub Update(ByVal parentList As BookEntryList, ByVal parent As JournalEntry)

            Dim myComm As SQLCommand
            If parent.ChronologyValidator.FinancialDataCanChange Then
                myComm = New SQLCommand("UpdateBookEntry")
                AddWithParamsFinancial(myComm)
            Else
                myComm = New SQLCommand("UpdateBookEntryGeneral")
            End If
            myComm.AddParam("?BD", _ID)
            AddWithParamsGeneral(myComm)

            myComm.Execute()

            MarkOld()

        End Sub

        Friend Sub DeleteSelf()

            Dim myComm As New SQLCommand("DeleteBookEntry")
            myComm.AddParam("?BD", _ID)

            myComm.Execute()

            MarkNew()

        End Sub

        Private Sub AddWithParamsGeneral(ByRef myComm As SQLCommand)

            If Not _Person Is Nothing AndAlso _Person.ID > 0 Then
                myComm.AddParam("?AE", _Person.ID)
            Else
                myComm.AddParam("?AE", 0)
            End If

        End Sub

        Private Sub AddWithParamsFinancial(ByRef myComm As SQLCommand)

            myComm.AddParam("?AC", _Account)
            myComm.AddParam("?AD", CRound(_Amount))

        End Sub

#End Region

    End Class

End Namespace