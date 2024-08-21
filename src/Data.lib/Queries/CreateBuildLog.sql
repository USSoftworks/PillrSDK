BEGIN;
INSERT INTO BuildLog(
  BuildId,        -- The unique id of the build
  UserId,         -- The id of the user doing the build
  MsbVersion,     -- The semver string version of MSBuild used for the build
  MsbRuntimeType, -- The runtime type. Can be Full, Core, or Mono
  CpuCount,       -- The cpu count from the MSBuildNodeCount property
  BuildTime,      -- The duration of the build
  CreatedOn)
VALUES(
  @BuildId,
  @UserId,
  @MsbVersion,
  @MsbRuntimeType,
  @CpuCount,
  @BuildTime,
  unixepoch());
SELECT
  BuildId AS _BuildId,
  UserId AS _UserId,
  MsbVersion,
  MsbRuntimeType,
  CpuCount,
  BuildTime,
  CreatedOn AS _CreatedOn
FROM BuildLog
WHERE RowId = last_insert_rowid();
END;
