--DROP TABLE IF EXISTS Authors;
--DROP TABLE IF EXISTS Users;
--DROP TABLE IF EXISTS BuildLog;
--DROP TABLE IF EXISTS Metadata;
--DROP TABLE IF EXISTS Deployments;
--DROP TABLE IF EXISTS SlackMessages;

BEGIN;

CREATE TABLE IF NOT EXISTS Users(
  UserId    UUID NOT NULL UNIQUE,
  Name      TEXT NOT NULL,
  Email     TEXT NOT NULL UNIQUE,
  Phone     TEXT,
  CreatedOn INTEGER NOT NULL,
  PRIMARY KEY (UserId)
);

CREATE TABLE IF NOT EXISTS BuildLog(
  BuildId        UUID NOT NULL UNIQUE,
  UserId         UUID NOT NULL,
  Major          INTEGER NOT NULL,
  Minor          INTEGER NOT NULL,
  Patch          INTEGER NOT NULL,
  MsbVersion     TEXT NOT NULL,
  MsbRuntimeType INTEGER NULL,
  CpuCount       INTEGER NOT NULL,
  BuildTime      INTEGER NOT NULL,
  CreatedOn      INTEGER NOT NULL,
  PRIMARY KEY (BuildId),
  FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE IF NOT EXISTS Metadata(
  ProjectName TEXT NOT NULL,
  Delta       INTEGER NOT NULL,
  UserId      UUID NOT NULL,
  CreatedOn   INTEGER NOT NULL,
  FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE IF NOT EXISTS Versions(
  Major      INTEGER NOT NULL,
  Minor      INTEGER NOT NULL,
  Patch      INTEGER NOT NULL,
  IsCurrent  BOOLEAN NOT NULL,
  UserId     UUID NOT NULL,
  CreatedOn  INTEGER NOT NULL,
  PRIMARY KEY (Major, Minor, Patch)
);

CREATE TABLE IF NOT EXISTS Releases(
  ReleaseId  UUID NOT NULL UNIQUE,
  BuildId    UUID NULL UNIQUE,
  CreatedOn  INTEGER NOT NULL,
  PRIMARY KEY (ReleaseId),
  FOREIGN KEY (BuildId) REFERENCES BuildLog(BuildId)
);

CREATE TABLE IF NOT EXISTS ReleaseNotes(
  ReleaseId  UUID NOT NULL,
  UserId     UUID NOT NULL,
  Note       TEXT NOT NULL,
  FOREIGN KEY (ReleaseId) REFERENCES Releases(ReleaseId),
  FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE IF NOT EXISTS Deployments(
  DeploymentId  UUID NOT NULL UNIQUE,
  CreatedOn     INTEGER NOT NULL,
  UserId        UUID NOT NULL,
  ReleaseId     UUID NULL UNIQUE,
  PRIMARY KEY (DeploymentId),
  FOREIGN KEY (UserId) REFERENCES Users(UserId),
  FOREIGN KEY (ReleaseId) REFERENCES Releases(ReleaseId)
);

CREATE TABLE IF NOT EXISTS SlackMessages(
  ChannelId TEXT NOT NULL,
  CreatedOn INTEGER NOT NULL
);

END;
