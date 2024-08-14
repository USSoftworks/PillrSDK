SELECT
  BuildId, 
  UserId,
  MsbVersion,
  MsbRuntimeType,
  CpuCount,
  BuildTime,
  CreatedOn
FROM BuildLog
WHERE {0} = @FieldValue
