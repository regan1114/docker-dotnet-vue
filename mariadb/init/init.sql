
CREATE DATABASE IF NOT EXISTS `vuenet` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE vuenet;

/*
 Navicat Premium Data Transfer

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 100238
 Source Host           : localhost:3306
 Source Schema         : shopline

 Target Server Type    : MySQL
 Target Server Version : 100238
 File Encoding         : 65001

 Date: 20/06/2021 18:06:47
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `password` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `token` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `last_login_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ----------------------------
-- Records of users
-- ----------------------------
BEGIN;
INSERT INTO `users` VALUES (1, 'admin', '21232F297A57A5A743894A0E4A801FC3', NULL, NULL);
COMMIT;

SET FOREIGN_KEY_CHECKS = 1;
