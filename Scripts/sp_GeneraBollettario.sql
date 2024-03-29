USE [Bollettari]
GO
/****** Object:  StoredProcedure [dbo].[sp_GeneraBollettario]    Script Date: 05/21/2013 11:50:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Pierpaolo Francocci
-- Create date: 31/03/2010
-- Description:	Serve a generare la struttura bollettario (singolo o multiplo)-bollette
-- prende in input il @progTipografo e genera @NumBollette progressive partendo da @DaBolletta 
-- Gestisce la variazione di stato in DATA_STATUS_HISTORY 
-- =============================================

ALTER PROCEDURE [dbo].[sp_GeneraBollettario]
	@progTipografo varchar(10),
    @NumBollette int,
    @DaBolletta int,
    @cOP varchar(20),
    @ret int OUTPUT AS
    
    
    DECLARE @IdBollettario int
    DECLARE @IdDataStatus int
    DECLARE @DaBollABoll varchar(50)
    DECLARE @cStatus int
    set @ret =-1
    DECLARE @TransactionName varchar(20) = 'Operazione';
BEGIN TRAN @TransactionName


	BEGIN TRY
    
		--SELECT 1/0;


		execute sp_GetMaxId 'BOLLETTARIO','IdBollettario',@IdBollettario OUTPUT 
		execute sp_GetMaxId 'DATA_STATUS_HISTORY','IdDataStatus',@IdDataStatus OUTPUT 
		
		set @DaBollABoll ='Da '+cast(@DaBolletta as varchar(10)) + 
		' a '+cast((@DaBolletta +@NumBollette-1 )as varchar(10))
		
		INSERT Bollettario (IdBollettario ,progTipografo, NumBollette ,DaBollettaABolletta ,dt_INS,cOP_INS  )
		VALUES (@IdBollettario , @progTipografo, @NumBollette , @DaBollABoll ,GETDATE(),@cOP)
		
		set @ret =@@rowcount
		IF (@ret =1) 
			BEGIN 
			
				set @cStatus =(select idTipodiTabella  from Tipi 
				WHERE cTipo ='BOLLETTARIO_STATO'
				AND Valore ='GENERATO')

				print @IdDataStatus
				print @cStatus
				
				INSERT DATA_STATUS_HISTORY (IdNode,NomeEntita ,IdDataStatus ,cStatus ,dtStatus ,Ricevente ,dt_INS ,cOP_INS )
				VALUES (@IdBollettario , 'BOLLETTARIO',@IdDataStatus, @cStatus ,GETDATE(), 'UFFICIO_UTENTI',GETDATE(),@cOP)
				
				
				declare @pOut int
				execute sp_CreaBollette @IdBollettario ,@DaBolletta ,@NumBollette ,@cOP, @pOut OUTPUT 
				
				if (@pOut <>@NumBollette )
				BEGIN
					ROLLBACK TRAN @TransactionName
				END
				ELSE 
					set @ret =0
			END
			ELSE
				ROLLBACK TRAN @TransactionName
		
	END TRY	
	BEGIN CATCH
    DECLARE @ERR int
    
    select @ERR=@@ERROR  
    if @ERR>0
    begin
		ROLLBACK TRAN @TransactionName
		
		DECLARE @ERROR_MESSAGE nvarchar(2048)
		DECLARE @ERROR_NUMBER int
		DECLARE @ERROR_SEVERITY int 
		DECLARE @ERROR_STATE int
		DECLARE @ERROR_PROCEDURE nvarchar(255)
		DECLARE @ERROR_LINE int 
		
		
		SET @ERROR_MESSAGE = ERROR_MESSAGE()
		SET @ERROR_NUMBER= ERROR_NUMBER()
		SET @ERROR_SEVERITY= ERROR_SEVERITY()
		SET @ERROR_STATE= ERROR_STATE()
		SET @ERROR_PROCEDURE= ERROR_PROCEDURE()
		SET @ERROR_LINE= ERROR_LINE()
		
		set @ret=@ERR	
		exec usp_LoggingGenericError @ERROR_MESSAGE ,@ERROR_NUMBER, @ERROR_SEVERITY , @ERROR_STATE,@ERROR_PROCEDURE,@ERROR_LINE,@cOP
	end
END CATCH;


	
	
	
	
	
IF @@TRANCOUNT > 0	
COMMIT TRAN @TransactionName

