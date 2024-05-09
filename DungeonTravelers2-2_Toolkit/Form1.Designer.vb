<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PCKExtractorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PCKCreatorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.VITAToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.BulkToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SingleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PCToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.BulkToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SingleToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TEXCreatorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VITAToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PCVitaTEXToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ofdPCKExtractor = New System.Windows.Forms.OpenFileDialog()
        Me.ofdVitaTex = New System.Windows.Forms.OpenFileDialog()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ofdPCTex = New System.Windows.Forms.OpenFileDialog()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ofdNewPNGTex = New System.Windows.Forms.OpenFileDialog()
        Me.fbdNewPCK = New System.Windows.Forms.FolderBrowserDialog()
        Me.fbdBulkTEX = New System.Windows.Forms.FolderBrowserDialog()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ToolsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(353, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.ExitToolStripMenuItem1})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.ExitToolStripMenuItem.Text = "About"
        '
        'ExitToolStripMenuItem1
        '
        Me.ExitToolStripMenuItem1.Name = "ExitToolStripMenuItem1"
        Me.ExitToolStripMenuItem1.Size = New System.Drawing.Size(107, 22)
        Me.ExitToolStripMenuItem1.Text = "Exit"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PCKExtractorToolStripMenuItem, Me.PCKCreatorToolStripMenuItem, Me.ToolStripMenuItem1, Me.TEXCreatorToolStripMenuItem, Me.PCVitaTEXToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'PCKExtractorToolStripMenuItem
        '
        Me.PCKExtractorToolStripMenuItem.Name = "PCKExtractorToolStripMenuItem"
        Me.PCKExtractorToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PCKExtractorToolStripMenuItem.Text = "PCK Extractor"
        '
        'PCKCreatorToolStripMenuItem
        '
        Me.PCKCreatorToolStripMenuItem.Name = "PCKCreatorToolStripMenuItem"
        Me.PCKCreatorToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PCKCreatorToolStripMenuItem.Text = "PCK Creator"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VITAToolStripMenuItem1, Me.PCToolStripMenuItem1})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.ToolStripMenuItem1.Text = "TEX Extractor"
        '
        'VITAToolStripMenuItem1
        '
        Me.VITAToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BulkToolStripMenuItem, Me.SingleToolStripMenuItem})
        Me.VITAToolStripMenuItem1.Name = "VITAToolStripMenuItem1"
        Me.VITAToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.VITAToolStripMenuItem1.Text = "VITA"
        '
        'BulkToolStripMenuItem
        '
        Me.BulkToolStripMenuItem.Name = "BulkToolStripMenuItem"
        Me.BulkToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.BulkToolStripMenuItem.Text = "Bulk"
        '
        'SingleToolStripMenuItem
        '
        Me.SingleToolStripMenuItem.Name = "SingleToolStripMenuItem"
        Me.SingleToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.SingleToolStripMenuItem.Text = "Single"
        '
        'PCToolStripMenuItem1
        '
        Me.PCToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BulkToolStripMenuItem1, Me.SingleToolStripMenuItem1})
        Me.PCToolStripMenuItem1.Name = "PCToolStripMenuItem1"
        Me.PCToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.PCToolStripMenuItem1.Text = "PC"
        '
        'BulkToolStripMenuItem1
        '
        Me.BulkToolStripMenuItem1.Name = "BulkToolStripMenuItem1"
        Me.BulkToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.BulkToolStripMenuItem1.Text = "Bulk"
        '
        'SingleToolStripMenuItem1
        '
        Me.SingleToolStripMenuItem1.Name = "SingleToolStripMenuItem1"
        Me.SingleToolStripMenuItem1.Size = New System.Drawing.Size(180, 22)
        Me.SingleToolStripMenuItem1.Text = "Single"
        '
        'TEXCreatorToolStripMenuItem
        '
        Me.TEXCreatorToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VITAToolStripMenuItem, Me.PCToolStripMenuItem})
        Me.TEXCreatorToolStripMenuItem.Name = "TEXCreatorToolStripMenuItem"
        Me.TEXCreatorToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.TEXCreatorToolStripMenuItem.Text = "TEX Creator"
        '
        'VITAToolStripMenuItem
        '
        Me.VITAToolStripMenuItem.Name = "VITAToolStripMenuItem"
        Me.VITAToolStripMenuItem.Size = New System.Drawing.Size(97, 22)
        Me.VITAToolStripMenuItem.Text = "VITA"
        '
        'PCToolStripMenuItem
        '
        Me.PCToolStripMenuItem.Name = "PCToolStripMenuItem"
        Me.PCToolStripMenuItem.Size = New System.Drawing.Size(97, 22)
        Me.PCToolStripMenuItem.Text = "PC"
        '
        'PCVitaTEXToolStripMenuItem
        '
        Me.PCVitaTEXToolStripMenuItem.Name = "PCVitaTEXToolStripMenuItem"
        Me.PCVitaTEXToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PCVitaTEXToolStripMenuItem.Text = "PC -> Vita TEX"
        '
        'ofdPCKExtractor
        '
        Me.ofdPCKExtractor.FileName = "OpenFileDialog1"
        '
        'ofdVitaTex
        '
        Me.ofdVitaTex.FileName = "OpenFileDialog1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(102, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(139, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Use the buttons above :D"
        '
        'ofdPCTex
        '
        Me.ofdPCTex.FileName = "OpenFileDialog1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(300, 110)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 15)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Yuvi v1.0"
        '
        'ofdNewPNGTex
        '
        Me.ofdNewPNGTex.FileName = "OpenFileDialog1"
        '
        'fbdNewPCK
        '
        Me.fbdNewPCK.Description = "Select DIR of file for new PCK"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(353, 134)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Dungeon Travelers 2-2 Toolkit"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PCKExtractorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ofdPCKExtractor As OpenFileDialog
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ofdVitaTex As OpenFileDialog
    Friend WithEvents PCVitaTEXToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label1 As Label
    Friend WithEvents ofdPCTex As OpenFileDialog
    Friend WithEvents TEXCreatorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VITAToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PCToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PCKCreatorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VITAToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents PCToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents Label2 As Label
    Friend WithEvents ofdNewPNGTex As OpenFileDialog
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents fbdNewPCK As FolderBrowserDialog
    Friend WithEvents BulkToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SingleToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents fbdBulkTEX As FolderBrowserDialog
    Friend WithEvents BulkToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SingleToolStripMenuItem1 As ToolStripMenuItem
End Class
