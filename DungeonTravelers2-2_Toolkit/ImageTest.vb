Imports System.IO

Public Class ImageTest
    Private Sub btnLoadBytes_Click(sender As Object, e As EventArgs) Handles btnLoadBytes.Click
        If ofdLoadRawBytes.ShowDialog = DialogResult.OK Then
            Dim RawBytes = File.ReadAllBytes(ofdLoadRawBytes.FileName)
            Dim pictureBytes As New MemoryStream(RawBytes)
            PictureBox1.Image = Image.FromStream(pictureBytes)
        End If
    End Sub
End Class