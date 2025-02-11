Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Public Class menu_form
    Public id_user As Integer
    Public user As String
    Dim button As Button
    Dim back_office As New Model
    Dim reader As SqlDataReader
    Dim select_menu As Integer
    Private Sub Label1_ParentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.ParentChanged
        Label1.Text = login_form.Str.ToString()
        '  Dim current_time = Date.Today
        DateTimePicker1.Value = Date.Today
        ' MsgBox("user : " + Label1.Text)
    End Sub
    Private Sub menu_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        year_data.Items.Clear()
        Dim current_time = Date.Today.Year
        Dim start_year As Integer = 2021
        Do While current_time >= start_year
            ' MsgBox(current_time)
            year_data.Items.Add(current_time)
            current_time -= 1
        Loop
    End Sub
    Public Sub New()
        InitializeComponent()
        Try
            Dim Model As New Model()
            Dim rs = Model.GET_DATA_USER(Trim(Label1.Text))
            If rs <> "0" Then
                Dim result_data_json As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
                For Each item As Object In result_data_json
                    Dim path As String = item("picture_path").ToString() & item("picture_name").ToString()
                    Dim teng As String = item("picture_name").ToString()
                    Dim filename As String = System.IO.Path.Combine(path)
                    Dim newbutton As New Button
                    newbutton.Name = teng
                    newbutton.Image = Api.DownloadImage(path)
                    TableLayoutPanel1.Controls.Add(newbutton)
                    newbutton.Tag = newbutton.Name
                    button = newbutton
                    newbutton.Size = New Size(110, 100)
                    newbutton.TabStop = False
                    newbutton.FlatStyle = FlatStyle.Flat
                    newbutton.FlatAppearance.BorderSize = 0
                    newbutton.FlatAppearance.BorderColor = Color.White
                    newbutton.FlatAppearance.CheckedBackColor = Color.White
                    newbutton.FlatAppearance.MouseDownBackColor = Color.White
                    newbutton.FlatAppearance.MouseOverBackColor = Color.White
                    AddHandler newbutton.Click, AddressOf Buttons_Click
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
    End Sub
    Private Sub Buttons_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim c As Control = DirectCast(sender, Control)
        If c.Tag = "reprint.png" Then
            scan_tag.Show()
            Me.Close()
        ElseIf c.Tag = "log_reprint.png" Then
            log_reprint()
        ElseIf c.Tag = "newfa_reprint.png" Then
            select_menu = 1
            new_fareprint()
        ElseIf c.Tag = "m83_reprint.png" Then
            _m83_reprint()
        ElseIf c.Tag = "dummy_tag.png" Then
            select_menu = 2
            dummy_tag()
        ElseIf c.Tag = "newfa_reprintPD4.png" Then
            select_menu = 3
            new_fareprint()
        End If
    End Sub
    Public Sub pd_line()
        Dim rs = back_office.getpd()
        If rs <> "0" Then
            Dim result_data_json As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
            For Each item As Object In result_data_json
                pd.Text = item("dep_cd").ToString()
                If pd_data.Items.Contains(pd.Text) Then
                Else
                    pd_data.Items.Add(pd.Text)
                End If
            Next
        End If

    End Sub
    Public Sub pd_dummy()
        Dim rs = back_office.getpd()
        If rs <> "0" Then
            Dim result_data_json As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
            For Each item As Object In result_data_json
                pd.Text = item("dep_cd").ToString()
                If pd_data.Items.Contains(pd.Text) Then
                Else
                    pd_data.Items.Add(pd.Text)
                End If
            Next
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        login_form.Visible = True
        Me.Close()
    End Sub
    Private Sub month_data_MouseDown(sender As Object, e As MouseEventArgs) Handles month_data.MouseDown
        If year_data.Text = "" Then
            alert.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/production_year.png")
            alert.Visible = True
            alert.BringToFront()
            alert.Focus()
            year_data.Focus()
        End If
    End Sub
    Private Sub part_no_data_MouseDown(sender As Object, e As MouseEventArgs) Handles part_no_data.MouseDown
        readonly_false()
    End Sub
    Private Sub part_no_data_MouseLeave(sender As Object, e As EventArgs) Handles part_no_data.MouseLeave
        If part_no_data.Text <> "" Then
            If lot_no_data.Text <> "" Then
                Button1.Visible = True
            End If
        End If
        Button1.BringToFront()
    End Sub
    Private Sub lot_no_data_MouseLeave(sender As Object, e As EventArgs) Handles lot_no_data.MouseLeave
        If part_no_data.Text <> "" Then
            If lot_no_data.Text <> "" Then
                Button1.Visible = True
            End If
        End If
        Button1.BringToFront()
    End Sub
    Public Sub readonly_false()
        If pd_data.Text <> "" Then
            If line_data.Text <> "" Then
                If year_data.Text <> "" Then
                    If month_data.Text <> "" Then
                        part_no_data.ReadOnly = False
                        lot_no_data.ReadOnly = False
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '   back_office.get_data_tag_log_reprint()
        If pd_data.Text.Length = 0 Then
            alert.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/production_department.png")
            alert.Visible = True
            alert.BringToFront()
            alert.Focus()
        ElseIf line_data.Text.Length = 0 Then
            alert.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/line_pro.png")
            alert.Visible = True
            alert.BringToFront()
            alert.Focus()
        ElseIf year_data.Text.Length = 0 Then
            alert.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/production_year.png")
            alert.Visible = True
            alert.BringToFront()
            alert.Focus()
        ElseIf month_data.Text.Length = 0 Then
            alert.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/production_month.png")
            alert.Visible = True
            alert.BringToFront()
            alert.Focus()
        Else
            log_reprint_form.Show()
            Me.Close()
            ' reader.Close()
        End If
    End Sub
    Private Sub pd_data_SelectedIndexChanged(sender As Object, e As EventArgs) Handles pd_data.SelectedIndexChanged
        asterisk_1.Visible = False
        line_data.Items.Clear()
        Dim rs = back_office.getline(pd_data.Text)
        If rs <> "0" Then
            Dim result_data_json As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
            For Each item As Object In result_data_json
                line.Text = item("line_cd").ToString()
                If pd_data.Items.Contains(line.Text) Then
                Else
                    line_data.Items.Add(line.Text)
                End If
            Next
        End If
    End Sub
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        fillter_data.Visible = False
        pd_data.Visible = False
        line_data.Visible = False
        year_data.Visible = False
        month_data.Visible = False
        part_no_data.Visible = False
        lot_no_data.Visible = False
        PictureBox2.Visible = False
        asterisk_1.Visible = False
        asterisk_2.Visible = False
        asterisk_3.Visible = False
        asterisk_4.Visible = False
        PictureBox3.Visible = False
        Button3.Visible = False
        Button2.Visible = False
        wi_data.Visible = False
        DateTimePicker1.ResetText()
        DateTimePicker1.Visible = False
        Button1.Visible = False
        pd_data.Text = Nothing
        line_data.Text = Nothing
        year_data.Text = Nothing
        wi_data.Text = Nothing
        month_data.Text = Nothing
        part_no_data.Text = Nothing
        lot_no_data.Text = Nothing
    End Sub
    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        PictureBox2.Visible = True
        PictureBox2.BringToFront()
    End Sub
    Private Sub alert_Click(sender As Object, e As EventArgs) Handles alert.Click
        alert.Visible = False
        If pd_data.Text.Length = 0 Then
            pd_data.Focus()
        ElseIf line_data.Text.Length = 0 Then
            line_data.Focus()
        ElseIf year_data.Text.Length = 0 Then
            year_data.Focus()
        ElseIf month_data.Text.Length = 0 Then
            month_data.Focus()
        End If
    End Sub
    Private Sub alert_focus(sender As Object, e As KeyEventArgs) Handles alert.KeyDown
        alert.Visible = False
        If pd_data.Text.Length = 0 Then
            pd_data.Focus()
        ElseIf line_data.Text.Length = 0 Then
            line_data.Focus()
        ElseIf year_data.Text.Length = 0 Then
            year_data.Focus()
        ElseIf month_data.Text.Length = 0 Then
            month_data.Focus()
        End If
    End Sub
    Private Sub line_data_SelectedIndexChanged(sender As Object, e As EventArgs) Handles line_data.SelectedIndexChanged
        asterisk_2.Visible = False

    End Sub
    Private Sub year_data_SelectedIndexChanged(sender As Object, e As EventArgs) Handles year_data.SelectedIndexChanged
        asterisk_3.Visible = False
    End Sub
    Private Sub month_data_SelectedIndexChanged(sender As Object, e As EventArgs) Handles month_data.SelectedIndexChanged
        asterisk_4.Visible = False
    End Sub
    Private Sub lot_no_data_KeyPress(sender As Object, e As KeyPressEventArgs) Handles lot_no_data.KeyPress
        lot_no_data.MaxLength = 4
    End Sub
    Private Sub wi_data_KeyPress(sender As Object, e As KeyPressEventArgs) Handles wi_data.KeyPress
        wi_data.MaxLength = 10
        Dim ch As Char = e.KeyChar
        If Not Char.IsDigit(ch) AndAlso Asc(ch) <> 8 Then
            e.Handled = True
        End If
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If pd_data.Text.Length = 0 Then
            ' MsgBox("BBB")
            alert.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/production_department.png")
            alert.Visible = True
            alert.BringToFront()
            alert.Focus()
        ElseIf line_data.Text.Length = 0 Then
            'MsgBox("CCC")
            alert.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/line_pro.png")
            alert.Visible = True
            alert.BringToFront()
            alert.Focus()
        Else
            'MsgBox("AAA")
            If select_menu = 1 Then
                newfa_reprint_form.Show()
                Me.Close()
            ElseIf select_menu = 2 Then
                dummy_tag_form.Show()
                Me.Close()
            ElseIf select_menu = 3 Then
                newfa_reprint_formPD4.Show()
                Me.Hide()
            End If
        End If
    End Sub
    Public Sub new_fareprint()
        fillter_data.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/filter_newfa.png")
        pd_data.BackColor = System.Drawing.Color.White
        line_data.BackColor = System.Drawing.Color.White
        lot_no_data.ReadOnly = False
        wi_data.Visible = True
        wi_data.Top = 426
        wi_data.Left = 313
        fillter_data.Top = 136
        PictureBox2.Top = 145
        PictureBox3.Top = 156
        lot_no_data.Left = 417
        DateTimePicker1.Location = New System.Drawing.Point(211, 339)
        lot_no_data.Top = 341
        lot_no_data.Visible = True
        fillter_data.Visible = True
        pd_data.Visible = True
        line_data.Visible = True
        PictureBox2.Visible = False
        Button3.Visible = True
        DateTimePicker1.Visible = True
        PictureBox3.Visible = True
        asterisk_1.Visible = True
        asterisk_2.Visible = True
        lot_no_data.BringToFront()
        Button3.BringToFront()
        DateTimePicker1.BringToFront()
        PictureBox3.BringToFront()
        asterisk_1.BringToFront()
        asterisk_2.BringToFront()
        pd_data.BringToFront()
        line_data.BringToFront()
        pd_line()
    End Sub
    Public Sub new_fareprintPD4()
        fillter_data.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/filter_newfa.png")
        pd_data.BackColor = System.Drawing.Color.White
        line_data.BackColor = System.Drawing.Color.White
        lot_no_data.ReadOnly = False
        wi_data.Visible = True
        wi_data.Top = 426
        wi_data.Left = 313
        fillter_data.Top = 136
        PictureBox2.Top = 145
        PictureBox3.Top = 156
        lot_no_data.Left = 417
        DateTimePicker1.Location = New System.Drawing.Point(211, 339)
        lot_no_data.Top = 341
        lot_no_data.Visible = True
        fillter_data.Visible = True
        pd_data.Visible = True
        line_data.Visible = True
        PictureBox2.Visible = False
        Button3.Visible = True
        DateTimePicker1.Visible = True
        PictureBox3.Visible = True
        asterisk_1.Visible = True
        asterisk_2.Visible = True
        lot_no_data.BringToFront()
        Button3.BringToFront()
        DateTimePicker1.BringToFront()
        PictureBox3.BringToFront()
        asterisk_1.BringToFront()
        asterisk_2.BringToFront()
        pd_data.BringToFront()
        line_data.BringToFront()
        pd_line()
    End Sub
    Public Sub log_reprint()
        fillter_data.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/filter_data.png")
        line_data.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(199, Byte), Integer))
        pd_data.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(252, Byte), Integer), CType(CType(199, Byte), Integer))
        lot_no_data.Left = 417
        PictureBox2.Top = 145
        PictureBox3.Top = 156
        fillter_data.Top = 136
        lot_no_data.Top = 424
        asterisk_4.Location = New System.Drawing.Point(556, 303)
        lot_no_data.ReadOnly = True
        fillter_data.Visible = True
        pd_data.Visible = True
        line_data.Visible = True
        year_data.Visible = True
        month_data.Visible = True
        part_no_data.Visible = True
        lot_no_data.Visible = True
        PictureBox2.Visible = False
        Button1.Visible = True
        PictureBox3.Visible = True
        asterisk_1.Visible = True
        asterisk_2.Visible = True
        asterisk_3.Visible = True
        asterisk_4.Visible = True
        pd_data.BringToFront()
        line_data.BringToFront()
        year_data.BringToFront()
        month_data.BringToFront()
        part_no_data.BringToFront()
        lot_no_data.BringToFront()
        Button1.BringToFront()
        PictureBox3.BringToFront()
        asterisk_1.BringToFront()
        asterisk_2.BringToFront()
        asterisk_3.BringToFront()
        asterisk_4.BringToFront()
        pd_line()
    End Sub
    Public Sub _m83_reprint()
        fillter_data.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/filter_m83.png")
        fillter_data.Top = 190
        PictureBox3.Top = 220
        PictureBox2.Top = 209
        wi_data.Top = 288
        wi_data.Left = 195
        lot_no_data.Top = 288
        lot_no_data.Left = 430
        Button2.Top = 407
        Button2.Left = 335
        DateTimePicker1.Left = 309
        DateTimePicker1.Top = 365
        asterisk_4.Location = New System.Drawing.Point(450, 333)
        lot_no_data.ReadOnly = False
        DateTimePicker1.Visible = True
        fillter_data.Visible = True
        fillter_data.BringToFront()
        PictureBox3.Visible = True
        wi_data.Visible = True
        lot_no_data.Visible = True
        asterisk_4.Visible = True
        PictureBox3.BringToFront()
        wi_data.BringToFront()
        DateTimePicker1.BringToFront()
        asterisk_4.BringToFront()
        lot_no_data.BringToFront()
        Button2.Visible = True
        Button2.BringToFront()
        pd_dummy()
    End Sub
    Public Sub dummy_tag()
        fillter_data.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/filter_newfa.png")
        pd_data.BackColor = System.Drawing.Color.White
        line_data.BackColor = System.Drawing.Color.White
        lot_no_data.ReadOnly = False
        wi_data.Visible = True
        wi_data.Top = 426
        wi_data.Left = 313
        fillter_data.Top = 136
        PictureBox2.Top = 145
        PictureBox3.Top = 156
        lot_no_data.Left = 417
        DateTimePicker1.Location = New System.Drawing.Point(211, 339)
        lot_no_data.Top = 341
        lot_no_data.Visible = True
        fillter_data.Visible = True
        pd_data.Visible = True
        line_data.Visible = True
        PictureBox2.Visible = False
        Button3.Visible = True
        DateTimePicker1.Visible = True
        PictureBox3.Visible = True
        asterisk_1.Visible = True
        asterisk_2.Visible = True
        lot_no_data.BringToFront()
        Button3.BringToFront()
        DateTimePicker1.BringToFront()
        PictureBox3.BringToFront()
        asterisk_1.BringToFront()
        asterisk_2.BringToFront()
        pd_data.BringToFront()
        line_data.BringToFront()
        pd_line()
    End Sub
    Private Sub DateTimePicker1_TextChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.TextChanged
        asterisk_4.Visible = False
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        If asterisk_4.Visible = True Then
            alert.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/please_insert_data.png")
            alert.Visible = True
            alert.BringToFront()
            alert.Focus()
        Else
            m83_reprint_form_batch.Show()
            Me.Close()
        End If
    End Sub
End Class