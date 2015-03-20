Namespace General

    ''' <summary>
    ''' Represents a single ledger operation that encompasses two or more <see cref="general.BookEntry">transactions</see>.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class JournalEntry
        Inherits BusinessBase(Of JournalEntry)
        Implements IIsDirtyEnough

#Region " Business Methods "

        Private _Guid As Guid = Guid.NewGuid
        Private _ID As Integer = 0
        Private _Date As Date = Today
        Private _DocNumber As String = ""
        Private _Content As String = ""
        Private _Person As PersonInfo = Nothing
        Private _DocType As DocumentType = DocumentType.None
        Private _DocTypeHumanReadable As String = ""
        Private WithEvents _DebetList As BookEntryList
        Private WithEvents _CreditList As BookEntryList
        Private _DebetSum As Double = 0
        Private _CreditSum As Double = 0
        Private _OldDate As Date = Today
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now
        Private _ChronologyValidator As IChronologicValidator = Nothing

        ' used to implement automatic sort in datagridview
        <NotUndoable()> _
        <NonSerialized()> _
        Private _DebetListSortedList As Csla.SortedBindingList(Of BookEntry) = Nothing
        ' used to implement automatic sort in datagridview
        <NotUndoable()> _
        <NonSerialized()> _
        Private _CreditListSortedList As Csla.SortedBindingList(Of BookEntry) = Nothing

        ''' <summary>
        ''' Gets an ID of the JournalEntry object (assigned by DB AUTO_INCREMENT).
        ''' </summary>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the operation was inserted into the database.
        ''' </summary>
        Public ReadOnly Property InsertDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _InsertDate
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the operation was last updated.
        ''' </summary>
        Public ReadOnly Property UpdateDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UpdateDate
            End Get
        End Property

        ''' <summary>
        ''' Gets <see cref="IChronologicValidator">IChronologicValidator</see> object that contains business restraints on updating the operation.
        ''' </summary>
        Public ReadOnly Property ChronologyValidator() As IChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ChronologyValidator
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a date of the Journal Entry.
        ''' </summary>
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

        ''' <summary>
        ''' Gets or sets a number of the document associated with the Journal Entry.
        ''' </summary>
        Public Property DocNumber() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DocNumber.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _DocNumber.Trim <> value.Trim Then
                    _DocNumber = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a content/description of the the Journal Entry.
        ''' </summary>
        Public Property Content() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Content.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Content.Trim <> value.Trim Then
                    _Content = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a person associated with the Journal Entry.
        ''' </summary>
        ''' <remarks>Use <see cref="HelperLists.PersonInfoList">PersonInfoList</see> for the datasource.</remarks>
        Public Property Person() As PersonInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Person
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As PersonInfo)
                CanWriteProperty(True)
                If Not (_Person Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _Person Is Nothing AndAlso Not value Is Nothing AndAlso _Person = value) Then
                    _Person = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets a <see cref="DocumentType">DocumentType</see> of the document associated with the Journal Entry (as enum).
        ''' </summary>
        Public ReadOnly Property DocType() As DocumentType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DocType
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="DocumentType">DocumentType</see> of the document associated with the Journal Entry 
        ''' (as a human readable string).
        ''' </summary>
        Public ReadOnly Property DocTypeHumanReadable() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DocTypeHumanReadable.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a BookEntryList of type Debit in the JuornalEntry. 
        ''' </summary>
        Public ReadOnly Property DebetList() As BookEntryList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DebetList
            End Get
        End Property

        ''' <summary>
        ''' Gets a BookEntryList of type Credit in the JournalEntry. 
        ''' </summary>
        Public ReadOnly Property CreditList() As BookEntryList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CreditList
            End Get
        End Property

        ''' <summary>
        ''' Gets a sortable BookEntryList of type Debet in the JuornalEntry. 
        ''' </summary>
        Public ReadOnly Property DebetListSorted() As Csla.SortedBindingList(Of BookEntry)
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _DebetListSortedList Is Nothing Then _DebetListSortedList = _
                    New Csla.SortedBindingList(Of BookEntry)(_DebetList)
                Return _DebetListSortedList
            End Get
        End Property

        ''' <summary>
        ''' Gets a sortable BookEntryList of type Credit in the JuornalEntry. 
        ''' </summary>
        Public ReadOnly Property CreditListSorted() As Csla.SortedBindingList(Of BookEntry)
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _CreditListSortedList Is Nothing Then _CreditListSortedList = _
                    New Csla.SortedBindingList(Of BookEntry)(_CreditList)
                Return _CreditListSortedList
            End Get
        End Property

        ''' <summary>
        ''' Gets a total sum of ammounts/values of all the BookEntryList of type Debit in the JuornalEntry. 
        ''' </summary>
        Public ReadOnly Property DebetSum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_DebetSum)
            End Get
        End Property

        ''' <summary>
        ''' Gets a total sum of ammounts/values of all the BookEntryList of type Credit in the JuornalEntry. 
        ''' </summary>
        Public ReadOnly Property CreditSum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CreditSum)
            End Get
        End Property

        ''' <summary>
        ''' Gets an original date of the JournalEntry, i.e. befor user changes. 
        ''' </summary>
        Public ReadOnly Property OldDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OldDate
            End Get
        End Property

        ''' <summary>
        ''' Returnes TRUE if the object is new and contains some user provided data 
        ''' OR
        ''' object is not new and was changed by the user.
        ''' </summary>
        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return (Not String.IsNullOrEmpty(_DocNumber.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Content.Trim) _
                    OrElse _DebetList.Count > 0 OrElse _CreditList.Count > 0)
            End Get
        End Property


        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _DebetList.IsDirty OrElse _CreditList.IsDirty
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return MyBase.IsValid AndAlso _DebetList.IsValid AndAlso _CreditList.IsValid
            End Get
        End Property



        Public Overrides Function Save() As JournalEntry

            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            Return MyBase.Save

        End Function

        ''' <summary>
        ''' Save child JournalEntry to database.
        ''' </summary>
        ''' <returns>Saved JournalEntry (incl. ID).</returns>
        ''' <remarks>Should only be invoked server side.</remarks>
        Friend Function SaveChild() As JournalEntry

            If _DocType = DocumentType.None Then
                Throw New InvalidOperationException(My.Resources.General_JournalEntry_UnexpectedChildType)
            End If

            If Not Me.IsValid Then
                Throw New InvalidOperationException(String.Format(My.Resources.Common_ContainsErrors, _
                    vbCrLf, GetAllBrokenRules()))
            End If

            Dim result As JournalEntry = Me.Clone

            If result.IsNew Then
                result.DoInsert()
            Else
                result.DoUpdate()
            End If

            Return result

        End Function


        Private Sub DebetList_Changed(ByVal sender As Object, _
            ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _DebetList.ListChanged
            _DebetSum = _DebetList.GetSum
            PropertyHasChanged("DebetSum")
        End Sub

        Private Sub CreditList_Changed(ByVal sender As Object, _
            ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _CreditList.ListChanged
            _CreditSum = _CreditList.GetSum
            PropertyHasChanged("CreditSum")
        End Sub

        ''' <summary>
        ''' Helper method. Takes care of child lists loosing their handlers.
        ''' </summary>
        Protected Overrides Function GetClone() As Object
            Dim result As JournalEntry = DirectCast(MyBase.GetClone(), JournalEntry)
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
        ''' Helper method. Takes care of DebetList and CreditList loosing their handlers. See GetClone method.
        ''' </summary>
        Friend Sub RestoreChildListsHandles()

            Try
                RemoveHandler _DebetList.ListChanged, AddressOf DebetList_Changed
                RemoveHandler _CreditList.ListChanged, AddressOf CreditList_Changed
            Catch ex As Exception
            End Try
            AddHandler _DebetList.ListChanged, AddressOf DebetList_Changed
            AddHandler _CreditList.ListChanged, AddressOf CreditList_Changed

        End Sub


        ''' <summary>
        ''' Gets a list of debet and credit book entries formated as e.g. "D 2711, K 443". 
        ''' </summary>
        Public Function GetCorrespondentionsString() As String

            Dim resultList As New List(Of String)
            resultList.Add(_DebetList.GetEntryListString)
            resultList.Add(_CreditList.GetEntryListString)

            Dim result As String = String.Join(", ", resultList.ToArray)

            If result.Trim.Length > 254 Then result = result.Trim.Substring(0, 249) & "<...>"

            Return result

        End Function

        ''' <summary>
        ''' Loads JournalEntryTemplate data (content and corespondences) to JournalEntry.
        ''' Clears current corespondences. In case of an old JournalEntry 
        ''' cleared corespondences are moved to DeletedList.
        ''' </summary>
        ''' <param name="EntryTemplate">JournalEntryTemplate object containing the data to load.</param>
        Public Sub LoadJournalEntryFromTemplate(ByVal EntryTemplate As HelperLists.TemplateJournalEntryInfo)

            Content = EntryTemplate.Content
            _DebetList.LoadBookEntryListFromTemplate(EntryTemplate.DebetListString, True)
            _CreditList.LoadBookEntryListFromTemplate(EntryTemplate.CreditListString, True)

        End Sub


        Public Function GetAllBrokenRules() As String
            Dim result As String = ""
            If Not MyBase.IsValid Then result = AddWithNewLine(result, _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error), False)
            If Not _DebetList.IsValid Then result = AddWithNewLine(result, _
                _DebetList.GetAllBrokenRules, False)
            If Not _CreditList.IsValid Then result = AddWithNewLine(result, _
                _CreditList.GetAllBrokenRules, False)
            Return result
        End Function

        Public Function GetAllWarnings() As String
            Dim result As String = ""
            If Me.BrokenRulesCollection.WarningCount > 0 Then
                result = AddWithNewLine(result, Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning), False)
            End If
            If _DebetList.HasWarnings Then
                result = AddWithNewLine(result, _DebetList.GetAllWarnings, False)
            End If
            If _CreditList.HasWarnings Then
                result = AddWithNewLine(result, _CreditList.GetAllWarnings, False)
            End If
            Return result
        End Function

        Public Function HasWarnings() As Boolean
            Return Me.BrokenRulesCollection.WarningCount > 0 OrElse _DebetList.HasWarnings _
                OrElse _CreditList.HasWarnings
        End Function


        Protected Overrides Function GetIdValue() As Object
            If IsNew Then
                Return _Guid.ToString()
            Else
                Return _ID.ToString
            End If
        End Function

        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.General_JournalEntry_ToString, _
                IIf(IsNew, "Naujas", "Esamas"), _ID.ToString, _Date.ToShortDateString, _
                _DocNumber, _Content, _DebetList.ToString(), _CreditList.ToString())
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.ChronologyValidation, _
                New CommonValidation.ChronologyRuleArgs("Date", "ChronologyValidator"))
            ValidationRules.AddRule(AddressOf CommonValidation.FutureDate, _
                New CommonValidation.SimpleRuleArgs("Date", My.Resources.General_JournalEntry_Date, _
                Validation.RuleSeverity.Warning))

            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("DocNumber", My.Resources.General_JournalEntry_DocNumber))
            ValidationRules.AddRule(AddressOf CommonValidation.StringMaxLength, _
                New CommonValidation.StringMaxLengthRuleArgs("DocNumber", 30, My.Resources.General_JournalEntry_DocNumber, _
                Validation.RuleSeverity.Warning))

            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("Content", My.Resources.General_JournalEntry_Content))
            ValidationRules.AddRule(AddressOf CommonValidation.StringMaxLength, _
                New CommonValidation.StringMaxLengthRuleArgs("Content", 255, My.Resources.General_JournalEntry_Content, _
                Validation.RuleSeverity.Warning))

            ValidationRules.AddRule(AddressOf CommonValidation.InfoObjectRequired, _
                New CommonValidation.InfoObjectRequiredRuleArgs("Person", My.Resources.General_JournalEntry_Person, _
                "ID", Validation.RuleSeverity.Warning))

            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("DebetSum", My.Resources.General_JournalEntry_DebetSum))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("CreditSum", My.Resources.General_JournalEntry_CreditSum))
            
            ValidationRules.AddRule(AddressOf DebetEqualsCreditValidation, New Validation.RuleArgs("DebetSum"))

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

            Dim ValObj As JournalEntry = DirectCast(target, JournalEntry)

            If Not CRound(ValObj._DebetSum) > 0 OrElse Not CRound(ValObj._CreditSum) > 0 Then Return True

            If CRound(ValObj._DebetSum) <> CRound(ValObj._CreditSum) Then
                e.Description = My.Resources.General_JournalEntry_DebitNotEqualsCredit
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return True

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite("General.JournalEntry2")
        End Sub

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.JournalEntry1")
        End Function

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.JournalEntry2")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.JournalEntry3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("General.JournalEntry3")
        End Function

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets as new JournalEntry as a standalone business object.
        ''' </summary>
        Public Shared Function NewJournalEntry() As JournalEntry
            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)
            Return New JournalEntry(DocumentType.None)
        End Function

        ''' <summary>
        ''' Gets as new JournalEntry as a part of another business object.
        ''' </summary>
        ''' <param name="AssociatedDocumentType"><see cref="DocumentType">Type</see> of parent business object.</param>
        ''' <remarks>Should only be called on server side.</remarks>
        Friend Shared Function NewJournalEntryChild(ByVal associatedDocumentType As DocumentType) As JournalEntry
            If associatedDocumentType = DocumentType.None Then Throw New InvalidOperationException( _
                My.Resources.General_JournalEntry_UnexpectedChildType)
            Return New JournalEntry(associatedDocumentType)
        End Function

        ''' <summary>
        ''' Gets an existing JournalEntry as a standalone business object.
        ''' </summary>
        ''' <param name="nID"><see cref="ID">ID</see> of the JournalEntry.</param>
        Public Shared Function GetJournalEntry(ByVal nID As Integer) As JournalEntry
            Return DataPortal.Fetch(Of JournalEntry)(New Criteria(nID))
        End Function

        ''' <summary>
        ''' Gets an existing JournalEntry as a part of another business object.
        ''' </summary>
        ''' <param name="JournalEntryID"><see cref="ID">ID</see> of the JournalEntry.</param>
        ''' <param name="ExpectedType"><see cref="DocumentType">Type</see> of parent business object.</param>
        ''' <remarks>Should only be called on server side.</remarks>
        Friend Shared Function GetJournalEntryChild(ByVal journalEntryID As Integer, _
            ByVal expectedType As DocumentType) As JournalEntry

            If expectedType = DocumentType.None Then Throw New InvalidOperationException( _
                My.Resources.General_JournalEntry_UnexpectedChildType)

            Dim result As JournalEntry = New JournalEntry(journalEntryID)

            If result._DocType <> expectedType Then Throw New InvalidOperationException( _
                String.Format(My.Resources.General_JournalEntry_UnexpectedParentType, _
                journalEntryID.ToString, result._DocTypeHumanReadable, _
                ConvertEnumHumanReadable(expectedType)))

            Return result

        End Function

        ''' <summary>
        ''' Deletes an existing JournalEntry from database.
        ''' </summary>
        ''' <param name="id"><see cref="ID">ID</see> of the JournalEntry.</param>
        Public Shared Sub DeleteJournalEntry(ByVal id As Integer)
            DataPortal.Delete(New Criteria(id))
        End Sub

        ''' <summary>
        ''' Deletes an existing JournalEntry from database.
        ''' </summary>
        ''' <param name="id"><see cref="ID">ID</see> of the JournalEntry.</param>
        Friend Shared Sub DeleteJournalEntryChild(ByVal id As Integer)
            DoDelete(id)
        End Sub


        Private Sub New()
            ' require use of factory methods
        End Sub

        Private Sub New(ByVal AssociatedDocumentType As DocumentType)
            If AssociatedDocumentType <> DocumentType.None Then MarkAsChild()
            Create(AssociatedDocumentType)
        End Sub

        Private Sub New(ByVal JournalEntryID As Integer)
            MarkAsChild()
            DoFetch(JournalEntryID)
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private _ID As Integer
            Public ReadOnly Property ID() As Integer
                Get
                    Return _ID
                End Get
            End Property
            Public Sub New(ByVal nID As Integer)
                _ID = nID
            End Sub
        End Class


        Private Sub Create(ByVal AssociatedDocumentType As DocumentType)

            _DocType = AssociatedDocumentType
            _DocTypeHumanReadable = ConvertEnumHumanReadable(AssociatedDocumentType)
            _DebetList = BookEntryList.NewBookEntryList(BookEntryType.Debetas)
            _CreditList = BookEntryList.NewBookEntryList(BookEntryType.Kreditas)
            _ChronologyValidator = SimpleChronologicValidator.NewSimpleChronologicValidator( _
                My.Resources.General_JournalEntry_TypeName)

            ValidationRules.CheckRules()

        End Sub


        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecuritySelectDenied)
            DoFetch(criteria.ID)
            If _DocType <> DocumentType.None Then MarkAsChild()
        End Sub

        Private Sub DoFetch(ByVal JournalEntryID As Integer)

            Dim myComm As New SQLCommand("JournalEntryFetch")
            myComm.AddParam("?BD", JournalEntryID)

            Using myData As DataTable = myComm.Fetch

                If Not myData.Rows.Count > 0 Then Throw New Exception("Klaida. Objektas, kurio ID='" _
                    & JournalEntryID.ToString & "', nerastas.)")

                Dim dr As DataRow = myData.Rows(0)

                _ID = JournalEntryID
                _Date = CDateSafe(dr.Item(0), Today)
                _OldDate = _Date
                _DocNumber = CStrSafe(dr.Item(1)).Trim
                _Content = CStrSafe(dr.Item(2)).Trim
                _DocType = ConvertEnumDatabaseStringCode(Of DocumentType)(CStrSafe(dr.Item(3)).Trim)
                _DocTypeHumanReadable = ConvertEnumHumanReadable(_DocType)
                _InsertDate = CTimeStampSafe(dr.Item(4))
                _UpdateDate = CTimeStampSafe(dr.Item(5))
                _Person = HelperLists.PersonInfo.GetPersonInfo(dr, 6)

            End Using

            _ChronologyValidator = SimpleChronologicValidator.GetSimpleChronologicValidator(_ID)

            myComm = New SQLCommand("BookEntriesFetch")
            myComm.AddParam("?BD", JournalEntryID)

            Using myData As DataTable = myComm.Fetch
                _DebetList = BookEntryList.GetBookEntryList(myData, BookEntryType.Debetas, _
                    _ChronologyValidator.FinancialDataCanChange, _
                    _ChronologyValidator.FinancialDataCanChangeExplanation)
                _CreditList = BookEntryList.GetBookEntryList(myData, BookEntryType.Kreditas, _
                    _ChronologyValidator.FinancialDataCanChange, _
                    _ChronologyValidator.FinancialDataCanChangeExplanation)
            End Using

            _DebetSum = _DebetList.GetSum
            _CreditSum = _CreditList.GetSum

            MarkOld()

            ValidationRules.CheckRules()

        End Sub


        Protected Overrides Sub DataPortal_Insert()

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            If _DocType <> DocumentType.None Then Throw New InvalidOperationException( _
                My.Resources.General_JournalEntry_InvalidTypeOnSave)

            Me.ValidationRules.CheckRules()
            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            DoInsert()

        End Sub

        Private Sub DoInsert()

            Dim TransactionExisted As Boolean = DatabaseAccess.TransactionExists
            If Not TransactionExisted Then DatabaseAccess.TransactionBegin()

            Dim myComm As New SQLCommand("JournalEntryInsert")
            AddWithParams(myComm)

            myComm.Execute()

            _ID = Convert.ToInt32(myComm.LastInsertID)

            _DebetList.Update(Me)
            _CreditList.Update(Me)

            If Not TransactionExisted Then DatabaseAccess.TransactionCommit()

            MarkOld()

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanEditObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityUpdateDenied)

            If _DocType <> DocumentType.None Then Throw New InvalidOperationException( _
                My.Resources.General_JournalEntry_InvalidTypeOnSave)

            Me.ValidationRules.CheckRules()
            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            CheckIfUpdateDateChanged()

            DoUpdate()

        End Sub

        Private Sub DoUpdate()

            Dim TransactionExisted As Boolean = DatabaseAccess.TransactionExists
            If Not TransactionExisted Then DatabaseAccess.TransactionBegin()

            Dim myComm As New SQLCommand("JournalEntryUpdate")
            AddWithParams(myComm)
            myComm.AddParam("?BD", _ID)

            myComm.Execute()

            If _ChronologyValidator.FinancialDataCanChange Then
                _DebetList.Update(Me)
                _CreditList.Update(Me)
            End If

            If Not TransactionExisted Then DatabaseAccess.TransactionCommit()

            MarkOld()

        End Sub


        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_ID))
        End Sub

        Protected Overrides Sub DataPortal_Delete(ByVal criteria As Object)

            If Not CanDeleteObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityUpdateDenied)

            Dim IndirectRelations As IndirectRelationInfoList = IndirectRelationInfoList. _
                GetIndirectRelationInfoListServerSide(DirectCast(criteria, Criteria).ID)

            IndirectRelations.CheckIfJournalEntryCanBeDeleted(New DocumentType() _
                {DocumentType.None, DocumentType.ClosingEntries})

            CheckIfDirectDeletionIsPossible(IndirectRelations.DocType, True)

            DoDelete(DirectCast(criteria, Criteria).ID)

            ' Last closing date is part of CompanyInfo object in GlobalContext
            If IndirectRelations.DocType = DocumentType.ClosingEntries Then _
                ApskaitaObjects.Settings.CompanyInfo.LoadCompanyInfoToGlobalContext("", "")

        End Sub

        Private Shared Sub DoDelete(ByVal JournalEntryID As Integer)

            Dim TransactionExisted As Boolean = DatabaseAccess.TransactionExists
            If Not TransactionExisted Then DatabaseAccess.TransactionBegin()

            Dim myComm As New SQLCommand("JournalEntryDelete")
            myComm.AddParam("?BD", JournalEntryID)
            myComm.Execute()

            Dim myComm2 As New SQLCommand("BookEntryClear")
            myComm2.AddParam("?BD", JournalEntryID)
            myComm2.Execute()

            If Not TransactionExisted Then DatabaseAccess.TransactionCommit()

        End Sub


        Private Sub AddWithParams(ByRef myComm As SQLCommand)

            myComm.AddParam("?AA", _Date.Date)
            myComm.AddParam("?AB", _DocNumber.Trim)
            myComm.AddParam("?AC", _Content.Trim)
            myComm.AddParam("?AD", ConvertEnumDatabaseStringCode(_DocType))
            If Not _Person Is Nothing AndAlso _Person.ID > 0 Then
                myComm.AddParam("?AE", _Person.ID)
            Else
                myComm.AddParam("?AE", 0)
            End If
            myComm.AddParam("?AF", Me.GetCorrespondentionsString)

            _UpdateDate = GetCurrentTimeStamp()
            If Me.IsNew Then _InsertDate = _UpdateDate

            myComm.AddParam("?AG", _UpdateDate.ToUniversalTime)

        End Sub

        Private Sub CheckIfUpdateDateChanged()

            Dim myComm As New SQLCommand("CheckIfJournalEntryUpdateDateChanged")
            myComm.AddParam("?CD", _ID)

            Using myData As DataTable = myComm.Fetch

                If myData.Rows.Count < 1 OrElse CDateTimeSafe(myData.Rows(0).Item(0), _
                    Date.MinValue) = Date.MinValue Then

                    Throw New Exception(String.Format(My.Resources.Common_ObjectNotFound, _
                        My.Resources.General_JournalEntry_TypeName, _ID.ToString))

                End If

                If CTimeStampSafe(myData.Rows(0).Item(0)) <> _UpdateDate Then

                    Throw New Exception(My.Resources.Common_UpdateDateHasChanged)

                End If

            End Using

        End Sub

        ''' <summary>
        ''' Indicates if the JournalEntry can be deleted as standalone document, 
        ''' i.e. is not a part of some complex document, e.g. invoice. 
        ''' </summary>
        Private Shared Function CheckIfDirectDeletionIsPossible(ByVal associatedDocumentType As DocumentType, _
            ByVal throwOnNotPossible As Boolean) As Boolean

            ' allowed types
            If associatedDocumentType = DocumentType.ClosingEntries _
                OrElse associatedDocumentType = DocumentType.None _
                OrElse associatedDocumentType = DocumentType.TransferOfBalance Then
                Return True
            End If

            ' disallowed types
            Dim ExceptionMessage As String = ""

            If associatedDocumentType = DocumentType.AdvanceReport OrElse _
                associatedDocumentType = DocumentType.InvoiceMade OrElse _
                associatedDocumentType = DocumentType.InvoiceReceived OrElse _
                associatedDocumentType = DocumentType.AccumulatedCosts OrElse _
                associatedDocumentType = DocumentType.BankOperation OrElse _
                associatedDocumentType = DocumentType.Offset OrElse _
                associatedDocumentType = DocumentType.TillIncomeOrder OrElse _
                associatedDocumentType = DocumentType.TillSpendingOrder Then

                ExceptionMessage = String.Format(My.Resources.General_JournalEntry_OnDeletingDocumentModule, _
                    ConvertEnumHumanReadable(associatedDocumentType))

            ElseIf associatedDocumentType = DocumentType.Amortization OrElse _
                associatedDocumentType = DocumentType.LongTermAssetAccountChange OrElse _
                associatedDocumentType = DocumentType.LongTermAssetDiscard Then

                ExceptionMessage = String.Format(My.Resources.General_JournalEntry_OnDeletingAssetsModule, _
                    ConvertEnumHumanReadable(associatedDocumentType))

            ElseIf associatedDocumentType = DocumentType.GoodsAccountChange OrElse _
                associatedDocumentType = DocumentType.GoodsInternalTransfer OrElse _
                associatedDocumentType = DocumentType.GoodsInventorization OrElse _
                associatedDocumentType = DocumentType.GoodsProduction OrElse _
                associatedDocumentType = DocumentType.GoodsRevalue OrElse _
                associatedDocumentType = DocumentType.GoodsWriteOff Then

                ExceptionMessage = String.Format(My.Resources.General_JournalEntry_OnDeletingGoodsModule, _
                    ConvertEnumHumanReadable(associatedDocumentType))

            ElseIf associatedDocumentType = DocumentType.ImprestSheet OrElse _
                associatedDocumentType = DocumentType.WageSheet Then

                ExceptionMessage = String.Format(My.Resources.General_JournalEntry_OnDeletingWorkersModule, _
                    ConvertEnumHumanReadable(associatedDocumentType))

            Else

                Throw New NotImplementedException(String.Format(My.Resources.Common_DocumentTypeNotImplemented, _
                    associatedDocumentType.ToString, GetType(JournalEntry).FullName, "DirectDeletionIsPossible"))

            End If

            If throwOnNotPossible Then Throw New Exception(ExceptionMessage)

            Return False

        End Function

#End Region

    End Class

End Namespace