SELECT
  UserId,
  Name,
  Email,
  Phone,
  CreatedOn
FROM Users
WHERE {0} = @Value
