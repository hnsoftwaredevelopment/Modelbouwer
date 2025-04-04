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

-- Tijdelijke tabel wordt verwijderd, en definitieve VIEW wordt aangemaakt.
DROP TABLE IF EXISTS `view_projectcosts`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `view_projectcosts` AS select `prj`.`Name` AS `ProjectName`,`cat`.`Fullpath` AS `Category`,`prd`.`Name` AS `ProductName`,sum(`pu`.`AmountUsed`) AS `AmountUsed`,`prd`.`Price` AS `Price`,(sum(`pu`.`AmountUsed`) * `prd`.`Price`) AS `Total` from (((`productusage` `pu` join `project` `prj` on((`pu`.`project_Id` = `prj`.`Id`))) join `product` `prd` on((`pu`.`product_Id` = `prd`.`Id`))) join `category` `cat` on((`prd`.`Category_Id` = `cat`.`Id`))) group by `prd`.`Name` order by `prj`.`Name`,`cat`.`Fullpath`,`prd`.`Name`;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
