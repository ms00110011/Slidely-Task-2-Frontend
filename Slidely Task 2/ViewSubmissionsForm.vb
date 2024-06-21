Imports System.Net.Http
Imports System.Threading.Tasks

Public Class ViewSubmissionsForm
    Private submissions As List(Of Submission)
    Private currentIndex As Integer = 0

    Private Async Sub ViewSubmissionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = "John Doe, Slidely Task 2 - View Submissions"
        Me.Size = New Size(400, 400)

        Dim lblName As New Label With {.Text = "Name", .Location = New Point(10, 10)}
        Me.Controls.Add(lblName)
        Dim txtName As New TextBox With {.Name = "txtName", .Location = New Point(150, 10), .Size = New Size(200, 20), .ReadOnly = True}
        Me.Controls.Add(txtName)

        Dim lblEmail As New Label With {.Text = "Email", .Location = New Point(10, 40)}
        Me.Controls.Add(lblEmail)
        Dim txtEmail As New TextBox With {.Name = "txtEmail", .Location = New Point(150, 40), .Size = New Size(200, 20), .ReadOnly = True}
        Me.Controls.Add(txtEmail)

        Dim lblPhone As New Label With {.Text = "Phone Num", .Location = New Point(10, 70)}
        Me.Controls.Add(lblPhone)
        Dim txtPhone As New TextBox With {.Name = "txtPhone", .Location = New Point(150, 70), .Size = New Size(200, 20), .ReadOnly = True}
        Me.Controls.Add(txtPhone)

        Dim lblGithub As New Label With {.Text = "Github Link For Task 2", .Location = New Point(10, 100)}
        Me.Controls.Add(lblGithub)
        Dim txtGithub As New TextBox With {.Name = "txtGithub", .Location = New Point(150, 100), .Size = New Size(200, 20), .ReadOnly = True}
        Me.Controls.Add(txtGithub)

        Dim lblStopwatch As New Label With {.Text = "Stopwatch time", .Location = New Point(10, 130)}
        Me.Controls.Add(lblStopwatch)
        Dim txtStopwatch As New TextBox With {.Name = "txtStopwatch", .Location = New Point(150, 130), .Size = New Size(200, 20), .ReadOnly = True}
        Me.Controls.Add(txtStopwatch)

        AddHandler btnPrevious.Click, AddressOf btnPrevious_Click
        Me.Controls.Add(btnPrevious)

        AddHandler btnNext.Click, AddressOf btnNext_Click
        Me.Controls.Add(btnNext)

        Dim submission As Submission = Await FetchSubmission(currentIndex)
        If submission IsNot Nothing Then
            DisplaySubmission(submission)
        End If
    End Sub

    Private Sub DisplaySubmission(submission As Submission)
        Me.Controls("txtName").Text = submission.Name
        Me.Controls("txtEmail").Text = submission.Email
        Me.Controls("txtPhone").Text = submission.Phone
        Me.Controls("txtGithub").Text = submission.Github_link
        Me.Controls("txtStopwatch").Text = submission.Stopwatch_time
    End Sub

    Private Async Function FetchSubmission(index As Integer) As Task(Of Submission)
        Dim submission As Submission = Nothing
        Using client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync($"http://localhost:3000/read?index={index}")
            If response.IsSuccessStatusCode Then
                Dim jsonString As String = Await response.Content.ReadAsStringAsync()
                Console.WriteLine(jsonString)
                submission = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Submission)(jsonString)
            End If
        End Using
        Return submission
    End Function

    Private Async Sub btnPrevious_Click(sender As Object, e As EventArgs)
        If currentIndex > 0 Then
            currentIndex -= 1
            Dim submission As Submission = Await FetchSubmission(currentIndex)
            If submission IsNot Nothing Then
                DisplaySubmission(submission)
            End If
        End If
    End Sub

    Private Async Sub btnNext_Click(sender As Object, e As EventArgs)
        currentIndex += 1
        Dim submission As Submission = Await FetchSubmission(currentIndex)
        If submission IsNot Nothing Then
            DisplaySubmission(submission)
        Else
            currentIndex -= 1
        End If
    End Sub


    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.P) Then
            btnPrevious.PerformClick()
            Return True
        ElseIf keyData = (Keys.Control Or Keys.N) Then
            btnNext.PerformClick()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub Label2_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub Phone_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs)
    End Sub
End Class


Public Class Submission
    Public Property Name As String
    Public Property Email As String
    Public Property Phone As String
    Public Property Github_link As String
    Public Property Stopwatch_time As String
End Class
