Imports System.IO
Imports System.Xml.Serialization

Public Class AppStateManager
    Private Shared instance As AppStateManager
    Private Shared ReadOnly lockObj As Object = New Object()

    Public Shared Function GetInstance() As AppStateManager
        If instance Is Nothing Then
            SyncLock lockObj
                If instance Is Nothing Then
                    instance = New AppStateManager()
                End If
            End SyncLock
        End If
        Return instance
    End Function

    Public State As AppState
    Private appDataPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WaterAnalysis")
    Private stateFilePath As String

    Private Sub New()
        ' Create app data directory if it doesn't exist
        If Not Directory.Exists(appDataPath) Then
            Directory.CreateDirectory(appDataPath)
        End If

        stateFilePath = Path.Combine(appDataPath, "AppState.xml")
        State = LoadAppState()
    End Sub

    Public Function LoadAppState() As AppState
        Try
            If File.Exists(stateFilePath) Then
                Dim serializer As New XmlSerializer(GetType(AppState))
                Using reader As New StreamReader(stateFilePath)
                    Return CType(serializer.Deserialize(reader), AppState)
                End Using
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading app state: " & ex.Message)
        End Try

        Return New AppState()
    End Function

    Public Sub SaveAppState()
        Try
            Dim serializer As New XmlSerializer(GetType(AppState))
            Using writer As New StreamWriter(stateFilePath)
                serializer.Serialize(writer, State)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error saving app state: " & ex.Message)
        End Try
    End Sub
End Class

