/*
 Navicat Premium Data Transfer

 Source Server         : mysql1
 Source Server Type    : MySQL
 Source Server Version : 80017
 Source Host           : localhost:3306
 Source Schema         : go_ab

 Target Server Type    : MySQL
 Target Server Version : 80017
 File Encoding         : 65001

 Date: 28/09/2022 22:48:25
*/

CREATE database IF NOT EXISTS `go_ab`;

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `go_ab`.`user`;
CREATE TABLE  `go_ab`.`user`  (
  `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `pass` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `name`(`name`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO  `go_ab`.`user` VALUES (1, 'admin123', 'admin123');
INSERT INTO  `go_ab`.`user` VALUES (2, 'admin1234', 'admin123');
INSERT INTO  `go_ab`.`user` VALUES (3, 'admin12345', 'admin123');

SET FOREIGN_KEY_CHECKS = 1;
