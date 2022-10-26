CREATE TABLE [dbo].[Contacts1] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR(50) NULL,
    [Surname]     VARCHAR(50) NULL,
    [TelephoneNumber]     VARCHAR(50)            NULL,
    [Email]        VARCHAR(50) NULL,
    [DateCreated]   VARCHAR(50) NULL,
    [TimeCreated]   VARCHAR(50) NULL,
    [DateChanged] VARCHAR(50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

