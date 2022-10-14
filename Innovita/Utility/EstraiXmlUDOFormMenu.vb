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

    Public Class EstraiXmlUDOFormMenu
        Inherits SBOBase.SBOMenu


        Private udos() As String = {"RGL_Rgl.Innovita_PRD"}
        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Sub New(ByRef parent As SBOBase.SBOAddOn, menuUID As String)
            MyBase.New(parent, menuUID)
        End Sub
        '-------------------------------------------------------------------------------------------
        ' Richiamata all'esecuzione del menu.
        '-------------------------------------------------------------------------------------------
        Public Overrides Sub Execute()


            Try

                Dim rs As New SBORecordset

                For Each udo In udos
                    Dim fileName As String = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop), udo & ".xml")

                    rs.DoQuery("SELECT ""NewFormSrf"" FROM OUDO WHERE ""Code"" = '" & udo & "'")
                    If Not rs.EoF Then
                        Dim srf As String = rs.Fields.Item("NewFormSrf").Value

                        System.IO.File.WriteAllText(fileName, srf)
                    End If
                Next

                parent.GetAppl.MessageBox("File di definizione estratti")

            Catch ex As Exception
                parent.ShowError("Impossibile salvare i file xml. $error$", ex)
            End Try

        End Sub

    End Class


End Namespace
