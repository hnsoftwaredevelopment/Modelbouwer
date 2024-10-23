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

-- Structuur van  tabel modelbuilder.supplier wordt geschreven
CREATE TABLE IF NOT EXISTS `supplier` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Code` varchar(20) DEFAULT NULL,
  `Name` varchar(150) DEFAULT NULL,
  `Address1` varchar(150) DEFAULT NULL,
  `Address2` varchar(150) DEFAULT NULL,
  `Zip` varchar(15) DEFAULT NULL,
  `City` varchar(40) DEFAULT NULL,
  `Url` varchar(255) DEFAULT NULL,
  `ShippingCosts` double NOT NULL DEFAULT '0',
  `MinShippingCosts` double NOT NULL DEFAULT '0',
  `OrderCosts` double NOT NULL DEFAULT '0',
  `Memo` longtext,
  `Currency_Id` int DEFAULT '1',
  `Country_Id` int DEFAULT '1',
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE KEY `supplier_Code` (`Code`) USING BTREE,
  KEY `Currency_Id` (`Currency_Id`) USING BTREE,
  KEY `Country_Id` (`Country_Id`) USING BTREE,
  CONSTRAINT `FK_Supplier_Country_Id` FOREIGN KEY (`Country_Id`) REFERENCES `country` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Supplier_Currency_Id` FOREIGN KEY (`Currency_Id`) REFERENCES `currency` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporteren was gedeselecteerd

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
