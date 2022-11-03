Imports RglFwk
Imports RglFwk.SBOBase
Imports RglFwk.SBOBase.Util
Imports SAPbouiCOM
Imports System.Collections.Generic
Imports System.Linq


Namespace AnagraficheSAP

    Public Class ArticoliSystemForm
        Inherits SBOBase.SBOSystemForm


        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Private oitmDb As SAPbouiCOM.DBDataSource

        Private genFld As SAPbouiCOM.Item

        Private uids As New Generic.SortedList(Of Integer, SAPbouiCOM.Item)
        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Sub New(ByRef parent As SBOBase.SBOAddOn, ByRef UID As String, f As SAPbouiCOM.Form)
            MyBase.New(parent, UID, f)

            ' FAMIGLIA SELEZIONATA INIZIALMENTE
            oitmDb = GetForm.DataSources.DBDataSources.Item("OITM")

            ' PRIMA FOLDER
            genFld = GetForm.Items.Item("163")

            ' ELENCO DELLE FAMIGLIE E FOLDER COLLEGATI
            Dim rs As New SBORecordset
            rs.DoQuery($"SELECT ""ItmsGrpCod"", ""U_RGL_INNO_IDF""  FROM ""OITB""")
            If rs.EoF Then Return
            While Not rs.EoF
                Try
                    uids.Add(rs.AsInteger("ItmsGrpCod"), GetForm.Items.Item(rs.AsString("U_RGL_INNO_IDF")))
                Catch ex As Exception
                    Log(String.Format(InnovitaResource.Messaggi.ImpossibileRecuperareFolderFamiglia, rs.AsInteger("ItmsGrpCod").ToString()))
                End Try

                rs.MoveNext()
            End While
        End Sub

        '--------------------------------------------------------------------------
        ' Imposta i filtri per gli eventi.
        '--------------------------------------------------------------------------
        Public Overrides Sub SetFilter()
            parent.AddFormEventFilters(GetFormType, New SAPbouiCOM.BoEventTypes() _
                {
                    SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD,
                    SAPbouiCOM.BoEventTypes.et_COMBO_SELECT,
                    SAPbouiCOM.BoEventTypes.et_VALIDATE
                })
        End Sub

        '-------------------------------------------------------------------------------------
        ' LOAD DELLA FORM
        '-------------------------------------------------------------------------------------

        Private Sub ArticoliSystemForm_OnFormDataEvent(ByRef objData As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles Me.OnFormDataEvent
            If Not objData.BeforeAction AndAlso objData.EventType = BoEventTypes.et_FORM_DATA_LOAD Then
                Log("ArticoliSystemForm_OnFormDataEvent")

                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------

                BlockEventi()
                Try

                    GestioneCampiUtente()

                Catch ex As Exception
                    Log(ex)
                End Try

                ResumeEventi()

            End If


        End Sub

        '--------------------------------------------------------------------------
        ' CAMBIO DELLA FAMIGLIA
        '--------------------------------------------------------------------------

        Private Sub ArticoliSystemForm_OnItemEvent(ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Handles Me.OnItemEvent
            If Not pVal.BeforeAction AndAlso pVal.ItemUID = "39" AndAlso pVal.EventType = BoEventTypes.et_COMBO_SELECT Then
                Log("ArticoliSystemForm_OnItemEvent")

                BlockEventi()

                Try
                    GestioneCampiUtente()
                Catch ex As Exception
                    Log(ex)
                End Try

                ResumeEventi()
            End If
        End Sub

        '--------------------------------------------------------------------------
        '--------------------------------------------------------------------------

        Private Sub GestioneCampiUtente()
            Try
                Dim itmsGrpCod As Integer = oitmDb.GetValue("ItmsGrpCod", 0)
                Log("FAMIGLIA SELEZIONATA " + itmsGrpCod.ToString())

                genFld.Click()

                For Each f In uids
                    f.Value.Visible = (f.Key = itmsGrpCod)
                Next

            Catch ex As Exception
                Log("GestioneCampiUtente")
                Log(ex)
            End Try
        End Sub

    End Class

End Namespace
