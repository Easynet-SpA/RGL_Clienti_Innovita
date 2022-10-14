Imports RglFwk
Imports RglFwk.SBOBase
Imports RglFwk.SBOBase.Util
Imports SAPbouiCOM
Imports System.Collections.Generic
Imports System.Linq

Namespace AnagraficheSAP


    '------------------------------------------------------------------------
    '------------------------------------------------------------------------
    Public Class Helper


        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Shared Function CreaRevisioneBase(itemCode As String) As String
            Dim rev As Integer = Config.RevisioneBase.Valore
            Dim revItemCode = $"{itemCode}.{rev:00}"

            Dim rs As New SBORecordset

            rs.DoQuery($"SELECT ""ItemCode"" FROM OITM WHERE ""ItemCode"" = '{revItemCode}'")
            If rs.EoF Then
                Dim oitm As SAPbobsCOM.Items = SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

                rs.DoQuery($"SELECT ""ItemName"" FROM OITM WHERE ""ItemCode"" = '{itemCode}'")

                oitm.ItemCode = revItemCode
                oitm.ItemName = $"{rs.AsString("ItemName")} - rev. {rev:#}"
                oitm.IsPhantom = SAPbobsCOM.BoYesNoEnum.tYES
                oitm.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO
                oitm.SalesItem = SAPbobsCOM.BoYesNoEnum.tNO
                oitm.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tNO

                If oitm.Add <> 0 Then Throw New Exception(String.Format(InnovitaResource.Messaggi.ImpossibileCreareArticolo, revItemCode, SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetLastErrorDescription))
            End If

            rs.DoQuery($"SELECT ""Code"" FROM OITT WHERE ""Code"" = '{revItemCode}'")
            If rs.EoF Then

                Dim bom As SAPbobsCOM.ProductTrees = SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees)

                bom.TreeCode = revItemCode

                If bom.Items.Count = 0 OrElse bom.Items.ItemCode <> "" OrElse bom.Items.LineText <> "" Then bom.Items.Add()
                bom.Items.ItemType = SAPbobsCOM.ProductionItemType.pit_Text
                bom.Items.LineText = $"REV. {rev:#} - {Today:dd/MM/yyyy}"

                If bom.Items.Count = 0 OrElse bom.Items.ItemCode <> "" OrElse bom.Items.LineText <> "" Then bom.Items.Add()
                bom.Items.ItemType = SAPbobsCOM.ProductionItemType.pit_Item
                bom.Items.ItemCode = "PLACEHOLDER"

                If bom.Add <> 0 Then Throw New Exception(String.Format(InnovitaResource.Messaggi.ImpossibileCreareBom, revItemCode, SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetLastErrorDescription))
            End If


            Return revItemCode
        End Function

        '-------------------------------------------------------------------------------------------
        '-------------------------------------------------------------------------------------------
        Public Shared Function CreaRevisione(itemCode As String) As String

            Dim rev As Integer = 0
            Dim srev As String = itemCode.Substring(itemCode.IndexOf(".") + 1)
            Dim baseItemCode As String = itemCode.Substring(0, itemCode.IndexOf("."))

            If Not Integer.TryParse(srev, rev) Then Throw New Exception(String.Format(InnovitaResource.Messaggi.RevisioneNoNumero, srev))

            Dim rs As New SBORecordset

            rs.DoQuery($"SELECT MAX(""ItemCode"") AS ""ItemCode"" FROM OITM WHERE ""ItemCode"" LIKE '{baseItemCode}.__'")
            If Not rs.EoF Then
                srev = rs.AsString("ItemCode").Substring(itemCode.IndexOf(".") + 1)
                If Not Integer.TryParse(srev, rev) Then Throw New Exception(String.Format(InnovitaResource.Messaggi.RevisioneNoNumero, srev))
            End If

            rev += 1
            Dim revItemCode = $"{baseItemCode}.{rev:00}"


            rs.DoQuery($"SELECT ""ItemCode"" FROM OITM WHERE ""ItemCode"" = '{revItemCode}'")
            If rs.EoF Then
                Dim oitm As SAPbobsCOM.Items = SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

                rs.DoQuery($"SELECT ""ItemName"" FROM OITM WHERE ""ItemCode"" = '{baseItemCode}'")

                oitm.ItemCode = revItemCode
                oitm.ItemName = $"{rs.AsString("ItemName")} - rev. {rev:#}"
                oitm.IsPhantom = SAPbobsCOM.BoYesNoEnum.tYES
                oitm.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO
                oitm.SalesItem = SAPbobsCOM.BoYesNoEnum.tNO
                oitm.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tNO

                If oitm.Add <> 0 Then Throw New Exception(String.Format(InnovitaResource.Messaggi.ImpossibileCreareArticolo, revItemCode, SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetLastErrorDescription))
            End If

            rs.DoQuery($"SELECT ""Code"" FROM OITT WHERE ""Code"" = '{revItemCode}'")
            If rs.EoF Then

                Dim bom As SAPbobsCOM.ProductTrees = SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees)

                bom.GetByKey(itemCode)

                SBOBase.SBOMain.GetAddOnInstance.GetCompany.XmlExportType = SAPbobsCOM.BoXmlExportTypes.xet_ExportImportMode
                SBOBase.SBOMain.GetAddOnInstance.GetCompany.XMLAsString = True
                Dim xml = bom.GetAsXML

                xml = xml.Replace(itemCode, revItemCode)

                '-----------------------------------------------------------------------------------------------------------------
                ' Se si lasciano questi campi valorizzati sulle righe di testo va in errore e non crea la bom ... (SAP 10 FP2202)
                '-----------------------------------------------------------------------------------------------------------------
                xml = xml.Replace("<Quantity>0.000000</Quantity>", "").Replace("<AdditionalQuantity>0.000000</AdditionalQuantity>", "").Replace("<Price>0.000000</Price>", "")

                bom = SBOMain.GetAddOnInstance.GetCompany.GetBusinessObjectFromXML(xml, 0)
                bom.Project = "NUOVA"
                bom.Items.SetCurrentLine(0)
                bom.Items.LineText = $"REV. {rev} - {Today:dd/MM/yyyy}"


                If bom.Add <> 0 Then Throw New Exception(String.Format(InnovitaResource.Messaggi.ImpossibileCreareBom, revItemCode, SBOBase.SBOMain.GetAddOnInstance.GetCompany.GetLastErrorDescription))
            End If


            Return revItemCode
        End Function

    End Class

End Namespace