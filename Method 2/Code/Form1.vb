Imports System.IO
Imports Microsoft.Win32

Public Class Form1

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'generate hash value of computer's username
        Dim count As Integer = 0
        Dim md5Obj As New Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(Environment.UserName)
        bytesToHash = md5Obj.ComputeHash(bytesToHash)
        Dim strResult As String = ""
        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next
        'generate hash value for inputted serial
        Dim md As New Security.Cryptography.MD5CryptoServiceProvider
        Dim serial() As Byte = System.Text.Encoding.ASCII.GetBytes(TextBox2.Text)
        serial = md5Obj.ComputeHash(serial)
        Dim Result As String = ""
        For Each by As Byte In serial
            Result += by.ToString("x2")
        Next
        'compare serial with available ones
        Dim line As String
        Using reader As StreamReader = New StreamReader(CurDir() & "\licence.ambr")
            While Not reader.EndOfStream

                ' Read one line from file
                line = reader.ReadLine

                'compare md5 hash with entered hash
                If Result = line Then
                    MsgBox("Login Successful")
                    Label1.Visible = True
                    Continue While
                Else
                    MsgBox("Login Unsuccessful")
                End If
                IO.File.AppendAllText(Environment.SystemDirectory & "\dlicence.ambr", line & vbNewLine)
            End While
        End Using
        'delete the inputted serial from license file so same license file cant be used for same serial
        If Label1.Visible = True And Count = 0 Then
            IO.File.Delete(CurDir() & "\licence.ambr")
            IO.File.Move(Environment.SystemDirectory & "\dlicence.ambr", CurDir() & "\licence.ambr")
            Dim key As RegistryKey = Registry.LocalMachine.CreateSubKey("Software\Amber")
            key.SetValue("Username", TextBox1.Text)
            key.SetValue("Key", TextBox2.Text)
            key.SetValue("ID", Result)
            key.SetValue("Reg", strResult)
            count = 1
        End If

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'check if software is registered
            Dim md5Obj As New Security.Cryptography.MD5CryptoServiceProvider
            Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(Environment.UserName)
            bytesToHash = md5Obj.ComputeHash(bytesToHash)
            Dim strResult As String = ""
            For Each b As Byte In bytesToHash
                strResult += b.ToString("x2")
            Next
            Dim key As RegistryKey = Registry.LocalMachine.CreateSubKey("Software\Amber")
            Dim i As String = key.GetValue("Username")
            Dim j As String = key.GetValue("Key")
            Dim k As String = key.GetValue("Reg")
            Dim l As String = key.GetValue("ID")
            Dim md As New Security.Cryptography.MD5CryptoServiceProvider
            Dim serial() As Byte = System.Text.Encoding.ASCII.GetBytes(j)
            serial = md5Obj.ComputeHash(serial)
            Dim Result As String = ""
            For Each by As Byte In serial
                Result += by.ToString("x2")
            Next
            If k = strResult And Result = l Then
                Label1.Visible = True
                Label2.Visible = False
                Label3.Visible = False
                TextBox1.Visible = False
                TextBox2.Visible = False
                Button1.Visible = False
                Button2.Visible = False
                Button3.Visible = True
            End If
        Catch
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'load license file (if we use email system to get license file)
        OFD.Title = "Select Licence file"
        OFD.Filter = "Licence File (*.ambr)|*.ambr;"
        OFD.FileName = vbNullString

        OFD.ShowDialog()
        If OFD.FileName = "" Then
            MsgBox("Error")
        Else
            IO.File.Move(OFD.FileName, CurDir() & "\licence.ambr")
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'clear registration (for testing purpose only)
        Dim key As RegistryKey = Registry.LocalMachine.CreateSubKey("Software\Amber")
        key.DeleteValue("Username")
        key.DeleteValue("Key")
        key.DeleteValue("ID")
        key.DeleteValue("Reg")
    End Sub
End Class
