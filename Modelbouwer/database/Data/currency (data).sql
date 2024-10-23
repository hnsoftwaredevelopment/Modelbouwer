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

-- Dumpen data van tabel modelbuilder.currency: ~4 rows (ongeveer)
DELETE FROM `currency`;
INSERT INTO `currency` (`Id`, `Code`, `Symbol`, `Name`, `ConversionRate`, `Created`, `Modified`) VALUES
	(1, 'EUR', '€', 'Euro', 1.0000, '2022-01-10 16:36:03', '2022-01-10 16:43:55'),
	(2, 'GBP', '£', 'Britse pond', 1.1079, '2022-01-10 16:36:03', '2022-01-10 16:44:01'),
	(3, 'USD', '$', 'Amerikaanse dollar', 0.8442, '2022-01-10 16:36:03', '2022-01-10 16:44:05'),
	(4, 'YEN', 'Y', 'Yen', 3.3400, '2022-01-10 16:36:03', '2022-01-10 16:44:08');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
