Namespace Goods

    <Serializable()> _
    Public Class GoodsComplexOperationInternalTransfer
        Inherits BusinessBase(Of GoodsComplexOperationInternalTransfer)
        Implements IIsDirtyEnough

#Region " Business Methods "

        Private _ID As Integer = 0
        Private _JournalEntryID As Integer = 0
        Private _OperationalLimitAcquisition As ComplexChronologicValidator = Nothing
        Private _OperationalLimitTransfer As ComplexChronologicValidator = Nothing
        Private _Date As Date = Today
        Private _OldDate As Date = Today
        Private _DocumentNumber As String = ""
        Private _Content As String = ""
        Private _OldWarehouseFromID As Integer = 0
        Private _OldWarehouseToID As Integer = 0
        Private _WarehouseFrom As WarehouseInfo = Nothing
        Private _WarehouseTo As WarehouseInfo = Nothing
        Private _RequiresJournalEntry As Boolean = False
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now
        Private _Items As GoodsInternalTransferItemList

        ' used to implement automatic sort in datagridview
        <NotUndoable()> _
        <NonSerialized()> _
        Private _ItemsSortedList As Csla.SortedBindingList(Of GoodsInternalTransferItem) = Nothing


        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property JournalEntryID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _JournalEntryID
            End Get
        End Property

        Public ReadOnly Property OperationalLimitTransfer() As ComplexChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OperationalLimitTransfer
            End Get
        End Property

        Public ReadOnly Property OperationalLimitAcquisition() As ComplexChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OperationalLimitAcquisition
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
                    _Items.SetDate(_Date)
                End If
            End Set
        End Property

        Public ReadOnly Property OldDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OldDate
            End Get
        End Property

        Public Property DocumentNumber() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DocumentNumber.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _DocumentNumber.Trim <> value.Trim Then
                    _DocumentNumber = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

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

        Public ReadOnly Property OldWarehouseFromID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OldWarehouseFromID
            End Get
        End Property

        Public ReadOnly Property OldWarehouseToID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OldWarehouseToID
            End Get
        End Property

        Public Property WarehouseFrom() As WarehouseInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WarehouseFrom
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WarehouseInfo)
                CanWriteProperty(True)
                If WarehouseFromIsReadOnly Then Exit Property
                If Not (_WarehouseFrom Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _WarehouseFrom Is Nothing AndAlso Not value Is Nothing _
                    AndAlso _WarehouseFrom.ID = value.ID) Then
                    _WarehouseFrom = value
                    UpdateItemsWithTransfareWarehouse(value)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property WarehouseTo() As WarehouseInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WarehouseTo
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As WarehouseInfo)
                CanWriteProperty(True)
                If WarehouseToIsReadOnly Then Exit Property
                If Not (_WarehouseTo Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _WarehouseTo Is Nothing AndAlso Not value Is Nothing _
                    AndAlso _WarehouseTo.ID = value.ID) Then
                    _WarehouseTo = value
                    UpdateItemsWithAcquisitionWarehouse(value)
                    PropertyHasChanged()
                End If
            End Set
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

        Public ReadOnly Property Items() As GoodsInternalTransferItemList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Items
            End Get
        End Property

        Public ReadOnly Property RequiresJournalEntry() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _RequiresJournalEntry
            End Get
        End Property


        Public ReadOnly Property WarehouseFromIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return ((_RequiresJournalEntry AndAlso Not _OperationalLimitTransfer. _
                    BaseValidator.FinancialDataCanChange) OrElse _
                    Not _OperationalLimitTransfer.FinancialDataCanChange OrElse _
                    Not _OperationalLimitAcquisition.FinancialDataCanChange)
            End Get
        End Property

        Public ReadOnly Property WarehouseToIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return ((_RequiresJournalEntry AndAlso Not _OperationalLimitTransfer. _
                    BaseValidator.FinancialDataCanChange) OrElse _
                    Not _OperationalLimitAcquisition.FinancialDataCanChange)
            End Get
        End Property


        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return (Not String.IsNullOrEmpty(_DocumentNumber.Trim) _
                    OrElse Not String.IsNullOrEmpty(_Content.Trim) _
                    OrElse _Items.Count > 0)
            End Get
        End Property

        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _Items.IsDirty
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return MyBase.IsValid AndAlso _Items.IsValid
            End Get
        End Property


        Public Function GetAllBrokenRules() As String
            Dim result As String = Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error).Trim
            result = AddWithNewLine(result, _Items.GetAllBrokenRules, False)

            'Dim GeneralErrorString As String = ""
            'SomeGeneralValidationSub(GeneralErrorString)
            'AddWithNewLine(result, GeneralErrorString, False)

            Return result
        End Function

        Public Function GetAllWarnings() As String
            Dim result As String = Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning).Trim
            result = AddWithNewLine(result, _Items.GetAllWarnings(), False)


            'Dim GeneralErrorString As String = ""
            'SomeGeneralValidationSub(GeneralErrorString)
            'AddWithNewLine(result, GeneralErrorString, False)

            Return result
        End Function


        Public Function GetSortedList() As Csla.SortedBindingList(Of GoodsInternalTransferItem)

            If _ItemsSortedList Is Nothing Then
                _ItemsSortedList = New Csla.SortedBindingList(Of GoodsInternalTransferItem)(_Items)
            End If

            Return _ItemsSortedList

        End Function

        Public Function GetCostsParams() As GoodsCostParam()
            Dim result As New List(Of GoodsCostParam)
            If _WarehouseFrom Is Nothing OrElse Not _WarehouseFrom.ID > 0 Then Return result.ToArray
            For Each i As GoodsInternalTransferItem In _Items
                result.Add(GoodsCostParam.GetGoodsCostParam(i.GoodsInfo.ID, _WarehouseFrom.ID, _
                    i.Amount, i.Transfer.ID, i.Acquisition.ID, i.GoodsInfo.ValuationMethod, _Date))
            Next
            Return result.ToArray
        End Function

        Public Sub ReloadCostInfo(ByVal list As GoodsCostItemList)

            If list Is Nothing Then Throw New ArgumentNullException("Klaida. " _
                & "Metodui GoodsComplexOperationInternalTransfer.ReloadCostInfo " _
                & "nenurodytas (null) GoodsCostItemList parametras.")

            If Not _OperationalLimitAcquisition.BaseValidator.FinancialDataCanChange Then _
                Throw New Exception("Klaida. Finansinių operacijos duomenų keisti negalima." _
                & vbCrLf & _OperationalLimitAcquisition.BaseValidator.FinancialDataCanChangeExplanation)

            _Items.ReloadCostInfo(list)

        End Sub

        Public Sub AddNewGoodsItem(ByVal item As GoodsInternalTransferItem)

            If _Items.ContainsGood(item.GoodsInfo.ID) Then Throw New Exception( _
                "Klaida. Eilutė prekei '" & item.GoodsName & "' jau yra.")
            If item.RequiresJournalEntry AndAlso Not _OperationalLimitTransfer. _
                BaseValidator.FinancialDataCanChange Then Throw New Exception( _
                "Klaida. Neleidžiama pridėti eilutės, kuri reikalauja bednrojo " _
                & "žurnalo operacijos keitimo:" & vbCrLf & _OperationalLimitTransfer. _
                BaseValidator.FinancialDataCanChangeExplanation)

            item.SetDate(_Date)
            item.SetWarehouseFrom(_WarehouseFrom)
            item.SetWarehouseTo(_WarehouseTo)

            _Items.Add(item)

            _OperationalLimitAcquisition.MergeNewValidationItem(item.Acquisition.OperationLimitations)
            _OperationalLimitTransfer.MergeNewValidationItem(item.Transfer.OperationLimitations)
            _RequiresJournalEntry = _RequiresJournalEntry OrElse item.RequiresJournalEntry

            ValidationRules.CheckRules()

        End Sub


        Private Sub UpdateItemsWithAcquisitionWarehouse(ByVal value As WarehouseInfo)
            _Items.SetWarehouseTo(value)
            _OperationalLimitAcquisition.ReloadValidationItems(_Items.GetLimitations(True))
        End Sub

        Private Sub UpdateItemsWithTransfareWarehouse(ByVal value As WarehouseInfo)
            _Items.SetWarehouseFrom(value)
            _OperationalLimitTransfer.ReloadValidationItems(_Items.GetLimitations(False))
        End Sub


        Public Overrides Function Save() As GoodsComplexOperationInternalTransfer

            If IsNew AndAlso Not CanAddObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujiems duomenims įvesti.")
            If Not IsNew AndAlso Not CanEditObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims pakeisti.")

            If Not _Items.Count > 0 Then Throw New Exception("Klaida. Neįvesta nė viena eilutė.")

            Me.ValidationRules.CheckRules()
            If Not IsValid Then Throw New Exception("Duomenyse yra klaidų: " & vbcrlf & Me.GetAllBrokenRules)
            Return MyBase.Save

        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _ID
        End Function

        Public Overrides Function ToString() As String
            Return "Goods.GoodsComplexOperationInternalTransfer"
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("DocumentNumber", "dokumento numeris"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringRequired, _
                New CommonValidation.SimpleRuleArgs("Content", "dokumento trumpas aprašymas"))
            ValidationRules.AddRule(AddressOf CommonValidation.InfoObjectRequired, _
                New CommonValidation.InfoObjectRequiredRuleArgs("WarehouseFrom", _
                "sandėlis, iš kurio prekės paimamos", "ID"))
            ValidationRules.AddRule(AddressOf CommonValidation.ChronologyValidation, _
                New CommonValidation.ChronologyRuleArgs("Date", "OperationalLimitAcquisition"))
            ValidationRules.AddRule(AddressOf CommonValidation.ChronologyValidation, _
                New CommonValidation.ChronologyRuleArgs("Date", "OperationalLimitTransfer"))

            ValidationRules.AddRule(AddressOf WarehouseToValidation, New Validation.RuleArgs("WarehouseTo"))

            ValidationRules.AddDependantProperty("WarehouseFrom", "WarehouseTo", False)
            ValidationRules.AddDependantProperty("WarehouseFrom", "Date", False)
            ValidationRules.AddDependantProperty("WarehouseTo", "Date", False)

        End Sub

        ''' <summary>
        ''' Rule ensuring that the value of property WarehouseTo is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function WarehouseToValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim ValObj As GoodsComplexOperationInternalTransfer = _
                DirectCast(target, GoodsComplexOperationInternalTransfer)

            If ValObj._WarehouseTo Is Nothing OrElse Not ValObj._WarehouseTo.ID > 0 Then
                e.Description = "Nepasirinktas sandėlis, į kurį prekės perduodamos."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            ElseIf Not ValObj._WarehouseTo Is Nothing AndAlso ValObj._WarehouseTo.ID > 0 _
                AndAlso Not ValObj._WarehouseFrom Is Nothing AndAlso ValObj._WarehouseFrom.ID > 0 _
                AndAlso ValObj._WarehouseTo.ID = ValObj._WarehouseFrom.ID Then
                e.Description = "Sandėlis, į kurį prekės perduodamos, negali būti tas pats, " _
                    & "kaip ir tas, iš kurio paimamos."
                e.Severity = Validation.RuleSeverity.Error
                Return False
            End If

            Return True

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite("Goods.GoodsOperationInternalTransfer2")
        End Sub

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationInternalTransfer1")
        End Function

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationInternalTransfer2")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationInternalTransfer3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("Goods.GoodsOperationInternalTransfer3")
        End Function

