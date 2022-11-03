Imports RglFwk


'------------------------------------------------------------------------------------------------------
' Sigla cliente per campi:
'
'         PUR
'
'------------------------------------------------------------------------------------------------------

Public Class InstallUserTable
    Inherits SBOBase.SBOInstallUserTable

#Region "Costruttori"
    '--------------------------------------------------------------------------------------
    ' Crea una nuova istanza.
    '--------------------------------------------------------------------------------------
    Public Sub New(ByRef p As SBOBase.SBOAddOn, ByRef m As SBOBase.SBOModule)
        MyBase.New(p, m)
    End Sub
#End Region

    '------------------------------------------------------------------------------------------------------
    '------------------------------------------------------------------------------------------------------

    Protected Overrides Function InstallUpdateCompany(ByVal oldMajor As Integer, ByVal oldMinor As Integer, ByVal newMajor As Integer, ByVal newMinor As Integer) As Boolean
        Dim tableName As String

        If True Then
            Try
                AggiungiCampoAlpha("OITB", "RGL_INNO_IDF", "Id folder su anagrafica articoli", 27)
            Catch ex As Exception
                RglFwk.SBOBase.Util.Log(ex)
            End Try
        End If

        Return True
    End Function

    '----------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------

    Public Sub ImportaForms()


    End Sub

End Class
