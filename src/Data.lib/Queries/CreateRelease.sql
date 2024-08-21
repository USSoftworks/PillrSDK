BEGIN;
INSERT INTO Releases(
  ReleaseId,
  UserId,
  CreatedOn
)
VALUES(
  @ReleaseId,
  @UserId,
  @Note,
  unixepoch()
);
SELECT 
  ReleaseId AS _ReleaseId,
  UserId AS _UserId,
  Note,
  CreatedOn AS _CreatedOn
FROM Release
WHERE RowId = last_insert_rowid();
END;
