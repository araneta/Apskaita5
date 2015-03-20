Namespace Documents

    <Serializable()> _
    Public Class AccumulativeCostsItemList
        Inherits BusinessListBase(Of AccumulativeCostsItemList, AccumulativeCostsItem)

#Region " Business Methods "

        Protected Overrides Function AddNewCore() As Object
            Dim NewItem As AccumulativeCostsItem = AccumulativeCostsItem.NewAccumulativeCostsItem
            Me.Add(NewItem)
            Return NewItem
        End Function


        Public Function GetAllBrokenRules() As String
            Dim result As String = GetAllBrokenRulesForList(Me)

            'Dim GeneralErrorString As String = ""
            'SomeGeneralValidationSub(GeneralErrorString)
            'AddWithNewLine(result, GeneralErrorString, False)

            Return result
        End Function

        Public Function GetAllWarnings() As String

            Dim result As String = GetAllWarningsForList(Me)

            'Dim GeneralErrorString As String = ""
            'SomeGeneralValidationSub(GeneralErrorString)
            'AddWithNewLine(result, GeneralErrorString, False)

            Return result
        End Function


        Friend Function GetMinDate() As Date
            Dim result As Date = Date.MaxValue
            For Each a As AccumulativeCostsItem In Me
                If a.Date.Date < result.Date Then result = a.Date.Date
            Next
            Return result
        End Function


        Friend Sub Distribute(ByVal sumToDistribute As Double)

            If Me.Count < 1 Then Throw New Exception("Klaida. Neįvestas nė vienas periodas.")
            If Not CRound(sumToDistribute) > 0 Then Throw New Exception("Klaida. Nenurodyta sukauptų sąnaudų suma.")

            For Each a As AccumulativeCostsItem In Me
                If Not a.ChronologyValidator.FinancialDataCanChange Then Throw New Exception( _
                    "Ne visos paskirstymo eilutės gali būti keičiamos dėl vėlesnėmis datomis " _
                    & "registruotų uždarymų.")
            Next

            If Me.Count = 1 Then
                Me.Item(0).Sum = sumToDistribute
                Exit Sub
            End If

            Me.RaiseListChangedEvents = False

            Dim itemValue As Double = CRound(sumToDistribute / Me.Count)
            Dim lastItemCorrection As Double = CRound(CRound(sumToDistribute) - CRound(CRound(itemValue) * Me.Count))

            For i As Integer = 1 To Me.Count
                Me.Item(i - 1).Sum = itemValue
            Next

            Me.Item(Me.Count - 1).Sum = Me.Item(Me.Count - 1).Sum + lastItemCorrection

            Me.RaiseListChangedEvents = True

            Me.ResetBindings()

        End Sub

        Public Sub Distribute(ByVal sumToDistribute As Double, ByVal StartingDate As Date, _
            ByVal PeriodLength As Integer, ByVal PeriodCount As Integer)

            If Not CRound(sumToDistribute) > 0 Then Throw New Exception("Klaida. Nenurodyta sukauptų sąnaudų suma.")

            For Each a As AccumulativeCostsItem In Me
                If Not a.ChronologyValidator.FinancialDataCanChange Then Throw New Exception( _
                    "Ne visos paskirstymo eilutės gali būti keičiamos dėl vėlesnėmis datomis " _
                    & "registruotų uždarymų.")
            Next

            If PeriodLength < 1 Then PeriodLength = 1
            If PeriodCount < 1 Then PeriodCount = 1

            Me.RaiseListChangedEvents = False

            Me.Clear()

            If PeriodCount = 1 Then

                Dim newItem As AccumulativeCostsItem = AccumulativeCostsItem.NewAccumulativeCostsItem
                newItem.Sum = sumToDistribute
                newItem.Date = StartingDate
                Me.Add(newItem)

            Else

                Dim itemValue As Double = CRound(sumToDistribute / PeriodCount)
                Dim lastItemCorrection As Double = CRound(CRound(sumToDistribute) - CRound(CRound(itemValue) * PeriodCount))

                For i As Integer = 1 To PeriodCount

                    Dim newItem As AccumulativeCostsItem = AccumulativeCostsItem.NewAccumulativeCostsItem
                    newItem.Sum = itemValue
                    newItem.Date = StartingDate.AddMonths(PeriodLength * (i - 1))
                    Me.Add(newItem)

                Next

                Me.Item(Me.Count - 1).Sum = Me.Item(Me.Count - 1).Sum + lastItemCorrection

            End If

            Me.RaiseListChangedEvents = True

            Me.ResetBindings()

        End Sub


