Imports RglFwk
Imports RglFwk.SBOBase
Imports RglFwk.SBOBase.Util
Imports SAPbouiCOM
Imports System.Collections.Generic


Namespace DocumentiSAP

    Public Class OrdineAcquistoSystemForm
        Inherits SBOBase.SBOSystemForm


        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Private loadDocEntry As Integer = -1

        Private cardCodeItm As SAPbouiCOM.EditText
        Private dueDateItm As SAPbouiCOM.EditText
        Private taxDateItm As SAPbouiCOM.EditText

        Private mtx As SAPbouiCOM.Matrix
        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Sub New(ByRef parent As SBOBase.SBOAddOn, ByRef UID As String, f As SAPbouiCOM.Form)
            MyBase.New(parent, UID, f)

            mtx = GetForm.Items.Item("38").Specific
            cardCodeItm = GetForm.Items.Item("4").Specific
            dueDateItm = GetForm.Items.Item("12").Specific
            taxDateItm = GetForm.Items.Item("46").Specific

        End Sub
        '------------------------------------------------------------------------------
        '------------------------------------------------------------------------------
        Public Overrides Sub SetFilter()
            MyBase.SetFilter()

            parent.AddFormEventFilters(GetFormType, New SAPbouiCOM.BoEventTypes() {SAPbouiCOM.BoEventTypes.et_VALIDATE,
                                                                                   SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST,
                                                                                   SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED})

        End Sub
        '----------------------------------------------------------------------------------------
        '----------------------------------------------------------------------------------------
        Private Sub BPSystemForm_OnFormDataEvent(ByRef objData As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles MyBase.OnFormDataEvent


            If objData.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD And Not objData.BeforeAction AndAlso objData.ActionSuccess Then
                '-------------------------------------------------------------------------------
                ' Caricamento
                '-------------------------------------------------------------------------------
                BlockEventi()

                Try

                    loadDocEntry = GetDataEventKey(objData)

                Catch ex As Exception
                    Log(ex)
                End Try

                ResumeEventi()

            ElseIf objData.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD AndAlso objData.BeforeAction AndAlso objData.Type <> "112" Then
                '-------------------------------------------------------------------------------
                ' Aggiunta di un documento non bozza
                '-------------------------------------------------------------------------------
                BlockEventi()

                Try

                Catch ex As Exception
                    Log(ex)
                End Try

                ResumeEventi()

            ElseIf (objData.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD OrElse objData.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) And Not objData.BeforeAction AndAlso objData.ActionSuccess Then
                '-------------------------------------------------------------------------------
                ' Aggiunta / modifica
                '-------------------------------------------------------------------------------
                BlockEventi()

                Try


                    loadDocEntry = GetDataEventKey(objData)

                Catch ex As Exception
                    Log(ex)
                End Try

                ResumeEventi()

            End If

        End Sub
        '------------------------------------------------------------------------------------------------------
        '------------------------------------------------------------------------------------------------------
        Private Sub ArticoliSystemForm_OnMenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Handles Me.OnMenuEvent

            If pVal.BeforeAction Then Return

            Select Case pVal.MenuUID

                Case SBOConstant.MENU_NEW_RECORD
                    loadDocEntry = -1

                Case SBOConstant.MENU_FIND_RECORD
                    loadDocEntry = -1

            End Select

        End Sub

    End Class

End Namespace
