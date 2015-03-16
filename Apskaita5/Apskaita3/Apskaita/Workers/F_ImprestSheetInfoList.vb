Imports ApskaitaObjects.Workers
Imports ApskaitaObjects.ActiveReports
Public Class F_ImprestSheetInfoList
    Implements ISupportsPrinting

    Private Obj As ImprestSheetInfoList

    Private Sub F_ImprestSheetInfoList_Activated(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles Me.Activated
        If Me.WindowState = FormWindowState.Maximized AndAlso MyCustomSettings.AutoSizeForm Then _
            Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub F_ImprestSheetInfoList_FormClosing(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GetDataGridViewLayOut(ImprestSheetInfoListDataGridView)
        GetFormLayout(Me)
    End Sub

    Private Sub F_ImprestSheetInfoList_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        RefreshButton.Enabled = ImprestSheetInfoList.CanGetObject

        DateFromDateTimePicker.Value = Today.Subtract(New TimeSpan(90, 0, 0, 0))

        AddDGVColumnSelector(ImprestSheetInfoListDataGridView)
        SetDataGridViewLayOut(ImprestSheetInfoListDataGridView)
        SetFormLayout(Me)

        InitializeMenu(Of ImprestSheetInfo)()

    End Sub


    Private Sub RefreshButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles RefreshButton.Click
        DoRefresh(DateFromDateTimePicker.Value.Date, DateToDateTimePicker.Value.Date)
    End Sub

    Private Sub ShowNonEmplyedCheckBox_CheckedChanged(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles ShowPayedOutCheckBox.CheckedChanged

        If Obj Is Nothing Then Exit Sub
        Obj.ApplyFilter(ShowPayedOutCheckBox.Checked)

    End Sub

    Private Sub NewButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles NewButton.Click
        MDIParent1.NewImprestSheetMenuItem_Click(New Object, New EventArgs)
    End Sub

    Private Sub InitializeMenu(Of T As ImprestSheetInfo)()

        Dim w As New ToolStripHelper(Of T)(ImprestSheetInfoListDataGridView, _
            ContextMenuStrip1, "", True)

        w.AddMenuItemHandler(ChangeItem_MenuItem, New DelegateContainer(Of T)(AddressOf ChangeItem))
        w.AddMenuItemHandler(PayoutItem_MenuItem, New DelegateContainer(Of T)(AddressOf PayOutItem))
        w.AddMenuItemHandler(DeleteItem_MenuItem, New DelegateContainer(Of T)(AddressOf DeleteItem))
        w.AddMenuItemHandler(NewItem_MenuItem, New DelegateContainer(Of T)(AddressOf NewItem))

        w.AddButtonHandler("Keisti", "Keisti avanso žiniaraščio duomenis.", _
            New DelegateContainer(Of T)(AddressOf ChangeItem))
        w.AddButtonHandler("Išmokėjimai", "Išmokėjimų pgl. žiniaraštį duomenys.", _
            New DelegateContainer(Of T)(AddressOf PayOutItem))
        w.AddButtonHandler("Ištrinti", "Pašalinti avanso žiniaraščio duomenis iš duomenų bazės.", _
            New DelegateContainer(Of T)(AddressOf DeleteItem))

    End Sub

    Private Sub ChangeItem(ByVal item As ImprestSheetInfo)
        If item Is Nothing Then Exit Sub
        MDIParent1.LaunchForm(GetType(F_ImprestSheet), False, False, item.ID, item.ID)
    End Sub

    Private Sub PayOutItem(ByVal item As ImprestSheetInfo)
        If item Is Nothing Then Exit Sub
        MDIParent1.LaunchForm(GetType(F_WagePayOut), False, False, item.ID, item.ID)
    End Sub

    Private Sub DeleteItem(ByVal item As ImprestSheetInfo)

        If item Is Nothing Then Exit Sub

        For Each frm As Form In MDIParent1.MdiChildren
            If TypeOf frm Is F_ImprestSheet AndAlso DirectCast(frm, F_ImprestSheet).ObjectID = item.ID Then
                MsgBox("Negalima pašalinti duomenų, kol jie yra redaguojami. Uždarykite redagavimo formą.", _
                    MsgBoxStyle.Exclamation, "Klaida")
                frm.Activate()
                Exit Sub
            End If
        Next

        If Not YesOrNo("Ar tikrai norite pašalinti avanso žiniaraščio duomenis?") Then Exit Sub

        Try
            Using busy As New StatusBusy
                ImprestSheet.DeleteImprestSheet(item.ID)
            End Using
        Catch ex As Exception
            ShowError(ex)
            Exit Sub
        End Try

        If Not YesOrNo("Avanso žiniaraščio duomenys sėkmingai pašalinti. Atnaujinti sąrašą?") Then Exit Sub

        DoRefresh(Obj.DateFrom, Obj.DateTo)

    End Sub

    Private Sub NewItem(ByVal item As ImprestSheetInfo)
        MDIParent1.NewImprestSheetMenuItem_Click(New Object, New EventArgs)
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

        Using bm As New BindingsManager(ImprestSheetInfoListBindingSource, _
            Nothing, Nothing, False, True)

            Try
                Obj = LoadObject(Of ImprestSheetInfoList)(Nothing, "GetImprestSheetInfoList", _
                    True, dateFrom, dateTo)
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

            Obj.ApplyFilter(ShowPayedOutCheckBox.Checked)
            bm.SetNewDataSource(Obj.GetFilteredList)

        End Using

        ImprestSheetInfoListDataGridView.Select()

    End Sub

End Class