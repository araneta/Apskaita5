Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports HtmlAgilityPack

Public Module CommonMethods


    Public Sub GetPersonInfo(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)

        If e.Argument Is Nothing OrElse String.IsNullOrEmpty(e.Argument.ToString.Trim) Then _
            Throw New ArgumentNullException("Klaida. Nenurodytas asmens/įmonės kodas.")

        Dim request As HttpWebRequest = GetRequestForJar(e.Argument.ToString)

        If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
            e.Cancel = True
            Exit Sub
        End If

        Dim response As HttpWebResponse = request.GetResponse

        If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
            e.Cancel = True
            Exit Sub
        End If

        Dim result As PersonInfo

        Using sr As New IO.StreamReader(response.GetResponseStream())
            Try
                result = ParseResultForJar(sr.ReadToEnd, New PersonInfo(e.Argument.ToString))
            Catch ex As Exception
                response.Close()
                sr.Close()
                Throw ex
            End Try
            sr.Close()
        End Using

        If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
            e.Cancel = True
            Exit Sub
        End If

        DirectCast(sender, System.ComponentModel.BackgroundWorker).ReportProgress(0, _
            "Gaunami duomenys iš PVM mokėtojų registro...")

        request = GetRequestForVmi(e.Argument.ToString)

        If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
            e.Cancel = True
            Exit Sub
        End If

        response = request.GetResponse

        If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
            e.Cancel = True
            Exit Sub
        End If

        Using sr As New IO.StreamReader(response.GetResponseStream())
            Try
                result = ParseResultForVmi(sr.ReadToEnd, result)
            Catch ex As Exception
                response.Close()
                sr.Close()
                Throw ex
            End Try
            sr.Close()
        End Using

        If String.IsNullOrEmpty(result.Name.Trim) AndAlso String.IsNullOrEmpty(result.VatCode.Trim) Then _
            Throw New Exception(result.Message)

        e.Result = result

    End Sub

    Public Function GetRequestForVmi(ByVal PersonCode As String) As HttpWebRequest

        Dim request As HttpWebRequest = WebRequest.Create("http://www.vmi.lt/lt/index.aspx?itemId=1003740")

        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:11.0) Gecko/20100101 Firefox/15.0"
        request.Accept = "Accept: image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        request.Headers.Add("Accept-Language: en-us")
        request.KeepAlive = True
        request.CookieContainer = New CookieContainer()
        request.Method = WebRequestMethods.Http.Post
        request.ContentType = "application/x-www-form-urlencoded"
        request.Referer = "http://www.vmi.lt/lt/?itemId=1003740"
        Dim byteArray As Byte() = Encoding.ASCII.GetBytes( _
            "EasyFormId=101546&Error1=%C4%AEveskite+%C4%AFmon%C4%97s+%2F+asmens+kod%C4%85&Erorr2=Kod%C4%85+turi+sudaryti+tik+skaitmenys&Error3=Patikrinkite%2C+ar+teisingai+%C4%AFved%C4%97te+kod%C4%85+%28neattinka+kodo+ilgis%29&Error4=%C4%AEveskite+%C4%AFmon%C4%97s+%2F+asmens+PVM+kod%C4%85&Error5=PVM+kod%C4%85+turi+sudaryti+tik+skaitmenys&Error6=Patikrinkite%2C+ar+teisingai+%C4%AFved%C4%97te+PVM+kod%C4%85+%28neatitinka+PVM+kodo+ilgis%29&Error7=Nurodykite+bent+keturi%C5%B3+raid%C5%BEi%C5%B3+ilgio+pavadinimo+%28vardo%2C+pavard%C4%97s%29+fragment%C4%85%21&SearchType=Code&InputByCode=300520954&InputByPVMCode=&InputByName=&LNGSubmit=Ie%C5%A1koti".Replace("InputByCode=300520954", "InputByCode=" & PersonCode.Trim))
        request.ContentLength = byteArray.Length
        Dim newStream As IO.Stream = request.GetRequestStream()
        newStream.Write(byteArray, 0, byteArray.Length)
        newStream.Close()

        Return request

    End Function

    Public Function ParseResultForVmi(ByVal WebResponseString As String, _
        ByVal CurrentPersonInfo As PersonInfo) As PersonInfo

        Dim StructuredTable As DataSet = ConvertHtmlToDataSet(WebResponseString)

        Dim VatIsFound As Boolean = False
        Dim NameIsFound As Boolean = False

        For Each tbl As DataTable In StructuredTable.Tables

            If tbl.Columns.Count = 2 Then

                For i As Integer = 1 To tbl.Rows.Count

                    If Not VatIsFound AndAlso tbl.Rows(i - 1).Item(0).ToString.Trim.ToLower = "PVM kodas:".Trim.ToLower Then

                        CurrentPersonInfo.VatCode = tbl.Rows(i - 1).Item(1).ToString.Trim
                        VatIsFound = True
                        If NameIsFound Then Exit For

                    ElseIf Not NameIsFound AndAlso tbl.Rows(i - 1).Item(0).ToString.Trim.ToLower _
                        = "Paieškos rezultatai".Trim.ToLower AndAlso i <> tbl.Rows.Count Then

                        If CurrentPersonInfo.Name Is Nothing OrElse _
                            String.IsNullOrEmpty(CurrentPersonInfo.Name.Trim) Then

                            CurrentPersonInfo.Message = "Duomenys apie asmenį, kurio kodas " _
                                & CurrentPersonInfo.Code & ", nerasti juridinių asmenų registre, " _
                                & "bet rasti PVM mokėtojų registre. Tikėtina, kad tai fizinis asmuo."

                            CurrentPersonInfo.Name = tbl.Rows(i).Item(0).ToString.Trim

                        End If

                        NameIsFound = True
                        If VatIsFound Then Exit For

                    End If

                Next

                If VatIsFound OrElse NameIsFound Then Exit For

            End If

        Next

        If String.IsNullOrEmpty(CurrentPersonInfo.Message.Trim) AndAlso _
            String.IsNullOrEmpty(CurrentPersonInfo.VatCode.Trim) AndAlso _
            Not String.IsNullOrEmpty(CurrentPersonInfo.Name.Trim) Then
            CurrentPersonInfo.Message = "Duomenys apie asmenį, kurio kodas " _
                & CurrentPersonInfo.Code & ", rasti juridinių asmenų registre, " _
                & "bet nerasti PVM mokėtojų registre, t.y. šis asmuo nėra PVM mokėtojas."
        ElseIf String.IsNullOrEmpty(CurrentPersonInfo.VatCode.Trim) AndAlso _
            String.IsNullOrEmpty(CurrentPersonInfo.Name.Trim) Then
            CurrentPersonInfo.Message = "Duomenys apie asmenį, kurio kodas " _
                & CurrentPersonInfo.Code & ", nerasti nei juridinių asmenų registre, " _
                & "nei PVM mokėtojų registre, t.y. arba toks asmuo neegzistuoja, " _
                & "arba tai užsienio asmuo, arba tai fizinis asmuo nesantis PVM mokėtoju."
        End If

        Return CurrentPersonInfo

    End Function

    Public Function GetRequestForJar(ByVal PersonCode As String) As HttpWebRequest

        Dim request As HttpWebRequest = WebRequest.Create("http://www.registrucentras.lt/jar/p/index.php?pav=&kod=" & PersonCode & "&sav=&sta=&for=&p=1")

        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:11.0) Gecko/20100101 Firefox/15.0"
        request.Accept = "Accept: image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        request.Headers.Add("Accept-Language: en-us")
        request.Headers.Add("Accept-Encoding: gzip, deflate")
        request.KeepAlive = True
        request.CookieContainer = New CookieContainer()
        request.Method = WebRequestMethods.Http.Get
        request.Referer = "http://www.registrucentras.lt/jar/p/index.php?"

        Return request

    End Function

    Public Function ParseResultForJar(ByVal WebResponseString As String, _
        ByVal CurrentPersonInfo As PersonInfo) As PersonInfo

        Dim d As New HtmlDocument
        d.LoadHtml(WebResponseString.Trim)

        Dim StructuredTable As DataSet = ConvertHtmlToDataSet(WebResponseString)

        If StructuredTable.Tables.Count > 17 AndAlso StructuredTable.Tables(17).Rows.Count > 1 Then
            If StructuredTable.Tables(17).Rows(1).Item(1).ToString.Trim.Split( _
                New String() {vbCrLf}, StringSplitOptions.None).Length < 2 Then _
                Throw New Exception("Klaida. Nepavyko iššifruoti iš Registrų centro " _
                    & "gautų duomenų. Gali būti, kad pasikeitė puslapio struktūra.")
            CurrentPersonInfo.Address = StructuredTable.Tables(17).Rows(1).Item(1).ToString.Trim.Split( _
                New String() {vbCrLf}, StringSplitOptions.None)(1)
            CurrentPersonInfo.Name = StructuredTable.Tables(17).Rows(1).Item(1).ToString.Trim.Split( _
                New String() {vbCrLf}, StringSplitOptions.None)(0)
            Return CurrentPersonInfo
        End If

        CurrentPersonInfo.Message = "Juridinių asmenų registre nėra duomenų apie asmenį," _
            & "kurio kodas " & CurrentPersonInfo.Code & "."
        Return CurrentPersonInfo

    End Function


    Public Sub GetCurrencyRateList(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)

        If e.Argument Is Nothing OrElse Not TypeOf e.Argument Is CurrencyRateList Then _
            Throw New Exception("Klaida. Nenurodyta nė viena valiuta.")

        Dim source As CurrencyRateList = DirectCast(e.Argument, CurrencyRateList)
        source.RemoveDuplicatesFromList()

        If source.Count < 1 Then Throw New Exception("Klaida. Nenurodyta nė viena valiuta.")

        Dim result As New CurrencyRateList

        For Each d As Date In source.GetDatesInList

            DirectCast(sender, System.ComponentModel.BackgroundWorker).ReportProgress(0, _
                "Gaunami valiutų kursai iš Lietuvos Banko " & d.ToShortDateString & " dienai...")

            Dim request As HttpWebRequest = GetRequestForCurrencies(d)

            If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If

            Dim response As HttpWebResponse = request.GetResponse

            If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If

            Using sr As New IO.StreamReader(response.GetResponseStream())
                Try
                    result.AddRange(ParseResultForCurrencies(sr.ReadToEnd, source, d))
                Catch ex As Exception
                    response.Close()
                    sr.Close()
                    Throw ex
                End Try
                sr.Close()
            End Using

            If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If

        Next

        If result.Count < 1 Then Throw New Exception("Klaida. Dėl nežinomų " _
            & "priežasčių nepavyko gauti valiutų kursų. Galimai pasikeitė " _
            & "Lietuvos Banko web puslapio struktūra.")

        e.Result = result

    End Sub

    Public Function GetRequestForCurrencies(ByVal RateAtDate As Date) As HttpWebRequest

        Dim request As HttpWebRequest = WebRequest.Create("http://www.lb.lt/exchange/Results.asp")

        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:11.0) Gecko/20100101 Firefox/15.0"
        request.Accept = "Accept: image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        request.Headers.Add("Accept-Language: en-us")
        request.Headers.Add("Accept-Encoding: gzip, deflate")
        request.ContentType = "application/x-www-form-urlencoded"
        request.KeepAlive = True
        request.CookieContainer = New CookieContainer()
        request.Method = WebRequestMethods.Http.Post
        request.Referer = "http://www.lb.lt/exchange/Results.asp"
        Dim byteArray As Byte() = Encoding.ASCII.GetBytes( _
            "Lang=L&id=7713&ord=1&dir=ASC&Y=" & RateAtDate.Year.ToString & "&M=" _
            & RateAtDate.Month.ToString & "&D=" & RateAtDate.Day.ToString & "&DD=D&WW=W&vykdyti=Vykdyti")
        request.ContentLength = byteArray.Length
        Dim newStream As IO.Stream = request.GetRequestStream()
        newStream.Write(byteArray, 0, byteArray.Length)
        newStream.Close()

        Return request

    End Function

    Public Function ParseResultForCurrencies(ByVal WebResponseString As String, _
        ByVal CurrencyRateList As CurrencyRateList, ByVal RateAtDate As Date) As CurrencyRateList

        Dim result As New CurrencyRateList

        Dim d As New HtmlDocument
        d.LoadHtml(WebResponseString.Trim)

        Dim StructuredTable As DataSet = ConvertHtmlToDataSet(WebResponseString)

        If StructuredTable.Tables(5).Rows.Count > 2 Then

            For Each dr As DataRow In StructuredTable.Tables(5).Rows
                If Not Integer.TryParse(dr.Item(1).ToString, New Integer) Then dr.Item(1) = 1
            Next
            StructuredTable.Tables(5).Rows.RemoveAt(0)
            StructuredTable.Tables(5).Rows.RemoveAt(0)

            For i As Integer = 1 To CurrencyRateList.Count

                If CurrencyRateList(i - 1).Date.Date = RateAtDate.Date Then

                    For Each dr As DataRow In StructuredTable.Tables(5).Rows
                        If dr.Item(3).ToString.Trim.ToUpper.Replace("*", "") _
                            = CurrencyRateList(i - 1).CurrencyCode Then
                            result.Add(New CurrencyRate(CurrencyRateList.Item(i - 1), _
                                Double.Parse(dr.Item(4).ToString.Trim) _
                                / Integer.Parse(dr.Item(1).ToString.Trim)))
                            Exit For
                        End If
                    Next

                End If

            Next

        End If

        Return result

    End Function


    Public Sub DownloadLatCivilCases(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)

        If e.Argument Is Nothing OrElse Not TypeOf e.Argument Is CourtRequest Then _
            Throw New Exception("Klaida. Nenurodyti užklausos parametrai.")

        Dim requestData As CourtRequest = DirectCast(e.Argument, CourtRequest)

        If requestData.FolderPath Is Nothing OrElse String.IsNullOrEmpty(requestData.FolderPath.Trim) Then _
            Throw New ArgumentNullException("Klaida. Nenurodytas folderis nutartims išsaugoti.")

        If requestData.DateFrom.Date > requestData.DateTo.Date Then Throw New ArgumentException( _
            "Klaida. Laikotarpio pradžios data negali būti vėlesnė už laikotarpio pabaigos datą.")

        Dim request As HttpWebRequest
        Dim response As HttpWebResponse

        Dim FileDownloaded As Integer = 0
        Dim CurrentProgress As Integer

        For i As Integer = 0 To DateDiff(DateInterval.DayOfYear, requestData.DateFrom, _
            requestData.DateTo)

            CurrentProgress = Math.Ceiling(100 * i / (DateDiff(DateInterval.DayOfYear, _
                requestData.DateFrom, requestData.DateTo) + 1))

            DirectCast(sender, System.ComponentModel.BackgroundWorker).ReportProgress( _
                CurrentProgress.ToString, "Gaunami duomenys apie nutartis " _
                & requestData.DateFrom.AddDays(i).ToShortDateString & " dienai...")

            request = GetRequestForLat(requestData.DateFrom.AddDays(i).Date)

            Dim responseReceived As Boolean = False
            Dim FailedRequests As Integer = 0

            While Not responseReceived
                Try
                    response = request.GetResponse
                    responseReceived = True
                Catch ex As Exception
                    FailedRequests += 1
                    DirectCast(sender, System.ComponentModel.BackgroundWorker).ReportProgress( _
                        CurrentProgress.ToString, "Atmesta užklausa: " & ex.Message)
                    If FailedRequests > 20 Then Throw New Exception( _
                        "Klaida. Užklausa buvo atmesta daugiau kaip 20 kartų iš eilės. " _
                        & "Parsisiųsta " & FileDownloaded.ToString & " dokumentų už laikotarpį nuo " _
                        & requestData.DateFrom.ToShortDateString & " iki " _
                        & requestData.DateFrom.AddDays(i - 1).ToShortDateString & ".")
                    System.Threading.Thread.Sleep(1000)
                End Try
            End While

            If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If

            Using sr As New IO.StreamReader(response.GetResponseStream())
                Try
                    ParseResultForLat(sr.ReadToEnd, requestData.FolderPath, _
                        DirectCast(sender, ComponentModel.BackgroundWorker), _
                        FileDownloaded, CurrentProgress)
                Catch ex As Exception
                    response.Close()
                    sr.Close()
                    Throw ex
                End Try
                response.Close()
                sr.Close()
            End Using

            If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If

        Next

        e.Result = FileDownloaded

    End Sub

    Public Function GetRequestForLat(ByVal AtDate As Date) As HttpWebRequest

        Dim request As HttpWebRequest = WebRequest.Create("http://www2.lat.lt/lat_web_test/default.aspx?item=tn_liteko&lang=1")

        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:11.0) Gecko/20100101 Firefox/15.0"
        request.Accept = "Accept: image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        request.Headers.Add("Accept-Language: en-us")
        request.Headers.Add("Accept-Encoding: gzip, deflate")
        request.ContentType = "application/x-www-form-urlencoded"
        request.KeepAlive = True
        request.CookieContainer = New CookieContainer()
        request.Method = WebRequestMethods.Http.Post
        request.Referer = "http://www2.lat.lt/lat_web_test/default.aspx?item=tn_liteko&lang=1 "
        Dim byteArray As Byte() = Encoding.ASCII.GetBytes( _
            "__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUKMTMwM" _
            & "jIxNDkyNw9kFgQCAQ8WAh4JaW5uZXJodG1sBVZMaWV0dXZvcyBBdWvFocSNaWF1c2lhc2lz" _
            & "IFRlaXNtYXMgLSBUZWlzbW8gbnV0YXJ0eXMgJmd0OyBOdXRhcnR5cyBudW8gMjAwNiBtLiA" _
            & "oTElURUtPKWQCAw9kFggCAQ8PFgIeBFRleHQFrAM8YSBocmVmPSI%2FaXRlbT1ob21lJmxhb" _
            & "mc9MSIgY2xhc3M9InN5bWIiPjxpbWcgY2xhc3M9InZhX2JvdCIgc3JjPSJpbWFnZXMvdG9wX2" _
            & "hvbWUuanBnIiBhbHQ9Ik51b3JvZGEgxK8gcGlybcSFIHB1c2xhcMSvIiBib3JkZXI9IjAiIC8%" _
            & "2BPC9hPiZuYnNwO3wmbmJzcDs8YSBocmVmPSI%2FaXRlbT1zaXRlbWFwJmxhbmc9MSIgY2xhc3M9" _
            & "InN5bWIiPjxpbWcgY2xhc3M9InZhX2JvdCIgc3JjPSJpbWFnZXMvdG9wX3NpdGVtYXAuanBnIiBh" _
            & "bHQ9Ik51b3JvZGEgxK8gc3ZldGFpbsSXcyDFvmVtxJdsYXDEryIgYm9yZGVyPSIwIiAvPjwvYT4m" _
            & "bmJzcDt8Jm5ic3A7PGEgaHJlZj0iP2l0ZW09cmFzeXRpJmxhbmc9MSIgY2xhc3M9InN5bWIiPjxpb" _
            & "WcgY2xhc3M9InZhX2JvdCIgc3JjPSJpbWFnZXMvdG9wX21haWxlci5qcGciIGFsdD0iIiBib3JkZX" _
            & "I9IjAiIC8%2BPC9hPmRkAgkPDxYCHghJbWFnZVVybAUXaW1hZ2VzL2J0bl9zZWFyY2hfMS5naWZkZA" _
            & "INDw8WAh8BBR1OdXRhcnR5cyBudW8gMjAwNiBtLiAoTElURUtPKWRkAg8PZBYCZg9kFgJmD2QWHgIB" _
            & "DxAPFgYeDURhdGFUZXh0RmllbGQFEHRpcGFzX2luc3RhbmNpamEeDkRhdGFWYWx1ZUZpZWxkBR" _
            & "N0aXBhc19pbnN0YW5jaWphX2lkHgtfIURhdGFCb3VuZGcWAh4Jb25rZXlkb3duBR9LZXlEb3du" _
            & "SGFuZGxlcihjdGwwM19jbWRTZWFyY2gpEBUFC3Zpc29zIGJ5bG9zQmFkbWluaXN0cmFjaW5pxb" _
            & "MgdGVpc8SXcyBwYcW%2BZWlkaW3FsyBieWxhICh2aWVuaW50ZWxlIGluc3RhbmNpamEpICRiYXV" _
            & "kxb5pYW1vamkgYnlsYSAoa2FzYWNpbmUgdHZhcmthKSAhY2l2aWxpbsSXIGJ5bGEgKGthc2FjaW5lI" _
            & "HR2YXJrYSkgRkTEl2wgYnlsb3MgdGVpc21pbmd1bW8gKHNwZWNpYWxpb2ppIGtvbGVnaWphKSAodml" _
            & "lbmludGVsZSBpbnN0YW5jaWphKSAVBQMwXzADNF80AzNfMwMyXzMDNl80FCsDBWdnZ2dnFgECA2QC" _
            & "Aw8QDxYGHwMFD2Z1bGxwYXZhZGluaW1hcx8EBQJpZB8FZxYCHwYFH0tleURvd25IYW5kbGVyKGN0b" _
            & "DAzX2NtZFNlYXJjaCkQFQlGQWRtaW5pc3RyYWNpbmnFsyBieWzFsyBrYXRlZ29yaWrFsyBrbGFzaWZ" _
            & "pa2F0b3JpdXMgKE4pKG51byAyMDA1LjAxLjAxKURCYXVkxb5pYW3Fs2rFsyBieWzFsyBrYXRlZ29ya" _
            & "WrFsyBrbGFzaWZpa2F0b3JpdXMgKE4pKG51byAyMDA1LjAxLjAxKUBDaXZpbGluacWzIGJ5bMWzIGth" _
            & "dGVnb3JpasWzIGtsYXNpZmlrYXRvcml1cyAoTikobnVvIDIwMDUuMDEuMDEpLUxBVCBiYXVkxb5p" _
            & "YW3Fs2rFsyBudXRhcsSNacWzIGtsYXNpZmlrYXRvcml1czlMQVQgY2l2aWxpbmnFsyBudXRhcsSNac" _
            & "WzIGtsYXNpZmlrYXRvcml1cyhudW8gMjAwMS4wMS4wMSkrUGFwaWxkb21pIGJ5bMWzIMW%2BeW3El2" _
            & "ppbWFpKG51byAyMDA4LjA5LjAxKWFUZWlzbcWzIHByb2Nlc2luacWzIHNwcmVuZGltxbMgYmF1ZMW" _
            & "%2BaWFtb3Npb3NlIGJ5bG9zZSBrYXRlZ29yaWrFsyBrbGFzaWZpa2F0b3JpdXMobnVvIDIwMDguMDYu" _
            & "MDEpYVRlaXNtxbMgcHJvY2VzaW5pxbMgc3ByZW5kaW3FsyBjaXZpbGluxJdzZSBieWxvc2Uga2F0Z" _
            & "WdvcmlqxbMga2xhc2lmaWthdG9yaXVzIChOKShudW8gMjAwNS4wMS4wMSlXVGVpc23FsyBzcHJlbm" _
            & "RpbcWzIGFkbWluaXN0cmFjaW7El3NlIGJ5bG9zZSBrYXRlZ29yaWrFsyBrbGFzaWZpa2F0b3JpdXMo" _
            & "bnVvIDIwMDguMDYuMDEpFQkBOQIxMQIxMwE0ATMCMTkCMTgCMTQCMTcUKwMJZ2dnZ2dnZ2dnZGQCBg8P" _
            & "ZBYCHwYFH0tleURvd25IYW5kbGVyKGN0bDAzX2NtZFNlYXJjaClkAgcPDxYCHwEF4QI8aW5wdXQgd" _
            & "HlwZT1idXR0b24gb25jbGljaz0iT3BlbldpbmRvdygna2xhc2lmaWthdG9yaWFpL2tsYXMuYXNweD9" _
            & "idGlwYXM9JyArIGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdjdGwwM19jbWJCVGlwYXMnKS52YWx1" _
            & "ZS5zdWJzdHIoMCwxKSArICcmaW5zdD0nICsgZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ2N0bDAzX2N" _
            & "tYkJUaXBhcycpLnZhbHVlLnN1YnN0cigyKSArICcmY3RybD1jdGwwM190eHRLYXRlZ29yaWpvc" _
            & "yZrbGFzPScgKyBkb2N1bWVudC5nZXRFbGVtZW50QnlJZCgnY3RsMDNfY21iS2xhc2lmaWthdG9ya" _
            & "WFpJykudmFsdWUgKyAnICcsNjAwLDQwMCwnJyk7IiB2YWx1ZT0iS2F0ZWdvcmlqxbMgbWVkaXMi" _
            & "PmRkAgkPEA9kFgIfBgUfS2V5RG93bkhhbmRsZXIoY3RsMDNfY21kU2VhcmNoKRAVEwR2aXNpBDI" _
            & "wMTIEMjAxMQQyMDEwBDIwMDkEMjAwOAQyMDA3BDIwMDYEMjAwNQQyMDA0BDIwMDMEMjAwMgQyMD" _
            & "AxBDIwMDAEMTk5OQQxOTk4BDE5OTcEMTk5NgQxOTk1FRMBMAQyMDEyBDIwMTEEMjAxMAQyMDA5BD" _
            & "IwMDgEMjAwNwQyMDA2BDIwMDUEMjAwNAQyMDAzBDIwMDIEMjAwMQQyMDAwBDE5OTkEMTk5OAQxOT" _
            & "k3BDE5OTYEMTk5NRQrAxNnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZGQCCw8QDxYGHwMFEG51bWVyaW9fc" _
            & "2FibG9uYXMfBAUCaWQfBWcWAh8GBR9LZXlEb3duSGFuZGxlcihjdGwwM19jbWRTZWFyY2gpEBUF" _
            & "GS0tIG51bWVyaXMgYmUgxaFhYmxvbm8gLS0XM0stMy1bTlVNRVJJU10vW01FVEFJNF0XM0stNy1bTlVN" _
            & "RVJJU10vW01FVEFJNF0XM0stUC1bTlVNRVJJU10vW01FVEFJNF0WQ0lLLVtOVU1FUklTXS9bTUVUQUk0XR" _
            & "UFATACNDMCNDQCNDUCNzEUKwMFZ2dnZ2dkZAIPDxAPZBYCHwYFH0tleURvd25IYW5kbGVyKGN0bD" _
            & "AzX2NtZFNlYXJjaClkZGQCEg8PFggeAlRUZR4CVFYFAVYeAlRFBQFFHgJUUgUBKmRkAhUPDxYIHwdlH" _
            & "wgFAVYfCQUBRR8KBQEqZGQCFw8QDxYGHwMFD2Z1bGxwYXZhZGluaW1hcx8EBQJpZB8FZxYCHwYFH0" _
            & "tleURvd25IYW5kbGVyKGN0bDAzX2NtZFNlYXJjaCkQFSQjLS0gcGFzaXJpbmtpdGUgbm9yaW3EhSB" _
            & "0ZWlzxJdqxIUgLS0VQXJtYW5hcyBBYnJhbWF2acSNaXVzEFZpa3RvcmFzIEFpZHVrYXMURGFuZ3V" _
            & "0xJcgQW1icmFzaWVuxJcTRGFsaWEgQmFqZXLEjWnFq3TElxRFZ2lkaWp1cyBCYXJhbmF1c2thcxFSa" _
            & "W1hbnRhcyBCYXVtaWxhcxVWYWxlcmlqdXMgxIxpdcSNaXVsa2EVR3Jhxb5pbmEgRGF2aWRvbmllbsS" _
            & "XEU9sZWdhcyBGZWRvc2l1a2FzDUdpbnRhcmFzIEdvZGEVVmlyZ2lsaWp1cyBHcmFiaW5za2FzElZ5" _
            & "dGF1dGFzIEdyZWnEjWl1cxNTaWdpdGFzIEd1cmV2acSNaXVzFkJpcnV0xJcgSmFuYXZpxI1pxat0" _
            & "xJcTSmFuaW5hIEphbnXFoWtpZW7ElxbEjGVzbG92YXMgSm9rxatiYXVza2FzFEFudGFuYXMgS2xpb" _
            & "WF2acSNaXVzFkdpbnRhcmFzIEtyecW%2BZXZpxI1pdXMSRWdpZGlqdXMgTGF1xb5pa2FzD1ppZ" _
            & "21hcyBMZXZpY2tpcxFWeXRhdXRhcyBNYXNpb2thcw9BbGdpcyBOb3JrxatuYXMTVnl0YXV0YXM" _
            & "gUGllc2xpYWthcw9BbHZ5ZGFzIFBpa2VsaXMQSm9uYXMgUHJhcGllc3RpcxNBbGRvbmEgUmFrY" _
            & "XVza2llbsSXE1ZsYWRpc2xvdmFzIFJhbm9uaXMSU2lnaXRhIFJ1ZMSXbmFpdMSXEkFudGFuYXMg" _
            & "U2ltbmnFoWtpcxBBbGJpbmFzIFNpcnZ5ZGlzFEphbmluYSBTdHJpcGVpa2llbsSXEUp1b3phcyDFoGVy" _
            & "a8WhbmFzElRvbWFzIMWgZcWha2F1c2thcxBWaW5jYXMgVmVyc2Vja2FzDlByYW5hcyDFvWVpbXlzFSQk" _
            & "MDAwMDAwMDAtMDAwMC0wMDAwLTAwMDAtMDAwMDAwMDAwMDAwJDI0ZjFlYmRjLTNiMWEtNDJjZC1hNjZkLT" _
            & "M1OTk0YmRhZWRlMiRhZGQwMDAwMS0wMDAwLTAxMTAtMDA4Ni0wNjUxMDUxMDAxMTckYWRkMDAwMDItMD" _
            & "AwMC0wMTAwLTAwNjgtMDY1MTA5MDk4MTE0JGQxY2I0MjEzLTQyOTItNDU2Ny1iMjRiLThiMTg1ZGEwM2U" _
            & "0YSQxZTkwMmYyYi1lZGI1LTRlODUtYTZjMi0wMWY3ZTA1MmU5ZWIkYWRkMDAwMDQtMDAwMC0wMTEwLTAwO" _
            & "DItMDY2MDk3MTE3MTA5JGFkZDAwMDExLTAwMDAtMDExMS0wMDg2LTIwMDEwNTExNzIzMiRlMzY4NGVlZS0xM" _
            & "WI0LTQ0ZDgtOTA2MS03NTljZTAyMDRiZDgkOTZiOTgwYjYtNzQ5YS00ZDkyLTk1MjAtODdhNWM3YjAyZDQyJ" _
            & "GQwMjRhNjI0LTE5OGEtNDQ4YS1hZDJkLTc4MmEwNDcyYWI5MyRhZGQwMDAyMC0wMDAwLTAxMTAtMDA4Ni0wN" _
            & "zExMTQwOTcwOTgkOGExNTAzOGUtNGI2Mi00MDU3LWE3NmYtNjhkNTlhNzdmNzRjJGFkZDAwMDI0LTAwMDAtM" _
            & "DEwMC0wMDgzLTA3MTExNzExNDEwMSQwY2MwOWY0Zi1iMTM4LTQyYTEtYWJiMS1hN2QxYjE0MzQ0MjgkYWRkMD" _
            & "AwMjctMDAwMC0wMTAwLTAwNzQtMDc0MDk3MTEwMTE3JGFkZDAwMDMwLTAwMDAtMDEwMC0wMjAwLTA3NDExMTEw" _
            & "NzI1MSQxMTQ2NGYyOS0wZGU3LTRkYTMtOTk1NS02N2EwNWQwYmRlMGEkZTg5NTFmNDUtMzY1OS00YzdiLWJj" _
            & "NjItYTdiZmU4ZjEzMGNjJGFkZDAwMDQ0LTAwMDAtMDIxMC0wMDY5LTA3NjA5NzExNzI1NCRhZGQwMDA0NS0wMD" _
            & "AwLTAxMDAtMDA5MC0wNzYxMDExMTgxMDUkYWRkMDAwNTMtMDAwMC0wMTExLTAwODYtMDc3MDk3MT" _
            & "E1MTA1JGFkZDAwMDYyLTAwMDAtMDIxMC0wMDY1LTA3ODExMTExNDEwNyRhZGQ4ODg4MC0xMDAxLTAwM" _
            & "DAtYjcxZC00MGExYWI1ZGZiMjIkYWRkMDAwNjctMDAwMC0wMTEwLTAwNjUtMDgwMTA1MTA3MTAxJGM3Mz" _
            & "NiNzg1LWY3OTctNGIzYi05NDNkLWIwY2M3ZjEzY2Y2MSRhZGQwMDA3My0wMDAwLTAyMTAtMDA2NS0wODIw" _
            & "OTcxMDcwOTckN2E0ZGYzMzQtNWMxOS00MTllLThiMWItMTVkZjM2MmJkNGViJDkxY2E3ZTQwLWZhYTMtNDk" _
            & "5OC1iZGQwLWU1OGQ3NzRjMWM1NyRhZGQwMDA4Ni0wMDAwLTAxMDAtMDA2NS0wODMxMDUxMDkxMTAkYWRk" _
            & "MDAwODctMDAwMC0wMTEwLTAwNjUtMDgzMTA1MTE0MTE4JDBiNjkzNzllLWZhMmItNDYzOC1hODEwLWM2Mz" _
            & "c5ZDg5ZGQyNCRhZGQwMDA4MS0wMDAwLTAxMDAtMDA3NC0yMDgxMDExMTQxMDckZmU5MzRhOTQtYzFjNy00" _
            & "NjdkLWEwZDUtMTQ5M2I0NzkwYTA3JGM2MWU5NTQyLTkwNTMtNDhmNS1iOGQxLWVkOGQ5NTIxZTk2YyRhZ" _
            & "GQwMDEwNC0wMDAwLTAyMTAtMDA4MC0yMjIxMDExMDUxMDkUKwMkZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ" _
            & "2dnZ2dnZ2dnZ2dnZGQCHQ9kFgICAQ9kFgJmDxAPFgYfAwULcGF2YWRpbmltYXMfBAUCaWQfBWcWAh8GBR9LZX" _
            & "lEb3duSGFuZGxlcihjdGwwM19jbWRTZWFyY2gpEBUtJy0tIHBhc2lyaW5raXRlIHByb2Nlc28gZGFseXZpbyB0" _
            & "aXDEhSAtLQphcGVsaWFudGFzEmFwZWxpYW50byBhdHN0b3Zhc0Rhc21lbnMgcGFkYXZ1c2lvIHByY" _
            & "cWheW3EhS9wYXJlacWha2ltxIUgYXRuYXVqaW50aSBwcm9jZXPEhSBhdHN0b3Zhc1xhc21lbnMs" _
            & "IGt1cmlhbSBuZXBhc2tpcnRvcyBwcml2ZXLEjS4gbWVkLiBwcmllbW9uxJdzIMWhZWltb3MgbmFyaWFp" _
            & "LCBhcnRpbWllamksIGdpbWluYWnEjWlhaT5hc21lbnMsIGt1cmlhbSBuZXBhc2tpcnRvcyBwcml2ZXLE" _
            & "jS4gbWVkLiBwcmllbW9uxJdzLCBhdHN0b3Zhcz5hc21lbnMsIGt1cmlhbSBuZXBhc2tpcnRvcyBwcml2ZX" _
            & "LEjS4gbWVkLiBwcmllbW9uxJdzLCBneW7El2phczxhc21lbnMsIGt1cmlhbSBwYXNraXJ0b3MgcHJpdmVy" _
            & "xI0uIG1lZC4gcHJpZW1vbsSXcywgZ3luxJdqYXNbYXNtZW5zLCBrdXJpYW0gcGFza2lydG9zIHBya" _
            & "XZlcsSNLiBtZWQuIHByaWVtb27El3MsIMWhZWltb3MgbmFyaWFpLCBhcnRpbWllamksIGdpbWluYWnEj" _
            & "WlhaTRhc21lbnMsIGt1cmlvIHR1cnRhcyAocGluaWdhaSkga29uZmlza3VvdGksIGF0c3RvdmFzJ2FzbXV" _
            & "vLCBhdHNpbGllcMSZcyDEryBrYXNhY2luxK8gc2t1bmTEhTNhc211bywga3VyaWFtIG5lcGFza2lydG9zIHBy" _
            & "aXZlcsSNLiBtZWQuIHByaWVtb27El3MpYXNtdW8sIGt1cmlvIHR1cnRhcyAocGluaWdhaSkga29uZmlza3Vv" _
            & "dGk6YXNtdW8sIHBhZGF2xJlzIHByYcWheW3EhS9wYXJlacWha2ltxIUgYXRuYXVqaW50aSBwcm9jZXPEhS1" _
            & "hc211by9pbnN0aXR1Y2lqYSB2aWXFoWFqYW0gaW50ZXJlc3VpIGFwZ2ludGkJYXRzYWtvdmFzEWF0c2Frb3Z" _
            & "vIGF0c3RvdmFzCWVrc3BlcnRhcwlpZcWha292YXMRaWXFoWtvdm8gYXRzdG92YXMOacWhaWXFoWtvdG9qYXM" _
            & "WacWhaWXFoWtvdG9qbyBhdHN0b3ZhcyHEr3N0YXR5bWluaXMgYXRzdG92YXMgKGdsb2LEl2phcykdacWhdmF" _
            & "kxIUgZHVvZGFudGkgaW5zdGl0dWNpamEUa2FzYXRvcmlhdXMgYXRzdG92YXMKa2FzYXRvcml1cyBraXRhcyBhc" _
            & "211byAobmUgcHJvY2VzbyBkYWx5dmlzKQprdXJhdG9yaXVzCmxpdWR5dG9qYXMNcGFyZWnFoWvEl2phcxVwYXJ" _
            & "lacWha8SXam8gYXRzdG92YXMqcHJpZSBhcGVsaWFjaW5pbyBza3VuZG8gcHJpc2lkxJdqxJlzIGFzbXVvNXByaW" _
            & "UgYXBlbGlhY2luaW8gc2t1bmRvIHByaXNpZMSXanVzaW8gYXNtZW5zIGF0c3RvdmFzKHByaWUgYXRza2lyb2" _
            & "pvIHNrdW5kbyBwcmlzaWTEl2rEmXMgYXNtdW8zcHJpZSBhdHNraXJvam8gc2t1bmRvIHByaXNpZMSXanVzaW8g" _
            & "YXNtZW5zIGF0c3RvdmFzC3Nrb2xpbmlua2FzE3Nrb2xpbmlua28gYXRzdG92YXMUc3VpbnRlcmVzdW90YX" _
            & "MgYXNtdW8dc3VpbnRlcmVzdW90byBhc21lbnMgYXRzdG92YXMQdHJlxI1pYXNpcyBhc211by90cmXEjWlh" _
            & "c2lzIGFzbXVvIGJlIHNhdmFyYW5racWha8WzIHJlaWthbGF2aW3FszF0cmXEjWlhc2lzIGFzbXVvIHN1IHN" _
            & "hdmFyYW5racWha2FpcyByZWlrYWxhdmltYWlzGXRyZcSNaW9qbyBhc21lbnMgYXRzdG92YXMRdcW%2Bc3Rh" _
            & "dG8gZGF2xJdqYXMZdcW%2Bc3RhdG8gZGF2xJdqbyBhdHN0b3ZhcxUtATACNTgCNTkCNzkDMTEyAzExMAMxMT" _
            & "EDMTA3AzEwOAMxMDYDMTEzAzEwOQMxMDUCNzgCMzMBMgE2Ajg1ATEBNQI5NwI5OAIzOAI5OQI2NgIzOQE5Aj" _
            & "M3AjQwATMCMTECNjACNjECNjICNjMCMzYCNjUCMzECMTUDMTM2AjU3AjU2AjMyAzEwMwMxMDQUKwMtZ2dnZ2" _
            & "dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnFgFmZAIfD2QWAgIBD2QWAgIBDw9kFg" _
            & "IfBgUfS2V5RG93bkhhbmRsZXIoY3RsMDNfY21kU2VhcmNoKWQCIQ8QD2QWAh8GBR9LZXlEb3duSGFuZGxlcih" _
            & "jdGwwM19jbWRTZWFyY2gpZGRkAiIPEA9kFgIfBgUfS2V5RG93bkhhbmRsZXIoY3RsMDNfY21kU2VhcmNoKW" _
            & "RkZAIsDzwrAAsAZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAgUKaWJ0blNlYXJjaAUOY3R" _
            & "sMDMkY2hrRWlsTnJE5JFWhpg8Uf59Bflne4wAJYppYw%3D%3D&__EVENTVALIDATION=%2FwEWXQKso8%2FV" _
            & "CwKZ7YzdCgKPnrNoAqyI14kPAoamxrQEAr7b35sLAtThvfUGAtXhvfUGArzb35sLAreLtZUHAq%2BL%2BZY" _
            & "HAq%2BLwZYHAqyLtZUHAq2LtZUHAq%2BLmZUHAq%2BLlZUHAq%2BLxZYHAq%2BL0ZYHAr2UjqcHAqfAvcEJ" _
            & "AvS41MkNAvS4wO4EAvS4%2FLMPAp%2BBzqoIAp%2BB%2Bk8Cn4HWpgYCn4HCyw4Cn4H%2BkAECn4HqtQgC" _
            & "n4GGWQKfgbL%2BCwKfga6DAgKfgdqoBQLb4rNeAtvir%2BMLAtvim9oOAtvit%2F8BAtvio4QIArmioNEJAqWi" _
            & "1NIJAqWi0NIJAqWi3NIJAqCi7NIJAv2h74wGAuPUxsgFAv6hyIwDAr2C8c8PAvmCsqQHAsD5%2BYgJArrd%2F" _
            & "aYCApWf9tAOArrXjZMNAqf8w8kHAv%2B3kLkCAu2O%2BcIBAvrb%2BC8Cx%2FKHwAMC45qf6AMCgOKd5gsC7s" _
            & "P1yQoC0Mv14QsCyPCggwcCo5rEwAMCkem8jgICnvvO0AECzpbdhw4CxM7agwECgOuSjQcCtciQuQoCk%2Fr" _
            & "y1AYC%2Buy9%2BgkC94a7qg0Cj%2F6C8g0C97iIlAsCnK%2FpbQL3jZzJDwKFmvivBgLOjbC6DAKy3sP5BwKXw" _
            & "7qvBQKtj5%2B9BALq2oCOAwKN9J%2FlDAKm6sGJAQLCyYbwCwL4svfJCgLc%2B6b1BQL1krZjAoGlvIsGAujmw" _
            & "uoEApXK45UNAqrrztkFArKO%2BKMPNtIScGocy3U8aH8u9WlLYYeeXMg%3D&txtSearch=&ctl03%24" _
            & "cmbBTipas=2_3&ctl03%24cmbKlasifikatoriai=14&ctl03%24txtKategorijos=&ctl03%24cmbMetai=0" _
            & "&ctl03%24cmbNumerioSablonas=0&ctl03%24txtNumeris=&ctl03%24txt" _
            & "IsnagNuo=" & ConvertDateToString(AtDate) & "&ctl03%24txtIsnagIki=" & ConvertDateToString(AtDate) _
            & "&ctl03%24cmbTeisejai=00000000-0000-0000-0000-000000000000&ctl03%24TextBox1=&" _
            & "ctl03%24cmbSort=standartin%C4%99+tvark%C4%85&ctl03%24cmbSortKryptis=desc&ctl03%24cmdSearch=Rasti")
        request.ContentLength = byteArray.Length
        Dim newStream As IO.Stream = request.GetRequestStream()
        newStream.Write(byteArray, 0, byteArray.Length)
        newStream.Close()

        Return request

    End Function

    Public Sub ParseResultForLat(ByVal WebResponseString As String, _
        ByVal FolderPath As String, ByVal CurrentBackgroundWorker _
        As System.ComponentModel.BackgroundWorker, ByRef FilesDownloaded As Integer, _
        ByVal CurrentProgressPercentage As Integer)

        Dim d As New HtmlDocument
        d.LoadHtml(WebResponseString.Trim)

        Dim StructuredTable As DataSet = ConvertHtmlToDataSet(WebResponseString, True)

        If StructuredTable.Tables(7).Rows.Count > 1 Then

            StructuredTable.Tables(7).Rows.RemoveAt(0)

            Dim cregex As Regex = New Regex("href\s*=\s*(?:""(?<1>[^""]*)""|(?<1>\S+))", _
                RegexOptions.IgnoreCase Or RegexOptions.Compiled)
            Dim FileName As String
            Dim version As Integer

            For Each dr As DataRow In StructuredTable.Tables(7).Rows

                If CurrentBackgroundWorker.CancellationPending Then Exit Sub

                version = 1

                For Each m As Match In cregex.Matches(dr.Item(5).ToString)

                    CurrentBackgroundWorker.ReportProgress(CurrentProgressPercentage, _
                        "Siunčiama " & dr.Item(3).ToString.Trim & " nutartis Nr. " _
                        & dr.Item(1).ToString.Trim & " (parsiųsta " & FilesDownloaded.ToString & ")...")

                    FileName = GetFilePathByDocument(FolderPath, _
                        ConvertStringToDate(dr.Item(3).ToString), dr.Item(1).ToString.Trim, _
                        version, ".doc")
                    version += 1

                    Dim client As New WebClient
                    client.DownloadFile("http://www2.lat.lt/lat_web_test/" & _
                        m.Value.Replace("href=", "").Replace("""", ""), FileName)

                    FilesDownloaded += 1

                    If CurrentBackgroundWorker.CancellationPending Then Exit Sub

                Next

            Next

        End If

    End Sub

    Private Function GetFilePathByDocument(ByVal ParentFolder As String, _
        ByVal DocumentDate As Date, ByVal DocumentNumber As String, _
        ByVal DocumentVersion As Integer, ByVal FileExtension As String) As String

        Dim result As String = ParentFolder & "\" & DocumentDate.Year.ToString & _
            "-" & DocumentDate.Month.ToString
        If Not IO.Directory.Exists(result) Then IO.Directory.CreateDirectory(result)
        If Not DocumentNumber.Contains(DocumentDate.Year.ToString) Then _
            DocumentNumber = DocumentNumber & "-" & DocumentDate.Year.ToString
        DocumentNumber = DocumentNumber.ToLower.Replace("3k3", "3k-3")
        result = result & "\" & DocumentNumber.Replace("/", "-") & "_" & _
            ConvertDateToString(DocumentDate)
        If DocumentVersion > 1 Then result = result & "(" & DocumentVersion.ToString & ")"
        result = result & FileExtension

        Return result

    End Function


    Public Sub DownloadLiteko(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)

        If e.Argument Is Nothing OrElse Not TypeOf e.Argument Is CourtRequest Then _
            Throw New Exception("Klaida. Nenurodyti užklausos parametrai.")

        Dim requestData As CourtRequest = DirectCast(e.Argument, CourtRequest)

        If requestData.FolderPath Is Nothing OrElse String.IsNullOrEmpty(requestData.FolderPath.Trim) Then _
            Throw New ArgumentNullException("Klaida. Nenurodytas folderis nutartims išsaugoti.")

        Dim request As HttpWebRequest
        Dim response As HttpWebResponse

        Dim FileDownloaded As Integer = 0
        Dim CurrentProgress As Integer

        For i As Integer = 0 To DateDiff(DateInterval.DayOfYear, requestData.DateFrom, _
            requestData.DateTo)

            CurrentProgress = Math.Ceiling(100 * i / DateDiff(DateInterval.DayOfYear, _
                requestData.DateFrom, requestData.DateTo))

            DirectCast(sender, System.ComponentModel.BackgroundWorker).ReportProgress( _
                CurrentProgress.ToString, "Gaunami duomenys apie nutartis " _
                & requestData.DateFrom.AddDays(i).ToShortDateString & " dienai...")

            request = GetRequestForLiteko(requestData.ForCourtType, requestData.DateFrom.AddDays(i).Date)

            Dim responseReceived As Boolean = False
            Dim FailedRequests As Integer = 0

            While Not responseReceived
                Try
                    response = request.GetResponse
                    responseReceived = True
                Catch ex As Exception
                    FailedRequests += 1
                    DirectCast(sender, System.ComponentModel.BackgroundWorker).ReportProgress( _
                        CurrentProgress.ToString, "Atmesta užklausa: " & ex.Message)
                    If FailedRequests > 20 Then Throw New Exception( _
                        "Klaida. Užklausa buvo atmesta daugiau kaip 20 kartų iš eilės. " _
                        & "Parsisiųsta " & FileDownloaded.ToString & " dokumentų už laikotarpį nuo " _
                        & requestData.DateFrom.ToShortDateString & " iki " _
                        & requestData.DateFrom.AddDays(i - 1).ToShortDateString & ".")
                    If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
                        e.Cancel = True
                        Exit Sub
                    End If
                    System.Threading.Thread.Sleep(1000)
                End Try
            End While

            If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If

            Using sr As New IO.StreamReader(response.GetResponseStream())
                Try
                    ParseResultForLiteko(sr.ReadToEnd, requestData.FolderPath, _
                        DirectCast(sender, ComponentModel.BackgroundWorker), _
                        FileDownloaded, CurrentProgress)
                Catch ex As Exception
                    response.Close()
                    sr.Close()
                    Throw ex
                End Try
                response.Close()
                sr.Close()
            End Using

            If DirectCast(sender, System.ComponentModel.BackgroundWorker).CancellationPending Then
                e.Cancel = True
                Exit Sub
            End If

        Next

        e.Result = FileDownloaded

    End Sub

    Public Function GetRequestForLiteko(ByVal nCourtType As CourtType, ByVal AtDate As Date) As HttpWebRequest

        Dim requestURL As String
        If nCourtType = CourtType.LitekoLatCivil Then
            requestURL = "http://liteko.teismai.lt/viesasprendimupaieska/paieska.aspx?" _
                & "detali=2&bnr=&byloseilesnr=&procesinisnr=&eilnr=False&tid=23&br=2&dr=&nuo=" _
                & ConvertDateToString(AtDate, ".") & "%2000:00:00&iki=" _
                & ConvertDateToString(AtDate, ".") & "%2023:59:59&teis=&tk=&bb=&rakt=&txt=&kat=&term="

        ElseIf nCourtType = CourtType.LitekoAdministrativeTribunal Then
            requestURL = "http://liteko.teismai.lt/viesasprendimupaieska/paieska.aspx?" _
                & "detali=2&bnr=&byloseilesnr=&procesinisnr=&eilnr=False&tid=19&br=5&dr=&nuo=" _
                & ConvertDateToString(AtDate, ".") & "%2000:00:00&iki=" _
                & ConvertDateToString(AtDate, ".") & "%2023:59:59&teis=&tk=&bb=&rakt=&txt=&kat=&term="

        End If

        Dim request As HttpWebRequest = WebRequest.Create(requestURL)

        request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:11.0) Gecko/20100101 Firefox/15.0"
        request.Accept = "Accept: image/gif, image/jpeg, image/pjpeg, image/pjpeg, application/x-shockwave-flash, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*"
        request.Headers.Add("Accept-Language: en-us")
        request.Headers.Add("Accept-Encoding: gzip, deflate")
        request.KeepAlive = True
        request.CookieContainer = New CookieContainer()
        request.Method = WebRequestMethods.Http.Get
        request.Referer = "http://liteko.teismai.lt/viesasprendimupaieska/detalipaieska.aspx?detali=2"

        Return request

    End Function

    Public Sub ParseResultForLiteko(ByVal WebResponseString As String, _
        ByVal FolderPath As String, ByVal CurrentBackgroundWorker _
        As System.ComponentModel.BackgroundWorker, ByRef FilesDownloaded As Integer, _
        ByVal CurrentProgressPercentage As Integer)

        Dim d As New HtmlDocument
        d.LoadHtml(WebResponseString.Trim)

        If d.DocumentNode.SelectNodes("//tr[@class='rlvI']") Is Nothing _
            OrElse d.DocumentNode.SelectNodes("//tr[@class='rlvI']").Count < 1 Then Exit Sub

        Dim FileName As String
        Dim DocumentNumber As String
        Dim DocumentDate As Date
        Dim version As Integer
        Dim DownloadedNumbers As New List(Of String)

        For Each row As HtmlNode In d.DocumentNode.SelectNodes("//tr[@class='rlvI']")

            If CurrentBackgroundWorker.CancellationPending Then Exit Sub

            If Not row.SelectNodes("td") Is Nothing AndAlso row.SelectNodes("td").Count > 1 _
                AndAlso Not row.SelectNodes("td")(1).SelectNodes("*[@href]") Is Nothing _
                AndAlso row.SelectNodes("td")(1).SelectNodes("*[@href]").Count > 0 Then

                For Each fileRef As HtmlNode In row.SelectNodes("td")(1).SelectNodes("*[@href]")

                    DocumentNumber = row.SelectNodes("td")(1).SelectNodes("span")(2).InnerText
                    DocumentDate = ConvertStringToDate(row.SelectNodes("td")(1). _
                        SelectNodes("span")(0).InnerText)

                    version = GetVersion(DownloadedNumbers, DocumentNumber)
                    FileName = GetFilePathByDocument(FolderPath, DocumentDate, _
                        DocumentNumber, version, ".html")

                    CurrentBackgroundWorker.ReportProgress(CurrentProgressPercentage, _
                        "Siunčiama " & DocumentDate.ToShortDateString & " nutartis Nr. " _
                        & DocumentNumber & " (parsiųsta " & FilesDownloaded.ToString & ")...")

                    Dim client As New WebClient
                    client.DownloadFile("http://liteko.teismai.lt/viesasprendimupaieska/" & _
                        fileRef.GetAttributeValue("href", ""), FileName)

                    AddNumberWithVersion(DownloadedNumbers, _
                        row.SelectNodes("td")(1).SelectNodes("span")(2).InnerText, version)

                    FilesDownloaded += 1

                    If CurrentBackgroundWorker.CancellationPending Then Exit Sub

                Next

            End If

        Next

    End Sub

    Private Function GetVersion(ByVal DownloadedNumbers As List(Of String), _
        ByVal CurrentNumber As String) As Integer

        Dim result As Integer = 1

        For Each s As String In DownloadedNumbers
            If s.Trim.ToLower.StartsWith(CurrentNumber.Trim.ToLower) Then result += 1
        Next

        Return result

    End Function

    Private Sub AddNumberWithVersion(ByRef DownloadedNumbers As List(Of String), _
        ByVal CurrentNumber As String, ByVal CurrentVersion As Integer)

        If Not CurrentVersion > 1 Then
            DownloadedNumbers.Add(CurrentNumber)
        Else
            DownloadedNumbers.Add(CurrentNumber & "(" & CurrentVersion.ToString & ")")
        End If

    End Sub


    Public Sub CheckForAvailableUpdatesAsync(ByVal AvailableUpdateDateFileUrl As String, _
        ByRef OnDataFetchedFromWeb As System.Net.DownloadDataCompletedEventHandler)

        If AvailableUpdateDateFileUrl Is Nothing OrElse String.IsNullOrEmpty(AvailableUpdateDateFileUrl.trim) Then Exit Sub

        Dim updateUrl As New Uri(AvailableUpdateDateFileUrl.Trim)

        Dim client As New System.Net.WebClient
        AddHandler client.DownloadDataCompleted, OnDataFetchedFromWeb

        client.DownloadDataAsync(updateUrl)

    End Sub

    ''' <summary>
    ''' Evaluates data fetched by CheckForAvailableUpdatesAsync method, compares
    ''' fetched remote date with the local file date and returns remote date. 
    ''' </summary>
    ''' <param name="FileUrl">Url of a remote text file containing available update date.</param>
    ''' <param name="ComparisionFilePath">Path to local text file 
    ''' containing the last installed update date.</param>
    ''' <param name="FileEncoding">Local text file encoding.</param>
    ''' <param name="RemoteFileEncoding">Remote text file encoding.</param>
    ''' <param name="DateConversionFunction">If date string format is NOT YYYY-MM-DD, 
    ''' function for conversion should be provided. Else set to nothing.</param>
    ''' <param name="e">DownloadDataCompletedEventArgs object returned by WebClient
    ''' and containing info about remote text file with the available update date.</param>
    ''' <returns>Available update date if it is later then last installed update date.
    ''' Else returns Date.MinValue</returns>
    Public Function ResolveAvailableUpdatesResult(ByVal FileUrl As String, _
        ByVal ComparisionFilePath As String, ByVal FileEncoding As System.Text.Encoding, _
        ByVal RemoteFileEncoding As System.Text.Encoding, _
        ByVal DateConversionFunction As ConvertStringToDateDelegate, _
        ByVal e As System.Net.DownloadDataCompletedEventArgs, _
        ByVal ThrowOnException As Boolean) As Date

        If DateConversionFunction Is Nothing Then _
            DateConversionFunction = AddressOf ConvertStringToDate
        If FileEncoding Is Nothing Then FileEncoding = System.Text.Encoding.UTF8
        If RemoteFileEncoding Is Nothing Then RemoteFileEncoding = System.Text.Encoding.Unicode

        Try

            If e.Cancelled Then
                Throw New Exception("Duomenų gavimas buvo atšauktas.")

            ElseIf Not e.Error Is Nothing Then
                Throw New Exception("Klaida gaunant duomenis: " & e.Error.Message, e.Error)

            ElseIf e.Result Is Nothing OrElse e.Result.Length < 2 Then
                Throw New Exception("Klaida gaunant duomenis: " & e.Error.Message, e.Error)

            Else

                Dim LocalDateString As String = Nothing
                Dim RemoteDateString As String = Nothing
                Dim LocalDate As Date = Date.MinValue
                Dim RemoteDate As Date = Date.MinValue

                Try
                    LocalDateString = IO.File.ReadAllText(ComparisionFilePath, FileEncoding).Trim
                Catch ex As Exception
                    Throw New Exception("Klaida nuskaitant arba dekoduojant datos failo '" _
                        & ComparisionFilePath & "' duomenis: " & ex.Message, ex)
                End Try

                If LocalDateString Is Nothing OrElse String.IsNullOrEmpty(LocalDateString.Trim) Then _
                    Throw New Exception("Klaida nuskaitant arba dekoduojant datos failo '" _
                        & ComparisionFilePath & "' duomenis: failas tuščias.")

                Try
                    RemoteDateString = RemoteFileEncoding.GetString(e.Result).Trim
                Catch ex As Exception
                    Throw New Exception("Klaida nuskaitant arba dekoduojant nutolusio failo '" _
                        & FileUrl & "' duomenis: " & ex.Message, ex)
                End Try

                If RemoteDateString Is Nothing OrElse String.IsNullOrEmpty(RemoteDateString.Trim) Then _
                    Throw New Exception("Klaida nuskaitant nutolusio failo '" & FileUrl _
                        & "' duomenis: negauta jokių duomenų.")

                Try
                    LocalDate = DateConversionFunction.Invoke(LocalDateString)
                Catch ex As Exception
                    Throw New Exception("Klaida konvertuojant datos failo '" _
                        & ComparisionFilePath & "' duomenis '" & LocalDateString.Trim _
                        & "' į datą: " & ex.Message, ex)
                End Try

                If LocalDate = Date.MinValue OrElse LocalDate = Date.MaxValue Then _
                    Throw New Exception("Klaida konvertuojant datos failo '" _
                        & ComparisionFilePath & "' duomenis '" & LocalDateString.Trim & "'į datą.")

                Try
                    RemoteDate = DateConversionFunction.Invoke(RemoteDateString)
                Catch ex As Exception
                    Throw New Exception("Klaida konvertuojant nutolusio failo '" _
                        & FileUrl & "' duomenis '" & RemoteDateString.Trim & "' į datą: " _
                        & ex.Message, ex)
                End Try

                If RemoteDate = Date.MinValue OrElse RemoteDate = Date.MaxValue Then _
                    Throw New Exception("Klaida konvertuojant nutolusio failo '" _
                        & FileUrl & "' duomenis '" & RemoteDateString.Trim & "' į datą.")

                If RemoteDate.Date > LocalDate.Date Then Return RemoteDate.Date

            End If

        Catch ex As Exception
            If ThrowOnException Then Throw ex
        End Try

        Return Date.MinValue

    End Function



    Public Delegate Function ConvertStringToDateDelegate(ByVal StringDate As String) As Date

    Public Function ConvertHtmlToDataSet(ByVal HtmlText As String, _
        Optional ByVal AddWithHtmlNode As Boolean = False) As DataSet

        Dim result As New DataSet

        Dim d As New HtmlDocument
        d.LoadHtml(HtmlText)

        For Each table As HtmlNode In d.DocumentNode.SelectNodes("//table")
            If Not table.SelectNodes("tr") Is Nothing Then _
                result.Tables.Add(ConvertHtmlTableToDataTable(table, AddWithHtmlNode))
        Next

        Return result

    End Function

    Public Function ConvertHtmlTableToDataTable(ByVal Table As HtmlNode, _
        ByVal AddWithHtmlNode As Boolean) As DataTable

        Dim result As New DataTable

        Dim colCount As Integer
        Dim dRow As DataRow

        If Not Table.SelectNodes("tr") Is Nothing Then
            For Each row As HtmlNode In Table.SelectNodes("tr")
                result.Rows.Add()
                dRow = result.Rows(result.Rows.Count - 1)
                colCount = 0
                If Not row.SelectNodes("th|td") Is Nothing Then
                    For Each cell As HtmlNode In row.SelectNodes("th|td")
                        colCount += 1
                        If colCount > result.Columns.Count Then result.Columns.Add()
                        cell.InnerHtml = cell.InnerHtml.Replace("<BR>", vbCrLf). _
                            Replace("<br>", vbCrLf).Replace("<Br>", vbCrLf)
                        If AddWithHtmlNode Then
                            dRow.Item(colCount - 1) = cell.InnerHtml
                        Else
                            dRow.Item(colCount - 1) = cell.InnerText
                        End If
                    Next
                End If
            Next
        End If

        Return result

    End Function

    Private Function ConvertDateToString(ByVal DateToConvert As Date, _
        Optional ByVal Separator As String = "-") As String

        Dim result As String = DateToConvert.Year.ToString & Separator

        If DateToConvert.Month < 10 Then
            result += "0" & DateToConvert.Month.ToString & Separator
        Else
            result += DateToConvert.Month.ToString & Separator
        End If
        If DateToConvert.Day < 10 Then
            result += "0" & DateToConvert.Day.ToString
        Else
            result += DateToConvert.Day.ToString
        End If

        Return result

    End Function

    Friend Function ConvertStringToDate(ByVal StringDate As String) As Date

        Return New Date(Integer.Parse(StringDate.Trim.Substring(0, 4)), _
            Integer.Parse(StringDate.Trim.Substring(5, 2)), _
            Integer.Parse(StringDate.Trim.Substring(8, 2)))

    End Function

End Module
