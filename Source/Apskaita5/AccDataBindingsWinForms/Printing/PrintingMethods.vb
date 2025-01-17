Imports AccControlsWinForms
Imports AccControlsWinForms.Printing
Imports AccDataAccessLayer
Imports ApskaitaObjects.ActiveReports
Imports ApskaitaObjects
Imports AccCommon
Imports System.Windows.Forms

Namespace Printing

    Public Module PrintingMethods

        Public Const RDLC_FOLDER As String = "Reports"


        ''' <summary>
        ''' Prints a business object in an associated report form to a printer.
        ''' </summary>
        ''' <param name="obj">a business object to print</param>
        ''' <param name="showPreview">whether to show a print preview form</param>
        ''' <param name="version">a version of the report to use (if there are 
        ''' multiple report forms for the business object type)</param>
        ''' <param name="exportFileName">a default file name to use when exporting
        ''' the report from a ReportViewer (to pdf, xls, etc.)</param>
        ''' <param name="parentForm">a form that initiates printing (if any)</param>
        ''' <param name="filterDescription">a description of filters that are
        ''' applied in DataListView controls (see class DataListViewEditControlManager 
        ''' method GetCurrentFilterDescription)</param>
        ''' <param name="displayIndexes">indexes of the base or child collections
        ''' in the order that the items are displyed in DataListView controls (if any)
        ''' (see class DataListViewEditControlManager method GetDisplayOrderIndexes)</param>
        ''' <remarks></remarks>
        Public Sub PrintObject(ByVal obj As Object, ByVal showPreview As Boolean, _
            ByVal version As Integer, ByVal exportFileName As String, _
            ByVal parentForm As Form, ByVal filterDescription As String, _
            ByVal ParamArray displayIndexes As List(Of Integer)())

            If obj Is Nothing Then
                Throw New ArgumentNullException("Klaida. Nenurodytas spausdintinas objektas.")
            End If

            Dim reportFileName As String = ""
            Dim numberOfTablesInUse As Integer = 0
            Dim datasource As ReportData = Nothing

            Try

                Dim reportFileStream As Byte() = GetFormFromFileStream(obj, (version = 1))

                Dim mdiParent As Form = Nothing
                If Not parentForm Is Nothing Then
                    mdiParent = parentForm.MdiParent
                Else
                    mdiParent = CurrentMdiParent
                End If

                datasource = MapObjToReport(obj, reportFileName, numberOfTablesInUse, _
                    version, filterDescription, displayIndexes)

                If Not StringIsNullOrEmpty(reportFileName) Then
                    reportFileName = IO.Path.Combine(IO.Path.Combine(AppPath(), _
                        RDLC_FOLDER), reportFileName)
                End If

                PrintReport(showPreview, mdiParent, datasource, numberOfTablesInUse, _
                    reportFileStream, reportFileName, exportFileName, "")

                If Not showPreview AndAlso Not datasource Is Nothing Then
                    Try
                        datasource.Dispose()
                    Catch e As Exception
                    End Try
                End If

            Catch ex As Exception

                If Not datasource Is Nothing Then
                    Try
                        datasource.Dispose()
                    Catch e As Exception
                    End Try
                End If

                ShowError(ex, obj)

            End Try

        End Sub

        ''' <summary>
        ''' Prints all the business objects in the list to a printer.
        ''' </summary>
        ''' <typeparam name="T">a type of the business objects in the list</typeparam>
        ''' <param name="list">a list of business objects</param>
        ''' <param name="version">a version of the report to use (if there are 
        ''' multiple report forms for the business object type)</param>
        ''' <remarks></remarks>
        Public Sub PrintObjectList(Of T)(ByVal list As BusinessObjectCollection(Of T), _
            ByVal version As Integer)

            If list Is Nothing OrElse list.Result.Count < 1 Then
                Throw New ArgumentNullException("Klaida. Nerastas spausdintinas objektas.")
            End If

            Dim printerName As String = ""

            Using dlgPrint As New PrintDialog

                If dlgPrint.ShowDialog() <> DialogResult.OK Then Exit Sub

                printerName = dlgPrint.PrinterSettings.PrinterName

            End Using

            ' list item index accesor is not accessible for some reason
            Dim reportFileStream As Byte() = Nothing
            For Each item As T In list.Result
                reportFileStream = GetFormFromFileStream(item, (version = 1))
                Exit For
            Next

            Dim reportFileName As String = ""
            Dim numberOfTablesInUse As Integer = 0

            For Each item As T In list.Result

                Using datasource As ReportData = MapObjToReport(item, reportFileName, _
                    numberOfTablesInUse, version, "", Nothing)

                    If Not StringIsNullOrEmpty(reportFileName) Then
                        reportFileName = IO.Path.Combine(IO.Path.Combine(AppPath(), _
                            RDLC_FOLDER), reportFileName)
                    End If

                    PrintReport(False, Nothing, datasource, numberOfTablesInUse, _
                        reportFileStream, reportFileName, "", printerName)

                End Using

            Next

        End Sub

        ''' <summary>
        ''' Prints a business object in an associated report form to a temp pdf file
        ''' and sends it by email
        ''' </summary>
        ''' <param name="obj">a business object to print and send</param>
        ''' <param name="emailAddress">an email address of the recipient</param>
        ''' <param name="emailSubject">an email subject text</param>
        ''' <param name="emailMessageBody">an email message (body) text</param>
        ''' <param name="version">a version of the report to use (if there are 
        ''' multiple report forms for the business object type)</param>
        ''' <param name="fileName">a name of the pdf file to create and send</param>
        ''' <param name="filterDescription">a description of filters that are
        ''' applied in DataListView controls (see class DataListViewEditControlManager 
        ''' method GetCurrentFilterDescription)</param>
        ''' <param name="displayIndexes">indexes of the base or child collections
        ''' in the order that the items are displyed in DataListView controls (if any)
        ''' (see class DataListViewEditControlManager method GetDisplayOrderIndexes)</param>
        ''' <remarks></remarks>
        Public Sub SendObjectToEmail(ByVal obj As Object, ByVal emailAddress As String, _
            ByVal emailSubject As String, ByVal emailMessageBody As String, _
            ByVal version As Integer, ByVal fileName As String, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            If obj Is Nothing Then
                Throw New NullReferenceException("Klaida. Nenurodytas spausdintinas objektas.")
            End If

            Dim reportFileStream As Byte() = GetFormFromFileStream(obj, (version = 1))

            Dim filePath As String = GetEmailAttachment(obj, version, fileName, _
                reportFileStream, filterDescription, displayIndexes)

            SendEmail(emailAddress, emailSubject, emailMessageBody, New String() {filePath})

        End Sub

        ''' <summary>
        ''' Prints all the business objects in the list to temp pdf files 
        ''' and sends them in a single email message
        ''' </summary>
        ''' <typeparam name="T">a type of the business objects in the list</typeparam>
        ''' <param name="list">a list of business objects</param>
        ''' <param name="emailAddress">an email address of the recipient</param>
        ''' <param name="emailSubject">an email subject text</param>
        ''' <param name="emailMessageBody">an email message (body) text</param>
        ''' <param name="version">a version of the report to use (if there are 
        ''' multiple report forms for the business object type)</param>
        ''' <remarks></remarks>
        Public Sub SendObjectListToEmail(Of T)(ByVal list As BusinessObjectCollection(Of T), _
            ByVal emailAddress As String, ByVal emailSubject As String, ByVal emailMessageBody As String, _
            Optional ByVal version As Integer = 0)

            If list Is Nothing OrElse list.Result.Count < 1 Then
                Throw New NullReferenceException("Klaida. Nerastas spausdintinas objektas.")
            End If

            ' list item index accesor is not accessible for some reason
            Dim reportFileStream As Byte() = Nothing
            For Each item As T In list.Result
                reportFileStream = GetFormFromFileStream(item, (version = 1))
                Exit For
            Next

            Dim filePathList As New List(Of String)

            Dim counter As Integer = 0
            Dim fileName As String
            For Each item As T In list.Result

                If TypeOf item Is Documents.InvoiceMade Then
                    fileName = CType(CType(item, Object), Documents.InvoiceMade).GetFileName
                Else
                    fileName = String.Format("{0}({1})", GetType(T).Name, counter.ToString)
                End If

                filePathList.Add(GetEmailAttachment(item, version, fileName, _
                    reportFileStream, "", Nothing))

                counter += 1

            Next

            SendEmail(emailAddress, emailSubject, emailMessageBody, filePathList.ToArray)

        End Sub

        Private Function GetEmailAttachment(ByVal obj As Object, ByVal version As Integer, _
            ByVal fileName As String, ByVal reportFileStream As Byte(), _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)()) As String

            If obj Is Nothing Then
                Throw New ArgumentNullException("obj")
            End If

            Dim result As String = ""

            Try

                If TypeOf obj Is InvoiceInfo.InvoiceInfo Then

                    result = IO.Path.Combine(IO.Path.GetTempPath, fileName & ".xml")

                    IO.File.WriteAllText(result, InvoiceInfo.ToXmlString(Of InvoiceInfo.InvoiceInfo) _
                        (DirectCast(obj, InvoiceInfo.InvoiceInfo)))

                Else

                    Dim reportFileName As String = ""
                    Dim numberOfTablesInUse As Integer = 0

                    Using datasource As ReportData = MapObjToReport(obj, reportFileName, _
                        numberOfTablesInUse, version, filterDescription, displayIndexes)

                        If Not StringIsNullOrEmpty(reportFileName) Then
                            reportFileName = IO.Path.Combine(IO.Path.Combine(AppPath(), _
                                RDLC_FOLDER), reportFileName)
                        End If

                        result = PrintReportToTempPdfFile(datasource, numberOfTablesInUse, _
                            reportFileStream, reportFileName, fileName)

                    End Using

                End If

            Catch ex As Exception
                Throw New Exception(String.Format("Klaida. Nepavyko generuoti ir (ar) išsaugoti dokumento:{0}{1}", _
                    vbCrLf, ex.Message), ex)
            End Try

            Return result

        End Function

        Private Sub SendEmail(ByVal emailAddress As String, ByVal emailSubject As String, _
            ByVal emailMessageBody As String, ByVal filePath As String())

            If filePath Is Nothing OrElse filePath.Length < 1 Then
                Throw New ArgumentNullException("filePath")
            End If

            If MyCustomSettings.UseDefaultEmailClient Then

                Dim mapi As New MAPI

                If Not StringIsNullOrEmpty(emailAddress) Then
                    mapi.AddRecipientTo(emailAddress)
                End If

                For Each path As String In filePath
                    mapi.AddAttachment(path)
                Next

                If MyCustomSettings.ShowDefaultMailClientWindow Then
                    mapi.SendMailPopup(emailSubject, emailMessageBody)
                Else
                    mapi.SendMailDirect(emailSubject, emailMessageBody)
                End If

                If Not mapi.GetLastError.Trim.ToLower.StartsWith("ok") Then

                    Try
                        For Each path As String In filePath
                            IO.File.Delete(path)
                        Next
                    Catch ex As Exception
                    End Try

                    Throw New Exception(String.Format("Klaida atidarant e-pašto klientą: {0}", _
                        mapi.GetLastError))

                End If

            Else

                If StringIsNullOrEmpty(emailAddress) Then
                    Throw New Exception("Klaida. Nenurodytas e-pašto adresas.")
                End If

                Dim msg As New Net.Mail.MailMessage(MyCustomSettings.UserEmail, emailAddress, _
                    emailSubject, emailMessageBody)

                For Each path As String In filePath
                    Dim attachedFile As New Net.Mail.Attachment(path)
                    msg.Attachments.Add(attachedFile)
                Next

                Dim client As New Net.Mail.SmtpClient(MyCustomSettings.SmtpServer)

                If Not StringIsNullOrEmpty(MyCustomSettings.SmtpPort) Then
                    client.Port = Integer.Parse(MyCustomSettings.SmtpPort.Trim)
                Else
                    client.Port = 27
                End If

                client.EnableSsl = MyCustomSettings.UseSslForEmail

                If MyCustomSettings.UseAuthForEmail Then
                    client.UseDefaultCredentials = False
                    client.Credentials = New Net.NetworkCredential( _
                        MyCustomSettings.UserEmailAccount, _
                        MyCustomSettings.UserEmailPassword)
                Else
                    client.Credentials = Net.CredentialCache.DefaultNetworkCredentials
                End If

                client.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
                client.Timeout = Integer.MaxValue

                client.Send(msg)

                Try
                    For Each path As String In filePath
                        IO.File.Delete(path)
                    Next
                Catch ex As Exception
                End Try

            End If

        End Sub


        Private Function GetFormFromFileStream(ByVal obj As Object, _
            ByVal forceLithuanianRegion As Boolean) As Byte()

            If obj Is Nothing Then
                Throw New ArgumentNullException("obj")
            End If

            If Not TypeOf obj Is Documents.InvoiceMade AndAlso
                Not TypeOf obj Is Documents.ProformaInvoiceMade Then Return Nothing

            Dim cCompany As HelperLists.CompanyRegionalInfo = GetRegionalData( _
                GetLanguageCodeByObject(obj, forceLithuanianRegion))

            If TypeOf obj Is Documents.InvoiceMade Then
                If Not cCompany.InvoiceForm Is Nothing AndAlso cCompany.InvoiceForm.Length > 50 Then
                    Return cCompany.InvoiceForm
                End If
            ElseIf TypeOf obj Is Documents.ProformaInvoiceMade Then
                If Not cCompany.ProformaInvoiceForm Is Nothing AndAlso cCompany.ProformaInvoiceForm.Length > 50 Then
                    Return cCompany.ProformaInvoiceForm
                End If
            End If

            Return Nothing

        End Function

        Private Function GetLanguageCodeByObject(ByVal obj As Object, _
            ByVal forceLithuanianRegion As Boolean) As String

            Dim result As String = LanguageCodeLith.Trim.ToUpper

            If obj Is Nothing OrElse forceLithuanianRegion Then Return result

            If TypeOf obj Is Documents.InvoiceMade _
               AndAlso Not DirectCast(obj, Documents.InvoiceMade).IsDoomyInvoice _
               AndAlso Not StringIsNullOrEmpty(DirectCast(obj, Documents.InvoiceMade).LanguageCode) _
               AndAlso DirectCast(obj, Documents.InvoiceMade).LanguageCode.Trim.ToUpper <> result Then

                result = DirectCast(obj, Documents.InvoiceMade).LanguageCode.Trim.ToUpper

            ElseIf TypeOf obj Is Documents.ProformaInvoiceMade _
                AndAlso Not DirectCast(obj, Documents.ProformaInvoiceMade).IsDoomyInvoice _
                AndAlso Not StringIsNullOrEmpty(DirectCast(obj, Documents.ProformaInvoiceMade).LanguageCode) _
                AndAlso DirectCast(obj, Documents.ProformaInvoiceMade).LanguageCode.Trim.ToUpper <> result Then

                result = DirectCast(obj, Documents.ProformaInvoiceMade).LanguageCode.Trim.ToUpper

            End If

            Return result

        End Function

        Friend Function GetRegionalData(ByVal languageCode As String, _
            Optional ByVal throwOnNotFound As Boolean = True) As HelperLists.CompanyRegionalInfo

            If StringIsNullOrEmpty(languageCode) OrElse Not IsLanguageCodeValid(languageCode) Then
                languageCode = LanguageCodeLith.Trim.ToUpper
            End If

            Dim result As HelperLists.CompanyRegionalInfo

            Try

                Using busy As New StatusBusy
                    result = HelperLists.CompanyRegionalInfoList.GetList. _
                        GetItemByLanguageCode(languageCode)
                End Using

            Catch ex As Exception
                Throw New Exception(String.Format("Klaida. Nepavyko gauti išsamių įmonės duomenų (regioninių):{0}{1}", _
                    vbCrLf, ex.Message), ex)
            End Try

            If result Is Nothing AndAlso throwOnNotFound Then
                Throw New Exception(String.Format("Klaida. Nėra įvesta įmonės duomenų kalbai ""{0}"".", _
                    GetLanguageName(languageCode, False)))
            End If

            Return result

        End Function


        Friend Function MapObjToReport(ByVal obj As Object, ByRef rdlcFileName As String, _
            ByRef numberOfTablesInUse As Integer, ByVal version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)()) As ReportData

            If obj Is Nothing Then
                Throw New ArgumentNullException("obj", "Klaida. Metode MapObjToReport nenurodytas dokumentas.")
            End If

            Dim addSignatureAndLogo As Boolean = (TypeOf obj Is Documents.InvoiceMade _
                OrElse TypeOf obj Is Documents.TillIncomeOrder _
                OrElse TypeOf obj Is Documents.TillSpendingsOrder _
                OrElse TypeOf obj Is Documents.ProformaInvoiceMade)

            Dim forceLithuanianRegion As Boolean = (version = 1)

            Dim result As ReportData = GetDataSetForReport( _
                GetLanguageCodeByObject(obj, forceLithuanianRegion), addSignatureAndLogo, _
                forceLithuanianRegion, True)

            If result Is Nothing Then
                Throw New Exception("Klaida. Nepavyko gauti įmonės duomenų, reikalingų ataskaitos generavimui.")
            End If

            If TypeOf obj Is General.JournalEntry Then
                MapObjectDetailsToReport(result, DirectCast(obj, General.JournalEntry), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.BookEntryInfoListParent Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.BookEntryInfoListParent), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.PersonInfoItemList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.PersonInfoItemList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is General.AccountList Then
                MapObjectDetailsToReport(result, DirectCast(obj, General.AccountList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.JournalEntryInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.JournalEntryInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Documents.BankOperationItemList Then
                MapObjectDetailsToReport(result, DirectCast(obj, Documents.BankOperationItemList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Documents.BankOperation Then
                MapObjectDetailsToReport(result, DirectCast(obj, Documents.BankOperation), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.Declaration Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.Declaration), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.ImprestSheetInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.ImprestSheetInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Workers.ImprestSheet Then
                MapObjectDetailsToReport(result, DirectCast(obj, Workers.ImprestSheet), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.PayOutNaturalPersonInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.PayOutNaturalPersonInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.WageSheetInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.WageSheetInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.WorkerWageInfoReport Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.WorkerWageInfoReport), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Workers.WageSheet Then
                MapObjectDetailsToReport(result, DirectCast(obj, Workers.WageSheet), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is LongTermAssetInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, LongTermAssetInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.LongTermAssetOperationInfoListParent Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.LongTermAssetOperationInfoListParent), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.AdvanceReportInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.AdvanceReportInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.CashOperationInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.CashOperationInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.InvoiceInfoItemList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.InvoiceInfoItemList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Documents.AdvanceReport Then
                MapObjectDetailsToReport(result, DirectCast(obj, Documents.AdvanceReport), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Documents.TillIncomeOrder Then
                MapObjectDetailsToReport(result, DirectCast(obj, Documents.TillIncomeOrder), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Documents.TillSpendingsOrder Then
                MapObjectDetailsToReport(result, DirectCast(obj, Documents.TillSpendingsOrder), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.FinancialStatementsInfo Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.FinancialStatementsInfo), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.DebtInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.DebtInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Documents.InvoiceMade Then
                MapObjectDetailsToReport(result, DirectCast(obj, Documents.InvoiceMade), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Documents.ProformaInvoiceMade Then
                MapObjectDetailsToReport(result, DirectCast(obj, Documents.ProformaInvoiceMade), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.WorkTimeSheetInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.WorkTimeSheetInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Workers.WorkTimeSheet Then
                MapObjectDetailsToReport(result, DirectCast(obj, Workers.WorkTimeSheet), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Workers.Contract Then
                MapObjectDetailsToReport(result, DirectCast(obj, Workers.Contract), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Workers.ContractUpdate Then
                MapObjectDetailsToReport(result, DirectCast(obj, Workers.ContractUpdate), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsComplexOperationDiscard Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsComplexOperationDiscard), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsComplexOperationInternalTransfer Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsComplexOperationInternalTransfer), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsComplexOperationInventorization Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsComplexOperationInventorization), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsComplexOperationPriceCut Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsComplexOperationPriceCut), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsComplexOperationProduction Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsComplexOperationProduction), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsOperationAccountChange Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsOperationAccountChange), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsOperationAcquisition Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsOperationAcquisition), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsOperationAdditionalCosts Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsOperationAdditionalCosts), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsOperationDiscard Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsOperationDiscard), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsOperationDiscount Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsOperationDiscount), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsOperationPriceCut Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsOperationPriceCut), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.GoodsOperationTransfer Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.GoodsOperationTransfer), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Goods.ProductionCalculation Then
                MapObjectDetailsToReport(result, DirectCast(obj, Goods.ProductionCalculation), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.GoodsOperationInfoListParent Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.GoodsOperationInfoListParent), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.GoodsTurnoverInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.GoodsTurnoverInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.OperationAccountChange Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.OperationAccountChange), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.OperationAcquisitionValueIncrease Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.OperationAcquisitionValueIncrease), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.OperationAmortization Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.OperationAmortization), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.OperationAmortizationPeriodChange Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.OperationAmortizationPeriodChange), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.OperationDiscard Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.OperationDiscard), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.OperationOperationalStatusChange Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.OperationOperationalStatusChange), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.OperationTransfer Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.OperationTransfer), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.OperationValueChange Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.OperationValueChange), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.ComplexOperationAmortization Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.ComplexOperationAmortization), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.ComplexOperationDiscard Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.ComplexOperationDiscard), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.ComplexOperationOperationalStatusChange Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.ComplexOperationOperationalStatusChange), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Assets.ComplexOperationValueChange Then
                MapObjectDetailsToReport(result, DirectCast(obj, Assets.ComplexOperationValueChange), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.UnsettledPersonInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.UnsettledPersonInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.WorkersVDUInfo Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.WorkersVDUInfo), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.WorkerHolidayInfo Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.WorkerHolidayInfo), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is WageSheetItem Then
                MapObjectDetailsToReport(result, DirectCast(obj, WageSheetItem), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is Workers.HolidayPayReserve Then
                MapObjectDetailsToReport(result, DirectCast(obj, Workers.HolidayPayReserve), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.ServiceTurnoverInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.ServiceTurnoverInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is DebtStatementItemListPrintView Then
                MapObjectDetailsToReport(result, DirectCast(obj, DebtStatementItemListPrintView), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.SharesOperationInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.SharesOperationInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.SharesAccountEntryList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.SharesAccountEntryList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is ActiveReports.ShareHolderInfoList Then
                MapObjectDetailsToReport(result, DirectCast(obj, ActiveReports.ShareHolderInfoList), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)
            ElseIf TypeOf obj Is General.SharesOperation Then
                MapObjectDetailsToReport(result, DirectCast(obj, General.SharesOperation), rdlcFileName, numberOfTablesInUse, version, filterDescription, displayIndexes)

                'ElseIf TypeOf Obj Is Documents.Offset Then
                '    MapObjectDetailsToReport(RD, DirectCast(Obj, Documents.Offset), ReportFileName, NumberOfTablesInUse, Version)
            Else
                Throw New NotImplementedException(String.Format( _
                    "Klaida. Objektas '{0}' negali būti atvaizduotas ataskaitoje.", _
                    obj.GetType.FullName))
            End If

            Return result

        End Function

        Friend Function GetDataSetForReport(ByVal languageCode As String, _
            ByVal addSignatureAndLogo As Boolean, ByVal forceLithuanianRegion As Boolean, _
            ByVal throwOnRegionalSettingsNotFound As Boolean) As ReportData

            If addSignatureAndLogo Then
                If Not CachedInfoLists.PrepareCache(Nothing, GetType(AccDataAccessLayer.Security.UserProfile)) Then
                    Return Nothing
                End If
            End If

            Dim regionInfo As HelperLists.CompanyRegionalInfo = _
                GetRegionalData(languageCode, throwOnRegionalSettingsNotFound)
            Dim baseRegionInfo As HelperLists.CompanyRegionalInfo = _
                GetRegionalData(LanguageCodeLith.Trim.ToUpper, True)
            Dim currentCompany As ApskaitaObjects.Settings.CompanyInfo = GetCurrentCompany()

            Dim result As New ReportData

            result.TableCompany.Rows.Add()

            result.TableCompany.Item(0).Column1 = String.Format("{0} (į.k. {1})", _
                currentCompany.Name, currentCompany.Code)
            result.TableCompany.Item(0).Column2 = currentCompany.Name
            result.TableCompany.Item(0).Column3 = currentCompany.Code
            result.TableCompany.Item(0).Column4 = currentCompany.CodeVat
            result.TableCompany.Item(0).Column5 = currentCompany.Address
            result.TableCompany.Item(0).Column6 = currentCompany.CodeSODRA
            result.TableCompany.Item(0).Column7 = currentCompany.Email
            result.TableCompany.Item(0).Column8 = currentCompany.HeadPerson
            If Not regionInfo Is Nothing Then
                result.TableCompany.Item(0).Column9 = regionInfo.Address
                result.TableCompany.Item(0).Column10 = regionInfo.BankAccount
                result.TableCompany.Item(0).Column11 = regionInfo.Bank
                result.TableCompany.Item(0).Column12 = regionInfo.BankSWIFT
                result.TableCompany.Item(0).Column13 = regionInfo.BankAddress
                result.TableCompany.Item(0).Column14 = regionInfo.InvoiceInfoLine
                result.TableCompany.Item(0).Column15 = regionInfo.Contacts
                result.TableCompany.Item(0).Column16 = regionInfo.DiscountName
                result.TableCompany.Item(0).Column17 = regionInfo.HeadTitle
                result.TableCompany.Item(0).Column18 = regionInfo.MeasureUnitInvoiceMade
            End If
            result.TableCompany.Item(0).Column19 = currentCompany.BaseCurrency.Trim.ToUpper

            If MyCustomSettings.SignInvoicesWithCompanySignature Then
                result.TableCompany.Item(0).Column20 = "1"
            ElseIf MyCustomSettings.SignInvoicesWithRemoteUserSignature Then
                result.TableCompany.Item(0).Column20 = "2"
            ElseIf MyCustomSettings.SignInvoicesWithLocalUserSignature Then
                result.TableCompany.Item(0).Column20 = "3"
            Else
                result.TableCompany.Item(0).Column20 = "0"
            End If

            result.TableCompany.Item(0).Column21 = MyCustomSettings.UserName

            If addSignatureAndLogo Then
                result.TableCompany.Item(0).Column22 = AccDataAccessLayer.Security. _
                    UserProfile.GetList.Position
                result.TableCompany.Item(0).Column23 = AccDataAccessLayer.DatabaseAccess. _
                    GetCurrentIdentity.UserRealName
            Else
                result.TableCompany.Item(0).Column22 = ""
                result.TableCompany.Item(0).Column23 = ""
            End If

            result.TableCompany.Item(0).Column24 = currentCompany.Accountant
            result.TableCompany.Item(0).Column25 = currentCompany.Cashier

            result.TableGeneral.Rows.Add()

            If addSignatureAndLogo Then

                Using busy As New StatusBusy
                    Try
                        If Not regionInfo Is Nothing Then result.TableGeneral.Item(0).P_Column1 = regionInfo.LogoImage
                        If MyCustomSettings.SignInvoicesWithLocalUserSignature Then
                            result.TableGeneral.Item(0).P_Column2 = ImageToByteArray( _
                                ByteArrayToImage(Convert.FromBase64String(MyCustomSettings.UserSignature)))
                        ElseIf MyCustomSettings.SignInvoicesWithCompanySignature AndAlso addSignatureAndLogo Then
                            result.TableGeneral.Item(0).P_Column2 = ImageToByteArray( _
                                General.Company.GetCompany.HeadPersonSignature)
                        ElseIf MyCustomSettings.SignInvoicesWithRemoteUserSignature AndAlso addSignatureAndLogo Then
                            result.TableGeneral.Item(0).P_Column2 = ImageToByteArray( _
                                AccDataAccessLayer.Security.UserProfile.GetList.Signature)
                        End If
                    Catch ex As Exception
                        Throw New Exception("Klaida. Nepavyko gauti įmonės duomenų, " _
                                            & "reikalingų ataskaitos generavimui.", ex)
                    End Try
                End Using

            End If

            Return result

        End Function

        Friend Sub UpdateReportDataSetWithRegionalData(ByVal datasource As ReportData, _
            ByVal regionalData As General.CompanyRegionalData)

            If regionalData Is Nothing Then Exit Sub

            datasource.TableCompany.Item(0).Column9 = regionalData.Address
            datasource.TableCompany.Item(0).Column10 = regionalData.BankAccount
            datasource.TableCompany.Item(0).Column11 = regionalData.Bank
            datasource.TableCompany.Item(0).Column12 = regionalData.BankSWIFT
            datasource.TableCompany.Item(0).Column13 = regionalData.BankAddress
            datasource.TableCompany.Item(0).Column14 = regionalData.InvoiceInfoLine
            datasource.TableCompany.Item(0).Column15 = regionalData.Contacts
            datasource.TableCompany.Item(0).Column16 = regionalData.DiscountName
            datasource.TableCompany.Item(0).Column17 = regionalData.HeadTitle
            datasource.TableCompany.Item(0).Column18 = regionalData.MeasureUnitInvoiceMade
            datasource.TableGeneral.Item(0).P_Column1 = ImageToByteArray(regionalData.LogoImage)

        End Sub


        ''' <summary>
        ''' Gets a new DisplayedList instance for a base collection
        ''' using the display indexes provided.
        ''' </summary>
        ''' <param name="baseList">a base collection</param>
        ''' <param name="displayOrderIndexes">a display indexes provided by the
        ''' DataListViewEditControlManager.GetDisplayOrderIndexes method (if any)</param>
        ''' <remarks></remarks>
        Public Function GetDisplayedList(Of TC)(ByVal baseList As IList(Of TC), _
            ByVal displayOrderIndexes As List(Of Integer)) As DisplayedList(Of TC)
            Return New DisplayedList(Of TC)(baseList, displayOrderIndexes)
        End Function

        ''' <summary>
        ''' Gets a new DisplayedList instance for a base collection
        ''' using the display indexes provided.
        ''' </summary>
        ''' <param name="baseList">a base collection</param>
        ''' <param name="displayOrderIndexes">a display indexes provided by the
        ''' DataListViewEditControlManager.GetDisplayOrderIndexes method (if any)</param>
        ''' <param name="listIndex">an index of the reguired display index list within the array</param>
        ''' <remarks></remarks>
        Public Function GetDisplayedList(Of TC)(ByVal baseList As IList(Of TC), _
            ByVal displayOrderIndexes As List(Of Integer)(), ByVal listIndex As Integer) As DisplayedList(Of TC)
            Return New DisplayedList(Of TC)(baseList, displayOrderIndexes, listIndex)
        End Function

#Region "Typed mappers"

#Region "General"

        ''' <summary>
        ''' Map <see cref="General.JournalEntry">JournalEntry</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As General.JournalEntry, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DocNumber
            rd.TableGeneral.Item(0).Column3 = R_Obj.Content
            If R_Obj.Person Is Nothing Then
                rd.TableGeneral.Item(0).Column4 = "Nenustatyta"
            Else
                rd.TableGeneral.Item(0).Column4 = R_Obj.Person.ToString
            End If
            rd.TableGeneral.Item(0).Column5 = R_Obj.DocTypeHumanReadable
            rd.TableGeneral.Item(0).Column7 = DblParser(R_Obj.DebetSum)
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.CreditSum)
            rd.TableGeneral.Item(0).Column9 = filterDescription

            For Each item As General.BookEntry In GetDisplayedList(Of General.BookEntry) _
                (R_Obj.DebetList, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Account.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = DblParser(item.Amount)
                If item.Person Is Nothing Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = "Nenustatyta"
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Person.ToString
                End If
            Next
            For Each item As General.BookEntry In GetDisplayedList(Of General.BookEntry) _
                (R_Obj.CreditList, displayIndexes, 1)
                rd.Table2.Rows.Add()
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = item.Account.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = DblParser(item.Amount)
                If item.Person Is Nothing Then
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = "Nenustatyta"
                Else
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = item.Person.ToString
                End If
            Next

            ReportFileName = "R_JournalEntry.rdlc"
            NumberOfTablesInUse = 2

        End Sub

        ''' <summary>
        ''' Map <see cref="General.SharesOperation">SharesOperation</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData,
            ByVal R_Obj As General.SharesOperation, ByRef ReportFileName As String,
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer,
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column2 = R_Obj.DocumentDate.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentName
            rd.TableGeneral.Item(0).Column4 = R_Obj.DocumentNumber
            rd.TableGeneral.Item(0).Column5 = R_Obj.Remarks
            rd.TableGeneral.Item(0).Column6 = R_Obj.InsertDate.ToString()
            rd.TableGeneral.Item(0).Column7 = R_Obj.UpdateDate.ToString()
            rd.TableGeneral.Item(0).Column8 = filterDescription

            For Each item As General.SharesAcquisition In GetDisplayedList(Of General.SharesAcquisition) _
                (R_Obj.Acquisitions, displayIndexes, 0)

                rd.Table1.Rows.Add()
                If item.IsCompanyShares Then
                    Dim cc As ApskaitaObjects.Settings.CompanyInfo = GetCurrentCompany()
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = "Nuosavos akcijos"
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = ""
                ElseIf item.ShareHolder <> HelperLists.PersonInfo.Empty Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ShareHolder.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.ShareHolder.Code
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.ShareHolder.LanguageCode
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.ShareHolder.Address
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.ShareHolder.Email
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = ""
                End If
                If item.Class <> HelperLists.SharesClassInfo.Empty Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.Class.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.Class.ValuePerUnit)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.Class.Description
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = ""
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.Amount)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = BooleanToCheckMark(item.IsEmission)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = item.Remarks

            Next
            For Each item As General.SharesDiscard In GetDisplayedList(Of General.SharesDiscard) _
                (R_Obj.Acquisitions, displayIndexes, 0)

                rd.Table2.Rows.Add()
                If item.IsCompanyShares Then
                    Dim cc As ApskaitaObjects.Settings.CompanyInfo = GetCurrentCompany()
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = cc.Name & " (nuosavos akcijos)"
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = cc.Code
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = ApskaitaObjects.StateCodeLith
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column4 = cc.Address
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column5 = ""
                ElseIf item.ShareHolder <> HelperLists.PersonInfo.Empty Then
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = item.ShareHolder.Name
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = item.ShareHolder.Code
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = item.ShareHolder.LanguageCode
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column4 = item.ShareHolder.Address
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column5 = item.ShareHolder.Email
                Else
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = ""
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = ""
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = ""
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column4 = ""
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column5 = ""
                End If
                If item.Class <> HelperLists.SharesClassInfo.Empty Then
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column6 = item.Class.Name
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column7 = DblParser(item.Class.ValuePerUnit)
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column8 = item.Class.Description
                Else
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column6 = ""
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column7 = ""
                    rd.Table2.Item(rd.Table2.Rows.Count - 1).Column8 = ""
                End If
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column9 = DblParser(item.Amount)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column10 = BooleanToCheckMark(item.IsCancellation)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column11 = item.Remarks

            Next

            ReportFileName = "R_SharesOperation.rdlc"
            NumberOfTablesInUse = 2

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.ShareHolderInfoList">ShareHolderInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData,
            ByVal R_Obj As ActiveReports.ShareHolderInfoList, ByRef ReportFileName As String,
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer,
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.AsOfDate.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column2 = filterDescription

            For Each item As ActiveReports.ShareHolderInfo In GetDisplayedList(Of ActiveReports.ShareHolderInfo) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Code
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.StateCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Address
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Email
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.Class
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.Description
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.ValuePerUnit)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.Amount)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.ValueTotal)
            Next

            ReportFileName = "R_ShareHolderInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.SharesOperationInfoList">SharesOperationInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData,
            ByVal R_Obj As ActiveReports.SharesOperationInfoList, ByRef ReportFileName As String,
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer,
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateBegin.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateEnd.ToString("yyyy-MM-dd")
            If R_Obj.CompanyShares Then
                rd.TableGeneral.Item(0).Column3 = "Nuosavos akcijos"
                rd.TableGeneral.Item(0).Column4 = ""
                rd.TableGeneral.Item(0).Column5 = ""
                rd.TableGeneral.Item(0).Column6 = ""
                rd.TableGeneral.Item(0).Column7 = ""
            ElseIf R_Obj.ShareHolder <> HelperLists.PersonInfo.Empty Then
                rd.TableGeneral.Item(0).Column3 = R_Obj.ShareHolder.Name
                rd.TableGeneral.Item(0).Column4 = R_Obj.ShareHolder.Code
                rd.TableGeneral.Item(0).Column5 = R_Obj.ShareHolder.LanguageCode
                rd.TableGeneral.Item(0).Column6 = R_Obj.ShareHolder.Address
                rd.TableGeneral.Item(0).Column7 = R_Obj.ShareHolder.Email
            Else
                rd.TableGeneral.Item(0).Column3 = ""
                rd.TableGeneral.Item(0).Column4 = ""
                rd.TableGeneral.Item(0).Column5 = ""
                rd.TableGeneral.Item(0).Column6 = ""
                rd.TableGeneral.Item(0).Column7 = ""
            End If
            If R_Obj.Class <> HelperLists.SharesClassInfo.Empty Then
                rd.TableGeneral.Item(0).Column8 = R_Obj.Class.Name
                rd.TableGeneral.Item(0).Column9 = DblParser(R_Obj.Class.ValuePerUnit)
                rd.TableGeneral.Item(0).Column10 = R_Obj.Class.Description
            Else
                rd.TableGeneral.Item(0).Column8 = ""
                rd.TableGeneral.Item(0).Column9 = ""
                rd.TableGeneral.Item(0).Column10 = ""
            End If
            rd.TableGeneral.Item(0).Column11 = filterDescription

            For Each item As ActiveReports.SharesOperationInfo In GetDisplayedList(Of ActiveReports.SharesOperationInfo) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Date.ToString("yyyy-MM-dd")
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.DocumentDate.ToString("yyyy-MM-dd")
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.DocumentName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.DocumentNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Remarks
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.InsertDate.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.UpdateDate.ToString
            Next

            ReportFileName = "R_SharesOperationInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.SharesAccountEntryList">SharesAccountEntryList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData,
            ByVal R_Obj As ActiveReports.SharesAccountEntryList, ByRef ReportFileName As String,
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer,
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.AsOfDate.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column2 = R_Obj.AccountNo
            If R_Obj.ShareHolder <> HelperLists.PersonInfo.Empty Then
                rd.TableGeneral.Item(0).Column3 = R_Obj.ShareHolder.Name
                rd.TableGeneral.Item(0).Column4 = R_Obj.ShareHolder.Code
                rd.TableGeneral.Item(0).Column5 = R_Obj.ShareHolder.LanguageCode
                rd.TableGeneral.Item(0).Column6 = R_Obj.ShareHolder.Address
                rd.TableGeneral.Item(0).Column7 = R_Obj.ShareHolder.Email
            Else
                rd.TableGeneral.Item(0).Column3 = "Nuosavos akcijos"
                rd.TableGeneral.Item(0).Column4 = ""
                rd.TableGeneral.Item(0).Column5 = ""
                rd.TableGeneral.Item(0).Column6 = ""
                rd.TableGeneral.Item(0).Column7 = ""
            End If
            If R_Obj.Class <> HelperLists.SharesClassInfo.Empty Then
                rd.TableGeneral.Item(0).Column8 = R_Obj.Class.Name
                rd.TableGeneral.Item(0).Column9 = DblParser(R_Obj.Class.ValuePerUnit)
                rd.TableGeneral.Item(0).Column10 = R_Obj.Class.Description
            Else
                rd.TableGeneral.Item(0).Column8 = ""
                rd.TableGeneral.Item(0).Column9 = ""
                rd.TableGeneral.Item(0).Column10 = ""
            End If
            rd.TableGeneral.Item(0).Column11 = filterDescription

            For Each item As ActiveReports.SharesAccountEntry In GetDisplayedList(Of ActiveReports.SharesAccountEntry) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Date.ToString("yyyy-MM-dd")
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Document
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.CorrespondingAccounts
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.AmountAcquired
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.AmountDiscarded
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.AmountAfterOperation
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.Remarks
            Next

            If Version = 0 Then
                ReportFileName = "R_SharesAccountEntryList.rdlc"
            ElseIf Version = 1 Then
                ReportFileName = "R_SharesAccountEntryListExtract.rdlc"
            Else
                Throw New NotImplementedException("Klaida. Akcijų vertybinių popierių sąskaitos " &
                    "spausdinamos formos 3 versijos dar nenupiešė barsukas.")
            End If
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.BookEntryInfoListParent">BookEntryInfoListParent</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.BookEntryInfoListParent, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Account.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column3 = R_Obj.DateTo.ToShortDateString
            If R_Obj.IncludeSubAccounts Then
                rd.TableGeneral.Item(0).Column4 = "X"
            Else
                rd.TableGeneral.Item(0).Column4 = ""
            End If
            If R_Obj.Group IsNot Nothing AndAlso R_Obj.Group.ID > 0 Then
                rd.TableGeneral.Item(0).Column5 = R_Obj.Group.Name
                rd.TableGeneral.Item(0).Column6 = "Netaikoma"
            Else
                rd.TableGeneral.Item(0).Column5 = "Netaikoma"
                If R_Obj.Person IsNot Nothing AndAlso R_Obj.Person.ID > 0 Then
                    rd.TableGeneral.Item(0).Column6 = R_Obj.Person.ToString
                Else
                    rd.TableGeneral.Item(0).Column6 = "Netaikoma"
                End If
            End If
            rd.TableGeneral.Item(0).Column7 = DblParser(R_Obj.DebetBalanceStart)
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.CreditBalanceStart)
            rd.TableGeneral.Item(0).Column9 = DblParser(R_Obj.DebetTurnover)
            rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.CreditTurnover)
            rd.TableGeneral.Item(0).Column11 = DblParser(R_Obj.DebetBalanceEnd)
            rd.TableGeneral.Item(0).Column12 = DblParser(R_Obj.CreditBalanceEnd)

            Dim accountsInfo As HelperLists.AccountInfoList = Nothing
            Try
                accountsInfo = HelperLists.AccountInfoList.GetList
            Catch ex As Exception
            End Try
            If Not accountsInfo Is Nothing Then
                rd.TableGeneral.Item(0).Column13 = accountsInfo.GetAccountByID(R_Obj.Account).Name
            Else
                rd.TableGeneral.Item(0).Column13 = ""
            End If

            rd.TableGeneral.Item(0).Column14 = filterDescription

            For Each item As ActiveReports.BookEntryInfo In GetDisplayedList(Of ActiveReports.BookEntryInfo) _
                (R_Obj.Items, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.JournalEntryDate.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.DocNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Content
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = DblParser(item.DebetTurnover)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.CreditTurnover)
                If Not item.PersonID > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = "Nenustatyta"
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.Person
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.BookEntriesString
            Next

            ReportFileName = "R_BookEntryInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.PersonInfoItemList">PersonInfoItemList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.PersonInfoItemList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.LikeString
            rd.TableGeneral.Item(0).Column2 = BooleanToCheckMark(R_Obj.ShowClients)
            rd.TableGeneral.Item(0).Column3 = BooleanToCheckMark(R_Obj.ShowSuppliers)
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(R_Obj.ShowWorkers)
            rd.TableGeneral.Item(0).Column5 = filterDescription

            For Each item As ActiveReports.PersonInfoItem In GetDisplayedList(Of ActiveReports.PersonInfoItem) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Code
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Address
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.CodeVAT
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.CodeSODRA
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.BankAccount
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.Bank
                ' RD.Table1.Item(RD.Table1.Rows.Count - 1).Column8 = item.AssignedToGroups
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.AccountAgainstBankBuyer.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.AccountAgainstBankSupplyer.ToString
            Next

            ReportFileName = "R_Persons.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="General.AccountList">AccountList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As General.AccountList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = filterDescription

            For Each item As General.Account In GetDisplayedList(Of General.Account) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Name
                If Not item.AssociatedReportItem Is Nothing Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.AssociatedReportItem.ToString()
                End If
            Next

            ReportFileName = "R_AccountList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.JournalEntryInfoList">JournalEntryInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.JournalEntryInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            If R_Obj.ApplyDocTypeFilter Then
                rd.TableGeneral.Item(0).Column3 = ApskaitaObjects.Utilities.ConvertLocalizedName(R_Obj.DocTypeFilter)
            Else
                rd.TableGeneral.Item(0).Column3 = "Netaikyta"
            End If
            rd.TableGeneral.Item(0).Column4 = R_Obj.ContentFilter

            If R_Obj.PersonGroupFilter > 0 Then
                rd.TableGeneral.Item(0).Column5 = R_Obj.PersonGroupName
                rd.TableGeneral.Item(0).Column6 = "Netaikyta"
            ElseIf R_Obj.PersonFilter > 0 Then
                rd.TableGeneral.Item(0).Column5 = "Netaikyta"
                rd.TableGeneral.Item(0).Column6 = R_Obj.PersonName
            Else
                rd.TableGeneral.Item(0).Column5 = "Netaikyta"
                rd.TableGeneral.Item(0).Column6 = "Netaikyta"
            End If

            rd.TableGeneral.Item(0).Column7 = filterDescription

            For Each item As ActiveReports.JournalEntryInfo In GetDisplayedList(Of ActiveReports.JournalEntryInfo) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.DocNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Content
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Person
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.Ammount)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.BookEntries
            Next

            ReportFileName = "R_GeneralLedger.rdlc"
            NumberOfTablesInUse = 1

        End Sub

