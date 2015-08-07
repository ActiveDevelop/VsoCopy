Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text
Imports Microsoft.TeamFoundation.Client
Imports Microsoft.TeamFoundation.Common
Imports Microsoft.TeamFoundation.Framework.Client
Imports Microsoft.TeamFoundation.Framework.Common
Imports Microsoft.TeamFoundation.VersionControl.Client
Imports Microsoft.TeamFoundation.WorkItemTracking.Client

Public Class MainForm


    Private mySourceConfigurationServer As TfsConfigurationServer
    Private mySourceTfsUri As Uri
    Private mySourceCollectionNodes As List(Of TfsCollectionDisplayItem)
    Private mySelectedSourceTfsCollection As TfsCollectionDisplayItem
    Private myCurrentWorkspace As Workspace

    Private myDestTfsUri As Uri
    Private myDestConfigurationServer As TfsConfigurationServer


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        mySourceTfsUri = New Uri(SourceTFSTextBox.Text)

    End Sub

    Private Sub QueryCollectionsButton_Click(sender As Object, e As EventArgs) Handles QueryCollectionsButton.Click

        mySourceTfsUri = New Uri(SourceTFSTextBox.Text)
        mySourceConfigurationServer = TfsConfigurationServerFactory.GetConfigurationServer(mySourceTfsUri)

        Dim gVar As Guid() = New Guid() {CatalogResourceTypes.ProjectCollection}
        mySourceCollectionNodes = (From item In mySourceConfigurationServer.CatalogNode.QueryChildren(gVar, False, CatalogQueryOptions.None)
                                   Select New TfsCollectionDisplayItem With {.DisplayItem = item.Resource.DisplayName,
                                                                       .TfsCollection = mySourceConfigurationServer.GetTeamProjectCollection(
                                                                        New Guid(item.Resource.Properties("InstanceId")))}).ToList

        SourceTfsCollectionNodesComboBox.DataSource = mySourceCollectionNodes
        If SourceTfsCollectionNodesComboBox.Items.Count > 0 Then
            SourceTfsCollectionNodesComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub QueryWorkspacesButton_Click(sender As Object, e As EventArgs) Handles QueryWorkspacesButton.Click

        Dim tfsUri As Uri
        tfsUri = New Uri(SourceTFSTextBox.Text)

        Dim verCtrlServ = mySelectedSourceTfsCollection.TfsCollection.GetService(Of VersionControlServer)
        If verCtrlServ Is Nothing Then
            MessageBox.Show("The selected TFS Collection does not provide a Version Control Server - sorry!")
            Return
        End If

        'Workspace ermitteln.
        Dim workspaces = verCtrlServ.QueryWorkspaces(Nothing, verCtrlServ.AuthorizedUser, My.Computer.Name)

        If workspaces Is Nothing OrElse workspaces.Count < 1 Then
            MessageBox.Show("For user '" & verCtrlServ.AuthorizedUser & "' on computer '" &
                            My.Computer.Name & "' a workspace could not be found - sorry!")
        End If

        WorkspacesComboBox.DataSource = workspaces
        WorkspacesComboBox.SelectedIndex = 0
    End Sub

    Private Sub SourceTfsCollectionNodesComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SourceTfsCollectionNodesComboBox.SelectedIndexChanged
        If SourceTfsCollectionNodesComboBox.SelectedIndex = -1 Then
            mySelectedSourceTfsCollection = Nothing
        Else
            mySelectedSourceTfsCollection = DirectCast(SourceTfsCollectionNodesComboBox.SelectedItem, TfsCollectionDisplayItem)
        End If
    End Sub


    Private Sub WorkspacesComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles WorkspacesComboBox.SelectedIndexChanged

        Dim workingFolder As WorkingFolder

        myCurrentWorkspace = DirectCast(WorkspacesComboBox.SelectedItem, Workspace)
        If myCurrentWorkspace IsNot Nothing Then
            Dim folderlist = myCurrentWorkspace.Folders
            If folderlist Is Nothing OrElse folderlist.Length < 1 Then
                MessageBox.Show("A WorkingFolder could not be retrieved. Is this workspace mapped?")
            ElseIf folderlist.Length > 1 Then
                MessageBox.Show("More than one WorkingFolder mapped. Please check your mapping.")
                Return
            End If

            workingFolder = folderlist(0)
            ServerPathLabel.Text = "Server Path: " & workingFolder.DisplayServerItem
            LocalPathLinkLabel.Text = workingFolder.LocalItem

            Dim verCtrlServ = mySelectedSourceTfsCollection.TfsCollection.GetService(Of VersionControlServer)

            Dim rootItems = verCtrlServ.GetItems(workingFolder.ServerItem, RecursionType.OneLevel)

            ServerPathTreeView.Nodes.Clear()

            For Each items In rootItems.Items
                ServerPathTreeView.Nodes.Add(New TreeNode With {.Text = items.ServerItem, .Tag = items})
            Next
        End If

    End Sub


    Private Sub BackupSelectedTreeNodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BackupSelectedTreeNodeToolStripMenuItem.Click

        If String.IsNullOrWhiteSpace(MaintenanceFolderTextBox.Text) Then
            MessageBox.Show("Please provide a valid path for Maintenance purposes!")
            Return
        End If

        Dim di As New DirectoryInfo(MaintenanceFolderTextBox.Text)
        If Not di.Exists Then
            di.Create()
        End If

        Dim verCtrlServ = mySelectedSourceTfsCollection.TfsCollection.GetService(Of VersionControlServer)
        If ServerPathTreeView.SelectedNode Is Nothing Then
            MessageBox.Show("Please, firstly select the node in the ServerPath TreeView, which you want to backup!")
            Return
        End If

        Dim serverPath = DirectCast(ServerPathTreeView.SelectedNode.Tag, Item).ServerItem

        'Demo: QueryHistory.
        Dim changeSets = verCtrlServ.QueryHistory(serverPath, VersionSpec.Latest, 0, RecursionType.Full,
                                     "", Nothing, Nothing, Integer.MaxValue, True, True, True, True)

        Dim currentBackupDir = di.CreateSubdirectory("Backup " & Now.ToString("yyyy-MM-dd HH-mm"))

        Dim infoFile = New StreamWriter(currentBackupDir.FullName & "\An_Overview_of_this_backup.txt")
        Dim stringContent As String = "Created by: " & My.User.Name & vbNewLine &
            "Date/Time:" & DateTime.Now.ToLongDateString & vbNewLine &
            "On Computer:" & My.Computer.Name
        infoFile.WriteLine(stringContent)
        infoFile.Flush()
        infoFile.Close()

        For Each changeSet As Changeset In changeSets

            Dim protstring = changeSet.ChangesetId & " by " & changeSet.OwnerDisplayName & " on " &
                    changeSet.CreationDate.ToLongDateString & vbNewLine &
                    changeSet.Comment & vbNewLine & vbNewLine &
                    "--- Files/Folders in Changeset: ---" & vbNewLine
            Debug.Print(protstring)

            Dim changeSetFolderString = "CS" & changeSet.ChangesetId.ToString("000000") &
                                        "-" & changeSet.CreationDate.ToString("yyyy-MM-dd HH-mm") &
                                        "-" & changeSet.OwnerDisplayName

            Dim currentChangeSetDir = currentBackupDir.CreateSubdirectory(changeSetFolderString)
            Dim protocolContent As New StringBuilder

            For Each changeItem As Change In changeSet.Changes
                Dim preFix = "   "
                If changeItem.ChangeType.HasFlag(ChangeType.Delete) Then
                    preFix += "-"
                End If

                If changeItem.ChangeType.HasFlag(ChangeType.Add) Then
                    preFix += "+"
                End If

                If changeItem.ChangeType.HasFlag(ChangeType.Edit) Then
                    preFix += "*"
                End If

                If changeItem.ChangeType.HasFlag(ChangeType.Rename) Then
                    preFix += "R"
                End If

                If changeItem.ChangeType.HasFlag(ChangeType.SourceRename) Then
                    preFix += "r"
                End If

                If changeItem.ChangeType.HasFlag(ChangeType.Merge) Then
                    preFix += " M "
                Else
                    preFix += "  "
                End If

                If changeItem.Item.ItemType = ItemType.Folder Then
                    preFix += "°  " 'Folder
                Else
                    preFix += "^  " 'File
                End If

                protocolContent.Append(preFix & changeItem.Item.ServerItem & vbNewLine)
                If Not changeItem.ChangeType.HasFlag(ChangeType.Delete) And changeItem.Item.ItemType = ItemType.File Then
                    'Herausfinden, ob Ordner existiert:
                    Dim remainingPathAndFilename = changeItem.Item.ServerItem.Substring(serverPath.Length + 1)
                    Dim fileInfo = New FileInfo(currentChangeSetDir.FullName & "\" & remainingPathAndFilename)
                    If Not fileInfo.Directory.Exists Then
                        fileInfo.Directory.Create()
                    End If
                    changeItem.Item.DownloadFile(fileInfo.FullName)
                    CurrentTaskItemInfoToolStripStatusLabel.Text = "Downloading Code for ChangeSet:" & changeSet.ChangesetId &
                                                                   " by " & changeSet.OwnerDisplayName &
                                                                   " from " & changeSet.CreationDate.ToShortDateString &
                                                                   "; file:" & changeItem.Item.ServerItem
                    Application.DoEvents()
                End If
            Next

            infoFile = New StreamWriter(currentChangeSetDir.FullName & "\Complete_FileInfo_Of_This_Changeset.txt")
            stringContent = protocolContent.ToString & vbNewLine & vbNewLine & "Symbolinfo: (M)=Merged; (°)=IsFolder; (^)=IsFile; (r)=SourceRename; (*)=Edited; (-)=Deleted; (+)=Added;"
            stringContent &= vbNewLine & vbNewLine
            stringContent &= changeSet.Comment
            infoFile.WriteLine(stringContent)
            infoFile.Flush()
            infoFile.Close()

            Debug.Print("***")
            Debug.Print("")
        Next
    End Sub

    Private Sub ChoosePathButton_Click(sender As Object, e As EventArgs) Handles ChoosePathButton.Click

        'Pick the folder to Backup:
        Dim fbd As New FolderBrowserDialog
        fbd.ShowNewFolderButton = True
        fbd.Description = "Please, pick the folder, you would like to create the backup in."
        Dim dr = fbd.ShowDialog()
        If dr = DialogResult.OK Then
            MaintenanceFolderTextBox.Text = fbd.SelectedPath
        End If

    End Sub

    Private Sub SetupDestServer()
        Dim tmpuri = New Uri(DestinationTFSTextBox.Text)
        If myDestTfsUri <> tmpuri Then
            myDestTfsUri = tmpuri
            myDestConfigurationServer = TfsConfigurationServerFactory.GetConfigurationServer(myDestTfsUri)
        End If
    End Sub

    Private Sub TeamCollectionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WorkItemsAbgleichenToolStripMenuItem.Click

        Dim projectName = DirectCast(ServerPathTreeView.SelectedNode?.Tag, Item)?.ServerItem.Split("/"c)(1)


        Dim a = Environment.UserName.ToLower = "andreasb"     ' HACK
        If a Then   ' TODO: Check if a destinationrojectNode is checked
            Dim dstColl As TfsTeamProjectCollection

            'HACK: 
            Dim childrenEnu As Guid() = New Guid() {CatalogResourceTypes.ProjectCollection}
            Dim colls = (From item In myDestConfigurationServer.CatalogNode.QueryChildren(childrenEnu, False, CatalogQueryOptions.None)
                         Select New TfsCollectionDisplayItem With {.DisplayItem = item.Resource.DisplayName,
                                                                           .TfsCollection = myDestConfigurationServer.GetTeamProjectCollection(
                                                                            New Guid(item.Resource.Properties("InstanceId")))}).ToList
            dstColl = colls(0).TfsCollection
            'END HACK


            Dim wis = New WorkItemStore(mySelectedSourceTfsCollection.TfsCollection)



            Dim sourceprojectName = wis.Projects(projectName).Name
            Console.Write("Project: ")
            Console.WriteLine(sourceprojectName)


            Dim queryHierarchy As QueryHierarchy = wis.Projects(projectName).QueryHierarchy
            Dim queryFolder = TryCast(queryHierarchy, QueryFolder)
            'Dim queryItem = queryFolder("Shared Queries")
            Dim queryItem = queryFolder("My Queries")
            queryFolder = TryCast(queryItem, QueryFolder)

            If queryFolder Is Nothing Then Return
            Dim qry = TryCast(queryFolder("4Copy"), QueryDefinition)
            If qry Is Nothing Then Return
            ' Get the type of work item to use
            'Dim categories As CategoryCollection
            'categories = wis.Projects(sourceprojectName).Categories
            'Dim wiType As String
            'wiType = categories("Requirement Category").DefaultWorkItemType.Name
            '' Query for the trees of active user stories in the team project collection
            'Dim queryString As New StringBuilder("SELECT [System.Id] FROM WorkItemLinks WHERE ")
            'queryString.Append("([Source].[System.WorkItemType] = '")
            'queryString.Append(wiType)
            'queryString.Append("' AND [Source].[System.TeamProject] = '")
            'queryString.Append(sourceprojectName)
            'queryString.Append("') AND ")
            'queryString.Append("([System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward') And ")
            'queryString.Append("([Target].[System.WorkItemType] = 'User Story' AND [Target].[System.State] = 'Active') ORDER BY [System.Id] mode(Recursive)")
            'Dim wiQuery As New Query(wis, queryString.ToString())


            Dim variables As New Dictionary(Of String, String)
            variables.Add("project", sourceprojectName)

            Dim wiQuery As New Query(wis, qry.QueryText, variables)
            Dim wiTrees() As WorkItemLinkInfo
            wiTrees = wiQuery.RunLinkQuery()

            ' Print the trees of user stories with the estimated size of each leaf.
            PrintTrees(wis, wiTrees, "   ", 0, 0)
        End If
    End Sub

    Private Sub ListDestinationTFSProjectsButton_Click(sender As Object, e As EventArgs) Handles ListDestinationTFSProjectsButton.Click
        SetupDestServer()

        'Dim gVar As Guid() = New Guid() {CatalogResourceTypes.ProjectCollection}
        'Dim colls = (From item In myDestConfigurationServer.CatalogNode.QueryChildren(gVar, False, CatalogQueryOptions.None)
        '             Select New TfsCollectionDisplayItem With {.DisplayItem = item.Resource.DisplayName,
        '                                                               .TfsCollection = myDestConfigurationServer.GetTeamProjectCollection(
        '                                                                New Guid(item.Resource.Properties("InstanceId")))}).ToList

        'DestTfsCollectionNodesComboBox.DataSource = colls
        'If DestTfsCollectionNodesComboBox.Items.Count > 0 Then
        '    DestTfsCollectionNodesComboBox.SelectedIndex = 0
        'End If

    End Sub

    Function PrintTrees(ByVal wiStore As WorkItemStore, ByVal wiTrees As WorkItemLinkInfo(), ByVal prefix As String, ByVal sourceId As Integer, ByVal iThis As Integer) As Integer

        Dim iNext As Integer = 0

        ' Get the parent of this user story, if it has one
        Dim source As WorkItem = Nothing

        If sourceId <> 0 Then
            source = wiStore.GetWorkItem(wiTrees(iThis).SourceId)
        End If

        ' Process the items in the list that have the same parent as this user story
        While (iThis <wiTrees.Length AndAlso wiTrees(iThis).SourceId = sourceId)
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

    Private Sub TestabgleichToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TestabgleichToolStripMenuItem.Click
        Dim sourceTfsUri As Uri = New Uri("https://activedevelop.visualstudio.com/")
        Dim sourceConfigurationServer As TfsConfigurationServer = TfsConfigurationServerFactory.GetConfigurationServer(sourceTfsUri)

        'HACK
        Dim childrenEnu As Guid() = New Guid() {CatalogResourceTypes.ProjectCollection}

        Dim colls = (From item In sourceConfigurationServer.CatalogNode.QueryChildren(childrenEnu, False, CatalogQueryOptions.None)
                     Select New TfsCollectionDisplayItem With {.DisplayItem = item.Resource.DisplayName,
                                                                       .TfsCollection = sourceConfigurationServer.GetTeamProjectCollection(New Guid(item.Resource.Properties("InstanceId")))}).ToList
        '->HACK

        Dim sourceTfsCollection = colls(0).TfsCollection


        Dim destTfsUri As Uri = New Uri("http://192.168.1.114:8080/tfs")
        Dim destConfigurationServer As TfsConfigurationServer = TfsConfigurationServerFactory.GetConfigurationServer(destTfsUri)

        'HACK
        colls = (From item In destConfigurationServer.CatalogNode.QueryChildren(childrenEnu, False, CatalogQueryOptions.None)
                 Select New TfsCollectionDisplayItem With {.DisplayItem = item.Resource.DisplayName,
                                                                       .TfsCollection = destConfigurationServer.GetTeamProjectCollection(New Guid(item.Resource.Properties("InstanceId")))}).ToList
        '->HACK

        Dim destTfsCollection = colls(0).TfsCollection

        TFSWorkItemCopy.Copy(sourceTfsCollection, "LSKNSamba", sourceConfigurationServer, destTfsCollection, "Blub", destConfigurationServer)

        Dim aa = 55

    End Sub

    Private Sub Get4CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Get4CopyToolStripMenuItem.Click
        Dim tpp As New TeamProjectPicker(TeamProjectPickerMode.SingleProject, False)
        Dim res = tpp.ShowDialog()
        If res = DialogResult.OK Then
            Dim coll = tpp.SelectedTeamProjectCollection

            If coll IsNot Nothing AndAlso tpp.SelectedProjects.Count > 0 Then
                Dim proj = tpp.SelectedProjects(0)
                Dim wis = New WorkItemStore(coll)

                Dim queryHierarchy As QueryHierarchy = wis.Projects(proj.Name).QueryHierarchy
                Dim queryFolder = TryCast(queryHierarchy, QueryFolder)
                'Dim queryItem = queryFolder("Shared Queries")
                Dim queryItem = queryFolder("My Queries")
                queryFolder = TryCast(queryItem, QueryFolder)

                If queryFolder Is Nothing Then Return
                Dim qry = TryCast(queryFolder("4Copy"), QueryDefinition)
                If qry Is Nothing Then Return

                Console.WriteLine(qry.QueryText)

            End If
        End If
    End Sub
End Class
