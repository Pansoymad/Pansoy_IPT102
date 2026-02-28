CREATE PROCEDURE [dbo].[Update_Movie]
	@MovieID INT,  
    @Title NVARCHAR(50) = NULL,            
    @Genre NVARCHAR(50) = NULL,                      
    @ReleaseYear INT,                       
    @DurationMinutes INT     
AS
	BEGIN
    UPDATE [dbo].[Movie]
	SET 
		[Title] = @Title,
		[Genre] = @Genre,
		[ReleaseYear] = @ReleaseYear,
		[DurationMinutes] = @DurationMinutes
	WHERE [MovieID] = @MovieID;
    END
