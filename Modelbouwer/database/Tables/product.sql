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

-- Structuur van  tabel modelbuilder.product wordt geschreven
CREATE TABLE IF NOT EXISTS `product` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Code` varchar(30) DEFAULT NULL,
  `Name` varchar(150) DEFAULT NULL,
  `Dimensions` varchar(150) DEFAULT '',
  `Price` double DEFAULT '0',
  `MinimalStock` double DEFAULT '0',
  `StandardOrderQuantity` double DEFAULT '0',
  `ProjectCosts` int DEFAULT '0',
  `Unit_Id` int DEFAULT NULL,
  `ImageRotationAngle` varchar(4) DEFAULT '0',
  `Image` longblob,
  `Brand_Id` int DEFAULT NULL,
  `Category_Id` int DEFAULT NULL,
  `Storage_Id` int DEFAULT '1',
  `Memo` longtext,
  `Hide` tinyint(1) DEFAULT '0',
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Code` (`Code`),
  KEY `Brand_Id` (`Brand_Id`),
  KEY `Category_Id` (`Category_Id`),
  KEY `Unit_Id` (`Unit_Id`),
  KEY `Storage_Id` (`Storage_Id`),
  CONSTRAINT `FK_Product_Brand_Id` FOREIGN KEY (`Brand_Id`) REFERENCES `brand` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Product_Category_Id` FOREIGN KEY (`Category_Id`) REFERENCES `category` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Product_Storage_Id` FOREIGN KEY (`Storage_Id`) REFERENCES `storage` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Product_Unit_Id` FOREIGN KEY (`Unit_Id`) REFERENCES `unit` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Data exporteren was gedeselecteerd

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
