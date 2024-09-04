﻿DROP DATABASE IF EXISTS MyBoardGameCafe;

CREATE DATABASE MyBoardGameCafe;

USE MyBoardGameCafe;

CREATE TABLE Designers (
	Id INT PRIMARY KEY IDENTITY(1,1),
	FirstName VARCHAR(20),
	LastName VARCHAR(20)
);

CREATE TABLE Games (
	Id INT PRIMARY KEY IDENTITY(1,1),
	GameName VARCHAR(40),
	Designer VARCHAR(40),
	Price DECIMAL(5,2),
	GenreId INT,
	CONSTRAINT FK_Games_Designers
	FOREIGN KEY (GenreId) REFERENCES Designers(Id)
);