Imports System.IO
Imports System.Xml.Serialization

' ===== GLOBAL SAVE MANAGER =====
Public Class GlobalSaveManager
    Private Shared instance As GlobalSaveManager
    Private Shared ReadOnly lockObj As Object = New Object()

    Public Shared Function GetInstance() As GlobalSaveManager
        If instance Is Nothing Then
            SyncLock lockObj
                If instance Is Nothing Then
                    instance = New GlobalSaveManager()
                End If
            End SyncLock
        End If
        Return instance
    End Function

    Public currentProjectPath As String
    Public currentProject As WaterAnalysisProject
    Public appState As AppState

    Private Sub New()
        appState = AppStateManager.GetInstance().State
    End Sub

    Public Sub SaveAllData()
        Try
            If String.IsNullOrEmpty(currentProjectPath) Then
                MessageBox.Show("No project is currently open!")
                Return
            End If

            ' Get references to all open forms
            Dim waterAnalysisForm As WaterAnalysis = Nothing
            Dim systemDesignForm As System_Configuration_Design = Nothing

            ' Find open forms
            For Each form In Application.OpenForms
                If TypeOf form Is WaterAnalysis Then
                    waterAnalysisForm = CType(form, WaterAnalysis)
                ElseIf TypeOf form Is System_Configuration_Design Then
                    systemDesignForm = CType(form, System_Configuration_Design)
                End If
            Next

            ' Update project data from WaterAnalysis form if it's open
            If waterAnalysisForm IsNot Nothing Then
                UpdateProjectFromWaterAnalysis(waterAnalysisForm)
            End If

            ' Update project data from System Design form if it's open
            If systemDesignForm IsNot Nothing Then
                UpdateProjectFromSystemDesign(systemDesignForm)
            End If

            ' Update last modified time
            currentProject.LastModified = DateTime.Now

            ' Save project file
            Dim serializer As New XmlSerializer(GetType(WaterAnalysisProject))
            Using writer As New StreamWriter(currentProjectPath)
                serializer.Serialize(writer, currentProject)
            End Using

            ' Save app state
            appState = AppStateManager.GetInstance().State
            AppStateManager.GetInstance().SaveAppState()

            'MessageBox.Show("All data saved successfully!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error saving data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UpdateProjectFromWaterAnalysis(form As WaterAnalysis)
        ' Water Analysis data
        currentProject.Temperature = form.txtTemperature.Text
        currentProject.FeedPH = form.txtpH.Text
        currentProject.SDI15min = form.txtSDI.Text
        currentProject.TDSm = form.txtTDSm.Text
        currentProject.WaterType = form.cbWaterType.Text
        currentProject.Na = form.txtNa.Text
        currentProject.Mg = form.txtMg.Text
        currentProject.Ca = form.txtCa.Text
        currentProject.K = form.txtK.Text
        currentProject.Cl = form.txtCl.Text
        currentProject.SO4 = form.txtSO4.Text
        currentProject.HCO3 = form.txtHCO3.Text
        currentProject.CO3 = form.txtCO3.Text
        currentProject.IonicBalanceError = form.txtIonicBalaceError.Text
        currentProject.TDSCalc = form.txtTDSc.Text
        currentProject.Hardness = form.txtHardness.Text
        currentProject.Alkalinity = form.txtAlkalinity.Text
        currentProject.OsmoticPressure = form.txtOsmoticPressure.Text
        currentProject.WaterType = form.cbWaterType.Text

        ' Update app state
        appState.Temperature = form.txtTemperature.Text
        appState.FeedPH = form.txtpH.Text
        appState.SDI15min = form.txtSDI.Text
        appState.TDSm = form.txtTDSm.Text
        appState.WaterType = form.cbWaterType.Text
        appState.Na = form.txtNa.Text
        appState.Mg = form.txtMg.Text
        appState.Ca = form.txtCa.Text
        appState.K = form.txtK.Text
        appState.Cl = form.txtCl.Text
        appState.SO4 = form.txtSO4.Text
        appState.HCO3 = form.txtHCO3.Text
        appState.CO3 = form.txtCO3.Text
        appState.IonicBalanceError = form.txtIonicBalaceError.Text
        appState.TDSCalc = form.txtTDSc.Text
        appState.Hardness = form.txtHardness.Text
        appState.Alkalinity = form.txtAlkalinity.Text
        appState.OsmoticPressure = form.txtOsmoticPressure.Text
        appState.WaterType = form.cbWaterType.Text
    End Sub

    Private Sub UpdateProjectFromSystemDesign(form As System_Configuration_Design)
        ' System Design data
        currentProject.PermeateRecovery = form.txtPermeateRecovery.Text
        currentProject.Permeateflux = form.txtPermeateFlux.Text
        currentProject.FeedFlowrate = form.txtFeedFlowRate.Text
        currentProject.ConcentrationFactor = form.txtConcentrationFactor.Text
        currentProject.SpecificFluxPerUnitArea = form.txtSpecificFluxPerUnitArea.Text
        currentProject.MembraneArea = form.txtMembraneArea.Text
        currentProject.ElementRecovery = form.txtElementRecovery.Text
        currentProject.AverageSystemFlux = form.txtAverageSystemFlux.Text
        currentProject.NumberofSerial = form.txtNumberOfSerial.Text
        currentProject.NumberofStages = form.txtNumberOfStages.Text
        currentProject.StagingRatio = form.txtStagingRatio.Text
        currentProject.TotalNumberOfElements = form.txtTotNumElements.Text
        currentProject.NumberofVesselsStage1 = form.txtNofVessels1.Text
        currentProject.NumberofVesselsStage2 = form.txtNofVessels2.Text

        ' Update app state
        appState.PermeateRecovery = form.txtPermeateRecovery.Text
        appState.Permeateflux = form.txtPermeateFlux.Text
        appState.FeedFlowrate = form.txtFeedFlowRate.Text
        appState.ConcentrationFactor = form.txtConcentrationFactor.Text
        appState.SpecificFluxPerUnitArea = form.txtSpecificFluxPerUnitArea.Text
        appState.MembraneArea = form.txtMembraneArea.Text
        appState.ElementRecovery = form.txtElementRecovery.Text
        appState.AverageSystemFlux = form.txtAverageSystemFlux.Text
        appState.NumberofSerial = form.txtNumberOfSerial.Text
        appState.NumberofStages = form.txtNumberOfStages.Text
        appState.StagingRatio = form.txtStagingRatio.Text
        appState.TotalNumberOfElements = form.txtTotNumElements.Text
        appState.NumberofVesselsStage1 = form.txtNofVessels1.Text
        appState.NumberofVesselsStage2 = form.txtNofVessels2.Text
    End Sub

    Public Sub SetCurrentProject(filePath As String, project As WaterAnalysisProject)
        currentProjectPath = filePath
        currentProject = project
    End Sub
End Class