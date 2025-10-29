Imports System.IO
Imports System.Xml.Serialization

Public Class StartupWindow

    Private projectsDirectory As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ProjectDatas")

    Private Sub StartupForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Create directory if it doesn't exist
        If Not Directory.Exists(projectsDirectory) Then
            Directory.CreateDirectory(projectsDirectory)
        End If

        LoadProjectList()
    End Sub

    Private Sub LoadProjectList()
        lstProjects.Items.Clear()

        Try
            Dim rosimFiles = Directory.GetFiles(projectsDirectory, "*.rosim")

            For Each filePath In rosimFiles
                lstProjects.Items.Add(Path.GetFileNameWithoutExtension(filePath))
            Next

            If rosimFiles.Length = 0 Then
                lblInfo.Text = "No projects found. Create a new one to start."
            Else
                lblInfo.Text = "Found " & rosimFiles.Length & " project(s)"
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading projects: " & ex.Message)
        End Try
    End Sub

    Private Sub btnNewProject_Click(sender As Object, e As EventArgs) Handles btnNewProject.Click
        Dim projectName = InputBox("Enter project name:", "Create New Project")

        If String.IsNullOrWhiteSpace(projectName) Then
            Return
        End If

        ' Open folder browser dialog to choose save location
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

        ' Check if project already exists
        If File.Exists(filePath) Then
            MessageBox.Show("A project with this name already exists in this folder!")
            Return
        End If

        Try
            ' Create new empty project
            Dim newProject As New ProjectData With {
                .ProjectName = projectName,
                .CreatedDate = DateTime.Now,
                .LastModified = DateTime.Now
            }

            ' Save project
            Dim serializer As New XmlSerializer(GetType(ProjectData))
            Using writer As New StreamWriter(filePath)
                serializer.Serialize(writer, newProject)
            End Using

            MessageBox.Show("Project created successfully at: " & filePath)
            LoadProjectList()
            lstProjects.Items.Add(projectName)
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

        ' Open main form and pass the file path
        Dim mainForm = New WaterAnalysis()
        mainForm.LoadProject(filePath)
        mainForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Dim openDialog As New OpenFileDialog With {
            .Filter = "ROSIM Files (*.rosim)|*.rosim|All Files (*.*)|*.*",
            .Title = "Import Project File"
        }

        If openDialog.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Try
            Dim sourceFile = openDialog.FileName
            Dim fileName = Path.GetFileName(sourceFile)
            Dim destinationFile = Path.Combine(projectsDirectory, fileName)

            ' Check if file already exists in current directory
            If File.Exists(destinationFile) Then
                Dim result = MessageBox.Show("A project with this name already exists. Overwrite?", "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result <> DialogResult.Yes Then
                    Return
                End If
            End If

            ' Copy file to projects directory
            File.Copy(sourceFile, destinationFile, True)
            MessageBox.Show("Project imported successfully!", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadProjectList()

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
        Application.Exit()
    End Sub
End Class