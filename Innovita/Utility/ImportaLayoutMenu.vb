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

    Public Class ImportaLayoutMenu
        Inherits SBOBase.SBOMenu

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Sub New(ByRef parent As SBOBase.SBOAddOn)
            MyBase.New(parent, "RGL_INNO_18")
        End Sub
        '-------------------------------------------------------------------------------------------
        ' Richiamata all'esecuzione del menu.
        '-------------------------------------------------------------------------------------------
        Public Overrides Sub Execute()

            Dim basePath As String = System.IO.Path.Combine(parent.GetCompany.WordDocsPath, "Layout")

            If parent.GetAppl.MessageBox("Importare tutti i layout di vendite e acquisti dalla cartella " & basePath & " ?", 2, "Si", "No") = 1 Then
                '-------------------------------------------------------------------------------------------
                ' Import di tutto
                '-------------------------------------------------------------------------------------------
                Utility.ImportaLayoutMenu.ImportaCicloAttivo(basePath)
                Utility.ImportaLayoutMenu.ImportaCicloPassivo(basePath)

                parent.GetAppl.MessageBox("Layout importati con successo")

                Return
            End If

            parent.GetAppl.MessageBox("Layout non importati")

        End Sub
        '-----------------------------------------------------------
        '-----------------------------------------------------------
        Public Shared Sub ImportaCicloAttivo(rptFilePath As String)

            Try

                Dim oCmpSrv As SAPbobsCOM.CompanyService
                Dim oReportLayoutService As ReportLayoutsService

                'Get report layout service
                oCmpSrv = SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetCompanyService
                oReportLayoutService = oCmpSrv.GetBusinessService(ServiceTypes.ReportLayoutsService)

                AddReport(GetReportFile(rptFilePath, New String() {"A_Consegna"}), oCmpSrv, oReportLayoutService, "A_Consegna - Servizio", "DLN1")
                AddReport(GetReportFile(rptFilePath, New String() {"A_Consegna"}), oCmpSrv, oReportLayoutService, "A_Consegna", "DLN2")

                AddReport(GetReportFile(rptFilePath, New String() {"A_Consegna", "A_Reso"}), oCmpSrv, oReportLayoutService, "A_Reso - Servizio", "RDN1")
                AddReport(GetReportFile(rptFilePath, New String() {"A_Consegna", "A_Reso"}), oCmpSrv, oReportLayoutService, "A_Reso", "RDN2")

                AddReport(GetReportFile(rptFilePath, New String() {"A_Fattura"}), oCmpSrv, oReportLayoutService, "A_Fattura - Servizio", "INV1")
                AddReport(GetReportFile(rptFilePath, New String() {"A_Fattura"}), oCmpSrv, oReportLayoutService, "A_Fattura", "INV2")

                AddReport(GetReportFile(rptFilePath, New String() {"A_Fattura", "A_Nota credito"}), oCmpSrv, oReportLayoutService, "A_Nota credito - Servizio", "RIN1")
                AddReport(GetReportFile(rptFilePath, New String() {"A_Fattura", "A_Nota credito"}), oCmpSrv, oReportLayoutService, "A_Nota credito", "RIN2")

                AddReport(GetReportFile(rptFilePath, New String() {"A_Offerta", "A_Ordine"}), oCmpSrv, oReportLayoutService, "A_Offerta - Servizio", "QUT1")
                AddReport(GetReportFile(rptFilePath, New String() {"A_Offerta", "A_Ordine"}), oCmpSrv, oReportLayoutService, "A_Offerta", "QUT2")

                AddReport(GetReportFile(rptFilePath, New String() {"A_Ordine", "A_Offerta"}), oCmpSrv, oReportLayoutService, "A_Ordine cliente - Servizio", "RDR1")
                AddReport(GetReportFile(rptFilePath, New String() {"A_Ordine", "A_Offerta"}), oCmpSrv, oReportLayoutService, "A_Ordine cliente", "RDR2")

                AddReport(GetReportFile(rptFilePath, New String() {"A_Trasferimento", "A_Consegna"}), oCmpSrv, oReportLayoutService, "A_Trasferimento", "WTR1")

                AddReport(GetReportFile(rptFilePath, New String() {"A_Ordine produzione"}), oCmpSrv, oReportLayoutService, "A_Ordine produzione", "WOR1")

            Catch ex As Exception
                SBOBase.SBOMain.GetAddOnInstance.ShowError("Impossibile importare i layout delle vendite. $error$", ex)
            End Try

        End Sub
        '-----------------------------------------------------------
        '-----------------------------------------------------------
        Public Shared Sub ImportaCicloPassivo(rptFilePath As String)


            Try

                Dim oCmpSrv As SAPbobsCOM.CompanyService
                Dim oReportLayoutService As ReportLayoutsService

                'Get report layout service
                oCmpSrv = SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetCompanyService
                oReportLayoutService = oCmpSrv.GetBusinessService(ServiceTypes.ReportLayoutsService)

                AddReport(GetReportFile(rptFilePath, New String() {"A_Ordine acquisto"}), oCmpSrv, oReportLayoutService, "A_Ordine acquisto - Servizio", "POR1")
                AddReport(GetReportFile(rptFilePath, New String() {"A_Ordine acquisto"}), oCmpSrv, oReportLayoutService, "A_Ordine acquisto", "POR2")

                'AddReport(GetReportFile(rptFilePath, New String() {"A_Entrata merce"}), oCmpSrv, oReportLayoutService, "A_Entrata merce", "PDN2")

                AddReport(GetReportFile(rptFilePath, New String() {"A_Reso fornitore", "A_Consegna"}), oCmpSrv, oReportLayoutService, "A_Reso fornitore - Servizio", "RPD1")
                AddReport(GetReportFile(rptFilePath, New String() {"A_Reso fornitore", "A_Consegna"}), oCmpSrv, oReportLayoutService, "A_Reso fornitore", "RPD2")

            Catch ex As Exception
                SBOBase.SBOMain.GetAddOnInstance.ShowError("Impossibile importare i layout degli acquisti. $error$", ex)
            End Try

        End Sub
        '-----------------------------------------------------------
        '-----------------------------------------------------------
        Private Shared Function GetReportFile(basePath As String, rpt() As String) As String
            Dim ret As String

            For Each r In rpt
                ret = System.IO.Path.Combine(basePath, r & ".rpt")
                If System.IO.File.Exists(ret) Then Return ret
            Next


            Return System.IO.Path.Combine(basePath, "A_Documento.rpt")
        End Function
        '-------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------
        Private Shared Function AddReport(rptFilePath As String, oCmpSrv As CompanyService, oReportLayoutService As ReportLayoutsService, name As String, typeCode As String) As String


            If System.IO.File.Exists(rptFilePath) = False Then
                Util.Log("Non trovato report " & rptFilePath & " per tipo " & typeCode)
                Return ""
            End If


            Dim rs As SAPbobsCOM.Recordset = SBOBase.SBOMain.GetAddOnInstance.NewRecordSet
            Dim oReport As ReportLayout = oReportLayoutService.GetDataInterface(ReportLayoutsServiceDataInterfaces.rlsdiReportLayout)
            Dim newReportCode As String


            rs.DoQuery("SELECT ""DocCode"" FROM RDOC WHERE ""DocName"" = " & toSQLString(name))
            If rs.EoF Then

                oReport.Name = name
                oReport.TypeCode = typeCode
                oReport.Author = SBOBase.SBOMain.GetAddOnInstance.GetCompany.UserName
                oReport.Category = ReportLayoutCategoryEnum.rlcCrystal

                Dim oNewReportParams As ReportLayoutParams = oReportLayoutService.AddReportLayout(oReport)
                newReportCode = oNewReportParams.LayoutCode
            Else
                newReportCode = rs.Fields.Item("DocCode").Value
            End If


            Dim oBlobParams As BlobParams = oCmpSrv.GetDataInterface(SAPbobsCOM.CompanyServiceDataInterfaces.csdiBlobParams)
            oBlobParams.Table = "RDOC"
            oBlobParams.Field = "Template"

            Dim oKeySegment As BlobTableKeySegment = oBlobParams.BlobTableKeySegments.Add()
            oKeySegment.Name = "DocCode"
            oKeySegment.Value = newReportCode

            Dim oBlob As Blob = oCmpSrv.GetDataInterface(CompanyServiceDataInterfaces.csdiBlob)

            Dim buf = System.IO.File.ReadAllBytes(rptFilePath)

            oBlob.Content = Convert.ToBase64String(buf, 0, buf.Length)

            oCmpSrv.SetBlob(oBlobParams, oBlob)

            Return newReportCode
        End Function
        '------------------------------------------------------
        '------------------------------------------------------
        Public Shared Function CreaSequenzaStampa(nome As String, objType As String, subObjType As String) As Integer
            Dim id As Integer


            Try
                Dim rs As SAPbobsCOM.Recordset = SBOBase.SBOMain.GetAddOnInstance.NewRecordSet


                rs.DoQuery("SELECT ""SeqID"" FROM RPRS WHERE ""SeqName"" = '" & nome & "' AND ""ObjectID"" = '" & objType & "' AND ""SubDocType"" = '" & subObjType & "'")
                If Not rs.EoF Then
                    id = rs.Fields.Item("SeqID").Value
                    Util.Log("Sequenza non creata perche' gia' presente con id " & id & ". Cancello il contenuto.")
                    rs.DoQuery("DELETE FROM PRS1 WHERE ""SeqID"" = " & id)
                    Return id
                End If

                rs.DoQuery("SELECT MAX(""SeqID"") AS ""SeqID"" FROM RPRS")
                id = rs.Fields.Item("SeqID").Value + 1

                rs.DoQuery("INSERT INTO RPRS(""SeqID"", ""SeqName"", ""LineNum"", ""ObjectID"", ""SubDocType"") " &
                           "SELECT " & id & ", '" & nome & "', NULL, '" & objType & "', '" & subObjType & "'")


                rs.DoQuery("UPDATE ONNM SET ""AutoKey"" = (SELECT MAX(""SeqID"") FROM RPRS) + 1 WHERE ""ObjectCode"" = '410000002'")

            Catch ex As Exception
                SBOBase.SBOMain.GetAddOnInstance.ShowError("Impossibile creare la sequenza di stampa. $error$", ex)
                Return -1
            End Try

            Return id
        End Function
        '------------------------------------------------------
        '------------------------------------------------------
        Public Shared Sub AggiungiASequenzaStampa(seqId As Integer, layoutName As String, copie As Integer)

            Try
                Dim rs As SAPbobsCOM.Recordset = SBOBase.SBOMain.GetAddOnInstance.NewRecordSet

                rs.DoQuery("SELECT ""SubDocType"", ""ObjectID"" FROM RPRS WHERE ""SeqId"" = " & seqId)
                Dim SubDocType As String = rs.Fields.Item("SubDocType").Value
                Dim ObjectID As String = rs.Fields.Item("ObjectID").Value

                rs.DoQuery("SELECT ""DocCode"" FROM RDOC WHERE ""DocName"" = '" & layoutName & "'")
                Dim layoutCode As String = rs.Fields.Item("DocCode").Value

                rs.DoQuery("SELECT MAX(""LineNum"") AS ""LineNum"" FROM PRS1 WHERE ""SeqId"" = " & seqId)
                Dim lineNum As Integer = If(rs.EoF, 1, rs.Fields.Item("LineNum").Value + 1)

                rs.DoQuery("INSERT INTO PRS1(""SeqID"", ""LineNum"", ""ObjectID"", ""LaytCode"", ""NumCopy"", ""UsrQuery"", ""SubDocType"", ""Printer"", ""Prtr1st"", ""Use1stPrtr"") " &
                           " VALUES(" & seqId & ", " & lineNum & ", '" & ObjectID & "', '" & layoutCode & "', " & copie &
                           " , NULL, '" & SubDocType & "', '', NULL, 'N')")

            Catch ex As Exception
                SBOBase.SBOMain.GetAddOnInstance.ShowError("Impossibile aggiornare la sequenza di stampa. $error$", ex)
            End Try

        End Sub

    End Class


End Namespace
