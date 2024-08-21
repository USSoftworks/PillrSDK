BEGIN;
INSERT INTO ReleaseNotes(
  ReleaseId,
  UserId,
  Note,
  CreatedOn)
VALUES(
  @ReleaseId,
  @UserId,
  @Note,
  unixepoch());
SELECT
  ReleaseId AS _ReleaseId,
  UserId AS _UserId,
  Note,
  CreatedOn AS _CreatedOn
FROM ReleaseNotes
WHERE RowId = last_insert_rowid()
END;
