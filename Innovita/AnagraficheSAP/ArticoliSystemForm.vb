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

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Sub New(ByRef parent As SBOBase.SBOAddOn, ByRef UID As String, f As SAPbouiCOM.Form)
            MyBase.New(parent, UID, f)


            oitmDb = GetForm.DataSources.DBDataSources.Item("OITM")

        End Sub
        '--------------------------------------------------------------------------
        ' Imposta i filtri per gli eventi.
        '--------------------------------------------------------------------------
        Public Overrides Sub SetFilter()
            parent.AddFormEventFilters(GetFormType, New SAPbouiCOM.BoEventTypes() _
                {
                    SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD,
                    SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD,
                    SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE,
                    SAPbouiCOM.BoEventTypes.et_CLICK,
                    SAPbouiCOM.BoEventTypes.et_COMBO_SELECT,
                    SAPbouiCOM.BoEventTypes.et_FORM_RESIZE,
                    SAPbouiCOM.BoEventTypes.et_VALIDATE
                })
        End Sub

    End Class

End Namespace
