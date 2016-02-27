Imports System.IO
Imports System
Imports System.Diagnostics

Public Class Form1
    Public text1 As String = vbNullChar

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button3.Enabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            Button3.Enabled = False
            TextBox2.Text = " "
            compile_status.Visible = False
            build_label.Visible = False
            text1 = OpenFileDialog1.FileName.ToString()
            TextBox1.Text = OpenFileDialog1.FileName.ToString()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim sb As New System.Text.StringBuilder
        Dim fileName As String = text1
        Dim result As String
        Dim directoryName As String
        Dim segment As String = TextBox3.Text
        Dim offset As String = TextBox4.Text
        result = Path.GetFileNameWithoutExtension(fileName)
        directoryName = Path.GetDirectoryName(fileName)

        Dim myProcess As New Process()
        Dim myProcessStartInfo As New ProcessStartInfo("cmd.exe", "/C " + "c:\fasm\bin2hex """ + directoryName + "\" + result + ".bin"" """ + directoryName + "\" + result + ".hex"" -o " + offset + " -s " + segment)
        myProcessStartInfo.UseShellExecute = False
        myProcessStartInfo.RedirectStandardOutput = True
        myProcessStartInfo.RedirectStandardError = True
        myProcess.StartInfo = myProcessStartInfo
        myProcess.Start()
        Dim myStreamReader As StreamReader = myProcess.StandardOutput
        Dim myStreamError As StreamReader = myProcess.StandardError
        TextBox2.AppendText(Environment.NewLine)
        TextBox2.AppendText(Environment.NewLine)
        TextBox2.Text &= "HEX File Generation :: "
        TextBox2.Text &= Environment.NewLine
        TextBox2.Text &= DateValue(Now)
        TextBox2.Text &= "      "
        TextBox2.Text &= TimeValue(Now)

        If myStreamError.Peek <= 0 Then
            build_label.Visible = True
            build_label.ForeColor = Color.White
            build_label.Text = "HEX file Convertion Successful.... :)"
            build_label.BackColor = Color.Green
            TextBox2.Text &= Environment.NewLine
            TextBox2.Text &= "HEX File Generated"
        Else
            build_label.Visible = True
            build_label.ForeColor = Color.White
            build_label.Text = "Error in Generating HEX File... :("
            build_label.BackColor = Color.Red


            Do Until myStreamError.EndOfStream = True
                TextBox2.AppendText(Environment.NewLine)
                TextBox2.Text &= myStreamError.ReadLine
            Loop
        End If
        myProcess.WaitForExit()
        myProcess.Close()

    End Sub

    Private Sub compile_Click(sender As Object, e As EventArgs) Handles compile.Click
        Dim compile_process As New Process()
        build_label.Visible = False
        If text1 = vbNullChar Then
            MessageBox.Show("ASM Source file is not selected or Path is Invalid." + Environment.NewLine + "Please Check ASM Source File Path")
        Else
            Dim compileProcessStartInfo As New ProcessStartInfo("cmd.exe", "/C " + " c:\fasm\FASM.exe """ + text1 + "")
            compileProcessStartInfo.UseShellExecute = False
            compileProcessStartInfo.RedirectStandardOutput = True
            compileProcessStartInfo.RedirectStandardError = True
            compile_process.StartInfo = compileProcessStartInfo
            compile_process.Start()
            Dim myStreamReader As StreamReader = compile_process.StandardOutput
            Dim myStreamError As StreamReader = compile_process.StandardError
            TextBox2.Text = "Compilation Results ::"
            TextBox2.Text &= Environment.NewLine
            TextBox2.Text &= DateValue(Now)
            TextBox2.Text &= "      "
            TextBox2.Text &= TimeValue(Now)
            Dim lineNo As String
            Dim lineNoTemp As String

            If myStreamError.Peek <= 0 Then
                Button3.Enabled = True
                compile_status.Visible = True
                compile_status.ForeColor = Color.White
                compile_status.Text = "Successfully Compiled... :) "
                compile_status.BackColor = Color.Green
                TextBox2.Text &= Environment.NewLine
                TextBox2.Text &= "Compile Successful.."
            Else
                Button3.Enabled = False
                compile_status.Visible = True
                compile_status.ForeColor = Color.White
                compile_status.Text = "Error in the Code ... :( "
                compile_status.BackColor = Color.Red
                Do Until myStreamError.EndOfStream = True
                    TextBox2.AppendText(Environment.NewLine)
                    lineNo = vbNullChar
                    lineNoTemp = myStreamError.ReadLine
                    Try
                        lineNo = Mid(lineNoTemp, (InStr(1, lineNoTemp, text1) + text1.Length + 2), (InStr(1, lineNoTemp, "]") - (InStr(1, lineNoTemp, text1) + text1.Length + 2)))
                    Catch ex As Exception
                    End Try
                    If lineNo = vbNullChar Then
                        TextBox2.Text &= lineNoTemp
                    Else
                        'TextBox2.Font = New System.Drawing.Font(TextBox2.Font, FontStyle.Bold)
                        TextBox2.Text &= "@Line No: "
                        TextBox2.Text &= lineNo
                    End If
                Loop
                compile_process.WaitForExit()
                compile_process.Close()
            End If
        End If
    End Sub

End Class

