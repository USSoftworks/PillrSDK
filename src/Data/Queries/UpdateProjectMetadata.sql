INSERT INTO Metadata(
  ProjectName,  -- The name of the project
  UserId,       -- The id of the user updating the metadata
  Delta,        -- The delta field tracking te changes to metadata
  CreatedOn     -- The timestamp of the metadata change
)
VALUES(
  @ProjectName,
  @UserId,
  @Delta,
  unixepoch()
);
