-- ==============================================
-- Script de Dados Iniciais (SEED DATA)
-- Execute APÓS aplicar as migrations do Entity Framework
-- Exemplo: Lanchonete/Hamburgueria
-- ==============================================

USE `controle_estoque`;

-- ========== 1. INSERIR USUÁRIOS ==========
INSERT INTO user (name, email, profile) VALUES
('Carlos Admin', 'carlos@estoque.com', 'Administrador'),
('Ana Operadora', 'ana@estoque.com', 'Operador'),
('Pedro Supervisor', 'pedro@estoque.com', 'Supervisor')
ON DUPLICATE KEY UPDATE name=VALUES(name);

-- ========== 2. INSERIR CATEGORIAS ==========
INSERT INTO category (name, description, creation_date) VALUES
('Hambúrgueres e Carnes', 'Carnes moídas, frango e preparados para hambúrgueres', NOW()),
('Pães e Massas', 'Pães de hambúrguer, pães de hot dog e massas diversas', NOW()),
('Laticínios', 'Queijo, leite, creme de leite e derivados', NOW()),
('Molhos e Condimentos', 'Molhos, temperos e condimentos', NOW()),
('Batatas e Acompanhamentos', 'Batata frita, onion rings e acompanhamentos', NOW()),
('Bebidas', 'Refrigerantes, sucos e bebidas diversas', NOW()),
('Sobremesas', 'Sorvetes, toppings e sobremesas', NOW()),
('Embalagens', 'Copos, embalagens e descartáveis', NOW())
ON DUPLICATE KEY UPDATE name=VALUES(name);

-- ========== 3. INSERIR PRODUTOS ==========
-- Hambúrgueres e Carnes
INSERT INTO product (sku, name, status, minimum_quantity, category_id, creation_date) VALUES
('HAM-001', 'Carne Moída 100g Hamburguer', 1, 200, (SELECT category_id FROM category WHERE name='Hambúrgueres e Carnes' LIMIT 1), NOW()),
('HAM-002', 'Carne Moída 150g Quarter Pounder', 1, 150, (SELECT category_id FROM category WHERE name='Hambúrgueres e Carnes' LIMIT 1), NOW()),
('HAM-003', 'Frango Empanado para Nuggets', 1, 100, (SELECT category_id FROM category WHERE name='Hambúrgueres e Carnes' LIMIT 1), NOW()),
('HAM-004', 'Peito de Frango Cru', 1, 80, (SELECT category_id FROM category WHERE name='Hambúrgueres e Carnes' LIMIT 1), NOW()),

-- Pães e Massas
('PAO-001', 'Pão de Hambúrguer Pequeno', 1, 500, (SELECT category_id FROM category WHERE name='Pães e Massas' LIMIT 1), NOW()),
('PAO-002', 'Pão de Hambúrguer Grande', 1, 300, (SELECT category_id FROM category WHERE name='Pães e Massas' LIMIT 1), NOW()),
('PAO-003', 'Pão de Hot Dog', 1, 400, (SELECT category_id FROM category WHERE name='Pães e Massas' LIMIT 1), NOW()),
('PAO-004', 'Biscoito para Chicken Sandwich', 1, 250, (SELECT category_id FROM category WHERE name='Pães e Massas' LIMIT 1), NOW()),

-- Laticínios
('LAT-001', 'Queijo Cheddar Fatiado', 1, 100, (SELECT category_id FROM category WHERE name='Laticínios' LIMIT 1), NOW()),
('LAT-002', 'Queijo Mussarela', 1, 80, (SELECT category_id FROM category WHERE name='Laticínios' LIMIT 1), NOW()),
('LAT-003', 'Leite Integral', 1, 50, (SELECT category_id FROM category WHERE name='Laticínios' LIMIT 1), NOW()),
('LAT-004', 'Creme de Leite', 1, 40, (SELECT category_id FROM category WHERE name='Laticínios' LIMIT 1), NOW()),
('LAT-005', 'Manteiga', 1, 30, (SELECT category_id FROM category WHERE name='Laticínios' LIMIT 1), NOW()),

