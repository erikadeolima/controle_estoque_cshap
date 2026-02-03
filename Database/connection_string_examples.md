# 🔌 Exemplos de Connection Strings MySQL

## 📝 Formato Básico

```
Server=SERVIDOR;Database=BANCO;User=USUARIO;Password=SENHA;
```

---

## 🖥️ Desenvolvimento Local

### MySQL Local (Usuário Root)

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=controle_estoque;User=root;Password=root;"
}
```

### MySQL Local (Porta Customizada)

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3307;Database=controle_estoque;User=root;Password=root;"
}
```

### MySQL com IP Específico

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=192.168.1.100;Database=controle_estoque;User=app_user;Password=senha123;"
}
```

---

## 🐳 Docker / Containers

### MySQL em Docker (Localhost)

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=controle_estoque;User=root;Password=docker123;"
}
```

### MySQL em Docker Compose

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=mysql_container;Database=controle_estoque;User=app_user;Password=docker_pass;"
}
```

---

## ☁️ Produção / Cloud

### Azure Database for MySQL

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=myserver.mysql.database.azure.com;Database=controle_estoque;User=admin@myserver;Password=SENHA_FORTE;SslMode=Required;"
}
```

### AWS RDS MySQL

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=myinstance.abc123.us-east-1.rds.amazonaws.com;Database=controle_estoque;User=admin;Password=SENHA_FORTE;SslMode=Required;"
}
```

### Google Cloud SQL

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=/cloudsql/project-id:region:instance-name;Database=controle_estoque;User=root;Password=SENHA_FORTE;"
}
```

---

## 🔧 Parâmetros Adicionais Úteis

### Com SSL/TLS

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=controle_estoque;User=root;Password=root;SslMode=Required;"
}
```

### Com Timeout Customizado

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=controle_estoque;User=root;Password=root;ConnectionTimeout=30;"
}
```

### Com Pool de Conexões

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=controle_estoque;User=root;Password=root;Pooling=true;MinimumPoolSize=0;MaximumPoolSize=100;"
}
```

### Charset Específico

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=controle_estoque;User=root;Password=root;CharSet=utf8mb4;"
}
```

---

## 🔐 Usando Variáveis de Ambiente

### Linux/Mac

```bash
export ConnectionStrings__DefaultConnection="Server=localhost;Database=controle_estoque;User=root;Password=root;"
```

### Windows PowerShell

```powershell
$env:ConnectionStrings__DefaultConnection="Server=localhost;Database=controle_estoque;User=root;Password=root;"
```

### Windows CMD

```cmd
set ConnectionStrings__DefaultConnection=Server=localhost;Database=controle_estoque;User=root;Password=root;
```

### Docker Compose (docker-compose.yml)

```yaml
services:
  app:
    environment:
      - ConnectionStrings__DefaultConnection=Server=mysql;Database=controle_estoque;User=root;Password=root;
```

---

## 📋 Arquivo appsettings.Development.json

Para desenvolvimento local, crie/edite `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=controle_estoque;User=root;Password=root;"
  }
}
```

**Benefícios:**

- ✅ Não sobrescreve o `appsettings.json` principal
- ✅ Já está no `.gitignore` por padrão
- ✅ Usado automaticamente em modo Development

---

## ⚠️ Boas Práticas

1. **NUNCA** commite senhas no Git
2. Use `appsettings.Development.json` para desenvolvimento local
3. Use variáveis de ambiente ou secrets em produção
4. Sempre use SSL/TLS em produção (`SslMode=Required`)
5. Configure timeouts apropriados para seu cenário
6. Limite o tamanho do pool de conexões em produção

---

## 🧪 Testar Connection String

```bash
# No diretório do projeto
dotnet ef dbcontext info

# Se conectar com sucesso, mostrará informações do DbContext
```
