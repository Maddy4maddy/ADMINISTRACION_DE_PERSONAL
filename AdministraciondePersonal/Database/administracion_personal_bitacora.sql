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
-- Table structure for table `bitacora`
--

DROP TABLE IF EXISTS `bitacora`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bitacora` (
  `id_bitacora` int NOT NULL AUTO_INCREMENT,
  `fecha_bitacora` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `usuario` varchar(100) NOT NULL,
  `descripcion_accion` text NOT NULL,
  PRIMARY KEY (`id_bitacora`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bitacora`
--

LOCK TABLES `bitacora` WRITE;
/*!40000 ALTER TABLE `bitacora` DISABLE KEYS */;
INSERT INTO `bitacora` VALUES (1,'2026-05-27 17:48:03','Usuario','Actualización de oferente Angie Romero Ceciliano'),(2,'2026-05-27 17:49:17','Usuario','Registro de oferente Yorleny Ceciliano Araya'),(3,'2026-05-27 17:49:44','Usuario','Actualización de oferente Andrea Gómez Solano'),(4,'2026-05-27 18:08:34','Usuario','Actualización de oferente Angie Romero Ceciliano'),(5,'2026-05-28 13:50:59','Usuario','Actualización de oferente Angie Romero Ceciliano'),(6,'2026-05-28 13:57:11','Usuario','Actualización de oferente Yorleny Ceciliano Araya'),(7,'2026-05-29 18:18:13','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(8,'2026-05-29 20:06:02','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(9,'2026-05-29 20:06:47','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(10,'2026-05-29 20:08:48','Usuario','Registro de oferente Juan Hidalgo Muñoz'),(11,'2026-05-29 20:13:04','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(12,'2026-05-29 20:13:17','Usuario','Actualización de oferente Andrea Gómez Solano');
/*!40000 ALTER TABLE `bitacora` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-06-07 14:25:52