#End Region

#Region " Factory Methods "

        Public Shared Function NewGoodsComplexOperationInternalTransfer() As GoodsComplexOperationInternalTransfer
            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujiems duomenims įvesti.")
            Dim result As New GoodsComplexOperationInternalTransfer
            result._Items = GoodsInternalTransferItemList.NewGoodsInternalTransferItemList
            Dim parentValidator As SimpleChronologicValidator = _
                SimpleChronologicValidator.NewSimpleChronologicValidator( _
                ConvertEnumHumanReadable(GoodsComplexOperationType.InternalTransfer))
            result._OperationalLimitAcquisition = ComplexChronologicValidator.NewComplexChronologicValidator( _
                 ConvertEnumHumanReadable(GoodsOperationType.Acquisition), parentValidator, Nothing)
            result._OperationalLimitTransfer = ComplexChronologicValidator.NewComplexChronologicValidator( _
                 ConvertEnumHumanReadable(GoodsOperationType.Transfer), parentValidator, Nothing)
            result.ValidationRules.CheckRules()
            Return result
        End Function

        Public Shared Function GetGoodsComplexOperationInternalTransfer(ByVal nID As Integer) As GoodsComplexOperationInternalTransfer
            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka šiems duomenims gauti.")
            Return DataPortal.Fetch(Of GoodsComplexOperationInternalTransfer)(New Criteria(nID))
        End Function

        Public Shared Sub DeleteGoodsComplexOperationInternalTransfer(ByVal id As Integer)
            If Not CanDeleteObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenų ištrynimui.")
            DataPortal.Delete(New Criteria(id))
        End Sub


        Private Sub New()
            ' require use of factory methods
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

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            Dim obj As ComplexOperationPersistenceObject = ComplexOperationPersistenceObject. _
                GetComplexOperationPersistenceObject(criteria.ID, True)

            Fetch(obj)

        End Sub

        Private Sub Fetch(ByVal obj As ComplexOperationPersistenceObject)

            If obj.OperationType <> GoodsComplexOperationType.InternalTransfer Then _
                Throw New Exception("Klaida. Kompleksinė operacija, kurios ID=" _
                & obj.ID.ToString & ", yra ne vidininis perkėlimas, o " _
                & ConvertEnumHumanReadable(obj.OperationType))

            _Content = obj.Content
            _DocumentNumber = obj.DocNo
            _ID = obj.ID
            _InsertDate = obj.InsertDate
            _JournalEntryID = obj.JournalEntryID
            _Date = obj.OperationDate
            _WarehouseTo = obj.SecondaryWarehouse
            _UpdateDate = obj.UpdateDate
            _WarehouseFrom = obj.Warehouse

            Dim parentValidator As IChronologicValidator = SimpleChronologicValidator. _
                GetSimpleChronologicValidator(_ID, _Date, ConvertEnumHumanReadable( _
                GoodsComplexOperationType.InternalTransfer))

            Using myData As DataTable = OperationalLimitList.GetDataSourceForComplexOperation(_ID, _Date)
                Dim objList As List(Of OperationPersistenceObject) = _
                    OperationPersistenceObject.GetOperationPersistenceObjectList(_ID)
                _Items = GoodsInternalTransferItemList.GetGoodsInternalTransferItemList( _
                    objList, myData, parentValidator.FinancialDataCanChange)
            End Using

            _RequiresJournalEntry = _Items.RequiresJournalEntry

            _OperationalLimitAcquisition = ComplexChronologicValidator.GetComplexChronologicValidator( _
                _JournalEntryID, _Date, ConvertEnumHumanReadable(GoodsOperationType.Acquisition), _
                parentValidator, _Items.GetLimitations(True))
            _OperationalLimitTransfer = ComplexChronologicValidator.GetComplexChronologicValidator( _
                _JournalEntryID, _Date, ConvertEnumHumanReadable(GoodsOperationType.Transfer), _
                parentValidator, _Items.GetLimitations(False))

            _OldWarehouseFromID = _WarehouseFrom.ID
            _OldWarehouseToID = _WarehouseTo.ID
            _OldDate = _Date

            MarkOld()

            ValidationRules.CheckRules()

        End Sub


        Protected Overrides Sub DataPortal_Insert()

            PrepareOperationConsignments()

            CheckIfCanUpdate()

            DoSave()

        End Sub

        Protected Overrides Sub DataPortal_Update()

            ComplexOperationPersistenceObject.CheckIfUpdateDateChanged(_ID, _UpdateDate)

            PrepareOperationConsignments()

            CheckIfCanUpdate()

            DoSave()

        End Sub

        Private Sub DoSave()

            Dim obj As ComplexOperationPersistenceObject = GetPersistenceObj()

            Dim JE As General.JournalEntry = GetJournalEntry()

            Dim TransactionExisted As Boolean = DatabaseAccess.TransactionExists
            If Not TransactionExisted Then DatabaseAccess.TransactionBegin()

            If Not JE Is Nothing Then
                JE = JE.SaveChild
                If IsNew Then
                    _JournalEntryID = JE.ID
                    obj.JournalEntryID = _JournalEntryID
                End If
            ElseIf JE Is Nothing AndAlso Not IsNew AndAlso _JournalEntryID > 0 Then
                General.JournalEntry.DeleteJournalEntryChild(_JournalEntryID)
                _JournalEntryID = 0
                obj.JournalEntryID = 0
            End If

            If IsNew Then
                _ID = obj.Save(_OperationalLimitAcquisition.FinancialDataCanChange _
                    AndAlso _OperationalLimitTransfer.FinancialDataCanChange, _
                    _OperationalLimitTransfer.FinancialDataCanChange, _
                    _OperationalLimitAcquisition.FinancialDataCanChange)
            Else
                obj.Save(_OperationalLimitAcquisition.FinancialDataCanChange _
                    AndAlso _OperationalLimitTransfer.FinancialDataCanChange, _
                    _OperationalLimitTransfer.FinancialDataCanChange, _
                    _OperationalLimitAcquisition.FinancialDataCanChange)
            End If

            _Items.Update(Me)

            If Not TransactionExisted Then DatabaseAccess.TransactionCommit()

            If IsNew Then _InsertDate = obj.InsertDate
            _UpdateDate = obj.UpdateDate
            _OldDate = _Date
            _OldWarehouseFromID = _WarehouseFrom.ID
            _OldWarehouseToID = _WarehouseTo.ID

            MarkOld()

            ReloadLimitations()

        End Sub

        Private Function GetPersistenceObj() As ComplexOperationPersistenceObject

            Dim obj As ComplexOperationPersistenceObject
            If IsNew Then
                obj = ComplexOperationPersistenceObject.NewComplexOperationPersistenceObject
            Else
                obj = ComplexOperationPersistenceObject.GetComplexOperationPersistenceObject(_ID, False)
            End If

            obj.AccountOperation = 0
            obj.GoodsID = 0
            obj.OperationType = GoodsComplexOperationType.InternalTransfer
            obj.Content = _Content
            obj.DocNo = _DocumentNumber
            obj.JournalEntryID = _JournalEntryID
            obj.OperationDate = _Date
            obj.SecondaryWarehouse = _WarehouseTo
            obj.Warehouse = _WarehouseFrom

            Return obj

        End Function



        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_ID))
        End Sub

        Protected Overrides Sub DataPortal_Delete(ByVal criteria As Object)

            Dim OperationToDelete As GoodsComplexOperationInternalTransfer = _
                New GoodsComplexOperationInternalTransfer
            OperationToDelete.DataPortal_Fetch(DirectCast(criteria, Criteria))

            If Not OperationToDelete._OperationalLimitAcquisition.FinancialDataCanChange Then _
                Throw New Exception("Klaida. Negalima ištrinti prekių " _
                    & "vidinio judėjimo operacijos:" & vbCrLf & OperationToDelete. _
                    _OperationalLimitAcquisition.FinancialDataCanChangeExplanation)
            If Not OperationToDelete._OperationalLimitTransfer.FinancialDataCanChange Then _
                Throw New Exception("Klaida. Negalima ištrinti prekių " _
                    & "vidinio judėjimo operacijos:" & vbCrLf & OperationToDelete. _
                    _OperationalLimitTransfer.FinancialDataCanChangeExplanation)

            If OperationToDelete.JournalEntryID > 0 Then IndirectRelationInfoList. _
                CheckIfJournalEntryCanBeDeleted(OperationToDelete.JournalEntryID, _
                DocumentType.GoodsInternalTransfer)

            Dim TransactionExisted As Boolean = DatabaseAccess.TransactionExists
            If Not TransactionExisted Then DatabaseAccess.TransactionBegin()

            ComplexOperationPersistenceObject.DeleteConsignmentDiscards(DirectCast(criteria, Criteria).ID)

            ComplexOperationPersistenceObject.DeleteConsignments(DirectCast(criteria, Criteria).ID)

            ComplexOperationPersistenceObject.DeleteOperations(DirectCast(criteria, Criteria).ID)

            ComplexOperationPersistenceObject.Delete(DirectCast(criteria, Criteria).ID)

            If OperationToDelete.JournalEntryID > 0 Then General.JournalEntry. _
                DeleteJournalEntryChild(OperationToDelete.JournalEntryID)

            If Not TransactionExisted Then DatabaseAccess.TransactionCommit()

            MarkNew()

        End Sub


        Private Sub CheckIfCanUpdate()

            ValidationRules.CheckRules()
            If Not IsValid Then Throw New Exception("Prekių vidinio judėjimo operacijoje " _
                & "yra klaidų: " & BrokenRulesCollection.ToString)

            _Items.SetValues(Me)

            Dim exceptionText As String = ""

            If IsNew Then
                exceptionText = AddWithNewLine(exceptionText, _Items.CheckIfCanUpdate(Nothing, False), False)
            Else
                Using myData As DataTable = OperationalLimitList.GetDataSourceForComplexOperation(_ID, _OldDate)
                    exceptionText = AddWithNewLine(exceptionText, _Items.CheckIfCanUpdate(myData, False), False)
                End Using
            End If

            If Not String.IsNullOrEmpty(exceptionText.Trim.Trim) Then _
                Throw New Exception(exceptionText.Trim)

        End Sub

        Private Sub ReloadLimitations()

            Using myData As DataTable = OperationalLimitList.GetDataSourceForComplexOperation(_ID, _Date)
                _Items.ReloadLimitations(myData, _OperationalLimitAcquisition.BaseValidator.FinancialDataCanChange)
            End Using

            _OperationalLimitAcquisition.ReloadValidationItems(_ID, _Date, _Items.GetLimitations(True))
            _OperationalLimitTransfer.ReloadValidationItems(_ID, _Date, _Items.GetLimitations(False))

            ValidationRules.CheckRules()

        End Sub

        Private Sub PrepareOperationConsignments()
            _Items.PrepareOperationConsignments()
        End Sub

        Private Function GetJournalEntry() As General.JournalEntry

            Dim result As General.JournalEntry
            If IsNew OrElse Not _JournalEntryID > 0 Then
                result = General.JournalEntry.NewJournalEntryChild(DocumentType.GoodsInternalTransfer)
            Else
                result = General.JournalEntry.GetJournalEntryChild(_JournalEntryID, DocumentType.GoodsInternalTransfer)
            End If

            result.Content = _Content
            result.Date = _Date
            result.DocNumber = _DocumentNumber

            If _OperationalLimitAcquisition.BaseValidator.FinancialDataCanChange Then

                Dim FullBookEntryList As BookEntryInternalList = BookEntryInternalList. _
                NewBookEntryInternalList(BookEntryType.Debetas)

                For Each i As GoodsInternalTransferItem In _Items
                    FullBookEntryList.AddRange(i.GetBookEntryInternalList())
                Next

                If Not FullBookEntryList.Count > 0 Then Return Nothing

                FullBookEntryList.Aggregate()

                result.DebetList.Clear()
                result.CreditList.Clear()

                result.DebetList.LoadBookEntryListFromInternalList(FullBookEntryList, False, False)
                result.CreditList.LoadBookEntryListFromInternalList(FullBookEntryList, False, False)

            End If

            If Not result.IsValid Then Throw New Exception("Klaida. Nepavyko generuoti " _
                & "bendrojo žurnalo įrašo:" & result.ToString & vbCrLf & result.GetAllBrokenRules)

            Return result

        End Function

#End Region

    End Class

End Namespace