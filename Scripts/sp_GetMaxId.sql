USE [Bollettari]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetMaxId]    Script Date: 05/21/2013 11:52:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Pierpaolo Francocci
-- Create date: 31/03/2010
-- Description:	Serve ad incrementare i campi chiavi 
-- di ogni tabella contenuti in tabella MaxId
-- =============================================
ALTER PROCEDURE [dbo].[sp_GetMaxId]
	(
	@TableName varchar(50),
	@FieldName varchar(50),
	@MaxId int OUT
	)
AS
BEGIN
	SET @MaxId=-1
	BEGIN TRANSACTION 
	 
	
	IF (SELECT COUNT(*) FROM MaxId 
	WHERE 
	NomeTabella =@TableName 
	And NomeCampo =@FieldName )=0
	Insert MaxId (NomeTabella ,NomeCampo ,MaxValore )
	values (@TableName , @FieldName ,0)

	SELECT @MaxId = coalesce(MaxValore, 0) + 1 FROM MaxId (UPDLOCK)
	WHERE 
	NomeTabella =@TableName 
	And NomeCampo =@FieldName 
	
	Update MaxId Set MaxValore =MaxValore +1
	WHERE 
	NomeTabella =@TableName 
	And NomeCampo =@FieldName 
	 
	
	
	IF @@TRANCOUNT > 0 
		COMMIT TRANSACTION 
	ELSE 
		ROLLBACK TRANSACTION 	
	
	return 

	
	
 
	--select @count = @@rowcount

 

END
