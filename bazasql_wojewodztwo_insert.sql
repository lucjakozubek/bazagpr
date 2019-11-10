--
-- Plik wygenerowany przez SQLiteStudio v3.2.1 dnia niedz. lis 10 15:39:48 2019
--
-- U¿yte kodowanie tekstu: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Tabela: Wojewodztwo
CREATE TABLE Wojewodztwo (Id_woj INTEGER NOT NULL, Wojewodztwo TEXT NOT NULL UNIQUE, CONSTRAINT PK_Miejscowosc PRIMARY KEY (Id_woj));
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (1, 'dolnoœl¹skie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (2, 'kujawsko-pomorskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (3, 'lubelskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (4, 'lubuskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (5, '³ódzkie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (6, 'ma³opolskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (7, 'mazowieckie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (8, 'opolskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (9, 'podkarpackie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (10, 'podlaskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (11, 'pomorskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (12, 'œl¹skie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (13, 'œwiêtokrzyskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (14, 'warmiñsko-mazurskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (15, 'wielkopolskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (16, 'zachodniopomorskie');

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
