Imports ApskaitaObjects.ActiveReports
Imports ApskaitaObjects.Workers
Public Class F_LabourContractInfoList
    Implements ISupportsPrinting

    Private Obj As ContractInfoList
    Private Loading As Boolean = True

    Private Sub F_LabourContractInfoList_Activated(ByVal sender As Object, _
        ByVal e As System.EventArgs) Handles Me.Activated

        If Me.WindowState = FormWindowState.Maximized AndAlso MyCustomSettings.AutoSizeForm Then _
            Me.WindowState = FormWindowState.Normal

        If Loading Then
            Loading = False
            Exit Sub
        End If

    End Sub

    Private Sub F_LabourContractInfoList_FormClosing(ByVal sender As Object, _
        ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GetDataGridViewLayOut(ContractInfoListDataGridView)
        GetDataGridViewLayOut(UpdatesListDataGridView)
        GetFormLayout(Me)
    End Sub

    Private Sub F_LabourContractInfoList_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load

        AddDGVColumnSelector(ContractInfoListDataGridView)
        AddDGVColumnSelector(UpdatesListDataGridView)

        SetDataGridViewLayOut(ContractInfoListDataGridView)
        SetDataGridViewLayOut(UpdatesListDataGridView)
        SetFormLayout(Me)

        InitializeMenu(Of ContractInfo)()
        InitializeSubMenu(Of LabourContractUpdateInfo)()

    End Sub


    Private Sub RefreshButton_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles RefreshButton.Click

        DoRefresh(Not NotOriginalDataCheckBox.Checked, AtDateCheckBox.Checked, _
            AtDateDateTimePicker.Value, AddWorkersWithoutContractCheckBox.Checked)

        If Obj.Count < 1 AndAlso Not AddWorkersWithoutContractCheckBox.Checked Then
            If YesOrNo("Darbo sutarčių pagal nurodytus parametrus nerasta. Pridėti darbuotojus be darbo sutarčių?") Then
                AddWorkersWithoutContractCheckBox.Checked = True
                DoRefresh(Not NotOriginalDataCheckBox.Checked, AtDateCheckBox.Checked, _
                    AtDateDateTimePicker.Value, AddWorkersWithoutContractCheckBox.Checked)
            End If
        End If

    End Sub

    Private Sub InitializeMenu(Of T As ContractInfo)()

        Dim w As New ToolStripHelper(Of T)(ContractInfoListDataGridView, _
            ContextMenuStrip1, "", True, AddressOf ActionIsAvailable)

        w.AddMenuItemHandler(ChangeItem_MenuItem, New DelegateContainer(Of T)(AddressOf ChangeItem))
        w.AddMenuItemHandler(DeleteItem_MenuItem, New DelegateContainer(Of T)(AddressOf DeleteItem))
        w.AddMenuItemHandler(NewItem_MenuItem, New DelegateContainer(Of T)(AddressOf NewItem))
        w.AddMenuItemHandler(NewItemUpdate_MenuItem, New DelegateContainer(Of T)(AddressOf NewItemUpdate))
        w.AddMenuItemHandler(ItemGeneral_MenuItem, New DelegateContainer(Of T)(AddressOf ItemGeneral))

        w.AddButtonHandler("Keisti", "Keisti darbo sutarties duomenis.", _
            New DelegateContainer(Of T)(AddressOf ChangeItem))
        w.AddButtonHandler("Ištrinti", "Pašalinti darbo sutarties duomenis iš duomenų bazės.", _
            New DelegateContainer(Of T)(AddressOf DeleteItem))
        w.AddButtonHandler("Nauja Sutartis", "Nauja darbo sutartis su darbuotoju.", _
            New DelegateContainer(Of T)(AddressOf NewItem))
        w.AddButtonHandler("Naujas Pakeitimas", "Naujas darbo sutarties pakeitimas.", _
            New DelegateContainer(Of T)(AddressOf NewItemUpdate))
        w.AddButtonHandler("Bendri Duomenys", "Keisti bendrus darbuotojo duomenis.", _
            New DelegateContainer(Of T)(AddressOf ItemGeneral))

    End Sub

    Private Sub ChangeItem(ByVal item As ContractInfo)
        If item Is Nothing Then Exit Sub
        Dim intID As Integer = 0
        If Not Integer.TryParse(item.ID, intID) OrElse Not intID > 0 Then Exit Sub
        MDIParent1.LaunchForm(GetType(F_LabourContract), False, False, intID, intID, False)
    End Sub

    Private Sub DeleteItem(ByVal item As ContractInfo)

        If item Is Nothing Then Exit Sub

        Dim longID As Integer = 0
        If Not Integer.TryParse(item.ID, longID) OrElse Not longID > 0 Then Exit Sub

        For Each frm As Form In MDIParent1.MdiChildren
            If TypeOf frm Is F_LabourContract AndAlso DirectCast(frm, F_LabourContract).ObjectID = longID Then
                MsgBox("Negalima pašalinti duomenų, kol jie yra redaguojami. Uždarykite redagavimo formą.", _
                    MsgBoxStyle.Exclamation, "Klaida")
                frm.Activate()
                Exit Sub
            End If
        Next

        If Not YesOrNo("Ar tikrai norite pašalinti pasirinktos darbo sutarties " & _
            "duomenis iš duomenų bazės?") Then Exit Sub

        Try
            Using busy As New StatusBusy
                Contract.DeleteContract(longID)
            End Using
        Catch ex As Exception
            ShowError(ex)
            Exit Sub
        End Try

        If Not YesOrNo("Darbo sutarties duomenys sėkmingai pašalinti iš duomenų bazės. Atnaujinti sąrašą?") Then Exit Sub

        DoRefresh(Obj.IsOriginalContractData, Obj.OnlyInOperationAtDate, Obj.AtDate, Obj.AddAllWorkers)

    End Sub

    Private Sub NewItem(ByVal item As ContractInfo)
        If item Is Nothing Then Exit Sub
        MDIParent1.LaunchForm(GetType(F_LabourContract), False, False, item.PersonID, item.PersonID, True)
    End Sub

    Private Sub NewItemUpdate(ByVal item As ContractInfo)

        If item Is Nothing Then Exit Sub

        Dim contractNumber As Integer = 0
        If Not Integer.TryParse(item.Number, contractNumber) OrElse Not contractNumber > 0 Then Exit Sub

        MDIParent1.LaunchForm(GetType(F_LabourContractUpdate), False, False, 0, _
            item.Serial, contractNumber)

    End Sub

    Private Sub ItemGeneral(ByVal item As ContractInfo)
        MDIParent1.LaunchForm(GetType(F_Person), False, False, item.PersonID, item.PersonID)
    End Sub

    Private Function ActionIsAvailable(ByVal item As ContractInfo, _
        ByVal actionName As String) As Boolean

        If item Is Nothing OrElse actionName Is Nothing OrElse String.IsNullOrEmpty(actionName.Trim) Then Return False

        Dim contractID As Integer = 0
        If Not Integer.TryParse(item.ID, New Integer) OrElse Not CInt(item.ID) > 0 Then

            If actionName = (New DelegateContainer(Of ContractInfo)(AddressOf ChangeItem)).GetActionName _
                OrElse actionName = (New DelegateContainer(Of ContractInfo)(AddressOf DeleteItem)).GetActionName _
                OrElse actionName = (New DelegateContainer(Of ContractInfo)(AddressOf NewItemUpdate)).GetActionName Then

                Return False

            End If

        End If

        Return True

    End Function

    Private Sub InitializeSubMenu(Of T As LabourContractUpdateInfo)()

        Dim w As New ToolStripHelper(Of T)(UpdatesListDataGridView, _
            ContextMenuStrip2, "", True)

        w.AddMenuItemHandler(ChangeSubItem_MenuItem, New DelegateContainer(Of T)(AddressOf ChangeSubItem))
        w.AddMenuItemHandler(DeleteSubItem_MenuItem, New DelegateContainer(Of T)(AddressOf DeleteSubItem))

        w.AddButtonHandler("Keisti", "Keisti darbo sutarties pakeitimo duomenis.", _
            New DelegateContainer(Of T)(AddressOf ChangeSubItem))
        w.AddButtonHandler("Ištrinti", "Pašalinti darbo sutarties pakeitimo duomenis iš duomenų bazės.", _
            New DelegateContainer(Of T)(AddressOf DeleteSubItem))

    End Sub

    Private Sub ChangeSubItem(ByVal item As LabourContractUpdateInfo)
        If item Is Nothing Then Exit Sub
        MDIParent1.LaunchForm(GetType(F_LabourContractUpdate), False, False, item.ID, item.ID)
    End Sub

    Private Sub DeleteSubItem(ByVal item As LabourContractUpdateInfo)

        If item Is Nothing Then Exit Sub

        For Each frm As Form In MDIParent1.MdiChildren
            If TypeOf frm Is F_LabourContractUpdate AndAlso DirectCast(frm, F_LabourContractUpdate).ObjectID = item.ID Then
                MsgBox("Negalima pašalinti duomenų, kol jie yra redaguojami. Uždarykite redagavimo formą.", _
                    MsgBoxStyle.Exclamation, "Klaida")
                frm.Activate()
                Exit Sub
            End If
        Next

        If Not YesOrNo("Ar tikrai norite pašalinti pasirinkto darbo sutarties pakeitimo " & _
                "duomenis iš duomenų bazės?") Then Exit Sub

        Using busy As New StatusBusy
            Try
                ContractUpdate.DeleteContractUpdate(item.ID)
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try
        End Using

        If Not YesOrNo("Darbo sutarties pakeitimo duomenys sėkmingai pašalinti iš duomenų bazės. Atnaujinti sąrašą?") Then Exit Sub

        DoRefresh(Obj.IsOriginalContractData, Obj.OnlyInOperationAtDate, Obj.AtDate, Obj.AddAllWorkers)

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


    Private Sub DoRefresh(ByVal isOriginalData As Boolean, ByVal onlyOperationalAtDate As Boolean, _
        ByVal atDate As Date, ByVal addAllWorkers As Boolean)

        Using bm As New BindingsManager(ContractInfoListBindingSource, _
            UpdatesListBindingSource, Nothing, False, True)

            Try
                Obj = LoadObject(Of ContractInfoList)(Nothing, "GetContractInfoList", True, _
                    isOriginalData, onlyOperationalAtDate, atDate, addAllWorkers)
            Catch ex As Exception
                ShowError(ex)
                Exit Sub
            End Try

            bm.SetNewDataSource(Obj)

        End Using

        ContractInfoListDataGridView.Select()

    End Sub

End Class