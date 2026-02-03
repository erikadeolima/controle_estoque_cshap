# 🗄️ Configuração do Banco de Dados MySQL

## 📋 Requisitos

- MySQL Server 8.0 ou superior
- Usuário com permissões de criação de banco

---

## 🚀 Setup Inicial

### 1. Criar o Banco de Dados

Execute o script `Scripts/01_create_database.sql`:

```bash
mysql -u root -p < Scripts/01_create_database.sql
```

Ou manualmente:

```sql
CREATE DATABASE IF NOT EXISTS controle_estoque
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;
```

### 2. Configurar Connection String

Edite o arquivo `appsettings.json` na raiz do projeto:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=controle_estoque;User=SEU_USUARIO;Password=SUA_SENHA;"
}
```

**Parâmetros:**

- **Server**: Endereço do servidor MySQL (padrão: `localhost`)
- **Database**: Nome do banco (`controle_estoque`)
- **User**: Usuário MySQL (ex: `root`)
- **Password**: Senha do usuário

### 3. Aplicar Migrations

Após configurar a connection string, execute:

```bash
dotnet ef migrations add Initial
dotnet ef database update
```

---

## 🔧 Comandos Úteis

### Verificar status do MySQL

```bash
# Linux
sudo systemctl status mysql

# Windows
services.msc → procurar "MySQL"
```

### Conectar ao MySQL via linha de comando

```bash
mysql -u root -p
```

### Listar bancos de dados

```sql
SHOW DATABASES;
```

### Usar o banco do projeto

```sql
USE controle_estoque;
SHOW TABLES;
```

### Ver estrutura de uma tabela

```sql
DESCRIBE Products;
```

---

## 📂 Estrutura de Pastas

```
Database/
├── README.md                    # Este arquivo
├── Scripts/
│   ├── 01_create_database.sql   # Criação do banco
│   ├── 02_create_user.sql       # (Opcional) Criar usuário específico
│   └── seed_data.sql            # (Opcional) Dados iniciais
└── connection_string_examples.md # Exemplos de connection strings
```

---

## 🔐 Segurança

⚠️ **IMPORTANTE:**

- **NUNCA** commite senhas reais no Git
- Use `appsettings.Development.json` para desenvolvimento local (já no `.gitignore`)
- Em produção, use variáveis de ambiente ou Azure Key Vault

### Exemplo com variáveis de ambiente:

```bash
export ConnectionStrings__DefaultConnection="Server=prod-server;Database=controle_estoque;User=app_user;Password=SENHA_FORTE"
```

---

## 📊 Schema do Banco (Após Migrations)

O Entity Framework criará automaticamente estas tabelas:

- **Categories** - Categorias de produtos
- **Products** - Produtos cadastrados
- **Items** - Lotes/estoque de produtos
- **Movements** - Histórico de movimentações
- **Users** - Usuários do sistema

Para ver o diagrama completo, consulte `PLANO_DESENVOLVIMENTO.md`.

---

## 🐛 Troubleshooting

### Erro: "Access denied for user"

✅ Verifique usuário e senha na connection string

### Erro: "Unknown database 'controle_estoque'"

✅ Execute o script `01_create_database.sql`

### Erro: "Unable to connect to any of the specified MySQL hosts"

✅ Certifique-se que o MySQL está rodando:

```bash
sudo systemctl start mysql
```

### Migrations não aplicam

✅ Verifique se instalou as ferramentas EF:

```bash
dotnet tool install --global dotnet-ef
```
