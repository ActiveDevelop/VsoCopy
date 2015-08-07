Imports Microsoft.TeamFoundation.Client
Imports Microsoft.TeamFoundation.Framework.Common
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

Public Class TFSWorkItemCopy

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
                Throw New InvalidOperationException("Mapping already started")
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

    End Class


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

        'For Each pitem As Project In destWis.Projects
        '    Dim types = pitem.WorkItemTypes

        '    Dim a = 5
        'Next

        Dim sourceprojectName = sourceWis.Projects(sourceProject).Name
        Console.Write("Project: ")
        Console.WriteLine(sourceprojectName)

        Dim variables As New Dictionary(Of String, String)
        variables.Add("project", sourceprojectName)

        Dim wiQuery As New Query(sourceWis, My.Resources.TaskQuery, variables)

        Dim tasklist = wiQuery.RunQuery


        Dim idmapper As New IDMapper
        'CopyBLIRecursive(sourceWis, sourceProject, tasklist, idmapper, destWis, destProject)


        ' BLI Web API - 'CopyBLIRecursive(sourceWis, sourceProject, "   ", 1308, idmapper, destWis, destProject)

        CopyBLIRecursive(sourceWis, sourceProject, "   ", 2485, idmapper, destWis, destProject)
        CopyBLIRecursive(sourceWis, sourceProject, "   ", 2301, idmapper, destWis, destProject)
    End Sub


    Private Shared Sub CopyBLIRecursive(ByVal sourceWis As WorkItemStore, sourceProject As String, ByVal tasklist As WorkItemCollection, idmapper As IDMapper, destWis As WorkItemStore, destProject As String)
        For Each item As WorkItem In tasklist
            CopyBLIRecursive(sourceWis, sourceProject, "   ", item.Id, idmapper, destWis, destProject)
        Next
    End Sub

    Private Shared Function CopyBLIRecursive(ByVal sourceWis As WorkItemStore, sourceProject As String, ByVal prefix As String, ByVal sourceId As Integer, IDMapper As IDMapper, destWis As WorkItemStore, destProject As String) As WorkItem

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

        ' Fields prüfen, die in beiden objecten vorhanden sind
        Dim srcFields = (From itm In sourceWorkItem.Fields.Cast(Of Microsoft.TeamFoundation.WorkItemTracking.Client.Field) Select itm.Name).ToList
        Dim dstFields = (From itm In ret.Fields.Cast(Of Microsoft.TeamFoundation.WorkItemTracking.Client.Field) Select itm.Name).ToList
        Dim commonFieldsName = srcFields.Intersect(dstFields).ToList

        'commonFieldsName.Remove("State")            ' e.g. State='Removed' is invalid
        'commonFieldsName.Remove("Changed By")
        commonFieldsName.Remove("Area ID")
        commonFieldsName.Remove("Area Path")
        commonFieldsName.Remove("Iteration ID")
        commonFieldsName.Remove("Iteration Path")

        Dim uneditableFields As New Text.StringBuilder()

        For Each fname As String In commonFieldsName
            Dim field = ret.Fields(fname)
            If field.IsEditable Then
                field.Value = sourceWorkItem.Fields(fname).Value
                Dim val = sourceWorkItem.Fields(fname).Value
                If val Is Nothing Then
                    val = "null"
                ElseIf CStr(val) = "" Then
                    val = """"
                End If

                Console.WriteLine("{0} := {1}", fname, val)
            Else
                'Console.WriteLine("{0} is not editable", fname)
                uneditableFields.AppendFormat("{0},", fname)
            End If

        Next

        ''HACK: 
        'ret.Fields("State").Value = ret.Fields("State").AllowedValues(0)
        'Dim reasons = ret.Fields("Reason").AllowedValues
        'Dim val2set As Object = Nothing
        'If reasons.Count > 0 Then
        '    val2set = ret.Fields("Reason").AllowedValues(0)
        'End If
        'ret.Fields("Reason").Value = val2set


        'ret.Fields("Assigned To").Value = Nothing

        'newWI.Fields("Changed By").Value = Nothing
        'newWI.Fields("Area ID").Value = Nothing
        'newWI.Fields("Area Path").Value = Nothing
        'newWI.Fields("Iteration ID").Value = Nothing
        'newWI.Fields("Iteration Path").Value = Nothing


        'END HACK

        Console.WriteLine("The fields {0} are not editable", uneditableFields.ToString.TrimEnd(","c))

        'For Each linked As Link In sourceWorkItem.Links
        '    If linked.BaseType = BaseLinkType.RelatedLink Then
        '        Dim relLink = DirectCast(linked, RelatedLink)
        '        Dim relID = relLink.RelatedWorkItemId
        '        If Not IDMapper.IsMapping(relID) AndAlso Not IDMapper.IsMapped(relID) Then
        '            ' mapping is not active
        '            Dim subItem = CopyBLIRecursive(sourceWis, sourceProject, prefix + "   ", relID, IDMapper, destWis, destProject)
        '            If subItem IsNot Nothing Then
        '                Dim newLink As New RelatedLink(subItem.Id)
        '                Try
        '                    ret.Links.Add(newLink)
        '                Catch ex As Exception When ex.Message.StartsWith("TF237099")
        '                    'TF237099=Duplicate link to workitem
        '                    ' we dont care
        '                End Try
        '            End If
        '        ElseIf IDMapper.IsMapped(relID) Then
        '            Dim linkID = IDMapper.GetMappedID(relID)
        '            Dim newLink As New RelatedLink(linkID)
        '            ret.Links.Add(newLink)

        '        Else
        '            If relLink.LinkTypeEnd.IsForwardLink Then
        '                'TODO
        '                'HACK
        '                'what to do now....
        '                Console.WriteLine("Help!!!!!!!!!!!")
        '            Else
        '                ' we link "upwards"??? so -> no problem
        '                Return Nothing
        '            End If
        '        End If
        '    End If

        'Next



        'Dim valRes = ret.Validate()

        'For Each invalFieldObj In valRes
        '    Dim invalField = TryCast(invalFieldObj, Microsoft.TeamFoundation.WorkItemTracking.Client.Field)
        '    ret.Fields(invalField.Name).Value = Nothing

        '    Console.WriteLine("The Field {0} has an invalid value.", invalField.Name)
        'Next



        Dim res = destWis.BatchSave(New WorkItem() {ret})
        If res.Length > 0 Then
            ' TODO: Log-Error
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
        End If

        IDMapper.FinishMapping(sourceId, ret.Id)

        Return ret
    End Function


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
