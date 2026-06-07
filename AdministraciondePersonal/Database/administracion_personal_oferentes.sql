-- MySQL dump 10.13  Distrib 8.0.44, for Win64 (x86_64)
--
-- Host: localhost    Database: administracion_personal
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `oferentes`
--

DROP TABLE IF EXISTS `oferentes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `oferentes` (
  `identificacion` varchar(20) NOT NULL,
  `tipo_identificacion` enum('Cédula de identidad','DIMEX','Pasaporte') NOT NULL,
  `nombre_completo` varchar(150) NOT NULL,
  `fecha_nacimiento` date NOT NULL,
  `correo` varchar(150) NOT NULL,
  `telefono` varchar(20) NOT NULL,
  PRIMARY KEY (`identificacion`),
  UNIQUE KEY `correo` (`correo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `oferentes`
--

LOCK TABLES `oferentes` WRITE;
/*!40000 ALTER TABLE `oferentes` DISABLE KEYS */;
INSERT INTO `oferentes` VALUES ('208400050','Cédula de identidad','Angie Romero Ceciliano','2003-03-28','angieroceciliano@gmail.com','83258567'),('304800515','Cédula de identidad','Juan Hidalgo Muñoz','1994-08-12','juanhidalgom@gmail.com','75652797'),('603000824','Cédula de identidad','Yorleny Ceciliano Araya','1975-07-06','yorlececilianoa@gmail.com','72013595'),('DIMEX001','DIMEX','Santiago Madriz Ceciliano','1995-11-20','santiagomadrizc@gmail.com','87776655'),('PAS12345','Pasaporte','Andrea Gómez Solano','1999-06-08','andreagomezs@gmail.com','86665544');
/*!40000 ALTER TABLE `oferentes` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-06-07 14:25:49
