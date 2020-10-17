# kawakudari-gdiplus-vb

This project implements part of the [std15.h](https://github.com/IchigoJam/c4ij/blob/master/src/std15.h) API (from [c4ij](https://github.com/IchigoJam/c4ij)) with [GDI+](https://docs.microsoft.com/dotnet/api/system.drawing)(System.Drawing), and [Kawakudari Game](https://ichigojam.github.io/print/en/KAWAKUDARI.html) on top of it.

It will allow programming for [IchigoJam](https://ichigojam.net/index-en.html)-like targets that display [IchigoJam FONT](https://mitsuji.github.io/ichigojam-font.json/) on screen using a Visual Basic programming language.
```
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

```

## Prerequisite

### Windows

* [Download](https://dotnet.microsoft.com/download/dotnet-framework) and install .Net Framework.
(In most cases, it is pre-installed.)


### Linux

* [Download](https://www.mono-project.com/download/stable/) and install mono suitable for your environment.
* [Download](https://www.mono-project.com/docs/about-mono/languages/visualbasic/) and install Visual Basic suitable for your environment.(In most cases, the distribution provides the package.)
* [Download](https://www.mono-project.com/docs/gui/libgdiplus/) and install libgdiplus suitable for your environment.(In most cases, the distribution provides the package.)


### macOS

GDI+ and its ports are seemed to not provided for recent versions of macOS.





## How to use

### Windows

To build it
```
> vbc Kawakudari.vb IchigoJam.vb
```
Or with full path to compiler,
```
> \Windows\Microsoft.NET\Framework64\v3.5\vbc.exe Kawakudari.vb IchigoJam.vb
```

To run it
```
> Kawakudari.exe
```


### Linux

To build it
```
$ vbc Kawakudari.vb IchigoJam.vb
```

To run it
```
$ mono Kawakudari.exe
```



## License
[![Creative Commons License](https://i.creativecommons.org/l/by/4.0/88x31.png)](http://creativecommons.org/licenses/by/4.0/)
[CC BY](https://creativecommons.org/licenses/by/4.0/) [mitsuji.org](https://mitsuji.org)

This work is licensed under a [Creative Commons Attribution 4.0 International License](http://creativecommons.org/licenses/by/4.0/).
