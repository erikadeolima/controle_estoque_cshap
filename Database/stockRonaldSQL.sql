-- CREATE SCHEMA if not exists `Stock Ronald`;
-- USE `Stock Ronald`;
USE `controle_estoque`;

-- Limpar tabelas existentes se houver
SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS movement;
DROP TABLE IF EXISTS item;
DROP TABLE IF EXISTS product;
DROP TABLE IF EXISTS user;
DROP TABLE IF EXISTS category;
SET FOREIGN_KEY_CHECKS = 1;

CREATE TABLE if not exists category (
category_id INT AUTO_INCREMENT PRIMARY KEY,
name VARCHAR(255) NOT NULL,
description VARCHAR(200),
creation_date TIMESTAMP DEFAULT NOW() 
);
CREATE TABLE if not exists product (
product_id INT AUTO_INCREMENT PRIMARY KEY,
sku VARCHAR(45) NOT NULL,
name VARCHAR(200) NOT NULL,
status TINYINT DEFAULT 1,
minimum_quantity INT,
creation_date TIMESTAMP DEFAULT NOW(),
category_id INT NOT NULL,
CONSTRAINT fk_product_category
	FOREIGN KEY (category_id)
    REFERENCES category(category_id)
);

CREATE TABLE if not exists item (
    item_id INT AUTO_INCREMENT PRIMARY KEY,
    batch VARCHAR(55) NOT NULL,
    expiration_date TIMESTAMP,
    quantity INT NOT NULL DEFAULT 0,
    location VARCHAR(100),
    status TINYINT DEFAULT 1,
    product_id INT NOT NULL,

    CONSTRAINT fk_item_product
        FOREIGN KEY (product_id)
        REFERENCES product(product_id)
);

CREATE TABLE if not exists `user` (
    id_user INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    email VARCHAR(100) NOT NULL,
    profile VARCHAR(50) NOT NULL
);

CREATE TABLE if not exists movement (
    movement_id INT AUTO_INCREMENT PRIMARY KEY,
    date TIMESTAMP DEFAULT NOW(),
    type VARCHAR(45) NOT NULL,
    quantity_moved INT NOT NULL,
    previous_quantity INT NOT NULL,
    new_quantity INT NOT NULL,
    item_id INT NOT NULL,
    CONSTRAINT fk_movement_item
        FOREIGN KEY (item_id)
        REFERENCES item(item_id),
    user_id INT NOT NULL,
    CONSTRAINT fk_movement_user
        FOREIGN KEY (user_id)
        REFERENCES `user`(id_user)
);

-- 3. INSERIR DADOS - USUÁRIOS
INSERT INTO `user` (name, email, profile) VALUES
('Carlos Admin', 'carlos@estoque.com', 'Administrador'),
('Ana Operadora', 'ana@estoque.com', 'Operador'),
('Pedro Supervisor', 'pedro@estoque.com', 'Supervisor');

-- 4. INSERIR DADOS - CATEGORIAS (LANCHONETE)
INSERT INTO category (name, description, creation_date) VALUES
('Hambúrgueres e Carnes', 'Carnes moídas, frango e preparados para hambúrgueres', NOW()),
('Pães e Massas', 'Pães de hambúrguer, pães de hot dog e massas diversas', NOW()),
('Laticínios', 'Queijo, leite, creme de leite e derivados', NOW()),
('Molhos e Condimentos', 'Molhos, temperos e condimentos', NOW()),
('Batatas e Acompanhamentos', 'Batata frita, onion rings e acompanhamentos', NOW()),
('Bebidas', 'Refrigerantes, sucos e bebidas diversas', NOW()),
('Sobremesas', 'Sorvetes, toppings e sobremesas', NOW()),
('Embalagens', 'Copos, embalagens e descartáveis', NOW());

