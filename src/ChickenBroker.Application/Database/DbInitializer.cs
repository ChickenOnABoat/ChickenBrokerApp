using Dapper;

namespace ChickenBroker.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        await connection.ExecuteAsync("""
                                      IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Address')
                                      BEGIN 
                                        CREATE TABLE Address (
                                          Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
                                          ZipCode VARCHAR(15) UNIQUE NOT NULL,
                                          Street VARCHAR(100) NOT NULL,
                                          City VARCHAR(100) NOT NULL,
                                          State VARCHAR(100) NOT NULL,
                                          StateAcronym VARCHAR(100) NOT NULL,
                                          Neighbourhood VARCHAR(100) NOT NULL,
                                        );
                                      END
                                      """);
        
        await connection.ExecuteAsync("""
                                      IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PropertyAgency')
                                      BEGIN 
                                        CREATE TABLE PropertyAgency (
                                          Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
                                          Name VARCHAR(100),
                                          IdentificationDocumentNumber VARCHAR(255),
                                          AddressNumber VARCHAR(30),
                                          AddressComplement VARCHAR(80),
                                          IdAddress UNIQUEIDENTIFIER NOT NULL,
                                          FOREIGN KEY (IdAddress) REFERENCES Address(Id),
                                          ContactNumber VARCHAR(50),
                                        );
                                      END
                                      """);
        //Check if FK Relationship exists between PropertyAgency and Address
        await connection.ExecuteAsync("""
                                      DECLARE @ConstraintName nvarchar(200)
                                      SELECT 
                                          @ConstraintName = KCU.CONSTRAINT_NAME
                                      FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
                                      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU
                                          ON KCU.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
                                          AND KCU.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
                                          AND KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
                                      WHERE
                                          KCU.TABLE_NAME = 'PropertyAgency' AND
                                          KCU.COLUMN_NAME = 'IdAddress';
                                      	
                                      	IF @ConstraintName IS NULL
                                      	BEGIN 
                                      		ALTER TABLE PropertyAgency
                                      		ADD CONSTRAINT FK_Address_IdAddress FOREIGN KEY (IdAddress)
                                      			REFERENCES Address(Id);
                                      	END
                                      """);
        
        
        await connection.ExecuteAsync("""
                                      IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'UserBroker')
                                      BEGIN 
                                        CREATE TABLE [UserBroker] (
                                          Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
                                          Name VARCHAR(100) NOT NULL,
                                          Email VARCHAR(255) NOT NULL,
                                          CurrentPropertyAgencyId UNIQUEIDENTIFIER,
                                          CONSTRAINT AK_Email UNIQUE(Email),
                                          FOREIGN KEY (CurrentPropertyAgencyId) REFERENCES PropertyAgency(Id)
                                        );
                                        
                                      END
                                      """);
        //Check if FK Relationship exists between UserBroker and PropertyAgency
        await connection.ExecuteAsync("""
                                      DECLARE @ConstraintName nvarchar(200)
                                      SELECT 
                                          @ConstraintName = KCU.CONSTRAINT_NAME
                                      FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
                                      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU
                                          ON KCU.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
                                          AND KCU.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
                                          AND KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
                                      WHERE
                                          KCU.TABLE_NAME = 'UserBroker' AND
                                          KCU.COLUMN_NAME = 'CurrentPropertyAgencyId';
                                      	
                                      	IF @ConstraintName IS NULL
                                      	BEGIN 
                                      		ALTER TABLE UserBroker
                                      		ADD CONSTRAINT FK_PropertyAgency_CurrentPropertyAgencyId FOREIGN KEY (CurrentPropertyAgencyId)
                                      			REFERENCES PropertyAgency(Id);
                                      	END
                                      """);

        await connection.ExecuteAsync("""
                                      IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'PropertySold')
                                      BEGIN 
                                        CREATE TABLE PropertySold (
                                          Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
                                          IdUserSold UNIQUEIDENTIFIER default NULL,
                                          IdUserCreator UNIQUEIDENTIFIER NOT NULL,
                                          UserCreatorDidAgencyOfProperty BIT NOT NULL DEFAULT 0,
                                          UserCreatorDidSaleOfProperty BIT NOT NULL DEFAULT 0,
                                          IdPropertyType UNIQUEIDENTIFIER NOT NULL,
                                          IdAddress UNIQUEIDENTIFIER NOT NULL,
                                          FOREIGN KEY (IdAddress) REFERENCES Address(Id),
                                          AddressNumber VARCHAR(100),
                                          AddressComplement VARCHAR(100),
                                          NumberOfBedrooms INT DEFAULT 0,
                                          NumberOfSuites INT DEFAULT 0,
                                          NumberOfBathrooms INT DEFAULT 0,
                                          NumberOfParkingSpots INT DEFAULT 0,
                                          PropertyArea FLOAT NOT NULL,
                                          YearOfConstruction INT NOT NULL,
                                          HasLobby BIT DEFAULT 0,
                                          DateOfSale DATE NOT NULL,
                                          TimeElapsedToSell VARCHAR(50),
                                          AnnouncedValue FLOAT NOT NULL,
                                          SaleValue FLOAT NOT NULL,
                                          CommissionValue FLOAT NOT NULL,
                                          FOREIGN KEY (IdUserSold) REFERENCES UserBroker(Id),
                                          FOREIGN KEY (IdUserCreator) REFERENCES UserBroker(Id)
                                        );
                                      END
                                      """);
        
        //Check if FK Relationship exists between PropertySold and Address
        await connection.ExecuteAsync("""
                                      DECLARE @ConstraintName nvarchar(200)
                                      SELECT 
                                          @ConstraintName = KCU.CONSTRAINT_NAME
                                      FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
                                      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU
                                          ON KCU.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
                                          AND KCU.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
                                          AND KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
                                      WHERE
                                          KCU.TABLE_NAME = 'PropertySold' AND
                                          KCU.COLUMN_NAME = 'IdAddress';
                                      	
                                      	IF @ConstraintName IS NULL
                                      	BEGIN 
                                      		ALTER TABLE PropertySold
                                      		ADD CONSTRAINT FK_Address_IdAddress FOREIGN KEY (IdAddress)
                                      			REFERENCES Address(Id);
                                      	END
                                      """);
        
        //Check if FK Relationship exists between PropertySold and UserBroker (IdUserSold)
        await connection.ExecuteAsync("""
                                      DECLARE @ConstraintName nvarchar(200)
                                      SELECT 
                                          @ConstraintName = KCU.CONSTRAINT_NAME
                                      FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
                                      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU
                                          ON KCU.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
                                          AND KCU.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
                                          AND KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
                                      WHERE
                                          KCU.TABLE_NAME = 'PropertySold' AND
                                          KCU.COLUMN_NAME = 'IdUserSold';
                                      	
                                      	IF @ConstraintName IS NULL
                                      	BEGIN 
                                      		ALTER TABLE PropertySold
                                      		ADD CONSTRAINT FK_UserBroker_IdUserSold FOREIGN KEY (IdUserSold)
                                      			REFERENCES UserBroker(Id);
                                      	END
                                      """);
        
        //Check if FK Relationship exists between PropertySold and UserBroker(IdUserCreator)
        await connection.ExecuteAsync("""
                                      DECLARE @ConstraintName nvarchar(200)
                                      SELECT 
                                          @ConstraintName = KCU.CONSTRAINT_NAME
                                      FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
                                      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU
                                          ON KCU.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
                                          AND KCU.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
                                          AND KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
                                      WHERE
                                          KCU.TABLE_NAME = 'PropertySold' AND
                                          KCU.COLUMN_NAME = 'IdUserCreator';
                                      	
                                      	IF @ConstraintName IS NULL
                                      	BEGIN 
                                      		ALTER TABLE PropertySold
                                      		ADD CONSTRAINT FK_UserBroker_IdUserCreator FOREIGN KEY (IdUserCreator)
                                      			REFERENCES UserBroker(Id);
                                      	END
                                      """);
        
        await connection.ExecuteAsync("""
                                      IF NOT EXISTS (
                                            SELECT
                                                  *
                                            FROM
                                                  INFORMATION_SCHEMA.TABLES
                                            WHERE
                                                  TABLE_NAME = N'PropertyType'
                                      ) BEGIN CREATE TABLE [PropertyType] (
                                            Id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
                                            Name VARCHAR(100) NOT NULL,
                                      );
                                      
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  '717A3E09-7A83-482A-B841-2FAEEB7740C0',
                                                  'House'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  '86670C3C-9CDD-45ED-913C-42CDA79F0145',
                                                  'Apartment'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  'D9CC1D14-E506-4D3A-B201-91ACC4295D47',
                                                  'Rooftop'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  '652FAF6E-11B4-48D5-8366-809B59A75E89',
                                                  'Comercial Space'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  '2775E67D-E639-4B2B-A93C-17E6ACF60679',
                                                  'Lot'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  'B9F72D09-DABF-4812-892C-B35EAD7FE6F6',
                                                  'Others'
                                            )
                                      
                                      END
                                      """);
        //Check if FK Relationship exists between PropertySold and PropertyType
        await connection.ExecuteAsync("""
                                      DECLARE @ConstraintName nvarchar(200)
                                      SELECT 
                                          @ConstraintName = KCU.CONSTRAINT_NAME
                                      FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
                                      INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU
                                          ON KCU.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
                                          AND KCU.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
                                          AND KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
                                      WHERE
                                          KCU.TABLE_NAME = 'PropertySold' AND
                                          KCU.COLUMN_NAME = 'IdPropertyType';
                                      	
                                      	IF @ConstraintName IS NULL
                                      	BEGIN 
                                      		ALTER TABLE PropertySold
                                      		ADD CONSTRAINT FK_PropertyType_IdPropertyType FOREIGN KEY (IdPropertyType)
                                      			REFERENCES PropertyType(Id);
                                      	END
                                      """);
    }
    /* --Template Add Constraint if not exists
		DECLARE @ConstraintName nvarchar(200)
		SELECT 
		    @ConstraintName = KCU.CONSTRAINT_NAME
		FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
		INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU
		    ON KCU.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
		    AND KCU.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
		    AND KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
		WHERE
		    KCU.TABLE_NAME = 'ThisTable' AND
		    KCU.COLUMN_NAME = 'ThisTableColumn';
			
			IF @ConstraintName IS NULL
			BEGIN 
				ALTER TABLE ThisTable
				ADD CONSTRAINT FK_OtherTable_ThisTableColumn FOREIGN KEY (ThisTableColumn)
					REFERENCES OtherTable(OtherTableColumn);
			END
     */
}