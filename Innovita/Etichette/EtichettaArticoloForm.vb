Imports RglFwk
Imports RglFwk.SBOBase
Imports RglFwk.SBOBase.Util
Imports SAPbouiCOM
Imports System.Collections.Generic


Namespace Etichette

    Public Class EtichettaArticoloForm
        Inherits SBOBase.SBODialogForm

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Private ARTICOLI_DT As New SBODataTableData(Me, "Articoli")

        Private OITM_GRD As New SBOGridData(Me, "Item_27", "Articoli", Nothing)

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Private WithEvents CREA_BTN As New SBOButton(Me, "302")

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Sub New(ByRef p As SBOBase.SBOAddOn)
            MyBase.New(p, "InnovitaResource.Etichette.etichetta-articolo.srf", , SBOBase.SBOForm.ResourceType.Integrata, GetType(InnovitaResource.InnovitaResource))
        End Sub
        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Private Sub CreazioneArticoloPFForm_OnAfterLoad() Handles Me.OnAfterLoad

            OITM_GRD.Grid.AutoResizeColumns()

            ARTICOLI_DT.AppendRow()

            GetForm.EnableMenu(SBOBase.SBOConstant.MENU_AGGIUNGI_RIGA, True)
            GetForm.EnableMenu(SBOBase.SBOConstant.MENU_CANCELLA_RIGA, True)

        End Sub
        '---------------------------------------------------------------------------------------
        '---------------------------------------------------------------------------------------
        Private Sub CREA_BTN_OnClick(ByRef bubbleEvent As Boolean) Handles CREA_BTN.OnClick
            Dim rs As New SBORecordset

            Dim c As Integer = ARTICOLI_DT.DataTable.Rows.Count - 1

            Try

                Dim bc As New InnovitaBarcodes.ArticoloBarcode

                For i As Integer = 0 To c
                    If ARTICOLI_DT.DataTable.GetValue("ItemCode", i) = "" Then Continue For

                    bc.ItemCode = ARTICOLI_DT.DataTable.GetValue("ItemCode", i)
                    bc.Quantity = ARTICOLI_DT.DataTable.GetValue("Quantity", i)

                    Dim n As Integer = ARTICOLI_DT.DataTable.GetValue("Etichette", i)
                    For j As Integer = 0 To n

                        rs.DoQuery($"CALL RGL_PUR_CALCOLA_LOTTO(NULL, '{parent.GetCompany.UserName}','{bc.ItemCode}', NULL)")
                        bc.DistNumber = rs.AsString("DistNumber")

                    Next

                Next

                ARTICOLI_DT.DataTable.Rows.Clear()
                ARTICOLI_DT.AppendRow()

            Catch ex As Exception
                parent.ShowError(String.Format(InnovitaResource.Messaggi.ImpossibileStampare, ex.Message))
            End Try


        End Sub
        '---------------------------------------------------------------------------------------
        '---------------------------------------------------------------------------------------
        Private Sub EtichettaArticoloForm_OnCFLChoosed(ByRef cflEvent As IChooseFromListEvent, ByRef cflDt As DataTable, ByRef bubbleEvent As Boolean) Handles Me.OnCFLChoosed


            If cflEvent.ActionSuccess AndAlso cflEvent.ColUID = "ItemCode" Then
                '-----------------------------------------------------------------------------------
                '-----------------------------------------------------------------------------------

                Try
                    Dim i As Integer = OITM_GRD.Grid.GetDataTableRowIndex(cflEvent.Row)
                    Dim rs As New SBORecordset

                    ARTICOLI_DT.DataTable.SetValue("ItemName", i, cflDt.GetValue("ItemName", 0))
                    ARTICOLI_DT.DataTable.SetValue("Etichette", i, 1)

                    If i = ARTICOLI_DT.DataTable.Rows.Count - 1 Then
                        ARTICOLI_DT.AppendRow()
                    End If

                Catch ex As Exception
                    Log(ex)
                End Try

            End If

        End Sub
        '-------------------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------------------
        Private Sub EtichettaArticoloForm_OnMenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Handles Me.OnMenuEvent


            Select Case pVal.MenuUID

                Case SBOConstant.MENU_AGGIUNGI_RIGA
                    ARTICOLI_DT.AppendRow()
                    BubbleEvent = False

                Case SBOConstant.MENU_CANCELLA_RIGA

                    If OITM_GRD.Grid.Rows.SelectedRows.Count = 0 Then
                        parent.ShowError(String.Format(InnovitaResource.Messaggi.SelezionareRigaCancellare))
                    Else
                        Dim i = OITM_GRD.Grid.GetDataTableRowIndex(OITM_GRD.Grid.Rows.SelectedRows.Item(0, BoOrderType.ot_SelectionOrder))
                        ARTICOLI_DT.DataTable.Rows.Remove(i)
                    End If

                    BubbleEvent = False

            End Select

        End Sub

    End Class

End Namespace
