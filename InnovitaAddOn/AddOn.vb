Imports RglFwk
'------------------------------------------------------------------------------------------
'------------------------------------------------------------------------------------------
Public Class AddOn

    '--------------------------------------
    '--------------------------------------
    Public Shared Sub Main()


        Try

            RglFwk.SBOBase.SBODefaultExtensionAddOn.Main("InnovitaAddOn", "Rgl.Innovita.Innovita", "Rgl.Innovita.Innovita.dll")

        Catch ex As Exception

            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)

        End Try

    End Sub

End Class
