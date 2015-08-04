Imports Microsoft.TeamFoundation.Client
Imports Microsoft.TeamFoundation.Framework.Client

Public Class TfsCollectionDisplayItem

    Public Property TfsCollection As TfsTeamProjectCollection
    Public Property DisplayItem As String

    Public Overrides Function ToString() As String
        Return DisplayItem
    End Function


End Class

