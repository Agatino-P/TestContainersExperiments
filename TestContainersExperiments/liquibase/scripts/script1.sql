-- liquibase formatted sql

-- changeset dforcisi:SMS_API-1
-- DDL

CREATE DATABASE sms_provider_api;

CREATE TABLE `sms_provider_api`.`msgbus_inbox` (
  `_id`             BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `msg_id`          VARCHAR(128)    NOT NULL,
  `ts`              TIMESTAMP(6)    NOT NULL,
  
  PRIMARY KEY ( `_id` ),
  UNIQUE KEY ( `msg_id` ),
  INDEX `idx_purge` ( `ts` )
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
