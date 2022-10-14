
Public Class Helper

    '-------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------
    Public Shared Icona_CompatibileFile As String = RglFwk.SBOBase.SBOMain.GetAddOnInstance.GetTempPath & "\COMPATIBILE.png"
    '-------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------
    Public Shared Icona_FuoriConsegnaFile As String = RglFwk.SBOBase.SBOMain.GetAddOnInstance.GetTempPath & "\FUORI_CONSEGNA.png"
    '-------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------
    Public Shared Icona_NonCompatibileFile As String = RglFwk.SBOBase.SBOMain.GetAddOnInstance.GetTempPath & "\NON_COMPATIBILE.png"
    '-------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------
    Public Shared Icona_TassativaFile As String = RglFwk.SBOBase.SBOMain.GetAddOnInstance.GetTempPath & "\TASSATIVA.png"
    '-------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------
    Public Shared Sub InitIcone()

        InnovitaResource.Immagini.COMPATIBILE.Save(Icona_CompatibileFile)
        InnovitaResource.Immagini.FUORI_CONSEGNA.Save(Icona_FuoriConsegnaFile)
        InnovitaResource.Immagini.NON_COMPATIBILE.Save(Icona_NonCompatibileFile)
        InnovitaResource.Immagini.TASSATIVA.Save(Icona_TassativaFile)

    End Sub

End Class
