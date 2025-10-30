Imports System.Diagnostics.Contracts
Imports System.Diagnostics.Eventing.Reader
Imports System.Drawing.Text
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml.Serialization

Public Class WaterAnalysis

    Private currentProjectPath As String
    Private currentProject As WaterAnalysisProject
    Private appState As AppState

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

        Me.Text = "Water Analysis - " & currentProject.ProjectName
        LoadDataToUI()
    End Sub

    Public Sub LoadDataToUI()
        txtTemperature.Text = If(currentProject.Temperature, "")
        txtpH.Text = If(currentProject.FeedPH, "")
        txtSDI.Text = If(currentProject.SDI15min, "")
        txtTDSm.Text = If(currentProject.TDSm, "")
        txtNa.Text = If(currentProject.Na, "")
        txtMg.Text = If(currentProject.Mg, "")
        txtCa.Text = If(currentProject.Ca, "")
        txtK.Text = If(currentProject.K, "")
        txtCl.Text = If(currentProject.Cl, "")
        txtSO4.Text = If(currentProject.SO4, "")
        txtHCO3.Text = If(currentProject.HCO3, "")
        txtCO3.Text = If(currentProject.CO3, "")
        txtIonicBalaceError.Text = If(currentProject.IonicBalanceError, "")
        txtTDSc.Text = If(currentProject.TDSCalc, "")
        txtHardness.Text = If(currentProject.Hardness, "")
        txtAlkalinity.Text = If(currentProject.Alkalinity, "")
        txtOsmoticPressure.Text = If(currentProject.OsmoticPressure, "")
    End Sub
    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnEvaluate.Click


        'Check if all the fields are not empty
        Dim textBoxes As New List(Of TextBox) From {
        txtTemperature, txtpH, txtSDI, txtTDSm, txtNa, txtMg, txtCa, txtK, txtCl, txtSO4, txtHCO3, txtCO3
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




        '------Variables------

        'Physical Properties
        Dim dTemperature As Double
        Dim dpH As Double
        Dim dSDI As Double
        Dim dTDSm As Double



        'Cations
        Dim dNa As Double
        Dim dMg As Double
        Dim dCa As Double
        Dim dK As Double


        'Anions
        Dim dCl As Double
        Dim dSO4 As Double
        Dim dHCO3 As Double
        Dim dCO3 As Double


        dNa = Convert.ToDouble(txtNa.Text)
        dMg = Convert.ToDouble(txtMg.Text)
        dCa = Convert.ToDouble(txtCa.Text)
        dK = Convert.ToDouble(txtK.Text)
        dCl = Convert.ToDouble(txtCl.Text)
        dSO4 = Convert.ToDouble(txtSO4.Text)
        dHCO3 = Convert.ToDouble(txtHCO3.Text)
        dCO3 = Convert.ToDouble(txtCO3.Text)

        Dim LCmgl As New List(Of Double) From {dNa, dMg, dCa, dK,
                                                dCl, dSO4, dHCO3, dCO3} 'mg/l

        'Calculation Variables
        Dim LCeq_Cations As New List(Of Double) 'Na - Mg - Ca - K
        Dim LCeq_Anions As New List(Of Double)  'Cl - SO4 - HCO3 - CO3
        Dim dCeqi As Double
        Dim dTotalCations_eq As Double = 0
        Dim dTotalAnions_eq As Double = 0
        Dim lEw As New List(Of Double) 'Equivalent weights list
        Dim dEwi As Double
        Dim dTDSc As Double = 0
        Dim dHardness As Double
        Dim dAlkalinity As Double
        Dim dOsmoticPressure As Double

        'Molecular Weights
        Dim LMw As New List(Of Double) From {22.989, 24.305, 40.078, 39.098,
                                             35.446, 96.026, 61.016, 60.008} 'g/mol

        Dim LCharges As New List(Of Integer) From {1, 2, 2, 1, 1, 2, 1, 2}


        'Calculations


        'Equivalent weights calculation
        For i As Integer = 0 To LMw.Count - 1

            dEwi = LMw(i) / LCharges(i)
            lEw.Add(dEwi)

        Next

        'Concentration meq/l calculation

        'Cations
        For j As Integer = 0 To LMw.Count - 1
            dCeqi = LCmgl(j) / lEw(j)  'meq/l
            LCeq_Cations.Add(dCeqi)
        Next

        'Anions
        For k As Integer = 4 To LCmgl.Count - 1
            dCeqi = LCmgl(k) / lEw(k)
            LCeq_Anions.Add(dCeqi)
        Next

        'Ionic balance Error calculation
        Dim dIBE As Double

        For i As Integer = 0 To LCeq_Anions.Count - 1
            dTotalCations_eq += LCeq_Cations(i)
            dTotalAnions_eq += LCeq_Anions(i)
        Next
        dIBE = ((Math.Abs(dTotalCations_eq - dTotalAnions_eq)) _
                                    / (dTotalCations_eq + dTotalAnions_eq)) * 100


        txtIonicBalaceError.Text = ConvAndRound(dIBE)

        'TDSc calculation

        For i As Integer = 0 To LCmgl.Count - 1
            dTDSc += LCmgl(i)
        Next

        txtTDSc.Text = ConvAndRound(dTDSc)

        'Hardness calculation
        dHardness = dCa * 2.497 + dMg * 4.118
        txtHardness.Text = ConvAndRound(dHardness)

        'Alkalinity calculation
        dAlkalinity = (dHCO3 / 61.016) * 50 + (dCO3 / 60.008) * 50
        txtAlkalinity.Text = ConvAndRound(dAlkalinity)

        'Osmotic Pressure calculation
        dOsmoticPressure = 1.12 * dTDSc * (273.15 + dTemperature) * 0.00001
        txtOsmoticPressure.Text = ConvAndRound(dOsmoticPressure)

        btnNext.Enabled = True

    End Sub

    Private Function ConvAndRound(variable As Double) As String
        Return Convert.ToString(Math.Round(variable, 2))
    End Function

    Private Sub bntSave_Click(sender As Object, e As EventArgs) Handles bntSave.Click
        GlobalSaveManager.GetInstance().SetCurrentProject(currentProjectPath, currentProject)
        GlobalSaveManager.GetInstance().SaveAllData()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        ' Save Water Analysis data first
        bntSave_Click(Nothing, Nothing)

        ' Load System Design form with current project
        System_Configuration_Design.LoadProject(currentProjectPath)
        System_Configuration_Design.Show()
        Me.Hide()
    End Sub

    Private Sub WaterAnalysis_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
