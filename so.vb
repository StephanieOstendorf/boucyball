Public Class frmBall
    Dim BallY As Integer, BallDir As Integer
    Dim MyDisplay As Drawing.Graphics
    Dim MyRectangle As Drawing.Rectangle
    Dim MyBall As Drawing.Image, BallSize As Integer = 50

    '---added for Bouncing Ball with detection----------
    Dim MyPaddle As Drawing.Rectangle
    Dim PaddleX As Integer = 100

    '---------------------------------------------------

    '----------added for bouncing with sound--------
    Dim BounceSound As System.Media.SoundPlayer
    Dim MissSound As System.Media.SoundPlayer
    '----------------------------------------------


    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        'toggle timer
        If timBall.Enabled Then
            timBall.Enabled = False
            btnStart.Text = "&Start"
        Else
            timBall.Enabled = True
            btnStart.Text = "&Stop"
        End If
    End Sub

    Private Sub timBall_Tick(sender As Object, e As EventArgs) Handles timBall.Tick
        'determine ball position and draw it
        'Y position of the Ball is moving 2% (or 1/50) of the panel height
        BallY = CInt(BallY + BallDir * pnlDisplay.ClientSize.Height / 50)
        'check for bounce
        If BallY < 0 Then
            BallY = 0
            BallDir = 1
            '---added for sound------

            BounceSound.Play()

            '--------------------
        Else
            'check for collision with paddle
            If (MyRectangle.IntersectsWith(MyPaddle)) Then
                BallY = MyPaddle.Y - BallSize
                BallDir = -1
            Else


                '---added for sound------

                BounceSound.Play()

                '--------------------
            End If
        End If

        'check to see if ball went off bottom - if so, reposition at top
        If (BallY > pnlDisplay.ClientSize.Height) Then

            BallY = -BallSize
            '---added for sound------

            MissSound.PlaySync()

            '--------------------
        End If

        pnlDisplay_Paint(Nothing, Nothing)
    End Sub

    Private Sub pnlDisplay_Paint(sender As Object, e As PaintEventArgs) Handles pnlDisplay.Paint
        'horizontally center ball in display rectangle
        MyRectangle = New Rectangle(CInt(0.5 * (pnlDisplay.ClientSize.Width - BallSize)), BallY, 50, 50)
        MyDisplay.Clear(pnlDisplay.BackColor)
        MyDisplay.DrawImage(MyBall, MyRectangle)

        'draw paddle
        MyDisplay.FillRectangle(Brushes.Red, MyPaddle)
    End Sub

    Private Sub frmBall_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = Keys.F) Then
            PaddleX -= 5
        ElseIf (e.KeyCode = Keys.J) Then
            PaddleX += 5
        End If
        MyPaddle.X = PaddleX
    End Sub

    Private Sub frmBall_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        'Display of objects
        MyDisplay.Dispose()
        MyBall.Dispose()
    End Sub

    Private Sub frmBall_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'initialize variables/set up graphics objects
        BallY = 0
        BallDir = 1
        MyDisplay = pnlDisplay.CreateGraphics
        MyBall = Drawing.Image.FromFile(Application.StartupPath + "\ball.gif")

        '------added for Bouncing Ball with Collision Detection --------
        MyPaddle = New Drawing.Rectangle(PaddleX, pnlDisplay.Height - 20, BallSize, 10)
        '---------------------------------------------------------------
        pnlDisplay_Paint(Nothing, Nothing)

        '----------added for bouncing with sound--------
        BounceSound = New System.Media.SoundPlayer(Application.StartupPath + "\bong.wav")
        MissSound = New System.Media.SoundPlayer(Application.StartupPath + "\missed.wav")
        '----------------------------------------------

    End Sub



End Class
