Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports IchigoJam

Class Kawakudari
  Inherits Form

  Private std15 As Std15 
  Private frame As UInteger
  Private rnd As Random
  Private x As Integer
  Private running As Boolean

  Private Sub OnSetup
    std15 = New Std15(512,384,32,24,Me)
    frame = 0
    rnd = New Random
    x = 15
    running = True
  End Sub

  Private Sub OnUpdate
    If Not running Then Return
    If frame Mod 5 = 0 Then
      std15.Locate(x,5)
      std15.Putc("0")
      std15.Locate(rnd.Next(0,32),23)
      std15.Putc("*")
      std15.Scroll(Std15.Direction.Up)
      If std15.Scr(x,5) <> ChrW(0) Then
        std15.Locate(0,23)
        std15.Putstr("Game Over...")
        std15.Putnum(CType(frame,Integer))
        running = False
      End If
    End If
    frame +=1
  End Sub

  Private Sub Kawakudari_OnKeyDown (sender As Object, e As KeyEventArgs)
    If e.KeyCode = Keys.Left  Then x-=1
    if e.KeyCode = Keys.Right Then x+=1
  End Sub

  Private Sub Kawakudari_OnPaint
    std15.DrawScreen()
  End Sub

  Shared Sub Main
    Application.Run(New Kawakudari)
  End Sub

  Private timer As Timer

  Sub New
    Size = New Size(512+20,384+40)
    AddHandler KeyDown, AddressOf Kawakudari_OnKeyDown
    timer = New Timer
    AddHandler timer.Tick, AddressOf Kawakudari_OnTick
    timer.Interval = 16
    OnSetup
    timer.Start
  End Sub

  Private Sub Kawakudari_OnTick (sender As Object, e As EventArgs)
    OnUpdate
    Kawakudari_OnPaint
  End Sub

End Class
