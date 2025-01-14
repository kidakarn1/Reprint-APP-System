﻿Public Class login_form
    Public Str
    Public id As Integer
    Public id_user As Integer

    Private Sub login_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PictureBox3.Visible = False
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Try
            Str = ""
            Str = User_id.Text
            Dim back_office As New Model
            If back_office.check_user(Trim(Str)) <> 0 And back_office.check_emp_cd(Trim(Str)) = 1 Then
                If back_office.check_status_perm() = 0 Then
                    '  MsgBox("teng")
                    PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_permission.png")
                    User_id.Text = Nothing
                    PictureBox3.Visible = True
                    PictureBox3.Focus()
                    Exit Sub
                ElseIf back_office.check_status_menu(Trim(Str)) = 0 Then
                    PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_permission.png")
                    User_id.Text = Nothing
                    PictureBox3.Visible = True
                    PictureBox3.Focus()
                    Exit Sub
                Else
                    menu_form.Show()
                    User_id.Text = Nothing
                    Me.Visible = False
                End If
            ElseIf back_office.check_user(Trim(Str)) <> 0 And back_office.check_emp_cd(Trim(Str)) = 0 Then
                PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/not_have_permission.png")
                User_id.Text = Nothing
                PictureBox3.Visible = True
                PictureBox3.Focus()

                Exit Sub
            Else
                PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/login_alert.png")
                User_id.Text = Nothing
                PictureBox3.Visible = True
                PictureBox3.Focus()
            End If
            User_id.Text = Nothing
        Catch ex As Exception
            ' MsgBox(ex.Message())
            PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/login_alert.png")
            User_id.Text = Nothing
            PictureBox3.Visible = True
            PictureBox3.Focus()
        End Try
    End Sub
    Private Sub User_id_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles User_id.KeyDown
        Try
            Select Case e.KeyCode
                Case Keys.Enter
                    Str = ""
                    Str = User_id.Text
                    Dim back_office As New Model
                    If back_office.check_user(Trim(Str)) <> 0 And back_office.check_emp_cd(Trim(Str)) = 1 Then
                        If back_office.check_status_perm() = 0 Then
                            '  MsgBox("teng")
                            PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_permission.png")
                            User_id.Text = Nothing
                            PictureBox3.Visible = True
                            PictureBox3.Focus()
                            Exit Sub
                        ElseIf back_office.check_status_menu(Trim(Str)) = 0 Then
                            PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_permission.png")
                            User_id.Text = Nothing
                            PictureBox3.Visible = True
                            PictureBox3.Focus()
                            Exit Sub
                        Else
                            menu_form.Show()
                            User_id.Text = Nothing
                            Me.Visible = False
                        End If
                    ElseIf back_office.check_user(Trim(Str)) <> 0 And back_office.check_emp_cd(Trim(Str)) = 0 Then
                        PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/not_have_permission.png")
                        User_id.Text = Nothing
                        PictureBox3.Visible = True
                        PictureBox3.Focus()

                        Exit Sub
                    Else
                        PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/login_alert.png")
                        User_id.Text = Nothing
                        PictureBox3.Visible = True
                        PictureBox3.Focus()
                    End If
                    User_id.Text = Nothing
            End Select
        Catch ex As Exception
            MsgBox(ex.Message())
            PictureBox3.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/login_alert.png")
            User_id.Text = Nothing
            PictureBox3.Visible = True
            PictureBox3.Focus()
        End Try
    End Sub
    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        PictureBox3.Visible = False
        User_id.Text = ""
    End Sub
    Private Sub Pb3_focus(sender As Object, e As EventArgs) Handles PictureBox3.KeyDown
        PictureBox3.Visible = False
        User_id.Text = ""
    End Sub
    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Application.Exit()
    End Sub
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        PictureBox5.Visible = True
        select_db.Visible = True
    End Sub
    Private Sub select_db_SelectedIndexChanged(sender As Object, e As EventArgs) Handles select_db.SelectedIndexChanged
        select_db.Visible = False
        PictureBox5.Visible = False
    End Sub
End Class

