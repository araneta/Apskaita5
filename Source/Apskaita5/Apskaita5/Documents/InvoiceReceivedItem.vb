Imports ApskaitaObjects.Documents.InvoiceAdapters
Imports ApskaitaObjects.Attributes

Namespace Documents

    ''' <summary>
    ''' Represents a line (item) within an <see cref="InvoiceReceived">invoice received</see>.
    ''' </summary>
    ''' <remarks>Should only be used as a child of a <see cref="InvoiceReceivedItemList">InvoiceReceivedItemList</see>.
    ''' Values are stored in the database table sfg.</remarks>
    <Serializable()> _
    Public NotInheritable Class InvoiceReceivedItem
        Inherits BusinessBase(Of InvoiceReceivedItem)
        Implements IGetErrorForListItem

#Region " Business Methods "

        Private _Guid As Guid = Guid.NewGuid
        Private _ID As Integer = -1
        Private _FinancialDataCanChange As Boolean = True
        Private _NameInvoice As String = ""
        Private _MeasureUnit As String = ""
        Private _AccountCosts As Long = 0
        Private _AccountVat As Long = 0
        Private _Ammount As Double = 0
        Private _UnitValueLTL As Double = 0
        Private _UnitValueLTLCorrection As Integer = 0
        Private _SumLTL As Double = 0
        Private _SumLTLCorrection As Integer = 0
        Private _VatRate As Double = 0
        Private _DeclarationSchema As VatDeclarationSchemaInfo = Nothing
        Private _SumVatLTL As Double = 0
        Private _SumVatLTLCorrection As Integer = 0
        Private _SumTotalLTL As Double = 0
        Private _UnitValue As Double = 0
        Private _Sum As Double = 0
        Private _SumCorrection As Integer = 0
        Private _SumVat As Double = 0
        Private _SumVatCorrection As Integer = 0
        Private _SumTotal As Double = 0
        Private _IncludeVatInObject As Boolean = False
        Private WithEvents _AttachedObject As IInvoiceAdapter = Nothing

        Private suspendChildChanged As Boolean = False


        ''' <summary>
        ''' Gets an ID of the item that is assigned by a database (AUTOINCREMENT).
        ''' </summary>
        ''' <remarks>Value is stored in the database table sfg.ID.</remarks>
        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        ''' <summary>
        ''' Returnes TRUE if the parent invoice allows financial changes 
        ''' due to business restrains.
        ''' </summary>
        ''' <remarks></remarks>
        Private ReadOnly Property FinancialDataCanChange() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a content (description) of the item.
        ''' </summary>
        ''' <remarks>Value is stored in the database table sfg.Preke.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 255)> _
        Public Property NameInvoice() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _NameInvoice.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _NameInvoice.Trim <> value.Trim Then
                    _NameInvoice = value.Trim
                    PropertyHasChanged()
                    If Not _AttachedObject Is Nothing AndAlso _AttachedObject.HandlesNameInvoice Then
                        suspendChildChanged = True
                        _AttachedObject.NameInvoice = value
                        suspendChildChanged = False
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a measure unit of the item.
        ''' </summary>
        ''' <remarks>Value is stored in the database table sfg.Vnt.</remarks>
        <StringField(ValueRequiredLevel.Mandatory, 10)> _
        Public Property MeasureUnit() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _MeasureUnit.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _MeasureUnit.Trim <> value.Trim Then
                    _MeasureUnit = value.Trim
                    PropertyHasChanged()
                    If Not _AttachedObject Is Nothing AndAlso _AttachedObject.HandlesMeasureUnit Then
                        suspendChildChanged = True
                        _AttachedObject.MeasureUnit = value
                        suspendChildChanged = False
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the costs <see cref="General.Account.ID">account</see>,
        ''' that is debited by the <see cref="SumLTL">SumLTL</see> amount 
        ''' (unless overriden by the attached operation).
        ''' </summary>
        ''' <remarks>Value is stored in the database table sfg.Sanaud.</remarks>
        <AccountField(ValueRequiredLevel.Mandatory, False, 1, 2, 3, 4, 5, 6)> _
        Public Property AccountCosts() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountCosts
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If AccountCostsIsReadOnly Then Exit Property
                If _AccountCosts <> value Then
                    _AccountCosts = value
                    PropertyHasChanged()
                    If Not _AttachedObject Is Nothing AndAlso _AttachedObject.HandlesAccount Then
                        suspendChildChanged = True
                        _AttachedObject.Account = value
                        suspendChildChanged = False
                    End If
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the VAT receivable <see cref="General.Account.ID">account</see>,
        ''' that is debited by the <see cref="SumVatLTL">SumVatLTL</see> amount.
        ''' </summary>
        ''' <remarks>Value is stored in the database table sfg.PVM_S.</remarks>
        <AccountField(ValueRequiredLevel.Mandatory, False, 2, 4, 6)> _
        Public Property AccountVat() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AccountVat
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If AccountVatIsReadOnly Then Exit Property
                If _AccountVat <> value Then
                    _AccountVat = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the amount of the goods/services bought.
        ''' </summary>
        ''' <remarks>Value round order is <see cref="ROUNDAMOUNTINVOICERECEIVED">ROUNDAMOUNTINVOICERECEIVED</see>.
        ''' Value is stored in the database table sfg.V_KK.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, True, ROUNDAMOUNTINVOICERECEIVED)> _
        Public Property Ammount() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If AmmountIsReadOnly Then Exit Property
                If CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED) <> CRound(value, ROUNDAMOUNTINVOICERECEIVED) Then
                    _Ammount = CRound(value, ROUNDAMOUNTINVOICERECEIVED)
                    CalculateSum(0)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the value of the goods/services bought per unit 
        ''' in <see cref="InvoiceReceived.CurrencyCode">the original invoice currency</see>.
        ''' </summary>
        ''' <remarks>Value round order is <see cref="ROUNDUNITINVOICERECEIVED">ROUNDUNITINVOICERECEIVED</see>.
        ''' Value is stored in the database table sfg.V_KN.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, True, ROUNDUNITINVOICERECEIVED)> _
        Public Property UnitValue() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValue, ROUNDUNITINVOICERECEIVED)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If UnitValueIsReadOnly Then Exit Property
                If CRound(_UnitValue, ROUNDUNITINVOICERECEIVED) <> CRound(value, ROUNDUNITINVOICERECEIVED) Then
                    _UnitValue = CRound(value, ROUNDUNITINVOICERECEIVED)
                    CalculateSum(0)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the total value of the goods/services bought (excluding VAT)
        ''' in <see cref="InvoiceReceived.CurrencyCode">the original invoice currency</see>.
        ''' </summary>
        ''' <remarks>Equals <see cref="Ammount">Ammount</see> multiplied by 
        ''' <see cref="UnitValue">UnitValue</see>
        ''' plus <see cref="SumCorrection">SumCorrection</see> divided by 100.
        ''' Value is stored in the database table sfg.SumOriginal.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, True, 2)> _
        Public ReadOnly Property Sum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_Sum)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a correction of <see cref="Sum">Sum</see> in cents (1/100 of the currency unit).
        ''' </summary>
        ''' <remarks>Value is calculated (not persisted) as the difference between 
        ''' <see cref="Ammount">Ammount</see> multiplied by  <see cref="UnitValue">UnitValue</see>
        ''' and <see cref="Sum">Sum</see>.</remarks>
        <CorrectionField()> _
        Public Property SumCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SumCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If SumCorrectionIsReadOnly Then Exit Property
                If _SumCorrection <> value Then
                    _SumCorrection = value
                    CalculateSum(0)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the applicable VAT rate in percents for the goods/services bought (21 = 21%).
        ''' </summary>
        ''' <remarks>Value is stored in the database table sfg.Tarif.</remarks>
        <TaxRateField(ValueRequiredLevel.Optional, ApskaitaObjects.Settings.TaxRateType.Vat)> _
        Public Property VatRate() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_VatRate)
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Double)
                CanWriteProperty(True)
                If VatRateIsReadOnly Then Exit Property
                If CRound(_VatRate) <> CRound(value) Then
                    _VatRate = CRound(value)
                    CalculateSumVat()
                    CalculateSumVatLTL(0)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the applicable VAT declaration schema for the goods/services bought.
        ''' </summary>
        ''' <remarks>Value is stored in the database table sfg.DeclarationSchemaID.</remarks>
        <VatDeclarationSchemaField(ValueRequiredLevel.Mandatory, TradedItemType.All)> _
        Public Property DeclarationSchema() As VatDeclarationSchemaInfo
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _DeclarationSchema
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As VatDeclarationSchemaInfo)
                CanWriteProperty(True)
                If DeclarationSchemaIsReadOnly Then Exit Property
                If Not (_DeclarationSchema Is Nothing AndAlso value Is Nothing) _
                    AndAlso Not (Not _DeclarationSchema Is Nothing AndAlso Not value Is Nothing _
                    AndAlso _DeclarationSchema = value) Then
                    _DeclarationSchema = value
                    If Not _DeclarationSchema Is Nothing AndAlso Not _DeclarationSchema.IsEmpty _
                        AndAlso Not VatRateIsReadOnly Then
                        VatRate = _DeclarationSchema.VatRate
                    End If
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the VAT value in <see cref="InvoiceReceived.CurrencyCode">the original invoice currency</see>.
        ''' </summary>
        ''' <remarks>Equals <see cref="Sum">Sum</see> multiplied by 
        ''' <see cref="VatRate">VatRate</see> and divided by 100
        ''' plus <see cref="SumVatCorrection">SumVatCorrection</see> divided by 100.
        ''' Value is stored in the database table sfg.SumVatOriginal.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, True, 2)> _
        Public ReadOnly Property SumVat() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumVat)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a correction of <see cref="SumVat">SumVat</see> in cents (1/100 of the currency unit).
        ''' </summary>
        ''' <remarks>Value is calculated (not persisted) as the difference between 
        ''' <see cref="Sum">Sum</see> multiplied by <see cref="VatRate">VatRate</see>, divided by 100
        ''' and <see cref="SumVat">SumVat</see>.</remarks>
        <CorrectionField()> _
        Public Property SumVatCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SumVatCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If SumVatCorrectionIsReadOnly Then Exit Property
                If _SumVatCorrection <> value Then
                    _SumVatCorrection = value
                    CalculateSumVat()
                    CalculateSumVatLTL(0)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the total value of the goods/services bought (including VAT)
        ''' in <see cref="InvoiceReceived.CurrencyCode">the original invoice currency</see>.
        ''' </summary>
        ''' <remarks>Equals <see cref="Sum">Sum</see> plus <see cref="SumVat">SumVat</see>.
        ''' Value is not stored in the database.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, True, 2)> _
        Public ReadOnly Property SumTotal() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumTotal)
            End Get
        End Property

        ''' <summary>
        ''' Gets the value of the goods/services bought per unit in the base currency.
        ''' </summary>
        ''' <remarks>Value equals <see cref="UnitValue">UnitValue</see> multiplied by
        ''' <see cref="InvoiceReceived.CurrencyRate">the invoice currency rate</see>
        ''' plus <see cref="UnitValueLTLCorrection">UnitValueLTLCorrection</see> divided by 100.
        ''' Value round order is <see cref="ROUNDUNITINVOICERECEIVED">ROUNDUNITINVOICERECEIVED</see>.
        ''' Value is stored in the database table sfg.UnitValueLTL.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, True, ROUNDUNITINVOICERECEIVED)> _
        Public ReadOnly Property UnitValueLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_UnitValueLTL, ROUNDUNITINVOICERECEIVED)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a correction of <see cref="UnitValueLTL">UnitValueLTL</see> in cents (1/100 of the currency unit).
        ''' </summary>
        ''' <remarks>Value is calculated (not persisted) as the difference between 
        ''' <see cref="UnitValue">UnitValue</see> multiplied by
        ''' <see cref="InvoiceReceived.CurrencyRate">the invoice currency rate</see>
        ''' and <see cref="UnitValueLTL">UnitValueLTL</see>.</remarks>
        <CorrectionField()> _
        Public Property UnitValueLTLCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _UnitValueLTLCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If UnitValueLTLCorrectionIsReadOnly Then Exit Property
                If _UnitValueLTLCorrection <> value Then
                    _UnitValueLTLCorrection = value
                    CalculateSumLTL(0)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the total value of the goods/services bought (excluding VAT) in the base currency.
        ''' </summary>
        ''' <remarks>Value equals <see cref="Sum">Sum</see> multiplied by
        ''' <see cref="InvoiceReceived.CurrencyRate">the invoice currency rate</see>
        ''' plus <see cref="SumLTLCorrection">SumLTLCorrection</see> divided by 100.
        ''' Value is stored in the database table sfg.SumLTL.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, True, 2)> _
        Public ReadOnly Property SumLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumLTL)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a correction of <see cref="SumLTL">SumLTL</see> in cents (1/100 of the currency unit).
        ''' </summary>
        ''' <remarks>Value is calculated (not persisted) as the difference between 
        ''' <see cref="Sum">Sum</see> multiplied by
        ''' <see cref="InvoiceReceived.CurrencyRate">the invoice currency rate</see>
        ''' and <see cref="SumLTL">SumLTL</see>.</remarks>
        <CorrectionField()> _
        Public Property SumLTLCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SumLTLCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If SumLTLCorrectionIsReadOnly Then Exit Property
                If _SumLTLCorrection <> value Then
                    _SumLTLCorrection = value
                    CalculateSumLTL(0)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the VAT value in the base currency.
        ''' </summary>
        ''' <remarks>Value equals <see cref="SumVat">SumVat</see> multiplied by
        ''' <see cref="InvoiceReceived.CurrencyRate">the invoice currency rate</see>
        ''' plus <see cref="SumVatLTLCorrection">SumVatLTLCorrection</see> divided by 100.
        ''' Value is stored in the database table sfg.SumVatLTL.</remarks>
        <DoubleField(ValueRequiredLevel.Optional, True, 2)> _
        Public ReadOnly Property SumVatLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumVatLTL)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a correction of <see cref="SumVatLTL">SumVatLTL</see> in cents (1/100 of the currency unit).
        ''' </summary>
        ''' <remarks>Value is calculated (not persisted) as the difference between 
        ''' <see cref="SumVat">SumVat</see> multiplied by
        ''' <see cref="InvoiceReceived.CurrencyRate">the invoice currency rate</see>
        ''' and <see cref="SumVatLTL">SumVatLTL</see>.</remarks>
        <CorrectionField()> _
        Public Property SumVatLTLCorrection() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _SumVatLTLCorrection
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If SumVatLTLCorrectionIsReadOnly Then Exit Property
                If _SumVatLTLCorrection <> value Then
                    _SumVatLTLCorrection = value
                    CalculateSumVatLTL(0)
                    PropertyHasChanged()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets the total value of the goods/services bought (including VAT) in the base currency.
        ''' </summary>
        ''' <remarks>Equals <see cref="SumLTL">SumLTL</see> plus <see cref="SumVatLTL">SumVatLTL</see>.
        ''' Value is not stored in the database.</remarks>
        <DoubleField(ValueRequiredLevel.Mandatory, True, 2)> _
        Public ReadOnly Property SumTotalLTL() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_SumTotalLTL)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets whether VAT should be handled by the attached operation, 
        ''' e.g. included into the acquisition costs of a long term asset.
        ''' Can only be used if the attached operation 
        ''' <see cref="IInvoiceAdapter.SumVatIsHandledOnRequest">
        ''' supports VAT handling</see>.
        ''' </summary>
        ''' <remarks>Value is stored in the database table sfg.IncludeVatInObject.</remarks>
        Public Property IncludeVatInObject() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IncludeVatInObject
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If IncludeVatInObjectIsReadOnly Then Exit Property
                If _IncludeVatInObject <> value Then
                    _IncludeVatInObject = value
                    PropertyHasChanged()
                    SetAttachedObjectFinancialData()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Gets a description of the attached operation. 
        ''' Returns empty string if the item does not have an attached operation.
        ''' </summary>
        ''' <remarks>Uses ToString method to get the value.
        ''' Value is stored in the database fields sfg.Rus and sfg.P_ID
        ''' where sfg.Rus represents the type of the operation
        ''' and sfg.P_ID represents an ID of the operation.</remarks>
        Public ReadOnly Property AttachedObject() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _AttachedObject Is Nothing Then Return ""
                Return _AttachedObject.ToString
            End Get
        End Property

        ''' <summary>
        ''' Gets (exposes) an attached operation by the common 
        ''' <see cref="IInvoiceAdapter">IInvoiceAdapter</see> interface.
        ''' </summary>
        ''' <remarks>Value is stored in the database fields sfg.Rus and sfg.P_ID
        ''' where sfg.Rus represents the type of the operation
        ''' and sfg.P_ID represents an ID of the operation.</remarks>
        Public ReadOnly Property AttachedObjectValue() As IInvoiceAdapter
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _AttachedObject
            End Get
        End Property


        ''' <summary>
        ''' Whether the <see cref="AccountCosts">AccountCosts</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property AccountCostsIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange OrElse (Not _AttachedObject Is Nothing _
                    AndAlso _AttachedObject.HandlesAccount AndAlso _
                    Not _AttachedObject.ChronologyValidator.FinancialDataCanChange)
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="AccountVat">AccountVat</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property AccountVatIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="Ammount">Ammount</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property AmmountIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange OrElse (Not _AttachedObject Is Nothing AndAlso _
                    Not _AttachedObject.ChronologyValidator.FinancialDataCanChange)
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="UnitValue">UnitValue</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property UnitValueIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange OrElse (Not _AttachedObject Is Nothing AndAlso _
                    Not _AttachedObject.ChronologyValidator.FinancialDataCanChange)
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="SumCorrection">SumCorrection</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SumCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange OrElse (Not _AttachedObject Is Nothing AndAlso _
                    Not _AttachedObject.ChronologyValidator.FinancialDataCanChange)
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="VatRate">VatRate</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property VatRateIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get

                If Not FinancialDataCanChange Then Return True

                If Not _AttachedObject Is Nothing AndAlso _
                    Not _AttachedObject.ChronologyValidator.FinancialDataCanChange Then

                    If _AttachedObject.HandlesVatRate OrElse _
                        (_AttachedObject.SumVatIsHandledOnRequest AndAlso _
                        _IncludeVatInObject) Then
                        Return True
                    End If

                End If

                Return False

            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="DeclarationSchema">DeclarationSchema</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property DeclarationSchemaIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="SumVatCorrection">SumVatCorrection</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SumVatCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get

                If Not FinancialDataCanChange Then Return True

                If Not _AttachedObject Is Nothing AndAlso _
                    Not _AttachedObject.ChronologyValidator.FinancialDataCanChange Then

                    If _AttachedObject.HandlesVatRate OrElse _
                        (_AttachedObject.SumVatIsHandledOnRequest AndAlso _
                        _IncludeVatInObject) Then
                        Return True
                    End If

                End If

                Return False

            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="UnitValueLTLCorrection">UnitValueLTLCorrection</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property UnitValueLTLCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange OrElse ParentCurrencyIsBaseCurrency()
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="SumLTLCorrection">SumLTLCorrection</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SumLTLCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return Not FinancialDataCanChange OrElse ParentCurrencyIsBaseCurrency() OrElse _
                    (Not _AttachedObject Is Nothing AndAlso _
                    Not _AttachedObject.ChronologyValidator.FinancialDataCanChange)
            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="SumVatLTLCorrection">SumVatLTLCorrection</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SumVatLTLCorrectionIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get

                If Not FinancialDataCanChange OrElse ParentCurrencyIsBaseCurrency() Then
                    Return True
                End If

                If Not _AttachedObject Is Nothing AndAlso _
                    Not _AttachedObject.ChronologyValidator.FinancialDataCanChange Then

                    If _AttachedObject.HandlesVatRate OrElse _
                        (_AttachedObject.SumVatIsHandledOnRequest AndAlso _IncludeVatInObject) Then
                        Return True
                    End If

                End If

                Return False

            End Get
        End Property

        ''' <summary>
        ''' Whether the <see cref="IncludeVatInObject">IncludeVatInObject</see> property is readonly
        ''' (due to chronological or other business rules).
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property IncludeVatInObjectIsReadOnly() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get

                If Not FinancialDataCanChange OrElse _AttachedObject Is Nothing Then
                    Return True
                End If

                If Not _AttachedObject.SumVatIsHandledOnRequest OrElse _
                    Not _AttachedObject.ChronologyValidator.FinancialDataCanChange Then

                    Return True

                End If

                Return False

            End Get
        End Property


        ''' <summary>
        ''' Whether the item can be copied. If not - the item is skiped.
        ''' </summary>
        ''' <remarks>Checks if there is an attached operation. If exists, checks 
        ''' <see cref="IInvoiceAdapter.ImplementsCopy">IInvoiceAdapter.ImplementsCopy</see> property.</remarks>
        Public ReadOnly Property CanBeCopied() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return (_AttachedObject Is Nothing OrElse _AttachedObject.ImplementsCopy)
            End Get
        End Property


        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse (Not _AttachedObject Is Nothing _
                    AndAlso _AttachedObject.ValueObjectIsDirty)
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return MyBase.IsValid AndAlso (_AttachedObject Is Nothing _
                    OrElse Not _AttachedObject.ValueObjectHasErrors)
            End Get
        End Property



        Private Sub CalculateSumVat()
            _SumVat = CRound(CRound(_Sum * _VatRate / 100) + _SumVatCorrection / 100)
            _SumTotal = CRound(_Sum + _SumVat)
            PropertyHasChanged("SumVat")
            PropertyHasChanged("SumTotal")
        End Sub

        Friend Sub CalculateSum(ByVal invoiceCurrencyRate As Double)
            _Sum = CRound(CRound(_UnitValue * _Ammount) + _SumCorrection / 100)
            PropertyHasChanged("Sum")
            CalculateSumVat()
            CalculateSumLTL(invoiceCurrencyRate)
        End Sub

        Private Sub CalculateSumLTL(ByVal invoiceCurrencyRate As Double)

            _UnitValueLTL = CRound(CRound(_UnitValue * GetCurrencyRate(invoiceCurrencyRate), ROUNDUNITINVOICERECEIVED) _
                + _UnitValueLTLCorrection / 100, ROUNDUNITINVOICERECEIVED)
            _SumLTL = CRound(CRound(_Sum * GetCurrencyRate(invoiceCurrencyRate)) + _SumLTLCorrection / 100)

            PropertyHasChanged("UnitValueLTL")
            PropertyHasChanged("SumLTL")

            CalculateSumVatLTL(invoiceCurrencyRate)

        End Sub

        Private Sub CalculateSumVatLTL(ByVal pCurrencyRate As Double)
            _SumVatLTL = CRound(CRound(_SumVat * GetCurrencyRate(pCurrencyRate)) + _SumVatLTLCorrection / 100)
            _SumTotalLTL = CRound(_SumLTL + _SumVatLTL)
            SetAttachedObjectFinancialData()
            PropertyHasChanged("SumVatLTL")
            PropertyHasChanged("SumTotalLTL")
        End Sub


        Friend Function GetCurrencyRate(ByVal invoiceCurrencyRate As Double) As Double

            If CRound(invoiceCurrencyRate, ROUNDCURRENCYRATE) > 0 Then

                Return invoiceCurrencyRate

            ElseIf Parent Is Nothing Then

                Return 1

            ElseIf IsBaseCurrency(DirectCast(Parent, InvoiceReceivedItemList).CurrencyCode, _
                GetCurrentCompany().BaseCurrency) Then

                Return 1

            ElseIf DirectCast(Parent, InvoiceReceivedItemList).CurrencyRate > 0 Then

                Return DirectCast(Parent, InvoiceReceivedItemList).CurrencyRate

            Else

                Return 1

            End If

        End Function

        Private Function GetCurrencyCode() As String

            Dim result As String = GetCurrentCompany().BaseCurrency

            If Not Parent Is Nothing AndAlso Not IsBaseCurrency( _
                DirectCast(Parent, InvoiceReceivedItemList).CurrencyCode, result) Then

                result = DirectCast(Parent, InvoiceReceivedItemList).CurrencyCode

            End If

            Return result.Trim.ToUpper()

        End Function

        Private Function ParentCurrencyIsBaseCurrency() As Boolean
            Return Parent Is Nothing OrElse IsBaseCurrency( _
                DirectCast(Parent, InvoiceReceivedItemList).CurrencyCode, GetCurrentCompany.BaseCurrency)
        End Function


        Friend Sub UpdateCurrencyRate(ByVal newCurrencyRate As Double, ByVal newCurrencyCode As String)

            If Not _AttachedObject Is Nothing AndAlso Not _AttachedObject.ChronologyValidator. _
                FinancialDataCanChange Then

                Throw New Exception(String.Format(My.Resources.Documents_InvoiceReceivedItem_InvalidItemUpdate, _
                    Me.ToString(), _AttachedObject.ChronologyValidator.FinancialDataCanChangeExplanation))

            ElseIf Not _FinancialDataCanChange Then

                Throw New Exception(My.Resources.Documents_InvoiceReceivedItem_InvalidDocumentUpdate)

            End If

            If IsBaseCurrency(newCurrencyCode, GetCurrentCompany.BaseCurrency) Then

                newCurrencyRate = 1

                _UnitValueLTLCorrection = 0
                _SumLTLCorrection = 0
                _SumVatLTLCorrection = 0

                PropertyHasChanged("UnitValueLTLCorrection")
                PropertyHasChanged("SumLTLCorrection")
                PropertyHasChanged("SumVatLTLCorrection")

            End If

            CalculateSumLTL(newCurrencyRate)

        End Sub


        Public Function GetErrorString() As String _
            Implements IGetErrorForListItem.GetErrorString

            If IsValid Then Return ""

            Dim result As String = ""
            result = AddWithNewLine(result, Me.BrokenRulesCollection.ToString( _
                Validation.RuleSeverity.Error), False)
            If Not _AttachedObject Is Nothing Then
                result = AddWithNewLine(result, _AttachedObject.GetAllErrors(), False)
            End If

            Return String.Format(My.Resources.Common_ErrorInItem, Me.ToString, vbCrLf, result)

        End Function

        Public Function GetWarningString() As String _
            Implements IGetErrorForListItem.GetWarningString

            If Not HasWarnings() Then Return ""

            Dim result As String = ""
            result = AddWithNewLine(result, Me.BrokenRulesCollection.ToString( _
                Validation.RuleSeverity.Warning), False)
            If Not _AttachedObject Is Nothing Then
                result = AddWithNewLine(result, _AttachedObject.GetAllWarnings(), False)
            End If

            Return String.Format(My.Resources.Common_WarningInItem, Me.ToString, vbCrLf, result)

        End Function

        Public Function HasWarnings() As Boolean
            Return BrokenRulesCollection.WarningCount > 0 OrElse _
               (Not _AttachedObject Is Nothing AndAlso _AttachedObject.ValueObjectHasWarnings)
        End Function


        Friend Sub SetAttachedObjectFinancialData()
            If _AttachedObject Is Nothing OrElse suspendChildChanged Then Exit Sub
            suspendChildChanged = True
            _AttachedObject.SetInvoiceFinancialData(Me)
            suspendChildChanged = False
        End Sub

        Friend Sub SetAttachedObjectInvoiceDate(ByVal invoiceDate As Date)
            If _AttachedObject Is Nothing Then Exit Sub
            suspendChildChanged = True
            _AttachedObject.SetInvoiceDate(invoiceDate)
            suspendChildChanged = False
        End Sub

        ''' <summary>
        ''' Gets a total sum for an invoice adapter taking into account
        ''' the adapter's ability to handle VAT and the invoice item state
        ''' (<see cref="IncludeVatInObject">IncludeVatInObject</see> property).
        ''' </summary>
        ''' <remarks></remarks>
        Friend Function GetTotalSumForInvoiceAdapter() As Double

            If _AttachedObject Is Nothing Then Return 0.0

            If _AttachedObject.SumVatIsHandledOnRequest AndAlso _IncludeVatInObject Then

                Return _SumTotalLTL

            Else

                Return _SumLTL

            End If

        End Function


        Private Sub AttachedObject_Changed(ByVal sender As Object, _
            ByVal e As System.ComponentModel.PropertyChangedEventArgs)

            If _AttachedObject Is Nothing OrElse suspendChildChanged Then
                PropertyHasChanged("AttachedObject")
                Exit Sub
            End If

            If _AttachedObject.HandlesMeasureUnit AndAlso StringIsNullOrEmpty(_MeasureUnit) Then
                _MeasureUnit = _AttachedObject.MeasureUnit
                PropertyHasChanged("MeasureUnit")
            End If

            If _AttachedObject.HandlesNameInvoice AndAlso StringIsNullOrEmpty(_NameInvoice) Then
                _NameInvoice = _AttachedObject.NameInvoice
                PropertyHasChanged("NameInvoice")
            End If

            If _AttachedObject.HandlesAccount AndAlso _AccountCosts _
                <> _AttachedObject.Account Then
                _AccountCosts = _AttachedObject.Account
                PropertyHasChanged("AccountIncome")
            End If

            Dim needsRecalculation As Boolean = False
            Dim needsToCalculateUnitValue As Boolean = False

            If _AttachedObject.HandlesAmount AndAlso CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED) _
                <> CRound(_AttachedObject.Amount, ROUNDAMOUNTINVOICERECEIVED) Then

                If CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED) = 0 Then
                    needsToCalculateUnitValue = True
                End If

                _Ammount = CRound(_AttachedObject.Amount, ROUNDAMOUNTINVOICERECEIVED)
                PropertyHasChanged("Ammount")
                needsRecalculation = True

            End If

            If _AttachedObject.HandlesVatRate AndAlso CRound(_AttachedObject.VatRate, 2) <> _
                CRound(_VatRate, 2) Then
                _VatRate = CRound(_AttachedObject.VatRate, 2)
                PropertyHasChanged("VatRate")
                needsRecalculation = True
            End If

            If needsRecalculation Then
                suspendChildChanged = True
                ' If needsToCalculateUnitValue Then CalculateUnitValue()
                CalculateSum(0)
                suspendChildChanged = False
            End If

            PropertyHasChanged("AttachedObject")

        End Sub

        Private Sub CalculateUnitValue()

            If _AttachedObject Is Nothing OrElse Not _AttachedObject.HandlesAmount _
                OrElse Not _AttachedObject.HandlesSum OrElse _
                CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED) = 0 Then Exit Sub

            Dim baseSum As Double

            If Not _AttachedObject.SumVatIsHandledOnRequest OrElse Not _IncludeVatInObject _
                OrElse Not CRound(_VatRate, 2) > 0 Then
                baseSum = _AttachedObject.Sum
            Else
                baseSum = CRound(_AttachedObject.Sum / ((_VatRate + 100) / 100), 2)
            End If

            Dim result As Double = CRound(baseSum / _Ammount, ROUNDUNITINVOICERECEIVED)

            If ParentCurrencyIsBaseCurrency() Then
                _UnitValue = result
            ElseIf GetCurrencyRate(0) > 0 Then
                _UnitValue = CRound(result / GetCurrencyRate(0))
            Else
                _UnitValue = result
            End If

        End Sub

        ''' <summary>
        ''' Helper method. Takes care of child objects loosing their handlers.
        ''' </summary>
        Protected Overrides Function GetClone() As Object
            Dim result As InvoiceReceivedItem = DirectCast(MyBase.GetClone(), InvoiceReceivedItem)
            result.RestoreChildHandles()
            Return result
        End Function

        Protected Overrides Sub OnDeserialized(ByVal context As System.Runtime.Serialization.StreamingContext)
            MyBase.OnDeserialized(context)
            RestoreChildHandles()
        End Sub

        Protected Overrides Sub UndoChangesComplete()
            MyBase.UndoChangesComplete()
            RestoreChildHandles()
        End Sub

        ''' <summary>
        ''' Helper method. Takes care of InvoiceAdapter loosing its handler. See GetClone method.
        ''' </summary>
        Private Sub RestoreChildHandles()
            If _AttachedObject Is Nothing Then Exit Sub
            Try
                RemoveHandler DirectCast(_AttachedObject, Csla.Core.BusinessBase). _
                    PropertyChanged, AddressOf AttachedObject_Changed
            Catch ex As Exception
            End Try
            AddHandler DirectCast(_AttachedObject, Csla.Core.BusinessBase). _
                PropertyChanged, AddressOf AttachedObject_Changed
        End Sub


        Friend Sub MarkAsCopy()

            _ID = -1
            _Guid = Guid.NewGuid
            _FinancialDataCanChange = True

            If Not _AttachedObject Is Nothing Then

                Try
                    RemoveHandler DirectCast(_AttachedObject, Csla.Core.BusinessBase). _
                        PropertyChanged, AddressOf AttachedObject_Changed
                Catch ex As Exception
                End Try

                If _AttachedObject.ImplementsCopy Then
                    _AttachedObject = _AttachedObject.GetCopy()
                    AddHandler DirectCast(_AttachedObject, Csla.Core.BusinessBase). _
                        PropertyChanged, AddressOf AttachedObject_Changed
                Else
                    _AttachedObject = Nothing
                End If

            End If

            MarkNew()

        End Sub

        ''' <summary>
        ''' Gets item data as a data transfer object.
        ''' </summary>
        ''' <remarks>Used to implement data exchange with other applications.</remarks>
        Friend Function GetInvoiceItemInfo() As InvoiceInfo.InvoiceItemInfo

            Dim result As New InvoiceInfo.InvoiceItemInfo

            result.Ammount = _Ammount
            result.AccountIncome = _AccountCosts
            result.AccountVat = _AccountVat
            result.Comments = ""
            result.Discount = 0
            result.DiscountCorrection = 0
            result.DiscountLTL = 0
            result.DiscountLTLCorrection = 0
            result.DiscountVat = 0
            result.DiscountVatCorrection = 0
            result.DiscountVatLTL = 0
            result.DiscountVatLTLCorrection = 0
            result.ID = _ID.ToString
            result.MeasureUnit = _MeasureUnit
            result.MeasureUnitAltLng = ""
            result.NameInvoice = _NameInvoice
            result.NameInvoiceAltLng = ""
            result.ProjectCode = ""
            result.Sum = _Sum
            result.SumCorrection = _SumCorrection
            result.SumLTL = _SumLTL
            result.SumLTLCorrection = _SumLTLCorrection
            result.SumReceived = 0
            result.SumTotal = _SumTotal
            result.SumTotalLTL = _SumTotalLTL
            result.SumVat = _SumVat
            result.SumVatCorrection = _SumVatCorrection
            result.SumVatLTL = _SumVatLTL
            result.SumVatLTLCorrection = _SumVatLTLCorrection
            result.UnitValue = _UnitValue
            result.UnitValueCorrection = 0
            result.UnitValueLTL = _UnitValueLTL
            result.UnitValueLTLCorrection = _UnitValueLTLCorrection
            result.VatRate = _VatRate
            If Not _DeclarationSchema Is Nothing AndAlso Not _DeclarationSchema.IsEmpty Then _
                result.VatDDeclarationSchemaID = _DeclarationSchema.ExternalCode

            Return result

        End Function


        Protected Overrides Function GetIdValue() As Object
            Return _Guid
        End Function

        Public Overrides Function ToString() As String
            Return String.Format(My.Resources.Documents_InvoiceReceivedItem_ToString, _
                _NameInvoice, DblParser(_Ammount, ROUNDAMOUNTINVOICERECEIVED), _MeasureUnit, _
                DblParser(_UnitValue, ROUNDUNITINVOICERECEIVED), GetCurrencyCode())
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Validation.RuleArgs("NameInvoice"))
            ValidationRules.AddRule(AddressOf CommonValidation.StringFieldValidation, _
                New Validation.RuleArgs("MeasureUnit"))
            ValidationRules.AddRule(AddressOf CommonValidation.DoubleFieldValidation, _
                New Validation.RuleArgs("UnitValue"))
            ValidationRules.AddRule(AddressOf CommonValidation.IntegerFieldValidation, _
                New Validation.RuleArgs("SumCorrection"))
            ValidationRules.AddRule(AddressOf CommonValidation.VatDeclarationSchemaFieldValidation, _
                New CommonValidation.VatDeclarationSchemaFieldRuleArgs("DeclarationSchema", _
                "VatRate"))

            ValidationRules.AddRule(AddressOf AccountCostsValidation, "AccountCosts")
            ValidationRules.AddRule(AddressOf AccountVatValidation, "AccountVat")
            ValidationRules.AddRule(AddressOf AmmountValidation, "Ammount")
            ValidationRules.AddRule(AddressOf SumVatCorrectionValidation, "SumVatCorrection")
            ValidationRules.AddRule(AddressOf UnitValueLTLCorrectionValidation, "UnitValueLTLCorrection")
            ValidationRules.AddRule(AddressOf SumLTLCorrectionValidation, "SumLTLCorrection")
            ValidationRules.AddRule(AddressOf SumVatLTLCorrectionValidation, "SumVatLTLCorrection")
            ValidationRules.AddRule(AddressOf SumLTLValidation, "SumLTL")
            ValidationRules.AddRule(AddressOf SumTotalLTLValidation, "SumTotalLTL")
            ValidationRules.AddRule(AddressOf AttachedObjectValidation, "AttachedObject")


            ValidationRules.AddDependantProperty("VatRate", "AccountVat", False)
            ValidationRules.AddDependantProperty("IncludeVatInObject", "AccountVat", False)
            ValidationRules.AddDependantProperty("UnitValue", "Ammount", False)
            ValidationRules.AddDependantProperty("VatRate", "SumVatCorrection", False)
            ValidationRules.AddDependantProperty("VatRate", "SumVatLTLCorrection", False)
            ValidationRules.AddDependantProperty("VatRate", "DeclarationSchema", False)

            ValidationRules.AddDependantProperty("AttachedObject", "AccountCosts", False)
            ValidationRules.AddDependantProperty("AttachedObject", "Ammount", False)
            ValidationRules.AddDependantProperty("AttachedObject", "SumLTL", False)
            ValidationRules.AddDependantProperty("AttachedObject", "SumTotalLTL", False)

        End Sub


        ''' <summary>
        ''' Rule ensuring that AccountCosts is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AccountCostsValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If Not valObj._AttachedObject Is Nothing AndAlso _
                valObj._AttachedObject.HandlesAccount Then

                Return valObj._AttachedObject.ValidateAccount(valObj, e)

            End If

            Return CommonValidation.CommonValidation.AccountFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that AccountVat is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AccountVatValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If CRound(valObj._VatRate) > 0 AndAlso (Not valObj._IncludeVatInObject OrElse _
                valObj._AttachedObject Is Nothing OrElse _
                Not valObj._AttachedObject.SumVatIsHandledOnRequest) Then

                Return CommonValidation.CommonValidation.AccountFieldValidation(target, e)

            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that Ammount is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AmmountValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            If Not CommonValidation.CommonValidation.DoubleFieldValidation(target, e) Then Return False

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If CRound(valObj._UnitValue, ROUNDUNITINVOICERECEIVED) < 0 _
                AndAlso CRound(valObj._Ammount, ROUNDUNITINVOICERECEIVED) < 0 Then

                e.Description = My.Resources.Documents_InvoiceReceivedItem_UnitValueAndAmountBothNegative
                e.Severity = Validation.RuleSeverity.Error
                Return False

            ElseIf Not valObj._AttachedObject Is Nothing AndAlso _
                valObj._AttachedObject.HandlesAmount Then

                Return valObj._AttachedObject.ValidateAmount(valObj, e)

            End If

            Return True

        End Function

        ''' <summary>
        ''' Rule ensuring that SumVatCorrection is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function SumVatCorrectionValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If Not CRound(valObj._VatRate, 2) > 0 AndAlso _
                valObj._SumVatCorrection <> 0 Then

                e.Description = My.Resources.Documents_InvoiceReceivedItem_SumVatCorrectionInvalid
                e.Severity = Validation.RuleSeverity.Error
                Return False

            End If

            Return CommonValidation.CommonValidation.IntegerFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that UnitValueLTLCorrection is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function UnitValueLTLCorrectionValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If valObj.ParentCurrencyIsBaseCurrency() AndAlso _
                valObj._UnitValueLTLCorrection <> 0 Then

                e.Description = My.Resources.Documents_InvoiceReceivedItem_BaseCurrencyCorrectionInvalid
                e.Severity = Validation.RuleSeverity.Error
                Return False

            End If

            Return CommonValidation.CommonValidation.IntegerFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that SumLTLCorrection is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function SumLTLCorrectionValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If valObj.ParentCurrencyIsBaseCurrency() AndAlso _
                valObj._SumLTLCorrection <> 0 Then

                e.Description = My.Resources.Documents_InvoiceReceivedItem_BaseCurrencyCorrectionInvalid
                e.Severity = Validation.RuleSeverity.Error
                Return False

            End If

            Return CommonValidation.CommonValidation.IntegerFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that SumVatLTLCorrection is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function SumVatLTLCorrectionValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If valObj.ParentCurrencyIsBaseCurrency() AndAlso _
                valObj._SumVatLTLCorrection <> 0 Then

                e.Description = My.Resources.Documents_InvoiceReceivedItem_BaseCurrencyCorrectionInvalid
                e.Severity = Validation.RuleSeverity.Error
                Return False

            ElseIf Not CRound(valObj._VatRate, 2) > 0 AndAlso _
                valObj._SumVatLTLCorrection <> 0 Then

                e.Description = My.Resources.Documents_InvoiceReceivedItem_SumVatCorrectionInvalid
                e.Severity = Validation.RuleSeverity.Error
                Return False

            End If

            Return CommonValidation.CommonValidation.IntegerFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that SumLTL is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function SumLTLValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If Not valObj._AttachedObject Is Nothing AndAlso _
                valObj._AttachedObject.HandlesSum Then
                Return valObj._AttachedObject.ValidateSum(valObj, e)
            End If

            Return CommonValidation.CommonValidation.DoubleFieldValidation(target, e)

        End Function

        ''' <summary>
        ''' Rule ensuring that SumTotalLTL is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function SumTotalLTLValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If Not valObj._AttachedObject Is Nothing AndAlso _
                valObj._AttachedObject.HandlesSum Then
                Return valObj._AttachedObject.ValidateTotalSum(valObj, e)
            End If

            Return CommonValidation.CommonValidation.DoubleFieldValidation(target, e)

        End Function


        ''' <summary>
        ''' Rule ensuring AttachedObject is valid.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Private Shared Function AttachedObjectValidation(ByVal target As Object, _
            ByVal e As Validation.RuleArgs) As Boolean

            Dim valObj As InvoiceReceivedItem = DirectCast(target, InvoiceReceivedItem)

            If Not valObj._AttachedObject Is Nothing Then

                If valObj._AttachedObject.ValueObjectHasErrors Then

                    e.Description = valObj._AttachedObject.GetAllErrors()
                    e.Severity = Validation.RuleSeverity.Error
                    Return False

                ElseIf valObj._AttachedObject.ValueObjectHasWarnings Then

                    e.Description = valObj._AttachedObject.GetAllWarnings()
                    e.Severity = Validation.RuleSeverity.Warning
                    Return False

                End If

            End If

            Return True

        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

        End Sub

