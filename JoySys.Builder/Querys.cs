using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoySys.Builder
{
    internal class Querys
    {

        static public string StoredProcedures_UpdateIntegratedModuleRecipeFields()
        {
            return @"

        /*************************************************************************** 
        -- WARNING: This script is auto-generated and must not be modified manually. 
        ****************************************************************************/

        CREATE OR ALTER PROCEDURE [dbo].[UpdateIntegratedModuleRecipeFields]
            @TableName SYSNAME,          
            @Fields NVARCHAR(MAX),      
            @Value NVARCHAR(255),       
            @RecipeID INT,
            @PhaseNum INT
        AS
        BEGIN
            SET NOCOUNT ON;

            DECLARE @sql NVARCHAR(MAX);
            DECLARE @set NVARCHAR(MAX);

            ;WITH SplitFields AS (
                SELECT LTRIM(RTRIM(value)) AS FieldName
                FROM STRING_SPLIT(@Fields, ',')
                WHERE LTRIM(RTRIM(value)) <> ''
            )
            SELECT @set = STRING_AGG(QUOTENAME(FieldName) + ' = @val', ', ')
            FROM SplitFields;

            SET @sql = N'
                UPDATE ' + QUOTENAME(@TableName) + '
                SET ' + @set + '
                WHERE RecipeID = @rid AND PhaseNum = @pid;
            ';

            EXEC sp_executesql 
                @sql,
                N'@val NVARCHAR(255), @rid INT, @pid INT',
                @val = @Value, @rid = @RecipeID, @pid = @PhaseNum;
        END

    ";
        }

        static public string StoredProcedures_UpdateSchema()
        {
            return @"

                   
                    /*************************************************************************** 
                    -- WARNING: This script is auto-generated and must not be modified manually. 
                    ****************************************************************************/

                  
                    CREATE or ALTER PROCEDURE [dbo].[UpdateSchema]
                        @Time DATETIME,
                        @Names NVARCHAR(MAX),   
                        @Types NVARCHAR(MAX) = NULL 
                    AS
                    BEGIN
                        SET NOCOUNT ON;


	                    DECLARE @Timestamp DATETIME=@Time;
                        DECLARE @FieldNames NVARCHAR(MAX)=@Names;   
                        DECLARE @FieldTypes NVARCHAR(MAX) =@Types;



                        DECLARE @YearMonth NVARCHAR(7) = FORMAT(@Timestamp, 'yyyy_MM');
                        DECLARE @DbName NVARCHAR(128) = 'Data_' + @YearMonth; -- 
                        DECLARE @TableName NVARCHAR(128) = 'tbDataCollection';

                        DECLARE @SQL NVARCHAR(MAX);
                        DECLARE @FieldCount INT = LEN(@FieldNames) - LEN(REPLACE(@FieldNames, ',', '')) + 1;
                        DECLARE @i INT = 1;

                        PRINT '--- Procedure Start ---';
                        PRINT 'Target Database: ' + @DbName;

 
                        IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DbName)
                        BEGIN
                            SET @SQL = 'CREATE DATABASE [' + @DbName + ']';
                            PRINT 'Creating database: ' + @DbName;
                            EXEC(@SQL);
                        END
                        ELSE
                        BEGIN
                            PRINT 'Database already exists: ' + @DbName;
                        END
 
                        SET @SQL = '
                        IF NOT EXISTS (
                            SELECT 1 FROM [' + @DbName + '].sys.tables WHERE name = ''' + @TableName + '''
                        )
                        BEGIN
                            CREATE TABLE [' + @DbName + '].dbo.[' + @TableName + '] (
                                [Timestamp] DATETIME NOT NULL
                            );
                            PRINT ''Table ' + @TableName + ' created.'';
                        END
                        ELSE
                            PRINT ''Table ' + @TableName + ' already exists.'';
                        ';
                        EXEC(@SQL);
 
                    WHILE @i <= @FieldCount
                    BEGIN
                        DECLARE @FieldName NVARCHAR(128) = LTRIM(RTRIM(PARSENAME(REPLACE(@FieldNames, ',', '.'), @FieldCount - @i + 1)));
                        DECLARE @FieldType NVARCHAR(50) =
                            CASE 
                                WHEN @FieldTypes IS NULL OR LEN(@FieldTypes) = 0 THEN 'NVARCHAR(MAX)'
                                ELSE LTRIM(RTRIM(PARSENAME(REPLACE(@FieldTypes, ',', '.'), @FieldCount - @i + 1)))
                            END;

                        SET @SQL = '
                        IF NOT EXISTS (
                            SELECT 1 
                            FROM [' + @DbName + '].INFORMATION_SCHEMA.COLUMNS 
                            WHERE TABLE_NAME = ''' + @TableName + ''' AND COLUMN_NAME = ''' + @FieldName + '''
                        )
                        BEGIN
                            PRINT ''Adding column: ' + @FieldName + ' (' + @FieldType + ')'';
                            ALTER TABLE [' + @DbName + '].dbo.[' + @TableName + '] ADD [' + @FieldName + '] ' + @FieldType + ';
                        END
                        ELSE
                        BEGIN
                            PRINT ''Column exists: ' + @FieldName + ''';
                        END
                        ';
                        EXEC(@SQL);

                        SET @i += 1;
                    END




	                    SET @SQL = '
                        IF NOT EXISTS (
                            SELECT 1
                            FROM [' + @DbName + '].sys.indexes
                            WHERE name = ''IX_tbDataCollection_Timestamp''
                                  AND object_id = OBJECT_ID(''' + @DbName + '.dbo.' + @TableName + ''')
                        )
                        BEGIN
                            CREATE CLUSTERED INDEX [IX_tbDataCollection_Timestamp]
                            ON [' + @DbName + '].dbo.[' + @TableName + '] ([Timestamp]);
                            PRINT ''Index IX_tbDataCollection_Timestamp created.'';
                        END
                        ELSE
                            PRINT ''Index IX_tbDataCollection_Timestamp already exists.'';';
                        EXEC(@SQL);


                        PRINT '--- Procedure End ---';
                    END



                    ";
        }



        static public string BuildRecreateTableQuery(string sourceName, string targetTableName, string ColTypeName = "rType")
        {
            if (string.IsNullOrWhiteSpace(sourceName))
                throw new ArgumentException("sourceName is required", nameof(sourceName));
            if (string.IsNullOrWhiteSpace(targetTableName))
                throw new ArgumentException("targetTableName is required", nameof(targetTableName));

            string cleanTargetTableName = targetTableName.Trim();
            if (cleanTargetTableName.StartsWith("dbo.", StringComparison.OrdinalIgnoreCase))
                cleanTargetTableName = cleanTargetTableName.Substring(4);

            string fullTargetTableName = $"[VSTUnitDB].[dbo].[{cleanTargetTableName}]";

            string safeSourceName = sourceName.Replace("'", "''");

            return $@"
            IF OBJECT_ID('{fullTargetTableName}', 'U') IS NOT NULL
                DROP TABLE {fullTargetTableName};

            CREATE TABLE {fullTargetTableName} (
                ID INT,             
                [{ColTypeName}] NVARCHAR(255)
            );";

           

        }



        static public string BuildRecipeTypesQuery(string sourceName, string targetTableName, string ColTypeName="rType")
        {
            if (string.IsNullOrWhiteSpace(sourceName))
                throw new ArgumentException("sourceName is required", nameof(sourceName));
            if (string.IsNullOrWhiteSpace(targetTableName))
                throw new ArgumentException("targetTableName is required", nameof(targetTableName));

            string cleanTargetTableName = targetTableName.Trim();
            if (cleanTargetTableName.StartsWith("dbo.", StringComparison.OrdinalIgnoreCase))
                cleanTargetTableName = cleanTargetTableName.Substring(4);

            string fullTargetTableName = $"[VSTUnitDB].[dbo].[{cleanTargetTableName}]";

            string safeSourceName = sourceName.Replace("'", "''");

            return $@"
            IF OBJECT_ID('{fullTargetTableName}', 'U') IS NOT NULL
                DROP TABLE {fullTargetTableName};

            CREATE TABLE {fullTargetTableName} (
                ID INT,             
                [{ColTypeName}] NVARCHAR(255)
            );

            WITH SourceValue AS (
                SELECT [CUSTOM4] AS Delimited
                FROM [VSTUnitDB].[dbo].[tbTagsInfo]
                WHERE [name] = '{safeSourceName}'
            ),
            SplitRaw AS (
                SELECT LTRIM(RTRIM(m.n.value('.', 'NVARCHAR(255)'))) AS [{ColTypeName}]
                FROM SourceValue
                CROSS APPLY (
                    SELECT CAST('<x>' + REPLACE(Delimited, ',', '</x><x>') + '</x>' AS XML) AS xmlData
                ) AS x
                CROSS APPLY x.xmlData.nodes('/x') AS m(n)
            ),
            SplitFiltered AS (
                SELECT [{ColTypeName}]
                FROM SplitRaw
                WHERE [{ColTypeName}] <> ''
            )
            INSERT INTO {fullTargetTableName} ([{ColTypeName}], ID)
            SELECT [{ColTypeName}], ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS ID
            FROM SplitFiltered;
        ";

        }

    }
}
