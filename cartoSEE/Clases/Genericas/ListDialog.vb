Public Class ListDialog
    Inherits System.Windows.Forms.Form
    Friend WithEvents btnBorrar As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button

#Region "Definición de propiedades"

    Dim mElems As ArrayList
    Dim mDialogResp As Boolean = False
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSaveList As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Dim mPrompt As String

    Public Property elementos() As ArrayList
        Get
            Return mElems
        End Get
        Set(ByVal Value As ArrayList)
            mElems = Value
            RellenarListView()
        End Set
    End Property

    Public Property dialogResp() As Boolean
        Get
            Return mDialogResp
        End Get
        Set(ByVal Value As Boolean)
            mDialogResp = Value
        End Set
    End Property

    Public Property prompt() As String
        Get
            Return mPrompt
        End Get
        Set(ByVal Value As String)
            mPrompt = Value
            Me.Label1.Text = Value
        End Set
    End Property

#End Region

#Region "Definición de controles"

    Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListDialog))
        Me.btnBorrar = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSaveList = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnBorrar
        '
        Me.btnBorrar.Image = CType(resources.GetObject("btnBorrar.Image"), System.Drawing.Image)
        Me.btnBorrar.Location = New System.Drawing.Point(338, 205)
        Me.btnBorrar.Name = "btnBorrar"
        Me.btnBorrar.Size = New System.Drawing.Size(86, 36)
        Me.btnBorrar.TabIndex = 1
        Me.btnBorrar.Text = "Aceptar"
        Me.btnBorrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnBorrar.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.Location = New System.Drawing.Point(244, 205)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(88, 36)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancelar"
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(349, 205)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Borrar"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(21, 51)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(403, 147)
        Me.ListBox1.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label1.Location = New System.Drawing.Point(18, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Label1"
        '
        'btnSaveList
        '
        Me.btnSaveList.Image = CType(resources.GetObject("btnSaveList.Image"), System.Drawing.Image)
        Me.btnSaveList.Location = New System.Drawing.Point(21, 205)
        Me.btnSaveList.Name = "btnSaveList"
        Me.btnSaveList.Size = New System.Drawing.Size(104, 36)
        Me.btnSaveList.TabIndex = 5
        Me.btnSaveList.Text = "Guardar lista"
        Me.btnSaveList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSaveList.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(131, 205)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Label2"
        '
        'ListDialog
        '
        Me.ClientSize = New System.Drawing.Size(452, 263)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnSaveList)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnBorrar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ListDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Sub RellenarListView()

        ListBox1.Items.Clear()
        For Each elem As String In mElems
            ListBox1.Items.Add(elem)
        Next
        Label2.Text = "Total: " & ListBox1.Items.Count

    End Sub

    Public Shared Function ListaBox(ByVal lista As ArrayList, ByVal title As String, ByVal prompt As String) As String
        Dim dlg As New ListDialog
        dlg.Text = title
        dlg.Label1.Text = prompt
        dlg.elementos = lista
        dlg.ShowDialog()
        Return dlg.dialogResp
    End Function


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrar.Click, Button1.Click
        Me.dialogResp = True
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.dialogResp = False
        Me.Close()
    End Sub

    Private Sub btnSaveList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveList.Click
        Dim swRes As System.IO.StreamWriter

        Try
            swRes = New System.IO.StreamWriter(My.Application.Info.DirectoryPath & "\Lista dialog.txt", False)
        Catch ex As Exception
            MessageBox.Show(ex.Message, AplicacionTitulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End Try

        For Each elem As String In mElems
            swRes.WriteLine(elem)
        Next

        swRes.Close()
        swRes.Dispose()
        swRes = Nothing

        Process.Start(My.Application.Info.DirectoryPath & "\Lista dialog.txt")
    End Sub
End Class
