BEGIN;
INSERT INTO Versions(
  Major,  -- The major version number of the project
  Minor,  -- The minor version number of the project
  Patch,  -- The patch number of the project
  UserId, -- The ID of the user that created the version
  CreatedOn
)
VALUES(
  @Major,
  @Minor,
  @Patch,
  @UserId,
  unixepoch()
);
SELECT
  Major,
  Minor,
  Patch,
  UserId AS _UserId,
  CreatedOn AS _CreatedOn
FROM Version
WHERE Major = @Major
  AND Minor = @Minor
  AND Patch = @Patch;
END;
