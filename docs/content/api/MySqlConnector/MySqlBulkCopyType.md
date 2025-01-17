---
title: MySqlBulkCopy
---

# MySqlBulkCopy class

[`MySqlBulkCopy`](../MySqlBulkCopyType/) lets you efficiently load a MySQL Server table with data from another source. It is similar to the [SqlBulkCopy](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlbulkcopy) class for SQL Server.

Due to [security features](https://mysqlconnector.net/troubleshooting/load-data-local-infile/) in MySQL Server, the connection string must have `AllowLoadLocalInfile=true` in order to use this class.

For data that is in CSV or TSV format, use [`MySqlBulkLoader`](../MySqlBulkLoaderType/) to bulk load the file.

Example code:

```csharp
// NOTE: to copy data between tables in the same database, use INSERT ... SELECT
// https://dev.mysql.com/doc/refman/8.0/en/insert-select.html
var dataTable = GetDataTableFromExternalSource();

// open the connection
using var connection = new MySqlConnection("...;AllowLoadLocalInfile=True");
await connection.OpenAsync();

// bulk copy the data
var bulkCopy = new MySqlBulkCopy(connection);
bulkCopy.DestinationTableName = "some_table_name";
var result = await bulkCopy.WriteToServerAsync(dataTable);

// check for problems
if (result.Warnings.Count != 0) { /* handle potential data loss warnings */ }
```

```csharp
public sealed class MySqlBulkCopy
```

## Public Members

| name | description |
| --- | --- |
| [MySqlBulkCopy](../MySqlBulkCopy/MySqlBulkCopy/)(…) | Initializes a [`MySqlBulkCopy`](../MySqlBulkCopyType/) object with the specified connection, and optionally the active transaction. |
| [BulkCopyTimeout](../MySqlBulkCopy/BulkCopyTimeout/) { get; set; } | The number of seconds for the operation to complete before it times out, or `0` for no timeout. |
| [ColumnMappings](../MySqlBulkCopy/ColumnMappings/) { get; } | A collection of [`MySqlBulkCopyColumnMapping`](../MySqlBulkCopyColumnMappingType/) objects. If the columns being copied from the data source line up one-to-one with the columns in the destination table then populating this collection is unnecessary. Otherwise, this should be filled with a collection of [`MySqlBulkCopyColumnMapping`](../MySqlBulkCopyColumnMappingType/) objects specifying how source columns are to be mapped onto destination columns. If one column mapping is specified, then all must be specified. |
| [DestinationTableName](../MySqlBulkCopy/DestinationTableName/) { get; set; } | The name of the table to insert rows into. |
| [NotifyAfter](../MySqlBulkCopy/NotifyAfter/) { get; set; } | If non-zero, this specifies the number of rows to be processed before generating a notification event. |
| event [MySqlRowsCopied](../MySqlBulkCopy/MySqlRowsCopied/) | This event is raised every time that the number of rows specified by the [`NotifyAfter`](../MySqlBulkCopy/NotifyAfter/) property have been processed. |
| [WriteToServer](../MySqlBulkCopy/WriteToServer/)(…) | Copies all rows in the supplied DataTable to the destination table specified by the [`DestinationTableName`](../MySqlBulkCopy/DestinationTableName/) property of the [`MySqlBulkCopy`](../MySqlBulkCopyType/) object. (3 methods) |
| [WriteToServerAsync](../MySqlBulkCopy/WriteToServerAsync/)(…) | Asynchronously copies all rows in the supplied DataTable to the destination table specified by the [`DestinationTableName`](../MySqlBulkCopy/DestinationTableName/) property of the [`MySqlBulkCopy`](../MySqlBulkCopyType/) object. (3 methods) |

## Remarks

Note: This API is a unique feature of MySqlConnector; you must [switch to MySqlConnector](https://mysqlconnector.net/overview/installing/) in order to use it.

This API is experimental and may change in the future.

## See Also

* namespace [MySqlConnector](../../MySqlConnectorNamespace/)
* assembly [MySqlConnector](../../MySqlConnectorAssembly/)

<!-- DO NOT EDIT: generated by xmldocmd for MySqlConnector.dll -->
