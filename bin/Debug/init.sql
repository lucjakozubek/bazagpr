--
-- Plik wygenerowany przez SQLiteStudio v3.2.1 dnia niedz. lis 10 15:38:35 2019
--
-- U¿yte kodowanie tekstu: UTF-8
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Tabela: Anteny
CREATE TABLE Anteny (Id_ant INTEGER PRIMARY KEY ASC AUTOINCREMENT NOT NULL, Czestotliwosc TEXT NOT NULL, Konstrukcja TEXT);

-- Tabela: Dane
CREATE TABLE Dane (Id_prof INTEGER PRIMARY KEY ASC AUTOINCREMENT NOT NULL, Typ_prof INTEGER REFERENCES Typ_prof (Id_typ) ON DELETE RESTRICT NOT NULL DEFAULT (2), Nazwa TEXT NOT NULL, RD3 BLOB NOT NULL, RAD BLOB NOT NULL, MRK BLOB, Id_proj INTEGER REFERENCES Projekt (Id_proj) ON DELETE CASCADE NOT NULL, Wspol_pocz REAL, Wspol_konca REAL, Wspol_opisowe_pocz TEXT NOT NULL, Wspol_opisowe_konca TEXT NOT NULL, Dl_prof REAL NOT NULL, Liczba_tras INTEGER NOT NULL, Odl_m_trasami REAL NOT NULL, Okno_czasowe REAL, L_probek INTEGER, Czest_prob REAL, Skladanie INTEGER, Id_ant INTEGER REFERENCES Anteny (Id_ant) ON DELETE RESTRICT NOT NULL, Uwagi TEXT);

-- Tabela: Foto
CREATE TABLE Foto (Id_fot INTEGER PRIMARY KEY ASC AUTOINCREMENT NOT NULL, Zdjecie BLOB NOT NULL, Nazwa TEXT NOT NULL, Opis TEXT, Id_proj INTEGER REFERENCES Projekt (Id_proj) ON DELETE CASCADE NOT NULL);

-- Tabela: Mapy
CREATE TABLE Mapy (Id_mapy INTEGER PRIMARY KEY UNIQUE NOT NULL, Mapa BLOB NOT NULL, Opis TEXT, Id_proj INTEGER REFERENCES Projekt (Id_proj) ON DELETE CASCADE NOT NULL);

-- Tabela: Markery
CREATE TABLE Markery (Id_mark INTEGER PRIMARY KEY ASC AUTOINCREMENT NOT NULL, Id_prof INTEGER NOT NULL REFERENCES Dane (Id_prof) ON DELETE CASCADE, Nr_mark INTEGER NOT NULL DEFAULT (1), Numer_trasy INTEGER NOT NULL, Odl_na_prof INTEGER NOT NULL, Nazwa TEXT NOT NULL, Opis TEXT);

-- Tabela: Projekt
CREATE TABLE Projekt (Id_proj INTEGER PRIMARY KEY ASC AUTOINCREMENT NOT NULL, Nazwa_proj TEXT NOT NULL UNIQUE, Data DATE NOT NULL, Adres TEXT, Opis_miejsca TEXT, Miejscowosc TEXT NOT NULL, Id_woj INTEGER REFERENCES Wojewodztwo (Id_woj) ON DELETE RESTRICT NOT NULL, Prowadzacy TEXT NOT NULL, Pogoda TEXT, War_geol TEXT, Zleceniodawca TEXT, Uwagi TEXT);

-- Tabela: Typ_prof
CREATE TABLE Typ_prof (Id_typ INTEGER PRIMARY KEY ASC AUTOINCREMENT NOT NULL, Typ TEXT NOT NULL UNIQUE);
INSERT INTO Typ_prof (Id_typ, Typ) VALUES (1, 'WARR');
INSERT INTO Typ_prof (Id_typ, Typ) VALUES (2, 'refleksyjne');

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

-- Widok: v_markery
CREATE VIEW v_markery 
AS
SELECT
    p.Nazwa AS [Profil],
    Nr_mark AS [Numer markeru na profilu],
    Numer_trasy AS [Numer trasy],
    Odl_na_prof AS [Odleg³oœæ na profilu],
    m.Nazwa AS [Nazwa markera],
    m.Opis
FROM
    Markery AS m
    INNER JOIN Dane AS p 
        ON p.Id_prof = m.Id_prof;

-- Widok: v_projekty_miejsca
CREATE VIEW v_projekty_miejsca
AS      
SELECT
    w.Wojewodztwo AS [Województwo],
    p.Miejscowosc AS [Miejscowoœæ],
    p.Adres,
    p.Opis_miejsca AS [Opis miejsca],
    p.Nazwa_proj AS [Nazwa projektu],
    p.Data
FROM
    Projekt AS p
    INNER JOIN Wojewodztwo AS w
        ON p.Id_woj = w.Id_woj;

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
