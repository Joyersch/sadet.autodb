DROP DATABASE IF EXISTS Sadet;
CREATE DATABASE sadet;
\c sadet;

CREATE TABLE Games (
	appid	int  PRIMARY KEY,
	name	varchar(1000)
);

CREATE  TABLE Data (
	index	SERIAL PRIMARY KEY,
	createdat	TimeStamp WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
	appid	int,
	completion float,
	isaverage boolean
);

INSERT INTO Games(appid, name) VALUES (-1, 'Average');

