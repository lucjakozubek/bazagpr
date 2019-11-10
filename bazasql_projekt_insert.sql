--
-- Plik wygenerowany przez SQLiteStudio v3.2.1 dnia niedz. lis 10 15:37:55 2019
--
-- U¿yte kodowanie tekstu: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Tabela: Projekt
CREATE TABLE Projekt (Id_proj INTEGER PRIMARY KEY ASC AUTOINCREMENT NOT NULL, Nazwa_proj TEXT NOT NULL UNIQUE, Data DATE NOT NULL, Adres TEXT, Opis_miejsca TEXT, Miejscowosc TEXT NOT NULL, Id_woj INTEGER REFERENCES Wojewodztwo (Id_woj) ON DELETE RESTRICT NOT NULL, Prowadzacy TEXT NOT NULL, Pogoda TEXT, War_geol TEXT, Zleceniodawca TEXT, Uwagi TEXT);
INSERT INTO Projekt (Id_proj, Nazwa_proj, Data, Adres, Opis_miejsca, Miejscowosc, Id_woj, Prowadzacy, Pogoda, War_geol, Zleceniodawca, Uwagi) VALUES (1, 'Bia³y Koœció³', '01-09-2019', NULL, 'Opis miejsca. Wieœ.', 'Bia³y Koœció³', 6, 'dr in¿. J. Karczewski', 'Pogoda s³oneczna', 'Warunki geologiczne', NULL, NULL);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
