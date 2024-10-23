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

-- Dumpen data van tabel modelbuilder.category: ~111 rows (ongeveer)
DELETE FROM `category`;
INSERT INTO `category` (`Id`, `ParentId`, `Name`, `Fullpath`, `Created`, `Modified`) VALUES
	(1, NULL, 'Afwerking', 'Afwerking', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(2, 1, 'Veroudering/Bruinering', 'Afwerking\\Veroudering/Bruinering', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(3, 1, 'Maskering', 'Afwerking\\Maskering', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(4, 1, 'Penselen', 'Afwerking\\Penselen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(5, 1, 'Verf', 'Afwerking\\Verf', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(6, 1, 'Lak', 'Afwerking\\Lak', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(7, 1, 'Lijm', 'Afwerking\\Lijm', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(8, 7, 'Houtlijm', 'Afwerking\\Lijm\\Houtlijm', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(9, 7, 'Secondenlijm', 'Afwerking\\Lijm\\Secondenlijm', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(10, NULL, 'Gereedschap', 'Gereedschap', '2021-12-23 08:50:06', '2024-09-02 16:06:57'),
	(11, 10, 'Algemeen', 'Gereedschap\\Algemeen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(12, 11, 'Accessoires', 'Gereedschap\\Algemeen\\Accessoires', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(13, 10, 'Elektrisch', 'Gereedschap\\Elektrisch', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(14, 13, 'Accessoires', 'Gereedschap\\Elektrisch\\Accessoires', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(15, 10, 'Hand', 'Gereedschap\\Hand', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(16, 15, 'Accessoires', 'Gereedschap\\Hand\\Accessoires', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(17, 10, 'Meetgereedschap', 'Gereedschap\\Meetgereedschap', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(18, 17, 'Accessoires', 'Gereedschap\\Meetgereedschap\\Accessoires', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(19, NULL, 'Hulpmiddelen', 'Hulpmiddelen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(20, 19, 'Opbergmiddelen', 'Hulpmiddelen\\Opbergmiddelen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(21, 19, 'Schuurpapier', 'Hulpmiddelen\\Schuurpapier', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(22, NULL, 'Materiaal', 'Materiaal', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(23, 105, 'Scheepsbeslag', 'Materiaal\\Scheepsbeslag', '2021-12-23 08:50:06', '2023-06-19 11:04:24'),
	(24, 23, 'Ankers', 'Materiaal\\Scheepsbeslag\\Ankers', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(25, 23, 'Boegbeelden', 'Materiaal\\Scheepsbeslag\\Boegbeelden', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(26, 23, 'Scheepsbellen/hoorns', 'Materiaal\\Scheepsbeslag\\Scheepsbellen/hoorns', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(27, 23, 'Blokken', 'Materiaal\\Scheepsbeslag\\Blokken', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(28, 23, 'Kikkers/Bolders/Klampen', 'Materiaal\\Scheepsbeslag\\Kikkers/Bolders/Klampen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(29, 23, 'Bijboten', 'Materiaal\\Scheepsbeslag\\Bijboten', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(30, 23, 'Dekattributen', 'Materiaal\\Scheepsbeslag\\Dekattributen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(31, 23, 'Scheepsklokken', 'Materiaal\\Scheepsbeslag\\Scheepsklokken', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(32, 23, 'Lantaarns', 'Materiaal\\Scheepsbeslag\\Lantaarns', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(33, 23, 'Mastvoetten', 'Materiaal\\Scheepsbeslag\\Mastvoeten', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(34, 23, 'Korvijnagels(Belayingpins)', 'Materiaal\\Scheepsbeslag\\Korvijnagels(Belayingpins)', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(35, 23, 'Ogen', 'Materiaal\\Scheepsbeslag\\Ogen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(36, 23, 'Bevestigingspinnen', 'Materiaal\\Scheepsbeslag\\Bevestigingspinnen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(37, 23, 'Pompen', 'Materiaal\\Scheepsbeslag\\Pompen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(38, 23, 'Patrijspoorten', 'Materiaal\\Scheepsbeslag\\Patrijspoorten', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(39, 23, 'Roosters', 'Materiaal\\Scheepsbeslag\\Roosters', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(40, 23, 'Geschut', 'Materiaal\\Scheepsbeslag\\Geschut', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(41, 23, 'Stuurwielen', 'Materiaal\\Scheepsbeslag\\Stuurwielen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(42, 23, 'Trappen', 'Materiaal\\Scheepsbeslag\\Trappen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(43, 23, 'Vlaggenmasten', 'Materiaal\\Scheepsbeslag\\Vlaggenmasten', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(44, 23, 'Want/Mast/Geleiding', 'Materiaal\\Scheepsbeslag\\Want/Mast/Geleiding', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(45, 22, 'Beuken', 'Materiaal\\Beuken', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(46, 45, 'Lat', 'Materiaal\\Beuken\\Lat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(47, 45, 'Plaat', 'Materiaal\\Beuken\\Plaat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(48, 45, 'Profiel', 'Materiaal\\Beuken\\Profiel', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(49, 45, 'Rond', 'Materiaal\\Beuken\\Rond', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(50, 45, 'Vierkant', 'Materiaal\\Beuken\\Vierkant', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(51, 22, 'Eiken', 'Materiaal\\Eiken', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(52, 51, 'Lat', 'Materiaal\\Eiken\\Lat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(53, 51, 'Plaat', 'Materiaal\\Eiken\\Plaat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(54, 51, 'Profiel', 'Materiaal\\Eiken\\Profiel', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(55, 51, 'Rond', 'Materiaal\\Eiken\\Rond', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(56, 51, 'Vierkant', 'Materiaal\\Eiken\\Vierkant', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(57, 22, 'Garen', 'Materiaal\\Garen', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(58, 57, 'Beige', 'Materiaal\\Garen\\Beige', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(59, 57, 'Bruin', 'Materiaal\\Garen\\Bruin', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(60, 57, 'Wit', 'Materiaal\\Garen\\Wit', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(61, 57, 'Zwart', 'Materiaal\\Garen\\Zwart', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(62, 22, 'Koper', 'Materiaal\\Koper', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(63, 62, 'Draad', 'Materiaal\\Koper\\Draad', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(64, 62, 'Plaat', 'Materiaal\\Koper\\Plaat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(65, 62, 'Profiel', 'Materiaal\\Koper\\Profiel', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(66, 62, 'Rond', 'Materiaal\\Koper\\Rond', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(67, 62, 'Strip', 'Materiaal\\Koper\\Strip', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(68, 62, 'Vierkant', 'Materiaal\\Koper\\Vierkant', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(69, 22, 'Kunststof', 'Materiaal\\Kunststof', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(70, 69, 'Plaat', 'Materiaal\\Kunststof\\Plaat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(71, 69, 'Profiel', 'Materiaal\\Kunststof\\Profiel', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(72, 69, 'Rond', 'Materiaal\\Kunststof\\Rond', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(73, 69, 'Strip', 'Materiaal\\Kunststof\\Strip', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(74, 69, 'Vierkant', 'Materiaal\\Kunststof\\Vierkant', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(75, 22, 'Mahoni', 'Materiaal\\Mahoni', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(76, 75, 'Lat', 'Materiaal\\Mahoni\\Lat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(77, 75, 'Plaat', 'Materiaal\\Mahoni\\Plaat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(78, 75, 'Profiel', 'Materiaal\\Mahoni\\Profiel', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(79, 75, 'Rond', 'Materiaal\\Mahoni\\Rond', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(80, 75, 'Vierkant', 'Materiaal\\Mahoni\\Vierkant', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(81, 22, 'Messing', 'Materiaal\\Messing', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(82, 81, 'Draad', 'Materiaal\\Messing\\Draad', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(83, 81, 'Plaat', 'Materiaal\\Messing\\Plaat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(84, 81, 'Profiel', 'Materiaal\\Messing\\Profiel', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(85, 81, 'Rond', 'Materiaal\\Messing\\Rond', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(86, 81, 'Strip', 'Materiaal\\Messing\\Strip', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(87, 81, 'Vierkant', 'Materiaal\\Messing\\Vierkant', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(88, 22, 'Noten', 'Materiaal\\Noten', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(89, 88, 'Lat', 'Materiaal\\Noten\\Lat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(90, 88, 'Plaat', 'Materiaal\\Noten\\Plaat', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(91, 88, 'Profiel', 'Materiaal\\Noten\\Profiel', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(92, 88, 'Rond', 'Materiaal\\Noten\\Rond', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(93, 88, 'Vierkant', 'Materiaal\\Noten\\Vierkant', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(94, 22, 'Vuren', 'Materiaal\\Vuren', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(100, 105, 'Zeilen', 'Materiaal\\Zeilen', '2021-12-23 08:50:06', '2023-06-19 11:08:27'),
	(101, 100, 'Bruin', 'Materiaal\\Zeilen\\Bruin', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(102, 100, 'Creme', 'Materiaal\\Zeilen\\Creme', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(103, 100, 'Wit', 'Materiaal\\Zeilen\\Wit', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(104, 100, 'Zwart', 'Materiaal\\Zeilen\\Zwart', '2021-12-23 08:50:06', '2021-12-23 08:50:06'),
	(105, NULL, 'Bouwdozen', 'Bouwdozen', '2023-06-19 10:59:31', '2023-06-19 10:59:31'),
	(106, 94, 'Lat', 'Materiaal\\Vuren\\Lat', '2023-06-19 10:59:52', '2023-06-19 10:59:52'),
	(107, 94, 'Plaat', 'Materiaal\\Vuren\\Plaat', '2023-06-19 11:00:02', '2023-06-19 11:00:02'),
	(108, 94, 'Profiel', 'Materiaal\\Vuren\\Profiel', '2023-06-19 11:00:12', '2023-06-19 11:00:12'),
	(109, 94, 'Rond', 'Materiaal\\Vuren\\Rond', '2023-06-19 11:00:23', '2023-06-19 11:00:23'),
	(110, 94, 'Vierkant', 'Materiaal\\Vuren\\Vierkant', '2023-06-19 11:00:33', '2023-06-19 11:00:33'),
	(113, 2, 'Test3', 'Afwerking\\Veroudering/Bruinering\\Test3', '2024-08-22 16:29:57', '2024-08-22 16:29:57');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
