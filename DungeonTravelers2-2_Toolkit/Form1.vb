Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging


Public Class Form1
    'PCK EXTRACTOR
    Private Sub PCKExtractorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PCKExtractorToolStripMenuItem.Click
        Dim WorkingPCKFile = ""
        If ofdPCKExtractor.ShowDialog = DialogResult.OK Then
            WorkingPCKFile = ofdPCKExtractor.FileName
        Else
            Exit Sub
        End If

        ExtractPCK(WorkingPCKFile)
    End Sub
    Public Function ExtractPCK(ByVal InputFile As String)
        'Setup Directory
        Dim PakName = Path.GetFileNameWithoutExtension(InputFile)
        Dim ExportDIR = "Export\" & PakName & "\"
        If Directory.Exists(ExportDIR) = False Then
            Directory.CreateDirectory(ExportDIR)
        End If

        Dim PCKHeader = "Filename"
        Using br As BinaryReader = New BinaryReader(File.Open(InputFile, FileMode.Open))
            Dim HeaderCheck = br.ReadBytes(&H8)
            br.ReadBytes(&HC)

            If PCKHeader <> Encoding.UTF8.GetString(HeaderCheck) Then
                MessageBox.Show("Incorrect PCK File")
                Exit Function
            End If

            Dim NearPackDataStart = br.ReadUInt32
            Dim ReturnPOS = br.BaseStream.Position
            Dim ExactPackDataStart As UInt32
            'Find the Exact PACK POS
            br.BaseStream.Seek(NearPackDataStart, SeekOrigin.Begin)
            While True
                Dim checkbyte = br.ReadByte
                If checkbyte = &H50 Then
                    ExactPackDataStart = br.BaseStream.Position - 1
                    Exit While
                End If
            End While
            br.BaseStream.Seek(ReturnPOS, SeekOrigin.Begin)
            Dim FileNameTablePOS = br.BaseStream.Position

            'Start Getting Filenames
            Dim FileNameList As New List(Of String)
            While True
                Dim FileNameOffset = br.ReadUInt32
                ReturnPOS = br.BaseStream.Position

                'Check if we have reached the end
                Dim FilenameStartPOS = FileNameTablePOS + FileNameOffset
                If FilenameStartPOS > NearPackDataStart Then
                    Exit While
                Else
                    br.BaseStream.Seek(FilenameStartPOS, SeekOrigin.Begin)
                End If

                'Get File name
                Dim LetterCount = 0
                While True
                    Dim Checkbyte = br.ReadByte
                    If Checkbyte = 0 Then
                        br.BaseStream.Seek(FilenameStartPOS, SeekOrigin.Begin)
                        Dim Filename = Encoding.UTF8.GetString(br.ReadBytes(LetterCount))
                        FileNameList.Add(Filename)
                        Exit While
                    Else
                        LetterCount += 1
                    End If
                End While
                br.BaseStream.Seek(ReturnPOS, SeekOrigin.Begin)
            End While


            'Lets get the Data for the Files now
            br.BaseStream.Seek(ExactPackDataStart, SeekOrigin.Begin)
            br.ReadBytes(&H14)
            Dim PointerTableLength = br.ReadUInt32
            Dim PointerCount = br.ReadUInt32
            For Each fn In FileNameList
                'GetFile 
                Dim FirstFileStartPOS = br.ReadUInt32
                Dim FileLength = br.ReadUInt32
                ReturnPOS = br.BaseStream.Position

                br.BaseStream.Seek(FirstFileStartPOS, SeekOrigin.Begin)
                Dim FileBytes = br.ReadBytes(FileLength)

                Dim FileExport = ExportDIR & fn
                If File.Exists(FileExport) Then File.Delete(FileExport)
                Using bw As BinaryWriter = New BinaryWriter(File.Open(FileExport, FileMode.Create))
                    bw.Write(FileBytes)
                End Using
                br.BaseStream.Seek(ReturnPOS, SeekOrigin.Begin)
            Next
        End Using
        MessageBox.Show("Extracted PCK")
    End Function


    'PCK CREATOR
    Private Sub PCKCreatorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PCKCreatorToolStripMenuItem.Click
        Dim WorkingPCKDIR = ""
        If fbdNewPCK.ShowDialog = DialogResult.OK Then
            WorkingPCKDIR = fbdNewPCK.SelectedPath
        Else
            Exit Sub
        End If

        CreatePCK(WorkingPCKDIR)
    End Sub
    Public Function CreatePCK(ByVal InputDIR As String)
        'Setup Directory
        Dim BuiltDIR = "Built\PCK"
        If Directory.Exists(BuiltDIR) = False Then
            Directory.CreateDirectory(BuiltDIR)
        End If
        Dim NewPCK = BuiltDIR & "\" & Path.GetFileName(InputDIR.TrimEnd(Path.DirectorySeparatorChar)) & ".pck"
        If File.Exists(NewPCK) = True Then
            File.Delete(NewPCK)
        End If

        'Get FileNames in DIR
        Dim FileNameList As New List(Of String)
        For Each F In Directory.GetFiles(InputDIR)
            FileNameList.Add(Path.GetFileName(F))
        Next
        Dim FileCount As UInt32 = FileNameList.Count


        Dim FileSignature As Byte() = {&H46, &H69, &H6C, &H65, &H6E, &H61, &H6D, &H65}
        Dim HeaderFill As Byte() = {&H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20}
        Dim Uint32Zeros As Byte() = {&H0, &H0, &H0, &H0}
        Dim FileNameAfterFill As Byte() = {&H0, &H0, &H0, &H0, &H0, &H0, &H0}
        Dim PackSectionheader As Byte() = {&H50, &H61, &H63, &H6B}
        Dim PackSectionFill As Byte() = {&H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20}
        Using BW As BinaryWriter = New BinaryWriter(File.Open(NewPCK, FileMode.Create))
            BW.Write(FileSignature)
            BW.Write(HeaderFill)

            'Write PACK START POS, We will come back here later
            Dim PACKSTARTRETURNPOS = BW.BaseStream.Position
            BW.Write(Uint32Zeros)

            'FileNameOffset POS, Come back later to fill this out
            Dim FileNameOffSet = BW.BaseStream.Position
            Dim CurrentFileNameOffsetPOS = BW.BaseStream.Position
            For i As Integer = 1 To FileCount
                BW.Write(Uint32Zeros)
            Next

            'Insert Filename
            Dim FilenameReturnPOS As UInt32 = BW.BaseStream.Position
            For Each FN In FileNameList
                FilenameReturnPOS = BW.BaseStream.Position

                'Go Write the offset to Pointer Above
                Dim OffsetDifference As UInt32 = FilenameReturnPOS - FileNameOffSet
                BW.BaseStream.Seek(CurrentFileNameOffsetPOS, SeekOrigin.Begin)
                BW.Write(OffsetDifference)
                CurrentFileNameOffsetPOS = BW.BaseStream.Position

                'Return to Write Filename
                BW.BaseStream.Seek(FilenameReturnPOS, SeekOrigin.Begin)
                Dim FilenameBytes = Encoding.UTF8.GetBytes(FN)
                BW.Write(FilenameBytes)
                BW.BaseStream.WriteByte(&H0)
            Next

            'Start of PACK Section
            Dim PackSTARTPOS As UInt32 = BW.BaseStream.Position

            'Go back insert PackStartPOS
            BW.BaseStream.Seek(PACKSTARTRETURNPOS, SeekOrigin.Begin)
            BW.Write(PackSTARTPOS)

            'Return to Pack Section
            BW.BaseStream.Seek(PackSTARTPOS, SeekOrigin.Begin)
            BW.Write(FileNameAfterFill)
            BW.Write(PackSectionheader)
            BW.Write(PackSectionFill)

            'Start Pointer Table for File Data
            Dim PointerTableLengthRETURNPOS = BW.BaseStream.Position
            BW.Write(Uint32Zeros)
            Dim PointerTableCountRETURNPOS = BW.BaseStream.Position
            BW.Write(FileCount)

            Dim CurrentPointerPOS = BW.BaseStream.Position
            For Each FN In FileNameList
                BW.Write(Uint32Zeros)
                BW.Write(Uint32Zeros)
            Next
            BW.Write(Uint32Zeros)
            Dim StartFileDataPOS = BW.BaseStream.Position

            'Write PTLength
            Dim PTLength As UInt32 = StartFileDataPOS - PointerTableLengthRETURNPOS + &H10
            BW.BaseStream.Seek(PointerTableLengthRETURNPOS, SeekOrigin.Begin)
            BW.Write(PTLength)

            'Return FileDataStart
            BW.BaseStream.Seek(StartFileDataPOS, SeekOrigin.Begin)

            'Start Writing File Data
            For Each F In Directory.GetFiles(InputDIR)
                Dim FileBytes = File.ReadAllBytes(F)
                Dim FileLength As UInt32 = FileBytes.Count
                Dim FileSTARTPOS As UInt32 = BW.BaseStream.Position

                'Return to Write File Start POS
                BW.BaseStream.Seek(CurrentPointerPOS, SeekOrigin.Begin)
                BW.Write(FileSTARTPOS)
                BW.Write(FileLength)
                CurrentPointerPOS = BW.BaseStream.Position

                'Return to WriteData
                BW.BaseStream.Seek(FileSTARTPOS, SeekOrigin.Begin)
                BW.Write(FileBytes)
            Next
        End Using

        MessageBox.Show("Created new PCK Successfully")
    End Function


    'TEX EXTRACTOR
    Private Sub PCBulkToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles BulkToolStripMenuItem1.Click
        Dim WorkingTEXFileFolder = ""
        fbdBulkTEX.Description = "Select Folder that contains VITA .TEX you wish to extract"
        If fbdBulkTEX.ShowDialog = DialogResult.OK Then
            WorkingTEXFileFolder = fbdBulkTEX.SelectedPath
        Else
            Exit Sub
        End If

        For Each f In Directory.GetFiles(WorkingTEXFileFolder, "*.tex")
            ExtractTEX(f, "PC")
        Next
        MessageBox.Show("Extracted TEXs")
    End Sub
    Private Sub PCSingleToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SingleToolStripMenuItem1.Click
        Dim WorkingTEXFile = ""
        If ofdVitaTex.ShowDialog = DialogResult.OK Then
            WorkingTEXFile = ofdVitaTex.FileName
        Else
            Exit Sub
        End If

        ExtractTEX(WorkingTEXFile, "PC")
        MessageBox.Show("Extracted TEX")
    End Sub
    Private Sub VITABulkToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BulkToolStripMenuItem.Click
        Dim WorkingTEXFileFolder = ""
        fbdBulkTEX.Description = "Select Folder that contains VITA .TEX you wish to extract"
        If fbdBulkTEX.ShowDialog = DialogResult.OK Then
            WorkingTEXFileFolder = fbdBulkTEX.SelectedPath
        Else
            Exit Sub
        End If

        For Each f In Directory.GetFiles(WorkingTEXFileFolder, "*.tex")
            ExtractTEX(f, "VITA")
        Next
        MessageBox.Show("Extracted TEXs")
    End Sub
    Private Sub VITASingleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SingleToolStripMenuItem.Click
        Dim WorkingTEXFile = ""
        ofdVitaTex.Title = "Select VITA .TEX you wish to extract"
        If ofdVitaTex.ShowDialog = DialogResult.OK Then
            WorkingTEXFile = ofdVitaTex.FileName
        Else
            Exit Sub
        End If

        ExtractTEX(WorkingTEXFile, "VITA")
        MessageBox.Show("Extracted TEX")
    End Sub
    Public Function ExtractTEX(ByVal InputFile As String, ByVal System As String)
        'Setup Directory
        Dim TEXName = Path.GetFileNameWithoutExtension(InputFile)
        Dim ExportTEX = "Export\" & TEXName & ".png"

        If File.Exists(ExportTEX) Then File.Delete(ExportTEX)

        If Directory.Exists("Export") = False Then
            Directory.CreateDirectory("Export")
        End If

        Using br As BinaryReader = New BinaryReader(File.Open(InputFile, FileMode.Open))
            If Encoding.UTF8.GetString(br.ReadBytes(&H7)) = "Texture" = False Then
                MessageBox.Show("Unsupported TEX")
                Exit Function
            End If

            br.BaseStream.Seek(&H14, SeekOrigin.Begin)
            Dim FileSize As UInt32
            Dim PaletteStartPOS As UInt32
            Dim RGBAType As UInt16
            Dim RGBAFlags As UInt16
            Dim ImageFileLength As UInt32
            Dim ImageWidth As UInt32
            Dim ImageHeight As UInt32
            Dim ImageStartPOS As UInt32
            If System = "VITA" Then
                FileSize = br.ReadUInt32
                RGBAType = br.ReadUInt16
                RGBAFlags = br.ReadUInt16
                ImageFileLength = br.ReadUInt32
                ImageWidth = br.ReadUInt32
                ImageHeight = br.ReadUInt32
                ImageStartPOS = br.BaseStream.Position
            ElseIf System = "PC" Then
                FileSize = br.ReadUInt32
                RGBAType = br.ReadUInt16
                RGBAFlags = br.ReadUInt16
                Dim PCU1 = br.ReadUInt32
                ImageFileLength = br.ReadUInt32
                ImageWidth = br.ReadUInt16
                ImageHeight = br.ReadUInt16
                ImageStartPOS = br.BaseStream.Position
            End If

            'Set Type of BPP
            Dim InputPixelFormat As PixelFormat
            If RGBAType = 512 Then '0x200
                'RGBA8 (palletized) r8g8b8a8
                InputPixelFormat = PixelFormat.Format8bppIndexed
            ElseIf RGBAType = 16384 Then
                InputPixelFormat = PixelFormat.Format32bppArgb
            Else
                MessageBox.Show("Found Different RGBA Type " + RGBAType.ToString)
            End If

            'Check if PNG
            Dim CheckByte = br.ReadUInt32
            If CheckByte = 1196314761 Then
                'Get PNG
                br.BaseStream.Seek(ImageStartPOS, SeekOrigin.Begin)
                Dim ImageBytes = br.ReadBytes(ImageFileLength)
                File.WriteAllBytes(ExportTEX, ImageBytes)

            ElseIf CheckByte = 926374476 Then
                'Got LZ77
                Dim lzsszsize = ImageFileLength
                br.BaseStream.Seek(ImageStartPOS, SeekOrigin.Begin)
                Dim lzssdata = br.ReadBytes(ImageFileLength)
                Dim UncompressedDataBytes = DecompressLZSS(lzssdata, lzsszsize)

                'Lets Check for Palletization
                If RGBAType = 512 Then
                    Dim ImageData = New Byte(UncompressedDataBytes.Length - &H400) {}
                    Array.Copy(UncompressedDataBytes, ImageData, UncompressedDataBytes.Length - &H400)
                    Dim PaletteData = New Byte(&H400) {}
                    Array.Copy(UncompressedDataBytes, UncompressedDataBytes.Length - &H400, PaletteData, 0, &H400)
                    Dim ColorPalette = ConvertBytesToPalette(PaletteData)
                    To8bppImage(ImageData, ColorPalette, ImageWidth, ImageHeight, ExportTEX)

                Else
                    DecodeRawImage(UncompressedDataBytes, ImageWidth, ImageHeight, ExportTEX)
                End If

            Else
                MessageBox.Show("Unsupported Image to Extraction")
                Exit Function
            End If
        End Using
    End Function


    'TEX CREATOR
    Private Sub VITAToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VITAToolStripMenuItem.Click
        Dim VITATexFile = ""
        ofdVitaTex.Title = "Select the orginal VITA .TEX you wish to Create"
        If ofdVitaTex.ShowDialog = DialogResult.OK Then
            VITATexFile = ofdVitaTex.FileName
        Else
            Exit Sub
        End If

        Dim PNGFile = ""
        ofdVitaTex.Title = "Select the new PNG you wish to create a TEX from"
        If ofdNewPNGTex.ShowDialog = DialogResult.OK Then
            PNGFile = ofdNewPNGTex.FileName
        Else
            Exit Sub
        End If

        CreateNewTEX_VITA(VITATexFile, PNGFile)
    End Sub
    Private Sub PCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PCToolStripMenuItem.Click
        Dim PCTexFile = ""
        ofdVitaTex.Title = "Select the orginal PC .TEX you wish to Create"
        If ofdVitaTex.ShowDialog = DialogResult.OK Then
            PCTexFile = ofdVitaTex.FileName
        Else
            Exit Sub
        End If

        Dim PNGFile = ""
        ofdVitaTex.Title = "Select the new PNG you wish to create a TEX from"
        If ofdNewPNGTex.ShowDialog = DialogResult.OK Then
            PNGFile = ofdNewPNGTex.FileName
        Else
            Exit Sub
        End If

        CreateNewTEX_PC(PCTexFile, PNGFile)
    End Sub
    Function CreateNewTEX_VITA(VitaTEX As String, InsertPNG As String)
        'Parse Vita TEX Info First
        Dim VITAPaletteStartPOS As UInt32
        Dim VITARGBAType As UInt16
        Dim VITARGBAFlags As UInt16
        Dim VITAImageFileLength As UInt32
        Dim VITAImageWidth As UInt32
        Dim VITAImageHeight As UInt32
        Dim VITAImageStartPOS
        Dim VITAImageBytes As Byte()
        Dim VITAPaletteBytes As Byte()
        Dim VITATempImage As Image
        Using br As BinaryReader = New BinaryReader(File.Open(VitaTEX, FileMode.Open))
            If Encoding.UTF8.GetString(br.ReadBytes(&H7)) = "Texture" = False Then
                MessageBox.Show("Unsupported TEX")
                Exit Function
            End If
            br.BaseStream.Seek(&H14, SeekOrigin.Begin)
            VITAPaletteStartPOS = br.ReadUInt32
            VITARGBAType = br.ReadUInt16
            VITARGBAFlags = br.ReadUInt16
            VITAImageFileLength = br.ReadUInt32
            VITAImageWidth = br.ReadUInt32
            VITAImageHeight = br.ReadUInt32
            VITAImageStartPOS = br.BaseStream.Position

            'Get the PNG Bytes
            'Check if PNG
            Dim CheckByte = br.ReadUInt32
            If CheckByte = 926374476 Then
                'Got LZ77
                Dim lzsszsize = VITAImageFileLength
                br.BaseStream.Seek(VITAImageStartPOS, SeekOrigin.Begin)
                Dim lzssdata = br.ReadBytes(VITAImageFileLength)
                Dim UncompressedDataBytes = DecompressLZSS(lzssdata, lzsszsize)
                DecodeRawImage(UncompressedDataBytes, VITAImageWidth, VITAImageHeight, "VITA_temp.png")
                File.Delete("VITA_temp.png")
            ElseIf CheckByte = 1196314761 Then
                'Get PNG
                br.BaseStream.Seek(VITAImageStartPOS, SeekOrigin.Begin)
                VITAImageBytes = br.ReadBytes(VITAImageFileLength)
                File.WriteAllBytes("VITA_temp.png", VITAImageBytes)
                VITATempImage = Image.FromFile("VITA_temp.png")
                File.Delete("VITA_temp.png")
            Else
                MessageBox.Show("Unsupported Image to Extraction")
                Exit Function
            End If

            'Get Palette
            br.BaseStream.Seek(VITAPaletteStartPOS, SeekOrigin.Begin)
            VITAPaletteBytes = br.ReadBytes(br.BaseStream.Length - VITAPaletteStartPOS)
        End Using

        'Setup DIR/File
        Dim ExportDIR = "Built\TEX\VITA"
        If Directory.Exists(ExportDIR) = False Then
            Directory.CreateDirectory(ExportDIR)
        End If
        Dim ExportNewTEX = ExportDIR & "\" & Path.GetFileName(VitaTEX)
        If File.Exists(ExportNewTEX) = True Then
            File.Delete(ExportNewTEX)
        End If

        'Insert PNG
        Dim InsertPNGBytes = File.ReadAllBytes(InsertPNG)
        Dim BMP = New Bitmap(InsertPNG)
        Dim InsertPNGWidth As UInt32 = BMP.Width
        Dim InsertPNGHeight As UInt32 = BMP.Height
        Dim FileSignature As Byte() = {&H54, &H65, &H78, &H74, &H75, &H72, &H65}
        Dim HeaderFill As Byte() = {&H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20}
        Using BW As BinaryWriter = New BinaryWriter(File.Open(ExportNewTEX, FileMode.Create))
            BW.Write(FileSignature)
            BW.Write(HeaderFill)
            'We will return here to fix Palette POS
            Dim VitaPaletteReturn = BW.BaseStream.Position
            BW.Write(VITAPaletteStartPOS)
            BW.Write(VITARGBAType)
            BW.Write(VITARGBAFlags)
            BW.Write(InsertPNGBytes.Length)
            BW.Write(InsertPNGWidth)
            BW.Write(InsertPNGHeight)
            BW.Write(InsertPNGBytes)
            Dim PalettePOS As UInt32 = BW.BaseStream.Position
            BW.Write(VITAPaletteBytes)
            BW.BaseStream.Seek(VitaPaletteReturn, SeekOrigin.Begin)
            BW.Write(PalettePOS)
        End Using

        MessageBox.Show("Created new VITA TEX Succesfully")

    End Function
    Function CreateNewTEX_PC(PCTEX As String, InsertPNG As String)
        'Parse Vita TEX Info First
        Dim PCPaletteStartPOS As UInt32
        Dim PCRGBAType As UInt16
        Dim PCRGBAFlags As UInt16
        Dim PCU1 As UInt32
        Dim PCImageFileLength As UInt32
        Dim PCImageWidth As UInt16
        Dim PCImageHeight As UInt16
        Dim PCImageStartPOS
        Dim PCImageBytes As Byte()
        Dim PCPaletteBytes As Byte()
        Dim PCTempImage As Image
        Using br As BinaryReader = New BinaryReader(File.Open(PCTEX, FileMode.Open))
            If Encoding.UTF8.GetString(br.ReadBytes(&H7)) = "Texture" = False Then
                MessageBox.Show("Unsupported TEX")
                Exit Function
            End If
            br.BaseStream.Seek(&H14, SeekOrigin.Begin)
            PCPaletteStartPOS = br.ReadUInt32
            PCRGBAType = br.ReadUInt16
            PCRGBAFlags = br.ReadUInt16
            PCU1 = br.ReadUInt32
            PCImageFileLength = br.ReadUInt32
            PCImageWidth = br.ReadUInt16
            PCImageHeight = br.ReadUInt16
            PCImageStartPOS = br.BaseStream.Position

            'Get the PNG Bytes
            'Check if PNG
            Dim CheckByte = br.ReadUInt32
            If CheckByte = 926374476 Then
                'Got LZ77
                Dim lzsszsize = PCImageFileLength
                br.BaseStream.Seek(PCImageStartPOS, SeekOrigin.Begin)
                Dim lzssdata = br.ReadBytes(PCImageFileLength)
                Dim UncompressedDataBytes = DecompressLZSS(lzssdata, lzsszsize)
                DecodeRawImage(UncompressedDataBytes, PCImageWidth, PCImageHeight, "PC_temp.png")
                File.Delete("PC_temp.png")
            ElseIf CheckByte = 1196314761 Then
                'Get PNG
                br.BaseStream.Seek(PCImageStartPOS, SeekOrigin.Begin)
                PCImageBytes = br.ReadBytes(PCImageFileLength)
                File.WriteAllBytes("PC_temp.png", PCImageBytes)
                PCTempImage = Image.FromFile("PC_temp.png")
                File.Delete("PC_temp.png")
            Else
                MessageBox.Show("Unsupported Image to Extraction")
                Exit Function
            End If

            'Get Palette
            br.BaseStream.Seek(PCPaletteStartPOS, SeekOrigin.Begin)
            PCPaletteBytes = br.ReadBytes(br.BaseStream.Length - PCPaletteStartPOS)
        End Using

        'Setup DIR/File
        Dim ExportDIR = "Built\TEX\PC"
        If Directory.Exists(ExportDIR) = False Then
            Directory.CreateDirectory(ExportDIR)
        End If
        Dim ExportNewTEX = ExportDIR & "\" & Path.GetFileName(PCTEX)
        If File.Exists(ExportNewTEX) = True Then
            File.Delete(ExportNewTEX)
        End If

        'Insert PNG
        Dim InsertPNGBytes = File.ReadAllBytes(InsertPNG)
        Dim BMP = New Bitmap(InsertPNG)
        Dim InsertPNGWidth As UInt16 = BMP.Width
        Dim InsertPNGHeight As UInt16 = BMP.Height
        Dim FileSignature As Byte() = {&H54, &H65, &H78, &H74, &H75, &H72, &H65}
        Dim HeaderFill As Byte() = {&H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20}
        Using BW As BinaryWriter = New BinaryWriter(File.Open(ExportNewTEX, FileMode.Create))
            BW.Write(FileSignature)
            BW.Write(HeaderFill)
            'We will return here to fix Palette POS
            Dim VitaPaletteReturn = BW.BaseStream.Position
            BW.Write(PCPaletteStartPOS)
            BW.Write(PCRGBAType)
            BW.Write(PCRGBAFlags)
            BW.Write(PCU1)
            BW.Write(InsertPNGBytes.Length)
            BW.Write(InsertPNGWidth)
            BW.Write(InsertPNGHeight)
            BW.Write(InsertPNGBytes)
            Dim PalettePOS As UInt32 = BW.BaseStream.Position
            BW.Write(PCPaletteBytes)
            BW.BaseStream.Seek(VitaPaletteReturn, SeekOrigin.Begin)
            BW.Write(PalettePOS)
        End Using

        MessageBox.Show("Created new PC TEX Succesfully")
    End Function


    'PC->VITA TEX CONVERSION
    Private Sub PCVitaTEXToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PCVitaTEXToolStripMenuItem.Click
        Dim WorkingVitaTEXFile = ""
        ofdVitaTex.Title = "Please select VITA .tex"
        If ofdVitaTex.ShowDialog = DialogResult.OK Then
            WorkingVitaTEXFile = ofdVitaTex.FileName
        Else
            Exit Sub
        End If

        Dim WorkingPCTEXFile = ""
        ofdPCTex.Title = "Please select SAME PC .tex"
        If ofdPCTex.ShowDialog = DialogResult.OK Then
            WorkingPCTEXFile = ofdPCTex.FileName
        Else
            Exit Sub
        End If

        PC2VitaTexConvertor(WorkingVitaTEXFile, WorkingPCTEXFile)
    End Sub
    Public Function PC2VitaTexConvertor(ByVal VitaTEX, ByVal PCTEX)
        Dim ExportFileTEX = "Export\" & Path.GetFileName(VitaTEX)

        'Get PC Tex Info
        Dim PCPaletteStartPOS As UInt32
        Dim PCRGBAType As UInt16
        Dim PCRGBAFlags As UInt16
        Dim PCU1 As UInt32
        Dim PCImageFileLength As UInt32
        Dim PCImageWidth As UInt16
        Dim PCImageHeight As UInt16
        Dim PCImageStartPOS
        Dim PCImageBytes As Byte()
        Dim PCTempImage As Image
        Using br As BinaryReader = New BinaryReader(File.Open(PCTEX, FileMode.Open))
            If Encoding.UTF8.GetString(br.ReadBytes(&H7)) = "Texture" = False Then
                MessageBox.Show("Unsupported TEX")
                Exit Function
            End If
            br.BaseStream.Seek(&H14, SeekOrigin.Begin)
            PCPaletteStartPOS = br.ReadUInt32
            PCRGBAType = br.ReadUInt16
            PCRGBAFlags = br.ReadUInt16
            PCU1 = br.ReadUInt32
            PCImageFileLength = br.ReadUInt32
            PCImageWidth = br.ReadUInt16
            PCImageHeight = br.ReadUInt16
            PCImageStartPOS = br.BaseStream.Position

            Dim CheckByte = br.ReadUInt32
            If CheckByte = 926374476 Then
                'Got LZ77
                Dim lzsszsize = PCImageFileLength
                br.BaseStream.Seek(PCImageStartPOS, SeekOrigin.Begin)
                Dim lzssdata = br.ReadBytes(PCImageFileLength)
                Dim UncompressedDataBytes = DecompressLZSS(lzssdata, lzsszsize)
                DecodeRawImage(UncompressedDataBytes, PCImageWidth, PCImageHeight, "PC_temp.png")
                PCTempImage = Image.FromFile("PC_temp.png")
            ElseIf CheckByte = 1196314761 Then
                'Got PNG
                br.BaseStream.Seek(PCImageStartPOS, SeekOrigin.Begin)
                PCImageBytes = br.ReadBytes(PCImageFileLength)
                File.WriteAllBytes("PC_temp.png", PCImageBytes)
                PCTempImage = Image.FromFile("PC_temp.png")
            Else
                MessageBox.Show("PC - Unsupported Image to Extraction")
                Exit Function
            End If
        End Using

        'Get Vita Tex Info
        Dim VITAPaletteStartPOS As UInt32
        Dim VITARGBAType As UInt16
        Dim VITARGBAFlags As UInt16
        Dim VITAImageFileLength As UInt32
        Dim VITAImageWidth As UInt32
        Dim VITAImageHeight As UInt32
        Dim VITAImageStartPOS
        Dim VITAImageBytes As Byte()
        Dim VITAPaletteBytes As Byte()
        Dim VITATempImage As Image
        Using br As BinaryReader = New BinaryReader(File.Open(VitaTEX, FileMode.Open))
            If Encoding.UTF8.GetString(br.ReadBytes(&H7)) = "Texture" = False Then
                MessageBox.Show("Unsupported TEX")
                Exit Function
            End If
            br.BaseStream.Seek(&H14, SeekOrigin.Begin)
            VITAPaletteStartPOS = br.ReadUInt32
            VITARGBAType = br.ReadUInt16
            VITARGBAFlags = br.ReadUInt16
            VITAImageFileLength = br.ReadUInt32
            VITAImageWidth = br.ReadUInt32
            VITAImageHeight = br.ReadUInt32
            VITAImageStartPOS = br.BaseStream.Position

            'Get the PNG Bytes
            'Check if PNG
            Dim CheckByte = br.ReadUInt32
            If CheckByte = 926374476 Then
                'Got LZ77
                Dim lzsszsize = VITAImageFileLength
                br.BaseStream.Seek(VITAImageStartPOS, SeekOrigin.Begin)
                Dim lzssdata = br.ReadBytes(VITAImageFileLength)
                Dim UncompressedDataBytes = DecompressLZSS(lzssdata, lzsszsize)
                DecodeRawImage(UncompressedDataBytes, VITAImageWidth, VITAImageHeight, "VITA_temp.png")
                PCTempImage = Image.FromFile("VITA_temp.png")
            ElseIf CheckByte = 1196314761 Then
                'Get PNG
                br.BaseStream.Seek(VITAImageStartPOS, SeekOrigin.Begin)
                VITAImageBytes = br.ReadBytes(VITAImageFileLength)
                File.WriteAllBytes("VITA_temp.png", VITAImageBytes)
                VITATempImage = Image.FromFile("VITA_temp.png")
            Else
                MessageBox.Show("VITA - Unsupported Image to Extraction")
                Exit Function
            End If

            'Get Palette
            br.BaseStream.Seek(VITAPaletteStartPOS, SeekOrigin.Begin)
            VITAPaletteBytes = br.ReadBytes(br.BaseStream.Length - VITAPaletteStartPOS)
        End Using

        'Resize PC Image to Match Vita
        If PCImageWidth <> VITAImageWidth Then
            Dim ResizedImage = ResizeImage(PCTempImage, VITAImageWidth, VITAImageHeight)
            ResizedImage.Save("ResizedImage.png")
        ElseIf PCImageHeight <> VITAImageHeight Then
            Dim ResizedImage = ResizeImage(PCTempImage, VITAImageWidth, VITAImageHeight)
            ResizedImage.Save("ResizedImage.png")
        End If

        Dim ResizedImagesBytes = File.ReadAllBytes("ResizedImage.png")
        'Time to Convert PC -> Vita
        If File.Exists(ExportFileTEX) Then File.Delete(ExportFileTEX)
        Dim TexHeader As Byte() = {&H54, &H65, &H78, &H74, &H75, &H72, &H65, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20, &H20}
        Dim WriteInt32Zeros As Byte() = {&H0, &H0, &H0, &H0}
        Using bw As BinaryWriter = New BinaryWriter(File.Open(ExportFileTEX, FileMode.Create))
            bw.Write(TexHeader)
            bw.Write(WriteInt32Zeros) ' This is the Start of Palette, We will write it later
            bw.Write(VITARGBAType)
            bw.Write(VITARGBAFlags)
            bw.Write(ResizedImagesBytes.Length)
            bw.Write(VITAImageWidth)
            bw.Write(VITAImageHeight)
            bw.Write(ResizedImagesBytes)
            Dim NewPalettePOS = bw.BaseStream.Position
            bw.Write(VITAPaletteBytes)
            bw.BaseStream.Seek(&H14, SeekOrigin.Begin)
            bw.Write(NewPalettePOS)
        End Using
    End Function


    'Functions
    Function ResizeImage(inputImage As Image, width As Integer, height As Integer) As Image
        Dim newImage As New Bitmap(width, height)

        Using graphics As Graphics = Graphics.FromImage(newImage)
            graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
            graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality

            graphics.DrawImage(inputImage, 0, 0, width, height)
        End Using

        Return newImage
    End Function
    Function DecompressLZSS(data As Byte(), zsize As Integer) As Byte()
        Dim dataStream As New BinaryReader(New MemoryStream(data))

        Dim magic As String = Encoding.ASCII.GetString(dataStream.ReadBytes(4))
        If magic <> "LZ77" Then
            Return Nothing
        End If

        dataStream.BaseStream.Position = 4
        Dim size As UInt32 = dataStream.ReadUInt32()
        Console.WriteLine("[i] Compressed size: " & zsize & " B. Decompressed size: " & size & " B. Ratio: " & Math.Round(zsize / size * 100, 2) & "%.")

        Dim lzssOpCount As UInt32 = dataStream.ReadUInt32()
        Dim lzssDataOffset As UInt32 = dataStream.ReadUInt32()
        Dim lzssFlagsLength As Integer = CInt(lzssDataOffset) - 16

        Dim flagsData As Byte() = dataStream.ReadBytes(lzssFlagsLength)
        Dim flagsStream As New BinaryReader(New MemoryStream(flagsData))
        Dim srcData As Byte() = dataStream.ReadBytes(zsize - CInt(lzssDataOffset))
        Dim srcStream As New BinaryReader(New MemoryStream(srcData))

        Dim dst(size - 1) As Byte
        Dim flagBufferBitCount As Integer = 0
        Dim dstPos As Integer = 0
        Dim FLAGS As Byte = 0

        For i As Integer = 0 To CInt(lzssOpCount) - 1
            If flagBufferBitCount = 0 Then
                FLAGS = flagsStream.ReadByte()
                flagBufferBitCount = 8
            End If

            If (FLAGS And &H80) <> 0 Then
                Dim b As Byte = srcStream.ReadByte()
                Dim c As Integer = srcStream.ReadByte() + 3
                Array.Copy(dst, dstPos - b, dst, dstPos, c)
                dstPos += c
            Else
                dst(dstPos) = srcStream.ReadByte()
                dstPos += 1
            End If

            FLAGS <<= 1
            flagBufferBitCount -= 1
        Next

        'Using fs As New FileStream("rawbytesdecomp.txt", FileMode.Create)
        'fs.Write(dst, 0, dst.Length)
        'End Using

        Return dst
    End Function
    Function DecodeRawImage(ImageData As Byte(), width As UInt16, height As UInt16, exportimage As String)
        Using stream = New MemoryStream(ImageData)
            Using bmp = New Bitmap(width, height, PixelFormat.Format32bppArgb)
                Dim bmpData As BitmapData = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.[WriteOnly], bmp.PixelFormat)
                Dim pNative As IntPtr = bmpData.Scan0
                Marshal.Copy(ImageData, 0, pNative, ImageData.Length)
                bmp.UnlockBits(bmpData)
                bmp.Save(exportimage)
            End Using
        End Using
    End Function
    Public Shared Function ConvertBytesToPalette(bytes As Byte()) As ColorPalette
        ' Assuming 8bpp RGBA format, each color has 4 bytes
        Dim numColors As Integer = bytes.Length \ 4
        Dim paletteSize As Integer = numColors

        ' Ensure palette size is within bounds (maximum 256 colors)
        If paletteSize > 256 Then
            paletteSize = 256
        End If

        Dim palette As ColorPalette = New Bitmap(1, 1, PixelFormat.Format8bppIndexed).Palette
        For i As Integer = 0 To paletteSize - 1
            Dim r As Byte = bytes(i * 4 + 0)
            Dim g As Byte = bytes(i * 4 + 1)
            Dim b As Byte = bytes(i * 4 + 2)
            Dim a As Byte = bytes(i * 4 + 3)

            palette.Entries(i) = Color.FromArgb(a, r, g, b)
        Next

        Return palette
    End Function
    Function To8bppImage(ImageData As Byte(), PaletteData As ColorPalette, width As UInt16, height As UInt16, exportimage As String)
        Using bmp = New Bitmap(width, height, PixelFormat.Format8bppIndexed)
            Dim bmpData As BitmapData = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.[WriteOnly], bmp.PixelFormat)

            'Create color table
            bmp.Palette = PaletteData

            Dim pNative As IntPtr = bmpData.Scan0
            Marshal.Copy(ImageData, 0, pNative, ImageData.Length)
            bmp.UnlockBits(bmpData)
            bmp.Save(exportimage, ImageFormat.Png)
        End Using
    End Function

    'Exit buttons
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        MessageBox.Show("Dungeon Travelers 2-2 Toolkit Created by @Yuviapp" & vbNewLine & "Combination of tools that will help you extract and created PCK, and TEX files.")
    End Sub
    Private Sub ExitToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem1.Click
        Application.Exit()
    End Sub

    'Extra
    Private Sub RawBytesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RawBytesToolStripMenuItem.Click
        ImageTest.Show()
    End Sub

End Class
