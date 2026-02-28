CREATE TABLE Movie (
    MovieID INT IDENTITY(1,1) PRIMARY KEY,  
    Title NVARCHAR(50) NULL,            
    Genre NVARCHAR(50),                      
    ReleaseYear INT,                       
    DurationMinutes INT                    
);