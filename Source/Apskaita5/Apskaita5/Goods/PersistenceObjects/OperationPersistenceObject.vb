Imports ApskaitaObjects.My.Resources

Namespace Goods

    ''' <summary>
    ''' Represents a helper object that persists data of a simple (atomic) goods operation,
    ''' e.g. <see cref="GoodsOperationDiscard">GoodsOperationDiscard</see>.
    ''' </summary>
    ''' <remarks>Should only be used as a child of a simple goods operation.
    ''' Values are stored in the database table goodsoperations.</remarks>
    <Serializable()>
    Friend NotInheritable Class OperationPersistenceObject

#Region " Business Methods "

        Private _ID As Integer = 0
        Private _OperationDate As Date = Today
        Private _OperationType As GoodsOperationType = GoodsOperationType.Acquisition
        Private _JournalEntryID As Integer = 0
        Private _DocNo As String = ""
        Private _Content As String = ""
        Private _GoodsID As Integer = 0
        Private _GoodsInfo As GoodsSummary = Nothing
        Private _WarehouseID As Integer = 0
        Private _Warehouse As WarehouseInfo = Nothing
        Private _Amount As Double = 0
        Private _AmountInWarehouse As Double = 0
        Private _AmountInPurchases As Double = 0
        Private _UnitValue As Double = 0
        Private _TotalValue As Double = 0
        Private _AccountGeneral As Double = 0
        Private _AccountSalesNetCosts As Double = 0
        Private _AccountPurchases As Double = 0
        Private _AccountDiscounts As Double = 0
        Private _AccountPriceCut As Double = 0
        Private _ComplexOperationID As Integer = 0
        Private _ComplexOperationType As GoodsComplexOperationType = GoodsComplexOperationType.InternalTransfer
        Private _ComplexOperationHumanReadable As String = ""
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now
        Private _AccountOperation As Long = 0
        Private _AccountOperationValue As Double = 0
        Private _JournalEntryContent As String = ""
        Private _JournalEntryCorrespondence As String = ""
        Private _JournalEntryRelatedPerson As String = ""
        Private _JournalEntryType As DocumentType = DocumentType.None
        Private _JournalEntryTypeHumanReadable As String = ""
        Private _JournalEntryDate As Date = Today
        Private _JournalEntryDocNo As String = ""


        ''' <summary>
        ''' Gets an ID of the operation that is assigned by a database (AUTOINCREMENT).
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.ID.</remarks>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the operation was inserted into the database.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.InsertDate.</remarks>
        Public ReadOnly Property InsertDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _InsertDate
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the operation was last updated.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.UpdateDate.</remarks>
        Public ReadOnly Property UpdateDate() As DateTime
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _UpdateDate
            End Get
        End Property

        ''' <summary>
        ''' Gets a type of the operation.
        ''' </summary>
        ''' <remarks>Is set when creating a new operation and cannot be changed afterwards.
        ''' Value is stored in the database field goodsoperations.OperationType.</remarks>
        Public ReadOnly Property OperationType() As GoodsOperationType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _OperationType
            End Get
        End Property

        ''' <summary>
        ''' Gets an <see cref="GoodsItem.ID">ID of the goods</see> 
        ''' that the operation operates with.
        ''' </summary>
        ''' <remarks>Is set when creating a new operation and cannot be changed afterwards.
        ''' Value is stored in the database field goodsoperations.GoodsID.</remarks>
        Public ReadOnly Property GoodsID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _GoodsID
            End Get
        End Property

        ''' <summary>
        ''' Gets <see cref="GoodsSummary">information about the goods</see> 
        ''' that the operation operates with.
        ''' </summary>
        ''' <remarks>Is set when creating a new operation and cannot be changed afterwards.
        ''' Value is stored in the database field goodsoperations.GoodsID.</remarks>
        Public ReadOnly Property GoodsInfo() As GoodsSummary
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _GoodsInfo
            End Get
        End Property


        ''' <summary>
        ''' An <see cref="ComplexOperationPersistenceObject.ID">ID of the complex 
        ''' goods operation</see> that the operation is a part of.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.ComplexOperationID.</remarks>
        Public Property ComplexOperationID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _ComplexOperationID
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Friend Set(ByVal value As Integer)
                If _ComplexOperationID <> value Then
                    _ComplexOperationID = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' A <see cref="ComplexOperationPersistenceObject.OperationType">type 
        ''' of the complex goods operation</see> that the operation is a part of.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.ComplexOperationID.</remarks>
        Public ReadOnly Property ComplexOperationType() As GoodsComplexOperationType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _ComplexOperationType
            End Get
        End Property

        ''' <summary>
        ''' A <see cref="ComplexOperationPersistenceObject.OperationType">localized
        ''' human readable type of the complex goods operation</see> 
        ''' that the operation is a part of.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.ComplexOperationID.</remarks>
        Public ReadOnly Property ComplexOperationHumanReadable() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _ComplexOperationHumanReadable.Trim
            End Get
        End Property


        ''' <summary>
        ''' Gets or sets a date of the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.OperationDate.</remarks>
        Public Property OperationDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _OperationDate
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Date)
                If _OperationDate.Date <> value.Date Then
                    _OperationDate = value.Date
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a document number of the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.DocNo.</remarks>
        Public Property DocNo() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _DocNo.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As String)
                If value Is Nothing Then value = ""
                If _DocNo.Trim <> value.Trim Then
                    _DocNo = value.Trim
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a content (description) of the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.Content.</remarks>
        Public Property Content() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _Content.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As String)
                If value Is Nothing Then value = ""
                If _Content.Trim <> value.Trim Then
                    _Content = value.Trim
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an <see cref="Goods.Warehouse.ID">ID of the warehouse</see> 
        ''' that the operation operates in.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.WarehouseID.</remarks>
        Public Property WarehouseID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _WarehouseID
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Integer)
                If _WarehouseID <> value Then
                    _WarehouseID = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a warehouse (warehouse value object) 
        ''' that the operation operates in.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.WarehouseID.</remarks>
        Public Property Warehouse() As WarehouseInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _Warehouse
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As WarehouseInfo)
                If Not (_Warehouse Is Nothing AndAlso value Is Nothing) AndAlso
                    (value Is Nothing OrElse _Warehouse Is Nothing OrElse _Warehouse.ID <> value.ID) Then
                    _Warehouse = value
                    If _Warehouse Is Nothing OrElse Not _Warehouse.ID > 0 Then
                        _WarehouseID = 0
                    Else
                        _WarehouseID = _Warehouse.ID
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a total change of the goods amount.
        ''' (irrespective of whether the change is accounted in the general ledger or not)
        ''' </summary>
        ''' <remarks>Could be positive (amount increase) or negative (amount decrease).
        ''' Value is stored in the database field goodsoperations.Amount.</remarks>
        Public Property Amount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_Amount, ROUNDAMOUNTGOODS)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_Amount, ROUNDAMOUNTGOODS) <> CRound(value, ROUNDAMOUNTGOODS) Then
                    _Amount = CRound(value, ROUNDAMOUNTGOODS)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a change of the goods amount that is accounted in the
        ''' <see cref="Goods.Warehouse.WarehouseAccount">warehouse account</see>.
        '''  </summary>
        ''' <remarks>Could be positive (amount increase) or negative (amount decrease).
        ''' Amount in warehouse changes per operation basis, when the
        ''' <see cref="GoodsItem.AccountingMethod">goods accounting method</see>
        ''' is <see cref="GoodsAccountingMethod.Persistent">Persistent</see>,
        ''' and does not change, when the method is <see cref="GoodsAccountingMethod.Periodic">Periodic</see>.
        ''' Value is stored in the database field goodsoperations.AmountInWarehouse.</remarks>
        Public Property AmountInWarehouse() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_AmountInWarehouse, ROUNDAMOUNTGOODS)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_AmountInWarehouse, ROUNDAMOUNTGOODS) <> CRound(value, ROUNDAMOUNTGOODS) Then
                    _AmountInWarehouse = CRound(value, ROUNDAMOUNTGOODS)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a change of the goods amount that is accounted in the
        ''' <see cref="Goods.GoodsItem.AccountPurchases">purchases account</see>.
        '''  </summary>
        ''' <remarks>Only applicable when the <see cref="GoodsItem.AccountingMethod">
        ''' goods accounting method</see> is <see cref="GoodsAccountingMethod.Periodic">Periodic</see>.
        ''' Can only be negative (amount in purchases account reduced) when
        ''' transfering goods between warehouses or inventorization.
        ''' Value is stored in the database field goodsoperations.AmountInPurchases.</remarks>
        Public Property AmountInPurchases() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_AmountInPurchases, ROUNDAMOUNTGOODS)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_AmountInPurchases, ROUNDAMOUNTGOODS) <> CRound(value, ROUNDAMOUNTGOODS) Then
                    _AmountInPurchases = CRound(value, ROUNDAMOUNTGOODS)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a goods value per unit within the operation. 
        ''' </summary>
        ''' <remarks>Either acquisition price or discard costs, i.e. always positive
        ''' (or zero, if the unit value is not applicable for the operation,
        ''' e.g. <see cref="GoodsOperationPriceCut">GoodsOperationPriceCut</see>).
        ''' Value is stored in the database field goodsoperations.UnitValue.</remarks>
        Public Property UnitValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_UnitValue, ROUNDUNITGOODS)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_UnitValue, ROUNDUNITGOODS) <> CRound(value, ROUNDUNITGOODS) Then
                    _UnitValue = CRound(value, ROUNDUNITGOODS)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a total goods value change within the <see cref="Warehouse">warehouse</see>. 
        ''' </summary>
        ''' <remarks>Could be positive (acquisition, additional costs) 
        ''' or negative (transfer, discard, discount).
        ''' Value is stored in the database field goodsoperations.TotalValue.</remarks>
        Public Property TotalValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_TotalValue)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_TotalValue) <> CRound(value) Then
                    _TotalValue = CRound(value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a total balance change of the <see cref="Goods.Warehouse.WarehouseAccount">
        ''' warehouse account</see>. 
        ''' </summary>
        ''' <remarks>A positive number represents debit balance, a negative number represents credit balance.
        ''' Value is stored in the database field goodsoperations.AccountGeneral.</remarks>
        Public Property AccountGeneral() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_AccountGeneral)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_AccountGeneral) <> CRound(value) Then
                    _AccountGeneral = CRound(value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a total balance change of the <see cref="GoodsItem.AccountSalesNetCosts">
        ''' sales net costs account</see>. 
        ''' </summary>
        ''' <remarks>Only applicable when the goods are accounted by the
        ''' <see cref="GoodsAccountingMethod.Periodic">Periodic</see> method.
        ''' A positive number represents debit balance, a negative number represents credit balance.
        ''' Value is stored in the database field goodsoperations.AccountSalesNetCosts.</remarks>
        Public Property AccountSalesNetCosts() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_AccountSalesNetCosts)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_AccountSalesNetCosts) <> CRound(value) Then
                    _AccountSalesNetCosts = CRound(value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a total balance change of the <see cref="GoodsItem.AccountPurchases">
        ''' purchases account</see>. 
        ''' </summary>
        ''' <remarks>Only applicable when the goods are accounted by the
        ''' <see cref="GoodsAccountingMethod.Periodic">Periodic</see> method.
        ''' A positive number represents debit balance, a negative number represents credit balance.
        ''' Value is stored in the database field goodsoperations.AccountPurchases.</remarks>
        Public Property AccountPurchases() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_AccountPurchases)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_AccountPurchases) <> CRound(value) Then
                    _AccountPurchases = CRound(value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a total balance change of the <see cref="GoodsItem.AccountDiscounts">
        ''' discounts account</see>. 
        ''' </summary>
        ''' <remarks>Only applicable when the goods are accounted by the
        ''' <see cref="GoodsAccountingMethod.Periodic">Periodic</see> method.
        ''' A positive number represents debit balance, a negative number represents credit balance.
        ''' Value is stored in the database field goodsoperations.AccountDiscounts.</remarks>
        Public Property AccountDiscounts() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_AccountDiscounts)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_AccountDiscounts) <> CRound(value) Then
                    _AccountDiscounts = CRound(value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a total balance change of the <see cref="GoodsItem.AccountValueReduction">
        ''' value reduction account</see>. 
        ''' </summary>
        ''' <remarks>A positive number represents debit balance, a negative number represents credit balance.
        ''' Value is stored in the database field goodsoperations.AccountPriceCut.</remarks>
        Public Property AccountPriceCut() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_AccountPriceCut)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_AccountPriceCut) <> CRound(value) Then
                    _AccountPriceCut = CRound(value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets an operation specific <see cref="General.Account.ID">account</see>.
        ''' </summary>
        ''' <remarks>Typicaly it is an actual sales net costs account for the goods
        ''' that are accounted by the <see cref="GoodsAccountingMethod.Persistent">Persistent</see>
        ''' method (because <see cref="GoodsItem.AccountSalesNetCosts">GoodsItem.AccountSalesNetCosts</see>
        ''' in this case only provides for the default account not actual).
        ''' Value is stored in the database field goodsoperations.AccountOperation.</remarks>
        Public Property AccountOperation() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _AccountOperation
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Long)
                If _AccountOperation <> value Then
                    _AccountOperation = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a total balance change of the <see cref="AccountOperation">
        ''' operation specific account</see>.
        ''' </summary>
        ''' <remarks>A positive number represents debit balance, a negative number represents credit balance.
        ''' Value is stored in the database field goodsoperations.AccountOperationValue.</remarks>
        Public Property AccountOperationValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return CRound(_AccountOperationValue)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Double)
                If CRound(_AccountOperationValue) <> CRound(value) Then
                    _AccountOperationValue = CRound(value)
                End If
            End Set
        End Property


        ''' <summary>
        ''' Gets or sets an <see cref="General.JournalEntry.ID">ID of the journal entry</see>
        ''' that is encapsulated by (or associated with) the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.JournalEntryID.</remarks>
        Public Property JournalEntryID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _JournalEntryID
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Set(ByVal value As Integer)
                If _JournalEntryID <> value Then
                    _JournalEntryID = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets a <see cref="General.JournalEntry.[Date]">date of the journal entry</see>
        ''' that is encapsulated by (or associated with) the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.JournalEntryID.</remarks>
        Public ReadOnly Property JournalEntryDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _JournalEntryDate
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="General.JournalEntry.DocNumber">document number of 
        ''' the journal entry</see> that is encapsulated by (or associated with) the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.JournalEntryID.</remarks>
        Public ReadOnly Property JournalEntryDocNo() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _JournalEntryDocNo.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="General.JournalEntry.Content">content of the journal entry</see>
        ''' that is encapsulated by (or associated with) the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.JournalEntryID.</remarks>
        Public ReadOnly Property JournalEntryContent() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _JournalEntryContent.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="General.JournalEntry.Person">person of the journal entry</see>
        ''' that is encapsulated by (or associated with) the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.JournalEntryID.</remarks>
        Public ReadOnly Property JournalEntryRelatedPerson() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _JournalEntryRelatedPerson.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="General.JournalEntry.DocType">document type of the journal entry</see>
        ''' that is encapsulated by (or associated with) the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.JournalEntryID.</remarks>
        Public ReadOnly Property JournalEntryType() As DocumentType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _JournalEntryType
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="General.JournalEntry.DocType">localized human readable 
        ''' document type of the journal entry</see> that is encapsulated by 
        ''' (or associated with) the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.JournalEntryID.</remarks>
        Public ReadOnly Property JournalEntryTypeHumanReadable() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _JournalEntryTypeHumanReadable.Trim
            End Get
        End Property

        ''' <summary>
        ''' Gets a <see cref="ActiveReports.JournalEntryInfo.BookEntries">description 
        ''' of the book entries of the journal entry</see> that is encapsulated by 
        ''' (or associated with) the operation.
        ''' </summary>
        ''' <remarks>Value is stored in the database field goodsoperations.JournalEntryID.</remarks>
        Public ReadOnly Property JournalEntryCorrespondence() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
            Get
                Return _JournalEntryCorrespondence.Trim
            End Get
        End Property


        ''' <summary>
        ''' Saves the operation to a database and returns a saved operation instance.
        ''' </summary>
        ''' <param name="updateFinancialData">whether to update operation's financial data</param>
        ''' <param name="updateWarehouse">whether to update operation's warehouse 
        ''' if financial data is not updated</param>
        ''' <remarks></remarks>
        Friend Function Save(ByVal updateFinancialData As Boolean, ByVal updateWarehouse As Boolean) As OperationPersistenceObject

            Dim result As OperationPersistenceObject = Clone(Of OperationPersistenceObject)(Me)

            If result._ID > 0 Then
                result.Update(updateFinancialData, updateWarehouse)
            Else
                result.Insert()
            End If

            Return result

        End Function


        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.Goods_OperationPersistenceObject_ToString,
                _OperationDate.ToString("yyyy-MM-dd"),
                Utilities.ConvertLocalizedName(_OperationType), _DocNo, _ID.ToString())
        End Function


