Namespace ActiveReports

    ''' <summary>
    ''' Represents a declaration (a report to a state institution).
    ''' </summary>
    ''' <remarks>The content and implementation is dependant on the <see cref="DeclarationCriteria">DeclarationCriteria</see>
    ''' and the object that implements <see cref="IDeclaration">IDeclaration</see>.</remarks>
    <Serializable()> _
Public NotInheritable Class Declaration
        Inherits ReadOnlyBase(Of Declaration)

#Region " Business Methods "

        Private Const EMPTY_TABLE_MARKER As String = "%#DELETE#"

        Private ReadOnly _ID As Guid = Guid.NewGuid

        ' manual (de)serializacion of dataset (to string) is used because 
        ' current dataset serialization by MS does not work with web services (at least)
        <NonSerialized()> _
        Private _DeclarationDataSet As DataSet = Nothing
        Private _DeclarationString As String = ""

        Private _DeclarationDateCulture As System.Globalization.DateTimeFormatInfo
        Private _NumberGroupSeparator As String
        Private _NumberDecimalSeparator As String

        Private _Criteria As DeclarationCriteria
        Private _Warning As String = ""


        Public ReadOnly Property ID() As Guid
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _ID
            End Get
        End Property

        Public ReadOnly Property DeclarationDataSet() As DataSet
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get

                If _DeclarationDataSet Is Nothing AndAlso StringIsNullOrEmpty(_DeclarationString) Then Return Nothing

                If _DeclarationDataSet Is Nothing Then

                    Using reader As IO.StringReader = New IO.StringReader(_DeclarationString)
                        _DeclarationDataSet = New DataSet("DeclarationDataSet")
                        _DeclarationDataSet.ReadXml(reader)
                    End Using

                    For Each table As DataTable In _DeclarationDataSet.Tables
                        If table.Rows.Count = 1 AndAlso Not table.Rows(0).Item(0) Is Nothing _
                           AndAlso table.Rows(0).Item(0).ToString.Trim.ToUpper() = EMPTY_TABLE_MARKER Then
                            table.Rows.RemoveAt(0)
                        End If
                    Next

                End If

                Return _DeclarationDataSet

            End Get
        End Property

        Public ReadOnly Property Criteria() As DeclarationCriteria
            <System.Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
            Get
                Return _Criteria
            End Get
        End Property

        Public ReadOnly Property Warning() As String
            Get
                Return _Warning
            End Get
        End Property


        Public Sub SaveToFFData(ByVal fileName As String, ByVal preparatorName As String)

            If _DeclarationDataSet Is Nothing AndAlso StringIsNullOrEmpty(_DeclarationString) Then _
                Throw New Exception(My.Resources.ActiveReports_Declaration_DeclarationDataSetNull)

            ' Set culture params that were used when parsing declaration's
            ' numbers and dates to string
            Dim oldCulture As Globalization.CultureInfo = _
                DirectCast(System.Threading.Thread.CurrentThread.CurrentCulture.Clone, Globalization.CultureInfo)

            System.Threading.Thread.CurrentThread.CurrentCulture = New Globalization.CultureInfo("lt-LT", False)
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = _DeclarationDateCulture
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = _
                _NumberDecimalSeparator
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = _
                _NumberGroupSeparator

            Try

                Using ffDataFormatDataSet As DataSet = _Criteria.DeclarationType.GetFfDataDataSet(DeclarationDataSet, preparatorName)

                    If ffDataFormatDataSet Is Nothing Then
                        Throw New Exception(My.Resources.ActiveReports_Declaration_FailedTransformationToFfdata)
                    End If

                    Using ffDataFileStream As IO.FileStream = New IO.FileStream(fileName, IO.FileMode.Create)
                        ffDataFormatDataSet.WriteXml(ffDataFileStream)
                        ffDataFileStream.Close()
                    End Using

                End Using

                System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture

            Catch ex As Exception
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture
                Throw
            End Try

        End Sub


        Protected Overrides Function GetIdValue() As Object
            Return _ID
        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()


        End Sub

        Public Shared Function CanGetObject() As Boolean
            Return ApplicationContext.User.IsInRole("Workers.Declaration1")
        End Function

#End Region

#Region " Factory Methods "

        ''' <summary>
        ''' Gets a new declaration (report for a state institution) defined by the declaration criteria provided.
        ''' </summary>
        ''' <param name="newCriteria">Declaration criteria, containg info about the declaration to be fetched.</param>
        ''' <remarks></remarks>
        Public Shared Function GetDeclaration(ByVal newCriteria As DeclarationCriteria) As Declaration

            If Not newCriteria.IsValid() Then
                Throw New Exception(String.Format(My.Resources.ActiveReports_IDeclaration_ArgumentsNull, _
                    vbCrLf, newCriteria.GetAllErrors()))
            End If

            Return DataPortal.Fetch(Of Declaration)(newCriteria)

        End Function


        Private Sub New()
            ' require use of factory methods
        End Sub

