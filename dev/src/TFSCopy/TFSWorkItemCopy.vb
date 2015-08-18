Imports Microsoft.TeamFoundation.Client
Imports Microsoft.TeamFoundation.Framework.Common
Imports Microsoft.TeamFoundation.WorkItemTracking.Client
Imports TFSCopy

Public Class TFSWorkItemCopy

    Private Class ADWorkItemVersion
        Public Shared Function FromKeyString(key As String) As ADWorkItemVersion
            Dim ret As New ADWorkItemVersion
            Dim vals = key.Split("@"c)
            If vals.Length <> 2 Then Throw New ArgumentException("key is invalid")
            ret.ID = Integer.Parse(vals(0))
            ret.RevIndex = Integer.Parse(vals(1))
            Return ret
        End Function


        Public Function ToKeyString() As String
            Return ToKeyString(ID, RevIndex)
        End Function

        Public Shared Function ToKeyString(sourceID As Integer, revision As Integer) As String
            Return String.Format("{0:00000000000000000000}@{1:00000000000000000000}", sourceID, revision)
        End Function
        Public Shared Function Create(sourceID As Integer, revision As Integer) As ADWorkItemVersion
            Dim ret As New ADWorkItemVersion
            ret.ID = sourceID
            ret.RevIndex = revision
            Return ret
        End Function

        Private Sub New()
        End Sub


        Property ID As Integer
        Property RevIndex As Integer     ' Index der Liste Workitem.Revisions
    End Class


    ''' <summary>
    ''' Class is NOT Thread Safe!!!!!!
    ''' </summary>
    Private Class IDMapper
        Private myMapping As New Dictionary(Of Integer, Integer?)

        Public Sub New()
        End Sub

        ''' <summary>
        ''' if mapping is somehow active on this Workitem with the given ID
        ''' </summary>
        ''' <param name="sourceID"></param>
        ''' <returns></returns>
        Public Function IsMapping(sourceID As Integer) As Boolean
            If myMapping.ContainsKey(sourceID) Then
                Dim val = myMapping.Item(sourceID)
                Return Not val.HasValue
            End If
            Return False
        End Function

        Public Sub StartMapping(sourceID As Integer)
            If myMapping.ContainsKey(sourceID) Then
                Dim val = myMapping.Item(sourceID)
                If Not val.HasValue Then
                    Throw New InvalidOperationException("Mapping already started")
                Else
                    Throw New InvalidOperationException("Mapping already done")
                End If
            End If
            myMapping.Add(sourceID, Nothing)
        End Sub

        Public Function IsMapped(sourceID As Integer) As Boolean
            If myMapping.ContainsKey(sourceID) Then
                Dim val = myMapping.Item(sourceID)
                Return val.HasValue
            End If
            Return False
        End Function

        Public Sub FinishMapping(sourceID As Integer, destID As Integer)
            If Not myMapping.ContainsKey(sourceID) Then
                Throw New InvalidOperationException("Mapping not started")
            End If
            myMapping.Item(sourceID) = destID
        End Sub

        Public Function GetMappedID(sourceID As Integer) As Integer
            If myMapping.ContainsKey(sourceID) Then
                Dim val = myMapping.Item(sourceID)
                If val.HasValue Then
                    Return val.Value
                End If
            End If
            Throw New InvalidOperationException(sourceID & " is not mapped(or finished) yet....")
        End Function

        Private myAnalyzeQueue As New Queue(Of String)
        Private myAnalyzeQueued As New HashSet(Of String)
        Private myCopyListSorted As New SortedList(Of String, ADWorkItemVersion)
        Private myLinkQueue As New Queue(Of Integer)
        Public Sub Add2AnalyzeQueue(sourceID As Integer, revision As Integer)
            Dim k = ADWorkItemVersion.ToKeyString(sourceID, revision)
            If myAnalyzeQueued.Contains(k) Then
                ' nichts zu tun
            Else
                myAnalyzeQueued.Add(k)
                myAnalyzeQueue.Enqueue(k)
            End If
        End Sub



        Public Sub Add2CopyQueue(sourceID As Integer, RevIndex As Integer)
            Dim adwi = ADWorkItemVersion.Create(sourceID, RevIndex)
            If myCopyListSorted.ContainsKey(adwi.ToKeyString) Then
                ' nothing to do
            Else
                SyncLock myCopyListSorted
                    myCopyListSorted.Add(adwi.ToKeyString, adwi)
                End SyncLock

            End If
        End Sub

        Public Function NextAnalyzeItem() As ADWorkItemVersion
            If myAnalyzeQueue.Count = 0 Then
                Return Nothing
            End If
            Dim k = myAnalyzeQueue.Dequeue
            Return ADWorkItemVersion.FromKeyString(k)
        End Function

        Public Function NextCopyItem() As ADWorkItemVersion
            Dim itm As ADWorkItemVersion = Nothing
            Do
                If myCopyListSorted.Count = 0 Then
                    Return Nothing
                End If
                SyncLock myCopyListSorted
                    Dim minKey = myCopyListSorted.Keys.Min
                    itm = myCopyListSorted(minKey)
                    myCopyListSorted.Remove(minKey)
                End SyncLock
                If Not IsMapped(itm.ID) Then
                    ' wurde noch nicht bearbeitet
                    myLinkQueue.Enqueue(itm.ID)
                    Return itm
                Else
                    ' wurde bereits kopiert -> nichts mehr zu tun
                End If
            Loop
        End Function

        Public Function NextLinkItem() As Integer?
            If myLinkQueue.Count = 0 Then
                Return Nothing
            End If
            Return myLinkQueue.Dequeue
        End Function

    End Class

    Private Shared ReadOnly Field2Ignore As String() = {"Area ID", "Area Path", "Iteration ID", "Iteration Path"}

    Public Shared Sub Copy(sourceTFSCollection As TfsTeamProjectCollection, sourceProject As String, sourceTFSConfigServer As TfsConfigurationServer, destTFSCollection As TfsTeamProjectCollection, destProject As String, destTFSConfigServer As TfsConfigurationServer)

        'HACK: 
        Dim childrenEnu As Guid() = New Guid() {CatalogResourceTypes.ProjectCollection}
        Dim colls = (From item In sourceTFSConfigServer.CatalogNode.QueryChildren(childrenEnu, False, CatalogQueryOptions.None)
                     Select New TfsCollectionDisplayItem With {.DisplayItem = item.Resource.DisplayName,
                                                                       .TfsCollection = sourceTFSConfigServer.GetTeamProjectCollection(
                                                                        New Guid(item.Resource.Properties("InstanceId")))}).ToList
        'END HACK



        Dim sourceWis = New WorkItemStore(sourceTFSCollection)

        Dim destWis = New WorkItemStore(destTFSCollection, WorkItemStoreFlags.BypassRules)

        Dim sourceprojectName = sourceWis.Projects(sourceProject).Name
        Console.Write("Project: ")
        Console.WriteLine(sourceprojectName)

        Dim variables As New Dictionary(Of String, String)
        variables.Add("project", sourceprojectName)

        Dim wiQuery As New Query(sourceWis, My.Resources.TaskQuery, variables)

        Dim tasklist = wiQuery.RunQuery

        Dim idmapper As New IDMapper
        'CopyItemsRecursive(sourceWis, sourceProject, tasklist, idmapper, destWis, destProject)


        ' BLI Web API - 'CopyItemsRecursive(sourceWis, sourceProject, "   ", 1308, idmapper, destWis, destProject)

        'CopyItemsRecursive(sourceWis, sourceProject, "   ", 2485, idmapper, destWis, destProject)
        'CopyItemsRecursive(sourceWis, sourceProject, "   ", 2301, idmapper, destWis, destProject)

        idmapper.Add2AnalyzeQueue(2485, 0)
        idmapper.Add2AnalyzeQueue(2301, 0)
        'idmapper.Add2CopyQueue(1587)

        Do
            Dim nextID = idmapper.NextAnalyzeItem
            If nextID Is Nothing Then Exit Do
            FillDeps(sourceWis, sourceProject, "   ", nextID.ID, nextID.RevIndex, idmapper)
        Loop

        ' Elemente kopieren
        Do
            Dim nextID = idmapper.NextCopyItem
            If nextID Is Nothing Then Exit Do
            CopyItemsRecursive(sourceWis, sourceProject, "   ", nextID.ID, nextID.RevIndex, idmapper, destWis, destProject)
        Loop

        ' und nun die links nachpflegen
        Do
            Dim nextID = idmapper.NextLinkItem
            If Not nextID.HasValue Then Exit Do
            LinkItems(sourceWis, sourceProject, "   ", nextID.Value, idmapper, destWis, destProject)
        Loop

        'Dim attachment As New Microsoft.TeamFoundation.WorkItemTracking.Client.Attachment("c:\somefile.txt", "My Comment")

        MessageBox.Show("Done")
    End Sub

    Private Shared Sub FillDeps(sourceWis As WorkItemStore, sourceProject As String, prefix As String, sourceId As Integer, RevIndex As Integer, idmapper As IDMapper)
        Dim sourceWorkItem As WorkItem
        sourceWorkItem = sourceWis.GetWorkItem(sourceId)


        For Each rev As Revision In sourceWorkItem.Revisions
            idmapper.Add2CopyQueue(sourceWorkItem.Id, rev.Index)
            If rev.Index = RevIndex Then
                For Each linked As Link In rev.Links
                    If linked.BaseType = BaseLinkType.RelatedLink Then
                        Dim relLink = DirectCast(linked, RelatedLink)
                        Dim relID = relLink.RelatedWorkItemId
                        idmapper.Add2AnalyzeQueue(relID, 0)
                    End If
                Next
            Else
                idmapper.Add2AnalyzeQueue(sourceWorkItem.Id, rev.Index)
            End If
        Next
    End Sub

    Private Shared Sub CopyItemsRecursive(ByVal sourceWis As WorkItemStore, sourceProject As String, ByVal tasklist As WorkItemCollection, idmapper As IDMapper, destWis As WorkItemStore, destProject As String)
        For Each item As WorkItem In tasklist
            CopyItemsRecursive(sourceWis, sourceProject, "   ", item.Id, 0, idmapper, destWis, destProject)
        Next
    End Sub

    Private Shared Function CopyItemsRecursive(ByVal sourceWis As WorkItemStore, sourceProject As String, ByVal prefix As String, ByVal sourceId As Integer, RevIndex As Integer, IDMapper As IDMapper, destWis As WorkItemStore, destProject As String) As WorkItem

        Dim sourceWorkItem As WorkItem
        sourceWorkItem = sourceWis.GetWorkItem(sourceId)

        IDMapper.StartMapping(sourceId)
        Dim dstWorkItemTypes = destWis.Projects(destProject).WorkItemTypes

        Dim ret As WorkItem = Nothing

        Dim destWorkItemType = dstWorkItemTypes(sourceWorkItem.Type.Name)

        If destWorkItemType Is Nothing Then
            Throw New InvalidOperationException(sourceWorkItem.Type.Name & " didn't exists as WorkItemTyp in the destination TFS")
        End If


        ret = New WorkItem(destWorkItemType)


        For Each rev As Revision In sourceWorkItem.Revisions
            If rev.Index <> RevIndex Then Continue For

            Dim currfulldict As New Dictionary(Of String, Object)
            Dim currdeltadict As New Dictionary(Of String, Object)

            For Each f As Field In sourceWorkItem.Fields
                Dim fieldname = f.Name
                Dim val = rev.Fields(fieldname).Value

                Dim hasChanged = True

                currfulldict.Add(fieldname, val)     ' für den Vergleich brauchen wir immer den aktuellen Wert

                If rev.Index > 0 AndAlso Object.Equals(ret.Fields(fieldname), val) Then
                    hasChanged = False
                End If

                If hasChanged Then

                    If fieldname = "Related Link Count" Then

                        Dim revLinkCount = rev.Links.Count
                        Dim delta As Integer = sourceWorkItem.Links.Count - ret.Links.Count

                        For Each linked As Link In rev.Links
                            If linked.BaseType = BaseLinkType.RelatedLink Then
                                Dim relSourceLink = DirectCast(linked, RelatedLink)
                                Dim idInDest As Integer

                                idInDest = IDMapper.GetMappedID(relSourceLink.RelatedWorkItemId)
                                Dim newLink As New RelatedLink(idInDest)
                                Try
                                    ret.Links.Add(newLink)
                                Catch ve As Exception When ve.Message.StartsWith("TF237099")        ' TF237099: Duplicate work item link.
                                    ' nichts zu tun 
                                End Try
                            End If
                        Next

                        Dim delta2 As Integer = sourceWorkItem.Links.Count - ret.Links.Count

                        Dim a = 555
                        'Dim relSourceIDs As New List(Of Integer)            ' das sind die LinkID's aus der Quelle aber schon in die DestID's überführt.
                        'For Each linked As Link In sourceWorkItem.Links
                        '    If linked.BaseType = BaseLinkType.RelatedLink Then
                        '        Dim relSourceLink = DirectCast(linked, RelatedLink)
                        '        Dim idInDest = IDMapper.GetMappedID(relSourceLink.RelatedWorkItemId)
                        '        relSourceIDs.Add(idInDest)
                        '    End If
                        'Next

                        'Dim relDestIDs As New List(Of Integer)
                        'For Each linked As Link In ret.Links
                        '    If linked.BaseType = BaseLinkType.RelatedLink Then
                        '        Dim relDestLink = DirectCast(linked, RelatedLink)
                        '        relDestIDs.Add(relDestLink.RelatedWorkItemId)
                        '    End If
                        'Next

                        '' gelöschte links ermitteln
                        'Dim removed = relDestIDs.Except(relSourceIDs)

                        'Dim itms2remove As New List(Of Link)
                        'For Each linked As Link In ret.Links
                        '    If linked.BaseType = BaseLinkType.RelatedLink Then
                        '        Dim relDestLink = DirectCast(linked, RelatedLink)
                        '        If removed.Contains(relDestLink.RelatedWorkItemId) Then
                        '            itms2remove.Add(linked)
                        '        End If
                        '    End If
                        'Next

                        '' neue Links ermitteln
                        'Dim added = relSourceIDs.Except(relDestIDs)

                        'Dim itms2add As New List(Of Link)
                        'For Each id In added
                        '    Dim newLink As New RelatedLink(id)
                        '    Try
                        '        ret.Links.Add(newLink)
                        '    Catch ve As Exception When ve.Message.StartsWith("TF237099")        ' TF237099: Duplicate work item link.
                        '        ' nichts zu tun 
                        '    End Try
                        'Next
                    ElseIf fieldname = "Attached File Count" Then
                    ElseIf fieldname = "Hyperlink Count" Then
                    Else
                        currdeltadict.Add(fieldname, val)
                        Dim field = ret.Fields(fieldname)
                        If field.IsEditable AndAlso Not Field2Ignore.Contains(fieldname) Then
                            ret.Fields(fieldname).Value = val
                        End If
                    End If
                End If
            Next

            Dim res = destWis.BatchSave(New WorkItem() {ret})
            If res.Length > 0 Then
                ' TODO: Log-Error
                If Debugger.IsAttached Then
                    Debugger.Break()
                End If
            End If

        Next


        IDMapper.FinishMapping(sourceId, ret.Id)

        Return ret
    End Function

    Private Shared Sub LinkItems(ByVal sourceWis As WorkItemStore, sourceProject As String, ByVal prefix As String, ByVal sourceId As Integer, IDMapper As IDMapper, destWis As WorkItemStore, destProject As String)

        Dim sourceWorkItem As WorkItem = sourceWis.GetWorkItem(sourceId)

        If Not IDMapper.IsMapped(sourceId) Then
            ' TODO
            ' HACK: why isn't it mapped yet????
            Return      ' Quickexit
        End If

        Dim destID = IDMapper.GetMappedID(sourceId)

        Dim destWorkItem As WorkItem = destWis.GetWorkItem(destID)

        For Each linked As Link In sourceWorkItem.Links
            If linked.BaseType = BaseLinkType.RelatedLink Then
                Dim relLink = DirectCast(linked, RelatedLink)

                Dim relSourceID = relLink.RelatedWorkItemId
                Dim relDestID = IDMapper.GetMappedID(relSourceID)
                Dim newLink As New RelatedLink(relDestID)
                Try
                    destWorkItem.Links.Add(newLink)
                Catch ve As Exception When ve.Message.StartsWith("TF237099")        ' TF237099: Duplicate work item link.
                    ' nichts zu tun 
                End Try
            End If

        Next

        Dim res = destWis.BatchSave(New WorkItem() {destWorkItem})
        If res.Length > 0 Then
            ' TODO: Log-Error
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
        End If
    End Sub



    Public Shared Function PrintTrees(ByVal wiStore As WorkItemStore, ByVal wiTrees As WorkItemLinkInfo(), ByVal prefix As String, ByVal sourceId As Integer, ByVal iThis As Integer) As Integer

        Dim iNext As Integer = 0

        ' Get the parent of this user story, if it has one
        Dim source As WorkItem = Nothing

        If sourceId <> 0 Then
            source = wiStore.GetWorkItem(wiTrees(iThis).SourceId)
        End If

        ' Process the items in the list that have the same parent as this user story
        While (iThis < wiTrees.Length AndAlso wiTrees(iThis).SourceId = sourceId)
            ' Get this user story

            Dim target As WorkItem
            target = wiStore.GetWorkItem(wiTrees(iThis).TargetId)
            Console.Write(prefix)
            Console.Write(target.Type.Name)
            Console.Write(":  ")
            Console.Write(target.Fields("Title").Value)
            If iThis < (wiTrees.Length - 1) Then
                If wiTrees(iThis).TargetId = wiTrees(iThis + 1).SourceId Then

                    ' The next item is the user story's child.
                    Console.WriteLine()
                    iNext = PrintTrees(wiStore, wiTrees, prefix + "   ", wiTrees(iThis + 1).SourceId, iThis + 1)
                Else

                    ' The next item is not the user story's child
                    iNext = iThis + 1
                End If
            Else
                ' This user story is the last one.
                iNext = iThis + 1
            End If
            Console.WriteLine()
            iThis = iNext
        End While
        Return iNext
    End Function
End Class
