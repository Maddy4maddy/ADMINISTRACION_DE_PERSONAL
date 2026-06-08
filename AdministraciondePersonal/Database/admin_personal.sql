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
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bitacora`
--

LOCK TABLES `bitacora` WRITE;
/*!40000 ALTER TABLE `bitacora` DISABLE KEYS */;
INSERT INTO `bitacora` VALUES (1,'2026-05-27 17:48:03','Usuario','Actualización de oferente Angie Romero Ceciliano'),(2,'2026-05-27 17:49:17','Usuario','Registro de oferente Yorleny Ceciliano Araya'),(3,'2026-05-27 17:49:44','Usuario','Actualización de oferente Andrea Gómez Solano'),(4,'2026-05-27 18:08:34','Usuario','Actualización de oferente Angie Romero Ceciliano'),(5,'2026-05-28 13:50:59','Usuario','Actualización de oferente Angie Romero Ceciliano'),(6,'2026-05-28 13:57:11','Usuario','Actualización de oferente Yorleny Ceciliano Araya'),(7,'2026-05-29 18:18:13','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(8,'2026-05-29 20:06:02','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(9,'2026-05-29 20:06:47','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(10,'2026-05-29 20:08:48','Usuario','Registro de oferente Juan Hidalgo Muñoz'),(11,'2026-05-29 20:13:04','Usuario','Actualización de oferente Santiago Madriz Ceciliano'),(12,'2026-05-29 20:13:17','Usuario','Actualización de oferente Andrea Gómez Solano'),(13,'2026-06-07 18:40:37','admin','Cambio de estado de usuario maddy: activo → inactivo'),(14,'2026-06-07 20:50:43','admin','Creación de usuario: {\"NombreUsuario\":\"Maria Ruiz\",\"NombreCompleto\":\"Maria Ruiz Lopez\",\"Correo\":\"marims@gmail.com\",\"Estado\":\"activo\",\"IdRol\":11}'),(15,'2026-06-07 21:00:10','Maria Ruiz','Cambio de estado de usuario maddy: activo → inactivo'),(16,'2026-06-07 21:00:26','Maria Ruiz','Cambio de estado de usuario maddy: inactivo → activo'),(17,'2026-06-07 21:00:46','Maria Ruiz','Actualización de usuario: {\"NombreUsuario\":\"Maria Ruiz\",\"NombreCompleto\":\"Maria Ruiz Lopez\",\"Correo\":\"marims@gmail.com\",\"Estado\":\"activo\",\"IdRol\":2}'),(18,'2026-06-07 21:10:36','admin','Creación de usuario: {\"NombreUsuario\":\"Josh Silver\",\"NombreCompleto\":\"Josh Silver\",\"Correo\":\"jonnysrr@gmail.com\",\"Estado\":\"activo\",\"IdRol\":1}'),(19,'2026-06-07 23:47:55','admin','El usuario consulta concursos.'),(20,'2026-06-07 23:48:03','admin','El usuario consulta concursos.'),(21,'2026-06-07 23:58:57','admin','El usuario consulta experiencia laboral del oferente .'),(22,'2026-06-07 23:59:01','admin','El usuario consulta experiencia laboral del oferente .');
/*!40000 ALTER TABLE `bitacora` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `entrevistas`
--

LOCK TABLES `entrevistas` WRITE;
/*!40000 ALTER TABLE `entrevistas` DISABLE KEYS */;
INSERT INTO `entrevistas` VALUES (4,'PAS12345',2,'2026-06-15 14:30:00','Pendiente'),(6,'208400050',2,'2026-06-17 13:00:00','Pendiente');
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `oferente_concurso`
--

LOCK TABLES `oferente_concurso` WRITE;
/*!40000 ALTER TABLE `oferente_concurso` DISABLE KEYS */;
INSERT INTO `oferente_concurso` VALUES (5,'208400050',1),(11,'603000824',6);
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
INSERT INTO `oferentes` VALUES ('208400050','Cédula de identidad','Angie Romero Ceciliano','2003-03-28','angieroceciliano@gmail.com','83258567'),('304800515','Cédula de identidad','Juan Hidalgo Muñoz','1994-08-12','juanhidalgom@gmail.com','75652797'),('603000824','Cédula de identidad','Yorleny Ceciliano Araya','1975-07-06','yorlececilianoa@gmail.com','72013595'),('DIMEX001','DIMEX','Santiago Madriz Ceciliano','1995-11-20','santiagomadrizc@gmail.com','87776655'),('PAS12345','Pasaporte','Andrea Gómez Solano','1999-06-08','andreagomezs@gmail.com','86665544');
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
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pantallas`
--

LOCK TABLES `pantallas` WRITE;
/*!40000 ALTER TABLE `pantallas` DISABLE KEYS */;
INSERT INTO `pantallas` VALUES (1,'Administración Roles','/Roles'),(2,'Administración Módulos','#'),(3,'Administración Usuarios','#'),(4,'Registro Oferentes','/Oferentes'),(5,'Registro Concursos','/Concursos'),(6,'Preparación Académica','/PreparacionAcademica'),(7,'Experiencia Laboral','/ExperienciaLaboral'),(8,'Agendar Entrevista','/Entrevistas'),(9,'Visualizar Bitácoras','/Bitacora'),(10,'Contratar Empleado','#'),(11,'Administración Puestos','#'),(12,'Administración Requisitos de Puestos','#'),(13,'Administración Áreas','#'),(14,'Administración Acciones de Personal','#'),(15,'Administración Parámetros','#'),(16,'Administración Compañías','#'),(17,'Cargar Datos de Ubicación','#'),(18,'Administración Instituciones Educativas','/InstitucionesEducativas'),(22,'Prueba_Pantalla','/prueba');
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `parametros`
--

LOCK TABLES `parametros` WRITE;
/*!40000 ALTER TABLE `parametros` DISABLE KEYS */;
INSERT INTO `parametros` VALUES (1,'MAX_INTENTOS_LOGIN','3'),(2,'DIAS_VIGENCIA_CONCURSO','30'),(3,'EDAD_MINIMA_OFERENTE','18'),(4,'MAX_ENTREVISTAS_OFERENTE','5'),(5,'CORREO_RRHH','rrhh@empresa.com'),(6,'NOMBRE_SISTEMA','Administracion de Personal ');
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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'Administracion'),(8,'PruebaRol'),(2,'Reclutador'),(11,'SECRETARIA'),(4,'Supervisor');
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
INSERT INTO `rolpantalla` VALUES (8,1),(11,2),(1,3),(8,4),(11,5),(11,6);
/*!40000 ALTER TABLE `rolpantalla` ENABLE KEYS */;
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
  UNIQUE KEY `nombre_usuario` (`nombre_usuario`),
  KEY `fk_usuarios_roles` (`id_rol`),
  CONSTRAINT `fk_usuarios_roles` FOREIGN KEY (`id_rol`) REFERENCES `roles` (`id_rol`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'maddy','Madeline Cordero','8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92',0,0,'activo','admin@gmail.com',11),(2,'admin','Johan Alvarado','3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2',0,0,'activo','Johan@gmail.com',1),(5,'Maria Ruiz','Maria Ruiz Lopez','b9e909168d8d6b8bd6dcc48023df6d91188babec308d0be1bf7e099ab33d75d9',0,0,'activo','marims@gmail.com',2),(6,'Josh Silver','Josh Silver','9c54ae47988f6dafaeb084deb2ecdcca772acee40cc2f4913cec327cb2b5a1f6',0,0,'activo','jonnysrr@gmail.com',1);
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

-- Dump completed on 2026-06-08  0:06:07
