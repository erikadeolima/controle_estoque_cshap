# Módulo: Gerenciamento de Estoque – Lanchonete (Itens Alimentícios)

## 1. Objetivo

O módulo de **Controle de Estoque** tem como objetivo gerenciar produtos alimentícios disponíveis na lanchonete, permitindo o acompanhamento de quantidades, status de disponibilidade, histórico e regras específicas para o ciclo de vida dos itens.

## 2. Escopo

O sistema abrangerá **somente produtos alimentícios**, excluindo qualquer outro tipo de insumo ou item não comestível.

## 3. Funcionalidades

### 3.1 Listagens

- **Listar produtos ativos:** Exibir todos os itens disponíveis em estoque.
- **Buscar item específico:** Retornar a quantidade atual de um produto conforme consulta por nome, SKU ou identificador equivalente.
- **Listar produtos inativos:** Exibir itens descontinuados, com filtro específico por status "inativo".

### 3.2 Criação

- **Cadastro de item:** Permitir o registro de novos produtos com os seguintes dados obrigatórios:
  - Nome do produto.
  - SKU (ou identificador equivalente).
  - Quantidade inicial.
  - Data de validade (quando aplicável).
  - Categoria (ex: frios, bebidas, ingredientes, etc.).
  - Status inicial (ativo/disponível).

### 3.3 Atualização

- **Atualizar quantidade:** Permitir a modificação da quantidade em estoque (entrada ou saída).
- **Atualizar status:**
  - **Ativo:** Produto disponível para uso/consumo.
  - **Inativo:** Produto descontinuado, não exibido na listagem principal.
  - **Disponível:** Estoque acima do nível mínimo.
  - **Esgotado:** Estoque zerado.
  - **Alerta:** Estoque abaixo do limite mínimo configurado.

### 3.4 Remoção

- **Exclusão automática:** Produtos armazenados há mais de _X anos_ podem ser removidos definitivamente do sistema, desde que não estejam marcados como inativos.

## 4. Regras de Negócio

1. **Identificação obrigatória:** Todo produto deve possuir um SKU único ou código equivalente.
2. **Inativação de produtos:**
   - Um produto inativado não pode ser removido manualmente.
   - Seus dados permanecem registrados apenas como "inativo" e não sofrem alterações adicionais.
   - Itens inativos não aparecem em listagens de produtos disponíveis.
3. **Histórico de movimentações:**
   - O sistema deve manter o histórico completo de movimentações (entradas, saídas, alterações de status) por um período mínimo de _X anos_.
4. **Notificações de estoque baixo:**
   - Quando a quantidade de um produto atingir o nível configurado como "alerta", o sistema deve gerar um aviso automático.

## 5. Considerações Técnicas (Opcional)

- O sistema deve permitir integração futura com módulos de **vendas** e **compras** para atualização automática de estoque.
- Campos de auditoria devem registrar: usuário responsável, data e hora das alterações.