-- Molhos e Condimentos
('MOL-001', 'Molho Barbecue', 1, 30, (SELECT category_id FROM category WHERE name='Molhos e Condimentos' LIMIT 1), NOW()),
('MOL-002', 'Molho Mostarda', 1, 30, (SELECT category_id FROM category WHERE name='Molhos e Condimentos' LIMIT 1), NOW()),
('MOL-003', 'Molho Ketchup', 1, 40, (SELECT category_id FROM category WHERE name='Molhos e Condimentos' LIMIT 1), NOW()),
('MOL-004', 'Molho Maionese', 1, 25, (SELECT category_id FROM category WHERE name='Molhos e Condimentos' LIMIT 1), NOW()),
('MOL-005', 'Molho Ranch', 1, 20, (SELECT category_id FROM category WHERE name='Molhos e Condimentos' LIMIT 1), NOW()),
('MOL-006', 'Sal Refinado', 1, 50, (SELECT category_id FROM category WHERE name='Molhos e Condimentos' LIMIT 1), NOW()),

-- Batatas e Acompanhamentos
('BAT-001', 'Batata Congelada para Frita', 1, 200, (SELECT category_id FROM category WHERE name='Batatas e Acompanhamentos' LIMIT 1), NOW()),
('BAT-002', 'Onion Rings Congelado', 1, 80, (SELECT category_id FROM category WHERE name='Batatas e Acompanhamentos' LIMIT 1), NOW()),
('BAT-003', 'Batata Doce Congelada', 1, 60, (SELECT category_id FROM category WHERE name='Batatas e Acompanhamentos' LIMIT 1), NOW()),

-- Bebidas
('BEB-001', 'Xarope de Refrigerante Cola', 1, 20, (SELECT category_id FROM category WHERE name='Bebidas' LIMIT 1), NOW()),
('BEB-002', 'Xarope de Refrigerante Uva', 1, 15, (SELECT category_id FROM category WHERE name='Bebidas' LIMIT 1), NOW()),
('BEB-003', 'Xarope de Refrigerante Limão', 1, 15, (SELECT category_id FROM category WHERE name='Bebidas' LIMIT 1), NOW()),
('BEB-004', 'Leite em Pó para Café', 1, 25, (SELECT category_id FROM category WHERE name='Bebidas' LIMIT 1), NOW()),
('BEB-005', 'Café Moído', 1, 20, (SELECT category_id FROM category WHERE name='Bebidas' LIMIT 1), NOW()),
('BEB-006', 'Água Mineral 500ml', 1, 100, (SELECT category_id FROM category WHERE name='Bebidas' LIMIT 1), NOW()),

-- Sobremesas
('SOB-001', 'Sorvete de Baunilha', 1, 40, (SELECT category_id FROM category WHERE name='Sobremesas' LIMIT 1), NOW()),
('SOB-002', 'Calda de Chocolate', 1, 15, (SELECT category_id FROM category WHERE name='Sobremesas' LIMIT 1), NOW()),
('SOB-003', 'Calda de Caramelo', 1, 12, (SELECT category_id FROM category WHERE name='Sobremesas' LIMIT 1), NOW()),
('SOB-004', 'Granulado Colorido', 1, 10, (SELECT category_id FROM category WHERE name='Sobremesas' LIMIT 1), NOW()),

-- Embalagens
('EMB-001', 'Copos 300ml para Refrigerante', 1, 1000, (SELECT category_id FROM category WHERE name='Embalagens' LIMIT 1), NOW()),
('EMB-002', 'Copos 500ml para Refrigerante', 1, 800, (SELECT category_id FROM category WHERE name='Embalagens' LIMIT 1), NOW()),
('EMB-003', 'Embalagem para Hambúrguer', 1, 1500, (SELECT category_id FROM category WHERE name='Embalagens' LIMIT 1), NOW()),
('EMB-004', 'Embalagem para Batata', 1, 600, (SELECT category_id FROM category WHERE name='Embalagens' LIMIT 1), NOW()),
('EMB-005', 'Guardanapos', 1, 2000, (SELECT category_id FROM category WHERE name='Embalagens' LIMIT 1), NOW()),
('EMB-006', 'Canudos', 1, 1500, (SELECT category_id FROM category WHERE name='Embalagens' LIMIT 1), NOW())
ON DUPLICATE KEY UPDATE name=VALUES(name);

-- Nota: Inserts de Items e Movements já foram incluídos no stockRonaldSQL.sql
-- Execute o stockRonaldSQL.sql para ter dados completos de exemplo

