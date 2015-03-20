Namespace Workers

    <Serializable()> _
    Public Class WageSheet
        Inherits BusinessBase(Of WageSheet)
        Implements IIsDirtyEnough

#Region " Business Methods "

        Private Enum WageSheetFetchMode
            NewWageSheet
            OldWageSheet
            Info
        End Enum

        Private _ID As Integer = 0
        Private _ChronologicValidator As SheetChronologicValidator
        Private _Number As Integer = 0
        Private _IsNonClosing As Boolean = False
        Private _CostAccount As Long = 0
        Private _Remarks As String = ""
        Private _Year As Integer = 0
        Private _Month As Integer = 0
        Private _Date As Date = Today
        Private _OldDate As Date = Today
        Private WithEvents _Items As WageItemList
        Private _TotalSum As Double = 0
        Private _TotalSumAfterDeductions As Double = 0
        Private WithEvents _WageRates As CompanyWageRates
        Private _InsertDate As DateTime = Now
        Private _UpdateDate As DateTime = Now

        Private SuspendChildListChangedEvents As Boolean = False
        ' used to implement automatic sort in datagridview
        <NotUndoable()> _
        <NonSerialized()> _
        Private _ItemsSortedList As Csla.SortedBindingList(Of WageItem) = Nothing


        Public ReadOnly Property ID() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property ChronologicValidator() As SheetChronologicValidator
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

        Public Property Number() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Number
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Integer)
                CanWriteProperty(True)
                If _Number <> value Then
                    _Number = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property IsNonClosing() As Boolean
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _IsNonClosing
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Boolean)
                CanWriteProperty(True)
                If _IsNonClosing <> value Then
                    _IsNonClosing = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property CostAccount() As Long
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _CostAccount
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As Long)
                CanWriteProperty(True)
                If Not _ChronologicValidator.FinancialDataCanChange Then Exit Property
                If _CostAccount <> value Then
                    _CostAccount = value
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public Property Remarks() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Remarks.Trim
            End Get
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Set(ByVal value As String)
                CanWriteProperty(True)
                If value Is Nothing Then value = ""
                If _Remarks.Trim <> value.Trim Then
                    _Remarks = value.Trim
                    PropertyHasChanged()
                End If
            End Set
        End Property

        Public ReadOnly Property Year() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Year
            End Get
        End Property

        Public ReadOnly Property Month() As Integer
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Month
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

        Public ReadOnly Property OldDate() As Date
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _OldDate
            End Get
        End Property

        Public ReadOnly Property Items() As WageItemList
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Items
            End Get
        End Property

        Public ReadOnly Property ItemsSorted() As Csla.SortedBindingList(Of WageItem)
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                If _ItemsSortedList Is Nothing Then _ItemsSortedList = _
                    New Csla.SortedBindingList(Of WageItem)(_Items)
                Return _ItemsSortedList
            End Get
        End Property

        Public ReadOnly Property TotalSum() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalSum)
            End Get
        End Property

        Public ReadOnly Property TotalSumAfterDeductions() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return CRound(_TotalSumAfterDeductions)
            End Get
        End Property

        Public ReadOnly Property RateSODRAEmployee() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RateSODRAEmployee
            End Get
        End Property

        Public ReadOnly Property RateSODRAEmployer() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RateSODRAEmployer
            End Get
        End Property

        Public ReadOnly Property RatePSDEmployee() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RatePSDEmployee
            End Get
        End Property

        Public ReadOnly Property RatePSDEmployer() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RatePSDEmployer
            End Get
        End Property

        Public ReadOnly Property RateGuaranteeFund() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RateGuaranteeFund
            End Get
        End Property

        Public ReadOnly Property RateGPM() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RateGPM
            End Get
        End Property

        Public ReadOnly Property RateHR() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RateHR
            End Get
        End Property

        Public ReadOnly Property RateSC() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RateSC
            End Get
        End Property

        Public ReadOnly Property RateON() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RateON
            End Get
        End Property

        Public ReadOnly Property RateSickLeave() As Double
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.RateSickLeave
            End Get
        End Property

        Public ReadOnly Property NPDFormula() As String
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates.NPDFormula
            End Get
        End Property

        Public ReadOnly Property WageRates() As CompanyWageRates
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _WageRates
            End Get
        End Property


        Public ReadOnly Property IsDirtyEnough() As Boolean _
            Implements IIsDirtyEnough.IsDirtyEnough
            Get
                If Not IsNew Then Return IsDirty
                Return (Not String.IsNullOrEmpty(_Remarks.Trim) OrElse _WageRates.IsDirty)
            End Get
        End Property


        Public Overrides ReadOnly Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty OrElse _Items.IsDirty OrElse _WageRates.IsDirty
            End Get
        End Property

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return MyBase.IsValid AndAlso _Items.IsValid AndAlso _WageRates.IsValid
            End Get
        End Property



        Public Overrides Function Save() As WageSheet

            Me.ValidationRules.CheckRules()
            If Not IsValid Then Throw New Exception("Duomenyse yra klaidų: " & vbCrLf & Me.GetAllBrokenRules)

            Return MyBase.Save

        End Function


        Public Sub UpdateTaxRates()

            If Not _ChronologicValidator.FinancialDataCanChange Then _
                Throw New Exception("Klaida. Taisyti finansinių žiniaraščio duomenų neleidžiama:" _
                & vbCrLf & _ChronologicValidator.FinancialDataCanChangeExplanation)

            SuspendChildListChangedEvents = True

            _WageRates.UpdateTaxRates()
            PropertyHasChanged("RateSODRAEmployee")
            PropertyHasChanged("RateSODRAEmployer")
            PropertyHasChanged("RatePSDEmployee")
            PropertyHasChanged("RatePSDEmployer")
            PropertyHasChanged("RateGuaranteeFund")
            PropertyHasChanged("RateGPM")
            PropertyHasChanged("NPDFormula")

            SuspendChildListChangedEvents = False

            _Items.UpdateTaxRates(_WageRates, True)

        End Sub

        Public Sub UpdateWageRates()

            If Not _ChronologicValidator.FinancialDataCanChange Then _
                Throw New Exception("Klaida. Taisyti finansinių žiniaraščio duomenų neleidžiama:" _
                & vbCrLf & _ChronologicValidator.FinancialDataCanChangeExplanation)

            SuspendChildListChangedEvents = True

            _WageRates.UpdateWageRates()
            PropertyHasChanged("RateHR")
            PropertyHasChanged("RateSC")
            PropertyHasChanged("RateON")
            PropertyHasChanged("RateSickLeave")

            SuspendChildListChangedEvents = False

            _Items.UpdateWageRates(_WageRates, True)

        End Sub


        Public Sub CalculateNPD()
            If Not _ChronologicValidator.FinancialDataCanChange Then _
                Throw New Exception("Klaida. Taisyti finansinių žiniaraščio duomenų neleidžiama:" _
                & vbCrLf & _ChronologicValidator.FinancialDataCanChangeExplanation)
            _Items.CalculateNPD(True)
        End Sub

        Public Sub UpdateWorkersVDUInfo(ByVal NewWorkersVDUInfo As WorkersVDUInfoList)
            If Not _ChronologicValidator.FinancialDataCanChange Then _
                Throw New Exception("Klaida. Taisyti finansinių žiniaraščio duomenų neleidžiama:" _
                & vbCrLf & _ChronologicValidator.FinancialDataCanChangeExplanation)
            _Items.UpdateWorkersVDUInfo(NewWorkersVDUInfo, True)
        End Sub

        Public Function GetWorkersVDUInfoArray() As WorkersVDUInfo()
            Dim resultList As New List(Of WorkersVDUInfo)
            For Each item As WageItem In _Items
                If item.IsChecked Then resultList.Add( _
                    WorkersVDUInfo.GetNewWorkersVDUInfo( _
                    item.ContractSerial, item.ContractNumber))
            Next
            Return resultList.ToArray
        End Function


        Public Function GetAllBrokenRules() As String
            Dim result As String = ""
            If Not MyBase.IsValid Then result = AddWithNewLine(result, _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error), False)
            If Not _Items.IsValid Then result = AddWithNewLine(result, _
                _Items.GetAllBrokenRules, False)
            If Not _WageRates.IsValid Then result = AddWithNewLine(result, _
                _WageRates.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error), False)
            Return result
        End Function

        Public Function GetAllWarnings() As String
            Dim result As String = ""
            If Not MyBase.BrokenRulesCollection.WarningCount > 0 Then result = AddWithNewLine(result, _
                Me.BrokenRulesCollection.ToString(Validation.RuleSeverity.Warning), False)
            result = AddWithNewLine(result, _Items.GetAllWarnings, False)
            result = AddWithNewLine(result, _WageRates.BrokenRulesCollection.ToString( _
                Validation.RuleSeverity.Warning), False)
            Return result
        End Function


        Private Sub Items_Changed(ByVal sender As Object, _
            ByVal e As System.ComponentModel.ListChangedEventArgs) Handles _Items.ListChanged

            If SuspendChildListChangedEvents Then Exit Sub

            _TotalSum = _Items.GetTotalSum
            _TotalSumAfterDeductions = _Items.GetTotalSumAfterDeductions
            PropertyHasChanged("TotalSum")
            PropertyHasChanged("TotalSumAfterDeductions")

        End Sub

        Private Sub WageRates_PropertyChanged(ByVal sender As Object, _
            ByVal e As System.ComponentModel.PropertyChangedEventArgs) _
            Handles _WageRates.PropertyChanged

            If SuspendChildListChangedEvents Then Exit Sub

            Dim wageRatesHasChanged As Boolean = True
            Dim taxRatesHasChanged As Boolean = True
            Dim propertyChangeWasHandled As Boolean = False

            If Not e.PropertyName Is Nothing AndAlso Not String.IsNullOrEmpty(e.PropertyName.Trim) Then

                propertyChangeWasHandled = True

                Select Case e.PropertyName.Trim
                    Case "RateHR"
                        taxRatesHasChanged = False
                    Case "RateSC"
                        taxRatesHasChanged = False
                    Case "RateON"
                        taxRatesHasChanged = False
                    Case "RateSickLeave"
                        taxRatesHasChanged = False
                    Case "RateSODRAEmployee"
                        wageRatesHasChanged = False
                    Case "RateSODRAEmployer"
                        wageRatesHasChanged = False
                    Case "RatePSDEmployee"
                        wageRatesHasChanged = False
                    Case "RatePSDEmployer"
                        wageRatesHasChanged = False
                    Case "RateGuaranteeFund"
                        wageRatesHasChanged = False
                    Case "RateGPM"
                        wageRatesHasChanged = False
                    Case "NPDFormula"
                        wageRatesHasChanged = False
                    Case Else
                        propertyChangeWasHandled = False
                End Select

            End If

            If wageRatesHasChanged Then _Items.UpdateWageRates(_WageRates, True)
            If taxRatesHasChanged Then _Items.UpdateTaxRates(_WageRates, True)

            If propertyChangeWasHandled Then
                PropertyHasChanged(e.PropertyName.Trim)
            Else
                If wageRatesHasChanged Then
                    PropertyHasChanged("RateHR")
                    PropertyHasChanged("RateSC")
                    PropertyHasChanged("RateON")
                    PropertyHasChanged("RateSickLeave")
                End If
                If taxRatesHasChanged Then
                    PropertyHasChanged("RateSODRAEmployee")
                    PropertyHasChanged("RateSODRAEmployer")
                    PropertyHasChanged("RatePSDEmployee")
                    PropertyHasChanged("RatePSDEmployer")
                    PropertyHasChanged("RateGuaranteeFund")
                    PropertyHasChanged("RateGPM")
                    PropertyHasChanged("NPDFormula")
                End If
            End If

        End Sub

        ''' <summary>
        ''' Helper method. Takes care of child lists loosing their handlers.
        ''' </summary>
        Protected Overrides Function GetClone() As Object
            Dim result As WageSheet = DirectCast(MyBase.GetClone(), WageSheet)
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
                RemoveHandler _Items.ListChanged, AddressOf Items_Changed
            Catch ex As Exception
            End Try
            Try
                RemoveHandler _WageRates.PropertyChanged, AddressOf WageRates_PropertyChanged
            Catch ex As Exception
            End Try
            AddHandler _Items.ListChanged, AddressOf Items_Changed
            AddHandler _WageRates.PropertyChanged, AddressOf WageRates_PropertyChanged
        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _ID
        End Function

        Public Overrides Function ToString() As String
            Return _Date.ToShortDateString & " Darbo užmokesčio žiniaraštis Nr. " _
                & _Number.ToString & " (ID=" & _ID.ToString & ")"
        End Function

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("Number", "žiniaraščio numeris"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("CostAccount", "sąnaudų sąskaita"))
            ValidationRules.AddRule(AddressOf CommonValidation.PositiveNumberRequired, _
                New CommonValidation.SimpleRuleArgs("TotalSum", _
                "bendra priskaičiuoto darbo užmokesčio suma (nepasirinkta nė viena " _
                & "eilutė arba pasirinktose eilutėse nepriskaičiuotas DU)"))
            ValidationRules.AddRule(AddressOf CommonValidation.ChronologyValidation, _
                New CommonValidation.ChronologyRuleArgs("Date", "ChronologicValidator"))
        End Sub

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()
            AuthorizationRules.AllowWrite("Workers.WageSheet2")
        End Sub

        Public Shared Function CanAddObject() As Boolean
            Return ApplicationContext.User.IsInRole("Workers.WageSheet2")
        End Function

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("Workers.WageSheet1")
        End Function

        Public Shared Function CanEditObject() As Boolean
            Return ApplicationContext.User.IsInRole("Workers.WageSheet3")
        End Function

        Public Shared Function CanDeleteObject() As Boolean
            Return ApplicationContext.User.IsInRole("Workers.WageSheet3")
        End Function

#End Region

#Region " Factory Methods "

        Public Shared Function NewWageSheet(ByVal nYear As Integer, ByVal nMonth As Integer) As WageSheet

            If Not nYear > 0 OrElse Not nMonth > 0 Then Throw New Exception( _
                "Klaida. Nenurodyta mėnuo ir (ar) metai.")

            Dim result As WageSheet = DataPortal.Create(Of WageSheet)(New Criteria(nYear, nMonth))
            result.MarkNew()
            Return result

        End Function

        Public Shared Function GetWageSheet(ByVal nID As Integer) As WageSheet
            Return DataPortal.Fetch(Of WageSheet)(New Criteria(nID))
        End Function

        Public Shared Sub DeleteWageSheet(ByVal id As Integer)
            DataPortal.Delete(New Criteria(id))
        End Sub

        Private Sub New()
            ' require use of factory methods
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria
            Private mId As Integer
            Private _Year As Integer
            Private _Month As Integer
            Public ReadOnly Property Id() As Integer
                Get
                    Return mId
                End Get
            End Property
            Public ReadOnly Property Year() As Integer
                Get
                    Return _Year
                End Get
            End Property
            Public ReadOnly Property Month() As Integer
                Get
                    Return _Month
                End Get
            End Property
            Public Sub New(ByVal id As Integer)
                mId = id
                _Year = 0
                _Month = 0
            End Sub
            Public Sub New(ByVal nYear As Integer, ByVal nMonth As Integer)
                mId = 0
                _Year = nYear
                _Month = nMonth
            End Sub
        End Class


        Private Overloads Sub DataPortal_Create(ByVal criteria As Criteria)

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujiems duomenims įvesti.")

            _WageRates = CompanyWageRates.NewCompanyWageRates
            _Year = criteria.Year
            _Month = criteria.Month
            _Date = New Date(criteria.Year, criteria.Month, _
                Date.DaysInMonth(criteria.Year, criteria.Month))
            _OldDate = _Date

            Dim myComm As New SQLCommand("FetchNewWageSheet")
            myComm.AddParam("?DT", New Date(_Year, _Month, Date.DaysInMonth(_Year, _Month)))
            myComm.AddParam("?DA", New Date(_Year, _Month, 1))
            myComm.AddParam("?YR", _Year)
            myComm.AddParam("?MN", _Month)

            Using myData As DataTable = myComm.Fetch
                _Items = WageItemList.NewWageItemList(myData, _WageRates, _Year, _Month)
            End Using

            _ChronologicValidator = SheetChronologicValidator.NewSheetChronologicValidator( _
                DocumentType.WageSheet, _Year, _Month)

            MarkNew()

            ValidationRules.CheckRules()

        End Sub

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims gauti.")

            Dim myComm As New SQLCommand("FetchWageGeneralData")
            myComm.AddParam("?SD", criteria.Id)

            Using myData As DataTable = myComm.Fetch

                If Not myData.Rows.Count = 1 Then Throw New Exception( _
                    "Klaida. Nepavyko gauti bendrų darbo užmokesčio žiniaraščio duomenų.")

                Dim dr As DataRow = myData.Rows(0)

                _ID = criteria.Id
                _Number = CIntSafe(dr.Item(0), 0)
                _Date = CDateSafe(dr.Item(1), Today)
                _OldDate = _Date
                _Year = CIntSafe(dr.Item(2), 0)
                _Month = CIntSafe(dr.Item(3), 0)
                _IsNonClosing = (CStrSafe(dr.Item(4)).Trim <> "n")
                _CostAccount = CIntSafe(dr.Item(5), 0)
                _Remarks = CStrSafe(dr.Item(6)).Trim
                ' _PayedOut = (CDblSafe(dr.Item(7), 2, 0) > 0)

                _ChronologicValidator = SheetChronologicValidator.GetSheetChronologicValidator( _
                    DocumentType.WageSheet, _ID)

                _WageRates = New CompanyWageRates(CDblSafe(dr.Item(14)), _
                    CDblSafe(dr.Item(15)), CDblSafe(dr.Item(16)), CDblSafe(dr.Item(17)), _
                    CDblSafe(dr.Item(13)), CDblSafe(dr.Item(12)), CStrSafe(dr.Item(18)), _
                    CDblSafe(dr.Item(8)), CDblSafe(dr.Item(10)), CDblSafe(dr.Item(9)), _
                    CDblSafe(dr.Item(11)), _ChronologicValidator.FinancialDataCanChange)
                _InsertDate = DateTime.SpecifyKind(CDateTimeSafe(dr.Item(19), Date.UtcNow), _
                    DateTimeKind.Utc).ToLocalTime
                _UpdateDate = DateTime.SpecifyKind(CDateTimeSafe(dr.Item(20), Date.UtcNow), _
                    DateTimeKind.Utc).ToLocalTime

            End Using

            _ChronologicValidator = SheetChronologicValidator.GetSheetChronologicValidator( _
               DocumentType.WageSheet, _ID)

            myComm = New SQLCommand("FetchWageDetails")
            myComm.AddParam("?SD", criteria.Id)
            myComm.AddParam("?DT", New Date(_Year, _Month, Date.DaysInMonth(_Year, _Month)))
            myComm.AddParam("?YR", _Year)
            myComm.AddParam("?MN", _Month)

            Using myData As DataTable = myComm.Fetch
                _Items = WageItemList.GetWageItemList(myData, _WageRates, _Year, _Month, _
                    _ChronologicValidator.FinancialDataCanChange)
            End Using

            _TotalSum = _Items.GetTotalSum
            _TotalSumAfterDeductions = _Items.GetTotalSumAfterDeductions

            MarkOld()

            ValidationRules.CheckRules()

        End Sub


        Protected Overrides Sub DataPortal_Insert()

            If Not CanAddObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka naujiems duomenims įvesti.")

            Dim JE As General.JournalEntry = GetJournalEntry()

            DatabaseAccess.TransactionBegin()

            JE = JE.SaveChild()

            _ID = JE.ID

            Dim myComm As New SQLCommand("InsertWageSheet")
            AddWithParamsGeneral(myComm)
            AddWithParamsFinancial(myComm)
            myComm.AddParam("?ME", _Year)
            myComm.AddParam("?MN", _Month)
            myComm.Execute()

            Items.Update(Me)

            DatabaseAccess.TransactionCommit()

            MarkOld()

        End Sub

        Protected Overrides Sub DataPortal_Update()

            If Not CanEditObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims pakeisti.")

            _ChronologicValidator = SheetChronologicValidator.GetSheetChronologicValidator( _
                DocumentType.WageSheet, _ID)
            ValidationRules.CheckRules()
            If Not IsValid Then Throw New Exception("Dokumente yra klaidų:" & vbCrLf & GetAllBrokenRules())

            Dim JE As General.JournalEntry = GetJournalEntry()

            CheckIfUpdateDateChanged()

            Dim myComm As SQLCommand
            If _ChronologicValidator.FinancialDataCanChange Then
                myComm = New SQLCommand("UpdateWageSheet")
                AddWithParamsFinancial(myComm)
            Else
                myComm = New SQLCommand("UpdateWageSheetNonFinancial")
            End If
            AddWithParamsGeneral(myComm)

            DatabaseAccess.TransactionBegin()

            JE = JE.SaveChild()

            myComm.Execute()

            If _ChronologicValidator.FinancialDataCanChange Then Items.Update(Me)

            DatabaseAccess.TransactionCommit()

            MarkOld()

        End Sub


        Protected Overrides Sub DataPortal_DeleteSelf()
            DataPortal_Delete(New Criteria(_ID))
        End Sub

        Protected Overrides Sub DataPortal_Delete(ByVal criteria As Object)

            If Not CanDeleteObject() Then Throw New System.Security.SecurityException( _
                "Klaida. Jūsų teisių nepakanka duomenims pašalinti.")

            SheetChronologicValidator.CheckIfCanDelete(DocumentType.WageSheet, DirectCast(criteria, Criteria).Id)

            IndirectRelationInfoList.CheckIfJournalEntryCanBeDeleted(DirectCast(criteria, Criteria).Id, DocumentType.WageSheet)

            Dim myComm As New SQLCommand("DeleteAllWageItems")
            myComm.AddParam("?SD", DirectCast(criteria, Criteria).Id)

            DatabaseAccess.TransactionBegin()

            General.JournalEntry.DeleteJournalEntryChild(DirectCast(criteria, Criteria).Id)

            myComm.Execute()

            myComm = New SQLCommand("DeleteWageSheet")
            myComm.AddParam("?SD", DirectCast(criteria, Criteria).Id)

            myComm.Execute()

            DatabaseAccess.TransactionCommit()

            MarkNew()

        End Sub


        Private Sub AddWithParamsGeneral(ByRef myComm As SQLCommand)

            myComm.AddParam("?BD", _ID)
            myComm.AddParam("?ZD", _Date.Date)
            myComm.AddParam("?NR", _Number)
            If _IsNonClosing Then
                myComm.AddParam("?DA", "t")
            Else
                myComm.AddParam("?DA", "n")
            End If
            myComm.AddParam("?RM", _Remarks)

            _UpdateDate = DateTime.Now
            _UpdateDate = New DateTime(Convert.ToInt64(Math.Floor(_UpdateDate.Ticks / TimeSpan.TicksPerSecond) _
                * TimeSpan.TicksPerSecond))
            If Me.IsNew Then _InsertDate = _UpdateDate
            myComm.AddParam("?UD", _UpdateDate.ToUniversalTime)

        End Sub

        Private Sub AddWithParamsFinancial(ByRef myComm As SQLCommand)

            myComm.AddParam("?PH", _WageRates.RateHR)
            myComm.AddParam("?NV", _WageRates.RateON)
            myComm.AddParam("?YS", _WageRates.RateSC)
            myComm.AddParam("?NE", _WageRates.RateSickLeave)
            myComm.AddParam("?GP", _WageRates.RateGPM)
            myComm.AddParam("?GA", _WageRates.RateGuaranteeFund)
            myComm.AddParam("?SD", _WageRates.RateSODRAEmployee)
            myComm.AddParam("?SV", _WageRates.RateSODRAEmployer)
            myComm.AddParam("?PW", _WageRates.RatePSDEmployee)
            myComm.AddParam("?PE", _WageRates.RatePSDEmployer)
            myComm.AddParam("?NF", _WageRates.NPDFormula.Trim)
            myComm.AddParam("?SA", _CostAccount)

        End Sub

        Private Function GetJournalEntry() As General.JournalEntry

            Dim result As General.JournalEntry = _
                General.JournalEntry.NewJournalEntryChild(DocumentType.WageSheet)

            If IsNew Then
                result = General.JournalEntry.NewJournalEntryChild(DocumentType.WageSheet)
            Else
                result = General.JournalEntry.GetJournalEntryChild(_ID, DocumentType.WageSheet)
            End If

            result.Content = "Darbo užmokesčio žiniaraštis už " & _Year.ToString & " m. " & _
                _Month.ToString & " mėn."
            result.Date = _Date.Date
            result.DocNumber = "DUŽ-" & _Number.ToString

            If _ChronologicValidator.FinancialDataCanChange Then

                result.CreditList.Clear()
                result.DebetList.Clear()

                Dim nTotalSODRA As Double = _Items.GetTotalSODRAPayments
                Dim nTotalPSD As Double = _Items.GetTotalPSDPaymentsForSODRA
                Dim nTotalPSDForVMI As Double = _Items.GetTotalPSDPaymentsForVMI
                Dim nTotalGPM As Double = _Items.GetTotalGPMDeductions
                Dim nTotalGF As Double = _Items.GetTotalGuaranteeFundContributions
                Dim nTotalWageForPayOut As Double = _Items.GetTotalSumAfterDeductions
                Dim nTotalImprestDeductions As Double = _Items.GetTotalImprestDeductions
                Dim nTotalOtherDeductions As Double = _Items.GetTotalOtherDeductions
                Dim nTotalCosts As Double = _Items.GetTotalCosts

                If Not CRound(nTotalCosts) > 0 Then
                    Throw New Exception("Klaida. Žiniaraštyje nėra priskaičiuota jokių sąnaudų.")
                Else
                    Dim DebetEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                    DebetEntry.Amount = CRound(nTotalCosts)
                    DebetEntry.Account = _CostAccount
                    result.DebetList.Add(DebetEntry)
                End If

                If CRound(nTotalWageForPayOut) > 0 Then
                    Dim CreditEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                    CreditEntry.Amount = CRound(nTotalWageForPayOut)
                    CreditEntry.Account = GetCurrentCompany.Accounts.GetAccount( _
                        General.DefaultAccountType.WagePayable)
                    result.CreditList.Add(CreditEntry)
                End If

                If CRound(nTotalGF) > 0 Then
                    Dim CreditEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                    CreditEntry.Amount = CRound(nTotalGF)
                    CreditEntry.Account = GetCurrentCompany.Accounts.GetAccount( _
                        General.DefaultAccountType.WageGuaranteeFundPayable)
                    result.CreditList.Add(CreditEntry)
                End If

                If CRound(nTotalOtherDeductions) > 0 Then
                    Dim CreditEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                    CreditEntry.Amount = CRound(nTotalOtherDeductions)
                    CreditEntry.Account = GetCurrentCompany.Accounts.GetAccount( _
                        General.DefaultAccountType.WageWithdraw)
                    result.CreditList.Add(CreditEntry)
                End If

                If CRound(nTotalImprestDeductions) > 0 Then
                    Dim CreditEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                    CreditEntry.Amount = CRound(nTotalImprestDeductions)
                    CreditEntry.Account = GetCurrentCompany.Accounts.GetAccount( _
                        General.DefaultAccountType.WageImprestPayable)
                    result.CreditList.Add(CreditEntry)
                End If

                If CRound(nTotalGPM) > 0 Then
                    Dim CreditEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                    CreditEntry.Amount = CRound(nTotalGPM)
                    CreditEntry.Account = GetCurrentCompany.Accounts.GetAccount( _
                        General.DefaultAccountType.WageGpmPayable)
                    result.CreditList.Add(CreditEntry)
                End If

                If CRound(nTotalSODRA) > 0 OrElse CRound(nTotalPSD) > 0 Then

                    If GetCurrentCompany.Accounts.GetAccount(General.DefaultAccountType.WagePsdPayable) _
                        = GetCurrentCompany.Accounts.GetAccount( _
                        General.DefaultAccountType.WageSodraPayable) Then

                        Dim CreditEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                        CreditEntry.Amount = CRound(CRound(nTotalSODRA) + CRound(nTotalPSD))
                        CreditEntry.Account = GetCurrentCompany.Accounts.GetAccount( _
                            General.DefaultAccountType.WageSodraPayable)
                        result.CreditList.Add(CreditEntry)

                    Else

                        If CRound(nTotalSODRA) > 0 Then
                            Dim CreditEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                            CreditEntry.Amount = CRound(nTotalSODRA)
                            CreditEntry.Account = GetCurrentCompany.Accounts.GetAccount( _
                                General.DefaultAccountType.WageSodraPayable)
                            result.CreditList.Add(CreditEntry)
                        End If
                        If CRound(nTotalPSD) > 0 Then
                            Dim CreditEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                            CreditEntry.Amount = CRound(nTotalPSD)
                            CreditEntry.Account = GetCurrentCompany.Accounts.GetAccount( _
                                General.DefaultAccountType.WagePsdPayable)
                            result.CreditList.Add(CreditEntry)
                        End If

                    End If

                End If

                If CRound(nTotalPSDForVMI) > 0 Then
                    Dim CreditEntry As General.BookEntry = General.BookEntry.NewBookEntry()
                    CreditEntry.Amount = CRound(nTotalPSDForVMI)
                    CreditEntry.Account = GetCurrentCompany.Accounts.GetAccount( _
                        General.DefaultAccountType.WagePsdPayableToVMI)
                    result.CreditList.Add(CreditEntry)
                End If

            End If

            If CRound(result.CreditList.GetSum) <> CRound(result.DebetList.GetSum) Then
                Throw New Exception("Klaida. Nepavyko suformuoti bendrojo žurnalo įrašo. " & _
                    "Gaunama debeto suma - " & DblParser(result.DebetList.GetSum) & _
                    "; kredito - " & DblParser(result.CreditList.GetSum))
            End If

            Return result

        End Function

        Private Sub CheckIfUpdateDateChanged()

            Dim myComm As New SQLCommand("CheckIfWageSheetUpdateDateChanged")
            myComm.AddParam("?CD", _ID)

            Using myData As DataTable = myComm.Fetch
                If myData.Rows.Count < 1 OrElse CDateTimeSafe(myData.Rows(0).Item(0), _
                    Date.MinValue) = Date.MinValue Then Throw New Exception( _
                    "Klaida. Objektas, kurio ID=" & _ID.ToString & ", nerastas.")
                If DateTime.SpecifyKind(CDateTimeSafe(myData.Rows(0).Item(0), DateTime.UtcNow), _
                    DateTimeKind.Utc).ToLocalTime <> _UpdateDate Then Throw New Exception( _
                    "Klaida. Dokumento atnaujinimo data pasikeitė. Teigtina, kad kitas " _
                    & "vartotojas redagavo šį objektą.")
            End Using

        End Sub

#End Region

    End Class

End Namespace