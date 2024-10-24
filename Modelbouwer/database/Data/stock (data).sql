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

-- Dumpen data van tabel modelbuilder.stock: ~20 rows (ongeveer)
DELETE FROM `stock`;
INSERT INTO `stock` (`Id`, `product_Id`, `Amount`, `Created`, `Modified`) VALUES
	(5, 1, 1, '2022-01-24 16:47:27', '2023-11-30 12:15:12'),
	(6, 2, 1, '2022-01-24 16:54:37', '2023-11-30 12:15:16'),
	(8, 9, 1, '2022-03-02 13:11:26', '2023-11-30 12:15:19'),
	(9, 10, 0, '2022-03-03 13:53:02', '2023-06-19 09:01:53'),
	(10, 11, 0, '2022-03-17 14:49:28', '2023-06-19 09:01:54'),
	(11, 12, 0, '2022-03-18 14:30:25', '2022-03-18 14:30:22'),
	(12, 13, 0, '2022-03-18 14:30:38', '2022-03-18 14:30:38'),
	(13, 14, 0, '2022-03-18 14:30:48', '2022-03-18 14:30:48'),
	(14, 15, 0, '2022-03-22 13:41:14', '2022-03-22 13:41:14'),
	(15, 16, 0, '2022-03-22 13:41:21', '2022-03-22 13:41:21'),
	(16, 17, 0, '2022-03-22 13:41:31', '2022-03-22 13:41:31'),
	(17, 18, 0, '2022-03-22 13:41:38', '2022-03-22 13:41:38'),
	(18, 19, 0, '2022-03-22 13:52:33', '2022-03-22 13:52:33'),
	(19, 20, 0, '2022-03-22 13:52:39', '2022-03-22 13:52:39'),
	(20, 21, 0, '2022-03-22 14:18:39', '2022-03-22 14:18:39'),
	(21, 22, 0, '2022-03-22 14:18:46', '2022-03-22 14:18:46'),
	(22, 23, 0, '2022-03-22 14:18:52', '2022-03-22 14:18:52'),
	(23, 24, 0, '2022-03-22 14:18:58', '2022-03-22 14:18:58'),
	(24, 27, 0, '2023-06-19 09:28:25', '2023-06-19 09:28:25'),
	(25, 29, 0, '2023-06-19 11:06:52', '2023-06-19 11:06:52');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