#End Region

#Region "Documents"

        ''' <summary>
        ''' Map <see cref="Documents.BankOperationItemList">BankOperationItemList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Documents.BankOperationItemList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            'RD.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            'RD.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            rd.TableGeneral.Item(0).Column3 = R_Obj.SourceType
            rd.TableGeneral.Item(0).Column4 = R_Obj.Account.ToString
            'If R_Obj.CurrentFilterState Then
            '    RD.TableGeneral.Item(0).Column5 = "X"
            'Else
            '    RD.TableGeneral.Item(0).Column5 = ""
            'End If
            'If R_Obj.BalanceStart >= 0 Then
            '    RD.TableGeneral.Item(0).Column6 = D_Parser(R_Obj.BalanceStart)
            '    RD.TableGeneral.Item(0).Column7 = ""
            'Else
            '    RD.TableGeneral.Item(0).Column6 = ""
            '    RD.TableGeneral.Item(0).Column7 = D_Parser(-R_Obj.BalanceStart)
            'End If
            'RD.TableGeneral.Item(0).Column8 = D_Parser(R_Obj.TotalIncome)
            'RD.TableGeneral.Item(0).Column9 = D_Parser(R_Obj.TotalSpendings)
            'If R_Obj.BalanceEnd >= 0 Then
            '    RD.TableGeneral.Item(0).Column10 = D_Parser(R_Obj.BalanceEnd)
            '    RD.TableGeneral.Item(0).Column11 = ""
            'Else
            '    RD.TableGeneral.Item(0).Column10 = ""
            '    RD.TableGeneral.Item(0).Column11 = D_Parser(-R_Obj.BalanceEnd)
            'End If

            rd.TableGeneral.Item(0).Column12 = filterDescription

            Dim SL As Csla.SortedBindingList(Of Documents.BankOperationItem) = R_Obj.GetSortedList()

            For Each item As Documents.BankOperationItem In GetDisplayedList(Of Documents.BankOperationItem) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.DocumentNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.UniqueCode
                If item.Person IsNot Nothing Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Person.Code
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Person.Name
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = ""
                End If
                If item.Inflow Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.OriginalSum)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = ""
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.OriginalSum)
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.AccountCorresponding.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.Content
            Next

            ReportFileName = "R_BankTransferList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Documents.BankOperation">BankOperation</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Documents.BankOperation, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DocumentNumber
            If Not R_Obj.Account Is Nothing AndAlso R_Obj.Account.ID > 0 Then
                rd.TableGeneral.Item(0).Column3 = ConvertLocalizedName(R_Obj.Account.Type)
                rd.TableGeneral.Item(0).Column4 = R_Obj.Account.Account.ToString
                rd.TableGeneral.Item(0).Column5 = R_Obj.Account.Name
                rd.TableGeneral.Item(0).Column6 = R_Obj.Account.CurrencyCode
                rd.TableGeneral.Item(0).Column7 = R_Obj.Account.BankName
                rd.TableGeneral.Item(0).Column8 = R_Obj.Account.BankAccountNumber
            End If
            If Not R_Obj.Person Is Nothing AndAlso R_Obj.Person.ID > 0 Then
                rd.TableGeneral.Item(0).Column9 = R_Obj.Person.Name
                rd.TableGeneral.Item(0).Column10 = R_Obj.Person.Code
            End If
            rd.TableGeneral.Item(0).Column11 = R_Obj.Content
            rd.TableGeneral.Item(0).Column12 = R_Obj.CurrencyCode
            rd.TableGeneral.Item(0).Column13 = DblParser(R_Obj.CurrencyRate, 6)
            rd.TableGeneral.Item(0).Column14 = DblParser(R_Obj.Sum)
            rd.TableGeneral.Item(0).Column15 = R_Obj.AccountCurrencyRateChangeImpact.ToString
            rd.TableGeneral.Item(0).Column16 = DblParser(R_Obj.CurrencyRateChangeImpact)
            rd.TableGeneral.Item(0).Column17 = DblParser(R_Obj.SumLTL)
            rd.TableGeneral.Item(0).Column18 = DblParser(R_Obj.CurrencyRateInAccount, 6)
            rd.TableGeneral.Item(0).Column19 = DblParser(R_Obj.SumInAccount)
            rd.TableGeneral.Item(0).Column20 = R_Obj.UniqueCode
            If R_Obj.IsDebit Then
                rd.TableGeneral.Item(0).Column21 = "Debetas"
            Else
                rd.TableGeneral.Item(0).Column21 = "Kreditas"
            End If
            rd.TableGeneral.Item(0).Column22 = filterDescription

            For Each item As General.BookEntry In GetDisplayedList(Of General.BookEntry) _
                (R_Obj.BookEntryItems, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Account.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = DblParser(item.Amount)
                If Not item.Person Is Nothing AndAlso item.Person.ID > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Person.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Person.Code
                End If
            Next

            ReportFileName = "R_BankTransfer.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.AdvanceReportInfoList">AdvanceReportInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.AdvanceReportInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            If Not R_Obj.Person Is Nothing AndAlso R_Obj.Person.ID > 0 Then
                rd.TableGeneral.Item(0).Column3 = R_Obj.Person.Name
                rd.TableGeneral.Item(0).Column4 = R_Obj.Person.Code
            End If
            rd.TableGeneral.Item(0).Column7 = filterDescription

            Dim SumIncome As Double = 0
            Dim SumExpenses As Double = 0

            For Each item As ActiveReports.AdvanceReportInfo In GetDisplayedList(Of ActiveReports.AdvanceReportInfo) _
                (R_Obj, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.DocumentNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.PersonName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.PersonCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Account.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.Content
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.ExpensesSum)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.ExpensesSumVat)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.ExpensesSumTotal)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.IncomeSum)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.IncomeSumVat)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.IncomeSumTotal)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = item.CurrencyCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.CurrencyRate, 6)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.ExpensesSumLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.ExpensesSumVatLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.ExpensesSumTotalLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.IncomeSumLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.IncomeSumVatLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.IncomeSumTotalLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = item.Comments
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = item.CommentsInternal
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = item.TillOrderData

                SumIncome += item.IncomeSumTotalLTL
                SumExpenses += item.ExpensesSumTotalLTL

            Next

            rd.TableGeneral.Item(0).Column5 = DblParser(SumExpenses)
            rd.TableGeneral.Item(0).Column6 = DblParser(SumIncome)

            ReportFileName = "R_AdvanceReportList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.CashOperationInfoList">CashOperationInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.CashOperationInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            If Not R_Obj.Person Is Nothing AndAlso R_Obj.Person.ID > 0 Then
                rd.TableGeneral.Item(0).Column3 = R_Obj.Person.Name
                rd.TableGeneral.Item(0).Column4 = R_Obj.Person.Code
            End If
            If Not R_Obj.Account Is Nothing AndAlso R_Obj.Account.ID > 0 Then
                rd.TableGeneral.Item(0).Column5 = ApskaitaObjects.Utilities.ConvertLocalizedName(R_Obj.Account.Type)
                rd.TableGeneral.Item(0).Column6 = R_Obj.Account.Account.ToString
                rd.TableGeneral.Item(0).Column7 = R_Obj.Account.Name
                rd.TableGeneral.Item(0).Column8 = R_Obj.Account.CurrencyCode
                rd.TableGeneral.Item(0).Column9 = R_Obj.Account.BankName
                rd.TableGeneral.Item(0).Column10 = R_Obj.Account.BankAccountNumber
            End If

            rd.TableGeneral.Item(0).Column11 = DblParser(R_Obj.BalanceStart)
            rd.TableGeneral.Item(0).Column12 = DblParser(R_Obj.BalanceBookEntryStart)
            rd.TableGeneral.Item(0).Column13 = DblParser(R_Obj.BalanceLTLStart)
            rd.TableGeneral.Item(0).Column14 = DblParser(R_Obj.TurnoverDebit)
            rd.TableGeneral.Item(0).Column15 = DblParser(R_Obj.TurnoverCredit)
            rd.TableGeneral.Item(0).Column16 = DblParser(R_Obj.TurnoverBookEntryDebit)
            rd.TableGeneral.Item(0).Column17 = DblParser(R_Obj.TurnoverBookEntryCredit)
            rd.TableGeneral.Item(0).Column18 = DblParser(R_Obj.TurnoverLTLDebit)
            rd.TableGeneral.Item(0).Column19 = DblParser(R_Obj.TurnoverLTLCredit)
            rd.TableGeneral.Item(0).Column20 = DblParser(R_Obj.TurnOverInListLTLDebit)
            rd.TableGeneral.Item(0).Column21 = DblParser(R_Obj.TurnoverInListLTLCredit)
            rd.TableGeneral.Item(0).Column22 = DblParser(R_Obj.BalanceEnd)
            rd.TableGeneral.Item(0).Column23 = DblParser(R_Obj.BalanceBookEntryEnd)
            rd.TableGeneral.Item(0).Column24 = DblParser(R_Obj.BalanceLTLEnd)

            Dim SumInflow As Double = 0
            Dim SumOutflow As Double = 0
            Dim SumInflowLTL As Double = 0
            Dim SumOutflowLTL As Double = 0
            Dim SumInflowBookEntries As Double = 0
            Dim SumOutflowBookEntries As Double = 0

            For Each item As ActiveReports.CashOperationInfo In GetDisplayedList(Of ActiveReports.CashOperationInfo) _
                (R_Obj, displayIndexes, 0)

                If Version <> 1 OrElse item.OperationType = DocumentType.TillIncomeOrder _
                   OrElse item.OperationType = DocumentType.TillSpendingOrder Then

                    rd.Table1.Rows.Add()

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Date.ToShortDateString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.DocumentNumber
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.OperationTypeHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Person
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Content
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.AccountName
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.Sum)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.CurrencyCode
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.CurrencyRate, 6)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.SumLTL)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.SumBookEntry)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.CurrencyRateInAccount, 6)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.SumInAccount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.CurrencyRateChangeImpact)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = item.UniqueCode
                    If item.Sum > 0 Then
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = "1"
                        SumInflow += item.SumInAccount
                        SumInflowLTL += item.SumLTL
                        SumInflowBookEntries += item.SumLTL
                    Else
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = "0"
                        SumOutflow += -item.SumInAccount
                        SumOutflowLTL += -item.SumLTL
                        SumOutflowBookEntries += -item.SumBookEntry
                    End If

                End If

            Next

            rd.TableGeneral.Item(0).Column25 = DblParser(SumInflow)
            rd.TableGeneral.Item(0).Column26 = DblParser(SumOutflow)
            rd.TableGeneral.Item(0).Column27 = DblParser(SumInflowLTL)
            rd.TableGeneral.Item(0).Column28 = DblParser(SumOutflowLTL)
            rd.TableGeneral.Item(0).Column29 = DblParser(SumInflowBookEntries)
            rd.TableGeneral.Item(0).Column30 = DblParser(SumOutflowBookEntries)
            rd.TableGeneral.Item(0).Column31 = DblParser(SumInflow - SumOutflow)
            rd.TableGeneral.Item(0).Column32 = DblParser(SumInflowLTL - SumOutflowLTL)
            rd.TableGeneral.Item(0).Column33 = DblParser(SumInflowBookEntries - SumOutflowBookEntries)

            rd.TableGeneral.Item(0).Column34 = filterDescription

            If Version = 0 Then
                ReportFileName = "R_CashOperationInfoList.rdlc"
            ElseIf Version = 1 Then
                ReportFileName = "R_TillBook.rdlc"
            Else
                Throw New NotImplementedException("Klaida. Lėšų apyvartos žiniaraščio " & _
                                                  "spausdinamos formos 3 versijos dar nenupiešė barsukas.")
            End If
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.InvoiceInfoItemList">InvoiceInfoItemList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.InvoiceInfoItemList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            rd.TableGeneral.Item(0).Column3 = R_Obj.InfoTypesHumanReadable
            If Not R_Obj.Person Is Nothing AndAlso R_Obj.Person.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.Person.Name
                rd.TableGeneral.Item(0).Column5 = R_Obj.Person.Code
            End If

            Dim SumLTL As Double = 0
            Dim SumVatLTL As Double = 0
            Dim SumDiscountLTL As Double = 0
            Dim SumDiscountVatLTL As Double = 0
            Dim SumTotalLTL As Double = 0

            For Each item As ActiveReports.InvoiceInfoItem In GetDisplayedList(Of ActiveReports.InvoiceInfoItem) _
                (R_Obj, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Number
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.PersonName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.PersonCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.PersonVatCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.PersonAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.PersonEmail
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.Content
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.LanguageName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.CommentsInternal
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.Sum)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.SumVat)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.SumDiscount)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.SumVatDiscount)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.TotalSumDiscount)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.TotalSum)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = item.CurrencyCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.CurrencyRate, 6)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.SumLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.SumVatLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.SumDiscountLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = DblParser(item.SumVatDiscountLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.TotalSumDiscountLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.TotalSumLTL)

                SumLTL += item.SumLTL
                SumVatLTL += item.SumVatLTL
                SumTotalLTL += item.TotalSumLTL
                SumDiscountLTL += item.SumDiscountLTL
                SumDiscountVatLTL += item.SumVatDiscountLTL

            Next

            rd.TableGeneral.Item(0).Column6 = DblParser(SumLTL)
            rd.TableGeneral.Item(0).Column7 = DblParser(SumVatLTL)
            rd.TableGeneral.Item(0).Column8 = DblParser(SumDiscountLTL)
            rd.TableGeneral.Item(0).Column9 = DblParser(SumDiscountVatLTL)
            rd.TableGeneral.Item(0).Column10 = DblParser(SumTotalLTL)

            rd.TableGeneral.Item(0).Column11 = filterDescription

            ReportFileName = "R_InvoiceInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Documents.AdvanceReport">AdvanceReport</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Documents.AdvanceReport, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DocumentNumber
            If Not R_Obj.Person Is Nothing AndAlso R_Obj.Person.ID > 0 Then
                rd.TableGeneral.Item(0).Column3 = R_Obj.Person.Name
                rd.TableGeneral.Item(0).Column4 = R_Obj.Person.Code
            End If
            rd.TableGeneral.Item(0).Column5 = R_Obj.Content
            rd.TableGeneral.Item(0).Column6 = R_Obj.Account.ToString
            rd.TableGeneral.Item(0).Column7 = R_Obj.CurrencyCode
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.CurrencyRate, 6)
            rd.TableGeneral.Item(0).Column9 = DblParser(R_Obj.Sum)
            rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.SumVat)
            rd.TableGeneral.Item(0).Column11 = DblParser(R_Obj.SumTotal)
            rd.TableGeneral.Item(0).Column12 = DblParser(R_Obj.SumLTL)
            rd.TableGeneral.Item(0).Column13 = DblParser(R_Obj.SumVatLTL)
            rd.TableGeneral.Item(0).Column14 = DblParser(R_Obj.SumTotalLTL)
            rd.TableGeneral.Item(0).Column15 = R_Obj.Comments
            rd.TableGeneral.Item(0).Column16 = R_Obj.CommentsInternal
            rd.TableGeneral.Item(0).Column17 = Convert.ToInt32(Math.Floor(R_Obj.SumTotal)).ToString
            rd.TableGeneral.Item(0).Column18 = Convert.ToInt32(CRound((R_Obj.SumTotal _
                - Math.Floor(R_Obj.SumTotal)) * 100, 0)).ToString.PadLeft(2, "0"c)
            rd.TableGeneral.Item(0).Column19 = ConvertDateToWordsLT(R_Obj.Date)
            rd.TableGeneral.Item(0).Column20 = R_Obj.ReportItemsSorted.Count.ToString
            rd.TableGeneral.Item(0).Column21 = filterDescription

            Dim sign As Integer

            For Each item As Documents.AdvanceReportItem In GetDisplayedList(Of Documents.AdvanceReportItem) _
                (R_Obj.ReportItems, displayIndexes, 0)

                rd.Table1.Rows.Add()

                If item.Expenses Then
                    sign = 1
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = "1"
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = "0"
                    sign = -1
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.DocumentNumber
                If Not item.Person Is Nothing AndAlso item.Person.ID > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Person.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Person.Code
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.Content
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.Account.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.AccountVat.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.AccountCurrencyRateChangeEffect.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.VatRate)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.Sum * sign)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.SumVat * sign)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.SumTotal * sign)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.SumLTL * sign)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.SumVatLTL * sign)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.SumTotalLTL * sign)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.CurrencyRateChangeEffect)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = item.InvoiceDateAndNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = item.InvoiceContent
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = item.InvoiceCurrencyCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.InvoiceCurrencyRate, 6)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = DblParser(item.InvoiceSumOriginal)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.InvoiceSumVatOriginal)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.InvoiceSumTotalOriginal)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(item.InvoiceSumLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.InvoiceSumVatLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = DblParser(item.InvoiceSumTotalLTL)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = DblParser(item.InvoiceSumTotal)

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = Convert.ToInt32(Math.Floor(item.SumTotal)).ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = Convert.ToInt32(CRound((item.SumTotal _
                                                                                            - Math.Floor(item.SumTotal)) * 100, 0)).ToString.PadLeft(2, "0"c)


            Next

            For Each item As General.BookEntry In R_Obj.GetBookEntryList(BookEntryType.Debetas)

                rd.Table2.Rows.Add()

                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = item.Account.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = DblParser(item.Amount)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = Convert.ToInt32(Math.Floor(item.Amount)).ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column4 = Convert.ToInt32(CRound((item.Amount _
                                                                                           - Math.Floor(item.Amount)) * 100, 0)).ToString.PadLeft(2, "0"c)

            Next

            For Each item As General.BookEntry In R_Obj.GetBookEntryList(BookEntryType.Kreditas)

                rd.Table3.Rows.Add()

                rd.Table3.Item(rd.Table3.Rows.Count - 1).Column1 = item.Account.ToString
                rd.Table3.Item(rd.Table3.Rows.Count - 1).Column2 = DblParser(item.Amount)
                rd.Table3.Item(rd.Table3.Rows.Count - 1).Column3 = Convert.ToInt32(Math.Floor(item.Amount)).ToString
                rd.Table3.Item(rd.Table3.Rows.Count - 1).Column4 = Convert.ToInt32(CRound((item.Amount _
                                                                                           - Math.Floor(item.Amount)) * 100, 0)).ToString.PadLeft(2, "0"c)

            Next

            ReportFileName = "R_AdvanceReport.rdlc"
            NumberOfTablesInUse = 3

        End Sub

        ''' <summary>
        ''' Map <see cref="Documents.TillIncomeOrder">TillIncomeOrder</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Documents.TillIncomeOrder, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = ConvertDateToWordsLT(R_Obj.Date)
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentSerial
            rd.TableGeneral.Item(0).Column4 = R_Obj.DocumentNumber.ToString
            rd.TableGeneral.Item(0).Column5 = R_Obj.FullDocumentNumber
            rd.TableGeneral.Item(0).Column6 = R_Obj.Account.Account.ToString
            rd.TableGeneral.Item(0).Column7 = R_Obj.Account.CurrencyCode.Trim.ToUpper
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.CurrencyRateInAccount, 6)
            rd.TableGeneral.Item(0).Column9 = R_Obj.Account.Name
            rd.TableGeneral.Item(0).Column10 = R_Obj.Content
            If Not R_Obj.Payer Is Nothing AndAlso R_Obj.Payer.ID > 0 Then
                rd.TableGeneral.Item(0).Column11 = R_Obj.Payer.Name
                rd.TableGeneral.Item(0).Column12 = R_Obj.Payer.Code
            End If
            rd.TableGeneral.Item(0).Column13 = R_Obj.PayersRepresentative
            rd.TableGeneral.Item(0).Column14 = R_Obj.AttachmentsDescription
            rd.TableGeneral.Item(0).Column15 = R_Obj.AdvanceReportDescription
            rd.TableGeneral.Item(0).Column16 = BooleanToCheckMark(R_Obj.IsUnderPayRoll)
            rd.TableGeneral.Item(0).Column17 = R_Obj.AdditionalContent
            rd.TableGeneral.Item(0).Column18 = DblParser(R_Obj.Sum)
            rd.TableGeneral.Item(0).Column19 = DblParser(R_Obj.SumLTL)
            rd.TableGeneral.Item(0).Column20 = DblParser(R_Obj.SumCorespondences)
            rd.TableGeneral.Item(0).Column21 = DblParser(R_Obj.CurrencyRateChangeImpact)
            rd.TableGeneral.Item(0).Column22 = R_Obj.AccountCurrencyRateChangeImpact.ToString
            rd.TableGeneral.Item(0).Column23 = SumLT(R_Obj.Sum, 0, True, _
                GetCurrencySafe(R_Obj.AccountCurrency, GetCurrentCompany.BaseCurrency))
            rd.TableGeneral.Item(0).Column24 = Convert.ToInt32(Math.Floor(R_Obj.Sum)).ToString
            rd.TableGeneral.Item(0).Column25 = Convert.ToInt32(CRound(CRound(R_Obj.Sum _
                - Math.Floor(R_Obj.Sum)) * 100, 0)).ToString.PadLeft(2, "0"c)

            If Not R_Obj.Payer Is Nothing AndAlso R_Obj.Payer.ID > 0 Then
                Dim PayerStringArray As String() = SplitStringByMaxLength( _
                    R_Obj.Payer.Name & " (" & R_Obj.Payer.Code & ") " _
                    & R_Obj.PayersRepresentative, 95)
                rd.TableGeneral.Item(0).Column27 = PayerStringArray(0)
                If PayerStringArray.Length > 1 Then _
                    rd.TableGeneral.Item(0).Column28 = PayerStringArray(1)
            End If
            Dim ContentStringArray As String() = SplitStringByMaxLength( _
                R_Obj.Content.Trim & " " & R_Obj.AdditionalContent.Trim, 95)
            rd.TableGeneral.Item(0).Column29 = ContentStringArray(0)
            If ContentStringArray.Length > 1 Then _
                rd.TableGeneral.Item(0).Column30 = ContentStringArray(1)
            Dim SumStringArray As String() = SplitStringByMaxLength(SumLT(Math.Floor(R_Obj.Sum), _
                0, False, GetCurrencySafe(R_Obj.AccountCurrency, GetCurrentCompany.BaseCurrency)), 95)
            rd.TableGeneral.Item(0).Column31 = SumStringArray(0)
            If SumStringArray.Length > 1 Then _
                rd.TableGeneral.Item(0).Column32 = SumStringArray(1)
            Dim AttachmentsStringArray As String() = SplitStringByMaxLength( _
                R_Obj.AttachmentsDescription.Trim, 95)
            rd.TableGeneral.Item(0).Column33 = AttachmentsStringArray(0)
            If AttachmentsStringArray.Length > 1 Then _
                rd.TableGeneral.Item(0).Column34 = AttachmentsStringArray(1)

            rd.TableGeneral.Item(0).Column35 = filterDescription

            Dim AccountList As String = ""

            For Each item As General.BookEntry In GetDisplayedList(Of General.BookEntry) _
                (R_Obj.BookEntryItems, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Account.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = DblParser(item.Amount)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = Convert.ToInt32(Math.Floor(item.Amount)).ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = Convert.ToInt32(CRound((item.Amount _
                                                                                           - Math.Floor(item.Amount)) * 100, 0)).ToString

                If String.IsNullOrEmpty(AccountList.Trim) Then
                    AccountList = item.Account.ToString
                Else
                    AccountList = AccountList & ", " & item.Account.ToString
                End If

            Next

            rd.TableGeneral.Item(0).Column26 = AccountList

            ReportFileName = "R_TillOrderIncome.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Documents.TillSpendingsOrder">TillSpendingsOrder</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Documents.TillSpendingsOrder, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = ConvertDateToWordsLT(R_Obj.Date)
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentSerial
            rd.TableGeneral.Item(0).Column4 = R_Obj.DocumentNumber.ToString
            rd.TableGeneral.Item(0).Column5 = R_Obj.FullDocumentNumber
            rd.TableGeneral.Item(0).Column6 = R_Obj.Account.Account.ToString
            rd.TableGeneral.Item(0).Column7 = R_Obj.Account.CurrencyCode.Trim.ToUpper
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.CurrencyRateInAccount, 6)
            rd.TableGeneral.Item(0).Column9 = R_Obj.Account.Name
            rd.TableGeneral.Item(0).Column10 = R_Obj.Content
            If Not R_Obj.Receiver Is Nothing AndAlso R_Obj.Receiver.ID > 0 Then
                rd.TableGeneral.Item(0).Column11 = R_Obj.Receiver.Name
                rd.TableGeneral.Item(0).Column12 = R_Obj.Receiver.Code
            Else
                rd.TableGeneral.Item(0).Column11 = "Pagal žiniaraštį"
                rd.TableGeneral.Item(0).Column12 = ""
            End If
            rd.TableGeneral.Item(0).Column13 = R_Obj.ReceiversRepresentative
            rd.TableGeneral.Item(0).Column14 = R_Obj.AttachmentsDescription
            rd.TableGeneral.Item(0).Column15 = R_Obj.AdvanceReportDescription
            rd.TableGeneral.Item(0).Column16 = BooleanToCheckMark(R_Obj.IsUnderPayRoll)
            rd.TableGeneral.Item(0).Column17 = R_Obj.AdditionalContent
            rd.TableGeneral.Item(0).Column18 = DblParser(R_Obj.Sum)
            rd.TableGeneral.Item(0).Column19 = DblParser(R_Obj.SumLTL)
            rd.TableGeneral.Item(0).Column20 = DblParser(R_Obj.SumCorespondences)
            rd.TableGeneral.Item(0).Column21 = DblParser(R_Obj.CurrencyRateChangeImpact)
            rd.TableGeneral.Item(0).Column22 = R_Obj.AccountCurrencyRateChangeImpact.ToString
            rd.TableGeneral.Item(0).Column23 = SumLT(CRound(R_Obj.Sum), 0, True, _
                 GetCurrencySafe(R_Obj.AccountCurrency, GetCurrentCompany.BaseCurrency))
            rd.TableGeneral.Item(0).Column24 = Convert.ToInt32(Math.Floor(R_Obj.Sum)).ToString
            rd.TableGeneral.Item(0).Column25 = Convert.ToInt32(CRound(CRound(R_Obj.Sum _
                 - Math.Floor(R_Obj.Sum)) * 100, 0)).ToString

            If Not R_Obj.Receiver Is Nothing AndAlso R_Obj.Receiver.ID > 0 Then
                Dim PayerStringArray As String() = SplitStringByMaxLength( _
                    R_Obj.Receiver.Name & " (" & R_Obj.Receiver.Code & ") " _
                    & R_Obj.ReceiversRepresentative, 95)
                rd.TableGeneral.Item(0).Column27 = PayerStringArray(0)
                If PayerStringArray.Length > 1 Then _
                    rd.TableGeneral.Item(0).Column28 = PayerStringArray(1)
            Else
                rd.TableGeneral.Item(0).Column27 = "Pagal žiniaraštį"
                rd.TableGeneral.Item(0).Column28 = ""
            End If
            Dim ContentStringArray As String() = SplitStringByMaxLength( _
                R_Obj.Content.Trim & " " & R_Obj.AdditionalContent.Trim, 95)
            rd.TableGeneral.Item(0).Column29 = ContentStringArray(0)
            If ContentStringArray.Length > 1 Then _
                rd.TableGeneral.Item(0).Column30 = ContentStringArray(1)
            Dim SumStringArray As String() = SplitStringByMaxLength(SumLT(Math.Floor(R_Obj.Sum), _
                                                                          0, False, GetCurrencySafe(R_Obj.AccountCurrency, GetCurrentCompany.BaseCurrency)), 95)
            rd.TableGeneral.Item(0).Column31 = SumStringArray(0)
            If SumStringArray.Length > 1 Then _
                rd.TableGeneral.Item(0).Column32 = SumStringArray(1)
            Dim AttachmentsStringArray As String() = SplitStringByMaxLength( _
                R_Obj.AttachmentsDescription.Trim, 95)
            rd.TableGeneral.Item(0).Column33 = AttachmentsStringArray(0)
            If AttachmentsStringArray.Length > 1 Then _
                rd.TableGeneral.Item(0).Column34 = AttachmentsStringArray(1)
            rd.TableGeneral.Item(0).Column35 = filterDescription

            Dim AccountList As String = ""

            For Each item As General.BookEntry In GetDisplayedList(Of General.BookEntry) _
                (R_Obj.BookEntryItems, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Account.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = DblParser(item.Amount)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = Convert.ToInt32(Math.Floor(item.Amount)).ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = Convert.ToInt32(CRound((item.Amount _
                                                                                           - Math.Floor(item.Amount)) * 100, 0)).ToString

                If String.IsNullOrEmpty(AccountList.Trim) Then
                    AccountList = item.Account.ToString
                Else
                    AccountList = AccountList & ", " & item.Account.ToString
                End If

            Next

            rd.TableGeneral.Item(0).Column26 = AccountList

            ReportFileName = "R_TillOrderSpendings.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Documents.InvoiceMade">InvoiceMade</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Documents.InvoiceMade, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Serial.Trim
            rd.TableGeneral.Item(0).Column3 = R_Obj.FullNumber.Trim
            rd.TableGeneral.Item(0).Column4 = DblParser(R_Obj.Sum + R_Obj.SumDiscount)
            rd.TableGeneral.Item(0).Column5 = DblParser(R_Obj.SumVat + R_Obj.SumDiscountVat)
            If R_Obj.SumDiscount > 0 Then
                rd.TableGeneral.Item(0).Column6 = DblParser(-R_Obj.SumDiscount)
                rd.TableGeneral.Item(0).Column7 = DblParser(-R_Obj.SumDiscountVat)
            Else
                rd.TableGeneral.Item(0).Column6 = ""
                rd.TableGeneral.Item(0).Column7 = ""
            End If
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.SumTotal)
            rd.TableGeneral.Item(0).Column9 = R_Obj.CurrencyCode
            rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.CurrencyRate, 6)
            If Not R_Obj.Payer Is Nothing AndAlso R_Obj.Payer.ID > 0 Then
                rd.TableGeneral.Item(0).Column11 = R_Obj.Payer.Name
                rd.TableGeneral.Item(0).Column12 = R_Obj.Payer.Code
                rd.TableGeneral.Item(0).Column13 = R_Obj.Payer.CodeVAT
                rd.TableGeneral.Item(0).Column14 = R_Obj.Payer.Address
            Else
                rd.TableGeneral.Item(0).Column11 = ""
                rd.TableGeneral.Item(0).Column12 = ""
                rd.TableGeneral.Item(0).Column13 = ""
                rd.TableGeneral.Item(0).Column14 = ""
            End If
            rd.TableGeneral.Item(0).Column15 = R_Obj.CustomInfo
            rd.TableGeneral.Item(0).Column16 = R_Obj.CustomInfoAltLng
            rd.TableGeneral.Item(0).Column17 = R_Obj.VatExemptInfo
            rd.TableGeneral.Item(0).Column18 = R_Obj.VatExemptInfoAltLng
            If R_Obj.Type = Documents.InvoiceType.Credit Then
                rd.TableGeneral.Item(0).Column19 = "1"
            ElseIf R_Obj.Type = Documents.InvoiceType.Debit Then
                rd.TableGeneral.Item(0).Column19 = "2"
            Else
                rd.TableGeneral.Item(0).Column19 = "0"
            End If
            If R_Obj.Date.Date >= New Date(2014, 7, 1).Date AndAlso R_Obj.Date.Date <= New Date(2015, 6, 30).Date Then
                If CurrenciesEquals(R_Obj.CurrencyCode, "LTL", GetCurrentCompany.BaseCurrency) Then
                    rd.TableGeneral.Item(0).Column20 = DblParser(R_Obj.SumTotal / 3.4528)
                    rd.TableGeneral.Item(0).Column21 = "1"
                ElseIf CurrenciesEquals(R_Obj.CurrencyCode, "EUR", GetCurrentCompany.BaseCurrency) Then
                    rd.TableGeneral.Item(0).Column20 = DblParser(R_Obj.SumTotal * 3.4528)
                    rd.TableGeneral.Item(0).Column21 = "2"
                Else
                    rd.TableGeneral.Item(0).Column20 = "0"
                    rd.TableGeneral.Item(0).Column21 = "0"
                End If
            Else
                rd.TableGeneral.Item(0).Column20 = ""
                rd.TableGeneral.Item(0).Column21 = "0"
            End If

            rd.TableGeneral.Item(0).Column22 = SumLT(R_Obj.SumTotal, 0, True, R_Obj.CurrencyCode)

            rd.TableGeneral.Item(0).Column23 = filterDescription

            For Each item As Documents.InvoiceMadeItem In GetDisplayedList(Of Documents.InvoiceMadeItem) _
                (R_Obj.InvoiceItemsSorted, displayIndexes, 0)

                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.NameInvoice.Trim
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.NameInvoiceAltLng.Trim
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = DblParser(item.Ammount, ROUNDAMOUNTINVOICEMADE)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.UnitValue, ROUNDUNITINVOICEMADE)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.Sum, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.VatRate, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.SumVat, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.Discount, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.DiscountVat, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.SumTotal, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = item.MeasureUnitAltLng
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.UnitValueLTL, ROUNDUNITINVOICEMADE)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.SumLTL, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.SumVatLTL, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.DiscountLTL, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.DiscountVatLTL, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.SumTotalLTL, 2)

            Next

            If Version > 0 OrElse R_Obj.LanguageCode Is Nothing OrElse String.IsNullOrEmpty(R_Obj.LanguageCode.Trim) _
               OrElse R_Obj.LanguageCode.Trim.ToUpper = LanguageCodeLith.Trim.ToUpper Then
                ReportFileName = "R_Invoice.rdlc"
            Else
                ReportFileName = "R_InvoiceAltLng.rdlc"
            End If

            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Documents.ProformaInvoiceMade">ProformaInvoiceMade</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Documents.ProformaInvoiceMade, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Serial.Trim
            rd.TableGeneral.Item(0).Column3 = R_Obj.FullNumber.Trim
            rd.TableGeneral.Item(0).Column4 = DblParser(R_Obj.Sum + R_Obj.SumDiscount)
            rd.TableGeneral.Item(0).Column5 = DblParser(R_Obj.SumVat + R_Obj.SumDiscountVat)
            If R_Obj.SumDiscount > 0 Then
                rd.TableGeneral.Item(0).Column6 = DblParser(-R_Obj.SumDiscount)
                rd.TableGeneral.Item(0).Column7 = DblParser(-R_Obj.SumDiscountVat)
            Else
                rd.TableGeneral.Item(0).Column6 = ""
                rd.TableGeneral.Item(0).Column7 = ""
            End If
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.SumTotal)
            rd.TableGeneral.Item(0).Column9 = R_Obj.CurrencyCode
            If Not R_Obj.Payer Is Nothing AndAlso R_Obj.Payer.ID > 0 Then
                rd.TableGeneral.Item(0).Column10 = R_Obj.Payer.Name
                rd.TableGeneral.Item(0).Column11 = R_Obj.Payer.Code
                rd.TableGeneral.Item(0).Column12 = R_Obj.Payer.CodeVAT
                rd.TableGeneral.Item(0).Column13 = R_Obj.Payer.Address
            Else
                rd.TableGeneral.Item(0).Column10 = ""
                rd.TableGeneral.Item(0).Column11 = ""
                rd.TableGeneral.Item(0).Column12 = ""
                rd.TableGeneral.Item(0).Column13 = ""
            End If
            rd.TableGeneral.Item(0).Column14 = R_Obj.CustomInfo
            rd.TableGeneral.Item(0).Column15 = R_Obj.CustomInfoAltLng

            rd.TableGeneral.Item(0).Column16 = SumLT(R_Obj.SumTotal, 0, True, R_Obj.CurrencyCode)

            rd.TableGeneral.Item(0).Column17 = filterDescription

            For Each item As Documents.ProformaInvoiceMadeItem In GetDisplayedList(Of Documents.ProformaInvoiceMadeItem) _
                (R_Obj.InvoiceItems, displayIndexes, 0)

                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.NameInvoice.Trim
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.NameInvoiceAltLng.Trim
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = DblParser(item.Amount, ROUNDAMOUNTINVOICEMADE)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.UnitValue, ROUNDUNITINVOICEMADE)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.Sum, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.VatRate, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.SumVat, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.Discount, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.DiscountVat, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.SumTotal, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = item.MeasureUnitAltLng

            Next

            If Version > 0 OrElse StringIsNullOrEmpty(R_Obj.LanguageCode) _
               OrElse R_Obj.LanguageCode.Trim.ToUpper = LanguageCodeLith.Trim.ToUpper Then
                ReportFileName = "R_ProformaInvoice.rdlc"
            Else
                ReportFileName = "R_ProformaInvoiceAltLng.rdlc"
            End If

            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.DebtStatementItemList">DebtStatementItemList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As DebtStatementItemListPrintView, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            If R_Obj.SignWithFacsimile Then
                If AccDataAccessLayer.DatabaseAccess.GetCurrentIdentity().ConnectionType _
                    <> DataAccessTypes.ConnectionType.Local Then
                    rd.TableGeneral.Item(0).P_Column1 = ImageToByteArray( _
                        AccDataAccessLayer.Security.UserProfile.GetList.Signature)
                Else
                    rd.TableGeneral.Item(0).P_Column1 = ImageToByteArray(ByteArrayToImage( _
                        Convert.FromBase64String(MyCustomSettings.UserSignature)))
                End If
            End If

            rd.TableGeneral.Item(0).Column1 = R_Obj.Source.PeriodStart.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column2 = R_Obj.Source.PeriodEnd.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column3 = R_Obj.Source.DebtAccount.ToString()
            rd.TableGeneral.Item(0).Column4 = R_Obj.StatementDate.ToString("yyyy-MM-dd")

            For Each item As ActiveReports.DebtStatementItem In R_Obj.Source

                If R_Obj.SelectedPersonsIds.Contains(item.PersonId) Then

                    rd.Table1.Rows.Add()

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.PersonId
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.PersonName
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.PersonCode
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.PersonVatCode
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.PersonAddress
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.PersonEmail
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.PersonDescription
                    If item.ItemType = DebtStatementItemType.Transaction Then
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.Date.ToString("yyyy-MM-dd")
                    Else
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = ""
                    End If
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.DocumentNo
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.Content
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.TransactionDebit)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.TransactionCredit)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = ConvertDatabaseID(item.ItemType).ToString()
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = R_Obj.GetStatementNumber(item.PersonId)

                End If

            Next

            ReportFileName = "R_DebtStatementItemList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

