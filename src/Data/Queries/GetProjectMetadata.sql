SELECT
  ProjectName,
  UserId,
  Delta,
  CreatedOn
FROM Metadata
ORDER BY CreatedOn DESC 
LIMIT 1;
