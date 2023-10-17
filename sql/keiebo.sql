CREATE DATABASE keiebo_db;
USE keiebo_db;

CREATE TABLE `reuniones` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`usuario_id` INT NOT NULL,	
	`titulo` VARCHAR(255) DEFAULT NULL,
	`descripcion` VARCHAR(255) DEFAULT NULL,
	`estado` INT NOT NULL DEFAULT 1,
  	PRIMARY KEY(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE `reunion_usuarios` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`reunion_id` INT NOT NULL,
	`usuario_id` INT NOT NULL,
	`estado` INT NOT NULL DEFAULT 1,
  	PRIMARY KEY(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE `reunion_usuario_tareas` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`reunion_id` INT NOT NULL,
	`usuario_id` INT NOT NULL,
	`tarea` VARCHAR(255) NOT NULL,
	`estado` INT NOT NULL DEFAULT 1,
  	PRIMARY KEY(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE `reunion_ubicaciones` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`reunion_id` INT NOT NULL,
	`nombre` VARCHAR(255) NOT NULL,
	`descripcion` VARCHAR(255) NOT NULL,
	`latitud` VARCHAR(255) NOT NULL,
	`longitud` VARCHAR(255) NOT NULL,
	`desde` DATETIME DEFAULT NULL,
	`hasta` DATETIME DEFAULT NULL,
  	PRIMARY KEY(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE `usuarios` (
	`id` INT NOT NULL AUTO_INCREMENT,
	`nombre` VARCHAR(255) NOT NULL,
	`apellido` VARCHAR(255) NOT  NULL,
	`email` VARCHAR(160) DEFAULT NULL,
	`password` VARCHAR(160) DEFAULT NULL,
	`avatar` VARCHAR(160) DEFAULT NULL,
	`estado` INT NOT NULL DEFAULT 1,
  	PRIMARY KEY(`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

/* CONTRASEÑA EN AMBOS USUARIOS: asdasd */
INSERT INTO `usuarios` (`id`, `nombre`, `apellido`, `email`, `password`, `avatar`, `estado`) VALUES (NULL, 'Alberto', 'Valdez', 'aavaldez@gmail.com', 'o3P72xbu1tuJBR6BSKYhoBUSl64w2I7ZJ3ctKgPwD34=', NULL, '1');
INSERT INTO `usuarios` (`id`, `nombre`, `apellido`, `email`, `password`, `avatar`, `estado`) VALUES (NULL, 'Mariano', 'Luzza', 'mluzza@gmail.com', 'o3P72xbu1tuJBR6BSKYhoBUSl64w2I7ZJ3ctKgPwD34=', NULL, '1');

ALTER TABLE `reunion_usuarios` ADD CONSTRAINT `reunion_usuarios_reunion_id` FOREIGN KEY (`reunion_id`) REFERENCES `reuniones`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE `reunion_usuarios` ADD CONSTRAINT `reunion_usuarios_usuario_id` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `reunion_usuario_tareas` ADD CONSTRAINT `reunion_usuario_tareas_reunion_id` FOREIGN KEY (`reunion_id`) REFERENCES `reuniones`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE `reunion_usuario_tareas` ADD CONSTRAINT `reunion_usuario_tareas_usuario_id` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE `reunion_ubicaciones` ADD CONSTRAINT `reunion_ubicaciones_reunion_id` FOREIGN KEY (`reunion_id`) REFERENCES `reuniones`(`id`) ON DELETE CASCADE ON UPDATE CASCADE;
