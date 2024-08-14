SELECT
  ReleaseId,
  UserId,
  Note,
  CreatedOn
FROM Releases
  JOIN ReleaseNotes
    ON Releases.ReleaseId = ReleaseNotes.ReleaseId
WHERE ReleaseId = @ReleaseId;
