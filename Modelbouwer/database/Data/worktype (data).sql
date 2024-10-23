-- --------------------------------------------------------
-- Host:                         localhost
-- Server versie:                8.3.0 - MySQL Community Server - GPL
-- Server OS:                    Win64
-- HeidiSQL Versie:              12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- Dumpen data van tabel modelbuilder.worktype: ~32 rows (ongeveer)
DELETE FROM `worktype`;
INSERT INTO `worktype` (`Id`, `ParentId`, `Name`, `FullPath`, `Created`, `Modified`) VALUES
	(0, NULL, 'Geen', 'Geen', '2022-03-19 14:37:41', '2022-03-19 14:38:31'),
	(1, NULL, 'Voorbereiding', 'Voorbereiding', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(2, NULL, 'Opruimen', 'Opruimen', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(3, NULL, 'Romp', 'Romp', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(4, 3, 'Kiel', 'Romp\\Kiel', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(5, 3, 'Spanten', 'Romp\\Spanten', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(6, 3, 'Eerste beplanking', 'Romp\\Eerste beplanking', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(7, 3, 'Tweede beplanking', 'Romp\\Tweede beplanking', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(8, 3, 'Achtersteven', 'Romp\\Achtersteven', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(9, 3, 'Afwerking romp', 'Romp\\Afwerking romp', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(10, 3, 'Schilderen/Lakken', 'Romp\\Schilderen/Lakken', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(11, NULL, 'Dek', 'Dek', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(12, 11, 'Dek beplanking', 'Dek\\Dek beplanking', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(13, 11, 'Dekbalk en reling', 'Dek\\Dekbalk en reling', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(14, 11, 'Schilderen/Lakken', 'Dek\\Schilderen/Lakken', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(15, 11, 'Afwerking dek', 'Dek\\Afwerking dek', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(16, NULL, 'Opbouw', 'Opbouw', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(17, 16, 'Bijboten', 'Opbouw\\Bijboten', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(18, 16, 'Wapens', 'Opbouw\\Wapens', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(19, 16, 'Ankers', 'Opbouw\\Ankers', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(20, 16, 'Heck', 'Opbouw\\Heck', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(21, 16, 'Masten', 'Opbouw\\Masten', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(22, NULL, 'Wand', 'Wand', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(23, 22, 'Staande wand', 'Wand\\Staande wand', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(24, 22, 'Lopend wand', 'Wand\\Lopend wand', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(25, 22, 'Afwerking', 'Wand\\Afwerking', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(26, 21, 'Ra', 'Opbouw\\Masten\\Ra', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(27, 21, 'Kraaiennest', 'Opbouw\\Masten\\Kraaiennest', '2022-01-11 12:10:00', '2022-01-11 12:10:00'),
	(28, 21, 'Zeilen', 'Opbouw\\Masten\\Zeilen', '2022-03-17 14:34:00', '2022-03-17 14:34:00');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
