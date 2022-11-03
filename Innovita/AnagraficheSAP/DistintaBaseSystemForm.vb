Imports RglFwk
Imports RglFwk.SBOBase
Imports RglFwk.SBOBase.Util
Imports SAPbouiCOM
Imports System.Collections.Generic
Imports System.Linq


Namespace AnagraficheSAP

    Public Class DistintaBaseSystemForm
        Inherits SBOBase.SBOSystemForm


        Private mtx As SAPbouiCOM.Matrix
        Private oitt As SAPbouiCOM.DBDataSource

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Sub New(ByRef parent As SBOBase.SBOAddOn, ByRef UID As String, f As SAPbouiCOM.Form)
            MyBase.New(parent, UID, f)
            mtx = GetForm.Items.Item("3").Specific
            oitt = GetForm.DataSources.DBDataSources.Item("OITT")
        End Sub

#Region "Tasto destro"

        '--------------------------------------------------------------------------------------------------
        '--------------------------------------------------------------------------------------------------
        Public Sub MyRightClickEvent(ByRef eventInfo As SAPbouiCOM.ContextMenuInfo, ByRef BubbleEvent As Boolean) Handles MyBase.OnRightClick

            If eventInfo.BeforeAction AndAlso (GetForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE OrElse GetForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE OrElse GetForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                Dim oMenuItem As SAPbouiCOM.MenuItem
                Dim oMenus As SAPbouiCOM.Menus
                Dim oCreationPackage As SAPbouiCOM.MenuCreationParams

                Try


                    oMenuItem = parent.GetAppl.Menus.Item("1280") 'Dati
                    oMenus = oMenuItem.SubMenus

                    If Not parent.GetAppl.Menus.Exists("RGL_INNO_DB_00") Then
                        oCreationPackage = parent.GetAppl.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)
                        oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP
                        oCreationPackage.UniqueID = "RGL_INNO_DB_00"
                        oCreationPackage.String = "Innovita"
                        oCreationPackage.Position = 1
                        oCreationPackage.Enabled = True
                        oMenus.AddEx(oCreationPackage)
                    End If

                    oMenus = parent.GetAppl.Menus.Item("RGL_INNO_DB_00").SubMenus

                    If Not parent.GetAppl.Menus.Exists("RGL_INNO_DB_02") Then
                        oCreationPackage = parent.GetAppl.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)
                        oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
                        oCreationPackage.UniqueID = "RGL_INNO_DB_02"
                        oCreationPackage.Position = 10
                        oCreationPackage.String = "Crea nuova revisione"
                        oCreationPackage.Enabled = True
                        oMenus.AddEx(oCreationPackage)
                    End If

                    If mtx.Columns.Item(mtx.GetCellFocus.ColumnIndex).UniqueID = "1" AndAlso Not parent.GetAppl.Menus.Exists("RGL_INNO_DB_01") Then
                        oCreationPackage = parent.GetAppl.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)
                        oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING
                        oCreationPackage.UniqueID = "RGL_INNO_DB_01"
                        oCreationPackage.Position = 20
                        oCreationPackage.String = "Inserisci revisione"
                        oCreationPackage.Enabled = True
                        oMenus.AddEx(oCreationPackage)
                    End If

                Catch ex As Exception
                    SBOBase.Util.Log(ex.Message)
                End Try
            Else
                Try
                    If parent.GetAppl.Menus.Exists("RGL_INNO_DB_00") Then parent.GetAppl.Menus.RemoveEx("RGL_INNO_DB_00")
                Catch ex As Exception
                    'SBOBase.Util.Log(ex.Message)
                End Try
            End If

        End Sub
        '--------------------------------------------------------------------------------------------------
        '--------------------------------------------------------------------------------------------------
        Private Sub DefinizioneGDOForm_OnMenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean) Handles Me.OnMenuEvent


            Select Case pVal.MenuUID

                Case "RGL_INNO_DB_01"
                    '---------------------------------------------------------------------
                    ' Inserisce la riga con la revisione di base
                    '---------------------------------------------------------------------
                    Dim itemCode = oitt.GetValue("Code", 0).Trim

                    Try
                        Dim revCode = Helper.CreaRevisioneBase(itemCode)

                        Dim row = mtx.GetCellFocus().rowIndex

                        Dim edt As SAPbouiCOM.EditText = mtx.GetCellSpecific("1", row)
                        edt.Value = revCode

                    Catch ex As Exception
                        parent.ShowError("$error$", ex)
                    End Try

                    BubbleEvent = False

                Case "RGL_INNO_DB_02"
                    '---------------------------------------------------------------------
                    ' Crea una nuova revisione
                    '---------------------------------------------------------------------
                    Dim itemCode = oitt.GetValue("Code", 0).Trim

                    Try
                        Dim revCode = Helper.CreaRevisione(itemCode)

                        parent.GetAppl.OpenForm(BoFormObjectEnum.fo_ProductTree, "66", revCode)

                    Catch ex As Exception
                        parent.ShowError("$error$", ex)
                    End Try

                    BubbleEvent = False

            End Select

        End Sub

#End Region

    End Class

End Namespace
