Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Text
Imports Microsoft.TeamFoundation.Client
Imports Microsoft.TeamFoundation.Common
Imports Microsoft.TeamFoundation.Framework.Client
Imports Microsoft.TeamFoundation.Framework.Common
Imports Microsoft.TeamFoundation.VersionControl.Client

Public Class MainForm

    Private myConfigurationServer As TfsConfigurationServer
    Private myTfsUri As Uri
    Private myCollectionNodes As List(Of TfsCollectionDisplayItem)
    Private mySelectedTfsCollection As TfsCollectionDisplayItem
    Private myCurrentWorkspace As Workspace

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        myTfsUri = New Uri(SourceTFSTextBox.Text)

    End Sub

    Private Sub QueryCollectionsButton_Click(sender As Object, e As EventArgs) Handles QueryCollectionsButton.Click

        myTfsUri = New Uri(SourceTFSTextBox.Text)
        myConfigurationServer = TfsConfigurationServerFactory.GetConfigurationServer(myTfsUri)

        Dim gVar As Guid() = New Guid() {CatalogResourceTypes.ProjectCollection}
        myCollectionNodes = (From item In myConfigurationServer.CatalogNode.QueryChildren(gVar, False, CatalogQueryOptions.None)
                             Select New TfsCollectionDisplayItem With {.DisplayItem = item.Resource.DisplayName,
                                                                       .TfsCollection = myConfigurationServer.GetTeamProjectCollection(
                                                                        New Guid(item.Resource.Properties("InstanceId")))}).ToList

        TfsCollectionNodesComboBox.DataSource = myCollectionNodes
        If TfsCollectionNodesComboBox.Items.Count > 0 Then
            TfsCollectionNodesComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub QueryWorkspacesButton_Click(sender As Object, e As EventArgs) Handles QueryWorkspacesButton.Click

        Dim tfsUri As Uri
        tfsUri = New Uri(SourceTFSTextBox.Text)

        Dim verCtrlServ = mySelectedTfsCollection.TfsCollection.GetService(Of VersionControlServer)
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

    Private Sub TfsCollectionNodesComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TfsCollectionNodesComboBox.SelectedIndexChanged
        If TfsCollectionNodesComboBox.SelectedIndex = -1 Then
            mySelectedTfsCollection = Nothing
        Else
            mySelectedTfsCollection = DirectCast(TfsCollectionNodesComboBox.SelectedItem, TfsCollectionDisplayItem)
        End If

        Return

        For Each collectionNode In myCollectionNodes

            Dim teamProjectCollection = mySelectedTfsCollection.TfsCollection

            Dim tfsCollectionTreeNode As New TreeNode(teamProjectCollection.Name)
            tfsCollectionTreeNode.Tag = teamProjectCollection
            SourceTFSTreeView.Nodes.Add(tfsCollectionTreeNode)

            ' Get a catalog of team projects for the collection
            Dim hVar As Guid() = New Guid() {CatalogResourceTypes.TeamProject}

            Dim projectNodes As ReadOnlyCollection(Of CatalogNode)
            'projectNodes = collectionNode.QueryChildren(hVar, False, CatalogQueryOptions.None)

            ' List the team projects in the collection
            For Each projectNode In projectNodes
                Dim projectTreeNode As New TreeNode(projectNode.Resource.DisplayName)

                projectTreeNode.Tag = projectNode
                tfsCollectionTreeNode.Nodes.Add(projectTreeNode)
            Next
            tfsCollectionTreeNode.Expand()
        Next

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

            Dim verCtrlServ = mySelectedTfsCollection.TfsCollection.GetService(Of VersionControlServer)

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

        Dim verCtrlServ = mySelectedTfsCollection.TfsCollection.GetService(Of VersionControlServer)

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
End Class
