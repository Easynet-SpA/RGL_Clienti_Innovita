DROP PROCEDURE RGL_SP_USR_Innovita;
--GO
-------------------------------------------------------------
-------------------------------------------------------------
CREATE PROCEDURE RGL_SP_USR_Innovita
(
	object_type nvarchar(30), 			-- SBO Object Type
	transaction_type nchar(1),			-- [A]dd, [U]pdate, [D]elete, [C]ancel, C[L]ose
	num_of_cols_in_key int,
	list_of_key_cols_tab_del nvarchar(255),
	list_of_cols_val_tab_del nvarchar(255)
)

AS

BEGIN
	DECLARE N          INT;
	
	

	IF 0 = 1 THEN
		-------------------------------------------------------------------
		-- Serve solo per compilare
		-------------------------------------------------------------------
		CREATE LOCAL TEMPORARY TABLE #Res
		(
			  error INT
			, error_message NVARCHAR(200)
		);
	END IF;


END;
