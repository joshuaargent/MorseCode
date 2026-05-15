' Skeleton Program for the AQA AS Summer 2018 examination
' this code should be used in conjunction with the Preliminary Material
' written by the AQA AS Programmer Team
' developed using Visual Studio Community Edition

' Version Number : 0.2

Imports System.IO

Module Module1
  Const SPACE As Char = " "
  Const EOL As Char = "#"
  Const EMPTYSTRING As String = ""

  Sub ReportError(ByVal s As String)
    Console.WriteLine(("*").PadRight(6) & s & ("*").PadLeft(6))
  End Sub

  Function StripLeadingSpaces(ByVal Transmission As String) As String
    Dim TransmissionLength As Integer = Transmission.Length()
    Dim FirstSignal As Char
    If TransmissionLength > 0 Then
      FirstSignal = Transmission(0)
      While FirstSignal = SPACE And TransmissionLength > 0
        TransmissionLength -= 1
        Transmission = Transmission.Substring(1)
        If TransmissionLength > 0 Then
          FirstSignal = Transmission(0)
        End If
      End While
    End If
    If TransmissionLength = 0 Then
      ReportError("No signal received")
    End If
    Return Transmission
  End Function

  Function StripTrailingSpaces(ByVal Transmission As String) As String
    Dim LastChar As Integer = Transmission.Length() - 1
    While Transmission(LastChar) = SPACE
      Transmission = Transmission.Remove(LastChar)
      LastChar -= 1
    End While
    Return Transmission
  End Function

  Function GetTransmission() As String
    Dim Filename As String
    Dim Transmission As String
    Console.Write("Enter file name: ")
    Try
      Filename = Console.ReadLine()
      Dim Reader As New StreamReader(Filename)
      Transmission = Reader.ReadLine
      Reader.Close()
      Transmission = StripLeadingSpaces(Transmission)
      If Transmission.Length() > 0 Then
        Transmission = StripTrailingSpaces(Transmission)
        Transmission = Transmission + EOL
      End If
    Catch
      ReportError("No transmission found")
      Transmission = EMPTYSTRING
    End Try
    Return Transmission
  End Function

  Function GetNextSymbol(ByRef i As Integer, ByVal Transmission As String) As String
    Dim Signal As String
    Dim SymbolLength As Integer
    Dim Symbol As Char
    If Transmission(i) = EOL Then
      Console.WriteLine()
      Console.WriteLine("End of transmission")
      Symbol = EMPTYSTRING
    Else
      SymbolLength = 0
      Signal = Transmission(i)
      While Signal <> SPACE And Signal <> EOL
        i += 1
        Signal = Transmission(i)
        SymbolLength += 1
      End While
      If SymbolLength = 1 Then
        Symbol = "."
      ElseIf SymbolLength = 3 Then
        Symbol = "-"
      ElseIf SymbolLength = 0 Then
        Symbol = SPACE
      Else
        ReportError("Non-standard symbol received")
        Symbol = EMPTYSTRING
      End If
    End If
    Return Symbol
  End Function

  Function GetNextLetter(ByRef i As Integer, ByVal Transmission As String) As String
    Dim SymbolString As String = EMPTYSTRING
    Dim LetterEnd As Boolean = False
    Dim Symbol As String
    While Not LetterEnd
      Symbol = GetNextSymbol(i, Transmission)
      If Symbol = SPACE Then
        LetterEnd = True
        i += 4
      ElseIf Transmission(i) = EOL Then
        LetterEnd = True
      ElseIf Transmission(i + 1) = SPACE And Transmission(i + 2) = SPACE Then
        LetterEnd = True
        i += 3
      Else
        i += 1
      End If
      SymbolString = SymbolString + Symbol
    End While
    Return SymbolString
  End Function

  Function Decode(ByVal CodedLetter As String, ByVal Dash() As Integer, ByVal Letter() As String, ByVal Dot() As Integer) As String
    Dim CodedLetterLength As Integer = CodedLetter.Length()
    Dim Pointer As Integer = 0
    Dim Symbol As Char
    For i = 0 To CodedLetterLength - 1
      Symbol = CodedLetter(i)
      If Symbol = SPACE Then
        Return SPACE
      ElseIf Symbol = "-" Then
        Pointer = Dash(Pointer)
      Else
        Pointer = Dot(Pointer)
      End If
    Next
    Return Letter(Pointer)
  End Function

  Sub ReceiveMorseCode(ByVal Dash() As Integer, ByVal Letter() As String, ByVal Dot() As Integer)
    Dim PlainText As String = EMPTYSTRING
    Dim MorseCodeString As String = EMPTYSTRING
    Dim Transmission As String = GetTransmission()
    Dim LastChar As Integer = Transmission.Length() - 1
    Dim i As Integer = 0
    Dim CodedLetter As String
    Dim PlainTextLetter As Char
    While i < LastChar
      CodedLetter = GetNextLetter(i, Transmission)
      MorseCodeString = MorseCodeString + SPACE + CodedLetter
      PlainTextLetter = Decode(CodedLetter, Dash, Letter, Dot)
      PlainText = PlainText + PlainTextLetter
    End While
    Console.WriteLine(MorseCodeString)
    Console.WriteLine(PlainText)
  End Sub

  Sub SendMorseCode(ByVal MorseCode() As String)
    Dim PlainText As String
    Dim PlainTextLength As Integer
    Dim MorseCodeString As String
    Dim PlainTextLetter As Char
    Dim CodedLetter As String
    Dim Index As Integer
    Console.Write("Enter your message (uppercase letters and spaces only): ")
    PlainText = Console.ReadLine()
    PlainTextLength = PlainText.Length()
    MorseCodeString = EMPTYSTRING
    For i = 0 To PlainTextLength - 1
      PlainTextLetter = PlainText(i)
      If PlainTextLetter = SPACE Then
        Index = 0
      Else
        Index = Asc(PlainTextLetter) - Asc("A") + 1
      End If
      CodedLetter = MorseCode(Index)
      MorseCodeString = MorseCodeString + CodedLetter + SPACE
    Next
    Console.WriteLine(MorseCodeString)
  End Sub

  Sub DisplayMenu()
    Console.WriteLine()
    Console.WriteLine("Main Menu")
    Console.WriteLine("=========")
    Console.WriteLine("R - Receive Morse code")
    Console.WriteLine("S - Send Morse code")
    Console.WriteLine("X - Exit program")
    Console.WriteLine()
  End Sub

  Function GetMenuOption() As String
    Dim MenuOption As String = EMPTYSTRING
    While MenuOption.Length() <> 1
      Console.Write("Enter your choice: ")
      MenuOption = Console.ReadLine()
    End While
    Return MenuOption
  End Function

  Sub SendReceiveMessages()
    Dim Dash = {20, 23, 0, 0, 24, 1, 0, 17, 0, 21, 0, 25, 0, 15, 11, 0, 0, 0, 0, 22, 13, 0, 0, 10, 0, 0, 0}
    Dim Letter = {" ", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}
    Dim Dot = {5, 18, 0, 0, 2, 9, 0, 26, 0, 19, 0, 3, 0, 7, 4, 0, 0, 0, 12, 8, 14, 6, 0, 16, 0, 0, 0}
    Dim MorseCode = {" ", ".-", "-...", "-.-.", "-..", ".", "..-.", "--.", "....", "..", ".---", "-.-", ".-..", "--", "-.", "---", ".--.", "--.-", ".-.", "...", "-", "..-", "...-", ".--", "-..-", "-.--", "--.."}
    Dim MenuOption As String
    Dim ProgramEnd As Boolean = False

    While Not ProgramEnd
      DisplayMenu()
      MenuOption = GetMenuOption()
      If MenuOption = "R" Then
        ReceiveMorseCode(Dash, Letter, Dot)
      ElseIf MenuOption = "S" Then
        SendMorseCode(MorseCode)
      ElseIf MenuOption = "X" Then
        ProgramEnd = True
      End If
    End While
  End Sub

  Sub Main()
    SendReceiveMessages()
    Console.ReadLine()
  End Sub
End Module
