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

-- Structuur van  tabel modelbuilder.supplyorderline wordt geschreven
CREATE TABLE IF NOT EXISTS `supplyorderline` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Supplyorder_Id` int NOT NULL DEFAULT '0',
  `Supplier_Id` int NOT NULL DEFAULT '0',
  `Product_Id` int DEFAULT '0',
  `SupplierProductName` varchar(150) DEFAULT '',
  `Project_Id` int DEFAULT '0',
  `Category_Id` int DEFAULT '0',
  `Amount` double DEFAULT '0',
  `OpenAmount` double DEFAULT '0',
  `Price` double DEFAULT '0',
  `RealRowTotal` double DEFAULT '0',
  `Closed` tinyint DEFAULT '0',
  `ClosedDate` date DEFAULT NULL,
  `Created` datetime DEFAULT CURRENT_TIMESTAMP,
  `Modified` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`) USING BTREE,
  KEY `FK_OrderRow_Supplyorder_Id` (`Supplyorder_Id`),
  KEY `FK_OrderRow_Supplier_Id` (`Supplier_Id`),
  KEY `FK_OrderRow_Project_Id` (`Project_Id`),
  KEY `FK_OrderRow_Category_Id` (`Category_Id`),
  KEY `FK_OrderRow_Product_Id` (`Product_Id`) USING BTREE,
  CONSTRAINT `FK_OrderRow_Category_Id` FOREIGN KEY (`Category_Id`) REFERENCES `category` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_OrderRow_Product_Id` FOREIGN KEY (`Product_Id`) REFERENCES `product` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_OrderRow_Project_Id` FOREIGN KEY (`Project_Id`) REFERENCES `project` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_OrderRow_Supplier_Id` FOREIGN KEY (`Supplier_Id`) REFERENCES `supplier` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_OrderRow_Supplyorder_Id` FOREIGN KEY (`Supplyorder_Id`) REFERENCES `supplyorder` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci ROW_FORMAT=DYNAMIC;

-- Data exporteren was gedeselecteerd

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
