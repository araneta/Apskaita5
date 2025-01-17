Imports ApskaitaObjects.HelperLists
Imports ApskaitaObjects.General
Imports ApskaitaObjects.Documents.InvoiceAdapters
Imports ApskaitaObjects.Attributes

Namespace Documents

    ''' <summary>
    ''' Represents an invoice received by the company.
    ''' </summary>
    ''' <remarks>Encapsulates a <see cref="General.JournalEntry">JournalEntry</see> of type <see cref="DocumentType.InvoiceReceived">DocumentType.InvoiceReceived</see>.
    ''' Values are stored in the database table invoicesreceived.</remarks>
    <Serializable()> _
    Public NotInheritable Class InvoiceReceived
        Inherits BusinessBase(Of InvoiceReceived)
        Implements IIsDirtyEnough, IValidationMessageProvider, ISyncable

#Region " Business Methods "

        Private _ID As Integer = -1
        Private _ChronologyValidator As ComplexChronologicValidator
        Private _Supplier As PersonInfo = Nothing
        Private _AccountSupplier As Long = 0
        Private _Date As Date = Today
        Private _Number As String = ""
        Private _Content As String = ""
        Private _Type As InvoiceType = InvoiceType.Normal
        Private WithEvents _InvoiceItems As InvoiceReceivedItemList
        Private _CurrencyCode As String = GetCurrentCompany.BaseCurrency
        Private _CurrencyRate As Double = 1
        Private _CommentsInternal As String = ""
        Private _SumLTL As Double = 0
        Private _SumVatLTL As Double = 0
        Private _SumTotalLTL As Double = 0
        Private _Sum As Double = 0
        Private _SumVat As Double = 0
        Private _SumTotal As Double = 0
        Private _ExternalID As String = ""
        Private _IndirectVatSum As Double = 0
        Private _IndirectVatAccount As Long = 0
        Private _IndirectVatCostsAccount As Long = 0
        Private _IndirectVatDeclarationSchema As VatDeclarationSchemaInfo = Nothing
        Private _ActualDate As Date = Today
        Private _ActualDateIsApplicable As Boolean = False
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now

        ' used to implement automatic sort in datagridview
        <NotUndoable()> _
        <NonSerialized()> _
        Private _SortedList As SortedBindingList(Of InvoiceReceivedItem) = Nothing


        ''' <summary>
        ''' Gets <see cref="General.JournalEntry.ID">an ID of the journal entry</see> that is created by the invoice.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.ID.</remarks>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the invoice data was inserted into the database.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.InsertDate.</remarks>
        Public ReadOnly Property InsertDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _InsertDate
            End Get
        End Property

        ''' <summary>
        ''' Gets the date and time when the invoice data was last updated.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.UpdateDate.</remarks>
        Public ReadOnly Property UpdateDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UpdateDate
            End Get
        End Property

        ''' <summary>
        ''' Gets <see cref="IChronologicValidator">IChronologicValidator</see> object that contains business restraints on updating the invoice.
        ''' </summary>
        ''' <remarks>A <see cref="ComplexChronologicValidator">ComplexChronologicValidator</see> is used to validate a till order chronological business rules.
        ''' It aggregates a <see cref="SimpleChronologicValidator">SimpleChronologicValidator</see>
        ''' and <see cref="IChronologicValidator">IChronologicValidator</see> instances
        ''' owned by the <see cref="InvoiceReceivedItem.AttachedObjectValue">attached operations</see>.</remarks>
        Public ReadOnly Property ChronologyValidator() As ComplexChronologicValidator
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ChronologyValidator
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a <see cref="General.Person">person</see> who issued the invoice.
        ''' </summary>
        ''' <remarks>Use <see cref="HelperLists.PersonInfoList">PersonInfoList</see> as a datasource.
        ''' Value is handled by the encapsulated <see cref="General.JournalEntry.Person">journal entry Person property</see>.</remarks>
        <PersonFieldAttribute(ValueRequiredLevel.Mandatory)> _
        Public Property Supplier() As PersonInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Supplier
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As PersonInfo)
                CanWriteProperty(True)
                If Not (_Supplier Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _Supplier Is Nothing AndAlso Not value Is Nothing _
                    AndAlso _Supplier = value) Then

                    _Supplier = value
                    PropertyHasChanged()

                    If Not _Supplier Is Nothing AndAlso Not _Supplier.IsEmpty Then

                        If Not AccountSupplierIsReadOnly Then
                            AccountSupplier = _Supplier.AccountAgainstBankSupplyer
                        End If

                        If Not CurrencyCodeIsReadOnly Then
                            CurrencyCode = _Supplier.CurrencyCode
                        End If

                    End If

                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the <see cref="General.Account.ID">account</see> that is credited
        ''' by the total sum payable to the <see cref="Supplier">Supplier</see>.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.AccountSupplier.</remarks>
        <AccountField(ValueRequiredLevel.Mandatory, False, 2, 4)> _
        Public Property AccountSupplier() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountSupplier
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If AccountSupplierIsReadOnly Then Exit Property
                If _AccountSupplier <> value Then
                    _AccountSupplier = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the date of the invoice as it is put into the accounting registry.
        ''' </summary>
        ''' <remarks>Value is handled by the encapsulated <see cref="General.JournalEntry.Date">journal entry Date property</see>.</remarks>
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
                    _InvoiceItems.UpdateDate(value.Date)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a number (including serial part) of the invoice.
        ''' </summary>
        ''' <remarks>Value is handled by the encapsulated <see cref="General.JournalEntry.DocNumber">journal entry DocNumber property</see>.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 30)> _
        Public Property Number() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Number.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Number.Trim.ToUpper <> value.Trim.ToUpper Then
                    _Number = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a content (description) of the invoice.
        ''' </summary>
        ''' <remarks>Value is handled by the encapsulated <see cref="General.JournalEntry.Content">journal entry Content property</see>.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 255)> _
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
        ''' Gets the invoice lines (items) collection.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property InvoiceItems() As InvoiceReceivedItemList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _InvoiceItems
            End Get
        End Property

        ''' <summary>
        ''' Gets a sortable view of the invoice lines (items) collection.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property InvoiceItemsSorted() As Csla.SortedBindingList(Of InvoiceReceivedItem)
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _SortedList Is Nothing Then _SortedList = _
                    New SortedBindingList(Of InvoiceReceivedItem)(_InvoiceItems)
                Return _SortedList
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a currency of the invoice.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.CurrencyCode.</remarks>
        <CurrencyFieldAttribute(ValueRequiredLevel.Mandatory)> _
        Public Property CurrencyCode() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CurrencyCode.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)

                If CurrencyCodeIsReadOnly Then Exit Property

                If value Is Nothing Then value = ""

                If _CurrencyCode.Trim <> value.Trim Then

                    _CurrencyCode = value.Trim
                    PropertyHasChanged()

                    If Not CurrencyRateIsReadOnly Then

                        Dim newRate As Double
                        If IsBaseCurrency(_CurrencyCode, GetCurrentCompany.BaseCurrency) Then
                            newRate = 1
                        Else
                            newRate = 0
                        End If

                        If CRound(_CurrencyRate, ROUNDCURRENCYRATE) <> _
                            CRound(newRate, ROUNDCURRENCYRATE) Then

                            _CurrencyRate = CRound(newRate, ROUNDCURRENCYRATE)
                            PropertyHasChanged("CurrencyRate")

                        End If

                    End If

                    If CRound(_CurrencyRate, ROUNDCURRENCYRATE) > 0 Then
                        _InvoiceItems.UpdateCurrencyRate(_CurrencyRate, _CurrencyCode)
                    End If

                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a currency rate for the invoice currency.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.CurrencyRate.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, False, ROUNDCURRENCYRATE)> _
        Public Property CurrencyRate() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_CurrencyRate, ROUNDCURRENCYRATE)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If CurrencyRateIsReadOnly Then Exit Property
                If CRound(_CurrencyRate, ROUNDCURRENCYRATE) <> CRound(value, ROUNDCURRENCYRATE) Then
                    _CurrencyRate = CRound(value, ROUNDCURRENCYRATE)
                    PropertyHasChanged()
                    If CRound(_CurrencyRate, ROUNDCURRENCYRATE) > 0 Then
                        _InvoiceItems.UpdateCurrencyRate(_CurrencyRate, _CurrencyCode)
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets internal comments for the invoice (accountant comments of different kinds).
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.CommentsInternal.</remarks>
        <StringField(ValueRequiredLevel.Optional, 255)> _
        Public Property CommentsInternal() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CommentsInternal.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _CommentsInternal.Trim <> value.Trim Then
                    _CommentsInternal = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a type of the invoice.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.InvoiceType.</remarks>
        Public Property [Type]() As InvoiceType
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Type
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As InvoiceType)
                CanWriteProperty(True)
                If _Type <> value Then
                    _Type = value
                    PropertyHasChanged()
                    PropertyHasChanged("TypeHumanReadable")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a type of the invoice as a human readable string.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.InvoiceType.</remarks>
        <LocalizedEnumField(GetType(InvoiceType), False, "")> _
        Public Property TypeHumanReadable() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Utilities.ConvertLocalizedName(_Type)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                Dim newValue As InvoiceType = InvoiceType.Normal
                Try
                    newValue = Utilities.ConvertLocalizedName(Of InvoiceType)(value)
                Catch ex As Exception
                End Try
                If newValue <> _Type Then
                    _Type = newValue
                    PropertyHasChanged()
                    PropertyHasChanged("Type")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a value of the indirect VAT within the invoice 
        ''' (VAT that is payable by the company, not by the supplier).
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.IndirectVatSum.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, False, 2)> _
        Public Property IndirectVatSum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_IndirectVatSum)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If IndirectVatSumIsReadOnly Then Exit Property
                If value < 0 Then value = 0
                If CRound(_IndirectVatSum) <> CRound(value) Then
                    _IndirectVatSum = CRound(value)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the <see cref="General.Account.ID">account</see> that is credited 
        ''' by the <see cref="IndirectVatSum">IndirectVatSum</see> (an obligation to pay VAT to the state).
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.IndirectVatAccount.</remarks>
        <AccountField(ValueRequiredLevel.Mandatory, False, 4)> _
        Public Property IndirectVatAccount() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IndirectVatAccount
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If IndirectVatAccountIsReadOnly Then Exit Property
                If _IndirectVatAccount <> value Then
                    _IndirectVatAccount = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the <see cref="General.Account.ID">account</see> that is debited 
        ''' by the <see cref="IndirectVatSum">IndirectVatSum</see> 
        ''' (company costs of the obligation to pay VAT to the state).
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.IndirectVatCostsAccount.</remarks>
        <AccountField(ValueRequiredLevel.Mandatory, False, 2, 6)> _
        Public Property IndirectVatCostsAccount() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IndirectVatCostsAccount
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If IndirectVatCostsAccountIsReadOnly Then Exit Property
                If _IndirectVatCostsAccount <> value Then
                    _IndirectVatCostsAccount = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the applicable VAT declaration schema for the indirect VAT.
        ''' </summary>
        ''' <remarks>Value is stored in the database table invoicesreceived.DeclarationSchemaID.</remarks>
        <VatDeclarationSchemaFieldAttribute(ValueRequiredLevel.Mandatory, TradedItemType.All)> _
        Public Property IndirectVatDeclarationSchema() As VatDeclarationSchemaInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IndirectVatDeclarationSchema
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As VatDeclarationSchemaInfo)
                CanWriteProperty(True)
                If IndirectVatDeclarationSchemaIsReadOnly Then Exit Property
                If Not (_IndirectVatDeclarationSchema Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _IndirectVatDeclarationSchema Is Nothing AndAlso Not value Is Nothing _
                    AndAlso _IndirectVatDeclarationSchema = value) Then
                    _IndirectVatDeclarationSchema = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the actual date of the invoice (if it is different from 
        ''' <see cref="Date">the registration date)</see>.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.ActualDate.</remarks>
        Public Property ActualDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ActualDate
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Date)
                CanWriteProperty(True)
                If Not _ActualDateIsApplicable Then Exit Property
                If _ActualDate.Date <> value.Date Then
                    _ActualDate = value.Date
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets whether the actual date of the invoice is different from 
        ''' <see cref="Date">the registration date)</see>.
        ''' </summary>
        ''' <remarks>Value is stored in the database field invoicesreceived.ActualDate 
        ''' (null value corresponds to false, real value corresponds to true).</remarks>
        Public Property ActualDateIsApplicable() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ActualDateIsApplicable
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _ActualDateIsApplicable <> value Then
                    _ActualDateIsApplicable = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets an external invoice ID (when the invoice data is imported from an external data source).
        ''' </summary>
        ''' <remarks>Should be unique or empty. If using multiple data sources (systems, applications),
        ''' each datasource should have unique prefix.
        ''' Value is stored in the database field invoicesreceived.ExternalID.</remarks>
        Public ReadOnly Property ExternalID() As String _
            Implements ISyncable.ExternalID
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ExternalID.Trim
            End Get
        End Property


        ''' <summary>
        ''' Gets the total value of the goods/services bought (excluding VAT) in the base currency.
        ''' </summary>
        ''' <remarks>Sum over <see cref="InvoiceReceivedItem.SumLTL">the items' SumLTL property</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, True, 2)> _
        Public ReadOnly Property SumLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumLTL)
            End Get
        End Property

        ''' <summary>
        ''' Gets the total VAT value in the base currency.
        ''' </summary>
        ''' <remarks>Sum over <see cref="InvoiceReceivedItem.SumVatLTL">the items' SumVatLTL property</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, True, 2)> _
        Public ReadOnly Property SumVatLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumVatLTL)
            End Get
        End Property

        ''' <summary>
        ''' Gets the total value of the goods/services bought (including VAT) in the base currency.
        ''' </summary>
        ''' <remarks>Sum over <see cref="InvoiceReceivedItem.SumTotalLTL">the items' SumTotalLTL property</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, True, 2)> _
        Public ReadOnly Property SumTotalLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumTotalLTL)
            End Get
        End Property

        ''' <summary>
        ''' Gets the total value of the goods/services bought (excluding VAT)
        ''' in <see cref="CurrencyCode">the original invoice currency</see>.
        ''' </summary>
        ''' <remarks>Sum over <see cref="InvoiceReceivedItem.Sum">the items' Sum property</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, True, 2)> _
        Public ReadOnly Property Sum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Sum)
            End Get
        End Property

        ''' <summary>
        ''' Gets the total VAT value in <see cref="CurrencyCode">the original invoice currency</see>.
        ''' </summary>
        ''' <remarks>Sum over <see cref="InvoiceReceivedItem.SumVat">the items' SumVat property</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, True, 2)> _
        Public ReadOnly Property SumVat() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumVat)
            End Get
        End Property

        ''' <summary>
        ''' Gets the total value of the goods/services bought (including VAT)
        ''' in <see cref="CurrencyCode">the original invoice currency</see>.
        ''' </summary>
        ''' <remarks>Sum over <see cref="InvoiceReceivedItem.SumTotal">the items' SumTotal property</see>.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, True, 2)> _
        Public ReadOnly Property SumTotal() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumTotal)
            End Get
        End Property


        ''' <summary>
        ''' Whether <see cref="AccountSupplier">AccountSupplier</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property AccountSupplierIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _ChronologyValidator.FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether <see cref="CurrencyCode">CurrencyCode</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrencyCodeIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _ChronologyValidator.FinancialDataCanChange _
                    OrElse Not _ChronologyValidator.ChildrenFinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether <see cref="CurrencyRate">CurrencyRate</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrencyRateIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _ChronologyValidator.FinancialDataCanChange _
                    OrElse Not _ChronologyValidator.ChildrenFinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether <see cref="IndirectVatSum">IndirectVatSum</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property IndirectVatSumIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _ChronologyValidator.FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether <see cref="IndirectVatAccount">IndirectVatAccount</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property IndirectVatAccountIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _ChronologyValidator.FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether <see cref="IndirectVatCostsAccount">IndirectVatCostsAccount</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property IndirectVatCostsAccountIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _ChronologyValidator.FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether <see cref="IndirectVatDeclarationSchema">IndirectVatDeclarationSchema</see> property is readonly.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property IndirectVatDeclarationSchemaIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not _ChronologyValidator.FinancialDataCanChange
            End Get
        End Property


        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return (_InvoiceItems.Count > 0 _
                    OrElse Not StringIsNullOrEmpty(_Content) _
                    OrElse Not StringIsNullOrEmpty(_CommentsInternal))
            End Get
        End Property

        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _InvoiceItems.IsDirty
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean _
            Implements IValidationMessageProvider.IsValid
            Get
                Return MyBase.IsValid AndAlso _InvoiceItems.IsValid
            End Get
        End Property



        ''' <summary>
        ''' Adds a new <see cref="InvoiceReceivedItem">InvoiceReceivedItem</see> with 
        ''' an <see cref="InvoiceAdapters.IInvoiceAdapter">attached operation</see>.
        ''' </summary>
        ''' <param name="newAttachedObject">An attached operation.</param>
        ''' <param name="regionalizedDictionary">A <see cref="RegionalInfoDictionary">RegionalInfoDictionary</see> 
        ''' instance to be used for setting regional names and prices.</param>
        ''' <returns>The <see cref="InvoiceReceivedItem">InvoiceReceivedItem</see> instance 
        ''' that was added to the invoice.</returns>
        ''' <remarks></remarks>
        Public Function AttachNewObject(ByVal newAttachedObject As IInvoiceAdapter, _
            Optional ByVal regionalizedDictionary As RegionalInfoDictionary = Nothing) As InvoiceReceivedItem

            If newAttachedObject Is Nothing Then
                Throw New Exception(My.Resources.Documents_InvoiceReceived_NewAttachedOperationIsNull)
            End If

            If Not _ChronologyValidator.BaseValidator.FinancialDataCanChange Then
                Throw New Exception(String.Format(My.Resources.Documents_InvoiceReceived_FinancialDataCannotChangeFull, _
                    vbCrLf, _ChronologyValidator.BaseValidator.FinancialDataCanChangeExplanation))
            End If

            _InvoiceItems.CheckIfNewAdapterCompatibleWithTheExistingAdapters(newAttachedObject)

            newAttachedObject.SetInvoiceDate(_Date)

            Dim item As InvoiceReceivedItem = InvoiceReceivedItem.NewInvoiceReceivedItem(newAttachedObject)
            _InvoiceItems.Add(item)

            If Not regionalizedDictionary Is Nothing Then

                Dim asRegionalizable As IRegionalDataObject = Nothing
                Try
                    asRegionalizable = DirectCast(item.AttachedObjectValue, IRegionalDataObject)
                Catch ex As Exception
                End Try

                If Not asRegionalizable Is Nothing Then
                    regionalizedDictionary.GetPurchasePrice(asRegionalizable.RegionalObjectType, _
                        asRegionalizable.RegionalObjectID, _CurrencyCode, item.UnitValue)
                End If

            End If

            _ChronologyValidator.MergeNewValidationItem(newAttachedObject.ChronologyValidator)

            Return item

        End Function


        ''' <summary>
        ''' Gets invoice data as a data transfer object.
        ''' </summary>
        ''' <param name="systemGuid">An ID of the system that generates the data transfer object.
        ''' Used to distinguish data transfer within and outside a system (an application instance).
        ''' Use <see cref="System.Guid">Guid.ToString</see> to generate an application instance Guid.</param>
        ''' <remarks>Used to implement data exchange with other applications.</remarks>
        Public Function GetInvoiceInfo(ByVal systemGuid As String) As InvoiceInfo.InvoiceInfo

            Dim result As New InvoiceInfo.InvoiceInfo

            result.AddDateToNumberOptionWasUsed = False
            result.CommentsInternal = Me._CommentsInternal
            result.Content = Me._Content
            result.CurrencyCode = Me._CurrencyCode
            result.CurrencyRate = Me._CurrencyRate
            result.CustomInfo = ""
            result.CustomInfoAltLng = ""
            result.Date = Me._Date
            result.Discount = 0
            result.DiscountLTL = 0
            result.DiscountVat = 0
            result.DiscountVatLTL = 0
            result.ExternalID = Me._ExternalID
            result.FullNumber = Me._Number
            result.ID = Me._ID.ToString
            result.LanguageCode = LanguageCodeLith.Trim.ToUpper
            result.Number = 0
            result.NumbersInInvoice = 0
            result.ProjectCode = ""
            result.Serial = ""
            result.SystemGuid = systemGuid
            result.Sum = Me._Sum
            result.SumLTL = Me._SumLTL
            result.SumReceived = 0
            result.SumTotal = Me._SumTotal
            result.SumTotalLTL = Me._SumTotalLTL
            result.SumVat = Me._SumVat
            result.SumVatLTL = Me._SumVatLTL
            result.UpdateDate = Me._UpdateDate
            result.VatExemptions = ""
            result.VatExemptionsAltLng = ""

            If Not _Supplier Is Nothing AndAlso _Supplier.ID > 0 Then
                result.Payer.Address = _Supplier.Address
                result.Payer.BalanceAtBegining = 0
                result.Payer.BreedCode = _Supplier.InternalCode
                result.Payer.Code = _Supplier.Code
                result.Payer.CodeVAT = _Supplier.CodeVAT
                result.Payer.Contacts = _Supplier.ContactInfo
                result.Payer.CurrencyCode = _Supplier.CurrencyCode
                result.Payer.Email = _Supplier.Email
                result.Payer.ExternalID = ""
                result.Payer.ID = _Supplier.ID.ToString
                result.Payer.IsClient = _Supplier.IsClient
                result.Payer.IsCodeLocal = False
                result.Payer.IsNaturalPerson = _Supplier.IsNaturalPerson
                result.Payer.IsObsolete = _Supplier.IsObsolete
                result.Payer.IsSupplier = _Supplier.IsSupplier
                result.Payer.IsWorker = _Supplier.IsWorker
                result.Payer.LanguageCode = _Supplier.LanguageCode
                result.Payer.Name = _Supplier.Name
                result.Payer.VatExemption = ""
                result.Payer.VatExemptionAltLng = ""
            End If

            result.InvoiceItems = Me._InvoiceItems.GetInvoiceItemInfoList

            Return result

        End Function


        Public Function GetAllBrokenRules() As String _
            Implements IValidationMessageProvider.GetAllBrokenRules
            Dim result As String = ""
            If Not MyBase.IsValid Then
                result = Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error)
            End If
            If Not _InvoiceItems.IsValid Then
                result = AddWithNewLine(result, _InvoiceItems.GetAllBrokenRules, False)
            End If
            Return result
        End Function

        Public Function GetAllWarnings() As String _
            Implements IValidationMessageProvider.GetAllWarnings
            Dim result As String = ""
            If MyBase.BrokenRulesCollection.WarningCount > 0 Then
                result = Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning)
            End If
            If _InvoiceItems.HasWarnings() Then
                result = AddWithNewLine(result, _InvoiceItems.GetAllWarnings, False)
            End If
            Return result
        End Function

        Public Function HasWarnings() As Boolean _
            Implements IValidationMessageProvider.HasWarnings
            Return (MyBase.BrokenRulesCollection.WarningCount > 0 _
                OrElse _InvoiceItems.HasWarnings())
        End Function


        Public Overrides Function Save() As InvoiceReceived

            Me.ValidationRules.CheckRules()
            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            Return MyBase.Save

        End Function


        ''' <summary>
        ''' Helper method to set <see cref="IndirectVatSum">IndirectVatSum</see> property
        ''' by multiplying <see cref="ApskaitaObjects.Settings.CompanyInfo.GetDefaultRate">
        ''' company's default VAT rate</see> by the <see cref="SumLTL">SumLTL</see> property value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CalculateIndirectVat()
            If Not _ChronologyValidator.BaseValidator.FinancialDataCanChange Then
                Throw New Exception(String.Format(My.Resources.Documents_InvoiceReceived_FinancialDataCannotChange, _
                    vbCrLf, _ChronologyValidator.BaseValidator.FinancialDataCanChangeExplanation))
            End If
            _IndirectVatSum = CRound(_SumLTL * GetCurrentCompany().GetDefaultRate(DefaultRateType.Vat) / 100)
            PropertyHasChanged("IndirectVatSum")
        End Sub


        Private Sub InvoiceItems_Changed(ByVal sender As Object, _
            ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _InvoiceItems.ListChanged

            CalculateSubTotals(True)

            If e.ListChangedType = ComponentModel.ListChangedType.ItemDeleted Then

                _ChronologyValidator = ComplexChronologicValidator.GetComplexChronologicValidator( _
                    _ChronologyValidator, _InvoiceItems.GetChronologyValidators())

            End If

        End Sub

        Private Sub CalculateSubTotals(ByVal raisePropertyChangedEvents As Boolean)

            _SumLTL = 0
            _SumVatLTL = 0
            _Sum = 0
            _SumVat = 0
                        _
            For Each i As InvoiceReceivedItem In _InvoiceItems
                _SumLTL = CRound(_SumLTL + i.SumLTL)
                _SumVatLTL = CRound(_SumVatLTL + i.SumVatLTL)
                _Sum = CRound(_Sum + i.Sum)
                _SumVat = CRound(_SumVat + i.SumVat)
            Next

            _SumTotalLTL = CRound(_SumLTL + _SumVatLTL)
            _SumTotal = CRound(_Sum + _SumVat)

            If raisePropertyChangedEvents Then
                PropertyHasChanged("SumLTL")
                PropertyHasChanged("SumVatLTL")
                PropertyHasChanged("Sum")
                PropertyHasChanged("SumVat")
                PropertyHasChanged("SumTotalLTL")
                PropertyHasChanged("SumTotal")
            End If

        End Sub

        ''' <summary>
        ''' Helper method. Takes care of child lists loosing their handlers.
        ''' </summary>
        Protected Overrides Function GetClone() As Object
            Dim result As InvoiceReceived = DirectCast(MyBase.GetClone(), InvoiceReceived)
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
        ''' Helper method. Takes care of child lists loosing their handlers.
        ''' </summary>
        Friend Sub RestoreChildListsHandles()
            Try
                RemoveHandler _InvoiceItems.ListChanged, AddressOf InvoiceItems_Changed
            Catch ex As Exception
            End Try
            AddHandler _InvoiceItems.ListChanged, AddressOf InvoiceItems_Changed
        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _ID
        End Function

        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.Documents_InvoiceReceived_ToString, _
                _Date.ToString("yyyy-MM-dd"), _Number, _ID.ToString(), _Content)
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Validation.RuleArgs("Content"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Validation.RuleArgs("Number"))
            ValidationRules.AddRule(AddressOf CommonValidation.CurrencyFieldValidation, _
                New Validation.RuleArgs("CurrencyCode"))
            ValidationRules.AddRule(AddressOf CommonValidation.DoubleFieldValidation, _
                New Validation.RuleArgs("CurrencyRate"))
            ValidationRules.AddRule(AddressOf CommonValidation.PersonFieldValidation, _
                New Validation.RuleArgs("Supplier"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Validation.RuleArgs("CommentsInternal"))
            ValidationRules.AddRule(AddressOf CommonValidation.ChronologyValidation, _
                New CommonValidation.ChronologyRuleArgs("Date", "ChronologyValidator"))

            ValidationRules.AddRule(AddressOf SumValidation, "Sum")
            ValidationRules.AddRule(AddressOf AccountSupplierValidation, "AccountSupplier")
            ValidationRules.AddRule(AddressOf IndirectVatAccountsValidation, "IndirectVatAccount")
            ValidationRules.AddRule(AddressOf IndirectVatAccountsValidation, "IndirectVatCostsAccount")
            ValidationRules.AddRule(AddressOf IndirectVatDeclarationSchemaValidation, _
                New CommonValidation.VatDeclarationSchemaFieldRuleArgs("IndirectVatDeclarationSchema", ""))

            ValidationRules.AddDependantProperty("Type", "Sum", False)
            ValidationRules.AddDependantProperty("Supplier", "AccountSupplier", False)
            ValidationRules.AddDependantProperty("IndirectVatSum", "IndirectVatAccount", False)
            ValidationRules.AddDependantProperty("IndirectVatSum", "IndirectVatCostsAccount", False)
            ValidationRules.AddDependantProperty("SumVatLTL", "IndirectVatAccount", False)
            ValidationRules.AddDependantProperty("SumVatLTL", "IndirectVatCostsAccount", False)
            ValidationRules.AddDependantProperty("IndirectVatSum", "IndirectVatDeclarationSchema", False)

        End Sub

        ''' <summary>
        ''' Rule ensuring that any items exist.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function SumValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceived = DirectCast(target, InvoiceReceived)

            If Not valObj._InvoiceItems.Count > 0 Then

                e.Description = My.Resources.Documents_InvoiceReceived_NoItems
                e.Severity = Validation.RuleSeverity.Error
                Return False

            ElseIf valObj._Type = InvoiceType.Credit AndAlso Not CRound(valObj._Sum) < 0 Then

                e.Description = My.Resources.Documents_InvoiceReceived_SumInvalidForCreditInvoice
                e.Severity = Validation.RuleSeverity.Error
                Return False

            ElseIf valObj._Type <> InvoiceType.Credit AndAlso Not CRound(valObj._Sum) > 0 Then

                e.Description = My.Resources.Documents_InvoiceReceived_SumInvalidForDebitInvoice
                e.Severity = Validation.RuleSeverity.Error
                Return False

            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that AccountSupplier is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AccountSupplierValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceived = DirectCast(target, InvoiceReceived)

            If valObj._Supplier Is Nothing OrElse valObj._Supplier.IsEmpty OrElse _
                Not valObj._Supplier.AccountAgainstBankSupplyer > 0 Then
                Return CommonValidation.CommonValidation.AccountFieldValidation(target, e)
            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that IndirectVatAccount and IndirectVatCostsAccount are valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function IndirectVatAccountsValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceived = DirectCast(target, InvoiceReceived)

            If CRound(valObj._IndirectVatSum) > 0 Then

                If valObj._SumVatLTL > 0 Then

                    e.Description = My.Resources.Documents_InvoiceReceived_IndirectVatInvalid
                    e.Severity = Validation.RuleSeverity.Error
                    Return False

                End If

                Return CommonValidation.CommonValidation.AccountFieldValidation(target, e)

            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that IndirectVatDeclarationSchema is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function IndirectVatDeclarationSchemaValidation(ByVal target As Object, _
            ByVal e As CommonValidation.VatDeclarationSchemaFieldRuleArgs) As Boolean

            If Not DirectCast(target, InvoiceReceived).IndirectVatSum > 0 Then Return True

            Return CommonValidation.VatDeclarationSchemaFieldValidation(target, e)

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite("Documents.InvoiceReceived2")
        End Sub

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("Documents.InvoiceReceived2")
        End Function

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("Documents.InvoiceReceived1")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("Documents.InvoiceReceived3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("Documents.InvoiceReceived3")
        End Function

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets a new instance of InvoiceReceived.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function NewInvoiceReceived() As InvoiceReceived

            Dim result As New InvoiceReceived
            result.InitializeNewEmptyInstance()
            Return result

        End Function

        ''' <summary>
        ''' Gets a new InvoiceReceived instance using data transfer object.
        ''' </summary>
        ''' <param name="info">A data transfer object that contains invoice data.</param>
        ''' <param name="systemGuid">An ID of the system that requests the creation of the invoice 
        ''' using the data transfer object. Used to distinguish data transfer within and outside a system (an application instance).
        ''' Use <see cref="System.Guid">Guid.ToString</see> to generate an application instance Guid.</param>
        ''' <param name="useImportedObjectExternalID">Whether to use the data transfer object ExternalID
        ''' property when setting <see cref="ExternalID">invoice ExternalID property</see>
        ''' (otherwise the ID property of the data transfer object is used).</param>
        ''' <param name="clientList">Lookup list for person data.</param>
        ''' <param name="accountList">a list of company accounts to validate incomming data account id's</param>
        ''' <param name="vatSchemaList">a list of company VAT declaration schemas to set a schema by schema ID</param>
        ''' <param name="unknownPerson">Output param. Is set to data transfer object person data,
        ''' if the person is not identified with any person in the company database.
        ''' Used for further data import (new person data).</param>
        ''' <remarks></remarks>
        Public Shared Function NewInvoiceReceived(ByVal info As InvoiceInfo.InvoiceInfo,
            ByVal systemGuid As String, ByVal useImportedObjectExternalID As Boolean,
            ByVal clientList As PersonInfoList, ByVal accountList As AccountInfoList,
            ByVal vatSchemaList As VatDeclarationSchemaInfoList,
            ByRef unknownPerson As InvoiceInfo.ClientInfo) As InvoiceReceived

            Dim result As New InvoiceReceived(info, systemGuid, useImportedObjectExternalID, clientList, accountList, vatSchemaList)

            If (result.Supplier Is Nothing OrElse result.Supplier.IsEmpty) AndAlso
                Not info.Payer Is Nothing AndAlso Not StringIsNullOrEmpty(info.Payer.Code) Then
                unknownPerson = info.Payer
            Else
                unknownPerson = Nothing
            End If

            Return result

        End Function


        ''' <summary>
        ''' Gets an existing instance of InvoiceReceived from a database.
        ''' </summary>
        ''' <param name="nID">An ID of the InvoiceReceived to get.</param>
        ''' <remarks></remarks>
        Public Shared Function GetInvoiceReceived(ByVal nID As Integer) As InvoiceReceived

            Return DataPortal.Fetch(Of InvoiceReceived)(New Criteria(nID))

        End Function


        ''' <summary>
        ''' Deletes an existing instance of InvoiceReceived from a database.
        ''' </summary>
        ''' <param name="id">An ID of the InvoiceReceived to delete.</param>
        ''' <remarks></remarks>
        Public Shared Sub DeleteInvoiceReceived(ByVal id As Integer)
            DataPortal.Delete(New Criteria(id))
        End Sub


        Public Function GetInvoiceReceivedCopy() As InvoiceReceived

            Dim result As InvoiceReceived = Me.Clone
            result._ID = -1
            result._Number = ""
            result._Date = Today

            result._InvoiceItems.MarkAsCopy()

            Dim baseValidator As SimpleChronologicValidator = _
                SimpleChronologicValidator.NewSimpleChronologicValidator( _
                Utilities.ConvertLocalizedName(DocumentType.InvoiceReceived), Nothing)

            result._ChronologyValidator = ComplexChronologicValidator. _
                NewComplexChronologicValidator(baseValidator.CurrentOperationName, _
                baseValidator, Nothing, result._InvoiceItems.GetChronologyValidators())

            result.MarkNew()

            result.ValidationRules.CheckRules()

            Return result

        End Function


        Private Sub New()
            ' require use of factory methods

        End Sub

        Private Sub New(ByVal info As InvoiceInfo.InvoiceInfo, ByVal systemGuid As String,
            ByVal useImportedObjectExternalID As Boolean, ByVal clientList As PersonInfoList,
            ByVal accountList As AccountInfoList, ByVal vatSchemaList As VatDeclarationSchemaInfoList)
            Create(info, systemGuid, useImportedObjectExternalID, clientList, accountList, vatSchemaList)
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


        Private Sub InitializeNewEmptyInstance()

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            _Content = GetCurrentCompany().DefaultInvoiceReceivedContent

            Dim baseValidator As SimpleChronologicValidator = SimpleChronologicValidator. _
                NewSimpleChronologicValidator(Utilities.ConvertLocalizedName(DocumentType.InvoiceReceived), Nothing)

            _ChronologyValidator = ComplexChronologicValidator.NewComplexChronologicValidator( _
                baseValidator.CurrentOperationName, baseValidator, Nothing, Nothing)

            _InvoiceItems = InvoiceReceivedItemList.NewInvoiceReceivedItemList(Me)

            ValidationRules.CheckRules()

        End Sub

        Private Sub Create(ByVal info As InvoiceInfo.InvoiceInfo, ByVal systemGuid As String,
            ByVal useImportedObjectExternalID As Boolean, ByVal clientList As PersonInfoList,
            ByVal accountList As AccountInfoList, ByVal vatSchemaList As VatDeclarationSchemaInfoList)

            ' data transfer within the same system (an application instance)
            If info.SystemGuid.Trim = systemGuid.Trim Then

                _Number = ""
                _ExternalID = ""

            Else

                _Number = info.FullNumber

                If useImportedObjectExternalID Then
                    Me._ExternalID = info.ExternalID
                Else
                    Me._ExternalID = info.ID
                End If

            End If

            Me._CommentsInternal = info.CommentsInternal
            Me._Content = info.Content
            Me._CurrencyCode = info.CurrencyCode
            Me._CurrencyRate = info.CurrencyRate
            Me._Date = info.Date
            Me._UpdateDate = info.UpdateDate

            If Not info.Payer Is Nothing AndAlso Not StringIsNullOrEmpty(info.Payer.Code) Then
                Me._Supplier = clientList.GetPersonInfo(info.Payer.Code)
                If Not Me._Supplier Is Nothing AndAlso Not _Supplier.IsEmpty Then
                    Me._AccountSupplier = Me._Supplier.AccountAgainstBankSupplyer
                End If
            End If

            Dim baseValidator As SimpleChronologicValidator = SimpleChronologicValidator.
                NewSimpleChronologicValidator(Utilities.ConvertLocalizedName(DocumentType.InvoiceReceived), Nothing)

            _ChronologyValidator = ComplexChronologicValidator.NewComplexChronologicValidator(
                baseValidator.CurrentOperationName, baseValidator, Nothing, Nothing)

            _InvoiceItems = InvoiceReceivedItemList.NewInvoiceReceivedItemList(info, Me, accountList, vatSchemaList)

            CalculateSubTotals(False)

            ValidationRules.CheckRules()

        End Sub


        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecuritySelectDenied)

            Dim myComm As New SQLCommand("FetchInvoiceReceived")
            myComm.AddParam("?MD", criteria.ID)

            Using myData As DataTable = myComm.Fetch

                If myData.Rows.Count < 1 Then Throw New Exception(String.Format( _
                    My.Resources.Common_ObjectNotFound, My.Resources.Documents_InvoiceReceived_TypeName, _
                    criteria.ID.ToString()))

                _ID = criteria.ID

                Dim dr As DataRow = myData.Rows(0)

                _Date = CDateSafe(dr.Item(0), Today)
                _Number = CStrSafe(dr.Item(1)).Trim
                _Content = CStrSafe(dr.Item(2)).Trim
                _CurrencyCode = CStrSafe(dr.Item(3)).Trim
                _CurrencyRate = CDblSafe(dr.Item(4), ROUNDCURRENCYRATE, 0)
                _CommentsInternal = CStrSafe(dr.Item(5)).Trim
                _AccountSupplier = CLongSafe(dr.Item(6), 0)
                _Type = Utilities.ConvertDatabaseID(Of InvoiceType)(CIntSafe(dr.Item(7), 0))
                _InsertDate = CTimeStampSafe(dr.Item(8))
                _UpdateDate = CTimeStampSafe(dr.Item(9))
                _ExternalID = CStrSafe(dr.Item(10)).Trim
                _IndirectVatSum = CDblSafe(dr.Item(11), 2, 0)
                _IndirectVatAccount = CLongSafe(dr.Item(12), 0)
                _IndirectVatCostsAccount = CLongSafe(dr.Item(13), 0)
                _ActualDate = CDateSafe(dr.Item(14), Date.MinValue)
                If _ActualDate = Date.MinValue Then
                    _ActualDate = _Date
                    _ActualDateIsApplicable = False
                Else
                    _ActualDateIsApplicable = True
                End If

                _Supplier = PersonInfo.GetPersonInfo(dr, 15)
                _IndirectVatDeclarationSchema = VatDeclarationSchemaInfo.GetVatDeclarationSchemaInfo(dr, 35)

            End Using

            Dim baseValidator As SimpleChronologicValidator = SimpleChronologicValidator. _
                GetSimpleChronologicValidator(_ID, _Date, Utilities.ConvertLocalizedName(DocumentType.InvoiceReceived), Nothing)

            _InvoiceItems = InvoiceReceivedItemList.GetInvoiceReceivedItemList(Me, baseValidator)

            CalculateSubTotals(False)

            _ChronologyValidator = ComplexChronologicValidator.GetComplexChronologicValidator( _
                _ID, _Date, baseValidator.CurrentOperationName, baseValidator, _
                Nothing, _InvoiceItems.GetChronologyValidators())

            MarkOld()

            ValidationRules.CheckRules()

        End Sub


        Protected Overrides Sub DataPortal_Insert()

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityInsertDenied)

            _InvoiceItems.CheckIfCanUpdate(Me)

            Me.ValidationRules.CheckRules()
            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            CheckIfExternalIdUnique()

            Dim entry As General.JournalEntry = GetJournalEntry(False)

            Using transaction As New SqlTransaction

                Try

                    entry = entry.SaveChild()

                    _ID = entry.ID

                    Dim myComm As New SQLCommand("InsertInvoiceReceived")
                    AddParamsGeneral(myComm)
                    AddParamsFinancial(myComm)

                    myComm.Execute()

                    _InvoiceItems.Update(Me)

                    For Each i As InvoiceReceivedItem In _InvoiceItems
                        If Not i.AttachedObjectValue Is Nothing AndAlso _
                            i.AttachedObjectValue.Type = InvoiceAdapterType.GoodsTransfer Then

                            entry = GetJournalEntry(True)
                            entry = entry.SaveChild()

                            Exit For

                        End If
                    Next

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

            MarkOld()

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanEditObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityUpdateDenied)

            _InvoiceItems.CheckIfCanUpdate(Me)

            Me.ValidationRules.CheckRules()
            If Not Me.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_ContainsErrors, vbCrLf, _
                    GetAllBrokenRules()))
            End If

            CheckIfExternalIdUnique()

            Dim entry As General.JournalEntry = GetJournalEntry(False)

            CheckIfUpdateDateChanged()

            Dim myComm As SQLCommand
            If _ChronologyValidator.FinancialDataCanChange Then
                myComm = New SQLCommand("UpdateInvoiceReceived")
                AddParamsFinancial(myComm)
            Else
                myComm = New SQLCommand("UpdateInvoiceReceivedGeneral")
            End If
            AddParamsGeneral(myComm)

            Using transaction As New SqlTransaction

                Try

                    entry = entry.SaveChild()

                    myComm.Execute()

                    _InvoiceItems.Update(Me)

                    For Each i As InvoiceReceivedItem In _InvoiceItems
                        If Not i.AttachedObjectValue Is Nothing AndAlso _
                            i.AttachedObjectValue.Type = InvoiceAdapterType.GoodsTransfer Then

                            entry = GetJournalEntry(True)
                            entry = entry.SaveChild()

                            Exit For

                        End If
                    Next

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

            MarkOld()

        End Sub

        Private Sub AddParamsGeneral(ByRef myComm As SQLCommand)

            myComm.AddParam("?AA", _ID)
            myComm.AddParam("?AD", _CommentsInternal.Trim)
            myComm.AddParam("?AF", Utilities.ConvertDatabaseID(_Type))
            myComm.AddParam("?AG", _ExternalID.Trim)
            If _ActualDateIsApplicable Then
                myComm.AddParam("?AL", _ActualDate.Date)
            Else
                myComm.AddParam("?AL", Nothing, GetType(Date))
            End If

            _UpdateDate = GetCurrentTimeStamp()
            If Me.IsNew Then _InsertDate = _UpdateDate
            myComm.AddParam("?AH", _UpdateDate.ToUniversalTime)

        End Sub

        Private Sub AddParamsFinancial(ByRef myComm As SQLCommand)

            myComm.AddParam("?AB", _CurrencyCode.Trim)
            myComm.AddParam("?AC", CRound(_CurrencyRate, ROUNDCURRENCYRATE))
            myComm.AddParam("?AE", _AccountSupplier)
            myComm.AddParam("?AI", CRound(_IndirectVatSum))
            myComm.AddParam("?AJ", _IndirectVatAccount)
            myComm.AddParam("?AK", _IndirectVatCostsAccount)
            If _IndirectVatDeclarationSchema Is Nothing OrElse _IndirectVatDeclarationSchema.IsEmpty _
                OrElse Not CRound(_IndirectVatSum, 2) > 0 Then
                myComm.AddParam("?AM", 0)
            Else
                myComm.AddParam("?AM", _IndirectVatDeclarationSchema.ID)
            End If

        End Sub


        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_ID))
        End Sub

        Protected Overrides Sub DataPortal_Delete(ByVal criteria As Object)

            If Not CanDeleteObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecurityUpdateDenied)

            Dim cInvoice As New InvoiceReceived
            cInvoice.DataPortal_Fetch(New Criteria(DirectCast(criteria, Criteria).ID))

            For Each item As InvoiceReceivedItem In cInvoice._InvoiceItems
                item.CheckIfCanDelete(cInvoice._ChronologyValidator.BaseValidator)
            Next

            IndirectRelationInfoList.CheckIfJournalEntryCanBeDeleted(DirectCast(criteria, Criteria).ID, _
                DocumentType.InvoiceReceived)

            Dim myComm As New SQLCommand("DeleteInvoiceReceived")
            myComm.AddParam("?MD", DirectCast(criteria, Criteria).ID)

            Using transaction As New SqlTransaction

                Try

                    For Each item As InvoiceReceivedItem In cInvoice._InvoiceItems
                        item.DeleteSelf()
                    Next

                    myComm.Execute()

                    General.JournalEntry.DeleteJournalEntryChild(DirectCast(criteria, Criteria).ID)

                    transaction.Commit()

                Catch ex As Exception
                    transaction.SetNonSqlException(ex)
                    Throw
                End Try

            End Using

        End Sub


        Private Function GetJournalEntry(ByVal forceOld As Boolean) As General.JournalEntry

            Dim result As General.JournalEntry
            If IsNew AndAlso Not forceOld Then
                result = General.JournalEntry.NewJournalEntryChild(DocumentType.InvoiceReceived)
            Else
                result = General.JournalEntry.GetJournalEntryChild(_ID, DocumentType.InvoiceReceived)
            End If

            result.Content = _Content
            result.Date = _Date
            result.DocNumber = _Number
            result.Person = _Supplier

            If IsNew OrElse _ChronologyValidator.ParentFinancialDataCanChange Then

                Dim fullBookEntryList As BookEntryInternalList = BookEntryInternalList. _
                NewBookEntryInternalList(BookEntryType.Debetas)

                Dim applicableAccountSupplier As Long = _AccountSupplier
                If Not applicableAccountSupplier > 0 Then
                    applicableAccountSupplier = _Supplier.AccountAgainstBankSupplyer
                End If

                For Each i As InvoiceReceivedItem In _InvoiceItems
                    fullBookEntryList.AddRange(i.GetBookEntryInternalList(applicableAccountSupplier))
                Next

                If CRound(_IndirectVatSum) > 0 AndAlso _IndirectVatAccount > 0 AndAlso _
                    _IndirectVatCostsAccount > 0 Then

                    fullBookEntryList.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas, _
                        _IndirectVatCostsAccount, CRound(_IndirectVatSum), Nothing))

                    fullBookEntryList.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas, _
                        _IndirectVatAccount, CRound(_IndirectVatSum), Nothing))

                End If

                fullBookEntryList.Aggregate()

                result.DebetList.Clear()
                result.CreditList.Clear()

                result.DebetList.LoadBookEntryListFromInternalList(fullBookEntryList, False, False)
                result.CreditList.LoadBookEntryListFromInternalList(fullBookEntryList, False, False)

            End If

            If Not result.IsValid Then
                Throw New Exception(String.Format(My.Resources.Common_FailedToCreateJournalEntry, _
                    vbCrLf, result.ToString, vbCrLf, result.GetAllBrokenRules))
            End If

            Return result

        End Function


        Private Sub CheckIfUpdateDateChanged()

            Dim myComm As New SQLCommand("CheckIfInvoiceReceivedUpdateDateChanged")
            myComm.AddParam("?SD", _ID)

            Using myData As DataTable = myComm.Fetch

                If myData.Rows.Count < 1 OrElse CDateTimeSafe(myData.Rows(0).Item(0), _
                    Date.MinValue) = Date.MinValue Then

                    Throw New Exception(String.Format(My.Resources.Common_ObjectNotFound, _
                        My.Resources.Documents_InvoiceReceived_TypeName, _ID.ToString))

                End If

                If CTimeStampSafe(myData.Rows(0).Item(0)) <> _UpdateDate Then

                    Throw New Exception(My.Resources.Common_UpdateDateHasChanged)

                End If

            End Using

        End Sub

        Private Sub CheckIfExternalIdUnique()

            If StringIsNullOrEmpty(_ExternalID) Then Exit Sub

            Dim myComm As New SQLCommand("FetchInvoiceReceivedIdByExternalID")
            myComm.AddParam("?ND", _ID)
            myComm.AddParam("?ED", _ExternalID.Trim)

            Using myData As DataTable = myComm.Fetch
                If myData.Rows.Count > 0 AndAlso CIntSafe(myData.Rows(0).Item(0), 0) > 0 Then
                    Throw New Exception(String.Format(My.Resources.Documents_InvoiceReceived_ExternalIdNotUnique, _ExternalID.Trim))
                End If
            End Using

        End Sub

#End Region

    End Class

End Namespace