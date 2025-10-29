Imports System.IO
Imports System.Xml.Serialization

Public Class System_Configuration_Design

    Private currentProjectPath As String
    Private currentProject As ProjectData




    Public Sub LoadProject(filePath As String)
        currentProjectPath = filePath

        Try
            Dim serializer As New XmlSerializer(GetType(ProjectData))
            Using reader As New StreamReader(filePath)
                currentProject = CType(serializer.Deserialize(reader), ProjectData)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading project: " & ex.Message)
            currentProject = New ProjectData()
        End Try

        ' Set form title with project name
        Me.Text = "System Design - " & currentProject.ProjectName

        ' Load data into textboxes
        LoadDataToUI()
    End Sub
    Private Sub LoadDataToUI()
        txtPermeateRecovery.Text = If(currentProject.PermeateRecovery, "")
        txtPermeateFlux.Text = If(currentProject.Permeateflux, "")
        txtFeedFlowRate.Text = If(currentProject.FeedFlowrate, "")
        txtConcentrationFactor.Text = If(currentProject.ConcentrationFactor, "")
        txtSpecificFluxPerUnitArea.Text = If(currentProject.SpecificFluxPerUnitArea, "")
        txtMembraneArea.Text = If(currentProject.MembraneArea, "")
        txtElementRecovery.Text = If(currentProject.ElementRecovery, "")
        txtAverageSystemFlux.Text = If(currentProject.AverageSystemFlux, "")
        txtNumberOfSerial.Text = If(currentProject.NumberofSerial, "")
        txtNumberOfELPerVessel.Text = If(currentProject.NumberofElperVessel, "")
        txtNumberOfStages.Text = If(currentProject.NumberofStages, "")
        txtStagingRatio.Text = If(currentProject.StagingRatio, "")
        txtTotNumElements.Text = If(currentProject.TotalNumberOfElements, "")
    End Sub



    Private Sub btnEvaluate_Click(sender As Object, e As EventArgs) Handles btnEvaluate.Click



        Dim textBoxes As New List(Of TextBox) From {
        txtPermeateRecovery, txtPermeateFlux, txtMembraneArea, txtSpecificFluxPerUnitArea
        }

        ' Loop through all textboxes to find empty ones
        For Each txt As TextBox In textBoxes
            If String.IsNullOrWhiteSpace(txt.Text) Then
                MessageBox.Show("Please fill out all required date before calculating!",
                          "Missing Data",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Warning)
                txt.Focus() ' Focus on the empty field
                Return ' Stop here - don't execute rest of code
            End If
        Next



        'Variables

        'Global Information
        Dim dPermeateRecovery As Double
        Dim dPermeateflux As Double
        Dim dFeedFlowrate As Double
        Dim dConcentrationFactor As Double
        Dim dSpecificFluxPerUnitArea As Double
        Dim dMembraneArea As Double
        Dim dElementRecovery As Double
        Dim dAverageSystemFlux As Double

        'Number of stages selection
        Dim iNumberofSerial As Integer
        Dim iNumberofElperVessel As Integer
        Dim iNumberofStages As Integer
        Dim dStagingRatio As Double
        Dim iTotalNumberOfElements As Double



        'Calculations
        dPermeateRecovery = Convert.ToDouble(txtPermeateRecovery.Text)
        dPermeateflux = Convert.ToDouble(txtPermeateFlux.Text)

        'Feed Flowrate
        dFeedFlowrate = dPermeateflux / dPermeateRecovery 'm3/h
        txtFeedFlowRate.Text = ConvAndRound(dFeedFlowrate)

        'Concentration factor
        dConcentrationFactor = 1 / (1 - dPermeateRecovery)
        txtConcentrationFactor.Text = ConvAndRound(dConcentrationFactor)




    End Sub

    Private Function ConvAndRound(variable As Double) As String
        Return Convert.ToString(Math.Round(variable, 2))
    End Function




End Class