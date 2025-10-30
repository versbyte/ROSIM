Public Class AppState

    ' Startup Form Data
    Public Property LastProjectsDirectory As String
    Public Property RecentProjects As List(Of String)

    ' Water Analysis Form Data
    Public Property CurrentProjectPath As String
    Public Property CurrentProjectName As String

    ' Physical Properties
    Public Property Temperature As String
    Public Property FeedPH As String
    Public Property SDI15min As String
    Public Property TDSm As String
    Public Property WaterType As String

    ' Cations
    Public Property Na As String
    Public Property Mg As String
    Public Property Ca As String
    Public Property K As String

    ' Anions
    Public Property Cl As String
    Public Property SO4 As String
    Public Property HCO3 As String
    Public Property CO3 As String

    ' Calculations
    Public Property IonicBalanceError As String
    Public Property TDSCalc As String
    Public Property Hardness As String
    Public Property Alkalinity As String
    Public Property OsmoticPressure As String

    ' System Design
    Public Property PermeateRecovery As String
    Public Property Permeateflux As String
    Public Property FeedFlowrate As String
    Public Property ConcentrationFactor As String
    Public Property SpecificFluxPerUnitArea As String
    Public Property MembraneArea As String
    Public Property ElementRecovery As String
    Public Property AverageSystemFlux As String
    Public Property NumberofSerial As String
    Public Property NumberofStages As String
    Public Property StagingRatio As String
    Public Property TotalNumberOfElements As String

    Public Property NumberofVesselsStage1 As String

    Public Property NumberofVesselsStage2 As String

    ' Window States
    Public Property FormLocation As String ' Serialized Point
    Public Property FormSize As String ' Serialized Size

    Public Sub New()
        RecentProjects = New List(Of String)()
    End Sub


End Class