-- 5. INSERIR DADOS - PRODUTOS
INSERT INTO product (sku, name, status, minimum_quantity, creation_date, category_id) VALUES
-- Hambúrgueres e Carnes
('HAM-001', 'Carne Moída 100g Hamburguer', 1, 200, NOW(), 1),
('HAM-002', 'Carne Moída 150g Quarter Pounder', 1, 150, NOW(), 1),
('HAM-003', 'Frango Empanado para Nuggets', 1, 100, NOW(), 1),
('HAM-004', 'Peito de Frango Cru', 1, 80, NOW(), 1),
-- Pães e Massas
('PAO-001', 'Pão de Hambúrguer Pequeno', 1, 500, NOW(), 2),
('PAO-002', 'Pão de Hambúrguer Grande', 1, 300, NOW(), 2),
('PAO-003', 'Pão de Hot Dog', 1, 400, NOW(), 2),
('PAO-004', 'Biscoito para Chicken Sandwich', 1, 250, NOW(), 2),
-- Laticínios
('LAT-001', 'Queijo Cheddar Fatiado', 1, 100, NOW(), 3),
('LAT-002', 'Queijo Mussarela', 1, 80, NOW(), 3),
('LAT-003', 'Leite Integral', 1, 50, NOW(), 3),
('LAT-004', 'Creme de Leite', 1, 40, NOW(), 3),
('LAT-005', 'Manteiga', 1, 30, NOW(), 3),
-- Molhos e Condimentos
('MOL-001', 'Molho Barbecue', 1, 30, NOW(), 4),
('MOL-002', 'Molho Mostarda', 1, 30, NOW(), 4),
('MOL-003', 'Molho Ketchup', 1, 40, NOW(), 4),
('MOL-004', 'Molho Maionese', 1, 25, NOW(), 4),
('MOL-005', 'Molho Ranch', 1, 20, NOW(), 4),
('MOL-006', 'Sal Refinado', 1, 50, NOW(), 4),
-- Batatas e Acompanhamentos
('BAT-001', 'Batata Congelada para Frita', 1, 200, NOW(), 5),
('BAT-002', 'Onion Rings Congelado', 1, 80, NOW(), 5),
('BAT-003', 'Batata Doce Congelada', 1, 60, NOW(), 5),
-- Bebidas
('BEB-001', 'Xarope de Refrigerante Cola', 1, 20, NOW(), 6),
('BEB-002', 'Xarope de Refrigerante Uva', 1, 15, NOW(), 6),
('BEB-003', 'Xarope de Refrigerante Limão', 1, 15, NOW(), 6),
('BEB-004', 'Leite em Pó para Café', 1, 25, NOW(), 6),
('BEB-005', 'Café Moído', 1, 20, NOW(), 6),
('BEB-006', 'Água Mineral 500ml', 1, 100, NOW(), 6),
-- Sobremesas
('SOB-001', 'Sorvete de Baunilha', 1, 40, NOW(), 7),
('SOB-002', 'Calda de Chocolate', 1, 15, NOW(), 7),
('SOB-003', 'Calda de Caramelo', 1, 12, NOW(), 7),
('SOB-004', 'Granulado Colorido', 1, 10, NOW(), 7),
-- Embalagens
('EMB-001', 'Copos 300ml para Refrigerante', 1, 1000, NOW(), 8),
('EMB-002', 'Copos 500ml para Refrigerante', 1, 800, NOW(), 8),
('EMB-003', 'Embalagem para Hambúrguer', 1, 1500, NOW(), 8),
('EMB-004', 'Embalagem para Batata', 1, 600, NOW(), 8),
('EMB-005', 'Guardanapos', 1, 2000, NOW(), 8),
('EMB-006', 'Canudos', 1, 1500, NOW(), 8);

