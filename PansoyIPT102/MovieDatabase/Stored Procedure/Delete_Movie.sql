CREATE PROCEDURE [dbo].[Delete_Movie]
	@MovieID INT
AS
BEGIN
	DELETE FROM [dbo].[Movie] WHERE MovieID = @MovieID
END