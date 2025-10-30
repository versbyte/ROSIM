<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StartupWindow
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnNewProject = New System.Windows.Forms.Button()
        Me.bntOpenProject = New System.Windows.Forms.Button()
        Me.lstProjects = New System.Windows.Forms.ListBox()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnNewProject
        '
        Me.btnNewProject.BackColor = System.Drawing.SystemColors.Highlight
        Me.btnNewProject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewProject.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnNewProject.Location = New System.Drawing.Point(12, 31)
        Me.btnNewProject.Name = "btnNewProject"
        Me.btnNewProject.Size = New System.Drawing.Size(139, 38)
        Me.btnNewProject.TabIndex = 8
        Me.btnNewProject.Text = "New Project"
        Me.btnNewProject.UseVisualStyleBackColor = False
        '
        'bntOpenProject
        '
        Me.bntOpenProject.BackColor = System.Drawing.SystemColors.Highlight
        Me.bntOpenProject.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bntOpenProject.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.bntOpenProject.Location = New System.Drawing.Point(12, 119)
        Me.bntOpenProject.Name = "bntOpenProject"
        Me.bntOpenProject.Size = New System.Drawing.Size(139, 38)
        Me.bntOpenProject.TabIndex = 9
        Me.bntOpenProject.Text = "Open Project"
        Me.bntOpenProject.UseVisualStyleBackColor = False
        '
        'lstProjects
        '
        Me.lstProjects.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lstProjects.Font = New System.Drawing.Font("Cascadia Code", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstProjects.ForeColor = System.Drawing.SystemColors.MenuHighlight
        Me.lstProjects.FormattingEnabled = True
        Me.lstProjects.ItemHeight = 21
        Me.lstProjects.Location = New System.Drawing.Point(157, 31)
        Me.lstProjects.Name = "lstProjects"
        Me.lstProjects.Size = New System.Drawing.Size(316, 359)
        Me.lstProjects.TabIndex = 10
        '
        'lblInfo
        '
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Location = New System.Drawing.Point(153, 407)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(57, 20)
        Me.lblInfo.TabIndex = 11
        Me.lblInfo.Text = "Label1"
        '
        'btnImport
        '
        Me.btnImport.BackColor = System.Drawing.SystemColors.Highlight
        Me.btnImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImport.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnImport.Location = New System.Drawing.Point(12, 75)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(139, 38)
        Me.btnImport.TabIndex = 12
        Me.btnImport.Text = "Import"
        Me.btnImport.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.IndianRed
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnDelete.Location = New System.Drawing.Point(12, 163)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(139, 38)
        Me.btnDelete.TabIndex = 13
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.IndianRed
        Me.btnExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnExit.Location = New System.Drawing.Point(12, 207)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(139, 38)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'StartupWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(524, 446)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.lstProjects)
        Me.Controls.Add(Me.bntOpenProject)
        Me.Controls.Add(Me.btnNewProject)
        Me.Name = "StartupWindow"
        Me.Text = "ROSIM"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnNewProject As Button
    Friend WithEvents bntOpenProject As Button
    Friend WithEvents lstProjects As ListBox
    Friend WithEvents lblInfo As Label
    Friend WithEvents btnImport As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnExit As Button
End Class