#End Region

#Region " Factory Methods "

        Friend Shared Function NewAccumulativeCostsItemList() As AccumulativeCostsItemList
            Return New AccumulativeCostsItemList
        End Function

        Friend Shared Function GetAccumulativeCostsItemList(ByVal ParentID As Integer) As AccumulativeCostsItemList
            Return New AccumulativeCostsItemList(ParentID)
        End Function


        Private Sub New()
            ' require use of factory methods
            MarkAsChild()
            Me.AllowEdit = True
            Me.AllowNew = True
            Me.AllowRemove = True
        End Sub

        Private Sub New(ByVal ParentID As Integer)
            ' require use of factory methods
            MarkAsChild()
            Me.AllowEdit = True
            Me.AllowNew = True
            Me.AllowRemove = True
            Fetch(ParentID)
        End Sub

#End Region

#Region " Data Access "

        Private Sub Fetch(ByVal ParentID As Integer)

            Dim myComm As New SQLCommand("FetchAccumulativeCostsItemList")
            myComm.AddParam("?PD", ParentID)

            Using myData As DataTable = myComm.Fetch

                Using closingsDataSource As DataTable = SimpleChronologicValidator.GetClosingsDataSource

                    RaiseListChangedEvents = False

                    For Each dr As DataRow In myData.Rows
                        Add(AccumulativeCostsItem.GetAccumulativeCostsItem(dr, closingsDataSource))
                    Next

                    RaiseListChangedEvents = True

                End Using

            End Using

        End Sub

        Friend Sub Update(ByVal parent As AccumulativeCosts)

            RaiseListChangedEvents = False

            For Each item As AccumulativeCostsItem In DeletedList
                If Not item.IsNew Then item.DeleteSelf()
            Next
            DeletedList.Clear()

            For Each item As AccumulativeCostsItem In Me
                If item.IsNew Then
                    item.Insert(parent)
                ElseIf item.IsDirty OrElse parent.IsAccumulatedIncomeHasChanged Then
                    item.Update(parent)
                End If
            Next

            RaiseListChangedEvents = True

        End Sub

        Friend Sub Delete(ByVal parent As AccumulativeCosts)

            Dim myComm As New SQLCommand("DeleteAccumulativeCostsItemList")
            myComm.AddParam("?PD", parent.ID)

            myComm.Execute()

            For Each a As AccumulativeCostsItem In Me
                General.JournalEntry.DeleteJournalEntryChild(a.ID)
            Next

        End Sub

        Friend Sub CheckIfCanUpdate(ByVal parent As AccumulativeCosts)

            For Each a As AccumulativeCostsItem In Me.DeletedList
                If Not a.IsNew Then
                    If Not a.IsNew AndAlso Not a.ChronologyValidator.FinancialDataCanChange Then _
                        Throw New Exception("Negalima pašalinti eilutės '" & a.ToString & "':" _
                        & vbCrLf & a.ChronologyValidator.FinancialDataCanChangeExplanation)
                    IndirectRelationInfoList.CheckIfJournalEntryCanBeDeleted(a.ID, DocumentType.AccumulatedCosts)
                End If
            Next

            For Each a As AccumulativeCostsItem In Me
                If Not a.IsValid Then Throw New Exception("Negalima pakeisti eilutės '" _
                    & a.ToString & "':" & vbCrLf & a.BrokenRulesCollection.ToString(Validation.RuleSeverity.Error))
                a.PrepareChildJournalEntry(parent)
            Next

        End Sub

        Friend Sub CheckIfCanDelete()
            For Each a As AccumulativeCostsItem In Me
                If Not a.IsNew Then
                    If Not a.ChronologyValidator.FinancialDataCanChange Then _
                        Throw New Exception("Negalima pašalinti eilutės '" & a.ToString & "':" _
                            & vbCrLf & a.ChronologyValidator.FinancialDataCanChangeExplanation)
                    IndirectRelationInfoList.CheckIfJournalEntryCanBeDeleted(a.ID, DocumentType.AccumulatedCosts)
                End If
            Next
        End Sub

#End Region

    End Class

End Namespace