#End Region

#Region " Data Access "

        Private Overloads Sub DataPortal_Fetch(ByVal newCriteria As DeclarationCriteria)

            If Not CanGetObject() Then Throw New System.Security.SecurityException( _
                My.Resources.Common_SecuritySelectDenied)

            Using result As DataSet = newCriteria.DeclarationType.GetBaseDataSet(newCriteria, _Warning)
                _DeclarationString = WriteDataSetToString(result)
            End Using

            _Criteria = newCriteria

            _DeclarationDateCulture = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat
            _NumberDecimalSeparator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator
            _NumberGroupSeparator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator

        End Sub

        Private Shared Function WriteDataSetToString(ByVal declarationDataSet As DataSet) As String

            ' this method does not adds datatables with 0 rows
            ' thus we need to create an empty row and mark it for deletion when deserializing

            For Each table As DataTable In declarationDataSet.Tables
                If table.Rows.Count < 1 AndAlso table.Columns.Count > 0 Then
                    table.Rows.Add()
                    table.Rows(0).Item(0) = EMPTY_TABLE_MARKER
                End If
            Next

            Dim result As String

            Dim objStream As New IO.MemoryStream()

            Try
                declarationDataSet.WriteXml(objStream)
                Dim objXmlWriter As New Xml.XmlTextWriter(objStream, Text.Encoding.UTF8)
                objStream = DirectCast(objXmlWriter.BaseStream, IO.MemoryStream)
                result = Text.Encoding.UTF8.GetString(objStream.ToArray())
            Catch ex As Exception
                objStream.Dispose()
                Throw
            End Try

            objStream.Dispose()

            Return result

        End Function


        ''' <summary>
        ''' Gets a standard datatable containing general company data (name, code, address, etc.)
        ''' </summary>
        ''' <remarks></remarks>
        Friend Shared Function FetchGeneralDataTable() As DataTable
            Dim result As New DataTable("General")

            result.Columns.Add("CompanyName")
            result.Columns.Add("Code")
            result.Columns.Add("Address")
            result.Columns.Add("CodeSODRA")
            result.Columns.Add("CodeVAT")
            result.Columns.Add("Bank")
            result.Columns.Add("BankAccount")
            result.Columns.Add("Email")
            result.Columns.Add("Tel")
            result.Columns.Add("HeadPerson")

            result.Rows.Add()
            result.Rows(0).Item(0) = GetLimitedLengthString(GetCurrentCompany.Name, 68)
            result.Rows(0).Item(1) = GetCurrentCompany.Code
            result.Rows(0).Item(2) = GetLimitedLengthString(GetCurrentCompany.Address, 68)
            result.Rows(0).Item(3) = GetCurrentCompany.CodeSODRA
            result.Rows(0).Item(4) = GetCurrentCompany.CodeVat
            result.Rows(0).Item(5) = GetCurrentCompany.Bank
            result.Rows(0).Item(6) = GetCurrentCompany.BankAccount
            result.Rows(0).Item(7) = GetLimitedLengthString(GetCurrentCompany.Email, 45)
            result.Rows(0).Item(8) = "" ' GetLimitedLengthString(GetCurrentCompany.tel, 15)
            result.Rows(0).Item(9) = GetCurrentCompany.HeadPerson

            Return result

        End Function

        ''' <summary>
        ''' Gets a number of employees at the date specified.
        ''' </summary>
        ''' <param name="atDate">as of date</param>
        ''' <remarks></remarks>
        Friend Shared Function GetEmployeesCount(ByVal atDate As Date) As Integer

            Dim result As Integer = 0

            Dim myComm As New SQLCommand("FetchEmployeesCount")
            myComm.AddParam("?DT", atDate.Date)

            Using myData As DataTable = myComm.Fetch
                If myData.Rows.Count > 0 AndAlso myData.Columns.Count > 1 Then _
                    result = CIntSafe(myData.Rows(0).Item(0), 0) - CIntSafe(myData.Rows(0).Item(1), 0)
                If result < 0 Then result = 0
            End Using

            Return result

        End Function

        ''' <summary>
        ''' Changes Null (DBNull) values in the datatable to 
        ''' the passed dbNull object (string, double or integer).
        ''' </summary>
        Friend Shared Sub ClearDatatable(ByRef myData As DataTable, ByVal dbNull As Object)
            For i As Integer = 1 To myData.Rows.Count
                For j As Integer = 1 To myData.Columns.Count
                    If IsDBNull(myData.Rows(i - 1).Item(j - 1)) _
                        OrElse myData.Rows(i - 1).Item(j - 1) Is Nothing Then
                        If Not myData.Columns(j - 1).DataType Is GetType(Date) AndAlso _
                            Not myData.Columns(j - 1).DataType Is GetType(DateTime) Then
                            myData.Rows(i - 1).Item(j - 1) = dbNull
                        End If
                    End If
                Next
            Next
        End Sub




#End Region

    End Class

End Namespace