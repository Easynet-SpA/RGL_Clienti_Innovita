DROP VIEW RGL_IN_ANALISI_VENDITE;
--GO
-----------------------------------------------
-----------------------------------------------
CREATE VIEW RGL_IN_ANALISI_VENDITE
AS
SELECT

		  T0."DocNum"
		, T0."DocDate"
		, T0."CardCode"
		, T0."CardName"
		, T0."DocTotal"
		, T0."DocTotal" - T0."VatSum" AS "Imponibile"
		, T1."ItemCode"
		, T1."Dscription"
		, T1."Quantity"
		, T1."PriceBefDi"
		, T1."DiscPrcnt"
		, T1."Price"
		, T1."LineTotal"
		, T2."SlpName"
		, T4."ItmsGrpNam"

FROM OINV      AS T0
	 JOIN INV1 AS T1 ON T0."DocEntry" = T1."DocEntry"
	 JOIN OSLP AS T2 ON T2."SlpCode" = T1."SlpCode"
	 JOIN OITM AS T3 ON T3."ItemCode" = T1."ItemCode"
	 JOIN OITB AS T4 ON T4."ItmsGrpCod" = T3."ItmsGrpCod"

WHERE T0."CANCELED" = 'N'
;
