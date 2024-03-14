create schema [weatherViewer]

create table [weatherViewer].[WeatherArchiveRecords]
(
	[id] [bigint] not null identity(1, 1),
	[created] [datetime] null,
	[temperature] [decimal](18, 2) null,
	[humidity] [decimal](18, 2) null,
	[pressure] [smallint] null,
	[dewPoint] [decimal](18, 2) null,
	[windDirection] [varchar](2000) collate Cyrillic_General_CI_AS null,
	[windSpeed] [tinyint] null,
	[cloudiness] [tinyint] null,
	[cloudBase] [smallint] null,
	[horizontalVisibility] [varchar](2000) collate Cyrillic_General_CI_AS null,
	[weatherСonditions] [varchar](2000) collate Cyrillic_General_CI_AS null
);

alter table [weatherViewer].[WeatherArchiveRecords]
add constraint [PK_WeatherArchiveRecords]
	primary key clustered ([id]) on [PRIMARY];

create nonclustered index [IX_WeatherArchiveRecords_created]
on [weatherViewer].[WeatherArchiveRecords] ([created])
on [PRIMARY];