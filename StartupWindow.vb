Imports System.IO
Imports System.Xml.Serialization

Public Class StartupWindow

    Private projectsDirectory As String = ""
    Private appState As AppState

    Private Sub StartupForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Get app state
        appState = AppStateManager.GetInstance().State

        ' Restore window position and size if available
        If Not String.IsNullOrEmpty(appState.FormLocation) Then
            Dim parts = appState.FormLocation.Split(","c)
            If parts.Length = 2 Then
                Me.Location = New Point(Integer.Parse(parts(0)), Integer.Parse(parts(1)))
            End If
        End If

        ' Use last projects directory if it exists, otherwise use default
        If Not String.IsNullOrEmpty(appState.LastProjectsDirectory) AndAlso Directory.Exists(appState.LastProjectsDirectory) Then
            projectsDirectory = appState.LastProjectsDirectory
        Else
            ' Use default My Documents folder on first launch
            projectsDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WaterAnalysisProjects")

            ' Create the directory if it doesn't exist
            If Not Directory.Exists(projectsDirectory) Then
                Directory.CreateDirectory(projectsDirectory)
            End If

            ' Save it to app state
            appState.LastProjectsDirectory = projectsDirectory
            AppStateManager.GetInstance().SaveAppState()
        End If

        LoadProjectList()
    End Sub

    Private Sub SelectProjectsDirectory()
        Dim folderDialog As New FolderBrowserDialog With {
            .Description = "Select folder containing your projects",
            .ShowNewFolderButton = True
        }

        If folderDialog.ShowDialog() <> DialogResult.OK Then
            MessageBox.Show("Please select a folder to continue.")
            SelectProjectsDirectory()
            Return
        End If

        projectsDirectory = folderDialog.SelectedPath
        appState.LastProjectsDirectory = projectsDirectory
        AppStateManager.GetInstance().SaveAppState()
        LoadProjectList()
    End Sub

    Private Sub LoadProjectList()
        lstProjects.Items.Clear()

        Try
            If Not Directory.Exists(projectsDirectory) Then
                lblInfo.Text = "No projects yet. Create or import one to get started."
                Return
            End If

            Dim rosimFiles = Directory.GetFiles(projectsDirectory, "*.rosim")

            For Each filePath In rosimFiles
                lstProjects.Items.Add(Path.GetFileNameWithoutExtension(filePath))
            Next

            If rosimFiles.Length = 0 Then
                lblInfo.Text = "No projects yet. Create or import one to get started."
            Else
                lblInfo.Text = "Found " & rosimFiles.Length & " project(s)"
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading projects: " & ex.Message)
        End Try
    End Sub



    ' NEW PROJECT BUTTON
    Private Sub btnNewProject_Click(sender As Object, e As EventArgs) Handles btnNewProject.Click
        Dim projectName = InputBox("Enter project name:", "Create New Project")

        If String.IsNullOrWhiteSpace(projectName) Then
            Return
        End If

        Dim saveDialog As New SaveFileDialog With {
            .Filter = "ROSIM Files (*.rosim)|*.rosim",
            .DefaultExt = "rosim",
            .FileName = projectName,
            .Title = "Save New Project",
            .AutoUpgradeEnabled = True,
            .DereferenceLinks = True
        }

        If saveDialog.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim filePath = saveDialog.FileName

        If File.Exists(filePath) Then
            MessageBox.Show("A project with this name already exists in this location!")
            Return
        End If

        Try
            Dim newProject As New WaterAnalysisProject With {
                .ProjectName = projectName,
                .CreatedDate = DateTime.Now,
                .LastModified = DateTime.Now
            }

            Dim serializer As New XmlSerializer(GetType(WaterAnalysisProject))
            Using writer As New StreamWriter(filePath)
                serializer.Serialize(writer, newProject)
            End Using

            MessageBox.Show("Project created successfully!")
            lstProjects.Items.Add(projectName)

            ' Update app state and open project
            appState.CurrentProjectPath = filePath
            appState.CurrentProjectName = projectName
            AppStateManager.GetInstance().SaveAppState()

            Dim mainForm = New WaterAnalysis()
            mainForm.LoadProject(filePath)
            mainForm.Show()
            Me.Hide()

        Catch ex As Exception
            MessageBox.Show("Error creating project: " & ex.Message)
        End Try
    End Sub
    Private Sub bntOpenProject_Click(sender As Object, e As EventArgs) Handles bntOpenProject.Click
        If lstProjects.SelectedIndex = -1 Then
            MessageBox.Show("Please select a project to open!")
            Return
        End If

        Dim selectedProject = lstProjects.SelectedItem.ToString()
        Dim filePath = Path.Combine(projectsDirectory, selectedProject & ".rosim")

        appState.CurrentProjectPath = filePath
        appState.CurrentProjectName = selectedProject
        AppStateManager.GetInstance().SaveAppState()

        Dim mainForm = New WaterAnalysis()
        mainForm.LoadProject(filePath)
        mainForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Dim openDialog As New OpenFileDialog With {
            .Filter = "ROSIM Files (*.rosim)|*.rosim|All Files (*.*)|*.*",
            .Title = "Import Project File",
            .AutoUpgradeEnabled = True,
            .DereferenceLinks = True
        }

        If openDialog.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Try
            Dim sourceFile = openDialog.FileName
            Dim fileName = Path.GetFileName(sourceFile)
            Dim destinationFile = Path.Combine(projectsDirectory, fileName)

            If File.Exists(destinationFile) Then
                Dim result = MessageBox.Show("A project with this name already exists. Overwrite?", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result <> DialogResult.Yes Then
                    Return
                End If
            End If

            File.Copy(sourceFile, destinationFile, True)
            MessageBox.Show("Project imported successfully!", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information)
            lstProjects.Items.Add(Path.GetFileNameWithoutExtension(fileName))

        Catch ex As Exception
            MessageBox.Show("Error importing project: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If lstProjects.SelectedIndex = -1 Then
            MessageBox.Show("Please select a project to delete!")
            Return
        End If

        Dim selectedProject = lstProjects.SelectedItem.ToString()
        Dim result = MessageBox.Show("Are you sure you want to delete '" & selectedProject & "'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If result = DialogResult.Yes Then
            Try
                Dim filePath = Path.Combine(projectsDirectory, selectedProject & ".rosim")
                File.Delete(filePath)
                MessageBox.Show("Project deleted successfully!")
                LoadProjectList()
            Catch ex As Exception
                MessageBox.Show("Error deleting project: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        ' Save form state
        appState.FormLocation = Me.Location.X & "," & Me.Location.Y
        AppStateManager.GetInstance().SaveAppState()
        Application.Exit()
    End Sub
End Class