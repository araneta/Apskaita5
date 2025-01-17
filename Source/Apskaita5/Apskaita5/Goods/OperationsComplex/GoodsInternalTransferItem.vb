Imports ApskaitaObjects.My.Resources
Imports Csla.Validation

Namespace Goods

    ''' <summary>
    ''' Represents a goods transfer between company's warehouses operation.
    ''' </summary>
    ''' <remarks>Values are persisted using an encapsulated <see cref="GoodsOperationTransfer">
    ''' simple goods transfer operation</see> and an encapsulated <see cref="GoodsOperationAcquisition">
    ''' simple goods acquisition operation.</see>.
    ''' Should only be used as a child of <see cref="GoodsInternalTransferItemList">GoodsInternalTransferItemList</see>.</remarks>
    <Serializable()> _
    Public NotInheritable Class GoodsInternalTransferItem
        Inherits BusinessBase(Of GoodsInternalTransferItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private ReadOnly _Guid As Guid = Guid.NewGuid
        Private _Acquisition As GoodsOperationAcquisition = Nothing
        Private _Transfer As GoodsOperationTransfer = Nothing


        ''' <summary>
        ''' Gets an <see cref="GoodsOperationTransfer.ID">ID of the encapsulated 
        ''' simple goods transfer operation</see>.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.ID
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="GoodsOperationAcquisition.ID">ID of the encapsulated 
        ''' simple goods acquisition operation</see>.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationAcquisition">
        ''' encapsulated simple goods acquisition operation</see>.</remarks>
        Public ReadOnly Property AcquisitionID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Acquisition.ID
            End Get
        End Property

        ''' <summary>
        ''' Gets <see cref="GoodsOperationTransfer.OperationLimitations">
        ''' chronologic limitations of the encapsulated simple goods 
        ''' transfer operation</see>.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        Public ReadOnly Property ChronologyValidatorTransfer() As OperationalLimitList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.OperationLimitations
            End Get
        End Property

        ''' <summary>
        ''' Gets <see cref="GoodsOperationAcquisition.OperationLimitations">
        ''' chronologic limitations of the encapsulated simple goods 
        ''' acquisition operation</see>.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationAcquisition">
        ''' encapsulated simple goods acquisition operation</see>.</remarks>
        Public ReadOnly Property ChronologyValidatorAcquisition() As OperationalLimitList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Acquisition.OperationLimitations
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="GoodsOperationTransfer.GoodsInfo">general information 
        ''' about the goods</see> that are transfered by the operation.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        Public ReadOnly Property GoodsInfo() As GoodsSummary
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.GoodsInfo
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="GoodsOperationTransfer.GoodsInfo">name of 
        ''' the goods</see> that are transfered by the operation.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        Public ReadOnly Property GoodsName() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.GoodsInfo.Name
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="GoodsOperationTransfer.GoodsInfo">measure unit of 
        ''' the goods</see> that are transfered by the operation.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        Public ReadOnly Property GoodsMeasureUnit() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.GoodsInfo.MeasureUnit
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="GoodsOperationTransfer.GoodsInfo">accounting method of 
        ''' the goods</see> that are transfered by the operation.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        Public ReadOnly Property GoodsAccountingMethod() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.GoodsInfo.AccountingMethodHumanReadable
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="GoodsOperationTransfer.GoodsInfo">valuation method of 
        ''' the goods</see> that are transfered by the operation.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        Public ReadOnly Property GoodsValuationMethod() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.GoodsInfo.ValuationMethodHumanReadable
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets <see cref="GoodsOperationTransfer.Description">remarks 
        ''' (content, description)</see> regarding the operation.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        <StringField(ValueRequiredLevel.Optional, 255)> _
        Public Property Remarks() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.Description
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Transfer.Description.Trim <> value.Trim OrElse _
                    _Acquisition.Description.Trim <> value.Trim Then
                    _Transfer.SetDescription(value)
                    _Acquisition.SetDescription(value)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an <see cref="GoodsOperationTransfer.Amount">amount of 
        ''' the goods</see> that is transfered by the operation.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, ROUNDAMOUNTGOODS)> _
        Public Property Amount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.Amount
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)

                CanWriteProperty(True)
                If AmountIsReadOnly Then Exit Property

                If _Transfer.Amount <> CRound(value, ROUNDAMOUNTGOODS) Then

                    _Transfer.Amount = value
                    _Acquisition.SetAmount(value)
                    _Acquisition.SetUnitCost(0)
                    _Acquisition.TotalCost = 0

                    PropertyHasChanged()
                    PropertyHasChanged("UnitCost")
                    PropertyHasChanged("TotalCost")

                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets costs of the transfered goods per unit.
        ''' </summary>
        ''' <remarks>Use <see cref="GoodsCostItem">goods cost query object</see> to fetch
        ''' costs for a transfered amount.
        ''' Is calculated as <see cref="TotalCost">TotalCost</see>
        ''' divided by <see cref="Amount">Amount</see>.
        ''' Final value is set by <see cref="ConsignmentDiscardPersistenceObjectList">
        ''' consignment discards persistence object</see>.
        ''' A proxy property to the <see cref="GoodsOperationTransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Recommended, False, ROUNDUNITGOODS)> _
        Public ReadOnly Property UnitCost() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.UnitCost
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="GoodsOperationtransfer.TotalCost">total costs 
        ''' of the goods</see> that are discarded by the operation.
        ''' </summary>
        ''' <remarks>A proxy property to the <see cref="GoodsOperationtransfer">
        ''' encapsulated simple goods transfer operation</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Recommended, False, 2)> _
        Public ReadOnly Property TotalCost() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Transfer.TotalCost
            End Get
        End Property


        ''' <summary>
        ''' Whether the <see cref="Amount">Amount</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property AmountIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _Transfer.OperationLimitations.FinancialDataCanChange _
                    OrElse Not _Transfer.OperationLimitations.BaseValidator.FinancialDataCanChange _
                    OrElse Not _Acquisition.OperationLimitations.FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="LoadCostInfo">LoadCostInfo</see> method could be invoked,
        ''' i.e. <see cref="UnitCost">UnitCost</see> and 
        ''' <see cref="TotalCost">TotalCost</see> can be set.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property LoadCostInfoIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _Acquisition.OperationLimitations.FinancialDataCanChange _
                    OrElse Not _Acquisition.OperationLimitations.ParentFinancialDataCanChange _
                    OrElse Not _Transfer.OperationLimitations.FinancialDataCanChange
            End Get
        End Property


        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _Acquisition.IsDirty OrElse _Transfer.IsDirty
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                ' encapsulated operations only get prepared before update
                ' they are not (generaly) valid at the save
                Return MyBase.IsValid
            End Get
        End Property



        Public Function HasWarnings() As Boolean
            Return MyBase.BrokenRulesCollection.WarningCount > 0
        End Function

        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString
            If Not MyBase.IsValid Then
                Return String.Format(My.Resources.Common_ErrorInItem, Me.ToString, _
                    vbCrLf, Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error))
            End If
            Return ""
        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString
            If Not HasWarnings() Then Return ""
            Return String.Format(My.Resources.Common_WarningInItem, Me.ToString, _
                vbCrLf, Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning))
        End Function


        ''' <summary>
        ''' Loads <see cref="UnitCost">UnitCost</see> and <see cref="TotalCost">TotalCost</see>
        ''' values from a <see cref="GoodsCostItem">query object</see>.
        ''' </summary>
        ''' <param name="costInfo">a query object</param>
        ''' <remarks></remarks>
        Public Sub LoadCostInfo(ByVal costInfo As GoodsCostItem)

            If LoadCostInfoIsReadOnly Then

                Dim fullDescription As String = _Acquisition.OperationLimitations.ParentFinancialDataCanChangeExplanation
                fullDescription = AddWithNewLine(fullDescription, _
                    _Acquisition.OperationLimitations.FinancialDataCanChangeExplanation, False)
                fullDescription = AddWithNewLine(fullDescription, _
                    _Transfer.OperationLimitations.FinancialDataCanChangeExplanation, False)

                Throw New Exception(String.Format(Goods_GoodsInternalTransferItem_CannotChangeFinancialData, _
                    Me.ToString, vbCrLf, fullDescription))

            End If

            _Transfer.LoadCostInfo(costInfo)
            _Acquisition.SetUnitCost(_Transfer.UnitCost)
            _Acquisition.TotalCost = _Transfer.TotalCost

            PropertyHasChanged("UnitCost")
            PropertyHasChanged("TotalCost")

        End Sub

        ''' <summary>
        ''' Gets a param object for a <see cref="GoodsCostItem">query object</see>.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetGoodsCostParam() As GoodsCostParam
            If _Transfer.Warehouse Is Nothing OrElse _Transfer.Warehouse.IsEmpty Then
                Throw New Exception(Goods_GoodsInternalTransferItem_WarehouseFromNull)
            End If
            Return GoodsCostParam.NewGoodsCostParam(_Transfer.GoodsInfo.ID, _
                _Transfer.Warehouse.ID, _Transfer.Amount, _Transfer.ID, _
                _Acquisition.ID, _Transfer.GoodsInfo.ValuationMethod, _Transfer.Date)
        End Function


        Friend Sub SetParentDate(ByVal value As Date)
            If _Transfer.Date.Date <> value.Date OrElse _Acquisition.JournalEntryDate.Date <> value.Date Then
                _Acquisition.SetParentDate(value)
                _Transfer.SetParentDate(value)
                PropertyHasChanged()
            End If
        End Sub

        Friend Sub SetParentWarehouseFrom(ByVal value As WarehouseInfo)

            If Not (_Transfer.Warehouse Is Nothing AndAlso value Is Nothing) _
                AndAlso Not (Not _Transfer.Warehouse Is Nothing AndAlso Not value Is Nothing _
                AndAlso _Transfer.Warehouse.ID = value.ID) Then

                If Not _Transfer.OperationLimitations.FinancialDataCanChange Then
                    Throw New Exception(String.Format(Goods_GoodsInternalTransferItem_CannotChangeFinancialData, _
                        Me.ToString, vbCrLf, _Transfer.OperationLimitations.FinancialDataCanChangeExplanation))
                End If

                _Transfer.Warehouse = value
                _Acquisition.SetUnitCost(0)
                _Acquisition.TotalCost = 0

                PropertyHasChanged()

            End If

        End Sub

        Friend Sub SetParentWarehouseTo(ByVal value As WarehouseInfo)

            If Not (_Acquisition.Warehouse Is Nothing AndAlso value Is Nothing) _
                AndAlso Not (Not _Acquisition.Warehouse Is Nothing AndAlso Not value Is Nothing _
                AndAlso _Acquisition.Warehouse.ID = value.ID) Then

                If Not _Acquisition.OperationLimitations.ParentFinancialDataCanChange _
                    OrElse Not _Transfer.OperationLimitations.FinancialDataCanChange Then

                    Dim fullDescription As String = _Acquisition.OperationLimitations.ParentFinancialDataCanChangeExplanation
                    fullDescription = AddWithNewLine(fullDescription, _
                        _Transfer.OperationLimitations.FinancialDataCanChangeExplanation, False)

                    Throw New Exception(String.Format(Goods_GoodsInternalTransferItem_CannotChangeFinancialData, _
                        Me.ToString, vbCrLf, fullDescription))

                End If

                _Acquisition.Warehouse = value

                If Not value Is Nothing AndAlso Not value.IsEmpty Then
                    _Transfer.AccountGoodsCost = value.WarehouseAccount
                End If

                PropertyHasChanged()

            End If

        End Sub

        Friend Sub SetParentProperties(ByVal parentDocumentNumber As String, _
            ByVal parentContent As String)
            _Transfer.SetParentProperties(parentDocumentNumber, parentContent)
            _Acquisition.SetParentProperties(parentDocumentNumber, parentContent)
        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return String.Format(Goods_GoodsInternalTransferItem_ToString, _
                _Transfer.GoodsInfo.Name)
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()
            ValidationRules.AddRule(AddressOf CommonValidation.DoubleFieldValidation, _
                New RuleArgs("Amount"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New RuleArgs("Remarks"))
        End Sub

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets a new GoodsInternalTransferItem instance.
        ''' </summary>
        ''' <param name="goodsID">an <see cref="GoodsItem.ID">ID of the goods</see>
        ''' to transfer</param>
        ''' <param name="warehouseFrom">a warehouse to transfer the goods from</param>
        ''' <param name="warehouseTo">a warehouse to transfer the goods to</param>
        ''' <param name="parentValidator">a chronologic validator of the parent document (if any)</param>
        ''' <remarks></remarks>
        Public Shared Function NewGoodsInternalTransferItem(ByVal goodsID As Integer, _
            ByVal warehouseFrom As WarehouseInfo, ByVal warehouseTo As WarehouseInfo, _
            ByVal parentValidator As IChronologicValidator) As GoodsInternalTransferItem
            Return New GoodsInternalTransferItem(goodsID, warehouseFrom, warehouseTo, parentValidator)
        End Function

        ''' <summary>
        ''' Gets an existing GoodsInternalTransferItem instance using a database query result.
        ''' </summary>
        ''' <param name="objAcquisition">a goods operation persistence object containing the 
        ''' encapsulated simple goods acquisition data</param>
        ''' <param name="objTransfer">a goods operation persistence object containing the 
        ''' encapsulated simple goods transfer data</param>
        ''' <param name="parentValidator">a chronologic validator of the parent document (if any)</param>
        ''' <param name="limitationsDataSource">a datasource for the 
        ''' <see cref="OperationalLimitList">chronologic validator</see></param>
        ''' <remarks></remarks>
        Friend Shared Function GetGoodsInternalTransferItem( _
            ByVal objAcquisition As OperationPersistenceObject, _
            ByVal objTransfer As OperationPersistenceObject, _
            ByVal limitationsDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator) As GoodsInternalTransferItem
            Return New GoodsInternalTransferItem(objAcquisition, objTransfer, limitationsDataSource, parentValidator)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal goodsID As Integer, ByVal warehouseFrom As WarehouseInfo, _
            ByVal warehouseTo As WarehouseInfo, ByVal parentValidator As IChronologicValidator)
            MarkAsChild()
            Create(goodsID, warehouseFrom, warehouseTo, parentValidator)
        End Sub

        Private Sub New(ByVal objAcquisition As OperationPersistenceObject, _
            ByVal objTransfer As OperationPersistenceObject, _
            ByVal limitationsDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator)
            MarkAsChild()
            Fetch(objAcquisition, objTransfer, limitationsDataSource, parentValidator)
        End Sub

