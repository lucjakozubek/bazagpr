--
-- Plik wygenerowany przez SQLiteStudio v3.2.1 dnia niedz. lis 10 15:39:48 2019
--
-- U�yte kodowanie tekstu: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Tabela: Wojewodztwo
CREATE TABLE Wojewodztwo (Id_woj INTEGER NOT NULL, Wojewodztwo TEXT NOT NULL UNIQUE, CONSTRAINT PK_Miejscowosc PRIMARY KEY (Id_woj));
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (1, 'dolno�l�skie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (2, 'kujawsko-pomorskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (3, 'lubelskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (4, 'lubuskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (5, '��dzkie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (6, 'ma�opolskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (7, 'mazowieckie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (8, 'opolskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (9, 'podkarpackie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (10, 'podlaskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (11, 'pomorskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (12, '�l�skie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (13, '�wi�tokrzyskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (14, 'warmi�sko-mazurskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (15, 'wielkopolskie');
INSERT INTO Wojewodztwo (Id_woj, Wojewodztwo) VALUES (16, 'zachodniopomorskie');

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
