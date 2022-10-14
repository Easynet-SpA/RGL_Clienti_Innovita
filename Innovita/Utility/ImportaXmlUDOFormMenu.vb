Imports RglFwk
Imports RglFwk.SBOBase.Util
Imports RglFwk.SBOBase
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports SAPbobsCOM
Imports System.Data.SqlClient


Namespace Utility

    Public Class ImportaXmlUDOFormMenu
        Inherits SBOBase.SBOMenu

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Sub New(ByRef parent As SBOBase.SBOAddOn)
            MyBase.New(parent, "RGL_Rgl.Innovita_05")
        End Sub
        '-------------------------------------------------------------------------------------------
        ' Richiamata all'esecuzione del menu.
        '-------------------------------------------------------------------------------------------
        Public Overrides Sub Execute()


            Try


                Dim inst As New InstallUserTable(parent, Nothing)

                inst.ImportaForms()

                parent.GetAppl.MessageBox("File di definizione della form importato")

            Catch ex As Exception
                parent.ShowError("Impossibile importare i file xml. $error$", ex)
            End Try

        End Sub

    End Class


End Namespace
