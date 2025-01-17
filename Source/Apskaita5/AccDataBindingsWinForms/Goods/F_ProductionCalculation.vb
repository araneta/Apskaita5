Imports ApskaitaObjects.Goods
Imports AccControlsWinForms
Imports AccDataBindingsWinForms.CachedInfoLists

Friend Class F_ProductionCalculation
    Implements IObjectEditForm

    Private ReadOnly _RequiredCachedLists As Type() = New Type() _
        {GetType(HelperLists.GoodsInfoList), GetType(HelperLists.AccountInfoList)}

    Private WithEvents _FormManager As CslaActionExtenderEditForm(Of ProductionCalculation)
    Private _ComponentListViewManager As DataListViewEditControlManager(Of ProductionComponentItem)
    Private _CostsListViewManager As DataListViewEditControlManager(Of ProductionCostItem)

    Private _DocumentToEdit As ProductionCalculation = Nothing


    Public ReadOnly Property ObjectID() As Integer Implements IObjectEditForm.ObjectID
        Get
            If _FormManager Is Nothing OrElse _FormManager.DataSource Is Nothing Then
                If _DocumentToEdit Is Nothing OrElse _DocumentToEdit.IsNew Then
                    Return Integer.MinValue
                Else
                    Return _DocumentToEdit.ID
                End If
            End If
            Return _FormManager.DataSource.ID
        End Get
    End Property

    Public ReadOnly Property ObjectType() As System.Type Implements IObjectEditForm.ObjectType
        Get
            Return GetType(ProductionCalculation)
        End Get
    End Property


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal documentToEdit As ProductionCalculation)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _DocumentToEdit = documentToEdit

    End Sub


    Private Sub F_ProductionCalculation_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        If _DocumentToEdit Is Nothing Then
            _DocumentToEdit = ProductionCalculation.NewProductionCalculation()
        End If

        If Not SetDataSources() Then Exit Sub

        Try

            _FormManager = New CslaActionExtenderEditForm(Of ProductionCalculation) _
                (Me, ProductionCalculationBindingSource, _DocumentToEdit, _
                _RequiredCachedLists, nOkButton, ApplyButton, nCancelButton, _
                Nothing, ProgressFiller1)

            _FormManager.ManageDataListViewStates(Me.ComponentListDataListView, CostListDataListView)

        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Exit Sub
        End Try

        ConfigureButtons()

    End Sub

    Private Function SetDataSources() As Boolean

        If Not PrepareCache(Me, _RequiredCachedLists) Then Return False

        Try

            _ComponentListViewManager = New DataListViewEditControlManager(Of ProductionComponentItem) _
                (ComponentListDataListView, Nothing, AddressOf OnComponentItemsDelete, _
                 AddressOf OnComponentItemAdd, Nothing, _DocumentToEdit)

            _CostsListViewManager = New DataListViewEditControlManager(Of ProductionCostItem) _
                (CostListDataListView, Nothing, AddressOf OnCostsItemsDelete, _
                 AddressOf OnCostsItemAdd, Nothing, _DocumentToEdit)

            SetupDefaultControls(Of ProductionCalculation)(Me, _
                ProductionCalculationBindingSource, _DocumentToEdit)

        Catch ex As Exception
            ShowError(ex, Nothing)
            DisableAllControls(Me)
            Return False
        End Try

        Return True

    End Function


    Private Sub OnComponentItemAdd()
        _FormManager.DataSource.ComponentList.AddNew()
    End Sub

    Private Sub OnComponentItemsDelete(ByVal items As ProductionComponentItem())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As ProductionComponentItem In items
            _FormManager.DataSource.ComponentList.Remove(item)
        Next
    End Sub

    Private Sub OnCostsItemAdd()
        _FormManager.DataSource.CostList.AddNew()
    End Sub

    Private Sub OnCostsItemsDelete(ByVal items As ProductionCostItem())
        If items Is Nothing OrElse items.Length < 1 OrElse _FormManager.DataSource Is Nothing Then Exit Sub
        For Each item As ProductionCostItem In items
            _FormManager.DataSource.CostList.Remove(item)
        Next
    End Sub


    Private Sub _FormManager_DataSourceStateHasChanged(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles _FormManager.DataSourceStateHasChanged
        ConfigureButtons()
    End Sub

    Private Sub ConfigureButtons()

        nCancelButton.Enabled = Not _FormManager.DataSource Is Nothing AndAlso Not _FormManager.DataSource.IsNew
        nOkButton.Enabled = Not _FormManager.DataSource Is Nothing
        ApplyButton.Enabled = Not _FormManager.DataSource Is Nothing

        EditedBaner.Visible = Not _FormManager.DataSource Is Nothing AndAlso Not _FormManager.DataSource.IsNew

    End Sub

End Class