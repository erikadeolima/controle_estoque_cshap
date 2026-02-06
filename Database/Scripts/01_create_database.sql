-- ==============================================
-- Script de Criação do Banco de Dados
-- Projeto: Sistema de Controle de Estoque
-- ==============================================

-- Criar banco de dados
CREATE DATABASE IF NOT EXISTS `controle_estoque` 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

-- Criar usuário (se não existir)
CREATE USER IF NOT EXISTS 'erikalima'@'localhost' IDENTIFIED BY 'erikalima';

-- Dar permissões ao usuário
GRANT ALL PRIVILEGES ON `controle_estoque`.* TO 'erikalima'@'localhost';
FLUSH PRIVILEGES;

-- Usar o banco de dados
USE `controle_estoque`;
