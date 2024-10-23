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

-- Dumpen data van tabel modelbuilder.productsupplier: ~15 rows (ongeveer)
DELETE FROM `productsupplier`;
INSERT INTO `productsupplier` (`Id`, `Product_Id`, `Supplier_Id`, `Currency_Id`, `ProductNumber`, `ProductName`, `Price`, `Url`, `DefaultSupplier`, `Created`, `Modified`) VALUES
	(1, 1, 6, 1, '16435', 'Everbuild secondelijm medium viscositeit', 4.07, 'https://www.toolstation.nl/everbuild-secondelijm-medium-viscositeit/p86374?channable=002a91696400383633373490&gclid=CjwKCAjwiuuRBhBvEiwAFXKaNPVd2npt7UgrPaFN8O48usa7l_bTtQIfm75c8CHHWXG11fiYv3wzexoCGvMQAvD_BwE', '*', '2022-01-10 16:49:00', '2024-10-01 11:40:09'),
	(4, 2, 6, 1, '10893', 'Everbuild secondelijm hoge viscosoteit', 4.07, 'https://www.toolstation.nl/everbuild-secondelijm-hoge-viscositeit/p78871?channable=002a9169640037383837311e&gclid=CjwKCAjwiuuRBhBvEiwAFXKaNOckRHnltQBvWbSaVg6AoQ1E4xOtL3iBeKXIL4NNTq1Mk8ustRGGfxoCbysQAvD_BwE', '*', '2022-01-10 16:49:00', '2024-10-01 11:40:09'),
	(5, 10, 1, 2, 'AN5423/15', 'Fairlead 15mm', 2.29, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=an5423%2F15&PN=5423%2D15%2DFairlead%2D15mm%2DAN5423_15%2Ehtml#SID=570', '*', '2022-03-03 13:54:10', '2024-10-01 11:40:09'),
	(6, 11, 1, 2, 'C82010N', 'Rigging Thread 0.10mm Natural (10m)', 1.98, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82010N%2Ehtml#SID=182', '*', '2022-03-17 14:50:40', '2024-10-01 11:40:09'),
	(7, 12, 1, 2, 'C82025N', 'Rigging Thread 0.25mm Natural (10m)', 1.98, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82025N%2Ehtml#SID=182', '*', '2022-03-17 14:50:40', '2024-10-01 11:40:09'),
	(10, 13, 1, 2, 'C82025B', 'Rigging Thread 0.25mm Black (10m)', 1.98, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82025B%2Ehtml#SID=182', '*', '2022-03-18 14:38:08', '2024-10-01 11:40:09'),
	(11, 14, 1, 2, 'C82050B', 'Rigging Thread 0.50mm Black (10m)', 2.14, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82050B%2Ehtml#SID=182', '*', '2022-03-18 14:38:34', '2024-10-01 11:40:09'),
	(14, 17, 1, 2, 'C82100N', 'Rigging Thread 1.00mm Natural (10m)', 2.51, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82100N%2Ehtml#SID=182', '*', '2022-03-22 14:01:31', '2024-10-01 11:40:09'),
	(15, 18, 1, 2, 'C82130N', 'Rigging Thread 1.30mm Natural (10m)', 3.02, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82130N%2Ehtml#SID=182', '*', '2022-03-22 14:01:31', '2024-10-01 11:40:09'),
	(16, 19, 1, 2, 'C82170N', 'Rigging Thread 1.70mm Natural (5m)', 3.72, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82170N%2Ehtml#SID=182', '*', '2022-03-22 14:01:31', '2024-10-01 11:40:09'),
	(17, 20, 1, 2, 'C82225N', 'Rigging Thread 2.25mm Natural (2.5m)', 5.16, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82250N%2Ehtml#SID=182', '*', '2022-03-22 14:01:31', '2024-10-01 11:40:09'),
	(18, 21, 1, 2, 'C82075B', 'Rigging Thread 0.75mm Black (10m)', 2.33, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82075B%2Ehtml#SID=182', '*', '2022-03-22 14:01:31', '2024-10-01 11:40:09'),
	(19, 22, 1, 2, 'C82100B', 'Rigging Thread 1.00mm Back (10m)', 2.51, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82100B%2Ehtml#SID=182', '*', '2022-03-22 14:01:31', '2024-10-01 11:40:09'),
	(20, 23, 1, 2, 'C82130B', 'Rigging Thread 1.30mm Black (10m)', 3.02, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82130B%2Ehtml#SID=182', '*', '2022-03-22 14:01:31', '2024-10-01 11:40:09'),
	(21, 24, 1, 2, 'C82180B', 'Rigging Thread 1.80mm Black (5m)', 3.72, 'https://www.cornwallmodelboats.co.uk/cgi-bin/sh000001.pl?WD=thread&PN=caldercraft_C82180B%2Ehtml#SID=182', '*', '2022-03-22 14:01:31', '2024-10-21 13:49:18'),
	(22, 24, 2, 1, 'Test', 'Test', 120, '', NULL, '2024-10-18 16:31:04', '2024-10-21 13:49:18'),
	(27, 31, 2, 1, '123', '', 10, '', '*', '2024-10-23 12:19:59', '2024-10-23 12:19:59');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
