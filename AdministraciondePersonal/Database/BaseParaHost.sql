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
) ENGINE=InnoDB AUTO_INCREMENT=183 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bitacora`
--

LOCK TABLES `bitacora` WRITE;
/*!40000 ALTER TABLE `bitacora` DISABLE KEYS */;
INSERT INTO `bitacora` VALUES (1,'2026-05-27 17:48:03','Usuario','Actualización de oferente Angie Romero Ceciliano'),(2,'2026-05-27 17:49:17','Usuario','Registro de oferente Yorleny Ceciliano Araya'),(3,'2026-05-27 17:49:44','Usuario','Actualización de oferente Andrea Gómez Solano'),(4,'2026-05-27 18:08:34','Usuario','Actualización de oferente Angie Romero Ceciliano'),(5,'2026-05-28 13:50:59','Usuario','Actualización de oferente Angie Romero Ceciliano'),(6,'2026-05-28 13:57:11','Usuario','Actualización de oferente Yorleny Ceciliano Araya'),(7,'2026-05-29 18:18:13','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(8,'2026-05-29 20:06:02','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(9,'2026-05-29 20:06:47','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(10,'2026-05-29 20:08:48','Usuario','Registro de oferente Juan Hidalgo Muñoz'),(11,'2026-05-29 20:13:04','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(12,'2026-05-29 20:13:17','Usuario','Actualización de oferente Andrea Gómez Solano'),(13,'2026-06-07 18:40:37','admin','Cambio de estado de usuario maddy: activo → inactivo'),(14,'2026-06-07 20:50:43','admin','Creación de usuario: {\"NombreUsuario\":\"Maria Ruiz\",\"NombreCompleto\":\"Maria Ruiz Lopez\",\"Correo\":\"marims@gmail.com\",\"Estado\":\"activo\",\"IdRol\":11}'),(15,'2026-06-07 21:00:10','Maria Ruiz','Cambio de estado de usuario maddy: activo → inactivo'),(16,'2026-06-07 21:00:26','Maria Ruiz','Cambio de estado de usuario maddy: inactivo → activo'),(17,'2026-06-07 21:00:46','Maria Ruiz','Actualización de usuario: {\"NombreUsuario\":\"Maria Ruiz\",\"NombreCompleto\":\"Maria Ruiz Lopez\",\"Correo\":\"marims@gmail.com\",\"Estado\":\"activo\",\"IdRol\":2}'),(18,'2026-06-07 21:10:36','admin','Creación de usuario: {\"NombreUsuario\":\"Josh Silver\",\"NombreCompleto\":\"Josh Silver\",\"Correo\":\"jonnysrr@gmail.com\",\"Estado\":\"activo\",\"IdRol\":1}'),(19,'2026-06-07 23:47:55','admin','El usuario consulta concursos.'),(20,'2026-06-07 23:48:03','admin','El usuario consulta concursos.'),(21,'2026-06-07 23:58:57','admin','El usuario consulta experiencia laboral del oferente .'),(22,'2026-06-07 23:59:01','admin','El usuario consulta experiencia laboral del oferente .'),(23,'2026-06-08 09:28:41','Maria Ruiz','Creación de usuario: {\"NombreUsuario\":\"Maria Ruiz\",\"NombreCompleto\":\"Maria Ruiz Lopez\",\"Correo\":\"marims@gmail.com\",\"Estado\":\"activo\",\"IdRol\":4}'),(24,'2026-06-08 09:37:25','Maria Ruiz','El usuario consulta concursos.'),(25,'2026-06-08 09:41:43','Maria Ruiz','Cambio de estado de usuario maddy: activo → inactivo'),(26,'2026-06-08 09:41:54','Maria Ruiz','Cambio de estado de usuario maddy: inactivo → activo'),(27,'2026-06-09 01:54:13','admin','El usuario consulta oferentes.'),(28,'2026-06-09 01:54:13','admin','El usuario consulta oferentes.'),(29,'2026-06-09 01:54:20','admin','El usuario consulta preparación académica del oferente .'),(30,'2026-06-09 01:54:21','admin','El usuario consulta preparación académica del oferente .'),(31,'2026-06-09 01:54:22','admin','El usuario consulta preparación académica del oferente .'),(32,'2026-06-09 01:54:23','admin','El usuario consulta preparación académica del oferente .'),(33,'2026-06-09 01:54:24','admin','El usuario consulta experiencia laboral del oferente .'),(34,'2026-06-09 01:54:25','admin','El usuario consulta entrevistas agendadas.'),(35,'2026-06-09 02:00:14','admin','Eliminación de usuario: {\"NombreUsuario\":\"Maicol Cordero \",\"NombreCompleto\":\"Maicol Cordero Calvo\",\"Correo\":\"maicol@gmail.com\"}'),(36,'2026-06-09 02:04:17','admin','Creación de usuario: {\"NombreUsuario\":\"Naza\",\"NombreCompleto\":\"Nazareth Artavia Perez\",\"Correo\":\"nanaz@gmail.com\",\"Estado\":\"activo\",\"IdRol\":2}'),(37,'2026-06-09 02:04:45','admin','Creación de usuario: {\"NombreUsuario\":\"Naza\",\"NombreCompleto\":\"Nazareth Artavia Perez\",\"Correo\":\"nanaz@gmail.com\",\"Estado\":\"activo\",\"IdRol\":1}'),(38,'2026-06-09 02:05:37','admin','Cambio de estado de usuario maddy: activo → inactivo'),(39,'2026-06-09 02:06:31','admin','Cambio de estado de usuario maddy: inactivo → activo'),(40,'2026-06-09 17:07:12','admin','El usuario consulta oferentes.'),(41,'2026-06-09 17:07:13','admin','El usuario consulta concursos.'),(42,'2026-06-09 17:07:15','admin','El usuario consulta preparación académica del oferente .'),(43,'2026-06-09 17:07:18','admin','El usuario consulta entrevistas agendadas.'),(44,'2026-06-09 17:07:25','admin','El usuario consulta preparación académica del oferente .'),(45,'2026-06-09 17:07:26','admin','El usuario consulta concursos.'),(46,'2026-06-09 17:07:27','admin','El usuario consulta oferentes.'),(47,'2026-06-09 17:16:42','admin','El usuario consulta concursos.'),(48,'2026-06-09 17:16:47','admin','El usuario consulta concursos.'),(49,'2026-06-09 17:20:29','admin','El usuario consulta oferentes.'),(50,'2026-06-09 17:20:33','admin','El usuario consulta oferentes.'),(51,'2026-06-09 17:20:38','admin','El usuario consulta entrevistas agendadas.'),(52,'2026-06-09 17:21:52','admin','El usuario consulta entrevistas agendadas.'),(53,'2026-06-09 17:22:04','admin','El usuario consulta oferentes.'),(54,'2026-06-09 17:23:29','admin','Registro de oferente \'Alejandro Ruiz Morales\' con identificación DIMEX002, asignado al concurso \'Contabilidad\'.'),(55,'2026-06-09 17:23:52','admin','El usuario consulta concursos.'),(56,'2026-06-09 17:23:53','admin','El usuario consulta oferentes.'),(57,'2026-06-09 17:25:10','admin','Registro de oferente \'Sofía Carvajal Fernández\' con identificación 704560846, asignado al concurso \'Desarrollo Web\'.'),(58,'2026-06-09 17:25:26','admin','El usuario consulta concursos.'),(59,'2026-06-09 17:25:27','admin','El usuario consulta oferentes.'),(60,'2026-06-09 17:27:19','admin','El usuario consulta oferentes.'),(61,'2026-06-09 17:28:06','admin','Registro de oferente \'Andrés Fallas Montero\' con identificación PAS1423, asignado al concurso \'Tecnología\'.'),(62,'2026-06-09 17:28:13','admin','El usuario consulta concursos.'),(63,'2026-06-09 17:28:14','admin','El usuario consulta oferentes.'),(64,'2026-06-09 17:29:37','admin','Registro de oferente \'Josué Solano Jiménez\' con identificación DIMEX003, asignado al concurso \'CCNA\'.'),(65,'2026-06-09 17:29:42','admin','El usuario consulta experiencia laboral del oferente .'),(66,'2026-06-09 17:29:45','admin','El usuario consulta oferentes.'),(67,'2026-06-09 17:31:06','admin','Registro de oferente \'María Rodríguez Araya \' con identificación 301450258, asignado al concurso \'Contabilidad\'.'),(68,'2026-06-09 17:32:11','admin','Registro de oferente \'Gabriel Ruiz Ceciliano\' con identificación PAS2463, asignado al concurso \'Soporte Técnico\'.'),(69,'2026-06-09 17:32:33','admin','El usuario consulta oferentes.'),(70,'2026-06-09 17:32:48','admin','El usuario consulta entrevistas agendadas.'),(71,'2026-06-09 17:32:51','admin','El usuario ingresa a la pantalla para agendar una nueva entrevista.'),(72,'2026-06-09 17:33:34','admin','Agenda de nueva entrevista para el oferente \'Alejandro Ruiz Morales\' con identificación DIMEX002, entrevistador \'Maria Ruiz Lopez\', fecha 2026-07-05 08:00, estado \'Pendiente\'.'),(73,'2026-06-09 17:33:51','admin','Agenda de nueva entrevista para el oferente \'Andrea Gómez Solano\' con identificación PAS12345, entrevistador \'Nazareth Artavia Perez\', fecha 2026-07-06 10:00, estado \'Pendiente\'.'),(74,'2026-06-09 17:33:52','admin','El usuario consulta entrevistas agendadas.'),(75,'2026-06-09 17:34:00','admin','El usuario ingresa a la pantalla para agendar una nueva entrevista.'),(76,'2026-06-09 17:34:07','admin','El usuario consulta entrevistas agendadas.'),(77,'2026-06-09 17:34:09','admin','El usuario ingresa a la pantalla para agendar una nueva entrevista.'),(78,'2026-06-09 17:34:40','admin','Agenda de nueva entrevista para el oferente \'Yorleny Ceciliano Araya\' con identificación 603000824, entrevistador \'Johan Alvarado\', fecha 2026-06-15 15:40, estado \'Pendiente\'.'),(79,'2026-06-09 17:34:50','admin','Agenda de nueva entrevista para el oferente \'Sofía Carvajal Fernández\' con identificación 704560846, entrevistador \'Johan Alvarado\', fecha 2026-06-15 15:00, estado \'Pendiente\'.'),(80,'2026-06-09 17:34:52','admin','El usuario consulta entrevistas agendadas.'),(81,'2026-06-09 17:34:59','admin','El usuario ingresa a la pantalla para agendar una nueva entrevista.'),(82,'2026-06-09 17:35:27','admin','Agenda de nueva entrevista para el oferente \'Santiago Madriz Ceciliano\' con identificación DIMEX001, entrevistador \'Nazareth Artavia Perez\', fecha 0072-01-01 11:30, estado \'Pendiente\'.'),(83,'2026-06-09 17:35:29','admin','El usuario consulta entrevistas agendadas.'),(84,'2026-06-09 17:35:36','admin','El usuario ingresa a la pantalla para agendar una nueva entrevista.'),(85,'2026-06-09 17:36:07','admin','Agenda de nueva entrevista para el oferente \'María Rodríguez Araya \' con identificación 301450258, entrevistador \'Nazareth Artavia Perez\', fecha 2026-06-17 14:15, estado \'Pendiente\'.'),(86,'2026-06-09 17:36:09','admin','El usuario consulta entrevistas agendadas.'),(87,'2026-06-09 17:36:13','admin','El usuario ingresa a la pantalla para agendar una nueva entrevista.'),(88,'2026-06-09 17:36:49','admin','Agenda de nueva entrevista para el oferente \'Andrés Fallas Montero\' con identificación PAS1423, entrevistador \'Maria Ruiz Lopez\', fecha 2026-06-12 13:15, estado \'Pendiente\'.'),(89,'2026-06-09 17:36:51','admin','El usuario consulta entrevistas agendadas.'),(90,'2026-06-09 17:37:00','admin','El usuario ingresa a la pantalla para agendar una nueva entrevista.'),(91,'2026-06-09 17:37:19','admin','Agenda de nueva entrevista para el oferente \'Josué Solano Jiménez\' con identificación DIMEX003, entrevistador \'Johan Alvarado\', fecha 2026-06-12 09:30, estado \'Pendiente\'.'),(92,'2026-06-09 17:37:25','admin','El usuario consulta entrevistas agendadas.'),(93,'2026-06-09 17:37:46','admin','El usuario consulta entrevistas agendadas.'),(94,'2026-06-09 17:37:49','admin','El usuario consulta los datos de la entrevista del oferente \'Andrea Gómez Solano\' programada para 2026-07-06 10:00.'),(95,'2026-06-09 17:37:56','admin','El usuario consulta entrevistas agendadas.'),(96,'2026-06-09 17:38:00','admin','Eliminación de entrevista del oferente \'Andrea Gómez Solano\' con identificación PAS12345, programada para 2026-07-06 10:00.'),(97,'2026-06-09 17:38:02','admin','El usuario ingresa a la pantalla para agendar una nueva entrevista.'),(98,'2026-06-09 17:38:21','admin','Agenda de nueva entrevista para el oferente \'Juan Hidalgo Muñoz\' con identificación 304800515, entrevistador \'Johan Alvarado\', fecha 2026-06-16 13:15, estado \'Pendiente\'.'),(99,'2026-06-09 17:38:22','admin','El usuario consulta entrevistas agendadas.'),(100,'2026-06-09 17:38:54','admin','El usuario ingresa a la pantalla para agendar una nueva entrevista.'),(101,'2026-06-09 17:39:31','admin','Agenda de nueva entrevista para el oferente \'Gabriel Ruiz Ceciliano\' con identificación PAS2463, entrevistador \'Johan Alvarado\', fecha 2026-07-01 10:20, estado \'Pendiente\'.'),(102,'2026-06-09 17:39:33','admin','El usuario consulta entrevistas agendadas.'),(103,'2026-06-09 17:39:34','admin','El usuario consulta entrevistas agendadas.'),(104,'2026-06-09 17:39:37','admin','El usuario consulta entrevistas agendadas.'),(105,'2026-06-09 17:39:38','admin','El usuario consulta entrevistas agendadas.'),(106,'2026-06-09 17:49:23','admin','Creación de usuario: {\"NombreUsuario\":\"Jana\",\"NombreCompleto\":\"Jana Jimenez \",\"Correo\":\"jaajajaa@gmail.com\",\"Estado\":\"activo\",\"IdRol\":1}'),(107,'2026-06-09 17:49:57','admin','Creación de usuario: {\"NombreUsuario\":\"Luis \",\"NombreCompleto\":\"Luis Lopez \",\"Correo\":\"lulu@gmail.com\",\"Estado\":\"activo\",\"IdRol\":2}'),(108,'2026-06-09 17:50:28','admin','Creación de usuario: {\"NombreUsuario\":\"Marta \",\"NombreCompleto\":\"Marta Ortis\",\"Correo\":\"marrr@gmail.com\",\"Estado\":\"activo\",\"IdRol\":11}'),(109,'2026-06-09 17:50:52','admin','Creación de usuario: {\"NombreUsuario\":\"Mar\",\"NombreCompleto\":\"Mar Solis\",\"Correo\":\"marmar@gmail.com\",\"Estado\":\"activo\",\"IdRol\":1}'),(110,'2026-06-09 18:12:57','admin','El usuario consulta concursos.'),(111,'2026-06-09 18:13:04','admin','El usuario consulta oferentes.'),(112,'2026-06-09 18:13:06','admin','El usuario consulta preparación académica del oferente DIMEX002.'),(113,'2026-06-09 18:13:56','Sistema','Eliminó el rol ID 1'),(114,'2026-06-09 18:14:04','Sistema','Eliminó el rol ID 15'),(115,'2026-06-09 18:15:27','admin','El usuario consulta entrevistas agendadas.'),(116,'2026-06-09 18:15:36','admin','El usuario consulta oferentes.'),(117,'2026-06-09 18:15:38','admin','El usuario consulta concursos.'),(118,'2026-06-09 18:15:39','admin','El usuario consulta preparación académica del oferente .'),(119,'2026-06-12 21:38:15','admin','El usuario consulta oferentes.'),(120,'2026-06-12 21:40:55','admin','El usuario consulta oferentes.'),(121,'2026-06-12 21:49:27','admin','El usuario consulta oferentes.'),(122,'2026-06-12 23:02:22','admin','El usuario consulta oferentes.'),(123,'2026-06-12 23:03:08','admin','El usuario consulta oferentes.'),(124,'2026-06-12 23:03:29','admin','El usuario consulta oferentes.'),(125,'2026-06-13 00:31:52','admin','El usuario consulta oferentes.'),(126,'2026-06-13 00:31:52','admin','El usuario consulta oferentes.'),(127,'2026-06-13 00:32:16','admin','El usuario consulta oferentes.'),(128,'2026-06-13 00:33:46','admin','El usuario consulta oferentes.'),(129,'2026-06-13 00:35:59','admin','Creación de usuario: {\"NombreUsuario\":\"Ryn\",\"NombreCompleto\":\"Ryn Foreman\",\"Correo\":\"rynryn@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"Administrador Web, Almacenista\"}'),(130,'2026-06-13 00:50:11','admin','El usuario consulta oferentes.'),(131,'2026-06-13 00:59:28','admin','El usuario consulta oferentes.'),(132,'2026-06-13 00:59:29','admin','El usuario consulta concursos.'),(133,'2026-06-13 00:59:34','admin','El usuario consulta experiencia laboral del oferente .'),(134,'2026-06-13 00:59:41','admin','El usuario consulta concursos.'),(135,'2026-06-13 00:59:42','admin','El usuario consulta oferentes.'),(136,'2026-06-13 01:00:54','admin','El usuario consulta oferentes.'),(137,'2026-06-13 01:11:22','admin','El usuario consulta oferentes.'),(138,'2026-06-13 01:11:32','admin','El usuario consulta entrevistas agendadas.'),(139,'2026-06-13 01:11:43','admin','El usuario consulta entrevistas agendadas.'),(140,'2026-06-13 01:11:46','admin','El usuario consulta entrevistas agendadas.'),(141,'2026-06-13 01:19:44','admin','El usuario consulta entrevistas agendadas.'),(142,'2026-06-13 01:20:29','admin','El usuario consulta entrevistas agendadas.'),(143,'2026-06-13 01:20:30','admin','El usuario consulta entrevistas agendadas.'),(144,'2026-06-13 02:10:44','admin','El usuario consulta entrevistas agendadas.'),(145,'2026-06-13 02:10:45','admin','El usuario consulta entrevistas agendadas.'),(146,'2026-06-13 02:16:57','admin','El usuario consulta entrevistas agendadas.'),(147,'2026-06-13 02:16:58','admin','El usuario consulta entrevistas agendadas.'),(148,'2026-06-13 02:43:42','admin','Creación de usuario: {\"NombreUsuario\":\"Megan\",\"NombreCompleto\":\"Megan Fox\",\"Correo\":\"megan@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"Administrador de Sistemas\"}'),(149,'2026-06-13 02:43:52','admin','Actualización de usuario: {\"NombreUsuario\":\"Megan\",\"NombreCompleto\":\"Megan Fox\",\"Correo\":\"megan@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"Administrador de Sistemas, Arquitecto de Software\"}'),(150,'2026-06-13 02:48:09','admin','Cambio de estado de usuario Mar: activo → inactivo'),(151,'2026-06-13 02:48:24','admin','Cambio de estado de usuario Mar: inactivo → activo'),(152,'2026-06-13 02:48:34','admin','Eliminación de usuario: {\"NombreUsuario\":\"Mar\",\"NombreCompleto\":\"Mar Solis\",\"Correo\":\"marmar@gmail.com\",\"RolesTexto\":\"\"}'),(153,'2026-06-13 02:49:24','admin','Creación de usuario: {\"NombreUsuario\":\"Amanda \",\"NombreCompleto\":\"Amanda Seyfield\",\"Correo\":\"Aman@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"Almacenista\"}'),(154,'2026-06-13 02:49:36','admin','Actualización de usuario: {\"NombreUsuario\":\"Amanda \",\"NombreCompleto\":\"Amanda Seyfield\",\"Correo\":\"Aman@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"Administrador de Base de Datos, Administrador de Redes, Almacenista\"}'),(155,'2026-06-13 17:54:50','multirol','El usuario consulta instituciones educativas.'),(156,'2026-06-13 18:00:56','multirol','El usuario consulta instituciones educativas.'),(157,'2026-06-13 18:02:37','multirol','El usuario consulta oferentes.'),(158,'2026-06-13 18:17:14','admin','Creación de usuario: {\"NombreUsuario\":\"LauraRRHH\",\"NombreCompleto\":\"Laura Morales\",\"Correo\":\"morales124ll@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"SECRETARIA\"}'),(159,'2026-06-13 18:18:18','admin','Actualización de usuario: {\"NombreUsuario\":\"LauraRRHH\",\"NombreCompleto\":\"Laura Morales\",\"Correo\":\"morales124ll@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"SECRETARIA\"}'),(160,'2026-06-13 18:22:42','LauraRRHH','El usuario consulta oferentes.'),(161,'2026-06-13 18:22:50','LauraRRHH','El usuario consulta entrevistas agendadas.'),(162,'2026-06-13 18:25:26','Sistema','Creó el rol: NuevoRol'),(163,'2026-06-13 18:26:43','admin','Creación de usuario: {\"NombreUsuario\":\"nuevoUsuario\",\"NombreCompleto\":\"Nazareth Artavia\",\"Correo\":\"naza@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"NuevoRol\"}'),(164,'2026-06-13 19:15:13','admin','Consultó compañías.'),(165,'2026-06-13 20:02:10','admin','Cambio de estado de usuario multirol: activo → inactivo'),(166,'2026-06-13 20:02:24','admin','Cambio de estado de usuario multirol: inactivo → activo'),(167,'2026-06-13 20:24:34','admin','Creación de usuario: {\"NombreUsuario\":\"Madeline\",\"NombreCompleto\":\"Madeline Cordero Ruiz \",\"Correo\":\"made@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"Administrador de Redes, Administrador de Sistemas, Administrador Web\"}'),(168,'2026-06-13 20:25:04','admin','Cambio de estado de usuario Madeline: activo → inactivo'),(169,'2026-06-13 20:25:18','admin','Cambio de estado de usuario Madeline: inactivo → activo'),(170,'2026-06-13 20:26:01','admin','Creación de usuario: {\"NombreUsuario\":\"usereliminacion\",\"NombreCompleto\":\"adswdwd\",\"Correo\":\"svdsvf@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"Contador\"}'),(171,'2026-06-13 20:26:18','admin','Actualización de usuario: {\"NombreUsuario\":\"usereliminacionyaa\",\"NombreCompleto\":\"adswdwd\",\"Correo\":\"svdsvf@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"Contador, Coordinador\"}'),(172,'2026-06-13 20:47:41','admin','El usuario consulta oferentes.'),(173,'2026-06-13 20:51:01','admin','Eliminación de usuario: {\"NombreUsuario\":\"usereliminacionyaa\",\"NombreCompleto\":\"adswdwd\",\"Correo\":\"svdsvf@gmail.com\",\"RolesTexto\":\"Contador, Coordinador\"}'),(174,'2026-06-13 20:53:08','admin','Cambio de estado de usuario Maria Ruiz: inactivo → activo'),(175,'2026-06-13 20:53:18','admin','Actualización de usuario: {\"NombreUsuario\":\"Maria Ruiz\",\"NombreCompleto\":\"Maria Ruiz Lopez\",\"Correo\":\"marims@gmail.com\",\"Estado\":\"activo\",\"Roles\":\"Administrador de Redes\"}'),(176,'2026-06-13 21:14:15','Sistema','Editó el rol ID 51 - Administrador de Redes'),(177,'2026-06-13 21:23:01','Sistema','Editó el rol ID 51 - Administrador de Redes'),(178,'2026-06-13 21:37:32','Maria Ruiz','Creación de usuario: {\"NombreUsuario\":\"Mary\",\"NombreCompleto\":\"Mary Perez\",\"Correo\":\"mas@gmail.coim\",\"Estado\":\"activo\",\"Roles\":\"Administrador de Sistemas, Administrador Web\"}'),(179,'2026-06-13 21:37:39','Maria Ruiz','Cambio de estado de usuario Mary: activo → inactivo'),(180,'2026-06-13 21:37:50','Maria Ruiz','Cambio de estado de usuario Mary: inactivo → activo'),(181,'2026-06-13 21:37:55','Maria Ruiz','Eliminación de usuario: {\"NombreUsuario\":\"Mary\",\"NombreCompleto\":\"Mary Perez\",\"Correo\":\"mas@gmail.coim\",\"RolesTexto\":\"Administrador de Sistemas, Administrador Web\"}'),(182,'2026-06-13 23:01:52','Sistema','Modificó el parámetro ID 7 - TIEMPO_EXPIRACION_SESION');
/*!40000 ALTER TABLE `bitacora` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `companias`
--

DROP TABLE IF EXISTS `companias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `companias` (
  `id_compania` int NOT NULL AUTO_INCREMENT,
  `nombre_compania` varchar(150) NOT NULL,
  PRIMARY KEY (`id_compania`)
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `companias`
--

LOCK TABLES `companias` WRITE;
/*!40000 ALTER TABLE `companias` DISABLE KEYS */;
INSERT INTO `companias` VALUES (1,'El Muisicologo C.R'),(2,'Tecnologias Avanzadas S.A.'),(3,'Servicios Empresariales CR'),(4,'Constructora del Valle'),(5,'Comercializadora Global'),(6,'Inversiones Horizonte'),(7,'Grupo Empresarial Omega'),(8,'Soluciones Informaticas CR'),(9,'Distribuidora Nacional'),(10,'Logistica Express'),(11,'Consultores Asociados'),(12,'Corporacion Delta'),(13,'Industrias Modernas'),(14,'Agropecuaria La Colina'),(15,'Transportes Unidos'),(16,'Finanzas Integrales'),(17,'Innovacion Digital'),(18,'Mercadeo Creativo'),(19,'Manufacturas Centroamericanas'),(20,'Servicios Tecnicos Especializados'),(21,'Alimentos del Pacifico'),(22,'Importadora Internacional'),(23,'Exportadora del Caribe'),(24,'Grupo Comercial Aurora'),(25,'Proyectos y Desarrollo S.A.'),(26,'Energias Renovables CR'),(27,'Ingenieria Integral'),(28,'Seguridad Empresarial'),(29,'Turismo y Aventura'),(30,'Clinica Empresarial'),(31,'Educacion y Futuro'),(32,'Telecomunicaciones Globales'),(33,'Tecnologia Verde'),(34,'Servicios Financieros CR'),(35,'Holding Empresarial Centroamericano');
/*!40000 ALTER TABLE `companias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `concursos`
--

DROP TABLE IF EXISTS `concursos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `concursos` (
  `codigo_concurso` int NOT NULL AUTO_INCREMENT,
  `nombre_concurso` varchar(150) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `estado` enum('Vigente','Vencido') NOT NULL DEFAULT 'Vigente',
  PRIMARY KEY (`codigo_concurso`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `concursos`
--

LOCK TABLES `concursos` WRITE;
/*!40000 ALTER TABLE `concursos` DISABLE KEYS */;
INSERT INTO `concursos` VALUES (1,'CCNA','2026-01-10','2026-03-15','Vigente'),(2,'Desarrollo Web','2026-02-01','2026-04-20','Vigente'),(3,'Soporte Técnico','2026-01-05','2026-02-28','Vencido'),(4,'Base de Datos','2026-03-01','2026-05-01','Vigente'),(5,'Administrativo','2026-05-01','2026-06-01','Vigente'),(6,'Recursos Humanos','2026-05-05','2026-06-10','Vigente'),(7,'Tecnología','2026-05-08','2026-06-15','Vigente'),(8,'Contabilidad','2026-06-02','2026-07-28','Vencido');
/*!40000 ALTER TABLE `concursos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `entrevistas`
--

DROP TABLE IF EXISTS `entrevistas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `entrevistas` (
  `id_entrevista` int NOT NULL AUTO_INCREMENT,
  `identificacion_oferente` varchar(50) NOT NULL,
  `id_usuario_entrevistador` int NOT NULL,
  `fecha_entrevista` datetime NOT NULL,
  `estado` enum('Pendiente','Realizada') NOT NULL DEFAULT 'Pendiente',
  PRIMARY KEY (`id_entrevista`),
  KEY `fk_entrevista_oferente` (`identificacion_oferente`),
  KEY `fk_entrevista_usuario` (`id_usuario_entrevistador`),
  CONSTRAINT `fk_entrevista_oferente` FOREIGN KEY (`identificacion_oferente`) REFERENCES `oferentes` (`identificacion`),
  CONSTRAINT `fk_entrevista_usuario` FOREIGN KEY (`id_usuario_entrevistador`) REFERENCES `usuarios` (`id_usuario`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `entrevistas`
--

LOCK TABLES `entrevistas` WRITE;
/*!40000 ALTER TABLE `entrevistas` DISABLE KEYS */;
INSERT INTO `entrevistas` VALUES (4,'PAS12345',2,'2026-06-15 14:30:00','Pendiente'),(6,'208400050',2,'2026-06-17 13:00:00','Pendiente'),(7,'DIMEX002',5,'2026-07-05 08:00:00','Pendiente'),(9,'603000824',2,'2026-06-15 15:40:00','Pendiente'),(10,'704560846',2,'2026-06-15 15:00:00','Pendiente'),(11,'DIMEX001',10,'0072-01-01 11:30:00','Pendiente'),(12,'301450258',10,'2026-06-17 14:15:00','Pendiente'),(13,'PAS1423',5,'2026-06-12 13:15:00','Pendiente'),(14,'DIMEX003',2,'2026-06-12 09:30:00','Pendiente'),(15,'304800515',2,'2026-06-16 13:15:00','Pendiente'),(16,'PAS2463',2,'2026-07-01 10:20:00','Pendiente');
/*!40000 ALTER TABLE `entrevistas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `experiencia_laboral`
--

DROP TABLE IF EXISTS `experiencia_laboral`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `experiencia_laboral` (
  `id_experiencia` int NOT NULL AUTO_INCREMENT,
  `identificacion_oferente` varchar(20) NOT NULL,
  `nombre_empresa` varchar(100) NOT NULL,
  `puesto_desempenado` varchar(100) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  PRIMARY KEY (`id_experiencia`),
  KEY `fk_experiencia_oferente` (`identificacion_oferente`),
  CONSTRAINT `fk_experiencia_oferente` FOREIGN KEY (`identificacion_oferente`) REFERENCES `oferentes` (`identificacion`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `experiencia_laboral`
--

LOCK TABLES `experiencia_laboral` WRITE;
/*!40000 ALTER TABLE `experiencia_laboral` DISABLE KEYS */;
INSERT INTO `experiencia_laboral` VALUES (1,'208400050','IBM','Analista Sistemas','2021-01-15','2023-12-20'),(2,'603000824','Grupo Purdy','Asistente Reclutamiento','2019-03-01','2022-10-30');
/*!40000 ALTER TABLE `experiencia_laboral` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `experiencia_laboral_asignacion`
--

DROP TABLE IF EXISTS `experiencia_laboral_asignacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `experiencia_laboral_asignacion` (
  `id_asignacion` int NOT NULL AUTO_INCREMENT,
  `id_experiencia` int NOT NULL,
  `descripcion_asignacion` varchar(150) NOT NULL,
  `fecha_asignacion` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_asignacion`),
  KEY `fk_asignacion_experiencia` (`id_experiencia`),
  CONSTRAINT `fk_asignacion_experiencia` FOREIGN KEY (`id_experiencia`) REFERENCES `experiencia_laboral` (`id_experiencia`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `experiencia_laboral_asignacion`
--

LOCK TABLES `experiencia_laboral_asignacion` WRITE;
/*!40000 ALTER TABLE `experiencia_laboral_asignacion` DISABLE KEYS */;
INSERT INTO `experiencia_laboral_asignacion` VALUES (1,1,'Experiencia laboral asignada para prueba de restricción','2026-05-31 11:54:33');
/*!40000 ALTER TABLE `experiencia_laboral_asignacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `expiracion_sesion`
--

DROP TABLE IF EXISTS `expiracion_sesion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `expiracion_sesion` (
  `id_expiracion_sesion` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `fecha_inicio` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_expiracion_sesion`),
  KEY `id_usuario` (`id_usuario`),
  CONSTRAINT `expiracion_sesion_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `expiracion_sesion`
--

LOCK TABLES `expiracion_sesion` WRITE;
/*!40000 ALTER TABLE `expiracion_sesion` DISABLE KEYS */;
/*!40000 ALTER TABLE `expiracion_sesion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `instituciones_educativas`
--

DROP TABLE IF EXISTS `instituciones_educativas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `instituciones_educativas` (
  `id_institucion` int NOT NULL AUTO_INCREMENT,
  `nombre_institucion` varchar(150) NOT NULL,
  PRIMARY KEY (`id_institucion`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `instituciones_educativas`
--

LOCK TABLES `instituciones_educativas` WRITE;
/*!40000 ALTER TABLE `instituciones_educativas` DISABLE KEYS */;
INSERT INTO `instituciones_educativas` VALUES (1,'Universidad de Costa Rica'),(2,'Universidad Nacional'),(3,'Instituto Tecnológico de Costa Rica'),(4,'Universidad Estatal a Distancia'),(5,'Universidad Latina'),(6,'Colegio Universitario de Cartago');
/*!40000 ALTER TABLE `instituciones_educativas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `modulos`
--

DROP TABLE IF EXISTS `modulos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `modulos` (
  `id_modulo` int NOT NULL AUTO_INCREMENT,
  `nombre_modulo` varchar(100) NOT NULL,
  `url` varchar(200) DEFAULT NULL,
  `icono` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_modulo`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `modulos`
--

LOCK TABLES `modulos` WRITE;
/*!40000 ALTER TABLE `modulos` DISABLE KEYS */;
/*!40000 ALTER TABLE `modulos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `oferente_concurso`
--

DROP TABLE IF EXISTS `oferente_concurso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `oferente_concurso` (
  `id_oferente_concurso` int NOT NULL AUTO_INCREMENT,
  `identificacion_oferente` varchar(20) NOT NULL,
  `codigo_concurso` int NOT NULL,
  PRIMARY KEY (`id_oferente_concurso`),
  KEY `identificacion_oferente` (`identificacion_oferente`),
  KEY `codigo_concurso` (`codigo_concurso`),
  CONSTRAINT `oferente_concurso_ibfk_1` FOREIGN KEY (`identificacion_oferente`) REFERENCES `oferentes` (`identificacion`),
  CONSTRAINT `oferente_concurso_ibfk_2` FOREIGN KEY (`codigo_concurso`) REFERENCES `concursos` (`codigo_concurso`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `oferente_concurso`
--

LOCK TABLES `oferente_concurso` WRITE;
/*!40000 ALTER TABLE `oferente_concurso` DISABLE KEYS */;
INSERT INTO `oferente_concurso` VALUES (5,'208400050',1),(11,'603000824',6),(12,'DIMEX002',8),(13,'704560846',2),(14,'PAS1423',7),(15,'DIMEX003',1),(16,'301450258',8),(17,'PAS2463',3);
/*!40000 ALTER TABLE `oferente_concurso` ENABLE KEYS */;
UNLOCK TABLES;

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
INSERT INTO `oferentes` VALUES ('208400050','Cédula de identidad','Angie Romero Ceciliano','2003-03-28','angieroceciliano@gmail.com','83258567'),('301450258','Cédula de identidad','María Rodríguez Araya ','1992-07-06','mariarodrigueza@gmail.com','65458793'),('304800515','Cédula de identidad','Juan Hidalgo Muñoz','1994-08-12','juanhidalgom@gmail.com','75652797'),('603000824','Cédula de identidad','Yorleny Ceciliano Araya','1975-07-06','yorlececilianoa@gmail.com','72013595'),('704560846','Cédula de identidad','Sofía Carvajal Fernández','2000-10-02','sofiacarvajalf@gmail.com','64128746'),('DIMEX001','DIMEX','Santiago Madriz Ceciliano','1995-11-20','santiagomadrizc@gmail.com','87776655'),('DIMEX002','DIMEX','Alejandro Ruiz Morales','1989-06-25','alejandroruizm@gmail.com','78953146'),('DIMEX003','DIMEX','Josué Solano Jiménez','2005-11-13','josuesolanoj@gmail.com','74159631'),('PAS12345','Pasaporte','Andrea Gómez Solano','1999-06-08','andreagomezs@gmail.com','86665544'),('PAS1423','Pasaporte','Andrés Fallas Montero','1995-12-12','andresfallasm@gmail.com','82459631'),('PAS2463','Pasaporte','Gabriel Ruiz Ceciliano','1996-11-16','gabrielruizc@gmail.com','65457854');
/*!40000 ALTER TABLE `oferentes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pantallas`
--

DROP TABLE IF EXISTS `pantallas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pantallas` (
  `id_pantalla` int NOT NULL AUTO_INCREMENT,
  `nombre_pantalla` varchar(100) NOT NULL,
  `ruta` varchar(200) NOT NULL,
  PRIMARY KEY (`id_pantalla`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pantallas`
--

LOCK TABLES `pantallas` WRITE;
/*!40000 ALTER TABLE `pantallas` DISABLE KEYS */;
INSERT INTO `pantallas` VALUES (1,'Administración Roles','/Roles'),(2,'Administración Usuarios','/Usuarios'),(3,'Registro Oferentes','/Oferentes'),(4,'Registro Concursos','/Concursos'),(5,'Registro Preparación Académica','/PreparacionAcademica'),(6,'Experiencia Laboral','/ExperienciaLaboral'),(7,'Agendar Entrevista','/Entrevistas'),(8,'Visualizar Bitácoras','/Bitacora'),(9,'Administración Parámetros','/Parametros'),(10,'Administración Compañías','/Compañia'),(11,'Administración Pantallas','/Pantalla'),(12,'Administración Instituciones Educativas','/InstitucionesEducativas');
/*!40000 ALTER TABLE `pantallas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `parametros`
--

DROP TABLE IF EXISTS `parametros`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `parametros` (
  `id_parametro` int NOT NULL AUTO_INCREMENT,
  `codigo` varchar(100) NOT NULL,
  `valor` varchar(500) NOT NULL,
  PRIMARY KEY (`id_parametro`),
  UNIQUE KEY `codigo` (`codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parametros`
--

LOCK TABLES `parametros` WRITE;
/*!40000 ALTER TABLE `parametros` DISABLE KEYS */;
INSERT INTO `parametros` VALUES (1,'MAX_INTENTOS_LOGIN','3'),(2,'DIAS_VIGENCIA_CONCURSO','30'),(3,'EDAD_MINIMA_OFERENTE','18'),(4,'MAX_ENTREVISTAS_OFERENTE','5'),(5,'CORREO_RRHH','rrhh@empresa.com'),(6,'NOMBRE_SISTEMA','Administracion de Personal '),(7,'TIEMPO_EXPIRACION_SESION','1');
/*!40000 ALTER TABLE `parametros` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `preparacion_academica`
--

DROP TABLE IF EXISTS `preparacion_academica`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `preparacion_academica` (
  `id_preparacion` int NOT NULL AUTO_INCREMENT,
  `identificacion_oferente` varchar(20) NOT NULL,
  `id_institucion` int NOT NULL,
  `titulo_obtenido` varchar(100) NOT NULL,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  PRIMARY KEY (`id_preparacion`),
  KEY `fk_preparacion_oferente` (`identificacion_oferente`),
  KEY `fk_preparacion_institucion` (`id_institucion`),
  CONSTRAINT `fk_preparacion_institucion` FOREIGN KEY (`id_institucion`) REFERENCES `instituciones_educativas` (`id_institucion`),
  CONSTRAINT `fk_preparacion_oferente` FOREIGN KEY (`identificacion_oferente`) REFERENCES `oferentes` (`identificacion`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `preparacion_academica`
--

LOCK TABLES `preparacion_academica` WRITE;
/*!40000 ALTER TABLE `preparacion_academica` DISABLE KEYS */;
INSERT INTO `preparacion_academica` VALUES (1,'208400050',1,'Ingenieria Informatica','2020-01-10','2024-12-15'),(2,'603000824',5,'Administracion Recursos Humanos','2015-02-01','2019-11-20');
/*!40000 ALTER TABLE `preparacion_academica` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `preparacion_academica_asignacion`
--

DROP TABLE IF EXISTS `preparacion_academica_asignacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `preparacion_academica_asignacion` (
  `id_asignacion` int NOT NULL AUTO_INCREMENT,
  `id_preparacion` int NOT NULL,
  PRIMARY KEY (`id_asignacion`),
  KEY `id_preparacion` (`id_preparacion`),
  CONSTRAINT `preparacion_academica_asignacion_ibfk_1` FOREIGN KEY (`id_preparacion`) REFERENCES `preparacion_academica` (`id_preparacion`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `preparacion_academica_asignacion`
--

LOCK TABLES `preparacion_academica_asignacion` WRITE;
/*!40000 ALTER TABLE `preparacion_academica_asignacion` DISABLE KEYS */;
/*!40000 ALTER TABLE `preparacion_academica_asignacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `id_rol` int NOT NULL AUTO_INCREMENT,
  `nombre_rol` varchar(40) NOT NULL,
  PRIMARY KEY (`id_rol`),
  UNIQUE KEY `nombre_rol` (`nombre_rol`)
) ENGINE=InnoDB AUTO_INCREMENT=82 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'Administracion'),(25,'Administrador de Base de Datos'),(51,'Administrador de Redes'),(52,'Administrador de Sistemas'),(78,'Administrador Web'),(57,'Almacenista'),(48,'Analista de Datos'),(47,'Analista de Sistemas'),(44,'Arquitecto de Software'),(34,'Asesor Comercial'),(18,'Asistente'),(20,'Auditor'),(58,'Auxiliar Administrativo'),(59,'Auxiliar Contable'),(32,'Capacitador'),(55,'Chofer'),(49,'Científico de Datos'),(77,'Community Manager'),(33,'Consultor'),(19,'Contador'),(17,'Coordinador'),(71,'Coordinador Académico'),(42,'Coordinador de Proyectos'),(26,'Desarrollador'),(64,'Director Comercial'),(66,'Director de Operaciones'),(65,'Director de Tecnología'),(63,'Director Financiero'),(62,'Director General'),(46,'Diseñador UI'),(45,'Diseñador UX'),(72,'Docente'),(67,'Encargado de Calidad'),(68,'Encargado de Compras'),(24,'Encargado de Inventario'),(54,'Encargado de Logística'),(70,'Encargado de Recursos Humanos'),(69,'Encargado de Ventas'),(76,'Especialista en Marketing'),(50,'Especialista en Seguridad'),(14,'Gerente'),(29,'Ingeniero de Software'),(41,'Inspector'),(73,'Investigador'),(23,'Jefe de Compras'),(21,'Jefe de Recursos Humanos'),(22,'Jefe de Ventas'),(43,'Líder Técnico'),(56,'Mensajero'),(31,'Mesa de Ayuda'),(81,'NuevoRol'),(75,'Oficial de Cumplimiento'),(16,'Operador'),(61,'Pasante'),(60,'Practicante'),(79,'Product Owner'),(27,'Programador Junior'),(28,'Programador Senior'),(8,'PruebaRol'),(36,'Recepcionista'),(2,'Reclutador'),(35,'Representante de Servicio al Cliente'),(80,'Scrum Master'),(11,'SECRETARIA'),(37,'Secretario Ejecutivo'),(30,'Soporte Técnico'),(4,'Supervisor'),(40,'Supervisor de Calidad'),(38,'Supervisor de Operaciones'),(39,'Supervisor de Producción'),(53,'Técnico de Mantenimiento'),(74,'Tesorero');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rolpantalla`
--

DROP TABLE IF EXISTS `rolpantalla`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rolpantalla` (
  `id_rol` int NOT NULL,
  `id_pantalla` int NOT NULL,
  PRIMARY KEY (`id_rol`,`id_pantalla`),
  KEY `id_pantalla` (`id_pantalla`),
  CONSTRAINT `rolpantalla_ibfk_1` FOREIGN KEY (`id_rol`) REFERENCES `roles` (`id_rol`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `rolpantalla_ibfk_2` FOREIGN KEY (`id_pantalla`) REFERENCES `pantallas` (`id_pantalla`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rolpantalla`
--

LOCK TABLES `rolpantalla` WRITE;
/*!40000 ALTER TABLE `rolpantalla` DISABLE KEYS */;
INSERT INTO `rolpantalla` VALUES (1,1),(81,1),(1,2),(51,2),(81,2),(1,3),(2,3),(4,3),(11,3),(1,4),(2,4),(4,4),(1,5),(2,5),(1,6),(2,6),(1,7),(2,7),(4,7),(11,7),(1,8),(4,8),(51,8),(1,9),(1,10),(51,10),(1,11),(1,12),(11,12);
/*!40000 ALTER TABLE `rolpantalla` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuariorol`
--

DROP TABLE IF EXISTS `usuariorol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuariorol` (
  `id_usuario` int NOT NULL,
  `id_rol` int NOT NULL,
  PRIMARY KEY (`id_usuario`,`id_rol`),
  KEY `FK_UsuarioRol_Rol` (`id_rol`),
  CONSTRAINT `FK_UsuarioRol_Rol` FOREIGN KEY (`id_rol`) REFERENCES `roles` (`id_rol`),
  CONSTRAINT `FK_UsuarioRol_Usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuariorol`
--

LOCK TABLES `usuariorol` WRITE;
/*!40000 ALTER TABLE `usuariorol` DISABLE KEYS */;
INSERT INTO `usuariorol` VALUES (2,1),(19,1),(21,1),(19,2),(20,2),(22,11),(18,25),(17,44),(5,51),(18,51),(24,51),(17,52),(24,52),(16,57),(18,57),(16,78),(24,78),(23,81);
/*!40000 ALTER TABLE `usuariorol` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `id_usuario` int NOT NULL AUTO_INCREMENT,
  `nombre_usuario` varchar(50) NOT NULL,
  `nombre_completo` varchar(100) DEFAULT NULL,
  `contrasena` varchar(64) NOT NULL,
  `intentos_fallidos` int DEFAULT '0',
  `bloqueado` tinyint(1) DEFAULT '0',
  `estado` enum('activo','inactivo','bloqueado') DEFAULT 'activo',
  `correo` varchar(100) DEFAULT NULL,
  `id_rol` int DEFAULT NULL,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `UK_usuarios_nombre_usuario` (`nombre_usuario`),
  UNIQUE KEY `idx_usuario_rol` (`nombre_usuario`,`id_rol`),
  UNIQUE KEY `UK_usuarios_correo` (`correo`),
  KEY `fk_usuarios_roles` (`id_rol`),
  CONSTRAINT `fk_usuarios_roles` FOREIGN KEY (`id_rol`) REFERENCES `roles` (`id_rol`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (2,'admin','Johan Alvarado','3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2',0,0,'activo','Johan@gmail.com',1),(5,'Maria Ruiz','Maria Ruiz Lopez','b9e909168d8d6b8bd6dcc48023df6d91188babec308d0be1bf7e099ab33d75d9',0,0,'activo','marims@gmail.com',2),(10,'Naza','Nazareth Artavia Perez','e7a2c6361f41f1d3bdb9c7e495aa1c6e145c72c495fa825bcd5b92c08c12ee23',0,0,'activo','nanaz@gmail.com',2),(12,'Jana','Jana Jimenez ','556a554696d5dc3a980741f26b86a5c7828009e44209a6174ca4a4619718d8e9',0,0,'activo','jaajajaa@gmail.com',1),(13,'Luis ','Luis Lopez ','ca04a933ade82b6022a108b5b067664662a66884b6c4c47c822a530cc7b435be',0,0,'activo','lulu@gmail.com',2),(14,'Marta ','Marta Ortis','c250104c73484c83eb0290f0b39614477c4357e8836ea1a3e0e4ad7df2f5b600',0,0,'activo','marrr@gmail.com',11),(16,'Ryn','Ryn Foreman','d1f774abb26e1bd4c0ed1368ae6bfb2898ffc2b6cb0ce07b4f90e05d1e048e1a',0,0,'activo','rynryn@gmail.com',NULL),(17,'Megan','Megan Fox','4502fb9d69e339a0c6c940f406aac6ed91d985a06dc85336cdb0a30e08cfa77d',0,0,'activo','megan@gmail.com',NULL),(18,'Amanda ','Amanda Seyfield','7b34826a2599181aae933bf7b0d5d350296154e503b0916031a1eaf7972e0bda',0,0,'activo','Aman@gmail.com',NULL),(19,'multirol','Usuario Multirol','8c7693c6608a7e7f3b5073022d99ef5a9e9212c04e87077377b23450d1f0ce91',0,0,'activo','multirol@gmail.com',NULL),(20,'reclutador','Reclutador Prueba','4e92b6b732c53b51213c9fa9b4599925d78b834b4001359f0cf37bc9fb1e647c',0,0,'activo','reclutador@gmail.com',NULL),(21,'ADMINPRINCI','Josue Bonilla Quesada','28c90269567c579bcfe631bc7e0fdabff64775b9da6a76382f4dd5a56c71199b',0,0,'activo','quesadaBonilla@gmail.com',NULL),(22,'LauraRRHH','Laura Morales','c3bdf4258d4c7a50ea083865dfd4ffbc591846536adf72d8d69c81218c2101f0',0,0,'activo','morales124ll@gmail.com',NULL),(23,'nuevoUsuario','Nazareth Artavia','5b566783d0dec0c118c01ae233d023ab2de385bafa2d76c28a312f341f65a092',0,0,'activo','naza@gmail.com',NULL),(24,'Madeline','Madeline Cordero Ruiz ','43c7d37419b57d39aeca9ccc21f008b860668eb9c47f813ad736032f47734701',0,0,'activo','made@gmail.com',NULL);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-06-13 23:14:54