#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets a new OperationPersistenceObject instance of requested type.
        ''' </summary>
        ''' <param name="newOperationType">a type of the operation to create</param>
        ''' <param name="operationGoodsID">an <see cref="GoodsItem.ID">ID of the goods</see>
        ''' that the operation operates with</param>
        ''' <remarks></remarks>
        Friend Shared Function NewOperationPersistenceObject(
            ByVal newOperationType As GoodsOperationType,
            ByVal operationGoodsID As Integer) As OperationPersistenceObject
            Return New OperationPersistenceObject(newOperationType, operationGoodsID)
        End Function

        ''' <summary>
        ''' Gets an existing OperationPersistenceObject instance from a database.
        ''' </summary>
        ''' <param name="operationID">an <see cref="ID">ID</see> of the operation to fetch</param>
        ''' <param name="expectedType">an expected type of the operation</param>
        ''' <param name="throwOnTypeMismatch">whether to throw an exception 
        ''' if the actual operation type does not match the expected type</param>
        ''' <remarks></remarks>
        Friend Shared Function GetOperationPersistenceObject(ByVal operationID As Integer,
            ByVal expectedType As GoodsOperationType,
            Optional ByVal throwOnTypeMismatch As Boolean = True) As OperationPersistenceObject
            Return New OperationPersistenceObject(operationID, expectedType, throwOnTypeMismatch)
        End Function

        ''' <summary>
        ''' Gets an existing OperationPersistenceObject instance tu use for save operation.
        ''' Only fetches current operation data (and performs type and type checks) 
        ''' if the parent operation is not a child.
        ''' </summary>
        ''' <param name="operationID">an <see cref="ID">ID</see> of the operation to fetch</param>
        ''' <param name="expectedType">an expected type of the operation</param>
        ''' <param name="currentUpdateDate">current operation last update timestamp</param>
        ''' <param name="isChild">whether the parent operation is a child operation</param>
        ''' <remarks></remarks>
        Friend Shared Function GetOperationPersistenceObjectForSave(ByVal operationID As Integer,
            ByVal expectedType As GoodsOperationType, ByVal currentUpdateDate As DateTime,
            ByVal isChild As Boolean) As OperationPersistenceObject
            Return New OperationPersistenceObject(operationID, expectedType, currentUpdateDate, isChild)
        End Function

        ''' <summary>
        ''' Gets a list of child OperationPersistenceObject instances
        ''' for a complex goods operation from a database
        ''' </summary>
        ''' <param name="complexOperationID">an <see cref="ComplexOperationPersistenceObject.ID">
        ''' ID of the complex goods operation</see> to fetch the child operations for</param>
        ''' <remarks></remarks>
        Friend Shared Function GetOperationPersistenceObjectList(
            ByVal complexOperationID As Integer) As List(Of OperationPersistenceObject)

            Dim result As List(Of OperationPersistenceObject)

            Dim myComm As New SQLCommand("FetchOperationPersistenceObjectList")
            myComm.AddParam("?OD", complexOperationID)

            Using myData As DataTable = myComm.Fetch

                result = New List(Of OperationPersistenceObject)

                For Each dr As DataRow In myData.Rows

                    result.Add(New OperationPersistenceObject(dr))

                Next

            End Using

            Return result

        End Function

        ''' <summary>
        ''' Deletes an existing OperationPersistenceObject instance from a database.
        ''' </summary>
        ''' <param name="operationID">an <see cref="ID">ID</see> of the operation to delete</param>
        ''' <param name="hasChildConsignments">whether the operation has child 
        ''' goods consignments (that should also be deleted)</param>
        ''' <param name="hasChildConsignmentDiscards">whether the operation has child 
        ''' goods consignments discards (that should also be deleted)</param>
        ''' <remarks></remarks>
        Friend Shared Sub Delete(ByVal operationID As Integer,
            ByVal hasChildConsignments As Boolean, ByVal hasChildConsignmentDiscards As Boolean)

            If hasChildConsignmentDiscards Then
                DeleteConsignmentDiscards(operationID)
            End If

            If hasChildConsignments Then
                DeleteConsignments(operationID)
            End If

            DeleteSelf(operationID)

        End Sub


        Private Sub New()

        End Sub

        Private Sub New(ByVal newOperationType As GoodsOperationType,
            ByVal operationGoodsID As Integer)
            _OperationType = newOperationType
            _GoodsID = operationGoodsID
        End Sub

        Private Sub New(ByVal operationID As Integer,
            ByVal expectedType As GoodsOperationType,
            ByVal throwOnTypeMismatch As Boolean)
            Fetch(operationID, expectedType, throwOnTypeMismatch)
        End Sub

        Private Sub New(ByVal operationID As Integer,
            ByVal expectedType As GoodsOperationType, ByVal currentUpdateDate As DateTime,
            ByVal isChild As Boolean)
            If isChild Then
                _ID = operationID
                _OperationType = expectedType
            Else
                Fetch(operationID, expectedType, True)
                If _UpdateDate <> currentUpdateDate Then
                    Throw New Exception(My.Resources.Common_UpdateDateHasChanged)
                End If
            End If
        End Sub

        Private Sub New(ByVal dr As DataRow)
            Fetch(dr)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Fetch(ByVal operationID As Integer, ByVal expectedType As GoodsOperationType,
            ByVal throwOnTypeMismatch As Boolean)

            If Not operationID > 0 Then
                Throw New ArgumentNullException("operationID", Goods_OperationPersistenceObject_OperationIdNull)
            End If

            Dim myComm As New SQLCommand("FetchOperationPersistenceObject")
            myComm.AddParam("?OD", operationID)

            Using myData As DataTable = myComm.Fetch

                If myData.Rows.Count < 1 Then Throw New Exception(String.Format(
                    My.Resources.Common_ObjectNotFound, My.Resources.Goods_OperationPersistenceObject_TypeName,
                    operationID.ToString()))

                Dim dr As DataRow = myData.Rows(0)

                Fetch(dr)

            End Using

            If throwOnTypeMismatch AndAlso expectedType <> _OperationType Then
                Throw New Exception(String.Format(Goods_OperationPersistenceObject_OperationTypeMismatch,
                    _ID.ToString, ConvertLocalizedName(_OperationType),
                    ConvertLocalizedName(expectedType)))
            End If

        End Sub

        Private Sub Fetch(ByVal dr As DataRow)

            _ID = CIntSafe(dr.Item(0), 0)
            _OperationDate = CDateSafe(dr.Item(1), Today)
            _OperationType = Utilities.ConvertDatabaseID(Of GoodsOperationType) _
                (CIntSafe(dr.Item(2), 1))
            _JournalEntryID = CIntSafe(dr.Item(3), 0)
            _DocNo = CStrSafe(dr.Item(4)).Trim
            _Content = CStrSafe(dr.Item(5)).Trim
            _GoodsID = CIntSafe(dr.Item(6), 0)
            _Amount = CDblSafe(dr.Item(7), ROUNDAMOUNTGOODS, 0)
            _AmountInWarehouse = CDblSafe(dr.Item(8), ROUNDAMOUNTGOODS, 0)
            _UnitValue = CDblSafe(dr.Item(9), ROUNDUNITGOODS, 0)
            _TotalValue = CDblSafe(dr.Item(10), 2, 0)
            _AccountGeneral = CDblSafe(dr.Item(11), 2, 0)
            _AccountSalesNetCosts = CDblSafe(dr.Item(12), 2, 0)
            _AccountPurchases = CDblSafe(dr.Item(13), 2, 0)
            _AccountDiscounts = CDblSafe(dr.Item(14), 2, 0)
            _AccountPriceCut = CDblSafe(dr.Item(15), 2, 0)
            _ComplexOperationID = CIntSafe(dr.Item(16), 0)
            If _ComplexOperationID > 0 Then
                _ComplexOperationType = Utilities.ConvertDatabaseID(Of GoodsComplexOperationType) _
                    (CIntSafe(dr.Item(17), 1))
                _ComplexOperationHumanReadable = Utilities.ConvertLocalizedName(
                    _ComplexOperationType)
            End If
            _InsertDate = CTimeStampSafe(dr.Item(18))
            _UpdateDate = CTimeStampSafe(dr.Item(19))
            _AccountOperation = CLongSafe(dr.Item(20), 0)
            _AccountOperationValue = CDblSafe(dr.Item(21), 2, 0)
            _JournalEntryDate = CDateSafe(dr.Item(22), Today)
            _JournalEntryContent = CStrSafe(dr.Item(23)).Trim
            _JournalEntryType = Utilities.ConvertDatabaseCharID(Of DocumentType) _
                (CStrSafe(dr.Item(24)).Trim)
            _JournalEntryTypeHumanReadable = Utilities.ConvertLocalizedName(_JournalEntryType)
            _JournalEntryDocNo = CStrSafe(dr.Item(25)).Trim
            _JournalEntryCorrespondence = CStrSafe(dr.Item(26)).Trim
            _JournalEntryRelatedPerson = CStrSafe(dr.Item(27)).Trim
            _AmountInPurchases = CDblSafe(dr.Item(28), ROUNDAMOUNTGOODS, 0)
            _WarehouseID = CIntSafe(dr.Item(29), 0)
            _Warehouse = WarehouseInfo.GetWarehouseInfo(dr, 29)
            _GoodsInfo = GoodsSummary.GetGoodsSummary(dr, 34)

            If Not StringIsNullOrEmpty(_JournalEntryDocNo) Then
                _DocNo = _JournalEntryDocNo.Trim
            End If

        End Sub


        Private Sub Insert()

            Dim myComm As New SQLCommand("InsertOperationPersistenceObject")
            AddWithParamsGeneral(myComm)
            AddWithParamsFinancial(myComm)
            myComm.AddParam("?AE", _GoodsID)
            myComm.AddParam("?AO", _ComplexOperationID)
            myComm.AddParam("?AP", Utilities.ConvertDatabaseID(_OperationType))

            myComm.Execute()

            _ID = Convert.ToInt32(myComm.LastInsertID)

        End Sub

        Private Sub Update(ByVal updateFinancialData As Boolean, ByVal updateWarehouse As Boolean)

            Dim myComm As SQLCommand

            If updateFinancialData Then
                myComm = New SQLCommand("UpdateOperationPersistenceObjectFull")
                AddWithParamsFinancial(myComm)
            ElseIf updateWarehouse Then
                myComm = New SQLCommand("UpdateOperationPersistenceObjectGeneralAndWarehouse")
                myComm.AddParam("?AF", _WarehouseID)
            Else
                myComm = New SQLCommand("UpdateOperationPersistenceObjectGeneral")
            End If
            AddWithParamsGeneral(myComm)
            myComm.AddParam("?CD", _ID)

            myComm.Execute()

        End Sub

        Private Sub AddWithParamsGeneral(ByRef myComm As SQLCommand)

            myComm.AddParam("?AA", _OperationDate.Date)
            myComm.AddParam("?AB", _JournalEntryID)
            myComm.AddParam("?AC", _DocNo.Trim)
            myComm.AddParam("?AD", _Content.Trim)
            myComm.AddParam("?AT", _AccountOperation)

            _UpdateDate = GetCurrentTimeStamp()
            If Not _ID > 0 Then _InsertDate = _UpdateDate
            myComm.AddParam("?AR", _UpdateDate.ToUniversalTime())

        End Sub

        Private Sub AddWithParamsFinancial(ByRef myComm As SQLCommand)

            myComm.AddParam("?AF", _WarehouseID)
            myComm.AddParam("?AG", CRound(_Amount, ROUNDAMOUNTGOODS))
            myComm.AddParam("?AH", CRound(_UnitValue, ROUNDUNITGOODS))
            myComm.AddParam("?AI", CRound(_TotalValue))
            myComm.AddParam("?AJ", CRound(_AccountGeneral))
            myComm.AddParam("?AK", CRound(_AccountSalesNetCosts))
            myComm.AddParam("?AL", CRound(_AccountPurchases))
            myComm.AddParam("?AM", CRound(_AccountDiscounts))
            myComm.AddParam("?AN", CRound(_AccountPriceCut))
            myComm.AddParam("?AQ", CRound(_AccountOperationValue))
            myComm.AddParam("?AU", CRound(_AmountInWarehouse, ROUNDAMOUNTGOODS))
            myComm.AddParam("?AV", CRound(_AmountInPurchases, ROUNDAMOUNTGOODS))

        End Sub


        Private Shared Sub DeleteSelf(ByVal operationID As Integer)

            Dim myComm As New SQLCommand("DeleteGoodsOperation")
            myComm.AddParam("?OD", operationID)

            myComm.Execute()

        End Sub

        Friend Shared Sub DeleteConsignments(ByVal operationID As Integer)

            Dim myComm As New SQLCommand("DeleteConsignmentsByParent")
            myComm.AddParam("?OD", operationID)

            myComm.Execute()

        End Sub

        Friend Shared Sub DeleteConsignmentDiscards(ByVal operationID As Integer)

            Dim myComm As New SQLCommand("DeleteConsignmentDiscardsByParent")
            myComm.AddParam("?OD", operationID)

            myComm.Execute()

        End Sub

#End Region

    End Class

End Namespace

