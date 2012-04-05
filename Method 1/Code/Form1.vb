Imports Microsoft.Win32
Public Class Form1
    'MD5 Hash is one way hashing algorithm, given a checksum value, it is infeasible to discover the password. Bruteforce takes ages
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'compute md5 hash
        Dim md5Obj As New Security.Cryptography.MD5CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(TextBox1.Text)
        bytesToHash = md5Obj.ComputeHash(bytesToHash)
        Dim strResult As String = ""
        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next
        'compare md5 hash with entered hash
        If TextBox2.Text = strResult Then
            MsgBox("Login Successful")
        Else
            MsgBox("Login Unsuccessful")
        End If
        'Write to registry
        Dim key As RegistryKey = Registry.LocalMachine.CreateSubKey("Software\Amber")
        key.SetValue("Username", TextBox1.Text)
        key.SetValue("Key", TextBox2.Text)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'check if software is registered
        Try
            Dim key As RegistryKey = Registry.LocalMachine.CreateSubKey("Software\Amber")
            Dim i As String = key.GetValue("Username")
            Dim j As String = key.GetValue("Key")
            Dim md As New Security.Cryptography.MD5CryptoServiceProvider
            Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(i)
            bytesToHash = md.ComputeHash(bytesToHash)
            Dim strResult As String = ""
            For Each b As Byte In bytesToHash
                strResult += b.ToString("x2")
            Next
            'if registered display welcome screen instead of login
            If j = strResult Then
                Label1.Text = "Registered"
                Label2.Visible = False
                Button1.Visible = False
                TextBox1.Visible = False
                TextBox2.Visible = False
                Button2.Visible = True
            End If
        Catch
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        'clear password for testing purpose only
        Dim key As RegistryKey = Registry.LocalMachine.CreateSubKey("Software\Amber")
        key.DeleteValue("Username")
        key.DeleteValue("Key")
        Label1.Text = "Username"
        Label2.Visible = True
        Button1.Visible = True
        TextBox1.Visible = True
        TextBox2.Visible = True
        Button2.Visible = False
    End Sub
End Class
