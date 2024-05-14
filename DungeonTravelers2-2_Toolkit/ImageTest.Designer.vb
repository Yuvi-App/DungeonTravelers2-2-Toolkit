<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImageTest
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnLoadBytes = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ofdLoadRawBytes = New System.Windows.Forms.OpenFileDialog()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(12, 79)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(468, 266)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'btnLoadBytes
        '
        Me.btnLoadBytes.Location = New System.Drawing.Point(229, 25)
        Me.btnLoadBytes.Name = "btnLoadBytes"
        Me.btnLoadBytes.Size = New System.Drawing.Size(75, 23)
        Me.btnLoadBytes.TabIndex = 1
        Me.btnLoadBytes.Text = "Load Bytes"
        Me.btnLoadBytes.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(211, 15)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "This will load RAW BYTES into a picture"
        '
        'ofdLoadRawBytes
        '
        Me.ofdLoadRawBytes.FileName = "OpenFileDialog1"
        '
        'ImageTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(488, 351)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnLoadBytes)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "ImageTest"
        Me.Text = "ImageTest"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnLoadBytes As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents ofdLoadRawBytes As OpenFileDialog
End Class
