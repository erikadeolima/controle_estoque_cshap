-- ==============================================
-- Script para RESETAR o Banco de Dados
-- ⚠️ CUIDADO: Apaga TODOS os dados!
-- Use apenas em desenvolvimento/testes
-- ==============================================

USE `controle_estoque`;

-- Remover constraints temporariamente
SET FOREIGN_KEY_CHECKS = 0;

-- Limpar dados na ordem correta (respeitar FKs)
TRUNCATE TABLE Movements;
TRUNCATE TABLE Items;
TRUNCATE TABLE Products;
TRUNCATE TABLE Categories;
TRUNCATE TABLE Users;

-- Reativar constraints
SET FOREIGN_KEY_CHECKS = 1;

-- Agora repopular com dados iniciais (execute seed_data.sql após este script)
