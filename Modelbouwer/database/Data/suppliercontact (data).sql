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

-- Dumpen data van tabel modelbuilder.suppliercontact: ~7 rows (ongeveer)
DELETE FROM `suppliercontact`;
INSERT INTO `suppliercontact` (`Id`, `Supplier_Id`, `Name`, `Contacttype_Id`, `Mail`, `Phone`, `Created`, `Modified`) VALUES
	(1, 1, 'Agemeen', 2, 'sales@cornwallmodelboats.co.uk', '+44 1840 211009', '2022-01-11 11:33:27', '2022-01-11 11:33:27'),
	(2, 2, 'Algemeen', 2, 'info@modelbouw-dordrecht.nl', '+31 78 6312711', '2022-01-11 11:33:27', '2022-01-11 11:33:27'),
	(3, 3, 'Ron', 2, 'ron@krikke.net', '+31 50 3140306', '2022-01-11 11:33:27', '2022-01-11 11:33:27'),
	(4, 4, 'Iwan en Petra', 2, 'info@hobby-en-modelbouw.nl', '+31 294 266587', '2022-01-11 11:33:27', '2022-01-11 11:33:27'),
	(5, 5, 'Algemeen', 2, 'info@meijerenblessing.nl', '+31 10 4145591', '2022-01-11 11:33:27', '2022-01-11 11:33:27'),
	(6, 6, 'Klantenservice', 5, 'info@toolstation.nl', '+31 71 5815050', '2022-01-11 11:33:27', '2022-01-11 11:33:27'),
	(7, 7, 'Klantenservice', 5, 'magazijn@toemen.nl', '+31 13 7370097', '2023-06-19 14:48:42', '2023-06-19 14:56:42');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
