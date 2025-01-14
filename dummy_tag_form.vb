Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.Web.Script.Serialization
Imports System.Threading.Tasks

Public Class dummy_tag_form
    Dim back_office As New Model
    Dim reader As SqlDataReader
    Dim QR_Generator As New MessagingToolkit.QRCode.Codec.QRCodeEncoder
    Dim reader_show As SqlDataReader
    Dim log_ref_id As Integer
    Dim lot_tag, new_qr_code, production_type, part_no_tag, line_tag, wi_notdata, _location, PD, part_name_tag, model_tag, part_no_class, lot_class, line_class, seq_class, qr_code_tag, next_process, plan_, _actual, wi_tag, seq_tag, shift_tag As String
    Dim qty_tag, box_tag, ref_id, box_class, qty_class, box_max, log_id, cur_qty, box_tag_ps As Integer

    Private Async Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        line_show_data.Text = line_tag
        wi_show_data.Text = wi_tag
        part_no_show_data.Text = part_no_tag
        lot_no_show_data.Text = lot_tag
        seq_show_data.Text = seq_tag
        shift_show_data.Text = shift_tag

        If back_office.get_tagtype(line_tag) = 1 Then
            Await get_by_tag_print_detail()
        Else
            get_by_tag_print_main()
        End If
        loading.Refresh()
        wi_show_data.Visible = True
        part_name_show_data.Visible = True
        part_no_show_data.Visible = True
        model_show_data.Visible = True
        line_show_data.Visible = True
        shift_show_data.Visible = True
        lot_no_show_data.Visible = True
        seq_show_data.Visible = True
        ac_date_show_data.Visible = True
        plan_date_show_data.Visible = True
        qty_show_data.Visible = True
        tag_show_data.Visible = True
        PictureBox2.Visible = True
        exit_pagedata.Visible = True
        exit_pagedata.BringToFront()
        print_BT.Visible = True
        max_box_ps.Visible = True
        print_BT.BringToFront()
        max_box_ps.BringToFront()
        reprint.Visible = False
        loading.Close()
        Timer1.Stop()
    End Sub

    Private Sub slide_Click(sender As Object, e As EventArgs) Handles slide.Click
        reprint.Visible = True
    End Sub
    Private Sub log_reprint_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub
    Public Sub New()
        Try
            Dim line_ref As String = menu_form.line_data.Text
            Dim prod_date_ref As String = menu_form.DateTimePicker1.Text
            ' MsgBox(prod_date_ref)
            Dim newformat As String = format_date(prod_date_ref)
            Dim wi_ref As String = menu_form.wi_data.Text
            Dim lot_ref As String = menu_form.lot_no_data.Text
            InitializeComponent()

            Dim rs = back_office.get_data_dummytag(line_ref, newformat, wi_ref, lot_ref)

            If rs <> "0" Then
                Dim result_data_json As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
                For Each item As Object In result_data_json
                    Dim line_show As String = item("line_cd").ToString()
                    Dim wi_show As String = item("wi").ToString()
                    wi_notdata = wi_show
                    Dim part_show As String = item("item_cd").ToString()
                    Dim lot_no_show As String = item("lot_no").ToString()
                    Dim seq_show As String = item("seq_no").ToString()
                    Dim shift_show As String = item("shift_prd").ToString()
                    Dim newitem As New ListViewItem(line_show)
                    newitem.SubItems.Add(wi_show)
                    newitem.SubItems.Add(part_show)
                    newitem.SubItems.Add(lot_no_show)
                    newitem.SubItems.Add("00" & seq_show)
                    newitem.SubItems.Add(shift_show)
                    ListView1.Items.Add(newitem)
                Next
            End If
            If wi_notdata = "" Then
                alert.Visible = True
            End If
        Catch ex As Exception
            alert.Visible = True
        End Try
    End Sub
    Public Function format_date(ByVal new_format As String)
        Try
            Dim day As String = new_format.Substring(0, 2)
            Dim month As String = new_format.Substring(3, 2)
            Dim year As String = new_format.Substring(6, 2)
            Dim current_time = Date.Today.Year
            Dim format_for_ref As String = (current_time.ToString.Substring(0, 2) & year & "-" & month & "-" & day)
            Return format_for_ref
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Return 0
    End Function
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        menu_form.Show()
        Me.Close()
    End Sub
    Private Sub alert_click(sender As Object, e As EventArgs) Handles alert.Click
        menu_form.Show()
        Me.Close()
    End Sub
    Private Sub succes_Click(sender As Object, e As EventArgs) Handles succes.Click
        succes.Visible = False
        tag_show_data.Focus()
    End Sub
    Private Sub succes_KeyPress(sender As Object, e As KeyPressEventArgs) Handles succes.KeyPress
        succes.Visible = False
        tag_show_data.Focus()
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim aPen = New Pen(Color.Black)
        aPen.Width = 2.0F
        '   Dim num_char_box1 As Integer = box_max.ToString().Length
        'Dim box_qr As String = ""
        'If num_char_box1 = 1 Then
        'box_qr = "00" & box_max
        'ElseIf num_char_box1 = 2 Then
        'box_qr = "0" & box_max
        'Else
        'box_qr = box_max
        'End If


        Dim qr_detail As String = (new_qr_code & box_max)
        'MsgBox(Label10.Text)
        'vertical
        e.Graphics.DrawLine(aPen, 150, 10, 150, 290)
        e.Graphics.DrawLine(aPen, 300, 175, 300, 290)
        e.Graphics.DrawLine(aPen, 590, 10, 590, 175)
        e.Graphics.DrawLine(aPen, 410, 120, 410, 235)
        e.Graphics.DrawLine(aPen, 410, 175, 410, 235)
        e.Graphics.DrawLine(aPen, 225, 175, 225, 235)
        e.Graphics.DrawLine(aPen, 490, 10, 490, 65)
        e.Graphics.DrawLine(aPen, 520, 175, 520, 290)
        e.Graphics.DrawLine(aPen, 610, 175, 610, 290)
        e.Graphics.DrawLine(aPen, 700, 10, 700, 290)
        'Horizontal
        e.Graphics.DrawLine(aPen, 150, 11, 700, 11)
        e.Graphics.DrawLine(aPen, 150, 65, 590, 65)
        e.Graphics.DrawLine(aPen, 150, 120, 700, 120)
        e.Graphics.DrawLine(aPen, 150, 175, 700, 175)
        e.Graphics.DrawLine(aPen, 150, 235, 610, 235)
        e.Graphics.DrawLine(aPen, 150, 289, 700, 289)
        'TAG LAYOUT
        e.Graphics.DrawString("PART NO.", lb_font1.Font, Brushes.Black, 152, 13)
        e.Graphics.DrawString(part_no_show_data.Text, lb_font2.Font, Brushes.Black, 152, 25)
        e.Graphics.DrawString("QTY.", lb_font1.Font, Brushes.Black, 492, 13)
        e.Graphics.DrawString(qty_show_data.Text, lb_font2.Font, Brushes.Black, 505, 25)
        e.Graphics.DrawString("PART NAME", lb_font1.Font, Brushes.Black, 152, 67)

        e.Graphics.DrawString(part_name_show_data.Text, font_part_name.Font, Brushes.Black, 152, 79)
        e.Graphics.DrawString("MODEL", lb_font1.Font, Brushes.Black, 152, 123)
        e.Graphics.DrawString(model_show_data.Text, lb_font5.Font, Brushes.Black, 152, 141)
        e.Graphics.DrawString("NEXT PROCESS", lb_font1.Font, Brushes.Black, 412, 123)
        e.Graphics.DrawString(next_process, lb_font5.Font, Brushes.Black, 414, 141)

        e.Graphics.DrawString("LOCATION", lb_font1.Font, Brushes.Black, 592, 123)
        e.Graphics.DrawString(_location, lb_font5.Font, Brushes.Black, 596, 141)
        e.Graphics.DrawString("SHIFT", lb_font1.Font, Brushes.Black, 152, 178)
        e.Graphics.DrawString(shift_show_data.Text, lb_font2.Font, Brushes.Black, 170, 190)
        e.Graphics.DrawString("PRO. SEQ.", lb_font1.Font, Brushes.Black, 227, 178)
        e.Graphics.DrawString(seq_show_data.Text, lb_font2.Font, Brushes.Black, 231, 190)
        e.Graphics.DrawString("BOX NO.", lb_font1.Font, Brushes.Black, 302, 178)
        e.Graphics.DrawString(box_max, lb_font2.Font, Brushes.Black, 320, 190)
        e.Graphics.DrawString("ACTUAL DATE", lb_font1.Font, Brushes.Black, 412, 178)
        e.Graphics.DrawString(ac_date_show_data.Text, lb_font3.Font, Brushes.Black, 412, 196)
        Dim factory_cd As String
        If PD = "K2PD06" Then
            factory_cd = "Phase8"
        Else
            factory_cd = "Phase10"
        End If
        e.Graphics.DrawString("FACTORY", lb_font1.Font, Brushes.Black, 522, 178)
        e.Graphics.DrawString(factory_cd, lb_font3.Font, Brushes.Black, 522, 196)
        e.Graphics.DrawString("INFO.", lb_font1.Font, Brushes.Black, 612, 178)
        ''e.Graphics.DrawString(Label14.Text, lb_font2.Font, Brushes.Black, 626, 190)
        e.Graphics.DrawString("LINE", lb_font1.Font, Brushes.Black, 152, 238)
        e.Graphics.DrawString(line_show_data.Text, lb_font2.Font, Brushes.Black, 152, 250)
        e.Graphics.DrawString("PLAN DATE", lb_font1.Font, Brushes.Black, 302, 238)
        e.Graphics.DrawString(plan_date_show_data.Text, plandate.Font, Brushes.Black, 334, 255)
        e.Graphics.DrawString("LOT NO.", lb_font1.Font, Brushes.Black, 522, 238)
        e.Graphics.DrawString(lot_no_show_data.Text, lb_font2.Font, Brushes.Black, 522, 250)
        ''e.Graphics.DrawString("PROD. SEQ.", lb_font1.Font, Brushes.Black, 612, 238)
        ''e.Graphics.DrawString(plan_seq, lb_font2.Font, Brushes.Black, 622, 250)
        e.Graphics.DrawString("TBKK", lb_font2.Font, Brushes.Black, 15, 13)
        e.Graphics.DrawString("(Thailand) Co., Ltd.", lb_font1.Font, Brushes.Black, 15, 45)
        e.Graphics.DrawString("Shop floor system", lb_font4.Font, Brushes.Black, 15, 73)
        e.Graphics.DrawString("(New FA system)", lb_font4.Font, Brushes.Black, 15, 85)
        e.Graphics.DrawString("WI : " & wi_tag, lb_font6.Font, Brushes.Black, 15, 123)
        production_type = back_office.product_type(wi_tag)
        e.Graphics.DrawString(production_type, lb_font6.Font, Brushes.Black, 15, 136)
        e.Graphics.DrawString("[ Dummy Tag ]", lb_font4.Font, Brushes.Black, 31, 172)
        Dim generate As New MessagingToolkit.QRCode.Codec.QRCodeEncoder

        qr_code_1.Image = generate.Encode(qr_detail)
        e.Graphics.DrawImage(qr_code_1.Image, 597, 17, 95, 95)
        e.Graphics.DrawImage(qr_code_1.Image, 31, 190, 95, 95)
        qr_code_2.Image = generate.Encode(qr_detail)
        e.Graphics.DrawImage(qr_code_2.Image, 620, 199, 70, 70)
        back_office.insert_dummy_tag(log_id, cur_qty, qty_show_data.Text, box_tag, box_max, next_process, qr_detail)
    End Sub
    Public Sub get_data()
        Dim Model_reprint As New Model()
        Dim rs = Model_reprint.GET_DATA_TAG_2(wi_tag)
        If rs <> "0" Then
            Dim result_data_json As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
            For Each item As Object In result_data_json
                model_show_data.Text = item("MODEL").ToString()
                part_name_show_data.Text = item("ITEM_NAME").ToString()
                PD = item("PD").ToString()
                _location = item("LOCATION_PART").ToString()
            Next
        End If
    End Sub

    Private Sub reprint_Click(sender As Object, e As EventArgs) Handles reprint.Click
        Try
            If ListView1.SelectedItems.Count > 0 Then
                loading.Refresh()
                line_tag = ListView1.SelectedItems(0).SubItems(0).Text
                wi_tag = ListView1.SelectedItems(0).SubItems(1).Text
                part_no_tag = ListView1.SelectedItems(0).SubItems(2).Text
                lot_tag = ListView1.SelectedItems(0).SubItems(3).Text
                seq_tag = ListView1.SelectedItems(0).SubItems(4).Text
                shift_tag = ListView1.SelectedItems(0).SubItems(5).Text
                box_max = back_office.find_max_box(part_no_tag, lot_tag, line_tag, seq_tag)
                loading.Refresh()
                box_tag_ps = 999 - box_max
                If box_max <= 989 Then
                    max_box_ps.Text = ("MAX 10 TAG/TIME")
                ElseIf box_max > 989 Then
                    max_box_ps.Text = ("MAX " & box_tag_ps & " TAG/TIME")
                End If
                If box_tag_ps = 0 Then
                    alert_2.Visible = True
                    alert_2.BringToFront()
                    alert_2.Focus()
                    Exit Sub
                End If
            Else
                succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/select_data_print.png")
                succes.Visible = True
                succes.Focus()
                succes.BringToFront()
                Exit Sub
            End If
            Me.Timer1.Start()
            ' loading.Show()
            loading.Show()
        Catch ex As Exception
            MsgBox("EROOR!!" & " " & ex.Message())
        End Try
    End Sub
    Public Sub get_by_tag_print_main()
        Try
            Dim rs = back_office.get_data_dummytag_m83(wi_tag, lot_tag, part_no_tag)
            If rs <> "0" Then
                Dim result_data_json As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
                For Each item As Object In result_data_json
                    log_id = item("tag_id").ToString()
                    qr_code_tag = item("tag_qr_detail").ToString()
                    Dim plan As String = qr_code_tag.Substring(8, 8)
                    box_tag = item("tag_batch_no").ToString()
                    Dim actual As String = qr_code_tag.Substring(44, 8)
                    Dim act As Date = Date.ParseExact(actual, "yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
                    Dim pld As Date = Date.ParseExact(plan, "yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
                    ac_date_show_data.Text = act.ToString("dd/MM/yyyy")
                    plan_date_show_data.Text = pld.ToString("dd/MM/yyyy")
                    Dim lot As String = qr_code_tag.Substring(44, 18)
                    Dim lot_show = lot.Substring(lot.Length - 4)
                    Dim qt_y As String = lot.Substring(Trim(lot.Length - 7))
                    qty_show_data.Text = qt_y.Remove(qt_y.Length - 4).Trim()
                    cur_qty = qty_show_data.Text
                Next
            End If
            get_data()
        Catch ex As Exception
            MsgBox("ERROR!!" & " " & ex.Message())
        End Try
    End Sub
    Public Async Function get_by_tag_print_detail() As Task(Of String)
        ' Try
        Dim rs = Await back_office.get_data_tag_for_dummy(wi_tag, seq_tag, lot_tag)
        If rs <> "0" Then
            Dim result_data_json As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
            For Each item As Object In result_data_json
                Dim qr_detail As String = item("qr_detail").ToString()
                log_id = item("id").ToString()
                box_tag = item("box_no").ToString()
                qr_code_tag = qr_detail
                Dim plan As String = qr_detail.Substring(8, 8)
                Dim actual As String = qr_detail.Substring(44, 8)
                Dim act As Date = Date.ParseExact(actual, "yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
                Dim pld As Date = Date.ParseExact(plan, "yyyyMMdd", Globalization.CultureInfo.InvariantCulture)
                ac_date_show_data.Text = act.ToString("dd/MM/yyyy")
                plan_date_show_data.Text = pld.ToString("dd/MM/yyyy")
                next_process = item("next_proc").ToString()
                Dim lot As String = qr_detail.Substring(44, 18)
                Dim lot_show = lot.Substring(lot.Length - 4)
                Dim qt_y As String = lot.Substring(Trim(lot.Length - 7))
                qty_show_data.Text = qt_y.Remove(qt_y.Length - 4).Trim()
                cur_qty = qty_show_data.Text
            Next
            get_data()
        End If
        Return "11111111111"
        'Catch ex As Exception
        '    MsgBox("ERROR!!" & " " & ex.Message())
        'End Try
    End Function
    Private Sub exit_pagedata_Click(sender As Object, e As EventArgs) Handles exit_pagedata.Click
        wi_show_data.Visible = False
        part_name_show_data.Visible = False
        part_no_show_data.Visible = False
        model_show_data.Visible = False
        line_show_data.Visible = False
        shift_show_data.Visible = False
        lot_no_show_data.Visible = False
        seq_show_data.Visible = False
        ac_date_show_data.Visible = False
        plan_date_show_data.Visible = False
        qty_show_data.Visible = False
        tag_show_data.Visible = False
        PictureBox2.Visible = False
        print_BT.Visible = False
        exit_pagedata.Visible = False
        max_box_ps.Text = Nothing
        max_box_ps.Visible = False
        tag_show_data.Text = Nothing
    End Sub
    Private Sub print_BT_Click(sender As Object, e As EventArgs) Handles print_BT.Click
        If tag_show_data.Text = Nothing Then
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/please_insert_data.png")
            succes.BringToFront()
            succes.Visible = True
            succes.Focus()
            Exit Sub
        End If
        If tag_show_data.Text = 0 Then
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/please_insert_data.png")
            succes.BringToFront()
            succes.Visible = True
            succes.Focus()
            Exit Sub
        End If
        gen_qr()
        If back_office.get_tagtype(line_tag) = 1 Then
            tag_type_1()
        Else
            tag_type_2()
        End If
    End Sub
    Private Sub gen_qr()
        Dim qr_code As String = qr_code_tag.Substring(0, 52)
        Dim qq As Integer = qty_show_data.Text
        Dim qr_code_ As String = qr_code_tag.Substring(87, 13)
        If qq < 10 Then
            new_qr_code = (qr_code & "     " & qq & lot_no_show_data.Text & "                         " & qr_code_)
        ElseIf qq < 100 Then
            new_qr_code = (qr_code & "    " & qq & lot_no_show_data.Text & "                         " & qr_code_)
        ElseIf qq < 1000 Then
            new_qr_code = (qr_code & "   " & qq & lot_no_show_data.Text & "                         " & qr_code_)
        ElseIf qq < 10000 Then
            new_qr_code = (qr_code & "  " & qq & lot_no_show_data.Text & "                         " & qr_code_)
        End If
    End Sub
    Public Sub tag_type_1()
        Dim tag_amount As Integer = tag_show_data.Text
        If tag_amount > 10 Then
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_max_tag_time.png")
            succes.BringToFront()
            succes.Visible = True
            succes.Focus()
            tag_show_data.Text = Nothing
            Exit Sub
        End If
        If box_max = 999 Then
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_box_number_over.png")
            succes.BringToFront()
            succes.Visible = True
            succes.Focus()
            Exit Sub
        End If
        If box_max = 0 Then
            box_max = 900
            Dim index As Integer = 1
            Do While index <= tag_show_data.Text
                PrintDocument1.Print()
                '  PrintPreviewDialog1.ShowDialog()
                If box_max = 999 Then
                    succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_box_number_over.png")
                    succes.BringToFront()
                    succes.Visible = True
                    succes.Focus()
                    Exit Sub
                End If
                box_max += 1
                index += 1
            Loop
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_success.png")
            succes.Focus()
            succes.BringToFront()
            succes.Visible = True
        Else
            box_max = back_office.find_max_box(part_no_tag, lot_tag, line_tag, seq_tag)
            box_max += 1
            Dim index As Integer = 1
            Do While index <= tag_show_data.Text
                PrintDocument1.Print()
                'PrintPreviewDialog1.ShowDialog()
                If box_max = 999 Then
                    succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_box_number_over.png")
                    succes.Visible = True
                    succes.Focus()
                    succes.BringToFront()
                    Exit Sub
                End If
                box_max += 1
                index += 1
            Loop
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_success.png")
            succes.Focus()
            succes.BringToFront()
            succes.Visible = True
        End If
        box_tag_ps = 999 - box_max
        If box_max <= 989 Then
            max_box_ps.Text = ("MAX 10 TAG/TIME")
        ElseIf box_max > 989 Then
            max_box_ps.Text = ("MAX " & box_tag_ps & " TAG/TIME")
        End If
    End Sub
    Public Sub tag_type_2()
        Dim tag_amount As Integer = tag_show_data.Text
        If tag_amount > 10 Then
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_max_tag_time.png")
            succes.BringToFront()
            succes.Visible = True
            succes.Focus()
            tag_show_data.Text = Nothing
            Exit Sub
        End If
        If box_max = 999 Then
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_box_number_over.png")
            succes.BringToFront()
            succes.Visible = True
            succes.Focus()
            Exit Sub
        End If
        If box_max = 0 Then
            box_max = 900
            Dim index As Integer = 1
            Do While index <= tag_show_data.Text
                PrintDocument2.Print()
                '  PrintPreviewDialog2.ShowDialog()
                If box_max = 999 Then
                    succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_box_number_over.png")
                    succes.BringToFront()
                    succes.Visible = True
                    succes.Focus()
                    Exit Sub
                End If
                box_max += 1
                index += 1
            Loop
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_success.png")
            succes.Focus()
            succes.BringToFront()
            succes.Visible = True
        Else
            box_max = back_office.find_max_box(part_no_tag, lot_tag, line_tag, seq_tag)
            box_max += 1
            Dim index As Integer = 1
            Do While index <= tag_show_data.Text
                PrintDocument2.Print()
                '  PrintPreviewDialog2.ShowDialog()
                If box_max = 999 Then
                    succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_box_number_over.png")
                    succes.BringToFront()
                    succes.Visible = True
                    succes.Focus()
                    Exit Sub
                End If
                box_max += 1
                index += 1
            Loop
            succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/alert_success.png")
            succes.Focus()
            succes.BringToFront()
            succes.Visible = True
        End If
        box_tag_ps = 999 - box_max
        If box_max <= 989 Then
            max_box_ps.Text = ("MAX 10 TAG/TIME")
        ElseIf box_max > 989 Then
            max_box_ps.Text = ("MAX " & box_tag_ps & " TAG/TIME")
        End If
    End Sub

    Private Sub PrintDocument2_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument2.PrintPage
        Try
            Dim aPen = New Pen(Color.Black)
            aPen.Width = 2.0F
            Dim qr_detail As String = (new_qr_code & box_max)
            'vertical ตรง
            e.Graphics.DrawLine(aPen, 10, 10, 10, 290)
            e.Graphics.DrawLine(aPen, 330, 58, 330, 95)
            e.Graphics.DrawLine(aPen, 460, 10, 460, 290)
            e.Graphics.DrawLine(aPen, 580, 10, 580, 290) 'QTY
            e.Graphics.DrawLine(aPen, 110, 10, 110, 290) 'QR right top
            e.Graphics.DrawLine(aPen, 700, 10, 700, 290)
            'Horizontal นอน
            e.Graphics.DrawLine(aPen, 110, 58, 580, 58)
            e.Graphics.DrawLine(aPen, 110, 95, 700, 95)
            e.Graphics.DrawLine(aPen, 10, 11, 700, 11)
            e.Graphics.DrawLine(aPen, 10, 110, 110, 110)
            e.Graphics.DrawLine(aPen, 10, 220, 110, 220)
            e.Graphics.DrawLine(aPen, 580, 67, 700, 67)
            e.Graphics.DrawLine(aPen, 460, 140, 700, 140)
            e.Graphics.DrawLine(aPen, 460, 190, 700, 190)
            e.Graphics.DrawLine(aPen, 460, 240, 580, 240)
            e.Graphics.DrawLine(aPen, 10, 289, 700, 289)
            e.Graphics.DrawString("TBKK", font_1.Font, Brushes.Black, 19, 15)
            e.Graphics.DrawString("WI : " & wi_show_data.Text, Label_wi_type.Font, Brushes.Black, 15, 60)
            Dim prodtype As String = back_office.product_type(wi_tag)
            e.Graphics.DrawString(prodtype, Label_wi_type.Font, Brushes.Black, 15, 85)
            e.Graphics.DrawString("Instr. Code", font_2.Font, Brushes.Black, 120, 15)
            e.Graphics.DrawString(part_no_show_data.Text & "   " & part_name_show_data.Text, font_4.Font, Brushes.Black, 120, 35)
            e.Graphics.DrawString("QTY.", font_3.Font, Brushes.Black, 465, 15)
            e.Graphics.DrawString(qty_show_data.Text, font_qty.Font, Brushes.Black, 495, 28)
            e.Graphics.DrawString("MODEL", font_2.Font, Brushes.Black, 120, 60)
            e.Graphics.DrawString(model_show_data.Text, font_4.Font, Brushes.Black, 140, 75)
            e.Graphics.DrawString("NEXT PROCESS", font_2.Font, Brushes.Black, 340, 60)
            e.Graphics.DrawString("ISUZU", font_4.Font, Brushes.Black, 360, 75)
            e.Graphics.DrawString("LOCATION", font_2.Font, Brushes.Black, 465, 60)
            e.Graphics.DrawString("D4U10A1", font_4.Font, Brushes.Black, 485, 75)
            e.Graphics.DrawString("SHIFT", font_2.Font, Brushes.Black, 585, 69)
            e.Graphics.DrawString(shift_show_data.Text, font_4.Font, Brushes.Black, 625, 75)
            e.Graphics.DrawString("LINE", font_2.Font, Brushes.Black, 467, 98)
            e.Graphics.DrawString("K1M083", font_4.Font, Brushes.Black, 470, 115)
            e.Graphics.DrawString("PRO SEQ", font_2.Font, Brushes.Black, 585, 98)
            e.Graphics.DrawString(seq_show_data.Text, font_4.Font, Brushes.Black, 610, 115)
            e.Graphics.DrawString("PLAN DATE", font_2.Font, Brushes.Black, 465, 145)
            e.Graphics.DrawString(plan_date_show_data.Text, font_4.Font, Brushes.Black, 470, 162)
            e.Graphics.DrawString("BATCH NO", font_2.Font, Brushes.Black, 585, 145)
            e.Graphics.DrawString(box_max, font_4.Font, Brushes.Black, 610, 162)
            e.Graphics.DrawString("ACTUAL DATE", font_2.Font, Brushes.Black, 465, 195)
            e.Graphics.DrawString(ac_date_show_data.Text, font_4.Font, Brushes.Black, 470, 215)
            e.Graphics.DrawString("LOT NO", font_2.Font, Brushes.Black, 465, 245)
            e.Graphics.DrawString(lot_no_show_data.Text, font_4.Font, Brushes.Black, 485, 265)
            e.Graphics.DrawString("NO.", font_2.Font, Brushes.Black, 120, 105)
            e.Graphics.DrawString("Part No.", font_2.Font, Brushes.Black, 160, 105)
            e.Graphics.DrawString("Part Name.", font_2.Font, Brushes.Black, 250, 105)
            e.Graphics.DrawString("QTY", font_2.Font, Brushes.Black, 420, 105)
            e.Graphics.DrawLine(aPen, 110, 123, 460, 123)
            e.Graphics.DrawLine(aPen, 110, 210, 460, 210)
            Dim margin_left_no = 125
            Dim margin_top_no = 130
            Dim margin_left_item_cd = 160
            Dim margin_left_part_name = 250
            Dim margin_left_QTY = 425
            Dim arr_item_cd() As String = {"898244-6240", "898244-6250", "898244-6260", "898244-6270", "898244-6280"}
            Dim arr_qr_code_sub() As String = {"01", "02", "03", "04", "05"}
            For i = 1 To 5 Step 1
                e.Graphics.DrawString(i, font_2.Font, Brushes.Black, margin_left_no, margin_top_no)
                e.Graphics.DrawString(arr_item_cd(i - 1), font_2.Font, Brushes.Black, margin_left_item_cd, margin_top_no)
                e.Graphics.DrawString("BRACKET ASM;CAM NO" & i, font_2.Font, Brushes.Black, margin_left_part_name, margin_top_no)
                e.Graphics.DrawString(qty_show_data.Text, font_2.Font, Brushes.Black, margin_left_QTY, margin_top_no)
                margin_top_no += 15
            Next
            Dim qrcode As New MessagingToolkit.QRCode.Codec.QRCodeEncoder
            qrcode.QRCodeScale = 10
            Dim qr_by_model = 120
            Dim bitmap_qr_box As Bitmap = qrcode.Encode("TEST")
            Dim qr_by_model_left = 118
            Dim rs = back_office.get_qrcode_tag_sub(log_id)
            Dim j = 1
            If rs <> "0" Then
                Dim result_data_json As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(rs)
                For Each item As Object In result_data_json
                    Dim qr_code As String = item("tag_qr_detail").ToString()
                    ' MsgBox(qr_code)
                    bitmap_qr_box = QR_Generator.Encode(qr_code)
                    e.Graphics.DrawString("QR No ." & j, font_2.Font, Brushes.Black, qr_by_model_left, 215)
                    e.Graphics.DrawImage(bitmap_qr_box, qr_by_model, 233, 50, 50) 'Right top
                    qr_by_model += 70
                    margin_top_no += 15
                    qr_by_model_left += 70
                    j += 1
                Next
            End If
            Dim qr_detail_main As Bitmap = QR_Generator.Encode(qr_detail)
            e.Graphics.DrawImage(qr_detail_main, 15, 120, 90, 90) 'left
            e.Graphics.DrawImage(qr_detail_main, 615, 15, 45, 45) 'Right top
            e.Graphics.DrawString("FACTORY", font_3.Font, Brushes.Black, 15, 230)
            e.Graphics.DrawString("Phase10", font_3.Font, Brushes.Black, 33, 250)
            e.Graphics.DrawImage(qr_detail_main, 600, 205, 75, 75) 'Right top
            back_office.insert_dummy_tag2(log_id, cur_qty, qty_show_data.Text, box_tag, box_max, "ISUZU", qr_detail)
        Catch ex As Exception
            MsgBox("EROOR!!" & " " & ex.Message())
        End Try
    End Sub
    Private Sub qty_show_data_KeyPress(sender As Object, e As KeyPressEventArgs) Handles qty_show_data.KeyPress
        qty_show_data.MaxLength = 4
        Dim ch As Char = e.KeyChar
        If Not Char.IsDigit(ch) AndAlso Asc(ch) <> 8 Then
            e.Handled = True
        End If
    End Sub
    Private Sub tag_show_data_KeyPress(sender As Object, e As KeyPressEventArgs) Handles tag_show_data.KeyPress
        tag_show_data.MaxLength = 2
        Dim ch As Char = e.KeyChar
        If Not Char.IsDigit(ch) AndAlso Asc(ch) <> 8 Then
            e.Handled = True
        End If
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        Try
            If ListView1.SelectedItems.Count > 0 Then
                loading.Refresh()
                line_tag = ListView1.SelectedItems(0).SubItems(0).Text
                wi_tag = ListView1.SelectedItems(0).SubItems(1).Text
                part_no_tag = ListView1.SelectedItems(0).SubItems(2).Text
                lot_tag = ListView1.SelectedItems(0).SubItems(3).Text
                seq_tag = ListView1.SelectedItems(0).SubItems(4).Text
                shift_tag = ListView1.SelectedItems(0).SubItems(5).Text
                box_max = back_office.find_max_box(part_no_tag, lot_tag, line_tag, seq_tag)
                loading.Refresh()
                box_tag_ps = 999 - box_max
                If box_max <= 989 Then
                    max_box_ps.Text = ("MAX 10 TAG/TIME")
                ElseIf box_max > 989 Then
                    max_box_ps.Text = ("MAX " & box_tag_ps & " TAG/TIME")
                End If
                If box_tag_ps = 0 Then
                    alert_2.Visible = True
                    alert_2.BringToFront()
                    alert_2.Focus()
                    Exit Sub
                End If
            Else
                succes.Image = Api.DownloadImage("http://192.168.161.102/reprint_app/images/alert/select_data_print.png")
                succes.Visible = True
                succes.Focus()
                succes.BringToFront()
                Exit Sub
            End If
            Me.Timer1.Start()
            ' loading.Show()
            loading.Show()
        Catch ex As Exception
            MsgBox("EROOR!!" & " " & ex.Message())
        End Try
    End Sub
    Private Sub alert_2_Click(sender As Object, e As EventArgs) Handles alert_2.Click
        alert_2.Visible = False
    End Sub

    Private Sub alert_2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles alert_2.KeyPress
        alert_2.Visible = False
    End Sub
End Class