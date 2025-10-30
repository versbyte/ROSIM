Imports System.IO
Imports System.Xml.Serialization

Public Class System_Configuration_Design
    Private currentProjectPath As String
    Private currentProject As WaterAnalysisProject
    Private appState As AppState

    Public waterTypeCondition As String


    Public Sub LoadProject(filePath As String)
        currentProjectPath = filePath
        appState = AppStateManager.GetInstance().State

        Try
            Dim serializer As New XmlSerializer(GetType(WaterAnalysisProject))
            Using reader As New StreamReader(filePath)
                currentProject = CType(serializer.Deserialize(reader), WaterAnalysisProject)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading project: " & ex.Message)
            currentProject = New WaterAnalysisProject()
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
        txtNumberOfStages.Text = If(currentProject.NumberofStages, "")
        txtStagingRatio.Text = If(currentProject.StagingRatio, "")
        txtTotNumElements.Text = If(currentProject.TotalNumberOfElements, "")
        txtNofVessels1.Text = If(currentProject.NumberofVesselsStage1, "")
        txtNofVessels2.Text = If(currentProject.NumberofVesselsStage2, "")

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
        Dim iNumberofSerial As Double
        Dim iNumberofStages As double
        Dim dStagingRatio As Double
        Dim iTotalNumberOfElements As Double


        Dim dNumberofVesselsPerStage1 As Integer
        Dim dNumberofVesselsPerStage2 As Integer


        'Calculations
        dPermeateRecovery = Convert.ToDouble(txtPermeateRecovery.Text)
        dPermeateflux = Convert.ToDouble(txtPermeateFlux.Text)

        'Feed Flowrate
        dFeedFlowrate = dPermeateflux / dPermeateRecovery 'm3/h
        txtFeedFlowRate.Text = ConvAndRound(dFeedFlowrate)

        'Concentration factor
        dConcentrationFactor = 1 / (1 - dPermeateRecovery)
        txtConcentrationFactor.Text = ConvAndRound(dConcentrationFactor)

        'n serial value
        If waterTypeCondition = "Brakisch Water" Then
            If dPermeateRecovery >= 0 And dPermeateRecovery < 0.4 Then
                iNumberofSerial = 6
                iNumberofStages = 1
                txtNumberOfSerial.Text = Convert.ToString(iNumberofSerial)
                txtNumberOfStages.Text = Convert.ToString(iNumberofStages)
            ElseIf dPermeateRecovery >= 0.4 And dPermeateRecovery <= 0.6 Then
                iNumberofSerial = 6
                iNumberofStages = 1
                txtNumberOfSerial.Text = Convert.ToString(iNumberofSerial)
                txtNumberOfStages.Text = Convert.ToString(iNumberofStages)
            ElseIf dPermeateRecovery > 0.6 And dPermeateRecovery <= 0.8 Then
                iNumberofSerial = 12
                iNumberofStages = 2
                txtNumberOfSerial.Text = Convert.ToString(iNumberofSerial)
                txtNumberOfStages.Text = Convert.ToString(iNumberofStages)
            ElseIf dPermeateRecovery > 0.8 And dPermeateRecovery <= 1 Then

                iNumberofSerial = 18
                iNumberofStages = 3
                txtNumberOfSerial.Text = Convert.ToString(iNumberofSerial)
                txtNumberOfStages.Text = Convert.ToString(iNumberofStages)
            Else
                MsgBox("Permeate recovery should be between 0 and 100%")
            End If
        End If

        If waterTypeCondition = "Sea Water" Then
            If dPermeateRecovery >= 0 And dPermeateRecovery < 0.4 Then
                iNumberofSerial = 6
                iNumberofStages = 1


                txtNumberOfSerial.Text = Convert.ToString(iNumberofSerial)
                txtNumberOfStages.Text = Convert.ToString(iNumberofStages)
            ElseIf dPermeateRecovery >= 0.4 And dPermeateRecovery < 0.55 Then
                iNumberofSerial = 8
                iNumberofStages = 2

                txtNumberOfSerial.Text = Convert.ToString(iNumberofSerial)
                txtNumberOfStages.Text = Convert.ToString(iNumberofStages)
            ElseIf dPermeateRecovery >= 0.55 And dPermeateRecovery <= 1 Then
                iNumberofSerial = 14
                iNumberofStages = 2


                txtNumberOfSerial.Text = Convert.ToString(iNumberofSerial)
                txtNumberOfStages.Text = Convert.ToString(iNumberofStages)
            Else
                MsgBox("Permeate recovery should be between 0 and 100%")
            End If
        End If


        'Staging Ratio
        dStagingRatio = Math.Pow((1 / (1 - dPermeateRecovery)), iNumberofStages)
        txtStagingRatio.Text = ConvAndRound(dStagingRatio)

        dSpecificFluxPerUnitArea = Convert.ToDouble(txtSpecificFluxPerUnitArea.Text) / 1000
        dMembraneArea = Convert.ToDouble(txtMembraneArea.Text)
        'Number of vessels per stage
        If iNumberofStages = 1 Then
            dNumberofVesselsPerStage1 = CType((dPermeateflux / (iNumberofSerial * dSpecificFluxPerUnitArea * dMembraneArea)), Integer)
            dNumberofVesselsPerStage2 = 0
            txtNofVessels1.Text = ConvAndRound(dNumberofVesselsPerStage1)
            txtNofVessels2.Text = ConvAndRound(dNumberofVesselsPerStage2)
        ElseIf iNumberofStages = 2 Then
            dNumberofVesselsPerStage2 = CType((dPermeateflux / (iNumberofSerial * dSpecificFluxPerUnitArea * dMembraneArea)), Integer)
            dNumberofVesselsPerStage1 = CType(dStagingRatio * dNumberofVesselsPerStage2, Integer)
            txtNofVessels1.Text = ConvAndRound(dNumberofVesselsPerStage1)
            txtNofVessels2.Text = ConvAndRound(dNumberofVesselsPerStage2)
        End If

        'total number of elements
        iTotalNumberOfElements = (dNumberofVesselsPerStage1 + dNumberofVesselsPerStage2) * iNumberofSerial
        txtTotNumElements.Text = Convert.ToString(iTotalNumberOfElements)

        'element recovery
        dElementRecovery = (1 - Math.Pow((1 - dPermeateRecovery), 1 / iNumberofSerial)) * 100
        txtElementRecovery.Text = ConvAndRound(dElementRecovery)

        'Average system flux
        dAverageSystemFlux = (dPermeateflux / (dMembraneArea * iTotalNumberOfElements)) * 1000
        txtAverageSystemFlux.Text = ConvAndRound(dAverageSystemFlux)

    End Sub

    Private Function ConvAndRound(variable As Double) As String
        Return Convert.ToString(Math.Round(variable, 2))
    End Function


    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        GlobalSaveManager.GetInstance().SetCurrentProject(currentProjectPath, currentProject)
        GlobalSaveManager.GetInstance().SaveAllData()
        WaterAnalysis.LoadProject(currentProjectPath)
        WaterAnalysis.Show()
        Me.Hide()

    End Sub



    Private Sub System_Configuration_Design_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Save app state on form close
        AppStateManager.GetInstance().SaveAppState()
    End Sub


    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        GlobalSaveManager.GetInstance().SetCurrentProject(currentProjectPath, currentProject)
        GlobalSaveManager.GetInstance().SaveAllData()
    End Sub
End Class