CREATE PROCEDURE [dbo].[Create_Movie]
    @Title NVARCHAR(50) = NULL,            
    @Genre NVARCHAR(50) = NULL,                      
    @ReleaseYear INT,                       
    @DurationMinutes INT     
AS
	BEGIN
    INSERT INTO [dbo].[Movie] ([Title], [Genre], [ReleaseYear],[DurationMinutes])
	VALUES (@Title,@Genre,@ReleaseYear,@DurationMinutes);
    END