-- 6. INSERIR DADOS - ITENS DE ESTOQUE
INSERT INTO item (batch, expiration_date, quantity, location, status, product_id) VALUES
-- Hambúrgueres e Carnes
('LOTE-HAM001-001', DATE_ADD(NOW(), INTERVAL 3 DAY), 500, 'F1-P1', 1, 1),
('LOTE-HAM002-001', DATE_ADD(NOW(), INTERVAL 3 DAY), 350, 'F1-P2', 1, 2),
('LOTE-HAM003-001', DATE_ADD(NOW(), INTERVAL 6 MONTH), 200, 'F1-P3', 1, 3),
('LOTE-HAM004-001', DATE_ADD(NOW(), INTERVAL 5 DAY), 150, 'F1-P4', 1, 4),
-- Pães e Massas
('LOTE-PAO001-001', DATE_ADD(NOW(), INTERVAL 7 DAY), 1200, 'F2-P1', 1, 5),
('LOTE-PAO002-001', DATE_ADD(NOW(), INTERVAL 7 DAY), 800, 'F2-P2', 1, 6),
('LOTE-PAO003-001', DATE_ADD(NOW(), INTERVAL 10 DAY), 1000, 'F2-P3', 1, 7),
('LOTE-PAO004-001', DATE_ADD(NOW(), INTERVAL 30 DAY), 600, 'F2-P4', 1, 8),
-- Laticínios
('LOTE-LAT001-001', DATE_ADD(NOW(), INTERVAL 14 DAY), 250, 'D1-P1', 1, 9),
('LOTE-LAT002-001', DATE_ADD(NOW(), INTERVAL 10 DAY), 200, 'D1-P2', 1, 10),
('LOTE-LAT003-001', DATE_ADD(NOW(), INTERVAL 7 DAY), 100, 'D1-P3', 1, 11),
('LOTE-LAT004-001', DATE_ADD(NOW(), INTERVAL 5 DAY), 80, 'D1-P4', 1, 12),
('LOTE-LAT005-001', DATE_ADD(NOW(), INTERVAL 30 DAY), 60, 'D1-P5', 1, 13),
-- Molhos e Condimentos
('LOTE-MOL001-001', DATE_ADD(NOW(), INTERVAL 12 MONTH), 80, 'E1-P1', 1, 14),
('LOTE-MOL002-001', DATE_ADD(NOW(), INTERVAL 12 MONTH), 80, 'E1-P2', 1, 15),
('LOTE-MOL003-001', DATE_ADD(NOW(), INTERVAL 12 MONTH), 100, 'E1-P3', 1, 16),
('LOTE-MOL004-001', DATE_ADD(NOW(), INTERVAL 6 MONTH), 60, 'E1-P4', 1, 17),
('LOTE-MOL005-001', DATE_ADD(NOW(), INTERVAL 6 MONTH), 50, 'E1-P5', 1, 18),
('LOTE-MOL006-001', DATE_ADD(NOW(), INTERVAL 24 MONTH), 150, 'E1-P6', 1, 19),
-- Batatas e Acompanhamentos
('LOTE-BAT001-001', DATE_ADD(NOW(), INTERVAL 9 MONTH), 400, 'B1-P1', 1, 20),
('LOTE-BAT002-001', DATE_ADD(NOW(), INTERVAL 8 MONTH), 180, 'B1-P2', 1, 21),
('LOTE-BAT003-001', DATE_ADD(NOW(), INTERVAL 10 MONTH), 120, 'B1-P3', 1, 22),
-- Bebidas
('LOTE-BEB001-001', DATE_ADD(NOW(), INTERVAL 12 MONTH), 50, 'C1-P1', 1, 23),
('LOTE-BEB002-001', DATE_ADD(NOW(), INTERVAL 12 MONTH), 40, 'C1-P2', 1, 24),
('LOTE-BEB003-001', DATE_ADD(NOW(), INTERVAL 12 MONTH), 40, 'C1-P3', 1, 25),
('LOTE-BEB004-001', DATE_ADD(NOW(), INTERVAL 12 MONTH), 60, 'C1-P4', 1, 26),
('LOTE-BEB005-001', DATE_ADD(NOW(), INTERVAL 6 MONTH), 50, 'C1-P5', 1, 27),
('LOTE-BEB006-001', DATE_ADD(NOW(), INTERVAL 24 MONTH), 300, 'C1-P6', 1, 28),
-- Sobremesas
('LOTE-SOB001-001', DATE_ADD(NOW(), INTERVAL 4 MONTH), 100, 'D2-P1', 1, 29),
('LOTE-SOB002-001', DATE_ADD(NOW(), INTERVAL 9 MONTH), 40, 'D2-P2', 1, 30),
('LOTE-SOB003-001', DATE_ADD(NOW(), INTERVAL 9 MONTH), 35, 'D2-P3', 1, 31),
('LOTE-SOB004-001', DATE_ADD(NOW(), INTERVAL 12 MONTH), 25, 'D2-P4', 1, 32),
-- Embalagens
('LOTE-EMB001-001', NULL, 2500, 'A1-P1', 1, 33),
('LOTE-EMB002-001', NULL, 2000, 'A1-P2', 1, 34),
('LOTE-EMB003-001', NULL, 3500, 'A1-P3', 1, 35),
('LOTE-EMB004-001', NULL, 1500, 'A1-P4', 1, 36),
('LOTE-EMB005-001', NULL, 5000, 'A2-P1', 1, 37),
('LOTE-EMB006-001', NULL, 4000, 'A2-P2', 1, 38);

