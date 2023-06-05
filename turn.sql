-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         10.4.24-MariaDB - mariadb.org binary distribution
-- SO del servidor:              Win64
-- HeidiSQL Versión:             12.2.0.6576
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para turnero
CREATE DATABASE IF NOT EXISTS `turnero` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `turnero`;

-- Volcando estructura para tabla turnero.clientes
CREATE TABLE IF NOT EXISTS `clientes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) DEFAULT NULL,
  `apellido` varchar(50) DEFAULT NULL,
  `mail` varchar(50) DEFAULT NULL,
  `pass` varchar(50) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  `telefono` varchar(50) DEFAULT NULL,
  `direccion` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.clientes: ~2 rows (aproximadamente)
INSERT INTO `clientes` (`id`, `nombre`, `apellido`, `mail`, `pass`, `activo`, `telefono`, `direccion`) VALUES
	(5, 'jose', 'cliente', 'cliente@mail', 'complicado', 1, '225544', 'calle 123'),
	(6, 'juan', 'cliente', 'cliente@juan', 'comlicadotambien', 1, '112255', 'calle 1234');

-- Volcando estructura para tabla turnero.clientes_mascotas
CREATE TABLE IF NOT EXISTS `clientes_mascotas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `mascotaid` int(11) DEFAULT NULL,
  `clienteid` int(11) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  PRIMARY KEY (`id`),
  KEY `fk_clientes_mascotas_clientes` (`clienteid`),
  KEY `fk_clientes_mascotas_mascotas` (`mascotaid`),
  CONSTRAINT `fk_clientes_mascotas_clientes` FOREIGN KEY (`clienteid`) REFERENCES `clientes` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_clientes_mascotas_mascotas` FOREIGN KEY (`mascotaid`) REFERENCES `mascotas` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.clientes_mascotas: ~4 rows (aproximadamente)
INSERT INTO `clientes_mascotas` (`id`, `mascotaid`, `clienteid`, `activo`) VALUES
	(1, 1, 5, 1),
	(2, 2, 5, 1),
	(3, 3, 6, 1),
	(4, 4, 6, 1);

-- Volcando estructura para tabla turnero.consultas
CREATE TABLE IF NOT EXISTS `consultas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `estado` int(11) DEFAULT NULL,
  `tiempoinicio` datetime DEFAULT NULL,
  `tiempofin` datetime DEFAULT NULL,
  `cliente_mascotaid` int(11) DEFAULT NULL,
  `empleadoid` int(11) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  `detalle` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `empleadoId` (`empleadoid`),
  KEY `fk_turnos_clientes_mascotas` (`cliente_mascotaid`),
  CONSTRAINT `fk_turno_empleado` FOREIGN KEY (`empleadoid`) REFERENCES `empleados` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_turnos_clientes_mascotas` FOREIGN KEY (`cliente_mascotaid`) REFERENCES `clientes_mascotas` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.consultas: ~1 rows (aproximadamente)
INSERT INTO `consultas` (`id`, `estado`, `tiempoinicio`, `tiempofin`, `cliente_mascotaid`, `empleadoid`, `activo`, `detalle`) VALUES
	(1, 1, '2023-05-31 09:00:00', '2023-05-31 09:30:00', 1, 5, 1, '"le agarro la chiripiorca"');

-- Volcando estructura para tabla turnero.empleados
CREATE TABLE IF NOT EXISTS `empleados` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) DEFAULT NULL,
  `apellido` varchar(50) DEFAULT NULL,
  `mail` varchar(50) DEFAULT NULL,
  `pass` varchar(50) DEFAULT NULL,
  `rolid` int(11) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  `telefono` varchar(50) DEFAULT NULL,
  `sucursalid` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `idx_empleado` (`rolid`),
  KEY `fk_empleados_sucursales` (`sucursalid`),
  CONSTRAINT `fk_empleado_rol` FOREIGN KEY (`rolid`) REFERENCES `roles` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_empleados_sucursales` FOREIGN KEY (`sucursalid`) REFERENCES `sucursales` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.empleados: ~2 rows (aproximadamente)
INSERT INTO `empleados` (`id`, `nombre`, `apellido`, `mail`, `pass`, `rolid`, `activo`, `telefono`, `sucursalid`) VALUES
	(5, 'luis', 'doc', 'luis@doc', 'asd123', 3, 1, '44321', 1),
	(6, 'laura', 'pelu', 'lau@pelu', 'dsa321', 3, 1, '15123', 1);

-- Volcando estructura para tabla turnero.empleados_tareas
CREATE TABLE IF NOT EXISTS `empleados_tareas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `empleadoid` int(11) DEFAULT NULL,
  `tareaid` int(11) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  PRIMARY KEY (`id`),
  KEY `fk_empleados_tareas_empleados` (`empleadoid`),
  KEY `fk_empleados_tareas_sucursales` (`tareaid`),
  CONSTRAINT `fk_empleados_tareas_empleados` FOREIGN KEY (`empleadoid`) REFERENCES `empleados` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_empleados_tareas_sucursales` FOREIGN KEY (`tareaid`) REFERENCES `tareas` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.empleados_tareas: ~6 rows (aproximadamente)
INSERT INTO `empleados_tareas` (`id`, `empleadoid`, `tareaid`, `activo`) VALUES
	(1, 5, 3, 1),
	(2, 5, 4, 1),
	(3, 5, 5, 1),
	(4, 5, 6, 1),
	(5, 6, 1, 1),
	(7, 6, 2, 1);

