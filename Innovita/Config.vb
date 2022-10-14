Imports RglFwk

Public Class Config

    '-----------------------------------------------------------------------------------------------
    '-----------------------------------------------------------------------------------------------
    Public Shared Sub Init(ByRef table As SBOBase.ConfigTable)

        AddOn = New SBOBase.ConfigSection(table,
                                          "personalizza",
                                          "Personalizzazione")


        RevisioneBase = New SBOBase.ConfigObject _
            (
                AddOn,
                "rev-base",
                "Revisione di base",
                SBOBase.ConfigObject.Tipi.Numero,
                1
            )

    End Sub

    Public Shared AddOn As SBOBase.ConfigSection

    Public Shared RevisioneBase As SBOBase.ConfigObject

End Class