-- 7. INSERIR DADOS - MOVIMENTAÇÕES
INSERT INTO movement (date, type, quantity_moved, previous_quantity, new_quantity, item_id, user_id) VALUES
(DATE_SUB(NOW(), INTERVAL 2 DAY), 'Entrada', 500, 0, 500, 1, 1),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Saída', 120, 500, 380, 1, 2),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Entrada', 350, 0, 350, 2, 1),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Entrada', 200, 0, 200, 3, 3),
(DATE_SUB(NOW(), INTERVAL 2 DAY), 'Entrada', 1200, 0, 1200, 5, 1),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Saída', 200, 1200, 1000, 5, 2),
(DATE_SUB(NOW(), INTERVAL 2 DAY), 'Entrada', 800, 0, 800, 6, 1),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Entrada', 250, 0, 250, 9, 1),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Saída', 50, 250, 200, 9, 2),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Entrada', 200, 0, 200, 10, 3),
(DATE_SUB(NOW(), INTERVAL 12 HOUR), 'Saída', 30, 200, 170, 10, 2),
(DATE_SUB(NOW(), INTERVAL 3 DAY), 'Entrada', 80, 0, 80, 14, 1),
(DATE_SUB(NOW(), INTERVAL 2 DAY), 'Entrada', 80, 0, 80, 15, 1),
(DATE_SUB(NOW(), INTERVAL 2 DAY), 'Entrada', 100, 0, 100, 16, 3),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Saída', 25, 100, 75, 16, 2),
(DATE_SUB(NOW(), INTERVAL 4 DAY), 'Entrada', 400, 0, 400, 20, 1),
(DATE_SUB(NOW(), INTERVAL 2 DAY), 'Saída', 80, 400, 320, 20, 2),
(DATE_SUB(NOW(), INTERVAL 3 DAY), 'Entrada', 180, 0, 180, 21, 3),
(DATE_SUB(NOW(), INTERVAL 5 DAY), 'Entrada', 50, 0, 50, 23, 1),
(DATE_SUB(NOW(), INTERVAL 3 DAY), 'Saída', 15, 50, 35, 23, 2),
(DATE_SUB(NOW(), INTERVAL 4 DAY), 'Entrada', 300, 0, 300, 28, 1),
(DATE_SUB(NOW(), INTERVAL 2 DAY), 'Entrada', 100, 0, 100, 29, 1),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Saída', 20, 100, 80, 29, 3),
(DATE_SUB(NOW(), INTERVAL 1 DAY), 'Entrada', 40, 0, 40, 30, 2),
(DATE_SUB(NOW(), INTERVAL 7 DAY), 'Entrada', 2500, 0, 2500, 33, 1),
(DATE_SUB(NOW(), INTERVAL 5 DAY), 'Saída', 500, 2500, 2000, 33, 2),
(DATE_SUB(NOW(), INTERVAL 6 DAY), 'Entrada', 3500, 0, 3500, 35, 1),
(DATE_SUB(NOW(), INTERVAL 4 DAY), 'Saída', 300, 3500, 3200, 35, 3),
(DATE_SUB(NOW(), INTERVAL 3 DAY), 'Entrada', 5000, 0, 5000, 37, 1);


