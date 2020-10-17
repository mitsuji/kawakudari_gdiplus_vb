Imports System
Imports System.Drawing
Imports System.Windows.Forms

Module IchigoJam

Class Std15

  Const CHAR_W As Integer = 8
  Const CHAR_H As Integer = 8

  Public Enum Direction
    Up
    Right
    Down
    Left
  End Enum

  Private screenW As Integer
  Private screenH As Integer
  Private buffW As Integer
  Private buffH As Integer
  Private dotW As Single
  Private dotH As Single
  Private buff() As Char
  Private cursorX As Integer = 0
  Private cursorY As Integer = 0
  Private graphics As Graphics
  Private bufferedGraphics As BufferedGraphics

  Sub New (screenW As Integer, screenH As Integer, buffW As Integer, buffH As Integer, control As Control)
    Me.screenW = screenW
    Me.screenH = screenH
    Me.buffW = buffW
    Me.buffH = buffH
    Me.dotW = screenW / buffW / CHAR_W
    Me.dotH = screenH / buffH / CHAR_H
    Me.buff = New Char (buffW * buffH) {}
    
    Me.graphics = Graphics.FromHwnd(control.Handle)
    Dim bgRect As Rectangle = New Rectangle(0,0,screenW,screenH)
    Me.bufferedGraphics = BufferedGraphicsManager.Current.Allocate(control.CreateGraphics,bgRect)
  End Sub

  Public Sub Locate (x As Integer, y As Integer)
    cursorX = x
    cursorY = y
  End Sub

  Public Sub Putc (c As Char)
    SetChar(cursorX,cursorY,c)
    If cursorX < buffW-1 Then
      cursorX +=1
    Else
      If cursorY < buffH-1 Then
        cursorX = 0
        cursorY +=1
      End If
    End If
  End Sub

  Public Sub Putstr (s As String)
    For Each c As Char In s
      Putc(c)
    Next
  End Sub

  Public Sub Putnum (n As Integer)
    Putstr(n.ToString())
  End Sub

  Public Function Scr (x As Integer, y As Integer) As Char
    Return buff (y*buffW+x)
  End Function

  Public Sub Cls
    For y As Integer = 0 To buffH-1
      For x As Integer = 0 To buffW-1
        SetChar (x,y,ChrW(0))
      Next
    Next
  End Sub

  Public Sub Scroll(dir As Direction)
    For y As Integer = 0 TO buffH-1
      For x As Integer = 0 TO buffW-1
        Select Case dir
          Case Direction.Up
            If y = buffH-1 Then
              SetChar(x,y,ChrW(0))
            Else
              SetChar(x,y,Scr(x,y+1))
            End If

          Case Direction.Right
            If x = buffW-1 Then
              SetChar(buffW-x-1,y,ChrW(0))
            Else
              SetChar(buffW-x-1,y,Scr((buffW-x-1)-1,y))
            End If

          Case Direction.Down
            If y = buffH-1 Then
              SetChar(x,buffH-y-1,ChrW(0))
            Else
              SetChar(x,buffH-y-1,Scr(x,(buffH-y-1)-1))
            End If

          Case Direction.Left
            If x = buffW-1 Then
              SetChar(x,y,ChrW(0))
            Else
              SetChar(x,y,Scr(x+1,y))
            End If

        End Select
      Next
    Next
  End Sub

  Public Sub Pset (x As Integer, y As Integer)
    Dim cx As Integer = x \ 2
    Dim cy As Integer = y \ 2
    Dim c As Byte = AscW(Scr(cx,cy))
    Dim b As Byte = 2 ^ (((y Mod 2) << 1) + (x Mod 2))
    Dim d As Byte = IIF((c And &hf0) = &h80, c, &h80) Or b
    SetChar(cx, cy, ChrW(d))
  End Sub


  Private Sub SetChar (x As Integer, y As Integer, c As Char)
    buff (y*buffW+x) = c
  End Sub

  Public Sub DrawChar (g As Graphics, cx As Integer, cy As Integer, c As Char)
    Dim glyph As ULong = ICHIGOJAM_FONT(AscW(c))
    For y As Integer = 0 TO CHAR_H -1
      Dim  line As ULong = (glyph >> ((CHAR_H-y-1)*CHAR_W)) And &hffUL
      For x As Integer = 0 TO CHAR_W -1
        If ((line >> (CHAR_W-x-1)) And &h1) = &h1 THen
          g.FillRectangle(New SolidBrush(Color.White), (cx*CHAR_W+x)*dotW, (cy*CHAR_H+y)*dotH, dotW, dotH)
        End If
      Next
    Next
  End Sub

  Public Sub DrawScreen
    Dim bg As Graphics = bufferedGraphics.Graphics
    bg.Clear(Color.Black)
    For y As Integer = 0 TO buffH -1
      For x As Integer = 0 TO buffW -1
        DrawChar (bg, x, y, Scr(x,y))
      Next
    Next
    bufferedGraphics.Render(graphics)
  End Sub



  ''
  ''
  '' CC BY IchigoJam & mitsuji.org
  '' https://mitsuji.github.io/ichigojam-font.json/
  ''
  ''
  Private Shared Readonly ICHIGOJAM_FONT() As ULong = { _
    &h0000000000000000UL, _
    &hffffffffffffffffUL, _
    &hffaaff55ffaaff55UL, _
    &h55aa55aa55aa55aaUL, _
    &h005500aa005500aaUL, _
    &h995a3c5a5a242466UL, _
    &hfbfbfb00dfdfdf00UL, _
    &h24182424183c6624UL, _
    &h0a042a40fe402000UL, _
    &h000000000000ee00UL, _
    &h00042464fc602000UL, _
    &heebaee447c447c44UL, _
    &h1042008001004208UL, _
    &h007e7e7e7e7e7e00UL, _
    &h007e424242427e00UL, _
    &h007e5e5e5e427e00UL, _
    &h007e7a7a6a427e00UL, _
    &h003c242424243c00UL, _
    &hc0c0c0c0c0c0c0c0UL, _
    &hffff000000000000UL, _
    &h000000000000ffffUL, _
    &h003c3c4242423c00UL, _
    &h003c665e5e663c00UL, _
    &h0303030303030303UL, _
    &h0000ff0000ff0000UL, _
    &h03070e1c3870e0c0UL, _
    &hc0e070381c0e0703UL, _
    &h606c34f018284e40UL, _
    &h102040fe40201000UL, _
    &h100804fe04081000UL, _
    &h1038549210101000UL, _
    &h1010109254381000UL, _
    &h0000000000000000UL, _
    &h1010101010001000UL, _
    &h2828000000000000UL, _
    &h28287c287c282800UL, _
    &h103c503814781000UL, _
    &h60640810204c0c00UL, _
    &h2050502054483400UL, _
    &h0810200000000000UL, _
    &h0810202020100800UL, _
    &h2010080808102000UL, _
    &h1054381038541000UL, _
    &h0010107c10100000UL, _
    &h0000000010102000UL, _
    &h0000007c00000000UL, _
    &h0000000000303000UL, _
    &h0000040810204000UL, _
    &h38444c5464443800UL, _
    &h1030501010107c00UL, _
    &h3844040418607c00UL, _
    &h3844041804443800UL, _
    &h18284848487c0800UL, _
    &h7c40780404443800UL, _
    &h3840784444443800UL, _
    &h7c44040808101000UL, _
    &h3844443844443800UL, _
    &h384444443c043800UL, _
    &h0000100000100000UL, _
    &h0000100010102000UL, _
    &h0810204020100800UL, _
    &h00007c007c000000UL, _
    &h2010080408102000UL, _
    &h3844440810001000UL, _
    &h3844043454543800UL, _
    &h384444447c444400UL, _
    &h7824243824247800UL, _
    &h3844404040443800UL, _
    &h7824242424247800UL, _
    &h7c40407c40407c00UL, _
    &h7c40407c40404000UL, _
    &h384440404c443c00UL, _
    &h4444447c44444400UL, _
    &h3810101010103800UL, _
    &h1c08080808483000UL, _
    &h4448506050484400UL, _
    &h4040404040407c00UL, _
    &h446c6c5454544400UL, _
    &h446464544c4c4400UL, _
    &h3844444444443800UL, _
    &h7844444478404000UL, _
    &h3844444454483400UL, _
    &h7844444478484400UL, _
    &h3844403804443800UL, _
    &h7c10101010101000UL, _
    &h4444444444443800UL, _
    &h4444282828101000UL, _
    &h4444545454282800UL, _
    &h4444281028444400UL, _
    &h4444281010101000UL, _
    &h7c04081020407c00UL, _
    &h3820202020203800UL, _
    &h0000402010080400UL, _
    &h3808080808083800UL, _
    &h1028440000000000UL, _
    &h0000000000007c00UL, _
    &h2010080000000000UL, _
    &h000038043c443a00UL, _
    &h4040586444447800UL, _
    &h0000384440443800UL, _
    &h0404344c44443c00UL, _
    &h000038447c403800UL, _
    &h1820207c20202000UL, _
    &h00003a44443c0438UL, _
    &h4040586444444400UL, _
    &h1000301010101000UL, _
    &h0800180808080830UL, _
    &h2020242830282400UL, _
    &h3010101010101800UL, _
    &h0000785454545400UL, _
    &h0000784444444400UL, _
    &h0000384444443800UL, _
    &h0000384444784040UL, _
    &h00003844443c0404UL, _
    &h0000586440404000UL, _
    &h00003c4038047800UL, _
    &h20207c2020201800UL, _
    &h0000484848483400UL, _
    &h0000444428281000UL, _
    &h0000445454282800UL, _
    &h0000442810284400UL, _
    &h0000444428281060UL, _
    &h00007c0810207c00UL, _
    &h0c10102010100c00UL, _
    &h1010101010101000UL, _
    &h6010100810106000UL, _
    &h0000205408000000UL, _
    &ha040a804fe040800UL, _
    &h0000000000000000UL, _
    &hf0f0f0f000000000UL, _
    &h0f0f0f0f00000000UL, _
    &hffffffff00000000UL, _
    &h00000000f0f0f0f0UL, _
    &hf0f0f0f0f0f0f0f0UL, _
    &h0f0f0f0ff0f0f0f0UL, _
    &hfffffffff0f0f0f0UL, _
    &h000000000f0f0f0fUL, _
    &hf0f0f0f00f0f0f0fUL, _
    &h0f0f0f0f0f0f0f0fUL, _
    &hffffffff0f0f0f0fUL, _
    &h00000000ffffffffUL, _
    &hf0f0f0f0ffffffffUL, _
    &h0f0f0f0fffffffffUL, _
    &hffffffffffffffffUL, _
    &h0000001818000000UL, _
    &h000000ffff000000UL, _
    &h1818181818181818UL, _
    &h181818ffff181818UL, _
    &h181818f8f8181818UL, _
    &h1818181f1f181818UL, _
    &h181818ffff000000UL, _
    &h000000ffff181818UL, _
    &h0000000f1f181818UL, _
    &h000000f0f8181818UL, _
    &h1818181f0f000000UL, _
    &h181818f8f0000000UL, _
    &hfffefcf8f0e0c080UL, _
    &hff7f3f1f0f070301UL, _
    &h80c0e0f0f8fcfeffUL, _
    &h0103070f1f3f7fffUL, _
    &h44287c107c101000UL, _
    &h0000000070507000UL, _
    &h0e08080000000000UL, _
    &h0000000010107000UL, _
    &h0000000040201000UL, _
    &h0000001818000000UL, _
    &h007e027e02041800UL, _
    &h0000007c14102000UL, _
    &h0000000c70101000UL, _
    &h0000107c44041800UL, _
    &h0000007c10107c00UL, _
    &h0000087c18284800UL, _
    &h0000207c24202000UL, _
    &h0000003808087c00UL, _
    &h00003c043c043c00UL, _
    &h0000005454040800UL, _
    &h000000007e000000UL, _
    &h00fe021410106000UL, _
    &h0006186808080800UL, _
    &h107e424202041800UL, _
    &h007c10101010fe00UL, _
    &h04047e0c14244400UL, _
    &h10107e1212224600UL, _
    &h10107e107e101000UL, _
    &h003e224202043800UL, _
    &h20203e4404043800UL, _
    &h00007e0202027e00UL, _
    &h0044fe4444043800UL, _
    &h0070027202047800UL, _
    &h007e020408146200UL, _
    &h0040fe4448403e00UL, _
    &h0042422404081000UL, _
    &h003e22520a043800UL, _
    &h043808fe08083000UL, _
    &h0052525202041800UL, _
    &h007c00fe08087000UL, _
    &h404040704c404000UL, _
    &h0008fe0808087000UL, _
    &h00007c000000fe00UL, _
    &h007e023408146200UL, _
    &h107e020418761000UL, _
    &h0002020202047800UL, _
    &h0028284444828200UL, _
    &h00404e7040403e00UL, _
    &h007e020202043800UL, _
    &h0000205088040200UL, _
    &h0010fe1054549200UL, _
    &h00fe024428100800UL, _
    &h00700e700e700e00UL, _
    &h001010202442fe00UL, _
    &h0002221408146200UL, _
    &h007c20fe20201e00UL, _
    &h2020fe2224202000UL, _
    &h00003c0404047e00UL, _
    &h007c047c04047c00UL, _
    &h007e007e02043800UL, _
    &h0044444404083000UL, _
    &h0050505052949800UL, _
    &h0020202224283000UL, _
    &h007e424242427e00UL, _
    &h007e424202043800UL, _
    &h0040220202047800UL, _
    &h1048200000000000UL, _
    &h7050700000000000UL, _
    &h183878ffff783818UL, _
    &h181c1effff1e1c18UL, _
    &h183c7effff181818UL, _
    &h181818ffff7e3c18UL, _
    &h10387cfefe387c00UL, _
    &h006cfefe7c381000UL, _
    &h3838d6fed6103800UL, _
    &h10387cfe7c381000UL, _
    &h3c66c38181c3663cUL, _
    &h3c7effffffff7e3cUL, _
    &h246a2a2a2a2a2400UL, _
    &h18244281bdbdbd7eUL, _
    &h245a4281a581423cUL, _
    &h3c4281a5817e2466UL, _
    &h0c0a0a0878f87000UL, _
    &h3c4299a5ada1924cUL, _
    &h181824247eff3c7eUL, _
    &h00182442ff540000UL, _
    &h1010080810100808UL, _
    &h7c101eb9ff9f107eUL, _
    &h085a6cfe3c7e4a11UL, _
    &h1c363a3a3a3e1c00UL, _
    &h003c427e5a427e00UL, _
    &h0006061e1e7e7e00UL, _
    &h007c446464447c00UL, _
    &h18183c5a5a242466UL, _
    &h00187e99183c2466UL, _
    &h00181a7e501c1466UL, _
    &h1818101010101018UL, _
    &h0018587e0a182e62UL, _
    &h1818080808080818UL, _
    &h043e2f566ad6acf0UL _
  }
End Class

End Module
