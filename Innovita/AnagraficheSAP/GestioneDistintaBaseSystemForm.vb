Imports RglFwk
Imports RglFwk.SBOBase
Imports RglFwk.SBOBase.Util
Imports SAPbouiCOM
Imports System.Collections.Generic
Imports System.Linq


Namespace AnagraficheSAP

    Public Class GestioneDistintaBaseSystemForm
        Inherits SBOBase.SBOSystemForm


        Private mtx As SAPbouiCOM.Matrix
        Private revBtn As SAPbouiCOM.Button

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Sub New(ByRef parent As SBOBase.SBOAddOn, ByRef UID As String, f As SAPbouiCOM.Form)
            MyBase.New(parent, UID, f)

            mtx = GetForm.Items.Item("1880000004").Specific

            Dim itm = GetForm.Items.Item("1880000002")

            revBtn = GetForm.Items.Add("RGL_IN_01", BoFormItemTypes.it_BUTTON).Specific

            revBtn.Item.Top = itm.Top
            revBtn.Item.Height = itm.Height
            revBtn.Item.Left = itm.Left + itm.Width + 10
            revBtn.Caption = "Crea versione"
            revBtn.Item.Width = itm.Width + 20

        End Sub

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Overrides Sub SetFilter()
            MyBase.SetFilter()

            parent.AddFormEventFilters(GetFormType, New SAPbouiCOM.BoEventTypes() {SAPbouiCOM.BoEventTypes.et_CLICK})
        End Sub
        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Private Sub GestioneDistintaBaseSystemForm_OnItemEvent(ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Handles Me.OnItemEvent

            If pVal.BeforeAction Then Return


            If pVal.EventType = BoEventTypes.et_CLICK AndAlso pVal.ItemUID = revBtn.Item.UniqueID Then
                '-------------------------------------------------------------------------------------------
                ' Aggiorna Bom con creazione versione
                '-------------------------------------------------------------------------------------------
                BlockEventi()

                Try
                    CreaVersioni()
                Catch ex As Exception
                    Log(ex)
                End Try

                ResumeEventi()
            End If

        End Sub
        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Private Sub CreaVersioni()
            Dim c As Integer = mtx.RowCount


            For i As Integer = 1 To c
                Dim chk As SAPbouiCOM.CheckBox = mtx.GetCellSpecific("1880000001", i)
                If Not chk.Checked Then Continue For

                Dim edt As SAPbouiCOM.EditText = mtx.GetCellSpecific("1880000002", i)

                Helper.CreaRevisione(edt.Value)
            Next

        End Sub

    End Class

End Namespace
