IF EXISTS (SELECT * FROM sys.views WHERE name = 'vu_Tables')
BEGIN
    DROP VIEW [dbo].[vu_Tables];
END
GO

CREATE view [dbo].[vu_Tables] AS
WITH NumberedTables AS (
    SELECT 
        ROW_NUMBER() OVER (ORDER BY TABLE_NAME) AS [No], 
        TABLE_NAME AS [Name]
    FROM 
        information_schema.tables
		where [TABLE_NAME] not in ('__EFMigrationsHistory',
		'Column',
		'Dashboard',
		'DashboardGraph',
		'Graph',
		'GraphColumn',
		'GraphType',
		'KeyValue',
		'vu_Tables',
		'sysdiagrams',
		'MergeQuery',
		'MergeQueryDetail',
		'GraphTableFilter',
		'GraphStyling')
)

SELECT 
    [No], 
    [Name]
FROM 
    NumberedTables
GO


---------------------------------------------

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'GetData')
BEGIN
    DROP VIEW [dbo].[vu_Tables];
END
GO

CREATE PROCEDURE [dbo].[GetData] 
    @TableName NVARCHAR(MAX),
    @Skip INT = 0,
    @Take INT = 0,
	@Filter nvarchar(max) = null,
	@Sort nvarchar(max) = null,
	@SortOrder nvarchar(10) = 'ASC'
AS
BEGIN
    -- Set the sort column if it is not provided
	DECLARE @DefaultSortColumn nvarchar(max);
    BEGIN
        SELECT TOP 1 @DefaultSortColumn = COLUMN_NAME 
        FROM INFORMATION_SCHEMA.COLUMNS 
        WHERE TABLE_NAME = @TableName;
    END

    -- Construct the SQL query
    DECLARE @SQL NVARCHAR(MAX);
SET @SQL = N'SELECT * FROM ' + QUOTENAME(@TableName) + 
            ' ' + ISNULL(@Filter, '') + 
            ' ORDER BY ' + QUOTENAME(ISNULL(@Sort,@DefaultSortColumn)) + 
            ' ' + CASE WHEN @SortOrder = 'ASC' THEN ' ASC ' ELSE ' DESC ' End + 
            ' OFFSET ' + CAST(@Skip AS NVARCHAR(10)) + ' ROWS ' + 
            ' FETCH NEXT ' + CAST(@Take AS NVARCHAR(10)) + ' ROWS ONLY;';

			print (@SQL)
    -- Execute the SQL query
    EXEC sp_executesql @SQL;
END

-----------------------------------------------------

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_GetColumns')
BEGIN
    DROP VIEW [dbo].[vu_Tables];
END
GO

CREATE proc [dbo].[SP_GetColumns]
@table nvarchar(max)
AS 
Begin
WITH RelationDetails AS (
    SELECT
        fk.name AS ForeignKey,
        OBJECT_NAME(fk.referenced_object_id) AS ParentTable,
        COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ParentColumn,
        OBJECT_NAME(fk.parent_object_id) AS ReferencedTable,
        COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ReferenceColumn
    FROM sys.foreign_keys AS fk
    INNER JOIN sys.foreign_key_columns AS fc
        ON fk.OBJECT_ID = fc.constraint_object_id
    WHERE 
        OBJECT_NAME(fk.referenced_object_id) = @table OR OBJECT_NAME(fk.parent_object_id) = @table
)

SELECT COLUMN_NAME AS [Name],
CASE WHEN DATA_TYPE IN ('bigint','int', 'decimal', 'tinyint', 'bit','money','smallmoney','numeric','real','smallint') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsNumber,
rd.ParentColumn AS PK,
rd.ReferenceColumn AS FK
FROM INFORMATION_SCHEMA.COLUMNS isc LEFT JOIN RelationDetails rd on isc.COLUMN_NAME = rd.ParentColumn OR isc.COLUMN_NAME = rd.ReferenceColumn
WHERE TABLE_NAME = @table
end
GO


