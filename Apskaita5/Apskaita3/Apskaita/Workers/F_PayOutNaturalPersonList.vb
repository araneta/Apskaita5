Imports ApskaitaObjects.Workers
Imports ApskaitaObjects.HelperLists
Imports ApskaitaObjects.ActiveReports
Public Class F_PayOutNaturalPersonList
    Implements ISupportsPrinting

    Private Obj As PayOutNaturalPersonInfoList = Nothing
    Private Loading As Boolean = True


    Private Sub F_PayOutNaturalPersonList_Activated(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles Me.Activated

        If Me.WindowState = FormWindowState.Maximized AndAlso MyCustomSettings.AutoSizeForm Then _
            Me.WindowState = FormWindowState.Normal

        If Loading Then
            Loading = False
            Exit Sub
        End If

    End Sub

    Private Sub F_PayOutNaturalPersonList_FormClosing(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        GetDataGridViewLayOut(PayOutNaturalPersonItemListDataGridView)
        GetFormLayout(Me)

    End Sub

    Private Sub F_PayOutNaturalPersonList_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        Me.DateFromDateTimePicker.Value = Today.AddMonths(-1)

        NewItemButton.Enabled = PayOutNaturalPerson.CanAddObject
        RefreshButton.Enabled = PayOutNaturalPersonInfoList.CanGetObject

        AddDGVColumnSelector(PayOutNaturalPersonItemListDataGridView)
        SetDataGridViewLayOut(PayOutNaturalPersonItemListDataGridView)
        SetFormLayout(Me)

        InitializeMenu(Of PayOutNaturalPersonInfo)()

    End Sub


    Private Sub NewItemButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles NewItemButton.Click
        NewItem(Nothing)
    End Sub

    Private Sub RefreshButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles RefreshButton.Click
        DoRefresh(DateFromDateTimePicker.Value, DateToDateTimePicker.Value)
    End Sub

    Private Sub InitializeMenu(Of T As PayOutNaturalPersonInfo)()

        Dim w As New ToolStripHelper(Of T)(PayOutNaturalPersonItemListDataGridView, _
            ContextMenuStrip1, "", True)

        w.AddMenuItemHandler(ChangeItem_MenuItem, New DelegateContainer(Of T)(AddressOf ChangeItem))
        w.AddMenuItemHandler(DeleteItem_MenuItem, New DelegateContainer(Of T)(AddressOf DeleteItem))
        w.AddMenuItemHandler(NewItem_MenuItem, New DelegateContainer(Of T)(AddressOf NewItem))

        w.AddButtonHandler("Keisti", "Keisti išmokos fiziniam asmeniui duomenis.", _
            New DelegateContainer(Of T)(AddressOf ChangeItem))
        w.AddButtonHandler("Ištrinti", "Pašalinti išmokos fiziniam asmeniui duomenis iš duomenų bazės.", _
            New DelegateContainer(Of T)(AddressOf DeleteItem))

    End Sub

    Private Sub ChangeItem(ByVal item As PayOutNaturalPersonInfo)
        If item Is Nothing Then Exit Sub
        MDIParent1.LaunchForm(GetType(F_PayOutNaturalPerson), False, False, item.ID, item.ID)
    End Sub

    Private Sub DeleteItem(ByVal item As PayOutNaturalPersonInfo)

        If item Is Nothing Then Exit Sub

        For Each frm As Form In MDIParent1.MdiChildren
            If TypeOf frm Is F_PayOutNaturalPerson AndAlso DirectCast(frm, F_PayOutNaturalPerson).ObjectID = item.ID Then
                MsgBox("Negalima pašalinti duomenų, kol jie yra redaguojami. Uždarykite redagavimo formą.", _
                    MsgBoxStyle.Exclamation, "Klaida")
                frm.Activate()
                Exit Sub
            End If
        Next

        If Not YesOrNo("Ar tikrai norite pašalinti išmokos duomenis iš duomenų bazės?") Then Exit Sub

        Try
            Using busy As New StatusBusy
                PayOutNaturalPerson.DeletePayOutNaturalPerson(item.ID)
            End Using
        Catch ex As Exception
            ShowError(ex)
            Exit Sub
        End Try

        If Not YesOrNo("Išmokos duomenys sėkmingai pašalinti iš duomenų bazės. Atnaujinti sąrašą?") Then Exit Sub

        DoRefresh(Obj.DateFrom, Obj.DateTo)

    End Sub

    Private Sub NewItem(ByVal item As PayOutNaturalPersonInfo)
        MDIParent1.LaunchForm(GetType(F_PayOutNaturalPerson), False, False, 0)
    End Sub


    Public Function GetMailDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetMailDropDownItems
        Return Nothing
    End Function

    Public Function GetPrintDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetPrintDropDownItems
        Return Nothing
    End Function

    Public Function GetPrintPreviewDropDownItems() As System.Windows.Forms.ToolStripDropDown _
        Implements ISupportsPrinting.GetPrintPreviewDropDownItems
        Return Nothing
    End Function

    Public Sub OnMailClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnMailClick
        If Obj Is Nothing Then Exit Sub

        Using frm As New F_SendObjToEmail(Obj, 0)
            frm.ShowDialog()
        End Using

    End Sub

    Public Sub OnPrintClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintClick
        If Obj Is Nothing Then Exit Sub
        Try
            PrintObject(Obj, False, 0)
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Public Sub OnPrintPreviewClick(ByVal sender As Object, ByVal e As System.EventArgs) _
        Implements ISupportsPrinting.OnPrintPreviewClick
        If Obj Is Nothing Then Exit Sub
        Try
            PrintObject(Obj, True, 0)
        Catch ex As Exception
            ShowError(ex)
        End Try
    End Sub

    Public Function SupportsEmailing() As Boolean _
        Implements ISupportsPrinting.SupportsEmailing
        Return True
    End Function


    Private Sub DoRefresh(ByVal dateFrom As Date, ByVal dateTo As Date)

        Using bm As New BindingsManager(PayOutNaturalPersonItemListBindingSource, Nothing, Nothing, False, True)
            Try
                Obj = LoadObject(Of PayOutNaturalPersonInfoList)(Nothing, _
                    "GetPayOutNaturalPersonInfoList", False, dateFrom, dateTo)
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try
            bm.SetNewDataSource(Obj.GetSortedList)
        End Using

        PayOutNaturalPersonItemListDataGridView.Select()

    End Sub

End Class