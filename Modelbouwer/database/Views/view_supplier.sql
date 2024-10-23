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
DROP TABLE IF EXISTS `view_supplier`;
CREATE ALGORITHM=MERGE SQL SECURITY DEFINER VIEW `view_supplier` AS select `s`.`Id` AS `Id`,`s`.`Code` AS `Code`,`s`.`Name` AS `Name`,`s`.`Address1` AS `Address1`,`s`.`Address2` AS `Address2`,`s`.`Zip` AS `Zip`,`s`.`City` AS `City`,`s`.`Url` AS `Url`,`s`.`Country_Id` AS `Country_Id`,`co`.`Name` AS `Country`,`s`.`Currency_Id` AS `Currency_Id`,`cu`.`Symbol` AS `Currency`,`s`.`ShippingCosts` AS `ShippingCosts`,`s`.`MinShippingCosts` AS `MinShippingCosts`,`s`.`OrderCosts` AS `OrderCosts`,`s`.`Memo` AS `memo` from ((`supplier` `s` join `country` `co` on((`s`.`Country_Id` = `co`.`Id`))) join `currency` `cu` on((`s`.`Currency_Id` = `cu`.`Id`)));

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
