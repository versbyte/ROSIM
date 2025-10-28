Imports System.Diagnostics.Contracts

Public Class Form1
    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnVerify.Click

        '------Variables------

        'Physical Properties
        Dim dTemperature As Double
        Dim dpH As Double
        Dim dSDI As Double
        Dim dTDSm As Double

        'Cations
        Dim dNa As Double
        dNa = Convert.ToDouble(txtNa.Text)
        Dim dMg As Double
        dMg = Convert.ToDouble(txtMg.Text)
        Dim dCa As Double
        dCa = Convert.ToDouble(txtCa.Text)
        Dim dK As Double
        dK = Convert.ToDouble(txtK.Text)

        'Anions
        Dim dCl As Double
        dCl = Convert.ToDouble(txtCl.Text)
        Dim dSO4 As Double
        dSO4 = Convert.ToDouble(txtSO4.Text)
        Dim dHCO3 As Double
        dHCO3 = Convert.ToDouble(txtHCO3.Text)
        Dim dCO3 As Double
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


        txtIonicBalaceError.Text = Convert.ToString(Math.Round(dIBE, 2))

        'TDSc calculation

        For i As Integer = 0 To LCmgl.Count - 1
            dTDSc += LCmgl(i)
        Next

        txtTDSc.Text = Convert.ToString(Math.Round(dTDSc, 2))

        'Hardness calculation
        dHardness = dCa * 2.497 + dMg * 4.118
        txtHardness.Text = Convert.ToString(Math.Round(dHardness, 2))

        'Alkalinity calculation
        dAlkalinity = (dHCO3 / 61.016) * 50 + (dCO3 / 60.008) * 50
        txtAlkalinity.Text = Convert.ToString(Math.Round(dAlkalinity, 2))

        'Osmotic Pressure calculation
        dOsmoticPressure = 1.12 * dTDSc * (273.15 + dTemperature) * 0.00001
        txtOsmoticPressure.Text = Convert.ToString(Math.Round(dOsmoticPressure, 2))
    End Sub


End Class
