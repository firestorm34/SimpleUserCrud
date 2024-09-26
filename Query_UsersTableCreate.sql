﻿CREATE TABLE [dbo].[USERS] (
    [Id]        UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Login]     NVARCHAR (100)   NOT NULL,
    [FirstName] NVARCHAR (100)   NOT NULL,
    [LastName]  NVARCHAR (100)   NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);