-- Volcando estructura para tabla turnero.mascotas
CREATE TABLE IF NOT EXISTS `mascotas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) DEFAULT NULL,
  `apellido` varchar(50) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  `fechanacimiento` date DEFAULT NULL,
  `peso` float DEFAULT NULL,
  `foto` varchar(50) DEFAULT NULL,
  `datos_varios` varchar(200) DEFAULT NULL,
  `uid` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.mascotas: ~4 rows (aproximadamente)
INSERT INTO `mascotas` (`id`, `nombre`, `apellido`, `activo`, `fechanacimiento`, `peso`, `foto`, `datos_varios`, `uid`) VALUES
	(1, 'bobi', 'gomez', 1, '2018-05-31', 20, NULL, NULL, '123123'),
	(2, 'gervacio el chiguagua', 'gomez', 1, '2020-05-31', 15, NULL, NULL, '321123'),
	(3, 'mortadela', 'paladini', 1, '2018-05-31', 54, NULL, NULL, '43331'),
	(4, 'salchichon', 'prima', 1, '2021-05-31', 14, NULL, NULL, '4522');

-- Volcando estructura para tabla turnero.roles
CREATE TABLE IF NOT EXISTS `roles` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `rol` varchar(50) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.roles: ~2 rows (aproximadamente)
INSERT INTO `roles` (`id`, `rol`, `activo`) VALUES
	(1, 'Admin', 1),
	(2, 'Usuario', 1),
	(3, 'Empleado', 1);

-- Volcando estructura para tabla turnero.sucursales
CREATE TABLE IF NOT EXISTS `sucursales` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) NOT NULL DEFAULT '0',
  `Telefono` varchar(50) DEFAULT NULL,
  `Direccion` varchar(50) DEFAULT NULL,
  `Redsocial` varchar(50) DEFAULT NULL,
  `horario` varchar(50) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.sucursales: ~0 rows (aproximadamente)
INSERT INTO `sucursales` (`id`, `Nombre`, `Telefono`, `Direccion`, `Redsocial`, `horario`, `activo`) VALUES
	(1, 'Vete riobamba', '321123', 'riobamba al 455', 'vete@insta', '09-14 y 17-21', 1);

-- Volcando estructura para tabla turnero.tareas
CREATE TABLE IF NOT EXISTS `tareas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tarea` varchar(50) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  `tiempo` time DEFAULT NULL,
  `precio` decimal(20,6) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.tareas: ~6 rows (aproximadamente)
INSERT INTO `tareas` (`id`, `tarea`, `activo`, `tiempo`, `precio`) VALUES
	(1, 'lavado canino menor a 20 kl', 1, '01:30:00', 1500.000000),
	(2, 'lavado canino mayor a 20 kl', 1, '02:00:00', 2000.000000),
	(3, 'consulta veterinaria', 1, '00:30:00', 4000.000000),
	(4, 'prequirurgico', 1, '00:40:00', 5000.000000),
	(5, 'cirugia menor', 1, '01:30:00', 8000.000000),
	(6, 'cirugia', 1, '02:00:00', 12500.000000);

-- Volcando estructura para tabla turnero.turnos
CREATE TABLE IF NOT EXISTS `turnos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `monday_ini` time DEFAULT NULL,
  `monday_fin` time DEFAULT NULL,
  `tuesday_ini` time DEFAULT NULL,
  `tuesday_fin` time DEFAULT NULL,
  `wednesday_ini` time DEFAULT NULL,
  `wednesday_fin` time DEFAULT NULL,
  `thursday_ini` time DEFAULT NULL,
  `thursday_fin` time DEFAULT NULL,
  `friday_ini` time DEFAULT NULL,
  `friday_fin` time DEFAULT NULL,
  `saturday_ini` time DEFAULT NULL,
  `saturday_fin` time DEFAULT NULL,
  `sunday_ini` time DEFAULT NULL,
  `sunday_fin` time DEFAULT NULL,
  `empleadoId` int(11) DEFAULT NULL,
  `activo` int(11) DEFAULT 1,
  PRIMARY KEY (`id`),
  KEY `fk_turnos_empleados` (`empleadoId`),
  CONSTRAINT `fk_turnos_empleados` FOREIGN KEY (`empleadoId`) REFERENCES `empleados` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

-- Volcando datos para la tabla turnero.turnos: ~4 rows (aproximadamente)
INSERT INTO `turnos` (`id`, `monday_ini`, `monday_fin`, `tuesday_ini`, `tuesday_fin`, `wednesday_ini`, `wednesday_fin`, `thursday_ini`, `thursday_fin`, `friday_ini`, `friday_fin`, `saturday_ini`, `saturday_fin`, `sunday_ini`, `sunday_fin`, `empleadoId`, `activo`) VALUES
	(2, '09:00:00', '13:00:00', '09:00:00', '13:00:00', '09:00:00', '13:00:00', '09:00:00', '13:00:00', '09:00:00', '13:00:00', '09:00:00', '14:00:00', NULL, NULL, 5, 1),
	(3, '17:00:00', '21:00:00', '17:00:00', '21:00:00', '17:00:00', '21:00:00', '17:00:00', '21:00:00', '17:00:00', '21:00:00', NULL, NULL, NULL, NULL, 5, 1),
	(4, '10:00:00', '14:00:00', '10:00:00', '14:00:00', '10:00:00', '14:00:00', '10:00:00', '14:00:00', '10:00:00', '14:00:00', '10:00:00', '14:00:00', NULL, NULL, 6, 1),
	(5, '17:00:00', '21:00:00', '17:00:00', '21:00:00', '17:00:00', '21:00:00', '17:00:00', '21:00:00', '17:00:00', '21:00:00', NULL, NULL, NULL, NULL, 6, 1);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