#End Region

#Region "Assets"

        ''' <summary>
        ''' Map <see cref="LongTermAssetInfoList">LongTermAssetInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As LongTermAssetInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.CustomAssetGroup
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column3 = R_Obj.DateTo.ToShortDateString
            rd.TableGeneral.Item(0).Column4 = filterDescription

            For Each item As LongTermAssetInfo In GetDisplayedList(Of LongTermAssetInfo) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.LegalGroup
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.CustomGroup
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.AcquisitionDate.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.AcquisitionJournalEntryDocNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.AcquisitionJournalEntryDocContent
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.InventoryNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.AccountAcquisition.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.AccountAccumulatedAmortization.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = item.AccountValueIncrease.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = item.AccountValueDecrease.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = item.AccountRevaluedPortionAmmortization.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.LiquidationUnitValue)
                If item.ContinuedUsage Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = "Taip"
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = "Ne"
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = item.DefaultAmortizationPeriod.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.AcquisitionAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.AcquisitionAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.AmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.AmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.ValueDecreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = DblParser(item.ValueDecreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.ValueIncreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.ValueIncreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(item.ValueIncreaseAmmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.ValueIncreaseAmmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = DblParser(item.Value)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = DblParser(item.ValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = item.Ammount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(item.ValueRevaluedPortion)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(item.ValueRevaluedPortionPerUnit, 4)

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(item.BeforeAcquisitionAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(item.BeforeAcquisitionAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(item.BeforeAmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(item.BeforeAmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(item.BeforeValueDecreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(item.BeforeValueDecreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(item.BeforeValueIncreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(item.BeforeValueIncreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(item.BeforeValueIncreaseAmmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(item.BeforeValueIncreaseAmmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(item.BeforeValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(item.BeforeValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = item.BeforeAmmount.ToString

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(item.ChangeAcquisitionAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(item.ChangeAcquisitionAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(item.ChangeAmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(item.ChangeAmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(item.ChangeValueDecreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = DblParser(item.ChangeValueDecreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = DblParser(item.ChangeValueIncreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = DblParser(item.ChangeValueIncreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column53 = DblParser(item.ChangeValueIncreaseAmmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column54 = DblParser(item.ChangeValueIncreaseAmmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column55 = DblParser(item.ChangeValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column56 = DblParser(item.ChangeValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column57 = item.ChangeAmmount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column58 = item.ChangeAmmountAcquired.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column59 = DblParser(item.ChangeValueAcquired)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column60 = item.ChangeAmmountTransfered.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column61 = DblParser(item.ChangeValueTransfered)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column62 = item.ChangeAmmountDiscarded.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column63 = DblParser(item.ChangeValueDiscarded)

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column64 = DblParser(item.AfterAcquisitionAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column65 = DblParser(item.AfterAcquisitionAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column66 = DblParser(item.AfterAmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column67 = DblParser(item.AfterAmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column68 = DblParser(item.AfterValueDecreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column69 = DblParser(item.AfterValueDecreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column70 = DblParser(item.AfterValueIncreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column71 = DblParser(item.AfterValueIncreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column72 = DblParser(item.AfterValueIncreaseAmmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column73 = DblParser(item.AfterValueIncreaseAmmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column74 = DblParser(item.AfterValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column75 = DblParser(item.AfterValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column76 = item.AfterAmmount.ToString
            Next

            ReportFileName = "R_LongTermAssetInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.LongTermAssetOperationInfoListParent">LongTermAssetOperationInfoListParent</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.LongTermAssetOperationInfoListParent, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Name
            rd.TableGeneral.Item(0).Column2 = R_Obj.MeasureUnit
            rd.TableGeneral.Item(0).Column3 = R_Obj.LegalGroup
            rd.TableGeneral.Item(0).Column4 = R_Obj.CustomGroup
            rd.TableGeneral.Item(0).Column5 = R_Obj.AcquisitionDate.ToShortDateString
            rd.TableGeneral.Item(0).Column6 = R_Obj.AcquisitionJournalEntryDocNumber
            rd.TableGeneral.Item(0).Column7 = R_Obj.AcquisitionJournalEntryDocContent
            rd.TableGeneral.Item(0).Column8 = R_Obj.InventoryNumber
            rd.TableGeneral.Item(0).Column9 = R_Obj.AccountAcquisition.ToString
            rd.TableGeneral.Item(0).Column10 = R_Obj.AccountAccumulatedAmortization.ToString
            rd.TableGeneral.Item(0).Column11 = R_Obj.AccountValueIncrease.ToString
            rd.TableGeneral.Item(0).Column12 = R_Obj.AccountValueDecrease.ToString
            rd.TableGeneral.Item(0).Column13 = R_Obj.AccountRevaluedPortionAmmortization.ToString
            rd.TableGeneral.Item(0).Column14 = DblParser(R_Obj.LiquidationUnitValue)
            If R_Obj.ContinuedUsage Then
                rd.TableGeneral.Item(0).Column15 = "Taip"
            Else
                rd.TableGeneral.Item(0).Column15 = "Ne"
            End If
            rd.TableGeneral.Item(0).Column16 = R_Obj.DefaultAmortizationPeriod.ToString
            rd.TableGeneral.Item(0).Column17 = DblParser(R_Obj.AcquisitionAccountValue)
            rd.TableGeneral.Item(0).Column18 = DblParser(R_Obj.AcquisitionAccountValuePerUnit, 4)
            rd.TableGeneral.Item(0).Column19 = DblParser(R_Obj.AmortizationAccountValue)
            rd.TableGeneral.Item(0).Column20 = DblParser(R_Obj.AmortizationAccountValuePerUnit, 4)
            rd.TableGeneral.Item(0).Column21 = DblParser(R_Obj.ValueDecreaseAccountValue)
            rd.TableGeneral.Item(0).Column22 = DblParser(R_Obj.ValueDecreaseAccountValuePerUnit, 4)
            rd.TableGeneral.Item(0).Column23 = DblParser(R_Obj.ValueIncreaseAccountValue)
            rd.TableGeneral.Item(0).Column24 = DblParser(R_Obj.ValueIncreaseAccountValuePerUnit, 4)
            rd.TableGeneral.Item(0).Column25 = DblParser(R_Obj.ValueIncreaseAmmortizationAccountValue)
            rd.TableGeneral.Item(0).Column26 = DblParser(R_Obj.ValueIncreaseAmmortizationAccountValuePerUnit, 4)
            rd.TableGeneral.Item(0).Column27 = DblParser(R_Obj.Value)
            rd.TableGeneral.Item(0).Column28 = DblParser(R_Obj.ValuePerUnit, 4)
            rd.TableGeneral.Item(0).Column29 = R_Obj.Ammount.ToString
            rd.TableGeneral.Item(0).Column30 = DblParser(R_Obj.ValueRevaluedPortion)
            rd.TableGeneral.Item(0).Column31 = DblParser(R_Obj.ValueRevaluedPortionPerUnit, 4)
            rd.TableGeneral.Item(0).Column32 = filterDescription

            For Each item As LongTermAssetOperationInfo In GetDisplayedList(Of LongTermAssetOperationInfo) _
                (R_Obj.OperationList, displayIndexes, 0)
                rd.Table1.Rows.Add()
                If item.IsComplexAct Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = "Taip"
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = "Ne"
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.OperationType
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.AccountChangeType
                If ApskaitaObjects.Utilities.ConvertLocalizedName(Of Assets.LtaOperationType)(item.OperationType) _
                   = Assets.LtaOperationType.AccountChange Then

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = _
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 _
                        & ", " & rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3

                Else

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = _
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2

                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.Content
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.ActNumber.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.AttachedJournalEntry
                If item.CorrespondingAccount > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.CorrespondingAccount.ToString
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = ""
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.BeforeOperationAcquisitionAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.BeforeOperationAcquisitionAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.BeforeOperationAmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.BeforeOperationAmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.BeforeOperationValueDecreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.BeforeOperationValueDecreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.BeforeOperationValueIncreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.BeforeOperationValueIncreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.BeforeOperationValueIncreaseAmmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.BeforeOperationValueIncreaseAmmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.BeforeOperationValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.BeforeOperationValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = item.BeforeOperationAmmount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.OperationAcquisitionAccountValueChange)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.OperationAcquisitionAccountValueChangePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(item.OperationAmortizationAccountValueChange)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.OperationAmortizationAccountValueChangePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = DblParser(item.OperationValueDecreaseAccountValueChange)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = DblParser(item.OperationValueDecreaseAccountValueChangePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = DblParser(item.OperationValueIncreaseAccountValueChange)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(item.OperationValueIncreaseAccountValueChangePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(item.OperationValueIncreaseAmmortizationAccountValueChange)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(item.OperationValueIncreaseAmmortizationAccountValueChangePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(item.OperationValueChange)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(item.OperationValueChangePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = item.OperationAmmountChange.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(item.AfterOperationAcquisitionAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(item.AfterOperationAcquisitionAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(item.AfterOperationAmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(item.AfterOperationAmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(item.AfterOperationValueDecreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(item.AfterOperationValueDecreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(item.AfterOperationValueIncreaseAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(item.AfterOperationValueIncreaseAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(item.AfterOperationValueIncreaseAmmortizationAccountValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(item.AfterOperationValueIncreaseAmmortizationAccountValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(item.AfterOperationValue)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(item.AfterOperationValuePerUnit, 4)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = item.AfterOperationAmmount.ToString
                If item.NewAmortizationPeriod > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = item.NewAmortizationPeriod.ToString
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = ""
                End If
            Next

            ReportFileName = "R_LongTermAssetOperationInfoListParent.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.OperationAccountChange">OperationAccountChange</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.OperationAccountChange, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.AccountTypeHumanReadable
            rd.TableGeneral.Item(0).Column3 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column5 = BooleanToCheckMark(R_Obj.IsComplexAct)
            rd.TableGeneral.Item(0).Column6 = R_Obj.ComplexActID.ToString
            rd.TableGeneral.Item(0).Column7 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column8 = R_Obj.Content
            rd.TableGeneral.Item(0).Column9 = R_Obj.DocumentNumber
            rd.TableGeneral.Item(0).Column10 = R_Obj.JournalEntryID.ToString

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.Background.AssetID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.Background.AssetName
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.Background.AssetMeasureUnit
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.Background.AssetAquisitionID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(R_Obj.Background.AssetLiquidationValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.Background.InitialAssetAcquiredAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = R_Obj.Background.InitialAssetContraryAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = R_Obj.Background.InitialAssetValueDecreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.Background.InitialAssetValueIncreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.Background.InitialAssetValueIncreaseAmortizationAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = R_Obj.Background.CurrentAssetAcquiredAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = R_Obj.Background.CurrentAssetContraryAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = R_Obj.Background.CurrentAssetValueDecreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = R_Obj.Background.CurrentAssetValueIncreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = R_Obj.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.Background.CurrentAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(R_Obj.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = R_Obj.Background.CurrentAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = DblParser(R_Obj.Background.CurrentAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = DblParser(R_Obj.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = R_Obj.Background.CurrentUsageTermMonths.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = R_Obj.Background.CurrentAmortizationPeriod.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = BooleanToCheckMark(R_Obj.Background.CurrentUsageStatus)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = R_Obj.CurrentAccount.ToString()
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(R_Obj.CurrentAccountBalance, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = R_Obj.NewAccount.ToString()

            ReportFileName = "R_LongTermAssetAccountChange.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.OperationAcquisitionValueIncrease">OperationAcquisitionValueIncrease</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.OperationAcquisitionValueIncrease, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(R_Obj.IsComplexAct)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ComplexActID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.JournalEntryID.ToString
            rd.TableGeneral.Item(0).Column9 = R_Obj.JournalEntryDocumentNumber
            rd.TableGeneral.Item(0).Column10 = R_Obj.JournalEntryContent
            rd.TableGeneral.Item(0).Column11 = R_Obj.JournalEntryPerson
            rd.TableGeneral.Item(0).Column12 = R_Obj.JournalEntryDocumentType
            rd.TableGeneral.Item(0).Column13 = DblParser(R_Obj.JournalEntryAmount, 2)
            rd.TableGeneral.Item(0).Column14 = R_Obj.JournalEntryBookEntries.ToString

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.Background.AssetID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.Background.AssetName
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.Background.AssetMeasureUnit
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.Background.AssetAquisitionID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(R_Obj.Background.AssetLiquidationValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.Background.CurrentAssetAcquiredAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = R_Obj.Background.CurrentAssetContraryAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = R_Obj.Background.CurrentAssetValueDecreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.Background.CurrentAssetValueIncreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(R_Obj.Background.CurrentAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(R_Obj.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = R_Obj.Background.CurrentAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(R_Obj.Background.CurrentAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(R_Obj.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = R_Obj.Background.CurrentUsageTermMonths.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = R_Obj.Background.CurrentAmortizationPeriod.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = BooleanToCheckMark(R_Obj.Background.CurrentUsageStatus)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = R_Obj.Background.ChangeAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(R_Obj.Background.ChangeAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(R_Obj.Background.ChangeAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(R_Obj.Background.ChangeAssetUnitValue, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(R_Obj.Background.ChangeAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(R_Obj.Background.ChangeAssetRevaluedPortionUnitValue, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(R_Obj.Background.ChangeAssetRevaluedPortionValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(R_Obj.Background.AfterOperationAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(R_Obj.Background.AfterOperationAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(R_Obj.Background.AfterOperationAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(R_Obj.Background.AfterOperationAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(R_Obj.ValueIncrease, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(R_Obj.ValueIncreasePerUnit, ROUNDUNITASSET)

            ReportFileName = "R_LongTermAssetAcquisitionValueIncrease.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.OperationAmortization">OperationAmortization</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.OperationAmortization, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(R_Obj.IsComplexAct)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ComplexActID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.JournalEntryID.ToString
            rd.TableGeneral.Item(0).Column9 = R_Obj.DocumentNumber
            rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.TotalValueChange, 2)

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.Background.AssetID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.Background.AssetName
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.Background.AssetMeasureUnit
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.Background.AssetAquisitionID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(R_Obj.Background.AssetLiquidationValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.Background.CurrentAssetAcquiredAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = R_Obj.Background.CurrentAssetContraryAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = R_Obj.Background.CurrentAssetValueDecreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.Background.CurrentAssetValueIncreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(R_Obj.Background.CurrentAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(R_Obj.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = R_Obj.Background.CurrentAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(R_Obj.Background.CurrentAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(R_Obj.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = R_Obj.Background.CurrentUsageTermMonths.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = R_Obj.Background.CurrentAmortizationPeriod.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = BooleanToCheckMark(R_Obj.Background.CurrentUsageStatus)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(R_Obj.Background.ChangeAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(R_Obj.Background.ChangeAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(R_Obj.Background.ChangeValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(R_Obj.Background.ChangeValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(R_Obj.Background.ChangeAssetUnitValue, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(R_Obj.Background.ChangeAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(R_Obj.Background.ChangeAssetRevaluedPortionUnitValue, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(R_Obj.Background.ChangeAssetRevaluedPortionValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(R_Obj.Background.AfterOperationAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(R_Obj.Background.AfterOperationAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(R_Obj.Background.AfterOperationAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(R_Obj.Background.AfterOperationAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(R_Obj.TotalValueChange, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(R_Obj.UnitValueChange, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(R_Obj.RevaluedPortionTotalValueChange, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(R_Obj.RevaluedPortionUnitValueChange, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = R_Obj.AmortizationCalculatedForMonths.ToString()
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = R_Obj.AmortizationCalculations
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = R_Obj.AccountCosts.ToString()

            ReportFileName = "R_LongTermAssetAmortization.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.ComplexOperationAmortization">ComplexOperationAmortization</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.ComplexOperationAmortization, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(True)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.JournalEntryID.ToString
            rd.TableGeneral.Item(0).Column9 = R_Obj.DocumentNumber
            rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.TotalValueChange, 2)
            rd.TableGeneral.Item(0).Column11 = filterDescription

            For Each item As Assets.OperationAmortization In GetDisplayedList(Of Assets.OperationAmortization) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Background.AssetID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Background.AssetName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Background.AssetMeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Background.AssetAquisitionID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.Background.AssetLiquidationValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.Background.CurrentAssetAcquiredAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.Background.CurrentAssetContraryAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.Background.CurrentAssetValueDecreaseAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.Background.CurrentAssetValueIncreaseAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = item.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.Background.CurrentAcquisitionAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.Background.CurrentAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.Background.CurrentValueDecreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.Background.CurrentValueIncreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = item.Background.CurrentAssetAmount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.Background.CurrentAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(item.Background.CurrentAssetValueRevaluedPortion, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = item.Background.CurrentUsageTermMonths.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = item.Background.CurrentAmortizationPeriod.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = BooleanToCheckMark(item.Background.CurrentUsageStatus)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(item.Background.ChangeAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(item.Background.ChangeAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(item.Background.ChangeValueIncreaseAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(item.Background.ChangeValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(item.Background.ChangeAssetUnitValue, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(item.Background.ChangeAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(item.Background.ChangeAssetRevaluedPortionUnitValue, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(item.Background.ChangeAssetRevaluedPortionValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(item.Background.AfterOperationAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(item.Background.AfterOperationAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(item.Background.AfterOperationValueIncreaseAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(item.Background.AfterOperationValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(item.Background.AfterOperationAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(item.Background.AfterOperationAssetValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(item.Background.AfterOperationAssetValueRevaluedPortion, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(item.Background.AfterOperationAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(item.TotalValueChange, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(item.UnitValueChange, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(item.RevaluedPortionTotalValueChange, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(item.RevaluedPortionUnitValueChange, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = item.AmortizationCalculatedForMonths.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = item.AmortizationCalculations
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = item.AccountCosts.ToString()

            Next

            ReportFileName = "R_LongTermAssetAmortization.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.OperationAmortizationPeriodChange">OperationAmortizationPeriodChange</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.OperationAmortizationPeriodChange, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(R_Obj.IsComplexAct)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ComplexActID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.DocumentNumber

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.Background.AssetID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.Background.AssetName
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.Background.AssetMeasureUnit
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.Background.AssetAquisitionID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(R_Obj.Background.AssetLiquidationValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.Background.CurrentAssetAcquiredAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = R_Obj.Background.CurrentAssetContraryAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = R_Obj.Background.CurrentAssetValueDecreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.Background.CurrentAssetValueIncreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(R_Obj.Background.CurrentAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(R_Obj.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = R_Obj.Background.CurrentAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(R_Obj.Background.CurrentAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(R_Obj.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = R_Obj.Background.CurrentUsageTermMonths.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = R_Obj.Background.CurrentAmortizationPeriod.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = BooleanToCheckMark(R_Obj.Background.CurrentUsageStatus)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = R_Obj.NewPeriod.ToString

            ReportFileName = "R_LongTermAssetAmortizationPeriod.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.OperationDiscard">OperationDiscard</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.OperationDiscard, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(R_Obj.IsComplexAct)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ComplexActID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.JournalEntryID.ToString
            rd.TableGeneral.Item(0).Column9 = R_Obj.DocumentNumber
            rd.TableGeneral.Item(0).Column10 = DblParser(-R_Obj.Background.ChangeAssetValue, 2)

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.Background.AssetID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.Background.AssetName
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.Background.AssetMeasureUnit
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.Background.AssetAquisitionID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(R_Obj.Background.AssetLiquidationValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.Background.CurrentAssetAcquiredAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = R_Obj.Background.CurrentAssetContraryAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = R_Obj.Background.CurrentAssetValueDecreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.Background.CurrentAssetValueIncreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(R_Obj.Background.CurrentAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(R_Obj.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = R_Obj.Background.CurrentAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(R_Obj.Background.CurrentAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(R_Obj.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = R_Obj.Background.CurrentUsageTermMonths.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = R_Obj.Background.CurrentAmortizationPeriod.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = BooleanToCheckMark(R_Obj.Background.CurrentUsageStatus)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = R_Obj.Background.ChangeAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(-R_Obj.Background.ChangeAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(-R_Obj.Background.ChangeAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(-R_Obj.Background.ChangeAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(-R_Obj.Background.ChangeAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(-R_Obj.Background.ChangeValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(-R_Obj.Background.ChangeValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(-R_Obj.Background.ChangeValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(-R_Obj.Background.ChangeValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(-R_Obj.Background.ChangeValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(-R_Obj.Background.ChangeValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(-R_Obj.Background.ChangeAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(-R_Obj.Background.ChangeAssetRevaluedPortionValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(R_Obj.Background.AfterOperationAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(R_Obj.Background.AfterOperationAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(R_Obj.Background.AfterOperationAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(R_Obj.Background.AfterOperationAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(R_Obj.Background.AfterOperationValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(R_Obj.Background.AfterOperationValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column53 = R_Obj.Background.AfterOperationAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column54 = DblParser(R_Obj.Background.AfterOperationAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column55 = DblParser(R_Obj.Background.AfterOperationAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column56 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column57 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column58 = R_Obj.AmountToDiscard.ToString()
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column59 = R_Obj.AccountCosts.ToString()

            ReportFileName = "R_LongTermAssetDiscard.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.ComplexOperationDiscard">ComplexOperationDiscard</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.ComplexOperationDiscard, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(True)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.JournalEntryID.ToString
            rd.TableGeneral.Item(0).Column9 = R_Obj.DocumentNumber
            rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.TotalDiscardCosts, 2)
            rd.TableGeneral.Item(0).Column11 = filterDescription

            For Each item As Assets.OperationDiscard In GetDisplayedList(Of Assets.OperationDiscard) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Background.AssetID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Background.AssetName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Background.AssetMeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Background.AssetAquisitionID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.Background.AssetLiquidationValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.Background.CurrentAssetAcquiredAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.Background.CurrentAssetContraryAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.Background.CurrentAssetValueDecreaseAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.Background.CurrentAssetValueIncreaseAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = item.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.Background.CurrentAcquisitionAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.Background.CurrentAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.Background.CurrentValueDecreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.Background.CurrentValueIncreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = item.Background.CurrentAssetAmount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.Background.CurrentAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(item.Background.CurrentAssetValueRevaluedPortion, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = item.Background.CurrentUsageTermMonths.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = item.Background.CurrentAmortizationPeriod.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = BooleanToCheckMark(item.Background.CurrentUsageStatus)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = item.Background.ChangeAssetAmount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(-item.Background.ChangeAcquisitionAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(-item.Background.ChangeAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(-item.Background.ChangeAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(-item.Background.ChangeAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(-item.Background.ChangeValueDecreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(-item.Background.ChangeValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(-item.Background.ChangeValueIncreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(-item.Background.ChangeValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(-item.Background.ChangeValueIncreaseAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(-item.Background.ChangeValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(-item.Background.ChangeAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(-item.Background.ChangeAssetRevaluedPortionValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(item.Background.AfterOperationAcquisitionAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(item.Background.AfterOperationAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(item.Background.AfterOperationAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(item.Background.AfterOperationAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(item.Background.AfterOperationValueDecreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(item.Background.AfterOperationValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(item.Background.AfterOperationValueIncreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = DblParser(item.Background.AfterOperationValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = DblParser(item.Background.AfterOperationValueIncreaseAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = DblParser(item.Background.AfterOperationValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column53 = item.Background.AfterOperationAssetAmount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column54 = DblParser(item.Background.AfterOperationAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column55 = DblParser(item.Background.AfterOperationAssetValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column56 = DblParser(item.Background.AfterOperationAssetValueRevaluedPortion, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column57 = DblParser(item.Background.AfterOperationAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column58 = item.AmountToDiscard.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column59 = item.AccountCosts.ToString()

            Next

            ReportFileName = "R_LongTermAssetDiscard.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.OperationOperationalStatusChange">OperationOperationalStatusChange</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.OperationOperationalStatusChange, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(R_Obj.IsComplexAct)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ComplexActID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.DocumentNumber

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.Background.AssetID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.Background.AssetName
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.Background.AssetMeasureUnit
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.Background.AssetAquisitionID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(R_Obj.Background.AssetLiquidationValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.Background.CurrentAssetAcquiredAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = R_Obj.Background.CurrentAssetContraryAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = R_Obj.Background.CurrentAssetValueDecreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.Background.CurrentAssetValueIncreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(R_Obj.Background.CurrentAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(R_Obj.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = R_Obj.Background.CurrentAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(R_Obj.Background.CurrentAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(R_Obj.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = R_Obj.Background.CurrentUsageTermMonths.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = R_Obj.Background.CurrentAmortizationPeriod.ToString

            If R_Obj.BeginOperationalPeriod Then
                ReportFileName = "R_LongTermAssetUsingStart.rdlc"
            Else
                ReportFileName = "R_LongTermAssetUsingEnd.rdlc"
            End If

            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.ComplexOperationOperationalStatusChange">ComplexOperationOperationalStatusChange</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.ComplexOperationOperationalStatusChange, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(True)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.DocumentNumber
            rd.TableGeneral.Item(0).Column9 = filterDescription

            For Each item As Assets.OperationOperationalStatusChange In GetDisplayedList(Of Assets.OperationOperationalStatusChange) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Background.AssetID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Background.AssetName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Background.AssetMeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Background.AssetAquisitionID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.Background.AssetLiquidationValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.Background.CurrentAssetAcquiredAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.Background.CurrentAssetContraryAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.Background.CurrentAssetValueDecreaseAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.Background.CurrentAssetValueIncreaseAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = item.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.Background.CurrentAcquisitionAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.Background.CurrentAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.Background.CurrentValueDecreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.Background.CurrentValueIncreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = item.Background.CurrentAssetAmount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.Background.CurrentAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(item.Background.CurrentAssetValueRevaluedPortion, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = item.Background.CurrentUsageTermMonths.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = item.Background.CurrentAmortizationPeriod.ToString

            Next

            If R_Obj.BeginOperationalPeriod Then
                ReportFileName = "R_LongTermAssetUsingStart.rdlc"
            Else
                ReportFileName = "R_LongTermAssetUsingEnd.rdlc"
            End If

            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.OperationTransfer">OperationTransfer</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.OperationTransfer, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(R_Obj.IsComplexAct)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ComplexActID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.JournalEntryID.ToString
            rd.TableGeneral.Item(0).Column9 = R_Obj.JournalEntryDocumentNumber
            rd.TableGeneral.Item(0).Column10 = R_Obj.JournalEntryContent
            rd.TableGeneral.Item(0).Column11 = R_Obj.JournalEntryPerson
            rd.TableGeneral.Item(0).Column12 = R_Obj.JournalEntryDocumentType
            rd.TableGeneral.Item(0).Column13 = DblParser(R_Obj.JournalEntryAmount, 2)
            rd.TableGeneral.Item(0).Column14 = R_Obj.JournalEntryBookEntries

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.Background.AssetID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.Background.AssetName
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.Background.AssetMeasureUnit
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.Background.AssetAquisitionID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(R_Obj.Background.AssetLiquidationValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.Background.CurrentAssetAcquiredAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = R_Obj.Background.CurrentAssetContraryAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = R_Obj.Background.CurrentAssetValueDecreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.Background.CurrentAssetValueIncreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(R_Obj.Background.CurrentAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(R_Obj.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = R_Obj.Background.CurrentAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(R_Obj.Background.CurrentAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(R_Obj.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = R_Obj.Background.CurrentUsageTermMonths.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = R_Obj.Background.CurrentAmortizationPeriod.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = BooleanToCheckMark(R_Obj.Background.CurrentUsageStatus)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = R_Obj.Background.ChangeAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(-R_Obj.Background.ChangeAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(-R_Obj.Background.ChangeAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(-R_Obj.Background.ChangeAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(-R_Obj.Background.ChangeAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(-R_Obj.Background.ChangeValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(-R_Obj.Background.ChangeValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(-R_Obj.Background.ChangeValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(-R_Obj.Background.ChangeValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(-R_Obj.Background.ChangeValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(-R_Obj.Background.ChangeValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(-R_Obj.Background.ChangeAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(-R_Obj.Background.ChangeAssetRevaluedPortionValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(R_Obj.Background.AfterOperationAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(R_Obj.Background.AfterOperationAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(R_Obj.Background.AfterOperationAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(R_Obj.Background.AfterOperationAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(R_Obj.Background.AfterOperationValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(R_Obj.Background.AfterOperationValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column53 = R_Obj.Background.AfterOperationAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column54 = DblParser(R_Obj.Background.AfterOperationAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column55 = DblParser(R_Obj.Background.AfterOperationAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column56 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column57 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column58 = R_Obj.AmountToTransfer.ToString()

            ReportFileName = "R_LongTermAssetTransfer.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.OperationValueChange">OperationValueChange</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.OperationValueChange, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(R_Obj.IsComplexAct)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ComplexActID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.JournalEntryID.ToString
            rd.TableGeneral.Item(0).Column9 = R_Obj.JournalEntryDocumentNumber
            rd.TableGeneral.Item(0).Column10 = R_Obj.JournalEntryContent
            rd.TableGeneral.Item(0).Column11 = R_Obj.JournalEntryPerson
            rd.TableGeneral.Item(0).Column12 = R_Obj.JournalEntryDocumentType
            rd.TableGeneral.Item(0).Column13 = DblParser(R_Obj.JournalEntryAmount, 2)
            rd.TableGeneral.Item(0).Column14 = R_Obj.JournalEntryBookEntries.ToString

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.Background.AssetID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.Background.AssetName
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.Background.AssetMeasureUnit
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.Background.AssetAquisitionID.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(R_Obj.Background.AssetLiquidationValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.Background.CurrentAssetAcquiredAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = R_Obj.Background.CurrentAssetContraryAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = R_Obj.Background.CurrentAssetValueDecreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.Background.CurrentAssetValueIncreaseAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(R_Obj.Background.CurrentAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(R_Obj.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(R_Obj.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(R_Obj.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = R_Obj.Background.CurrentAssetAmount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(R_Obj.Background.CurrentAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(R_Obj.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(R_Obj.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = R_Obj.Background.CurrentUsageTermMonths.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = R_Obj.Background.CurrentAmortizationPeriod.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = BooleanToCheckMark(R_Obj.Background.CurrentUsageStatus)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(R_Obj.Background.ChangeValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(R_Obj.Background.ChangeValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(R_Obj.Background.ChangeValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(R_Obj.Background.ChangeValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(R_Obj.Background.ChangeAssetUnitValue, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(R_Obj.Background.ChangeAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(R_Obj.Background.ChangeAssetRevaluedPortionUnitValue, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(R_Obj.Background.ChangeAssetRevaluedPortionValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(R_Obj.Background.AfterOperationValueDecreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(R_Obj.Background.AfterOperationValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAccountValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(R_Obj.Background.AfterOperationValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(R_Obj.Background.AfterOperationAssetValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(R_Obj.Background.AfterOperationAssetValuePerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortion, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(R_Obj.Background.AfterOperationAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(R_Obj.ValueChangeTotal, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(R_Obj.ValueChangePerUnit, ROUNDUNITASSET)

            ReportFileName = "R_LongTermAssetValueChange.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Assets.ComplexOperationValueChange">ComplexOperationValueChange</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Assets.ComplexOperationValueChange, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.InsertDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column3 = R_Obj.UpdateDate.ToString("yyyy-MM-dd HH:mm:ss")
            rd.TableGeneral.Item(0).Column4 = BooleanToCheckMark(True)
            rd.TableGeneral.Item(0).Column5 = R_Obj.ID.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Date.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column8 = R_Obj.JournalEntryID.ToString
            rd.TableGeneral.Item(0).Column9 = R_Obj.JournalEntryDocumentNumber
            rd.TableGeneral.Item(0).Column10 = R_Obj.JournalEntryContent
            rd.TableGeneral.Item(0).Column11 = R_Obj.JournalEntryPerson
            rd.TableGeneral.Item(0).Column12 = R_Obj.JournalEntryDocumentType
            rd.TableGeneral.Item(0).Column13 = DblParser(R_Obj.JournalEntryAmount, 2)
            rd.TableGeneral.Item(0).Column14 = R_Obj.JournalEntryBookEntries.ToString
            rd.TableGeneral.Item(0).Column15 = filterDescription

            For Each item As Assets.OperationValueChange In GetDisplayedList(Of Assets.OperationValueChange) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Background.AssetID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Background.AssetName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Background.AssetMeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Background.AssetDateAcquired.ToString("yyyy-MM-dd")
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Background.AssetAquisitionID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.Background.AssetLiquidationValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.Background.CurrentAssetAcquiredAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.Background.CurrentAssetContraryAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.Background.CurrentAssetValueDecreaseAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.Background.CurrentAssetValueIncreaseAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = item.Background.CurrentAssetValueIncreaseAmortizationAccount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.Background.CurrentAcquisitionAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.Background.CurrentAcquisitionAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.Background.CurrentAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.Background.CurrentAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.Background.CurrentValueDecreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.Background.CurrentValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.Background.CurrentValueIncreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.Background.CurrentValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.Background.CurrentValueIncreaseAmortizationAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.Background.CurrentValueIncreaseAmortizationAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = item.Background.CurrentAssetAmount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.Background.CurrentAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.Background.CurrentAssetValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(item.Background.CurrentAssetValueRevaluedPortion, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.Background.CurrentAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = item.Background.CurrentUsageTermMonths.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = item.Background.CurrentAmortizationPeriod.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = BooleanToCheckMark(item.Background.CurrentUsageStatus)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(item.Background.ChangeValueDecreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(item.Background.ChangeValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(item.Background.ChangeValueIncreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(item.Background.ChangeValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(item.Background.ChangeAssetUnitValue, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(item.Background.ChangeAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(item.Background.ChangeAssetRevaluedPortionUnitValue, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(item.Background.ChangeAssetRevaluedPortionValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(item.Background.AfterOperationValueDecreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(item.Background.AfterOperationValueDecreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(item.Background.AfterOperationValueIncreaseAccountValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(item.Background.AfterOperationValueIncreaseAccountValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(item.Background.AfterOperationAssetValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(item.Background.AfterOperationAssetValuePerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(item.Background.AfterOperationAssetValueRevaluedPortion, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(item.Background.AfterOperationAssetValueRevaluedPortionPerUnit, ROUNDUNITASSET)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(item.ValueChangeTotal, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(item.ValueChangePerUnit, ROUNDUNITASSET)

            Next

            ReportFileName = "R_LongTermAssetValueChange.rdlc"
            NumberOfTablesInUse = 1

        End Sub

#End Region

#Region "Workers"

        ''' <summary>
        ''' Map <see cref="ActiveReports.Declaration">Declaration</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.Declaration, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            CopyDataTable(rd.TableCompany, R_Obj.DeclarationDataSet, "General")
            CopyDataTable(rd.TableGeneral, R_Obj.DeclarationDataSet, "Specific")
            NumberOfTablesInUse = R_Obj.Criteria.DeclarationType.DetailsTableCount

            If R_Obj.Criteria.DeclarationType.DetailsTableCount > 0 Then _
                CopyDataTable(rd.Table1, R_Obj.DeclarationDataSet, R_Obj.DeclarationDataSet.Tables(2).TableName)
            If R_Obj.Criteria.DeclarationType.DetailsTableCount > 1 Then _
                CopyDataTable(rd.Table2, R_Obj.DeclarationDataSet, R_Obj.DeclarationDataSet.Tables(3).TableName)
            If R_Obj.Criteria.DeclarationType.DetailsTableCount > 2 Then _
                CopyDataTable(rd.Table3, R_Obj.DeclarationDataSet, R_Obj.DeclarationDataSet.Tables(4).TableName)
            If R_Obj.Criteria.DeclarationType.DetailsTableCount > 3 Then _
                CopyDataTable(rd.Table4, R_Obj.DeclarationDataSet, R_Obj.DeclarationDataSet.Tables(5).TableName)

            ReportFileName = R_Obj.Criteria.DeclarationType.RdlcFileName

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.ImprestSheetInfoList">ImprestSheetInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.ImprestSheetInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            If R_Obj.ShowPayedOut Then
                rd.TableGeneral.Item(0).Column3 = "X"
            Else
                rd.TableGeneral.Item(0).Column3 = ""
            End If
            rd.TableGeneral.Item(0).Column4 = R_Obj.TotalWorkersCount.ToString
            rd.TableGeneral.Item(0).Column5 = DblParser(R_Obj.TotalSum)
            rd.TableGeneral.Item(0).Column6 = DblParser(R_Obj.TotalSumPayedOut)
            rd.TableGeneral.Item(0).Column7 = filterDescription

            For Each item As ActiveReports.ImprestSheetInfo In GetDisplayedList(Of ActiveReports.ImprestSheetInfo) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Year.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Month.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Number.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.WorkersCount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.TotalSum)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.TotalSumPayedOut)
            Next

            ReportFileName = "R_ImprestSheetInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Workers.ImprestSheet">ImprestSheet</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Workers.ImprestSheet, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Number.ToString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Date.Year.ToString
            rd.TableGeneral.Item(0).Column3 = GetLithuanianMonth(R_Obj.Date.Month)
            rd.TableGeneral.Item(0).Column4 = R_Obj.Date.Day.ToString
            rd.TableGeneral.Item(0).Column5 = R_Obj.Year.ToString
            rd.TableGeneral.Item(0).Column6 = GetLithuanianMonth(R_Obj.Month)
            rd.TableGeneral.Item(0).Column7 = SumLT(R_Obj.TotalSum, 0, True, GetCurrentCompany.BaseCurrency)
            rd.TableGeneral.Item(0).Column8 = Convert.ToInt32(Math.Floor(R_Obj.TotalSum)).ToString
            rd.TableGeneral.Item(0).Column9 = DblParser((R_Obj.TotalSum - Math.Floor(R_Obj.TotalSum)) * 100, 0)
            rd.TableGeneral.Item(0).Column10 = filterDescription

            For Each item As Workers.ImprestItem In GetDisplayedList(Of Workers.ImprestItem) _
                (R_Obj.Items, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.PersonName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.PersonCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.ContractSerial & item.ContractNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = DblParser(item.PayOffSumTotal)
            Next

            ReportFileName = "R_ImprestSheet.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.PayOutNaturalPersonInfoList">PayOutNaturalPersonInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.PayOutNaturalPersonInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            rd.TableGeneral.Item(0).Column3 = filterDescription

            For Each item As ActiveReports.PayOutNaturalPersonInfo In GetDisplayedList(Of ActiveReports.PayOutNaturalPersonInfo) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.DocNumber
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Content
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.PersonInfo.Trim
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.PersonCodeSODRA.Trim
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.SumBruto)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.RateGPM)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.DeductionGPM)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.RatePSDForPerson)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.DeductionPSD)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.RateSODRAForPerson)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.DeductionSODRA)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.SumNeto)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.RatePSDForCompany)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.ContributionPSD)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.RateSODRAForCompany)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.ContributionSODRA)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = GetMinLengthString(item.CodeVMI, 2, "0"c)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = GetMinLengthString(item.CodeSODRA, 2, "0"c)
            Next

            ReportFileName = "R_PayOutNaturalPersonList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.WageSheetInfoList">WageSheetInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
             ByVal R_Obj As ActiveReports.WageSheetInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            If R_Obj.ShowPayedOut Then
                rd.TableGeneral.Item(0).Column3 = "Taip"
            Else
                rd.TableGeneral.Item(0).Column3 = "Ne"
            End If
            rd.TableGeneral.Item(0).Column4 = filterDescription

            For Each item As ActiveReports.WageSheetInfo In GetDisplayedList(Of ActiveReports.WageSheetInfo) _
                (R_Obj, displayIndexes, 0)
                rd.Table1.Rows.Add()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Year.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Month.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Number.ToString
                If item.IsNonClosing Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = "X"
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = ""
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.WorkersCount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.HoursWorked, 3)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.DaysWorked.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.PayOutWage + item.PayOutSickLeave)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.PayOutHoliday)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.PayOutRedundancy)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.DeductionsGPM)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.DeductionsSODRA + item.DeductionsPSD _
                                                                              + item.DeductionsPSDSickLeave)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.DeductionsOther)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.DeductionsImprest)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.PayOutAfterDeductions)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.PayedOut)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.Debt)
            Next

            ReportFileName = "R_WageSheetInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.WorkerWageInfoReport">WorkerWageInfoReport</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.WorkerWageInfoReport, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToString("yyyy-MM-dd")
            rd.TableGeneral.Item(0).Column3 = R_Obj.ContractNumber.ToString
            rd.TableGeneral.Item(0).Column4 = R_Obj.ContractSerial
            If R_Obj.IsConsolidated Then
                rd.TableGeneral.Item(0).Column5 = "Taip"
            Else
                rd.TableGeneral.Item(0).Column5 = "Ne"
            End If
            rd.TableGeneral.Item(0).Column6 = R_Obj.PersonID.ToString
            rd.TableGeneral.Item(0).Column7 = R_Obj.PersonInfo
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.DebtAtTheStart, 2)
            rd.TableGeneral.Item(0).Column9 = DblParser(R_Obj.DebtAtEnd, 2)
            rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.UnusedHolidaysAtStart, ROUNDACCUMULATEDHOLIDAY)
            rd.TableGeneral.Item(0).Column11 = DblParser(R_Obj.UnusedHolidaysAtEnd, ROUNDACCUMULATEDHOLIDAY)

            Dim debt As Double = R_Obj.DebtAtTheStart
            Dim normalHoursWorked As Double = 0
            Dim hrHoursWorked As Double = 0
            Dim onHoursWorked As Double = 0
            Dim scHoursWorked As Double = 0
            Dim totalHoursWorked As Double = 0
            Dim truancyHours As Double = 0
            Dim totalDaysWorked As Integer = 0
            Dim holidayWD As Integer = 0
            Dim holidayRD As Integer = 0
            Dim sickDays As Integer = 0
            Dim standartHours As Double = 0
            Dim standartDays As Integer = 0
            Dim bonusPayQuarterly As Double = 0
            Dim bonusPayAnnual As Double = 0
            Dim otherPayRelatedToWork As Double = 0
            Dim otherPayNotRelatedToWork As Double = 0
            Dim payOutWage As Double = 0
            Dim payOutExtraPay As Double = 0
            Dim payOutHR As Double = 0
            Dim payOutON As Double = 0
            Dim payOutSC As Double = 0
            Dim payOutSickLeave As Double = 0
            Dim payOutHoliday As Double = 0
            Dim payOutTotal As Double = 0
            Dim deductionGPM As Double = 0
            Dim deductionPSD As Double = 0
            Dim deductionSODRA As Double = 0
            Dim deductionOther As Double = 0
            Dim contributionSODRA As Double = 0
            Dim contributionPSD As Double = 0
            Dim contributionGuaranteeFund As Double = 0
            Dim payableTotal As Double = 0
            Dim npd As Double = 0
            Dim pnpd As Double = 0
            Dim payedOutTotalSum As Double = 0
            Dim totalDeductions As Double = 0
            Dim totalContributions As Double = 0

            For Each item As ActiveReports.WorkerWageInfo In GetDisplayedList(Of ActiveReports.WorkerWageInfo) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Year.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Month.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = DblParser(item.RateSODRAEmployee, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = DblParser(item.RateSODRAEmployer, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.RatePSDEmployee, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.RatePSDEmployer, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.RateGuaranteeFund, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.RateGPM, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.RateHR, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.RateSC, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.RateON, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.RateSickLeave, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = item.NPDFormula
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.WorkLoad, ROUNDWORKLOAD)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.ApplicableVDUHourly, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.ApplicableVDUDaily, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.NormalHoursWorked, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.HRHoursWorked, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.ONHoursWorked, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.SCHoursWorked, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.TotalHoursWorked, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = DblParser(item.TruancyHours, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = item.TotalDaysWorked.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = item.HolidayWD.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = item.HolidayRD.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = item.SickDays.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = DblParser(item.StandartHours, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = item.StandartDays.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = DblParser(item.ConventionalWage, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = item.WageTypeHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(item.ConventionalExtraPay, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(item.BonusPayQuarterly, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(item.BonusPayAnnual, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(item.OtherPayRelatedToWork, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(item.OtherPayNotRelatedToWork, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(item.PayOutWage, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(item.PayOutExtraPay, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(item.PayOutHR, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(item.PayOutON, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(item.PayOutSC, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(item.PayOutSickLeave, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(item.UnusedHolidayDaysForCompensation, ROUNDACCUMULATEDHOLIDAY)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(item.PayOutHoliday, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(item.PayOutUnusedHolidayCompensation, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(item.PayOutRedundancyPay, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(item.PayOutTotal, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(item.DeductionGPM, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(item.DeductionPSD + item.DeductionPSDSickLeave, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(item.DeductionPSDSickLeave, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = DblParser(item.DeductionSODRA, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = DblParser(item.DeductionOther, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = DblParser(item.ContributionSODRA, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column53 = DblParser(item.ContributionPSD, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column54 = DblParser(item.ContributionGuaranteeFund, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column55 = DblParser(item.PayableTotal, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column56 = DblParser(item.NPD, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column57 = DblParser(item.PNPD, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column58 = DblParser(item.HoursForVDU, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column59 = item.DaysForVDU.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column60 = DblParser(item.WageForVDU, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column61 = DblParser(item.PayedOutTotalSum, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column62 = DblParser(item.DeductionGPM _
                     + item.DeductionPSD + item.DeductionPSDSickLeave + item.DeductionSODRA _
                     + item.DeductionOther, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column63 = DblParser(item.ContributionSODRA _
                     + item.ContributionPSD + item.ContributionGuaranteeFund, 2)

                debt = CRound(debt + item.PayOutTotal - item.PayedOutTotalSum, 2)

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column64 = DblParser(debt, 2)

                normalHoursWorked = CRound(normalHoursWorked + item.NormalHoursWorked, ROUNDWORKHOURS)
                hrHoursWorked = CRound(hrHoursWorked + item.HRHoursWorked, ROUNDWORKHOURS)
                onHoursWorked = CRound(onHoursWorked + item.ONHoursWorked, ROUNDWORKHOURS)
                scHoursWorked = CRound(scHoursWorked + item.SCHoursWorked, ROUNDWORKHOURS)
                totalHoursWorked = CRound(totalHoursWorked + item.TotalHoursWorked, ROUNDWORKHOURS)
                truancyHours = CRound(truancyHours + item.TruancyHours, ROUNDWORKHOURS)
                totalDaysWorked = totalDaysWorked + item.TotalDaysWorked
                holidayWD = holidayWD + item.HolidayWD
                holidayRD = holidayRD + item.HolidayRD
                sickDays = sickDays + item.SickDays
                standartHours = CRound(standartHours + item.StandartHours, ROUNDWORKHOURS)
                standartDays = standartDays + item.StandartDays
                bonusPayQuarterly = CRound(bonusPayQuarterly + item.BonusPayQuarterly, 2)
                bonusPayAnnual = CRound(bonusPayAnnual + item.BonusPayAnnual, 2)
                otherPayRelatedToWork = CRound(otherPayRelatedToWork + item.OtherPayRelatedToWork, 2)
                otherPayNotRelatedToWork = CRound(otherPayNotRelatedToWork + item.OtherPayNotRelatedToWork, 2)
                payOutWage = CRound(payOutWage + item.PayOutWage, 2)
                payOutExtraPay = CRound(payOutExtraPay + item.PayOutExtraPay, 2)
                payOutHR = CRound(payOutHR + item.PayOutHR, 2)
                payOutON = CRound(payOutON + item.PayOutON, 2)
                payOutSC = CRound(payOutSC + item.PayOutSC, 2)
                payOutSickLeave = CRound(payOutSickLeave + item.PayOutSickLeave, 2)
                payOutHoliday = CRound(payOutHoliday + item.PayOutHoliday, 2)
                payOutTotal = CRound(payOutTotal + item.PayOutTotal, 2)
                deductionGPM = CRound(deductionGPM + item.DeductionGPM, 2)
                deductionPSD = CRound(deductionPSD + item.DeductionPSD, 2)
                deductionSODRA = CRound(deductionSODRA + item.DeductionSODRA, 2)
                deductionOther = CRound(deductionOther + item.DeductionOther, 2)
                contributionSODRA = CRound(contributionSODRA + item.ContributionSODRA, 2)
                contributionPSD = CRound(contributionPSD + item.ContributionPSD, 2)
                contributionGuaranteeFund = CRound(contributionGuaranteeFund + item.ContributionGuaranteeFund, 2)
                payableTotal = CRound(payableTotal + item.PayableTotal, 2)
                npd = CRound(npd + item.NPD, 2)
                pnpd = CRound(pnpd + item.PNPD, 2)
                payedOutTotalSum = CRound(payedOutTotalSum + item.PayedOutTotalSum, 2)
                totalDeductions = CRound(totalDeductions + item.DeductionGPM _
                    + item.DeductionPSD + item.DeductionPSDSickLeave + item.DeductionSODRA _
                    + item.DeductionOther, 2)
                totalContributions = CRound(totalContributions + item.ContributionSODRA _
                    + item.ContributionPSD + item.ContributionGuaranteeFund, 2)

            Next

            rd.TableGeneral.Item(0).Column12 = DblParser(normalHoursWorked, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column13 = DblParser(hrHoursWorked, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column14 = DblParser(onHoursWorked, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column15 = DblParser(scHoursWorked, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column16 = DblParser(totalHoursWorked, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column17 = DblParser(truancyHours, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column18 = totalDaysWorked.ToString()
            rd.TableGeneral.Item(0).Column19 = holidayWD.ToString()
            rd.TableGeneral.Item(0).Column20 = holidayRD.ToString()
            rd.TableGeneral.Item(0).Column21 = sickDays.ToString()
            rd.TableGeneral.Item(0).Column22 = DblParser(standartHours, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column23 = standartDays.ToString()
            rd.TableGeneral.Item(0).Column24 = DblParser(bonusPayQuarterly, 2)
            rd.TableGeneral.Item(0).Column25 = DblParser(bonusPayAnnual, 2)
            rd.TableGeneral.Item(0).Column26 = DblParser(otherPayRelatedToWork, 2)
            rd.TableGeneral.Item(0).Column27 = DblParser(otherPayNotRelatedToWork, 2)
            rd.TableGeneral.Item(0).Column28 = DblParser(payOutWage, 2)
            rd.TableGeneral.Item(0).Column29 = DblParser(payOutExtraPay, 2)
            rd.TableGeneral.Item(0).Column30 = DblParser(payOutHR, 2)
            rd.TableGeneral.Item(0).Column31 = DblParser(payOutON, 2)
            rd.TableGeneral.Item(0).Column32 = DblParser(payOutSC, 2)
            rd.TableGeneral.Item(0).Column33 = DblParser(payOutSickLeave, 2)
            rd.TableGeneral.Item(0).Column34 = DblParser(payOutHoliday, 2)
            rd.TableGeneral.Item(0).Column35 = DblParser(payOutTotal, 2)
            rd.TableGeneral.Item(0).Column36 = DblParser(deductionGPM, 2)
            rd.TableGeneral.Item(0).Column37 = DblParser(deductionPSD, 2)
            rd.TableGeneral.Item(0).Column38 = DblParser(deductionSODRA, 2)
            rd.TableGeneral.Item(0).Column39 = DblParser(deductionOther, 2)
            rd.TableGeneral.Item(0).Column40 = DblParser(contributionSODRA, 2)
            rd.TableGeneral.Item(0).Column41 = DblParser(contributionPSD, 2)
            rd.TableGeneral.Item(0).Column42 = DblParser(contributionGuaranteeFund, 2)
            rd.TableGeneral.Item(0).Column43 = DblParser(payableTotal, 2)
            rd.TableGeneral.Item(0).Column44 = DblParser(npd, 2)
            rd.TableGeneral.Item(0).Column45 = DblParser(pnpd, 2)
            rd.TableGeneral.Item(0).Column46 = DblParser(payedOutTotalSum, 2)
            rd.TableGeneral.Item(0).Column47 = DblParser(totalDeductions, 2)
            rd.TableGeneral.Item(0).Column48 = DblParser(totalContributions, 2)
            rd.TableGeneral.Item(0).Column49 = filterDescription

            ReportFileName = "R_WorkerWageInfoReport.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Workers.WageSheet">WageSheet</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Workers.WageSheet, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Date.Year.ToString
            rd.TableGeneral.Item(0).Column3 = GetLithuanianMonth(R_Obj.Date.Month)
            rd.TableGeneral.Item(0).Column4 = R_Obj.Date.Day.ToString
            rd.TableGeneral.Item(0).Column5 = R_Obj.Year.ToString
            rd.TableGeneral.Item(0).Column6 = GetLithuanianMonth(R_Obj.Month)
            rd.TableGeneral.Item(0).Column7 = R_Obj.Number.ToString
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.TotalSum)
            rd.TableGeneral.Item(0).Column9 = DblParser(R_Obj.TotalSumAfterDeductions)
            rd.TableGeneral.Item(0).Column10 = SumLT(R_Obj.TotalSumAfterDeductions, 0, _
                 True, GetCurrentCompany.BaseCurrency)
            rd.TableGeneral.Item(0).Column11 = Convert.ToInt32(Math.Floor(R_Obj.TotalSumAfterDeductions)).ToString
            rd.TableGeneral.Item(0).Column12 = DblParser((R_Obj.TotalSumAfterDeductions - _
                 Math.Floor(R_Obj.TotalSumAfterDeductions)) * 100, 0)
            rd.TableGeneral.Item(0).Column13 = R_Obj.CostAccount.ToString
            If R_Obj.IsNonClosing Then
                rd.TableGeneral.Item(0).Column15 = "Taip"
            Else
                rd.TableGeneral.Item(0).Column15 = "Ne"
            End If
            rd.TableGeneral.Item(0).Column16 = R_Obj.Remarks
            If Not R_Obj.WageRates Is Nothing Then
                rd.TableGeneral.Item(0).Column17 = DblParser(R_Obj.WageRates.RateHR)
                rd.TableGeneral.Item(0).Column18 = DblParser(R_Obj.WageRates.RateON)
                rd.TableGeneral.Item(0).Column19 = DblParser(R_Obj.WageRates.RateSC)
                rd.TableGeneral.Item(0).Column20 = DblParser(R_Obj.WageRates.RateSickLeave)
                rd.TableGeneral.Item(0).Column21 = DblParser(R_Obj.WageRates.RateGPM)
                rd.TableGeneral.Item(0).Column22 = DblParser(R_Obj.WageRates.RatePSDEmployee)
                rd.TableGeneral.Item(0).Column23 = DblParser(R_Obj.WageRates.RateSODRAEmployee)
                rd.TableGeneral.Item(0).Column24 = DblParser(R_Obj.WageRates.RatePSDEmployer)
                rd.TableGeneral.Item(0).Column25 = DblParser(R_Obj.WageRates.RateSODRAEmployer)
                rd.TableGeneral.Item(0).Column26 = DblParser(R_Obj.WageRates.RateGuaranteeFund)
                rd.TableGeneral.Item(0).Column27 = R_Obj.WageRates.NPDFormula.Trim
            Else
                rd.TableGeneral.Item(0).Column17 = "NaN"
                rd.TableGeneral.Item(0).Column18 = "NaN"
                rd.TableGeneral.Item(0).Column19 = "NaN"
                rd.TableGeneral.Item(0).Column20 = "NaN"
                rd.TableGeneral.Item(0).Column21 = "NaN"
                rd.TableGeneral.Item(0).Column22 = "NaN"
                rd.TableGeneral.Item(0).Column23 = "NaN"
                rd.TableGeneral.Item(0).Column24 = "NaN"
                rd.TableGeneral.Item(0).Column25 = "NaN"
                rd.TableGeneral.Item(0).Column26 = "NaN"
                rd.TableGeneral.Item(0).Column27 = ""
            End If
            rd.TableGeneral.Item(0).Column28 = filterDescription

            For Each item As Workers.WageItem In GetDisplayedList(Of Workers.WageItem) _
                (R_Obj.Items, displayIndexes, 0)
                If item.IsChecked Then
                    rd.Table1.Rows.Add()
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.PersonName
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.PersonCode
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.PersonCodeSODRA
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.ContractSerial.Trim & _
                                                                       item.ContractNumber.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.WorkLoad, 3)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.ConventionalWage)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.WageTypeHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.ConventionalExtraPay)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.ApplicableVDUDaily)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.ApplicableVDUHourly)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = item.StandartDays.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.StandartHours, 3)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = item.TotalDaysWorked.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.NormalHoursWorked, 3)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.HRHoursWorked, 3)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.ONHoursWorked, 3)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.SCHoursWorked, 3)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.TotalHoursWorked, 3)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.TruancyHours, 3)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = item.SickDays.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = item.HolidayWD.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = item.HolidayRD.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = _
                        DblParser(item.AnnualWorkingDaysRatio, 4)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = _
                        DblParser(item.UnusedHolidayDaysForCompensation)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(item.PayOutWage)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.PayOutExtraPay)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = DblParser(item.ActualHourlyPay)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = DblParser(item.PayOutHR)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = DblParser(item.PayOutON)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(item.PayOutSC)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(item.PayOutWage +
                        item.PayOutHR + item.PayOutON + item.PayOutSC)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(item.BonusPay)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = item.BonusType.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(item.OtherPayRelatedToWork)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(item.OtherPayNotRelatedToWork)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(item.OtherPayRelatedToWork +
                        item.OtherPayNotRelatedToWork)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(item.PayOutRedundancyPay)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(item.PayOutHoliday)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(item.PayOutUnusedHolidayCompensation)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(item.PayOutHoliday + _
                                                                                  item.PayOutUnusedHolidayCompensation)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(item.PayOutSickLeave)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(item.PayOutTotal)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(item.NPD + item.NpdSickLeave)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(item.PNPD)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(item.DeductionGPM + item.DeductedGpmSickLeave)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(item.DeductionPSD)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(item.DeductionPSDSickLeave)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(item.DeductionPSD + _
                                                                                  item.DeductionPSDSickLeave)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(item.DeductionSODRA)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = DblParser(item.PayOutTotalAfterTaxes)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = DblParser(item.DeductionImprest)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = DblParser(item.DeductionOtherApplicable)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column53 = DblParser(item.PayOutTotalAfterDeductions)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column54 = DblParser(item.ContributionSODRA)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column55 = DblParser(item.ContributionPSD)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column56 = DblParser(item.ContributionGuaranteeFund)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column57 = DblParser(item.DaysForVDU)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column58 = DblParser(item.HoursForVDU)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column59 = DblParser(item.WageForVDU)
                    If item.UnusedHolidayDaysForCompensation > 0 Then
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column60 = "X"
                    Else
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column60 = ""
                    End If
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column61 = DblParser(item.DeductionPSD + _
                          item.DeductionPSDSickLeave + item.DeductionSODRA)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column62 = DblParser(item.ContributionSODRA + _
                          item.ContributionPSD)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column63 = DblParser(item.DeductionPSD + _
                          item.DeductionPSDSickLeave + item.DeductionSODRA + item.DeductionGPM + _
                          item.DeductionImprest + item.DeductionOtherApplicable)

                End If
            Next

            If Version = 0 Then
                ReportFileName = "R_WageSheet(1).rdlc"
            ElseIf Version = 1 Then
                ReportFileName = "R_WageSheet(2).rdlc"
            ElseIf Version = 2 Then
                ReportFileName = "R_PayChecks.rdlc"
            Else
                Throw New NotSupportedException("Klaida. Darbo užmokesčio žiniaraščio spausdinamos formos 3 versijos dar nenupiešiau.")
            End If
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.WorkTimeSheetInfoList">WorkTimeSheetInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.WorkTimeSheetInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            rd.TableGeneral.Item(0).Column3 = filterDescription

            For Each item As ActiveReports.WorkTimeSheetInfo In GetDisplayedList(Of ActiveReports.WorkTimeSheetInfo) _
                (R_Obj, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Date.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Year.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Month.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.SubDivision
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.WorkersCount.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.TotalWorkDays.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.TotalWorkTime, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.NightWork, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.OvertimeWork, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.PublicHolidaysAndRestDayWork, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.UnusualWork, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.Truancy, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.DownTime, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = item.AnnualHolidays.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = item.OtherHolidays.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = item.SickDays.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.OtherIncluded, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.OtherExcluded, ROUNDWORKHOURS)

            Next

            ReportFileName = "R_WorkTimeSheetInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Workers.Contract">Contract</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Workers.Contract, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Serial
            rd.TableGeneral.Item(0).Column3 = R_Obj.Number.ToString
            rd.TableGeneral.Item(0).Column4 = R_Obj.Position
            rd.TableGeneral.Item(0).Column5 = R_Obj.Content
            rd.TableGeneral.Item(0).Column6 = DblParser(R_Obj.WorkLoad, 3)
            rd.TableGeneral.Item(0).Column7 = R_Obj.HumanReadableWageType
            rd.TableGeneral.Item(0).Column8 = DblParser(R_Obj.Wage, 2)
            rd.TableGeneral.Item(0).Column9 = DblParser(R_Obj.ExtraPay, 2)
            rd.TableGeneral.Item(0).Column10 = R_Obj.AnnualHoliday.ToString
            rd.TableGeneral.Item(0).Column11 = DblParser(R_Obj.NPD)
            rd.TableGeneral.Item(0).Column12 = DblParser(R_Obj.PNPD)
            rd.TableGeneral.Item(0).Column13 = R_Obj.PersonName

            ReportFileName = "R_LabourContract.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Workers.ContractUpdate">ContractUpdate</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Workers.ContractUpdate, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Content
            rd.TableGeneral.Item(0).Column3 = R_Obj.Number.ToString
            rd.TableGeneral.Item(0).Column4 = R_Obj.Serial
            rd.TableGeneral.Item(0).Column5 = R_Obj.Content
            ' RD.TableGeneral.Item(0).Column6 = R_Obj.Value
            rd.TableGeneral.Item(0).Column7 = R_Obj.HumanReadableWageType
            rd.TableGeneral.Item(0).Column8 = R_Obj.PersonName
            rd.TableGeneral.Item(0).Column9 = R_Obj.HumanReadableWageType

            'If R_Obj.Type = Workers.WorkerStatusType.Employed Then
            '    Throw New NotImplementedException("Darbo sutarties spausdinimas neimplementuotas.")
            'ElseIf R_Obj.Type = Workers.WorkerStatusType.Fired Then
            '    Throw New NotImplementedException("Įsakymo dėl darbo sutarties nutraukimo spausdinimas neimplementuotas.")
            'ElseIf R_Obj.Type = Workers.WorkerStatusType.HolidayCorrection Then
            '    Throw New NotImplementedException("Atostogų korekcijos spausdinimas neimplementuotas.")
            'ElseIf R_Obj.Type = Workers.WorkerStatusType.Holiday Then
            '    ReportFileName = "R_LabourOrderOnHolidays.rdlc"
            'ElseIf R_Obj.Type = Workers.WorkerStatusType.ExtraPay _
            '    OrElse R_Obj.Type = Workers.WorkerStatusType.Wage _
            '    OrElse R_Obj.Type = Workers.WorkerStatusType.WorkLoad Then
            '    ReportFileName = "R_LabourContractAmendment.rdlc"
            'ElseIf R_Obj.Type = Workers.WorkerStatusType.NPD _
            '    OrElse R_Obj.Type = Workers.WorkerStatusType.PNPD Then
            '    ReportFileName = "R_MemorandumNPD.rdlc"
            'End If
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.WorkersVDUInfo">WorkersVDUInfo</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.WorkersVDUInfo, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = BooleanToCheckMark(R_Obj.IncludeCurrentMonth)
            rd.TableGeneral.Item(0).Column3 = R_Obj.StandartDaysForTheCurrentMonth.ToString()
            rd.TableGeneral.Item(0).Column4 = DblParser(R_Obj.StandartHoursForTheCurrentMonth, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column5 = R_Obj.PersonID.ToString()
            rd.TableGeneral.Item(0).Column6 = R_Obj.PersonName
            rd.TableGeneral.Item(0).Column7 = R_Obj.PersonCode
            rd.TableGeneral.Item(0).Column8 = R_Obj.PersonCodeSODRA
            rd.TableGeneral.Item(0).Column9 = R_Obj.ContractSerial
            rd.TableGeneral.Item(0).Column10 = R_Obj.ContractNumber.ToString
            rd.TableGeneral.Item(0).Column11 = R_Obj.Position
            rd.TableGeneral.Item(0).Column12 = DblParser(R_Obj.WorkLoad, ROUNDWORKLOAD)
            rd.TableGeneral.Item(0).Column13 = DblParser(R_Obj.ConventionalWage, 2)
            rd.TableGeneral.Item(0).Column14 = R_Obj.WageTypeHumanReadable
            rd.TableGeneral.Item(0).Column15 = DblParser(R_Obj.ConventionalExtraPay, 2)
            rd.TableGeneral.Item(0).Column16 = DblParser(R_Obj.ApplicableVDUDaily, 2)
            rd.TableGeneral.Item(0).Column17 = DblParser(R_Obj.ApplicableVDUHourly, 2)
            rd.TableGeneral.Item(0).Column18 = DblParser(R_Obj.TotalWage, 2)
            rd.TableGeneral.Item(0).Column19 = R_Obj.TotalWorkDays.ToString()
            rd.TableGeneral.Item(0).Column20 = DblParser(R_Obj.TotalWorkHours, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column21 = DblParser(R_Obj.WageVDUDaily, 2)
            rd.TableGeneral.Item(0).Column22 = DblParser(R_Obj.WageVDUHourly, 2)
            rd.TableGeneral.Item(0).Column23 = DblParser(R_Obj.BonusYearly, 2)
            rd.TableGeneral.Item(0).Column24 = DblParser(R_Obj.BonusQuarterly, 2)
            rd.TableGeneral.Item(0).Column25 = DblParser(R_Obj.BonusBase, 2)
            rd.TableGeneral.Item(0).Column26 = R_Obj.TotalScheduledDays.ToString()
            rd.TableGeneral.Item(0).Column27 = DblParser(R_Obj.TotalScheduledHours, ROUNDWORKHOURS)
            rd.TableGeneral.Item(0).Column28 = DblParser(R_Obj.BonusVDUDaily, 2)
            rd.TableGeneral.Item(0).Column29 = DblParser(R_Obj.BonusVDUHourly, 2)
            rd.TableGeneral.Item(0).Column30 = filterDescription

            For Each item As ActiveReports.WageVDUInfo In GetDisplayedList(Of ActiveReports.WageVDUInfo) _
                (R_Obj.WageList, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Year.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Month.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.ScheduledDays.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = DblParser(item.ScheduledHours, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.WorkDays.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.WorkHours, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.Wage, 2)

            Next

            For Each item As ActiveReports.BonusVDUInfo In GetDisplayedList(Of ActiveReports.BonusVDUInfo) _
                (R_Obj.BonusList, displayIndexes, 1)

                rd.Table2.Rows.Add()

                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = item.Year.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = item.Month.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = DblParser(item.BonusAmount, 2)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column4 = item.BonusTypeHumanReadable

            Next

            ReportFileName = "R_WorkersVDUInfo.rdlc"
            NumberOfTablesInUse = 2

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.WorkerHolidayInfo">WorkerHolidayInfo</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.WorkerHolidayInfo, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = BooleanToCheckMark(R_Obj.IsForCompensation)
            rd.TableGeneral.Item(0).Column3 = R_Obj.PersonID.ToString()
            rd.TableGeneral.Item(0).Column4 = R_Obj.PersonName
            rd.TableGeneral.Item(0).Column5 = R_Obj.PersonCode
            rd.TableGeneral.Item(0).Column6 = R_Obj.PersonCodeSodra
            rd.TableGeneral.Item(0).Column7 = R_Obj.ContractDate.ToShortDateString()
            rd.TableGeneral.Item(0).Column8 = R_Obj.ContractSerial
            rd.TableGeneral.Item(0).Column9 = R_Obj.ContractNumber.ToString()
            rd.TableGeneral.Item(0).Column10 = BooleanToCheckMark(R_Obj.ContractIsTerminated)
            rd.TableGeneral.Item(0).Column11 = R_Obj.ContractTerminationDate
            rd.TableGeneral.Item(0).Column12 = BooleanToCheckMark(R_Obj.CompensationIsGranted)
            rd.TableGeneral.Item(0).Column13 = R_Obj.Position
            rd.TableGeneral.Item(0).Column14 = R_Obj.HolidayRate.ToString()
            rd.TableGeneral.Item(0).Column15 = DblParser(R_Obj.WorkLoad, ROUNDWORKLOAD)
            rd.TableGeneral.Item(0).Column16 = R_Obj.TotalWorkPeriodInDays.ToString()
            rd.TableGeneral.Item(0).Column17 = DblParser(R_Obj.TotalWorkPeriodInYears, ROUNDWORKYEARS)
            rd.TableGeneral.Item(0).Column18 = DblParser(R_Obj.TotalCumulatedHolidayDays, ROUNDACCUMULATEDHOLIDAY)
            rd.TableGeneral.Item(0).Column19 = R_Obj.TotalHolidayDaysGranted.ToString()
            rd.TableGeneral.Item(0).Column20 = DblParser(R_Obj.TotalHolidayDaysCompensated, ROUNDACCUMULATEDHOLIDAY)
            rd.TableGeneral.Item(0).Column21 = DblParser(R_Obj.TotalHolidayDaysCorrection, ROUNDACCUMULATEDHOLIDAY)
            rd.TableGeneral.Item(0).Column22 = DblParser(R_Obj.TotalHolidayDaysUsed, ROUNDACCUMULATEDHOLIDAY)
            rd.TableGeneral.Item(0).Column23 = DblParser(R_Obj.TotalUnusedHolidayDays, ROUNDACCUMULATEDHOLIDAY)
            rd.TableGeneral.Item(0).Column24 = filterDescription

            For Each item As ActiveReports.HolidayCalculationPeriod In GetDisplayedList(Of ActiveReports.HolidayCalculationPeriod) _
                (R_Obj.HolidayCalculatedList, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.DateBegin.ToShortDateString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.DateEnd.ToShortDateString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.LengthDays.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = DblParser(item.LengthYears, ROUNDWORKYEARS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.CumulatedHolidayDaysPerPeriod, ROUNDACCUMULATEDHOLIDAY)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.HolidayRate.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.StatusDescription

            Next

            For Each item As ActiveReports.HolidaySpentItem In GetDisplayedList(Of ActiveReports.HolidaySpentItem) _
                (R_Obj.HolidaySpentList, displayIndexes, 1)

                rd.Table2.Rows.Add()

                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = item.TypeHumanReadable
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = item.DocumentID.ToString()
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = item.DocumentDate.ToShortDateString()
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column4 = item.DocumentNumber
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column5 = item.Spent.ToString()
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column6 = DblParser(item.Compensated, ROUNDACCUMULATEDHOLIDAY)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column7 = DblParser(item.Correction, ROUNDACCUMULATEDHOLIDAY)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column8 = DblParser(item.Total, ROUNDACCUMULATEDHOLIDAY)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column9 = item.DocumentContent

            Next

            ReportFileName = "R_WorkerHolidayInfo.rdlc"
            NumberOfTablesInUse = 2

        End Sub

        ''' <summary>
        ''' Map <see cref="WageSheetItem">WageSheetItem</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef RD As ReportData,
            ByVal R_Obj As WageSheetItem, ByRef ReportFileName As String,
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer,
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            RD.TableGeneral.Item(0).Column1 = R_Obj.Sheet.Date.ToShortDateString
            RD.TableGeneral.Item(0).Column2 = R_Obj.Sheet.Date.Year.ToString
            RD.TableGeneral.Item(0).Column3 = GetLithuanianMonth(R_Obj.Sheet.Date.Month)
            RD.TableGeneral.Item(0).Column4 = R_Obj.Sheet.Date.Day.ToString
            RD.TableGeneral.Item(0).Column5 = R_Obj.Sheet.Year.ToString
            RD.TableGeneral.Item(0).Column6 = GetLithuanianMonth(R_Obj.Sheet.Month)
            RD.TableGeneral.Item(0).Column7 = R_Obj.Sheet.Number.ToString
            RD.TableGeneral.Item(0).Column8 = DblParser(R_Obj.Sheet.TotalSum)
            RD.TableGeneral.Item(0).Column9 = DblParser(R_Obj.Sheet.TotalSumAfterDeductions)
            RD.TableGeneral.Item(0).Column10 = SumLT(R_Obj.Sheet.TotalSumAfterDeductions, 0,
                True, GetCurrentCompany.BaseCurrency)
            RD.TableGeneral.Item(0).Column11 = Convert.ToInt32(Math.Floor(R_Obj.Sheet.TotalSumAfterDeductions)).ToString
            RD.TableGeneral.Item(0).Column12 = DblParser((R_Obj.Sheet.TotalSumAfterDeductions -
                Math.Floor(R_Obj.Sheet.TotalSumAfterDeductions)) * 100, 0)
            RD.TableGeneral.Item(0).Column13 = R_Obj.Sheet.CostAccount.ToString
            If R_Obj.Sheet.IsNonClosing Then
                RD.TableGeneral.Item(0).Column15 = "Taip"
            Else
                RD.TableGeneral.Item(0).Column15 = "Ne"
            End If
            RD.TableGeneral.Item(0).Column16 = R_Obj.Sheet.Remarks
            If Not R_Obj.Sheet.WageRates Is Nothing Then
                RD.TableGeneral.Item(0).Column17 = DblParser(R_Obj.Sheet.WageRates.RateHR)
                RD.TableGeneral.Item(0).Column18 = DblParser(R_Obj.Sheet.WageRates.RateON)
                RD.TableGeneral.Item(0).Column19 = DblParser(R_Obj.Sheet.WageRates.RateSC)
                RD.TableGeneral.Item(0).Column20 = DblParser(R_Obj.Sheet.WageRates.RateSickLeave)
                RD.TableGeneral.Item(0).Column21 = DblParser(R_Obj.Sheet.WageRates.RateGPM)
                RD.TableGeneral.Item(0).Column22 = DblParser(R_Obj.Sheet.WageRates.RatePSDEmployee)
                RD.TableGeneral.Item(0).Column23 = DblParser(R_Obj.Sheet.WageRates.RateSODRAEmployee)
                RD.TableGeneral.Item(0).Column24 = DblParser(R_Obj.Sheet.WageRates.RatePSDEmployer)
                RD.TableGeneral.Item(0).Column25 = DblParser(R_Obj.Sheet.WageRates.RateSODRAEmployer)
                RD.TableGeneral.Item(0).Column26 = DblParser(R_Obj.Sheet.WageRates.RateGuaranteeFund)
                RD.TableGeneral.Item(0).Column27 = R_Obj.Sheet.WageRates.NPDFormula.Trim
            Else
                RD.TableGeneral.Item(0).Column17 = "NaN"
                RD.TableGeneral.Item(0).Column18 = "NaN"
                RD.TableGeneral.Item(0).Column19 = "NaN"
                RD.TableGeneral.Item(0).Column20 = "NaN"
                RD.TableGeneral.Item(0).Column21 = "NaN"
                RD.TableGeneral.Item(0).Column22 = "NaN"
                RD.TableGeneral.Item(0).Column23 = "NaN"
                RD.TableGeneral.Item(0).Column24 = "NaN"
                RD.TableGeneral.Item(0).Column25 = "NaN"
                RD.TableGeneral.Item(0).Column26 = "NaN"
                RD.TableGeneral.Item(0).Column27 = ""
            End If

            Dim item As Workers.WageItem = R_Obj.Item

            RD.Table1.Rows.Add()
            RD.Table1.Item(0).Column1 = item.PersonName
            RD.Table1.Item(0).Column2 = item.PersonCode
            RD.Table1.Item(0).Column3 = item.PersonCodeSODRA
            RD.Table1.Item(0).Column4 = item.ContractSerial.Trim &
                item.ContractNumber.ToString
            RD.Table1.Item(0).Column5 = DblParser(item.WorkLoad, 3)
            RD.Table1.Item(0).Column6 = DblParser(item.ConventionalWage)
            RD.Table1.Item(0).Column7 = item.WageTypeHumanReadable
            RD.Table1.Item(0).Column8 = DblParser(item.ConventionalExtraPay)
            RD.Table1.Item(0).Column9 = DblParser(item.ApplicableVDUDaily)
            RD.Table1.Item(0).Column10 = DblParser(item.ApplicableVDUHourly)
            RD.Table1.Item(0).Column11 = item.StandartDays.ToString
            RD.Table1.Item(0).Column12 = DblParser(item.StandartHours, 3)
            RD.Table1.Item(0).Column13 = item.TotalDaysWorked.ToString
            RD.Table1.Item(0).Column14 = DblParser(item.NormalHoursWorked, 3)
            RD.Table1.Item(0).Column15 = DblParser(item.HRHoursWorked, 3)
            RD.Table1.Item(0).Column16 = DblParser(item.ONHoursWorked, 3)
            RD.Table1.Item(0).Column17 = DblParser(item.SCHoursWorked, 3)
            RD.Table1.Item(0).Column18 = DblParser(item.TotalHoursWorked, 3)
            RD.Table1.Item(0).Column19 = DblParser(item.TruancyHours, 3)
            RD.Table1.Item(0).Column20 = item.SickDays.ToString
            RD.Table1.Item(0).Column21 = item.HolidayWD.ToString
            RD.Table1.Item(0).Column22 = item.HolidayRD.ToString
            RD.Table1.Item(0).Column23 =
                DblParser(item.AnnualWorkingDaysRatio, 4)
            RD.Table1.Item(0).Column24 =
                DblParser(item.UnusedHolidayDaysForCompensation)
            RD.Table1.Item(0).Column25 = DblParser(item.PayOutWage)
            RD.Table1.Item(0).Column26 = DblParser(item.PayOutExtraPay)
            RD.Table1.Item(0).Column27 = DblParser(item.ActualHourlyPay)
            RD.Table1.Item(0).Column28 = DblParser(item.PayOutHR)
            RD.Table1.Item(0).Column29 = DblParser(item.PayOutON)
            RD.Table1.Item(0).Column30 = DblParser(item.PayOutSC)
            RD.Table1.Item(0).Column31 = DblParser(item.PayOutWage +
                item.PayOutHR + item.PayOutON + item.PayOutSC)
            RD.Table1.Item(0).Column32 = DblParser(item.BonusPay)
            RD.Table1.Item(0).Column33 = item.BonusType.ToString
            RD.Table1.Item(0).Column34 = DblParser(item.OtherPayRelatedToWork)
            RD.Table1.Item(0).Column35 = DblParser(item.OtherPayNotRelatedToWork)
            RD.Table1.Item(0).Column36 = DblParser(item.OtherPayRelatedToWork +
                item.OtherPayNotRelatedToWork)
            RD.Table1.Item(0).Column37 = DblParser(item.PayOutRedundancyPay)
            RD.Table1.Item(0).Column38 = DblParser(item.PayOutHoliday)
            RD.Table1.Item(0).Column39 = DblParser(item.PayOutUnusedHolidayCompensation)
            RD.Table1.Item(0).Column40 = DblParser(item.PayOutHoliday +
                 item.PayOutUnusedHolidayCompensation)
            RD.Table1.Item(0).Column41 = DblParser(item.PayOutSickLeave)
            RD.Table1.Item(0).Column42 = DblParser(item.PayOutTotal)
            RD.Table1.Item(0).Column43 = DblParser(item.NPD + item.NpdSickLeave)
            RD.Table1.Item(0).Column44 = DblParser(item.PNPD)
            RD.Table1.Item(0).Column45 = DblParser(item.DeductionGPM + item.DeductedGpmSickLeave)
            RD.Table1.Item(0).Column46 = DblParser(item.DeductionPSD)
            RD.Table1.Item(0).Column47 = DblParser(item.DeductionPSDSickLeave)
            RD.Table1.Item(0).Column48 = DblParser(item.DeductionPSD +
                item.DeductionPSDSickLeave)
            RD.Table1.Item(0).Column49 = DblParser(item.DeductionSODRA)
            RD.Table1.Item(0).Column50 = DblParser(item.PayOutTotalAfterTaxes)
            RD.Table1.Item(0).Column51 = DblParser(item.DeductionImprest)
            RD.Table1.Item(0).Column52 = DblParser(item.DeductionOtherApplicable)
            RD.Table1.Item(0).Column53 = DblParser(item.PayOutTotalAfterDeductions)
            RD.Table1.Item(0).Column54 = DblParser(item.ContributionSODRA)
            RD.Table1.Item(0).Column55 = DblParser(item.ContributionPSD)
            RD.Table1.Item(0).Column56 = DblParser(item.ContributionGuaranteeFund)
            RD.Table1.Item(0).Column57 = DblParser(item.DaysForVDU)
            RD.Table1.Item(0).Column58 = DblParser(item.HoursForVDU)
            RD.Table1.Item(0).Column59 = DblParser(item.WageForVDU)
            If item.UnusedHolidayDaysForCompensation > 0 Then
                RD.Table1.Item(0).Column60 = "X"
            Else
                RD.Table1.Item(0).Column60 = ""
            End If
            RD.Table1.Item(0).Column61 = DblParser(item.DeductionPSD +
                item.DeductionPSDSickLeave + item.DeductionSODRA)
            RD.Table1.Item(0).Column62 = DblParser(item.ContributionSODRA +
                item.ContributionPSD)
            RD.Table1.Item(0).Column63 = DblParser(item.DeductionPSD +
                item.DeductionPSDSickLeave + item.DeductionSODRA + item.DeductionGPM +
                item.DeductionImprest + item.DeductionOtherApplicable)


            ReportFileName = "R_PayChecks.rdlc"

            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Workers.HolidayPayReserve">HolidayPayReserve</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Workers.HolidayPayReserve, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.ID.ToString()
            rd.TableGeneral.Item(0).Column2 = R_Obj.Date.ToShortDateString()
            rd.TableGeneral.Item(0).Column3 = R_Obj.Date.Year.ToString
            rd.TableGeneral.Item(0).Column4 = GetLithuanianMonth(R_Obj.Date.Month)
            rd.TableGeneral.Item(0).Column5 = R_Obj.Date.Day.ToString
            rd.TableGeneral.Item(0).Column6 = R_Obj.Number
            rd.TableGeneral.Item(0).Column7 = R_Obj.Content
            rd.TableGeneral.Item(0).Column9 = R_Obj.AccountCosts.ToString()
            rd.TableGeneral.Item(0).Column10 = R_Obj.AccountReserve.ToString()
            rd.TableGeneral.Item(0).Column11 = R_Obj.Comments
            rd.TableGeneral.Item(0).Column12 = R_Obj.InsertDate.ToString()
            rd.TableGeneral.Item(0).Column13 = R_Obj.UpdateDate.ToString()
            rd.TableGeneral.Item(0).Column14 = DblParser(R_Obj.TaxRate, 2)
            rd.TableGeneral.Item(0).Column15 = DblParser(R_Obj.TotalSumCurrent, 2)
            rd.TableGeneral.Item(0).Column16 = DblParser(R_Obj.TotalSumEvaluatedBeforeTaxes, 2)
            rd.TableGeneral.Item(0).Column17 = DblParser(R_Obj.TotalSumEvaluated, 2)
            rd.TableGeneral.Item(0).Column18 = DblParser(R_Obj.TotalSumChange, 2)
            rd.TableGeneral.Item(0).Column19 = filterDescription

            For Each item As Workers.HolidayPayReserveItem In GetDisplayedList(Of Workers.HolidayPayReserveItem) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.PersonID.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.PersonName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.PersonCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.PersonCodeSodra
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.ContractDate.ToShortDateString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.ContractSerial
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.ContractNumber.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.ContractSerial & item.ContractNumber.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.Position
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.WorkLoad, ROUNDWORKLOAD)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.ConventionalWage, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = item.WageTypeHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.ConventionalExtraPay, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = item.HolidayRate.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = item.TotalWorkPeriodInDays.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.TotalWorkPeriodInYears, ROUNDWORKYEARS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.TotalCumulatedHolidayDays, ROUNDACCUMULATEDHOLIDAY)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = item.TotalHolidayDaysGranted.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.TotalHolidayDaysCorrection, ROUNDACCUMULATEDHOLIDAY)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.TotalHolidayDaysUsed, ROUNDACCUMULATEDHOLIDAY)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = DblParser(item.TotalUnusedHolidayDays, ROUNDACCUMULATEDHOLIDAY)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = item.TotalScheduledDays.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.TotalScheduledHours, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = item.StandartDaysForTheCurrentMonth.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.StandartHoursForTheCurrentMonth, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = item.TotalWorkDays.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = DblParser(item.TotalWorkHours, ROUNDWORKHOURS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = DblParser(item.TotalWage, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(item.BonusQuarterly, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(item.BonusYearly, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(item.BonusBase, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(item.TotalVDUDaily, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(item.TotalVDUHourly, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(item.ApplicableUnusedHolidayDays, ROUNDACCUMULATEDHOLIDAY)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(item.ApplicableVDUDaily, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(item.ApplicableWorkDaysRatio, ROUNDWORKDAYSRATIO)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(item.HolidayPayReserveValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = item.Comments

            Next

            ReportFileName = "R_HolidayPayReserve.rdlc"
            NumberOfTablesInUse = 1

        End Sub

#Region " WorkTimeSheet maping methods "

        ''' <summary>
        ''' Map <see cref="Workers.WorkTimeSheet">WorkTimeSheet</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Workers.WorkTimeSheet, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Year.ToString
            rd.TableGeneral.Item(0).Column3 = R_Obj.Month.ToString
            rd.TableGeneral.Item(0).Column4 = R_Obj.Number
            rd.TableGeneral.Item(0).Column5 = R_Obj.PreparedByName
            rd.TableGeneral.Item(0).Column6 = R_Obj.PreparedByPosition
            rd.TableGeneral.Item(0).Column7 = R_Obj.SignedByName
            rd.TableGeneral.Item(0).Column8 = R_Obj.SignedByPosition
            rd.TableGeneral.Item(0).Column9 = R_Obj.SubDivision
            rd.TableGeneral.Item(0).Column10 = R_Obj.GetTotalDays.ToString
            rd.TableGeneral.Item(0).Column11 = R_Obj.GetTotalHours.ToString
            rd.TableGeneral.Item(0).Column12 = R_Obj.GetTotalHoursByType(Workers.WorkTimeType.NightWork).ToString
            rd.TableGeneral.Item(0).Column13 = R_Obj.GetTotalHoursByType(Workers.WorkTimeType.OvertimeWork).ToString
            rd.TableGeneral.Item(0).Column14 = R_Obj.GetTotalHoursByType(Workers.WorkTimeType.UnusualWork).ToString
            rd.TableGeneral.Item(0).Column15 = R_Obj.GetTotalHoursByType(Workers.WorkTimeType.PublicHolidaysAndRestDayWork).ToString
            rd.TableGeneral.Item(0).Column16 = R_Obj.GetTotalAbsenceDays.ToString
            rd.TableGeneral.Item(0).Column17 = R_Obj.GetTotalAbsenceHours.ToString

            Dim maxDayCount As Integer = Date.DaysInMonth(R_Obj.Year, R_Obj.Month)
            Dim workerCount As Integer = 1
            For Each item As Workers.WorkTimeItem In R_Obj.GeneralItemListSorted

                If item.IsChecked Then

                    rd.Table1.Rows.Add()

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Worker
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.WorkerPosition
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.ContractSerial
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.ContractNumber.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.WorkerLoad, 3)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.QuotaDays.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.QuotaHours.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = GetDayString(item, 1, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = GetDayString(item, 2, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = GetDayString(item, 3, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = GetDayString(item, 4, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = GetDayString(item, 5, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = GetDayString(item, 6, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = GetDayString(item, 7, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = GetDayString(item, 8, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = GetDayString(item, 9, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = GetDayString(item, 10, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = GetDayString(item, 11, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = GetDayString(item, 12, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = GetDayString(item, 13, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = GetDayString(item, 14, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = GetDayString(item, 15, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = GetDayString(item, 16, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = GetDayString(item, 17, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = GetDayString(item, 18, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = GetDayString(item, 19, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = GetDayString(item, 20, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = GetDayString(item, 21, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = GetDayString(item, 22, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = GetDayString(item, 23, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = GetDayString(item, 24, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = GetDayString(item, 25, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = GetDayString(item, 26, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = GetDayString(item, 27, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = GetDayString(item, 28, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = GetDayString(item, 29, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = GetDayString(item, 30, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = GetDayString(item, 31, maxDayCount)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = item.TotalDays.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = item.TotalHours.ToString

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = ""

                    GetAgregateTimeForItem(item, R_Obj, rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41, _
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42, _
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43, _
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44, _
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45, _
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46, _
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47)

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = workerCount.ToString
                    workerCount += 1

                    For Each row As DataRow In GetDetailsDatatable(item, R_Obj).Rows

                        rd.Table1.Rows.Add()

                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = row(0).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = row(1).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = row(2).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = row(3).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = row(4).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = row(5).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = row(6).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = row(7).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = row(8).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = row(9).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = row(10).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = row(11).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = row(12).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = row(13).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = row(14).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = row(15).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = row(16).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = row(17).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = row(18).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = row(19).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = row(20).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = row(21).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = row(22).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = row(23).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = row(24).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = row(25).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = row(26).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = row(27).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = row(28).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = row(29).ToString
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = row(30).ToString

                    Next

                End If

            Next

            rd.TableGeneral.Item(0).Column18 = GetAggregateTable(R_Obj, rd).ToString

            ReportFileName = "R_WorkTimeSheet.rdlc"
            NumberOfTablesInUse = 2

        End Sub

        Private Function GetDayString(ByVal item As Workers.WorkTimeItem, _
                                      ByVal Day As Integer, ByVal maxDayCount As Integer) As String

            If Day < 1 Then Day = 1
            If Day > maxDayCount Then Return ""

            Try
                Dim Hours As Double = DirectCast(GetType(Workers.WorkTimeItem). _
                        GetProperty("Day" & Day.ToString).GetValue(item, Nothing), Double)
                Dim itemType As HelperLists.WorkTimeClassInfo = DirectCast(GetType(Workers.WorkTimeItem). _
                        GetProperty("DayType" & Day.ToString).GetValue(item, Nothing), HelperLists.WorkTimeClassInfo)
                If Not itemType Is Nothing AndAlso itemType.ID > 0 Then Return itemType.Code
                Return Hours.ToString
            Catch ex As Exception
                Return ""
            End Try

        End Function

        Private Function GetDetailsDatatable(ByVal item As Workers.WorkTimeItem, _
                                             ByVal sheet As Workers.WorkTimeSheet) As DataTable

            Dim result As New DataTable
            For i As Integer = 1 To 31
                result.Columns.Add()
            Next

            Dim maxDayCount As Integer = Date.DaysInMonth(sheet.Year, sheet.Month)

            For Each subitem As Workers.SpecialWorkTimeItem In sheet.SpecialItemList
                If subitem.WorkerID = item.WorkerID AndAlso subitem.ContractNumber = item.ContractNumber _
                   AndAlso subitem.ContractSerial.Trim.ToUpper = item.ContractSerial.Trim.ToUpper Then

                    result.Rows.Add(result.NewRow)

                    For i As Integer = 1 To 31
                        result.Rows(result.Rows.Count - 1).Item(i - 1) = _
                            GetSpecialDayString(subitem, i, maxDayCount)
                    Next

                End If
            Next

            For i As Integer = result.Rows.Count To 2 Step -1
                If MergeSpecialWorkTimeDataRows(result.Rows(i - 1), result.Rows(i - 2)) Then _
                    result.Rows.RemoveAt(i - 1)
            Next

            Return result

        End Function

        Private Function GetSpecialDayString(ByVal item As Workers.SpecialWorkTimeItem, _
                                             ByVal Day As Integer, ByVal maxDayCount As Integer) As String

            If Day < 1 Then Day = 1
            If Day > maxDayCount Then Return ""

            Try
                Dim Hours As Double = DirectCast(GetType(Workers.SpecialWorkTimeItem). _
                        GetProperty("Day" & Day.ToString).GetValue(item, Nothing), Double)
                If Not CRound(Hours, ROUNDWORKHOURS) > 0 Then Return ""
                Dim TimeClass As String = ""
                If Not item.Type Is Nothing AndAlso item.Type.ID > 0 Then TimeClass = item.Type.Code
                Return TimeClass & Hours.ToString
            Catch ex As Exception
                Return ""
            End Try

        End Function

        Private Function MergeSpecialWorkTimeDataRows(ByVal source As DataRow, _
                                                      ByRef target As DataRow) As Boolean

            For i As Integer = 1 To 31
                If Not String.IsNullOrEmpty(source(i - 1).ToString.Trim) AndAlso _
                   Not String.IsNullOrEmpty(target(i - 1).ToString.Trim) Then Return False
            Next

            For i As Integer = 1 To 31
                If Not String.IsNullOrEmpty(source(i - 1).ToString.Trim) Then _
                    target(i - 1) = source(i - 1).ToString.Trim
            Next

            Return True

        End Function

        Private Sub GetAgregateTimeForItem(ByVal item As Workers.WorkTimeItem, _
                                           ByVal sheet As Workers.WorkTimeSheet, ByRef NightTime As String, _
                                           ByRef Overtime As String, ByRef PublicHolidaysAndRestTime As String, _
                                           ByRef UnusualTime As String, ByRef AbsenceCode As String, _
                                           ByRef AbsenceDays As String, ByRef AbsenceHours As String)

            Dim cNightTime As Double = 0
            Dim cOvertime As Double = 0
            Dim cPublicHolidaysAndRestTime As Double = 0
            Dim cUnusualTime As Double = 0
            Dim cAbsenceCode As New List(Of String)
            Dim cAbsenceDays As Integer = 0
            Dim cAbsenceHours As Double = 0

            For Each subitem As Workers.SpecialWorkTimeItem In sheet.SpecialItemList
                If subitem.WorkerID = item.WorkerID AndAlso subitem.ContractNumber = item.ContractNumber _
                   AndAlso subitem.ContractSerial.Trim.ToUpper = item.ContractSerial.Trim.ToUpper Then

                    If Not subitem.Type Is Nothing AndAlso subitem.Type.ID > 0 Then

                        If subitem.Type.Type = Workers.WorkTimeType.AnnualHolidays OrElse _
                           subitem.Type.Type = Workers.WorkTimeType.DownTime OrElse _
                           subitem.Type.Type = Workers.WorkTimeType.OtherExcluded OrElse _
                           subitem.Type.Type = Workers.WorkTimeType.OtherHolidays OrElse _
                           subitem.Type.Type = Workers.WorkTimeType.SickDays OrElse _
                           subitem.Type.Type = Workers.WorkTimeType.Truancy Then

                            If Not cAbsenceCode.Contains(subitem.Type.Code.Trim.ToUpper) Then _
                                cAbsenceCode.Add(subitem.Type.Code.Trim.ToUpper)

                            If subitem.Type.WithoutWorkHours Then
                                cAbsenceDays += 1
                            Else
                                cAbsenceHours = CRound(cAbsenceHours + subitem.TotalHours, ROUNDWORKHOURS)
                            End If

                        ElseIf subitem.Type.Type = Workers.WorkTimeType.NightWork Then

                            cNightTime = CRound(cNightTime + subitem.TotalHours, ROUNDWORKHOURS)

                        ElseIf subitem.Type.Type = Workers.WorkTimeType.OvertimeWork Then

                            cOvertime = CRound(cOvertime + subitem.TotalHours, ROUNDWORKHOURS)

                        ElseIf subitem.Type.Type = Workers.WorkTimeType.PublicHolidaysAndRestDayWork Then

                            cPublicHolidaysAndRestTime = CRound(cPublicHolidaysAndRestTime _
                                                                + subitem.TotalHours, ROUNDWORKHOURS)

                        ElseIf subitem.Type.Type = Workers.WorkTimeType.UnusualWork Then

                            cUnusualTime = CRound(cUnusualTime + subitem.TotalHours, ROUNDWORKHOURS)

                        End If


                    End If

                End If

            Next

            Dim curType As HelperLists.WorkTimeClassInfo

            For i As Integer = 1 To 31

                curType = Nothing

                Try
                    curType = DirectCast(GetType(Workers.WorkTimeItem).GetProperty( _
                        "DayType" & i.ToString).GetValue(item, Nothing), HelperLists.WorkTimeClassInfo)
                Catch ex As Exception
                End Try

                If Not curType Is Nothing AndAlso curType.ID > 0 AndAlso _
                   curType.ID <> sheet.DefaultRestTimeClass.ID AndAlso _
                   curType.ID <> sheet.DefaultPublicHolidayTimeClass.ID AndAlso _
                   (curType.Type = Workers.WorkTimeType.AnnualHolidays OrElse _
                    curType.Type = Workers.WorkTimeType.DownTime OrElse _
                    curType.Type = Workers.WorkTimeType.OtherExcluded OrElse _
                    curType.Type = Workers.WorkTimeType.OtherHolidays OrElse _
                    curType.Type = Workers.WorkTimeType.SickDays OrElse _
                    curType.Type = Workers.WorkTimeType.Truancy) Then

                    If Not cAbsenceCode.Contains(curType.Code.Trim.ToUpper) Then _
                        cAbsenceCode.Add(curType.Code.Trim.ToUpper)
                    cAbsenceDays += 1

                End If

            Next

            NightTime = cNightTime.ToString
            Overtime = cOvertime.ToString
            PublicHolidaysAndRestTime = cPublicHolidaysAndRestTime.ToString
            UnusualTime = cUnusualTime.ToString
            AbsenceCode = String.Join(",", cAbsenceCode.ToArray)
            AbsenceDays = cAbsenceDays.ToString
            AbsenceHours = cAbsenceHours.ToString

        End Sub

        Private Function GetAggregateTable(ByVal sheet As Workers.WorkTimeSheet, _
                                           ByRef RD As ReportData) As Integer

            Dim resultDays As New Dictionary(Of String, Integer)
            Dim resultHours As New Dictionary(Of String, Double)

            For Each subitem As Workers.SpecialWorkTimeItem In sheet.SpecialItemList
                If Not subitem.Type Is Nothing AndAlso subitem.Type.ID > 0 Then
                    If subitem.Type.WithoutWorkHours Then
                        If resultDays.ContainsKey(subitem.Type.Code.Trim.ToUpper) Then
                            resultDays(subitem.Type.Code.Trim.ToUpper) = _
                                resultDays(subitem.Type.Code.Trim.ToUpper) + 1
                        Else
                            resultDays.Add(subitem.Type.Code.Trim.ToUpper, 1)
                        End If
                    Else
                        If resultHours.ContainsKey(subitem.Type.Code.Trim.ToUpper) Then
                            resultHours(subitem.Type.Code.Trim.ToUpper) = _
                                CRound(resultHours(subitem.Type.Code.Trim.ToUpper) _
                                       + subitem.TotalHours, ROUNDWORKHOURS)
                        Else
                            resultHours.Add(subitem.Type.Code.Trim.ToUpper, subitem.TotalHours)
                        End If
                    End If
                End If
            Next

            Dim curType As HelperLists.WorkTimeClassInfo

            For i As Integer = 1 To 31

                For Each item As Workers.WorkTimeItem In sheet.GeneralItemList

                    curType = Nothing

                    Try
                        curType = DirectCast(GetType(Workers.WorkTimeItem).GetProperty( _
                            "DayType" & i.ToString).GetValue(item, Nothing), HelperLists.WorkTimeClassInfo)
                    Catch ex As Exception
                    End Try

                    If Not curType Is Nothing AndAlso curType.ID > 0 AndAlso _
                       curType.ID <> sheet.DefaultPublicHolidayTimeClass.ID AndAlso _
                       curType.ID <> sheet.DefaultRestTimeClass.ID AndAlso _
                       (curType.Type = Workers.WorkTimeType.AnnualHolidays OrElse _
                        curType.Type = Workers.WorkTimeType.DownTime OrElse _
                        curType.Type = Workers.WorkTimeType.OtherExcluded OrElse _
                        curType.Type = Workers.WorkTimeType.OtherHolidays OrElse _
                        curType.Type = Workers.WorkTimeType.SickDays OrElse _
                        curType.Type = Workers.WorkTimeType.Truancy) Then

                        If resultDays.ContainsKey(curType.Code.Trim.ToUpper) Then
                            resultDays(curType.Code.Trim.ToUpper) = _
                                resultDays(curType.Code.Trim.ToUpper) + 1
                        Else
                            resultDays.Add(curType.Code.Trim.ToUpper, 1)
                        End If

                    End If

                Next

            Next

            Dim dt As New DataTable
            dt.Rows.Add()
            dt.Rows.Add()
            dt.Rows.Add()

            dt.Columns.Add()
            dt.Rows(0).Item(0) = "Tarnybinės komandiruotės ir neatvykimo į darbą atvejai per mėnesį"
            dt.Rows(1).Item(0) = "Dienos"
            dt.Rows(2).Item(0) = "Valandos"

            For Each k As KeyValuePair(Of String, Double) In resultHours

                dt.Columns.Add()
                dt.Rows(0).Item(dt.Columns.Count - 1) = k.Key
                dt.Rows(2).Item(dt.Columns.Count - 1) = k.Value.ToString

            Next

            For Each k As KeyValuePair(Of String, Integer) In resultDays

                dt.Columns.Add()
                dt.Rows(0).Item(dt.Columns.Count - 1) = k.Key
                dt.Rows(1).Item(dt.Columns.Count - 1) = k.Value.ToString

            Next

            CopyDataTableValues(dt, RD.Table2)

            Return dt.Columns.Count

        End Function

#End Region

#End Region

#Region "Goods"

        ''' <summary>
        ''' Map <see cref="Goods.GoodsComplexOperationDiscard">GoodsComplexOperationDiscard</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsComplexOperationDiscard, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Content
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentNumber
            If Not R_Obj.Warehouse Is Nothing AndAlso R_Obj.Warehouse.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.Warehouse.Name
                rd.TableGeneral.Item(0).Column5 = R_Obj.Warehouse.WarehouseAccount.ToString
            End If

            Dim totalAmount As Double = 0
            Dim totalSum As Double = 0

            For Each item As Goods.GoodsOperationDiscard In GetDisplayedList(Of Goods.GoodsOperationDiscard) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                If Not item.GoodsInfo Is Nothing AndAlso item.GoodsInfo.ID > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.GoodsInfo.ID.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.GoodsInfo.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.GoodsInfo.GroupName
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.GoodsInfo.MeasureUnit
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.GoodsInfo.AccountingMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.GoodsInfo.ValuationMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.GoodsInfo.PriceSale, ROUNDUNITGOODS)
                End If
                If item.UnitCost > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.UnitCost, ROUNDUNITGOODS)
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.Ammount, ROUNDAMOUNTGOODS)
                If item.TotalCost > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.TotalCost, 2)
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(0, ROUNDUNITGOODS) ' NormativeUnitValue
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(0, 2) ' NormativeTotalValue
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = item.AccountGoodsDiscardCosts.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = item.Description

                totalAmount = CRound(totalAmount + item.Ammount, ROUNDAMOUNTGOODS)
                totalSum = CRound(totalSum + item.TotalCost, 2)

            Next

            rd.TableGeneral.Item(0).Column6 = DblParser(totalAmount, ROUNDUNITGOODS)
            If CRound(totalSum, 2) > 0 Then
                rd.TableGeneral.Item(0).Column7 = DblParser(totalSum, 2)
            End If
            rd.TableGeneral.Item(0).Column8 = filterDescription


            ReportFileName = "R_GoodsComplexOperationDiscard.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsOperationDiscard">GoodsOperationDiscard</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsOperationDiscard, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentNumber
            If Not R_Obj.Warehouse Is Nothing AndAlso R_Obj.Warehouse.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.Warehouse.Name
                rd.TableGeneral.Item(0).Column5 = R_Obj.Warehouse.WarehouseAccount.ToString
            End If

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.ID.ToString
            If Not R_Obj.GoodsInfo Is Nothing AndAlso R_Obj.GoodsInfo.ID > 0 Then
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.GoodsInfo.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.GoodsInfo.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.GoodsInfo.GroupName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.GoodsInfo.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = R_Obj.GoodsInfo.AccountingMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.GoodsInfo.ValuationMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(R_Obj.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(R_Obj.GoodsInfo.PriceSale, ROUNDUNITGOODS)
            End If
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(R_Obj.UnitCost, ROUNDUNITGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(R_Obj.Ammount, ROUNDAMOUNTGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.TotalCost, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.NormativeUnitValue, ROUNDUNITGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(R_Obj.NormativeTotalValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = R_Obj.AccountGoodsDiscardCosts.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = R_Obj.Description

            ReportFileName = "R_GoodsOperationDiscard.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsComplexOperationInternalTransfer">GoodsComplexOperationInternalTransfer</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsComplexOperationInternalTransfer, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Content
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentNumber
            If Not R_Obj.WarehouseFrom Is Nothing AndAlso R_Obj.WarehouseFrom.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.WarehouseFrom.Name
                rd.TableGeneral.Item(0).Column5 = R_Obj.WarehouseFrom.WarehouseAccount.ToString
            End If
            If Not R_Obj.WarehouseTo Is Nothing AndAlso R_Obj.WarehouseTo.ID > 0 Then
                rd.TableGeneral.Item(0).Column6 = R_Obj.WarehouseTo.Name
                rd.TableGeneral.Item(0).Column7 = R_Obj.WarehouseTo.WarehouseAccount.ToString
            End If
            rd.TableGeneral.Item(0).Column8 = filterDescription

            For Each item As Goods.GoodsInternalTransferItem In GetDisplayedList(Of Goods.GoodsInternalTransferItem) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                If Not item.GoodsInfo Is Nothing AndAlso item.GoodsInfo.ID > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.GoodsInfo.ID.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.GoodsInfo.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.GoodsInfo.GroupName
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.GoodsInfo.MeasureUnit
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.GoodsInfo.AccountingMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.GoodsInfo.ValuationMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.GoodsInfo.PriceSale, ROUNDUNITGOODS)
                End If
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.UnitCost, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.Amount, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.TotalCost, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = item.Remarks

            Next

            ReportFileName = "R_GoodsOperationInternalTransfer.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsOperationTransfer">GoodsOperationTransfer</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsOperationTransfer, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description
            rd.TableGeneral.Item(0).Column3 = R_Obj.JournalEntryDocNo
            If Not R_Obj.Warehouse Is Nothing AndAlso R_Obj.Warehouse.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.Warehouse.Name
                rd.TableGeneral.Item(0).Column5 = R_Obj.Warehouse.WarehouseAccount.ToString
            End If
            rd.TableGeneral.Item(0).Column6 = R_Obj.JournalEntryDate.ToShortDateString
            rd.TableGeneral.Item(0).Column7 = R_Obj.JournalEntryTypeHumanReadable
            rd.TableGeneral.Item(0).Column8 = R_Obj.JournalEntryRelatedPerson
            rd.TableGeneral.Item(0).Column9 = R_Obj.JournalEntryContent
            rd.TableGeneral.Item(0).Column10 = R_Obj.JournalEntryCorrespondence

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.ID.ToString
            If Not R_Obj.GoodsInfo Is Nothing AndAlso R_Obj.GoodsInfo.ID > 0 Then
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.GoodsInfo.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.GoodsInfo.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.GoodsInfo.GroupName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.GoodsInfo.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = R_Obj.GoodsInfo.AccountingMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.GoodsInfo.ValuationMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(R_Obj.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(R_Obj.GoodsInfo.PriceSale, ROUNDUNITGOODS)
            End If
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(R_Obj.UnitCost, ROUNDUNITGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(R_Obj.Amount, ROUNDAMOUNTGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.TotalCost, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = R_Obj.AccountGoodsCost.ToString

            ReportFileName = "R_GoodsOperationTransfer.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsComplexOperationInventorization">GoodsComplexOperationInventorization</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsComplexOperationInventorization, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Content
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentNumber
            rd.TableGeneral.Item(0).Column4 = R_Obj.WarehouseName
            rd.TableGeneral.Item(0).Column5 = R_Obj.WarehouseAccount.ToString
            rd.TableGeneral.Item(0).Column6 = filterDescription

            For Each item As Goods.GoodsInventorizationItem In GetDisplayedList(Of Goods.GoodsInventorizationItem) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                If Not item.GoodsInfo Is Nothing AndAlso item.GoodsInfo.ID > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.GoodsInfo.ID.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.GoodsInfo.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.GoodsInfo.GroupName
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.GoodsInfo.MeasureUnit
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.GoodsInfo.AccountingMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.GoodsInfo.ValuationMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.GoodsInfo.PriceSale, ROUNDUNITGOODS)
                End If

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.UnitValueLastInventorization, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.AmountLastInventorization, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.TotalValueLastInventorization, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.AmountAcquisitions, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.TotalValueAcquisitions, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.AmountTransfered, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.TotalValueTransfered, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.AmountDisposed, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.TotalValueDisposed, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.TotalValueDiscount, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.TotalValueAdditionalCosts, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.UnitValueCalculated, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = DblParser(item.AmountCalculated, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.TotalValueCalculated, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.AmountChange, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = item.AccountCorresponding.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.AmountAfterInventorization, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = DblParser(item.TotalValueAfterInventorization, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = item.Remarks

            Next

            ReportFileName = "R_GoodsOperationInventorization.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsComplexOperationPriceCut">GoodsComplexOperationPriceCut</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsComplexOperationPriceCut, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentNumber
            rd.TableGeneral.Item(0).Column4 = filterDescription

            For Each item As Goods.GoodsOperationPriceCut In GetDisplayedList(Of Goods.GoodsOperationPriceCut) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                If Not item.GoodsInfo Is Nothing AndAlso item.GoodsInfo.ID > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.GoodsInfo.ID.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.GoodsInfo.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.GoodsInfo.GroupName
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.GoodsInfo.MeasureUnit
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.GoodsInfo.AccountingMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.GoodsInfo.ValuationMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.GoodsInfo.PriceSale, ROUNDUNITGOODS)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.GoodsInfo.AccountValueReduction.ToString
                End If

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.AmountInWarehouseAccounts, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.UnitValueInWarehouseAccounts, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.TotalValueInWarehouseAccounts, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.TotalValueCurrentPriceCut, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.UnitValuePriceCut, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.TotalValuePriceCut, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = item.AccountPriceCutCosts.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.UnitValueAfterPriceCut, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.TotalValueAfterPriceCut, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = item.Description

            Next

            ReportFileName = "R_GoodsOperationPriceCut.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsOperationPriceCut">GoodsOperationPriceCut</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsOperationPriceCut, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentNumber

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.ID.ToString
            If Not R_Obj.GoodsInfo Is Nothing AndAlso R_Obj.GoodsInfo.ID > 0 Then
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.GoodsInfo.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.GoodsInfo.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.GoodsInfo.GroupName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.GoodsInfo.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = R_Obj.GoodsInfo.AccountingMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.GoodsInfo.ValuationMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(R_Obj.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(R_Obj.GoodsInfo.PriceSale, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.GoodsInfo.AccountValueReduction.ToString
            End If

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(R_Obj.AmountInWarehouseAccounts, ROUNDAMOUNTGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.UnitValueInWarehouseAccounts, ROUNDUNITGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.TotalValueInWarehouseAccounts, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(R_Obj.TotalValueCurrentPriceCut, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(R_Obj.UnitValuePriceCut, ROUNDUNITGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(R_Obj.TotalValuePriceCut, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = R_Obj.AccountPriceCutCosts.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(R_Obj.UnitValueAfterPriceCut, ROUNDUNITGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.TotalValueAfterPriceCut, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = R_Obj.Description

            ReportFileName = "R_GoodsOperationPriceCut.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsComplexOperationProduction">GoodsComplexOperationProduction</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsComplexOperationProduction, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Content
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentNumber

            If Not R_Obj.GoodsInfo Is Nothing AndAlso R_Obj.GoodsInfo.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.GoodsInfo.ID.ToString
                rd.TableGeneral.Item(0).Column5 = R_Obj.GoodsInfo.Name
                rd.TableGeneral.Item(0).Column6 = R_Obj.GoodsInfo.GroupName
                rd.TableGeneral.Item(0).Column7 = R_Obj.GoodsInfo.MeasureUnit
                rd.TableGeneral.Item(0).Column8 = R_Obj.GoodsInfo.AccountingMethodHumanReadable
                rd.TableGeneral.Item(0).Column9 = R_Obj.GoodsInfo.ValuationMethodHumanReadable
                rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                rd.TableGeneral.Item(0).Column11 = DblParser(R_Obj.GoodsInfo.PriceSale, ROUNDUNITGOODS)
            End If

            rd.TableGeneral.Item(0).Column12 = DblParser(R_Obj.UnitValue, ROUNDUNITGOODS)
            rd.TableGeneral.Item(0).Column13 = DblParser(R_Obj.Amount, ROUNDAMOUNTGOODS)
            rd.TableGeneral.Item(0).Column14 = DblParser(R_Obj.TotalValue, 2)

            If Not R_Obj.WarehouseForProduction Is Nothing AndAlso R_Obj.WarehouseForProduction.ID > 0 Then
                rd.TableGeneral.Item(0).Column15 = R_Obj.WarehouseForProduction.Name
                rd.TableGeneral.Item(0).Column16 = R_Obj.WarehouseForProduction.WarehouseAccount.ToString
            End If
            If Not R_Obj.WarehouseForComponents Is Nothing AndAlso R_Obj.WarehouseForComponents.ID > 0 Then
                rd.TableGeneral.Item(0).Column17 = R_Obj.WarehouseForComponents.Name
                rd.TableGeneral.Item(0).Column18 = R_Obj.WarehouseForComponents.WarehouseAccount.ToString
            End If

            rd.TableGeneral.Item(0).Column19 = DblParser(R_Obj.CalculationIsPerUnit, ROUNDUNITGOODS)
            rd.TableGeneral.Item(0).Column20 = filterDescription

            For Each item As Goods.GoodsComponentItem In GetDisplayedList(Of Goods.GoodsComponentItem) _
                (R_Obj.ComponentItems, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                If Not item.GoodsInfo Is Nothing AndAlso item.GoodsInfo.ID > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.GoodsInfo.ID.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.GoodsInfo.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.GoodsInfo.GroupName
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.GoodsInfo.MeasureUnit
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.GoodsInfo.AccountingMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = item.GoodsInfo.ValuationMethodHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.GoodsInfo.PriceSale, ROUNDUNITGOODS)
                End If

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.AmountPerProductionUnit, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.Amount, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.NormativeUnitCost, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = item.AccountContrary.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.UnitCost, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.TotalCost, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = item.Remarks

            Next

            For Each item As Goods.GoodsProductionCostItem In GetDisplayedList(Of Goods.GoodsProductionCostItem) _
                (R_Obj.CostsItems, displayIndexes, 1)

                rd.Table2.Rows.Add()

                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = item.Account.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = DblParser(item.CostPerProductionUnit, ROUNDUNITGOODS)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = DblParser(item.TotalCost, 2)

            Next

            ReportFileName = "R_GoodsOperationProduction.rdlc"
            NumberOfTablesInUse = 2

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsOperationAdditionalCosts">GoodsOperationAdditionalCosts</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
           ByVal R_Obj As Goods.GoodsOperationAdditionalCosts, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())


            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description
            rd.TableGeneral.Item(0).Column3 = R_Obj.JournalEntryDocNo

            If Not R_Obj.GoodsInfo Is Nothing AndAlso R_Obj.GoodsInfo.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.GoodsInfo.ID.ToString
                rd.TableGeneral.Item(0).Column5 = R_Obj.GoodsInfo.Name
                rd.TableGeneral.Item(0).Column6 = R_Obj.GoodsInfo.GroupName
                rd.TableGeneral.Item(0).Column7 = R_Obj.GoodsInfo.MeasureUnit
                rd.TableGeneral.Item(0).Column8 = R_Obj.GoodsInfo.AccountingMethodHumanReadable
                rd.TableGeneral.Item(0).Column9 = R_Obj.GoodsInfo.ValuationMethodHumanReadable
                rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                rd.TableGeneral.Item(0).Column11 = DblParser(R_Obj.GoodsInfo.PriceSale, ROUNDUNITGOODS)
            End If

            If Not R_Obj.Warehouse Is Nothing AndAlso R_Obj.Warehouse.ID > 0 Then
                rd.TableGeneral.Item(0).Column12 = R_Obj.Warehouse.Name
                rd.TableGeneral.Item(0).Column13 = R_Obj.Warehouse.WarehouseAccount.ToString
            End If

            rd.TableGeneral.Item(0).Column14 = DblParser(R_Obj.TotalGoodsValueChange, 2)
            rd.TableGeneral.Item(0).Column15 = DblParser(R_Obj.TotalNetValueChange, 2)
            rd.TableGeneral.Item(0).Column16 = DblParser(R_Obj.TotalValueChange, 2)
            rd.TableGeneral.Item(0).Column17 = BooleanToCheckMark(R_Obj.AccountPurchasesIsClosed)
            rd.TableGeneral.Item(0).Column18 = R_Obj.AccountGoodsGeneral.ToString
            rd.TableGeneral.Item(0).Column19 = R_Obj.AccountGoodsNetCosts.ToString
            rd.TableGeneral.Item(0).Column20 = R_Obj.JournalEntryTypeHumanReadable.ToString
            rd.TableGeneral.Item(0).Column21 = R_Obj.JournalEntryContent.ToString
            rd.TableGeneral.Item(0).Column22 = R_Obj.JournalEntryCorrespondence.ToString
            rd.TableGeneral.Item(0).Column23 = R_Obj.JournalEntryRelatedPerson.ToString
            rd.TableGeneral.Item(0).Column24 = filterDescription

            For Each item As Goods.ConsignmentItem In GetDisplayedList(Of Goods.ConsignmentItem) _
                (R_Obj.Consignments, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.AcquisitionID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.AcquisitionDate.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.AcquisitionDocTypeHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.AcquisitionDocNo
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.Amount, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.UnitValue, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.TotalValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.AmountWithdrawn, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.TotalValueWithdrawn, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.AmountLeft, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.TotalValueLeft, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.UnitValueChange, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.TotalValueChange, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = item.WarehouseName

            Next

            ReportFileName = "R_GoodsOperationAdditionalCosts.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsOperationDiscount">GoodsOperationDiscount</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsOperationDiscount, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description
            rd.TableGeneral.Item(0).Column3 = R_Obj.JournalEntryDocNo

            If Not R_Obj.GoodsInfo Is Nothing AndAlso R_Obj.GoodsInfo.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.GoodsInfo.ID.ToString
                rd.TableGeneral.Item(0).Column5 = R_Obj.GoodsInfo.Name
                rd.TableGeneral.Item(0).Column6 = R_Obj.GoodsInfo.GroupName
                rd.TableGeneral.Item(0).Column7 = R_Obj.GoodsInfo.MeasureUnit
                rd.TableGeneral.Item(0).Column8 = R_Obj.GoodsInfo.AccountingMethodHumanReadable
                rd.TableGeneral.Item(0).Column9 = R_Obj.GoodsInfo.ValuationMethodHumanReadable
                rd.TableGeneral.Item(0).Column10 = DblParser(R_Obj.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                rd.TableGeneral.Item(0).Column11 = DblParser(R_Obj.GoodsInfo.PriceSale, ROUNDUNITGOODS)
            End If

            If Not R_Obj.Warehouse Is Nothing AndAlso R_Obj.Warehouse.ID > 0 Then
                rd.TableGeneral.Item(0).Column12 = R_Obj.Warehouse.Name
                rd.TableGeneral.Item(0).Column13 = R_Obj.Warehouse.WarehouseAccount.ToString
            End If

            rd.TableGeneral.Item(0).Column14 = DblParser(R_Obj.TotalGoodsValueChange, 2)
            rd.TableGeneral.Item(0).Column15 = DblParser(R_Obj.TotalNetValueChange, 2)
            rd.TableGeneral.Item(0).Column16 = DblParser(R_Obj.TotalValueChange, 2)
            rd.TableGeneral.Item(0).Column17 = BooleanToCheckMark(R_Obj.AccountPurchasesIsClosed)
            rd.TableGeneral.Item(0).Column18 = R_Obj.AccountGoodsGeneral.ToString
            rd.TableGeneral.Item(0).Column19 = R_Obj.AccountGoodsNetCosts.ToString
            rd.TableGeneral.Item(0).Column20 = R_Obj.JournalEntryTypeHumanReadable.ToString
            rd.TableGeneral.Item(0).Column21 = R_Obj.JournalEntryContent.ToString
            rd.TableGeneral.Item(0).Column22 = R_Obj.JournalEntryCorrespondence.ToString
            rd.TableGeneral.Item(0).Column23 = R_Obj.JournalEntryRelatedPerson.ToString
            rd.TableGeneral.Item(0).Column24 = filterDescription

            For Each item As Goods.ConsignmentItem In GetDisplayedList(Of Goods.ConsignmentItem) _
                (R_Obj.Consignments, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.AcquisitionID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.AcquisitionDate.ToShortDateString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.AcquisitionDocTypeHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.AcquisitionDocNo
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.Amount, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.UnitValue, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.TotalValue, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.AmountWithdrawn, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.TotalValueWithdrawn, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.AmountLeft, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.TotalValueLeft, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.UnitValueChange, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.TotalValueChange, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = item.WarehouseName

            Next

            ReportFileName = "R_GoodsOperationDiscount.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsOperationAccountChange">GoodsOperationAccountChange</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsOperationAccountChange, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description
            rd.TableGeneral.Item(0).Column3 = R_Obj.DocumentNumber

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.ID.ToString
            If Not R_Obj.GoodsInfo Is Nothing AndAlso R_Obj.GoodsInfo.ID > 0 Then
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.GoodsInfo.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.GoodsInfo.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.GoodsInfo.GroupName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.GoodsInfo.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = R_Obj.GoodsInfo.AccountingMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.GoodsInfo.ValuationMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(R_Obj.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(R_Obj.GoodsInfo.PriceSale, ROUNDUNITGOODS)
            End If
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.TypeHumanReadable
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.PreviousAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.CorrespondationValue, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = R_Obj.NewAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = R_Obj.Description

            ReportFileName = "R_GoodsOperationAccountChange.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsOperationValuationMethod">GoodsOperationValuationMethod</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsOperationValuationMethod, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.ID.ToString
            If Not R_Obj.GoodsInfo Is Nothing AndAlso R_Obj.GoodsInfo.ID > 0 Then
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.GoodsInfo.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.GoodsInfo.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.GoodsInfo.GroupName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.GoodsInfo.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = R_Obj.GoodsInfo.AccountingMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(R_Obj.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(R_Obj.GoodsInfo.PriceSale, ROUNDUNITGOODS)
            End If
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = R_Obj.PreviousMethodHumanReadable
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.NewMethodHumanReadable
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = R_Obj.Description

            ReportFileName = "R_GoodsOperationAccountChange.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.GoodsOperationAcquisition">GoodsOperationAcquisition</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.GoodsOperationAcquisition, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.JournalEntryDate.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description
            rd.TableGeneral.Item(0).Column3 = R_Obj.JournalEntryDocNo

            If Not R_Obj.Warehouse Is Nothing AndAlso R_Obj.Warehouse.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.Warehouse.Name
                rd.TableGeneral.Item(0).Column5 = R_Obj.Warehouse.WarehouseAccount.ToString
            End If

            rd.Table1.Rows.Add()

            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.ID.ToString
            If Not R_Obj.GoodsInfo Is Nothing AndAlso R_Obj.GoodsInfo.ID > 0 Then
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.GoodsInfo.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.GoodsInfo.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.GoodsInfo.GroupName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.GoodsInfo.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = R_Obj.GoodsInfo.AccountingMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = R_Obj.GoodsInfo.ValuationMethodHumanReadable
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(R_Obj.GoodsInfo.PricePurchase, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(R_Obj.GoodsInfo.PriceSale, ROUNDUNITGOODS)
            End If
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = R_Obj.AcquisitionAccount.ToString
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(R_Obj.UnitCost, ROUNDUNITGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.Ammount, ROUNDAMOUNTGOODS)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(R_Obj.TotalCost, 2)
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = R_Obj.JournalEntryTypeHumanReadable
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = R_Obj.JournalEntryContent
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = R_Obj.JournalEntryRelatedPerson
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = R_Obj.JournalEntryCorrespondence
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = R_Obj.JournalEntryCorrespondence
            rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = R_Obj.Description

            ReportFileName = "R_GoodsOperationAcquisition.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="Goods.ProductionCalculation">ProductionCalculation</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As Goods.ProductionCalculation, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.Date.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Description
            rd.TableGeneral.Item(0).Column3 = DblParser(R_Obj.Amount, ROUNDAMOUNTGOODS)

            If Not R_Obj.Goods Is Nothing AndAlso R_Obj.Goods.ID > 0 Then
                rd.TableGeneral.Item(0).Column4 = R_Obj.Goods.ID.ToString
                rd.TableGeneral.Item(0).Column5 = R_Obj.Goods.Name
                rd.TableGeneral.Item(0).Column6 = R_Obj.Goods.MeasureUnit
                rd.TableGeneral.Item(0).Column7 = R_Obj.Goods.GoodsCode
                rd.TableGeneral.Item(0).Column8 = R_Obj.Goods.GoodsBarcode
            End If

            rd.TableGeneral.Item(0).Column9 = BooleanToCheckMark(R_Obj.IsObsolete)
            rd.TableGeneral.Item(0).Column10 = filterDescription

            For Each item As Goods.ProductionComponentItem In GetDisplayedList(Of Goods.ProductionComponentItem) _
                (R_Obj.ComponentList, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                If Not item.Goods Is Nothing AndAlso item.Goods.ID > 0 Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Goods.ID.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Goods.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Goods.MeasureUnit
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.Goods.GoodsCode
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.Goods.GoodsBarcode
                End If

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.Amount, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.NormativeUnitCost, ROUNDUNITGOODS)

            Next

            For Each item As Goods.ProductionCostItem In GetDisplayedList(Of Goods.ProductionCostItem) _
                (R_Obj.CostList, displayIndexes, 1)

                rd.Table2.Rows.Add()

                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = item.ID.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = item.Account.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = DblParser(item.Amount, ROUNDUNITGOODS)

            Next

            ReportFileName = "R_GoodsProductionCalculation.rdlc"
            NumberOfTablesInUse = 2

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.GoodsOperationInfoListParent">GoodsOperationInfoListParent</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.GoodsOperationInfoListParent, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString

            If Not R_Obj.Warehouse Is Nothing AndAlso R_Obj.Warehouse.ID > 0 Then
                rd.TableGeneral.Item(0).Column3 = R_Obj.Warehouse.Name
                rd.TableGeneral.Item(0).Column4 = R_Obj.Warehouse.WarehouseAccount.ToString
            End If
            rd.TableGeneral.Item(0).Column5 = filterDescription

            rd.Table1.Rows.Add()

            If Not R_Obj.GoodsTurnoverInfo Is Nothing AndAlso R_Obj.GoodsTurnoverInfo.ID > 0 Then

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = R_Obj.GoodsTurnoverInfo.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = R_Obj.GoodsTurnoverInfo.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = R_Obj.GoodsTurnoverInfo.GroupName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = R_Obj.GoodsTurnoverInfo.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = R_Obj.GoodsTurnoverInfo.BarCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = R_Obj.GoodsTurnoverInfo.Code
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = BooleanToCheckMark(R_Obj.GoodsTurnoverInfo.IsObsolete)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = R_Obj.GoodsTurnoverInfo.TradeType
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(R_Obj.GoodsTurnoverInfo.DefaultVatRatePurchase, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(R_Obj.GoodsTurnoverInfo.DefaultVatRateSales, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(R_Obj.GoodsTurnoverInfo.PricePurchase, ROUNDAMOUNTINVOICERECEIVED)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(R_Obj.GoodsTurnoverInfo.PriceSale, ROUNDAMOUNTINVOICEMADE)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = R_Obj.GoodsTurnoverInfo.AccountDiscounts.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = R_Obj.GoodsTurnoverInfo.AccountPurchases.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = R_Obj.GoodsTurnoverInfo.AccountSalesNetCosts.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = R_Obj.GoodsTurnoverInfo.AccountValueReduction.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = R_Obj.GoodsTurnoverInfo.AccountingMethod
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = R_Obj.GoodsTurnoverInfo.ValuationMethod
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(R_Obj.GoodsTurnoverInfo.AmountPeriodStart, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(R_Obj.GoodsTurnoverInfo.UnitValuePeriodStart, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(R_Obj.GoodsTurnoverInfo.TotalValuePeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = DblParser(R_Obj.GoodsTurnoverInfo.AmountInWarehousePeriodStart, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(R_Obj.GoodsTurnoverInfo.UnitValueInWarehousePeriodStart, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(R_Obj.GoodsTurnoverInfo.TotalValueInWarehousePeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(R_Obj.GoodsTurnoverInfo.AmountPurchasesPeriodStart, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(R_Obj.GoodsTurnoverInfo.AmountPendingPeriodStart, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = DblParser(R_Obj.GoodsTurnoverInfo.AmountAcquisitions, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = DblParser(R_Obj.GoodsTurnoverInfo.AmountAcquisitionsInWarehouse, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = DblParser(R_Obj.GoodsTurnoverInfo.AmountDiscarded, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(R_Obj.GoodsTurnoverInfo.AmountDiscardedInWarehouse, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(R_Obj.GoodsTurnoverInfo.AmountTransfered, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(R_Obj.GoodsTurnoverInfo.AmountTransferedInWarehouse, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(R_Obj.GoodsTurnoverInfo.AmountChangeInventorization, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(R_Obj.GoodsTurnoverInfo.TotalAdditionalCosts, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(R_Obj.GoodsTurnoverInfo.TotalAdditionalCostsForDiscardedGoods, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(R_Obj.GoodsTurnoverInfo.TotalDiscounts, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(R_Obj.GoodsTurnoverInfo.TotalDiscountsForDiscardedGoods, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(R_Obj.GoodsTurnoverInfo.AmountPeriodEnd, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(R_Obj.GoodsTurnoverInfo.UnitValuePeriodEnd, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(R_Obj.GoodsTurnoverInfo.TotalValuePeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(R_Obj.GoodsTurnoverInfo.AmountInWarehousePeriodEnd, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(R_Obj.GoodsTurnoverInfo.UnitValueInWarehousePeriodEnd, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(R_Obj.GoodsTurnoverInfo.TotalValueInWarehousePeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(R_Obj.GoodsTurnoverInfo.AmountPurchasesPeriodEnd, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(R_Obj.GoodsTurnoverInfo.AmountPendingPeriodEnd, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(R_Obj.GoodsTurnoverInfo.AccountWarehousePeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(R_Obj.GoodsTurnoverInfo.AccountWarehouseDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(R_Obj.GoodsTurnoverInfo.AccountWarehouseCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(R_Obj.GoodsTurnoverInfo.AccountWarehousePeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = DblParser(R_Obj.GoodsTurnoverInfo.AccountPurchasesPeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = DblParser(R_Obj.GoodsTurnoverInfo.AccountPurchasesDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = DblParser(R_Obj.GoodsTurnoverInfo.AccountPurchasesCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column53 = DblParser(R_Obj.GoodsTurnoverInfo.AccountPurchasesPeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column54 = DblParser(R_Obj.GoodsTurnoverInfo.AccountSalesNetCostsPeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column55 = DblParser(R_Obj.GoodsTurnoverInfo.AccountSalesNetCostsDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column56 = DblParser(R_Obj.GoodsTurnoverInfo.AccountSalesNetCostsCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column57 = DblParser(R_Obj.GoodsTurnoverInfo.AccountSalesNetCostsPeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column58 = DblParser(R_Obj.GoodsTurnoverInfo.AccountDiscountsPeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column59 = DblParser(R_Obj.GoodsTurnoverInfo.AccountDiscountsDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column60 = DblParser(R_Obj.GoodsTurnoverInfo.AccountDiscountsCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column61 = DblParser(R_Obj.GoodsTurnoverInfo.AccountDiscountsPeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column62 = DblParser(R_Obj.GoodsTurnoverInfo.AccountValueReductionPeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column63 = DblParser(R_Obj.GoodsTurnoverInfo.AccountValueReductionDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column64 = DblParser(R_Obj.GoodsTurnoverInfo.AccountValueReductionCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column65 = DblParser(R_Obj.GoodsTurnoverInfo.AccountValueReductionPeriodEnd, 2)

            End If

            For Each item As ActiveReports.GoodsOperationInfo In GetDisplayedList(Of ActiveReports.GoodsOperationInfo) _
                (R_Obj.Items, displayIndexes, 0)

                rd.Table2.Rows.Add()

                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column1 = item.ID.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column2 = item.Type
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column3 = item.ComplexOperationID.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column4 = item.ComplexType
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column5 = item.Date.ToShortDateString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column6 = item.DocNo
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column7 = item.Content
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column8 = item.JournalEntryID.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column9 = item.JournalEntryType
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column10 = item.JournalEntryDate.ToShortDateString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column11 = item.JournalEntryDocNo
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column12 = item.JournalEntryContent
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column13 = item.JournalEntryCorrespondentions
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column14 = item.WarehouseName
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column15 = item.WarehouseAccount.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column16 = DblParser(item.Amount, ROUNDAMOUNTGOODS)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column17 = DblParser(item.AmountInWarehouse, ROUNDAMOUNTGOODS)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column18 = DblParser(item.UnitValue, ROUNDUNITGOODS)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column19 = DblParser(item.TotalValue, 2)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column20 = DblParser(item.AccountGeneral, 2)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column21 = DblParser(item.AccountPurchases, 2)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column22 = DblParser(item.AccountSalesNetCosts, 2)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column23 = DblParser(item.AccountDiscounts, 2)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column24 = DblParser(item.AccountPriceCut, 2)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column25 = item.AccountOperation.ToString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column26 = DblParser(item.AccountOperationValue, 2)
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column27 = item.InsertDate.ToShortDateString
                rd.Table2.Item(rd.Table2.Rows.Count - 1).Column28 = item.UpdateDate.ToShortDateString

            Next

            ReportFileName = "R_GoodsOperationInfoListParent.rdlc"
            NumberOfTablesInUse = 2

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.GoodsTurnoverInfoList">GoodsTurnoverInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.GoodsTurnoverInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString

            If Not R_Obj.Warehouse Is Nothing AndAlso R_Obj.Warehouse.ID > 0 Then
                rd.TableGeneral.Item(0).Column3 = R_Obj.Warehouse.Name
                rd.TableGeneral.Item(0).Column4 = R_Obj.Warehouse.WarehouseAccount.ToString
            Else
                rd.TableGeneral.Item(0).Column3 = "Visi sandėliai"
                rd.TableGeneral.Item(0).Column4 = ""
            End If

            If Not R_Obj.Group Is Nothing AndAlso R_Obj.Group.ID > 0 Then
                rd.TableGeneral.Item(0).Column5 = R_Obj.Group.Name
            Else
                rd.TableGeneral.Item(0).Column5 = "Visos grupės"
            End If
            rd.TableGeneral.Item(0).Column6 = filterDescription

            Dim balanceStart As Double = 0.0
            Dim gain As Double = 0.0
            Dim expanditure As Double = 0.0
            Dim balanceEnd As Double = 0.0


            For Each item As ActiveReports.GoodsTurnoverInfo In GetDisplayedList(Of ActiveReports.GoodsTurnoverInfo) _
                (R_Obj, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.GroupName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.MeasureUnit
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.BarCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.Code
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = BooleanToCheckMark(item.IsObsolete)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.TradeType
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.DefaultVatRatePurchase, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.DefaultVatRateSales, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.PricePurchase, ROUNDAMOUNTINVOICERECEIVED)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.PriceSale, ROUNDAMOUNTINVOICEMADE)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = item.AccountDiscounts.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = item.AccountPurchases.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = item.AccountSalesNetCosts.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = item.AccountValueReduction.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = item.AccountingMethod
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = item.ValuationMethod
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.AmountPeriodStart, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.UnitValuePeriodStart, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.TotalValuePeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column22 = DblParser(item.AmountInWarehousePeriodStart, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column23 = DblParser(item.UnitValueInWarehousePeriodStart, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column24 = DblParser(item.TotalValueInWarehousePeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column25 = DblParser(item.AmountPurchasesPeriodStart, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column26 = DblParser(item.AmountPendingPeriodStart, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column27 = DblParser(item.AmountAcquisitions, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column28 = DblParser(item.AmountAcquisitionsInWarehouse, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column29 = DblParser(item.AmountDiscarded, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column30 = DblParser(item.AmountDiscardedInWarehouse, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column31 = DblParser(item.AmountTransfered, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column32 = DblParser(item.AmountTransferedInWarehouse, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column33 = DblParser(item.AmountChangeInventorization, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column34 = DblParser(item.TotalAdditionalCosts, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column35 = DblParser(item.TotalAdditionalCostsForDiscardedGoods, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column36 = DblParser(item.TotalDiscounts, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column37 = DblParser(item.TotalDiscountsForDiscardedGoods, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column38 = DblParser(item.AmountPeriodEnd, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column39 = DblParser(item.UnitValuePeriodEnd, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column40 = DblParser(item.TotalValuePeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column41 = DblParser(item.AmountInWarehousePeriodEnd, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column42 = DblParser(item.UnitValueInWarehousePeriodEnd, ROUNDUNITGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column43 = DblParser(item.TotalValueInWarehousePeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column44 = DblParser(item.AmountPurchasesPeriodEnd, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column45 = DblParser(item.AmountPendingPeriodEnd, ROUNDAMOUNTGOODS)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column46 = DblParser(item.AccountWarehousePeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column47 = DblParser(item.AccountWarehouseDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column48 = DblParser(item.AccountWarehouseCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column49 = DblParser(item.AccountWarehousePeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column50 = DblParser(item.AccountPurchasesPeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column51 = DblParser(item.AccountPurchasesDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column52 = DblParser(item.AccountPurchasesCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column53 = DblParser(item.AccountPurchasesPeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column54 = DblParser(item.AccountSalesNetCostsPeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column55 = DblParser(item.AccountSalesNetCostsDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column56 = DblParser(item.AccountSalesNetCostsCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column57 = DblParser(item.AccountSalesNetCostsPeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column58 = DblParser(item.AccountDiscountsPeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column59 = DblParser(item.AccountDiscountsDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column60 = DblParser(item.AccountDiscountsCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column61 = DblParser(item.AccountDiscountsPeriodEnd, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column62 = DblParser(item.AccountValueReductionPeriodStart, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column63 = DblParser(item.AccountValueReductionDebit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column64 = DblParser(item.AccountValueReductionCredit, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column65 = DblParser(item.AccountValueReductionPeriodEnd, 2)

                If item.AccountingMethodInt = Goods.GoodsAccountingMethod.Periodic Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column66 = ""
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column67 = ""
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column66 = DblParser(item.AmountDiscarded _
                        + item.AmountTransfered, ROUNDAMOUNTGOODS)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column67 = DblParser(item.AccountWarehouseCredit, 2)
                End If
                If item.AccountingMethodInt = Goods.GoodsAccountingMethod.Periodic Then
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column68 = DblParser(item.AccountPurchasesDebit, 2)
                Else
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column68 = DblParser(item.AccountWarehouseDebit, 2)
                End If

                balanceStart = CRound(balanceStart + item.TotalValuePeriodStart)
                If item.AccountingMethodInt = Goods.GoodsAccountingMethod.Periodic Then
                    gain = CRound(gain + item.AccountPurchasesDebit)
                Else
                    gain = CRound(gain + item.AccountWarehouseDebit)
                    expanditure = CRound(expanditure + item.AccountWarehouseCredit)
                End If
                balanceEnd = CRound(balanceEnd + item.TotalValuePeriodEnd)

            Next

            rd.TableGeneral.Item(0).Column7 = DblParser(balanceStart, 2)
            rd.TableGeneral.Item(0).Column8 = DblParser(gain, 2)
            rd.TableGeneral.Item(0).Column9 = DblParser(expanditure, 2)
            rd.TableGeneral.Item(0).Column10 = DblParser(balanceEnd, 2)

            ReportFileName = "R_GoodsTurnoverInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

#End Region

        ''' <summary>
        ''' Map <see cref="ActiveReports.FinancialStatementsInfo">FinancialStatementsInfo</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.FinancialStatementsInfo, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.FirstPeriodDateStart.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.SecondPeriodDateStart.ToShortDateString
            rd.TableGeneral.Item(0).Column3 = R_Obj.SecondPeriodDateEnd.ToShortDateString
            rd.TableGeneral.Item(0).Column4 = R_Obj.ClosingSummaryAccount.ToString
            rd.TableGeneral.Item(0).Column5 = R_Obj.ClosingSummaryBalanceItem
            rd.TableGeneral.Item(0).Column6 = ConvertDateToWordsLT(R_Obj.SecondPeriodDateEnd)
            rd.TableGeneral.Item(0).Column7 = (Version - 1).ToString
            rd.TableGeneral.Item(0).Column8 = filterDescription


            If Version = 0 OrElse Version = 1 Then

                Dim totalDebitBalanceFormerPeriodStart As Double = 0
                Dim totalCreditBalanceFormerPeriodStart As Double = 0
                Dim totalDebitTurnoverFormerPeriod As Double = 0
                Dim totalCreditTurnoverFormerPeriod As Double = 0
                Dim totalDebitClosingFormerPeriod As Double = 0
                Dim totalCreditClosingFormerPeriod As Double = 0
                Dim totalDebitBalanceCurrentPeriodStart As Double = 0
                Dim totalCreditBalanceCurrentPeriodStart As Double = 0
                Dim totalDebitTurnoverCurrentPeriod As Double = 0
                Dim totalCreditTurnoverCurrentPeriod As Double = 0
                Dim totalDebitClosingCurrentPeriod As Double = 0
                Dim totalCreditClosingCurrentPeriod As Double = 0
                Dim totalDebitBalanceCurrentPeriodEnd As Double = 0
                Dim totalCreditBalanceCurrentPeriodEnd As Double = 0


                For Each item As ActiveReports.AccountTurnoverInfo In GetDisplayedList(Of ActiveReports.AccountTurnoverInfo) _
                    (R_Obj.AccountTurnoverList, displayIndexes, 0)

                    rd.Table1.Rows.Add()

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Name
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.FinancialStatementItem
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = DblParser(item.DebitBalanceFormerPeriodStart)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.CreditBalanceFormerPeriodStart)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = DblParser(item.DebitTurnoverFormerPeriod)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.CreditTurnoverFormerPeriod)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.DebitClosingFormerPeriod)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.CreditClosingFormerPeriod)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.DebitBalanceCurrentPeriodStart)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.CreditBalanceCurrentPeriodStart)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.DebitTurnoverCurrentPeriod)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.CreditTurnoverCurrentPeriod)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.DebitClosingCurrentPeriod)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.CreditClosingCurrentPeriod)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.DebitBalanceCurrentPeriodEnd)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.CreditBalanceCurrentPeriodEnd)

                    totalDebitBalanceFormerPeriodStart = CRound(totalDebitBalanceFormerPeriodStart + _
                        item.DebitBalanceFormerPeriodStart)
                    totalCreditBalanceFormerPeriodStart = CRound(totalCreditBalanceFormerPeriodStart + _
                        item.CreditBalanceFormerPeriodStart)
                    totalDebitTurnoverFormerPeriod = CRound(totalDebitTurnoverFormerPeriod + _
                        item.DebitTurnoverFormerPeriod)
                    totalCreditTurnoverFormerPeriod = CRound(totalCreditTurnoverFormerPeriod + _
                        item.CreditTurnoverFormerPeriod)
                    totalDebitClosingFormerPeriod = CRound(totalDebitClosingFormerPeriod + _
                        item.DebitClosingFormerPeriod)
                    totalCreditClosingFormerPeriod = CRound(totalCreditClosingFormerPeriod + _
                        item.CreditClosingFormerPeriod)
                    totalDebitBalanceCurrentPeriodStart = CRound(totalDebitBalanceCurrentPeriodStart + _
                        item.DebitBalanceCurrentPeriodStart)
                    totalCreditBalanceCurrentPeriodStart = CRound(totalCreditBalanceCurrentPeriodStart + _
                        item.CreditBalanceCurrentPeriodStart)
                    totalDebitTurnoverCurrentPeriod = CRound(totalDebitTurnoverCurrentPeriod + _
                        item.DebitTurnoverCurrentPeriod)
                    totalCreditTurnoverCurrentPeriod = CRound(totalCreditTurnoverCurrentPeriod + _
                        item.CreditTurnoverCurrentPeriod)
                    totalDebitClosingCurrentPeriod = CRound(totalDebitClosingCurrentPeriod + _
                        item.DebitClosingCurrentPeriod)
                    totalCreditClosingCurrentPeriod = CRound(totalCreditClosingCurrentPeriod + _
                        item.CreditClosingCurrentPeriod)
                    totalDebitBalanceCurrentPeriodEnd = CRound(totalDebitBalanceCurrentPeriodEnd + _
                        item.DebitBalanceCurrentPeriodEnd)
                    totalCreditBalanceCurrentPeriodEnd = CRound(totalCreditBalanceCurrentPeriodEnd + _
                        item.CreditBalanceCurrentPeriodEnd)

                Next

                rd.TableGeneral.Item(0).Column9 = DblParser(totalDebitBalanceFormerPeriodStart)
                rd.TableGeneral.Item(0).Column10 = DblParser(totalCreditBalanceFormerPeriodStart)
                rd.TableGeneral.Item(0).Column11 = DblParser(totalDebitTurnoverFormerPeriod)
                rd.TableGeneral.Item(0).Column12 = DblParser(totalCreditTurnoverFormerPeriod)
                rd.TableGeneral.Item(0).Column13 = DblParser(totalDebitClosingFormerPeriod)
                rd.TableGeneral.Item(0).Column14 = DblParser(totalCreditClosingFormerPeriod)
                rd.TableGeneral.Item(0).Column15 = DblParser(totalDebitBalanceCurrentPeriodStart)
                rd.TableGeneral.Item(0).Column16 = DblParser(totalCreditBalanceCurrentPeriodStart)
                rd.TableGeneral.Item(0).Column17 = DblParser(totalDebitTurnoverCurrentPeriod)
                rd.TableGeneral.Item(0).Column18 = DblParser(totalCreditTurnoverCurrentPeriod)
                rd.TableGeneral.Item(0).Column19 = DblParser(totalDebitClosingCurrentPeriod)
                rd.TableGeneral.Item(0).Column20 = DblParser(totalCreditClosingCurrentPeriod)
                rd.TableGeneral.Item(0).Column21 = DblParser(totalDebitBalanceCurrentPeriodEnd)
                rd.TableGeneral.Item(0).Column22 = DblParser(totalCreditBalanceCurrentPeriodEnd)


            ElseIf Version = 2 Then

                For Each item As ActiveReports.BalanceSheetInfo In GetDisplayedList(Of ActiveReports.BalanceSheetInfo) _
                    (R_Obj.BalanceSheet, displayIndexes, 1)

                    rd.Table1.Rows.Add()

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Name
                    If item.IsCreditBalance Then
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = "X"
                    Else
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = ""
                    End If
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Level.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.RelatedAccounts
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = _
                        item.OptimizedBalanceCurrent.ToString("##,0.00;(##,0.00);""""")
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = _
                        item.OptimizedBalanceFormer.ToString("##,0.00;(##,0.00);""""")
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = _
                        item.ActualBalanceCurrent.ToString("##,0.00;(##,0.00);""""")
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = _
                        item.ActualBalanceFormer.ToString("##,0.00;(##,0.00);""""")
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.Number

                Next

            ElseIf Version = 3 Then

                For Each item As ActiveReports.IncomeStatementInfo In GetDisplayedList(Of ActiveReports.IncomeStatementInfo) _
                    (R_Obj.IncomeStatement, displayIndexes, 2)

                    rd.Table1.Rows.Add()

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.Name
                    If item.IsCreditBalance Then
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = "X"
                    Else
                        rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = ""
                    End If
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.Level.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.RelatedAccounts
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = _
                        item.OptimizedBalanceCurrent.ToString("##,0.00;(##,0.00);""""")
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = _
                        item.OptimizedBalanceFormer.ToString("##,0.00;(##,0.00);""""")
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = _
                        item.ActualBalanceCurrent.ToString("##,0.00;(##,0.00);""""")
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = _
                        item.ActualBalanceFormer.ToString("##,0.00;(##,0.00);""""")
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.Number

                Next


            Else

                Throw New NotImplementedException(String.Format( _
                    "Klaida. Objekto '{0}' versijos {1} atvaizdavimas ataskaitoje neimplementuotas.", _
                    R_Obj.GetType.FullName, Version.ToString))

            End If

            If Version = 0 Then
                ReportFileName = "R_AccountTurnoverInfoList2.rdlc"
            ElseIf Version = 1 Then
                ReportFileName = "R_AccountTurnoverInfoList.rdlc"
            ElseIf Version = 2 Then
                ReportFileName = "R_ConsolidatedReport.rdlc"
            ElseIf Version = 3 Then
                ReportFileName = "R_ConsolidatedReport.rdlc"
            Else
                Throw New NotImplementedException(String.Format( _
                    "Klaida. Objekto '{0}' versijos {1} atvaizdavimas ataskaitoje neimplementuotas.", _
                    R_Obj.GetType.FullName, Version.ToString))
            End If
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.DebtInfoList">DebtInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.DebtInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            rd.TableGeneral.Item(0).Column3 = R_Obj.Account.ToString
            If R_Obj.IsBuyer Then
                rd.TableGeneral.Item(0).Column4 = "X"
                rd.TableGeneral.Item(0).Column5 = ""
            Else
                rd.TableGeneral.Item(0).Column5 = "X"
                rd.TableGeneral.Item(0).Column4 = ""
            End If
            If R_Obj.ShowZeroDebtsFilterState Then
                rd.TableGeneral.Item(0).Column6 = "Taip"
            Else
                rd.TableGeneral.Item(0).Column6 = "Ne"
            End If
            If Not R_Obj.GroupInfo Is Nothing AndAlso R_Obj.GroupInfo.ID > 0 Then
                rd.TableGeneral.Item(0).Column7 = R_Obj.GroupInfo.Name
            Else
                rd.TableGeneral.Item(0).Column7 = "Visos grupės"
            End If

            Dim DebtStart As Double = 0
            Dim TurnoverDebet As Double = 0
            Dim TurnoverCredit As Double = 0
            Dim DebtEnd As Double = 0

            For Each item As ActiveReports.DebtInfo In GetDisplayedList(Of ActiveReports.DebtInfo) _
                (R_Obj, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.PersonID.ToString
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.PersonName
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.PersonCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.PersonVatCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.PersonAddress
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.PersonGroup
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.DebtBegin)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.TurnoverDebet)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = DblParser(item.TurnoverCredit)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = DblParser(item.DebtEnd)

                DebtStart += item.DebtBegin
                TurnoverDebet += item.TurnoverDebet
                TurnoverCredit += item.TurnoverCredit
                DebtEnd += item.DebtEnd

            Next

            rd.TableGeneral.Item(0).Column8 = DblParser(DebtStart)
            rd.TableGeneral.Item(0).Column9 = DblParser(TurnoverDebet)
            rd.TableGeneral.Item(0).Column10 = DblParser(TurnoverCredit)
            rd.TableGeneral.Item(0).Column11 = DblParser(DebtEnd)
            rd.TableGeneral.Item(0).Column12 = filterDescription

            ReportFileName = "R_DebtInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.UnsettledPersonInfoList">UnsettledPersonInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
           ByVal R_Obj As ActiveReports.UnsettledPersonInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.AsOfDate.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.Account.ToString
            rd.TableGeneral.Item(0).Column3 = DblParser(R_Obj.MarginOfError, 2)
            If R_Obj.ForBuyers Then
                rd.TableGeneral.Item(0).Column4 = "X"
                rd.TableGeneral.Item(0).Column5 = ""
                rd.TableGeneral.Item(0).Column6 = "Pirkėjai"
            Else
                rd.TableGeneral.Item(0).Column4 = ""
                rd.TableGeneral.Item(0).Column5 = "X"
                rd.TableGeneral.Item(0).Column6 = "Tiekėjai"
            End If
            If R_Obj.PersonGroup Is Nothing OrElse R_Obj.PersonGroup.IsEmpty Then
                rd.TableGeneral.Item(0).Column7 = "Visos grupės"
            Else
                rd.TableGeneral.Item(0).Column7 = R_Obj.PersonGroup.Name
            End If
            rd.TableGeneral.Item(0).Column8 = filterDescription

            Dim currentPerson As String

            For Each personItem As ActiveReports.UnsettledPersonInfo In R_Obj.GetSortedList

                currentPerson = String.Format("{0} ({1}) skola = {2}", personItem.Name, personItem.Code, _
                    DblParser(personItem.Debt, 2))

                For Each item As ActiveReports.UnsettledDocumentInfo In personItem.ItemsSorted

                    rd.Table1.Rows.Add()

                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = currentPerson
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.ID.ToString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.DocTypeHumanReadable
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = item.Date.ToShortDateString
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = item.DocNo
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.Content
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = DblParser(item.SumInDocument)
                    rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = DblParser(item.Debt)

                Next

            Next

            ReportFileName = "R_UnsettledPersonInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

        ''' <summary>
        ''' Map <see cref="ActiveReports.ServiceTurnoverInfoList">ServiceTurnoverInfoList</see> to 
        ''' <see cref="ReportData">ReportData</see> dataset.
        ''' </summary>
        ''' <param name="rd">Report dataset of type ReportData.</param>
        ''' <param name="R_Obj">Object to be maped.</param>
        ''' <param name="ReportFileName">.rdlc form file name.</param>
        ''' <param name="NumberOfTablesInUse">Number of details tables needed to map object data.</param>
        ''' <param name="Version">Version of .rdlc form.</param>
        Friend Sub MapObjectDetailsToReport(ByRef rd As ReportData, _
            ByVal R_Obj As ActiveReports.ServiceTurnoverInfoList, ByRef ReportFileName As String, _
            ByRef NumberOfTablesInUse As Integer, ByVal Version As Integer, _
            ByVal filterDescription As String, ByVal displayIndexes As List(Of Integer)())

            rd.TableGeneral.Item(0).Column1 = R_Obj.DateFrom.ToShortDateString
            rd.TableGeneral.Item(0).Column2 = R_Obj.DateTo.ToShortDateString
            rd.TableGeneral.Item(0).Column3 = R_Obj.TradedType
            If R_Obj.ShowWithoutTurnover Then
                rd.TableGeneral.Item(0).Column4 = "Taip"
            Else
                rd.TableGeneral.Item(0).Column4 = "Ne"
            End If

            Dim purchasedSum As Double = 0
            Dim purchasedSumReturned As Double = 0
            Dim purchasedSumReductions As Double = 0
            Dim soldSum As Double = 0
            Dim soldSumReturned As Double = 0
            Dim soldSumReductions As Double = 0
            Dim discounts As Double = 0

            For Each item As ActiveReports.ServiceTurnoverInfo In GetDisplayedList(Of ActiveReports.ServiceTurnoverInfo) _
                (R_Obj, displayIndexes, 0)

                rd.Table1.Rows.Add()

                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column1 = item.ID.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column2 = item.Name
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column3 = item.TradedType
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column4 = DblParser(item.DefaultRateVatSales, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column5 = DblParser(item.DefaultRateVatPurchase, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column6 = item.ServiceCode
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column7 = BooleanToCheckMark(item.IsObsolete)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column8 = item.AccountSales.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column9 = item.AccountPurchase.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column10 = item.AccountVatPurchase.ToString()
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column11 = DblParser(item.PurchasedAmount, ROUNDAMOUNTINVOICERECEIVED)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column12 = DblParser(item.PurchasedSum, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column13 = DblParser(item.PurchasedAmountReturned, ROUNDAMOUNTINVOICERECEIVED)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column14 = DblParser(item.PurchasedSumReturned, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column15 = DblParser(item.PurchasedSumReductions, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column16 = DblParser(item.SoldAmount, ROUNDAMOUNTINVOICEMADE)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column17 = DblParser(item.SoldSum, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column18 = DblParser(item.SoldAmountReturned, ROUNDAMOUNTINVOICEMADE)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column19 = DblParser(item.SoldSumReturned, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column20 = DblParser(item.SoldSumReductions, 2)
                rd.Table1.Item(rd.Table1.Rows.Count - 1).Column21 = DblParser(item.SoldSumDiscounts, 2)

                purchasedSum = CRound(purchasedSum + item.PurchasedSum, 2)
                purchasedSumReturned = CRound(purchasedSumReturned + item.PurchasedSumReturned, 2)
                purchasedSumReductions = CRound(purchasedSumReductions + item.PurchasedSumReductions, 2)
                soldSum = CRound(soldSum + item.SoldSum, 2)
                soldSumReturned = CRound(soldSumReturned + item.SoldSumReturned, 2)
                soldSumReductions = CRound(soldSumReductions + item.SoldSumReductions, 2)
                discounts = CRound(discounts + item.SoldSumDiscounts, 2)

            Next

            rd.TableGeneral.Item(0).Column5 = DblParser(purchasedSum, 2)
            rd.TableGeneral.Item(0).Column6 = DblParser(purchasedSumReturned, 2)
            rd.TableGeneral.Item(0).Column7 = DblParser(purchasedSumReductions, 2)
            rd.TableGeneral.Item(0).Column8 = DblParser(soldSum, 2)
            rd.TableGeneral.Item(0).Column9 = DblParser(soldSumReturned, 2)
            rd.TableGeneral.Item(0).Column10 = DblParser(soldSumReductions, 2)
            rd.TableGeneral.Item(0).Column11 = DblParser(discounts, 2)
            rd.TableGeneral.Item(0).Column12 = filterDescription

            ReportFileName = "R_ServiceTurnoverInfoList.rdlc"
            NumberOfTablesInUse = 1

        End Sub

#End Region

#Region "Helpers"

        Private Function BooleanToCheckMark(ByVal cValue As Boolean) As String
            If cValue Then Return "X"
            Return ""
        End Function

        Private Sub CopyDataTableValues(ByVal SourceDataTable As DataTable, ByRef TargetDataTable As DataTable)
            Dim i As Integer
            For Each dr As DataRow In SourceDataTable.Rows
                TargetDataTable.Rows.Add()
                For i = 1 To SourceDataTable.Columns.Count
                    TargetDataTable.Rows(TargetDataTable.Rows.Count - 1).Item(i - 1) = dr.Item(i - 1)
                Next
            Next
        End Sub

        Private Sub CopyDataTable(ByRef TargetDataTable As DataTable, _
                                  ByVal SourceDataSet As DataSet, ByVal SourceTableName As String)
            Dim i, j As Integer
            For i = 1 To SourceDataSet.Tables(SourceTableName).Rows.Count
                TargetDataTable.Rows.Add()
                For j = 1 To Math.Min(SourceDataSet.Tables(SourceTableName).Columns.Count, _
                                      TargetDataTable.Columns.Count)

                    TargetDataTable.Rows(i - 1).Item(j - 1) = _
                        SourceDataSet.Tables(SourceTableName).Rows(i - 1).Item(j - 1)

                Next
            Next
        End Sub

        Private Function ConvertDateToWordsLT(ByVal DateToConvert As Date) As String
            Select Case DateToConvert.Month
                Case 1
                    Return DateToConvert.Year.ToString & " m. Sausio mėn. " & DateToConvert.Day.ToString & " d."
                Case 2
                    Return DateToConvert.Year.ToString & " m. Vasario mėn. " & DateToConvert.Day.ToString & " d."
                Case 3
                    Return DateToConvert.Year.ToString & " m. Kovo mėn. " & DateToConvert.Day.ToString & " d."
                Case 4
                    Return DateToConvert.Year.ToString & " m. Balandžio mėn. " & DateToConvert.Day.ToString & " d."
                Case 5
                    Return DateToConvert.Year.ToString & " m. Gegužės mėn. " & DateToConvert.Day.ToString & " d."
                Case 6
                    Return DateToConvert.Year.ToString & " m. Birželio mėn. " & DateToConvert.Day.ToString & " d."
                Case 7
                    Return DateToConvert.Year.ToString & " m. Liepos mėn. " & DateToConvert.Day.ToString & " d."
                Case 8
                    Return DateToConvert.Year.ToString & " m. Rugpjūčio mėn. " & DateToConvert.Day.ToString & " d."
                Case 9
                    Return DateToConvert.Year.ToString & " m. Rugsėjo mėn. " & DateToConvert.Day.ToString & " d."
                Case 10
                    Return DateToConvert.Year.ToString & " m. Spalio mėn. " & DateToConvert.Day.ToString & " d."
                Case 11
                    Return DateToConvert.Year.ToString & " m. Lapkričio mėn. " & DateToConvert.Day.ToString & " d."
                Case 12
                    Return DateToConvert.Year.ToString & " m. Gruodžio mėn. " & DateToConvert.Day.ToString & " d."
                Case Else
                    Throw New ArgumentOutOfRangeException("Invalid date " & DateToConvert.ToShortDateString & ".")
            End Select
        End Function

        Private Function SplitStringByMaxLength(ByVal StringToSplit As String, _
                                                ByVal MaxCharCountInLine As Integer) As String()

            If StringToSplit Is Nothing OrElse String.IsNullOrEmpty(StringToSplit.Trim) Then _
                Return New String() {""}

            If Not StringToSplit.Trim.Length > MaxCharCountInLine Then _
                Return New String() {StringToSplit.Trim}

            Dim result As New List(Of String)

            Dim WordString As String = ""

            For Each c As Char In StringToSplit

                WordString = WordString & c

                If c = ","c OrElse c = "."c OrElse c = ";"c OrElse c = " "c _
                   OrElse c = "-"c OrElse c = "!"c OrElse c = "?"c OrElse c = "%"c _
                   OrElse c = "&"c OrElse c = "%"c OrElse c = "+"c Then

                    If Not String.IsNullOrEmpty(WordString.Trim) Then result.Add(WordString.Trim)
                    WordString = ""

                End If

            Next

            If Not String.IsNullOrEmpty(WordString.Trim) Then result.Add(WordString.Trim)
            WordString = ""

            Dim FinalResult As New List(Of String)

            For Each w As String In result

                If String.IsNullOrEmpty(WordString.Trim) Then
                    WordString = w
                ElseIf Not (WordString.Trim & " " & w).Length > MaxCharCountInLine Then
                    WordString = WordString & " " & w
                Else
                    FinalResult.Add(WordString.Trim)
                    WordString = w
                End If

            Next

            If Not String.IsNullOrEmpty(WordString.Trim) Then FinalResult.Add(WordString.Trim)

            Return FinalResult.ToArray

        End Function

#End Region

    End Module

End Namespace