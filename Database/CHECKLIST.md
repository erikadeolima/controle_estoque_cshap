# 📝 Checklist - Configuração do Banco MySQL

Use este checklist para garantir que tudo está configurado corretamente.

---

## ✅ Pré-requisitos

- [ ] MySQL Server instalado (versão 8.0 ou superior recomendada)
- [ ] MySQL rodando/ativo
- [ ] Acesso com usuário e senha (root ou outro)
- [ ] .NET SDK instalado
- [ ] EF Core Tools instalado (`dotnet tool install --global dotnet-ef`)

---

## 🔧 Passos de Configuração

### 1. Criar o Banco de Dados

- [ ] Conectar ao MySQL: `mysql -u root -p`
- [ ] Executar script: `source Database/Scripts/01_create_database.sql`
- [ ] Ou manualmente:
  ```sql
  CREATE DATABASE controle_estoque CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
  ```
- [ ] Verificar: `SHOW DATABASES;` (deve aparecer `controle_estoque`)

### 2. (Opcional) Criar Usuário Específico

- [ ] Editar `Database/Scripts/02_create_user.sql` com senha segura
- [ ] Executar: `source Database/Scripts/02_create_user.sql`
- [ ] Verificar: `SELECT User, Host FROM mysql.user WHERE User = 'app_user';`

### 3. Configurar Connection String

- [ ] Criar/editar `appsettings.Development.json` na raiz do projeto
- [ ] Adicionar:
  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=controle_estoque;User=root;Password=SUA_SENHA_AQUI;"
    }
  }
  ```
- [ ] **NÃO** commitar este arquivo (já deve estar no .gitignore)

### 4. Aplicar Migrations

- [ ] Criar migration inicial: `dotnet ef migrations add InitialCreate`
- [ ] Verificar pasta `Migrations/` criada
- [ ] Aplicar ao banco: `dotnet ef database update`
- [ ] Verificar no MySQL:
  ```sql
  USE controle_estoque;
  SHOW TABLES;
  ```

### 5. (Opcional) Inserir Dados Iniciais

- [ ] Executar: `source Database/Scripts/seed_data.sql`
- [ ] Verificar dados:
  ```sql
  SELECT * FROM Categories;
  SELECT * FROM Products;
  SELECT * FROM Users;
  ```

### 6. Testar Aplicação

- [ ] Compilar: `dotnet build`
- [ ] Executar: `dotnet run`
- [ ] Acessar Swagger: `https://localhost:5001/swagger`
- [ ] Verificar logs de conexão no console

---

## 🐛 Troubleshooting

### ❌ Erro: "Unknown database 'controle_estoque'"

**Solução:** Execute o script `01_create_database.sql`

### ❌ Erro: "Access denied for user 'root'@'localhost'"

**Solução:**

1. Verifique senha na connection string
2. Teste login manual: `mysql -u root -p`

### ❌ Erro: "Unable to connect to any of the specified MySQL hosts"

**Solução:**

1. Verifique se MySQL está rodando: `sudo systemctl status mysql`
2. Inicie se necessário: `sudo systemctl start mysql`
3. Verifique porta (padrão 3306)

### ❌ Migrations não criam tabelas

**Solução:**

1. Verifique connection string
2. Delete pasta `Migrations/`
3. Recrie: `dotnet ef migrations add InitialCreate`
4. Aplique: `dotnet ef database update`

### ❌ Erro: "dotnet ef: command not found"

**Solução:** Instale as ferramentas EF Core:

```bash
dotnet tool install --global dotnet-ef
```

---

## 📊 Verificação Final

Execute estes comandos para confirmar que tudo está OK:

```bash
# 1. Testar conexão com DbContext
dotnet ef dbcontext info

# 2. Listar migrations aplicadas
dotnet ef migrations list

# 3. Compilar e rodar
dotnet run
```

No MySQL:

```sql
-- Ver estrutura das tabelas
USE controle_estoque;
SHOW TABLES;
DESCRIBE Products;
DESCRIBE Items;

-- Ver dados (se executou seed)
SELECT COUNT(*) FROM Categories;
SELECT COUNT(*) FROM Products;
```

---

## ✅ Status de Conclusão

Marque quando completar cada seção:

- [ ] Banco de dados criado
- [ ] Connection string configurada
- [ ] Migrations aplicadas
- [ ] Tabelas criadas no MySQL
- [ ] (Opcional) Dados iniciais inseridos
- [ ] Aplicação rodando e conectando ao banco

---

## 📞 Próximos Passos

Quando tudo estiver verde ✅:

1. Commitar o código (SEM appsettings.Development.json)
2. Compartilhar instruções com o time
3. Começar a implementar Repositories e Services
4. Criar endpoints da API

**Boa sorte! 🚀**
