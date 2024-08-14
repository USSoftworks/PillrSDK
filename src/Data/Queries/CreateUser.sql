BEGIN;
INSERT INTO Users(
  UserId,
  Name,
  Email,
  Phone,
  CreatedOn
)
VALUES(
  @UserId,
  @Name,
  @Email,
  @Phone,
  unixepoch()
);
SELECT
  UserId AS _UserId,
  Name,
  Email,
  Phone,
  CreatedOn AS _CreatedOn
FROM Users
WHERE RowId = last_insert_rowid();
END;
