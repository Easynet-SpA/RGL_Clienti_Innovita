----------------------------------------------------
----------------------------------------------------
DROP VIEW RGL_APP_OWHS;
--GO
CREATE VIEW RGL_APP_OWHS
AS
SELECT
		  T0."WhsCode"
		, T0."WhsName"
		, T0."BinActivat"
FROM OWHS AS T0
WHERE T0."DropShip" = 'N' AND T0."Inactive" = 'N'
;
----------------------------------------------------
----------------------------------------------------
DROP VIEW RGL_APP_OITM;
--GO
CREATE VIEW RGL_APP_OITM
AS
SELECT
		  T0."ItemCode"
		, T0."ItemName"
		, T0."ManBtchNum"
FROM OITM AS T0
WHERE T0."InvntItem" = 'Y'
;
--GO
----------------------------------------------------
----------------------------------------------------
DROP VIEW RGL_APP_OBIN;
--GO
CREATE VIEW RGL_APP_OBIN
AS
SELECT 
		  T0."AbsEntry"
		, T0."BinCode"
		, T0."WhsCode"
		, T0."SL1Code"
		, T0."SL2Code"
		, T0."SL3Code"
		, T0."SL4Code"
		, T0."BarCode"
		, T0."AltSortCod"

FROM OBIN AS T0
WHERE T0."Disabled" = 'N'
;
----------------------------------------------------
----------------------------------------------------
DROP VIEW RGL_APP_OBTN;
--GO
CREATE VIEW RGL_APP_OBTN
AS
SELECT
		  T2."ItemCode"
		, T2."ItemName"
		, T0."DistNumber"
		, T0."MnfSerial"
		, T0."LotNumber"
		, T0."SysNumber"
		, T0."AbsEntry"
FROM RGL_APP_OITM AS T2
	 JOIN OBTN    AS T0 ON T2."ItemCode" = T0."ItemCode"
;
--GO
----------------------------------------------------
----------------------------------------------------
DROP VIEW RGL_APP_OBTQ;
--GO
CREATE VIEW RGL_APP_OBTQ
AS
SELECT 
		  T0."ItemCode"
		, T0."ItemName"
		, T0."DistNumber"
		, T0."MnfSerial"
		, T0."LotNumber"
		, T0."SysNumber"
		, T0."AbsEntry"
		, T1."Quantity"
		, T1."CommitQty"
		, T1."WhsCode"

FROM RGL_APP_OBTN      AS T0
	 JOIN OBTQ         AS T1 ON T0."SysNumber" = T1."SysNumber" AND T0."ItemCode" = T1."ItemCode"
	 JOIN RGL_APP_OWHS AS T2 ON T2."WhsCode" = T1."WhsCode"
WHERE T1."Quantity" > 0
;
----------------------------------------------------
----------------------------------------------------
DROP VIEW RGL_APP_OBBQ;
--GO
CREATE VIEW RGL_APP_OBBQ
AS
SELECT 
		  T0."ItemCode"
		, T0."ItemName"
		, T0."DistNumber"
		, T0."SysNumber"
		, T1."WhsCode"
		, T2."OnHandQty"
		, T3."BinCode"
		, T2."BinAbs"
		, T3."AltSortCod"
		, T3."BarCode"
FROM RGL_APP_OBTN      AS T0 	  
	 JOIN OBTQ         AS T1 ON T0."ItemCode" = T1."ItemCode" AND T0."SysNumber" = T1."SysNumber" AND T1."Quantity" > 0 	  
	 JOIN OBBQ         AS T2 ON T2."ItemCode" = T0."ItemCode" AND T2."SnBMDAbs" = T0."AbsEntry" AND T2."WhsCode" = T1."WhsCode" 	  
	 JOIN RGL_APP_OBIN AS T3 ON T3."AbsEntry" = T2."BinAbs"      
WHERE T2."OnHandQty" > 0
;
---GO
----------------------------------------------------
----------------------------------------------------
DROP VIEW RGL_APP_OITW;
--GO
CREATE VIEW RGL_APP_OITW
AS
SELECT 
		  T0."ItemCode"
		, T0."ItemName"
		, T1."WhsCode"
		, T1."OnHand"
FROM RGL_APP_OITM      AS T0 	  
	 JOIN OITW         AS T1 ON T0."ItemCode" = T1."ItemCode"
	 JOIN RGL_APP_OWHS AS T2 ON T2."WhsCode" = T1."WhsCode"
WHERE T1."OnHand" > 0
;
---GO
----------------------------------------------------
----------------------------------------------------
DROP VIEW RGL_APP_OIBQ;
--GO
CREATE VIEW RGL_APP_OIBQ
AS
SELECT 
		  T0."ItemCode"
		, T0."ItemName"
		, T1."WhsCode"
		, T1."OnHandQty"
		, T2."BinCode"
		, T1."BinAbs"
		, T2."AltSortCod"
		, T2."BarCode"
FROM RGL_APP_OITM      AS T0 	  
	 JOIN OIBQ         AS T1 ON T0."ItemCode" = T1."ItemCode"
	 JOIN RGL_APP_OBIN AS T2 ON T2."AbsEntry" = T1."BinAbs"
WHERE T1."OnHandQty" > 0
;
--GO