#End Region

#Region " Data Access "

        <NonSerialized(), NotUndoable()>
        Private _ConsignmentDiscards As ConsignmentDiscardPersistenceObjectList = Nothing


        Private Sub Create(ByVal goodsID As Integer, ByVal warehouseFrom As WarehouseInfo, _
            ByVal warehouseTo As WarehouseInfo, ByVal parentValidator As IChronologicValidator)

            _Acquisition = GoodsOperationAcquisition.NewGoodsOperationAcquisitionChild( _
                goodsID, warehouseTo, parentValidator)
            _Transfer = GoodsOperationTransfer.NewGoodsOperationTransferChild( _
                _Acquisition.GoodsInfo, warehouseFrom, parentValidator)

            If Not warehouseTo Is Nothing AndAlso Not warehouseTo.IsEmpty Then
                _Transfer.AccountGoodsCost = warehouseTo.WarehouseAccount
            End If

            MarkNew()

            ValidationRules.CheckRules()

        End Sub


        Private Sub Fetch(ByVal objAcquisition As OperationPersistenceObject, _
            ByVal objTransfer As OperationPersistenceObject, _
            ByVal limitationsDataSource As DataTable, _
            ByVal parentValidator As IChronologicValidator)

            _Acquisition = GoodsOperationAcquisition.GetGoodsOperationAcquisitionChild( _
                objAcquisition, limitationsDataSource, parentValidator)
            _Transfer = GoodsOperationTransfer.GetGoodsOperationTransferChild( _
                objTransfer, parentValidator, limitationsDataSource)

            MarkOld()

            ValidationRules.CheckRules()

        End Sub

        Friend Sub Update(ByVal parent As GoodsComplexOperationInternalTransfer)

            _Acquisition.SetParentDate(parent.Date) ' just in case
            _Acquisition.SetParentProperties(parent.DocumentNumber, parent.Content)
            _Transfer.SetParentDate(parent.Date)
            _Transfer.SetParentProperties(parent.DocumentNumber, parent.Content)

            _Acquisition.SaveChild(parent.JournalEntryID, parent.ID,
                _Transfer.OperationLimitations.FinancialDataCanChange, True)
            _Transfer.SaveChild(parent.JournalEntryID, parent.ID, False, True, False)
            If IsNew OrElse (_Transfer.OperationLimitations.FinancialDataCanChange _
                AndAlso _Acquisition.OperationLimitations.FinancialDataCanChange) Then
                _ConsignmentDiscards.Update(_Transfer.ID)
            End If

            MarkOld()

        End Sub

        Friend Sub DeleteSelf()

            _Acquisition.DeleteGoodsOperationAcquisitionChild()
            _Transfer.DeleteGoodsOperationTransferChild()

            MarkNew()

        End Sub


        Friend Sub CheckIfCanUpdate(ByVal parentValidator As IChronologicValidator, _
            ByVal limitationsDataSource As DataTable)

            _Acquisition.CheckIfCanUpdate(parentValidator, limitationsDataSource)
            _Transfer.CheckIfCanUpdate(limitationsDataSource, parentValidator)

        End Sub

        Friend Sub CheckIfCanDelete(ByVal parentValidator As IChronologicValidator, _
            ByVal limitationsDataSource As DataTable)

            If IsNew Then Exit Sub

            _Acquisition.CheckIfCanDelete(parentValidator, limitationsDataSource)
            _Transfer.CheckIfCanDelete(limitationsDataSource, parentValidator)

        End Sub

        Friend Function GetBookEntryInternalList() As BookEntryInternalList

            Dim result As BookEntryInternalList = _
               BookEntryInternalList.NewBookEntryInternalList(BookEntryType.Debetas)

            ' acquisition costs are accounted in the GoodsItem.PurchasesAccount, 
            ' i.e. does not change when the warehouse is changed
            If _Transfer.GoodsInfo.AccountingMethod = Goods.GoodsAccountingMethod.Periodic Then Return result

            result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas, _
                _Transfer.Warehouse.WarehouseAccount, _Acquisition.TotalCost))
            result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas, _
                _Acquisition.Warehouse.WarehouseAccount, _Acquisition.TotalCost))

            Return result

        End Function

        Friend Sub PrepareConsignements()

            If Not IsNew AndAlso Not _Acquisition.OperationLimitations.FinancialDataCanChange Then Exit Sub

            Dim availableConsignments As ConsignmentPersistenceObjectList = _
                ConsignmentPersistenceObjectList.NewConsignmentPersistenceObjectList( _
                _Transfer.GoodsInfo.ID, _Transfer.Warehouse.ID, _Transfer.ID, _Acquisition.ID, _
                (_Transfer.GoodsInfo.ValuationMethod = Goods.GoodsValuationMethod.LIFO))

            availableConsignments.RemoveLateEntries(_Transfer.Date)

            Dim newConsignmentDiscards As ConsignmentDiscardPersistenceObjectList = _
                ConsignmentDiscardPersistenceObjectList.NewConsignmentDiscardPersistenceObjectList( _
                availableConsignments, _Transfer.Amount, _Transfer.GoodsInfo.Name)

            If IsNew Then
                _ConsignmentDiscards = newConsignmentDiscards
            Else
                _ConsignmentDiscards = ConsignmentDiscardPersistenceObjectList. _
                    GetConsignmentDiscardPersistenceObjectList(_Transfer.ID)
                _ConsignmentDiscards.MergeChangedList(newConsignmentDiscards)
            End If

            Dim totalCost As Double = _ConsignmentDiscards.GetTotalValue
            Dim unitCost As Double = CRound(totalCost / _Transfer.Amount, ROUNDUNITGOODS)
            _Transfer.SetCosts(unitCost, totalCost)
            _Acquisition.Ammount = _Transfer.Amount
            _Acquisition.UnitCost = unitCost
            _Acquisition.TotalCost = totalCost

        End Sub

#End Region

    End Class

End Namespace