#End Region

#Region " Factory Methods "

        Friend Shared Function NewInvoiceReceivedItem() As InvoiceReceivedItem
            Return New InvoiceReceivedItem(Nothing)
        End Function

        Friend Shared Function NewInvoiceReceivedItem(ByVal newAttachedObject As IInvoiceAdapter) As InvoiceReceivedItem
            Return New InvoiceReceivedItem(newAttachedObject)
        End Function

        Friend Shared Function NewInvoiceReceivedItem(ByVal info As InvoiceInfo.InvoiceItemInfo,
            ByVal invoiceCurrencyRate As Double, ByVal accountList As AccountInfoList,
            ByVal vatSchemaList As VatDeclarationSchemaInfoList) As InvoiceReceivedItem
            Return New InvoiceReceivedItem(info, invoiceCurrencyRate, accountList, vatSchemaList)
        End Function


        Friend Shared Function GetInvoiceReceivedItem(ByVal dr As DataRow, ByVal invoiceCurrencyRate As Double, _
            ByVal baseChronologyValidator As SimpleChronologicValidator) As InvoiceReceivedItem
            Return New InvoiceReceivedItem(dr, invoiceCurrencyRate, baseChronologyValidator)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
        End Sub

        Private Sub New(ByVal newAttachedObject As IInvoiceAdapter)
            MarkAsChild()
            Create(newAttachedObject)
        End Sub

        Private Sub New(ByVal info As InvoiceInfo.InvoiceItemInfo, ByVal invoiceCurrencyRate As Double,
            ByVal accountList As AccountInfoList, ByVal vatSchemaList As VatDeclarationSchemaInfoList)
            MarkAsChild()
            Create(info, invoiceCurrencyRate, accountList, vatSchemaList)
        End Sub

        Private Sub New(ByVal dr As DataRow, ByVal invoiceCurrencyRate As Double, _
            ByVal baseChronologyValidator As SimpleChronologicValidator)
            MarkAsChild()
            Fetch(dr, invoiceCurrencyRate, baseChronologyValidator)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Create()

            _MeasureUnit = GetCurrentCompany.MeasureUnitInvoiceReceived
            _AccountVat = GetCurrentCompany.Accounts.GetAccount(General.DefaultAccountType.VatReceivable)

            If GetCurrentCompany.UseVatDeclarationSchemas AndAlso GetCurrentCompany.DeclarationSchemaPurchase _
                <> VatDeclarationSchemaInfo.Empty Then
                _DeclarationSchema = GetCurrentCompany.DeclarationSchemaPurchase
                _VatRate = GetCurrentCompany.DeclarationSchemaPurchase.VatRate
            Else
                _VatRate = GetCurrentCompany.GetDefaultRate(General.DefaultRateType.Vat)
            End If

        End Sub

        Private Sub Create(ByVal newAttachedObject As IInvoiceAdapter)

            Create()

            If newAttachedObject Is Nothing Then
                ValidationRules.CheckRules()
                Exit Sub
            End If

            If newAttachedObject.IsForInvoiceMade Then
                Throw New InvalidOperationException(My.Resources.Documents_InvoiceReceivedItem_InvalidInvoiceAdapter)
            End If

            _AttachedObject = newAttachedObject

            If newAttachedObject.ProvidesDefaultVatRate Then
                If GetCurrentCompany.UseVatDeclarationSchemas Then
                    _DeclarationSchema = newAttachedObject.DefaultDeclarationSchema
                    If _DeclarationSchema <> VatDeclarationSchemaInfo.Empty Then
                        _VatRate = _DeclarationSchema.VatRate
                    End If
                Else
                    _VatRate = newAttachedObject.DefaultVatRate
                End If
            End If

            If newAttachedObject.ProvidesDefaultAccount Then
                _AccountCosts = newAttachedObject.DefaultAccount
            End If

            If newAttachedObject.ProvidesDefaultMeasureUnit Then
                _MeasureUnit = newAttachedObject.DefaultMeasureUnit
            End If

            If newAttachedObject.ProvidesDefaultNameInvoice Then
                _NameInvoice = newAttachedObject.DefaultNameInvoice
            End If

            If newAttachedObject.ProvidesDefaultVatAccount Then
                _AccountVat = newAttachedObject.DefaultVatAccount
            End If

            RestoreChildHandles()

            MarkNew()

            ValidationRules.CheckRules()

        End Sub

        Private Sub Create(ByVal info As InvoiceInfo.InvoiceItemInfo, ByVal invoiceCurrencyRate As Double,
            ByVal accountList As AccountInfoList, ByVal vatSchemaList As VatDeclarationSchemaInfoList)

            Me._Ammount = info.Ammount
            Me._MeasureUnit = info.MeasureUnit
            Me._NameInvoice = info.NameInvoice
            Me._Sum = info.Sum
            Me._SumTotal = info.SumTotal
            Me._SumVat = info.SumVat
            Me._UnitValue = info.UnitValue
            Me._VatRate = info.VatRate

            If invoiceCurrencyRate = 1.0 OrElse invoiceCurrencyRate = 0.0 Then
                Me._SumLTL = info.Sum
                Me._SumTotalLTL = info.SumTotal
                Me._SumVatLTL = info.SumVat
                Me._UnitValueLTL = info.UnitValue
            Else
                Me._SumLTL = info.SumLTL
                Me._SumTotalLTL = info.SumTotalLTL
                Me._SumVatLTL = info.SumVatLTL
                Me._UnitValueLTL = info.UnitValueLTL
            End If

            If Not accountList.GetAccountByID(info.AccountIncome) Is Nothing Then
                Me._AccountCosts = info.AccountIncome
            End If
            If Not accountList.GetAccountByID(info.AccountVat) Is Nothing Then
                Me._AccountVat = info.AccountVat
            End If

            If Not StringIsNullOrEmpty(info.VatDDeclarationSchemaID) Then
                Me._DeclarationSchema = vatSchemaList.GetItem(info.VatDDeclarationSchemaID)
            End If

            CalculateCorrections(invoiceCurrencyRate)

            ValidationRules.CheckRules()

        End Sub


        Private Sub Fetch(ByVal dr As DataRow, ByVal invoiceCurrencyRate As Double, _
            ByVal baseChronologyValidator As SimpleChronologicValidator)

            _ID = CIntSafe(dr.Item(0), 0)
            _NameInvoice = CStrSafe(dr.Item(1)).Trim
            _Ammount = CDblSafe(dr.Item(2), ROUNDAMOUNTINVOICERECEIVED, 0)
            _UnitValueLTL = CDblSafe(dr.Item(3), ROUNDUNITINVOICERECEIVED, 0)
            _SumLTL = CDblSafe(dr.Item(4), 2, 0)
            _VatRate = CDblSafe(dr.Item(5), 2, 0)
            _SumVatLTL = CDblSafe(dr.Item(6), 2, 0)
            _UnitValue = CDblSafe(dr.Item(7), ROUNDUNITINVOICERECEIVED, 0)
            _Sum = CDblSafe(dr.Item(8), 2, 0)
            _SumVat = CDblSafe(dr.Item(9), 2, 0)
            _MeasureUnit = CStrSafe(dr.Item(10)).Trim
            _AccountVat = CLongSafe(dr.Item(13), 0)
            _AccountCosts = CLongSafe(dr.Item(14), 0)
            _IncludeVatInObject = ConvertDbBoolean(CIntSafe(dr.Item(15), 0))
            _DeclarationSchema = VatDeclarationSchemaInfo.GetVatDeclarationSchemaInfo(dr, 16)

            CalculateCorrections(invoiceCurrencyRate)

            _SumTotal = CRound(_Sum + _SumVat)
            _SumTotalLTL = CRound(_SumLTL + _SumVatLTL)

            If CIntSafe(dr.Item(11), 0) > 0 AndAlso CIntSafe(dr.Item(12), 0) > 0 Then
                _AttachedObject = InvoiceAdapterFactory.GetInvoiceAdapter( _
                    Utilities.ConvertDatabaseID(Of InvoiceAdapterType) _
                    (CIntSafe(dr.Item(11), 0)), CIntSafe(dr.Item(12), 0), _
                    baseChronologyValidator, False)
                RestoreChildHandles()
            End If

            _FinancialDataCanChange = baseChronologyValidator.FinancialDataCanChange

            MarkOld()

            ValidationRules.CheckRules()

        End Sub

        Private Sub CalculateCorrections(ByVal invoiceCurrencyRate As Double)

            '_UnitValueLTL = CRound(CRound(_UnitValue * GetCurrencyRate(pCurrencyRate), 4) _
            '   + _UnitValueLTLCorrection / 100, 4)
            _UnitValueLTLCorrection = Convert.ToInt32(Math.Floor(CRound(_UnitValueLTL - _
                CRound(_UnitValue * GetCurrencyRate(invoiceCurrencyRate), ROUNDUNITINVOICERECEIVED)) * 100))
            ' _SumLTL = CRound(CRound(_Sum * GetCurrencyRate(pCurrencyRate)) + _SumLTLCorrection / 100)
            _SumLTLCorrection = Convert.ToInt32(Math.Floor(CRound(_SumLTL - CRound(_Sum * GetCurrencyRate(invoiceCurrencyRate))) * 100))
            ' _SumVatLTL = CRound(CRound(_SumVat * GetCurrencyRate(pCurrencyRate)) + _SumVatLTLCorrection / 100)
            _SumVatLTLCorrection = Convert.ToInt32(Math.Floor(CRound(_SumVatLTL - CRound(_SumVat * GetCurrencyRate(invoiceCurrencyRate))) * 100))
            ' _Sum = CRound(CRound(_UnitValue * _Ammount) + _SumCorrection / 100)
            _SumCorrection = Convert.ToInt32(Math.Floor(CRound(_Sum - CRound(_UnitValue * _Ammount)) * 100))
            ' _SumVat = CRound(CRound(_Sum * _VatRate / 100) + _SumVatCorrection / 100)
            _SumVatCorrection = Convert.ToInt32(Math.Floor(CRound(_SumVat - CRound(_Sum * _VatRate / 100)) * 100))

        End Sub


        Friend Sub Insert(ByVal parentInvoice As InvoiceReceived)

            If Not _AttachedObject Is Nothing Then
                _AttachedObject.Update(parentInvoice)
            End If

            Dim myComm As New SQLCommand("InsertInvoiceReceivedItem")
            myComm.AddParam("?AA", parentInvoice.ID)
            If Not _AttachedObject Is Nothing Then
                myComm.AddParam("?AB", Utilities.ConvertDatabaseID(_AttachedObject.Type))
                myComm.AddParam("?AC", _AttachedObject.Id)
            Else
                myComm.AddParam("?AB", 0)
                myComm.AddParam("?AC", 0)
            End If
            AddWithParamsGeneral(myComm)
            AddWithParamsFinancial(myComm)

            myComm.Execute()

            _ID = Convert.ToInt32(myComm.LastInsertID)

            MarkOld()

        End Sub

        Friend Sub Update(ByVal parentInvoice As InvoiceReceived)

            If Not _AttachedObject Is Nothing Then
                _AttachedObject.Update(parentInvoice)
            End If

            Dim myComm As SQLCommand
            If FinancialDataCanChange() Then
                myComm = New SQLCommand("UpdateInvoiceReceivedItem")
                AddWithParamsFinancial(myComm)
            Else
                myComm = New SQLCommand("UpdateInvoiceReceivedItemGeneral")
            End If
            myComm.AddParam("?MD", _ID)
            AddWithParamsGeneral(myComm)

            myComm.Execute()

            MarkOld()

        End Sub

        Private Sub AddWithParamsGeneral(ByRef myComm As SQLCommand)
            myComm.AddParam("?AD", _NameInvoice.Trim)
            myComm.AddParam("?AE", _MeasureUnit.Trim)
        End Sub

        Private Sub AddWithParamsFinancial(ByRef myComm As SQLCommand)
            myComm.AddParam("?AF", CRound(_UnitValue, ROUNDUNITINVOICERECEIVED))
            myComm.AddParam("?AG", CRound(_Ammount, ROUNDAMOUNTINVOICERECEIVED))
            myComm.AddParam("?AH", CRound(_VatRate))
            myComm.AddParam("?AI", _AccountVat)
            myComm.AddParam("?AJ", _AccountCosts)
            myComm.AddParam("?AK", CRound(_Sum))
            myComm.AddParam("?AL", CRound(_SumVat))
            myComm.AddParam("?AM", CRound(_UnitValueLTL, ROUNDUNITINVOICERECEIVED))
            myComm.AddParam("?AN", CRound(_SumLTL))
            myComm.AddParam("?AO", CRound(_SumVatLTL))
            myComm.AddParam("?AQ", ConvertDbBoolean(_IncludeVatInObject))
            If _DeclarationSchema Is Nothing OrElse _DeclarationSchema.IsEmpty Then
                myComm.AddParam("?AP", 0)
            Else
                myComm.AddParam("?AP", _DeclarationSchema.ID)
            End If
        End Sub


        Friend Sub DeleteSelf()

            If Not _AttachedObject Is Nothing Then
                _AttachedObject.DeleteSelf()
            End If

            Dim myComm As New SQLCommand("DeleteInvoiceReceivedItem")
            myComm.AddParam("?MD", _ID)

            myComm.Execute()

            MarkNew()

        End Sub


        Friend Sub SetParentData(ByVal parentInvoice As InvoiceReceived)
            If Not _AttachedObject Is Nothing Then
                suspendChildChanged = True
                _AttachedObject.SetInvoiceDate(parentInvoice.Date)
                _AttachedObject.SetParentData(parentInvoice)
                suspendChildChanged = False
            End If
        End Sub

        Friend Sub CheckIfCanUpdate(ByVal parentChronologyValidator As IChronologicValidator)
            If Not _AttachedObject Is Nothing Then
                _AttachedObject.CheckIfCanUpdate(parentChronologyValidator)
            End If
        End Sub

        Friend Sub CheckIfCanDelete(ByVal parentChronologyValidator As IChronologicValidator)

            If Not parentChronologyValidator.FinancialDataCanChange Then
                Throw New Exception(String.Format(My.Resources.Documents_InvoiceReceivedItem_InvalidItemDelete, _
                    Me.ToString(), vbCrLf, parentChronologyValidator.FinancialDataCanChangeExplanation))
            End If

            If Not _AttachedObject Is Nothing Then
                _AttachedObject.CheckIfCanDelete(parentChronologyValidator)
            End If

        End Sub


        Friend Function GetBookEntryInternalList(ByVal accountSupplier As Long) As BookEntryInternalList

            Dim result As BookEntryInternalList = BookEntryInternalList. _
                NewBookEntryInternalList(BookEntryType.Debetas)

            ' normal or debit invoice
            If CRound(_SumLTL) > 0 Then

                ' without attached object
                If _AttachedObject Is Nothing Then

                    result.Add(BookEntryInternal.NewBookEntryInternal( _
                        BookEntryType.Debetas, _AccountCosts, CRound(_SumLTL), Nothing))

                    If CRound(_SumVatLTL) > 0 Then result.Add(BookEntryInternal.NewBookEntryInternal( _
                        BookEntryType.Debetas, _AccountVat, CRound(_SumVatLTL), Nothing))

                    ' with delegation to attached object
                Else

                    result.AddRange(_AttachedObject.GetBookEntryList(Me))

                    If (Not _AttachedObject.SumVatIsHandledOnRequest OrElse _
                        Not _IncludeVatInObject) AndAlso CRound(_SumVatLTL) > 0 Then

                        result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas, _
                            _AccountVat, CRound(_SumVatLTL), Nothing))

                    End If

                End If

                result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas, _
                    accountSupplier, CRound(_SumTotalLTL), Nothing))

                ' Credit invoice
            Else

                ' without attached object
                If _AttachedObject Is Nothing Then

                    result.Add(BookEntryInternal.NewBookEntryInternal( _
                        BookEntryType.Kreditas, _AccountCosts, CRound(-_SumLTL), Nothing))

                    If CRound(-_SumVatLTL) > 0 Then result.Add(BookEntryInternal.NewBookEntryInternal( _
                        BookEntryType.Kreditas, _AccountVat, CRound(-_SumVatLTL), Nothing))

                    ' with delegation to attached object
                Else

                    result.AddRange(_AttachedObject.GetBookEntryList(Me))

                    If (Not _AttachedObject.SumVatIsHandledOnRequest OrElse _
                        Not _IncludeVatInObject) AndAlso CRound(-_SumVatLTL) > 0 Then

                        result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Kreditas, _
                            _AccountVat, CRound(-_SumVatLTL), Nothing))

                    End If

                End If

                result.Add(BookEntryInternal.NewBookEntryInternal(BookEntryType.Debetas, _
                    accountSupplier, CRound(-_SumTotalLTL), Nothing))

            End If

            Return result

        End Function

#End Region

    End Class

End Namespace