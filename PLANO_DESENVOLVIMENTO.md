# 📋 Plano de Desenvolvimento - Sistema de Controle de Estoque

## 🎯 Objetivo

Construir uma API de controle de estoque seguindo Clean Architecture, aplicando SOLID, Design Patterns e Clean Code.

---

## 📚 Conceitos Importantes (Entenda Antes de Começar)

### Clean Architecture

- **Domain**: Regras de negócio puras, sem dependências externas
- **Application**: Casos de uso da aplicação, coordena as operações
- **Infrastructure**: Implementações técnicas (banco de dados, serviços externos)
- **API**: Camada de apresentação (recebe requisições HTTP)

### SOLID (5 Princípios)

1. **Single Responsibility**: Cada classe tem apenas uma responsabilidade
2. **Open/Closed**: Aberto para extensão, fechado para modificação
3. **Liskov Substitution**: Subclasses podem substituir suas classes base
4. **Interface Segregation**: Interfaces pequenas e específicas
5. **Dependency Inversion**: Dependa de abstrações, não de implementações concretas

### Design Patterns que você vai usar

- **Repository**: Abstrai acesso aos dados
- **Strategy**: Permite trocar algoritmos em tempo de execução
- **Singleton**: Garante uma única instância de uma classe
- **Factory Method**: Centraliza criação de objetos
- **Dependency Injection**: Injeta dependências via construtor

---

## 🗂️ FASE 1: Estrutura de Pastas

### Passos:

1. Dentro do projeto, crie a pasta `src`
2. Dentro de `src`, crie 4 pastas principais:
   - `Domain`
   - `Application`
   - `Infrastructure`
   - `API`

3. Dentro de `Domain`, crie:
   - `Entities`
   - `Interfaces`

4. Dentro de `Application`, crie:
   - `DTOs`
   - `Interfaces`
   - `UseCases`
   - `Strategies`

5. Dentro de `Infrastructure`, crie:
   - `Data`
   - `Repositories`
   - `Services`

6. Dentro de `API`, crie:
   - `Controllers`

### ✅ Checklist:

- [ ] Todas as 4 pastas principais criadas
- [ ] Subpastas do Domain criadas
- [ ] Subpastas do Application criadas
- [ ] Subpastas do Infrastructure criadas
- [ ] Subpastas do API criadas

---

## 🏗️ FASE 2: Camada Domain (Entidades)

### O que fazer:

#### 2.1 - Criar BaseEntity

**Onde**: `src/Domain/Entities/BaseEntity.cs`

**O que incluir**:

- Marque a classe como `abstract` (não pode ser instanciada diretamente)
- Adicione propriedades comuns a todas as entidades:
  - Id (tipo Guid)
  - DataCriacao (tipo DateTime)
  - DataAtualizacao (tipo DateTime nullable)
  - Ativo (tipo bool)
- Todas as propriedades devem ter `protected set` (só podem ser alteradas pela própria classe ou herdeiras)
- Crie um construtor sem parâmetros que:
  - Gera um novo Guid para o Id
  - Define DataCriacao como a data/hora atual UTC
  - Define Ativo como true
- Crie métodos públicos:
  - `Atualizar()`: atualiza DataAtualizacao
  - `Desativar()`: marca Ativo como false e atualiza DataAtualizacao
  - `Ativar()`: marca Ativo como true e atualiza DataAtualizacao

**Conceito aplicado**: Herança e DRY (Don't Repeat Yourself)

---

#### 2.2 - Criar Entidade Produto

**Onde**: `src/Domain/Entities/Produto.cs`

**O que incluir**:

- Herda de BaseEntity (use `: BaseEntity`)
- Adicione propriedades específicas (todas com `private set`):
  - Nome (string)
  - Descricao (string)
  - Preco (decimal)
  - QuantidadeEstoque (int)
  - Categoria (string)
  - CodigoBarras (string)
- Construtor privado (para forçar uso do Factory Method)
- Método estático público `Criar()` que:
  - Recebe todos os parâmetros necessários
  - Cria a instância do produto
  - Chama o método de validação
  - Retorna o produto criado
- Método público `AtualizarDados()` que:
  - Recebe os dados que podem ser alterados
  - Atualiza as propriedades
  - Chama o método Atualizar() herdado
  - Valida os dados
- Método público `AdicionarEstoque(int quantidade)` que:
  - Valida se quantidade é positiva
  - Adiciona à QuantidadeEstoque
  - Atualiza a DataAtualizacao
- Método público `RemoverEstoque(int quantidade)` que:
  - Valida se quantidade é positiva
  - Valida se há estoque suficiente
  - Remove da QuantidadeEstoque
  - Atualiza a DataAtualizacao
- Método privado `Validar()` que:
  - Lança exceção se Nome estiver vazio
  - Lança exceção se Preco for negativo
  - Lança exceção se QuantidadeEstoque for negativa

**Conceitos aplicados**: Herança, Encapsulamento, Factory Method, Validação no Domínio

---

#### 2.3 - Criar Entidade Categoria

**Onde**: `src/Domain/Entities/Categoria.cs`

**O que incluir**:

- Herda de BaseEntity
- Propriedades (com `private set`):
  - Nome (string)
  - Descricao (string)
- Construtor privado
- Método estático `Criar()` (Factory Method)
- Método `AtualizarDados()`
- Método privado `Validar()` que valida se Nome não está vazio

**Conceitos aplicados**: Mesmos da entidade Produto

---

### ✅ Checklist Fase 2:

- [ ] BaseEntity criada com propriedades comuns
- [ ] BaseEntity tem métodos Atualizar, Ativar e Desativar
- [ ] Produto herda de BaseEntity
- [ ] Produto tem construtor privado
- [ ] Produto tem método estático Criar (Factory Method)
- [ ] Produto valida suas regras de negócio
- [ ] Produto tem métodos para gerenciar estoque
- [ ] Categoria criada seguindo mesmo padrão

---

## 🔌 FASE 3: Camada Domain (Interfaces)

### O que fazer:

#### 3.1 - Criar Interface Genérica de Repositório

**Onde**: `src/Domain/Interfaces/IRepository.cs`

**O que incluir**:

- Interface genérica `IRepository<T>` com restrição `where T : BaseEntity`
- Defina métodos assíncronos (retornam Task):
  - `ObterPorIdAsync(Guid id)` - retorna Task com T nullable
  - `ObterTodosAsync()` - retorna Task com coleção de T
  - `ObterAtivosAsync()` - retorna Task com coleção de T
  - `AdicionarAsync(T entity)` - retorna Task
  - `AtualizarAsync(T entity)` - retorna Task
  - `RemoverAsync(Guid id)` - retorna Task
  - `ExisteAsync(Guid id)` - retorna Task com bool

**Conceito aplicado**: SOLID (Interface Segregation), Generics

---

#### 3.2 - Criar Interface Específica de Produto

**Onde**: `src/Domain/Interfaces/IProdutoRepository.cs`

**O que incluir**:

- Interface `IProdutoRepository` que herda de `IRepository<Produto>`
- Adicione métodos específicos de produtos:
  - `BuscarPorCategoriaAsync(string categoria)` - retorna coleção de Produto
  - `BuscarPorCodigoBarrasAsync(string codigoBarras)` - retorna Produto nullable
  - `BuscarPorNomeAsync(string nome)` - retorna coleção de Produto
  - `BuscarEstoqueBaixoAsync(int quantidadeMinima)` - retorna coleção de Produto

**Conceito aplicado**: SOLID (Interface Segregation + Open/Closed), Herança de Interface

---

#### 3.3 - Criar Interface Específica de Categoria

**Onde**: `src/Domain/Interfaces/ICategoriaRepository.cs`

**O que incluir**:

- Interface `ICategoriaRepository` que herda de `IRepository<Categoria>`
- Adicione método específico:
  - `BuscarPorNomeAsync(string nome)` - retorna Categoria nullable

### ✅ Checklist Fase 3:

- [ ] IRepository genérico criado com métodos CRUD
- [ ] IProdutoRepository herda de IRepository e adiciona métodos específicos
- [ ] ICategoriaRepository herda de IRepository e adiciona métodos específicos
- [ ] Todos os métodos retornam Task (assíncronos)

---

## 📦 FASE 4: Camada Application (DTOs)

### O que fazer:

#### 4.1 - Criar DTOs (Data Transfer Objects)

**Importante**: DTOs são objetos simples para transferir dados. NÃO têm lógica de negócio.

**Onde e O que**:

1. **ProdutoDto** (`src/Application/DTOs/ProdutoDto.cs`)
   - Propriedades públicas com get e set
   - Mesmas propriedades da entidade Produto
   - Adicione as propriedades herdadas (Id, DataCriacao, Ativo)
   - Inicialize strings vazias para evitar valores nulos

2. **CriarProdutoDto** (`src/Application/DTOs/CriarProdutoDto.cs`)
   - Apenas os campos necessários para CRIAR um produto
   - NÃO inclua Id, DataCriacao (são gerados automaticamente)
   - Inclua: Nome, Descricao, Preco, QuantidadeEstoque, Categoria, CodigoBarras

3. **AtualizarProdutoDto** (`src/Application/DTOs/AtualizarProdutoDto.cs`)
   - Apenas os campos que podem ser ATUALIZADOS
   - NÃO inclua QuantidadeEstoque (tem métodos específicos)
   - Inclua: Nome, Descricao, Preco, Categoria

4. **CategoriaDto** (`src/Application/DTOs/CategoriaDto.cs`)
   - Propriedades: Id, Nome, Descricao, Ativo

**Conceito aplicado**: Separação de Responsabilidades, Clean Code

### ✅ Checklist Fase 4:

- [ ] ProdutoDto criado com todas as propriedades
- [ ] CriarProdutoDto criado apenas com dados de criação
- [ ] AtualizarProdutoDto criado apenas com dados editáveis
- [ ] CategoriaDto criado
- [ ] Todos os DTOs têm apenas propriedades, sem métodos

---

## 🎯 FASE 5: Camada Application (Strategies e Interfaces)

### O que fazer:

#### 5.1 - Criar Interface de Strategy

**Onde**: `src/Application/Interfaces/IValidacaoStrategy.cs`

**O que incluir**:

- Interface genérica `IValidacaoStrategy<T>`
- Um único método: `ValidarAsync(T dto)`
- Retorno: Task contendo uma tupla (bool IsValid, string[] Errors)

**Conceito aplicado**: Strategy Pattern, SOLID (Open/Closed)

---

#### 5.2 - Criar Strategy de Validação de Produto

**Onde**: `src/Application/Strategies/ValidacaoProdutoStrategy.cs`

**O que incluir**:

- Classe que implementa `IValidacaoStrategy<CriarProdutoDto>`
- Implemente o método `ValidarAsync`:
  - Crie uma lista de erros
  - Valide cada campo do DTO (Nome vazio, Preço negativo, etc)
  - Para cada erro, adicione mensagem descritiva na lista
  - Retorne tupla (se não há erros, array de erros)

**Conceito aplicado**: Strategy Pattern, Validação na Camada de Aplicação

---

#### 5.3 - Criar Interface de Serviço

**Onde**: `src/Application/Interfaces/IProdutoService.cs`

**O que incluir**:

- Interface `IProdutoService`
- Métodos que representam os casos de uso:
  - `ObterPorIdAsync(Guid id)` - retorna ProdutoDto nullable
  - `ObterTodosAsync()` - retorna coleção de ProdutoDto
  - `CriarAsync(CriarProdutoDto dto)` - retorna ProdutoDto
  - `AtualizarAsync(Guid id, AtualizarProdutoDto dto)` - retorna ProdutoDto
  - `RemoverAsync(Guid id)` - retorna Task
  - `AdicionarEstoqueAsync(Guid id, int quantidade)` - retorna Task
  - `RemoverEstoqueAsync(Guid id, int quantidade)` - retorna Task

**Conceito aplicado**: SOLID (Dependency Inversion)

### ✅ Checklist Fase 5:

- [ ] IValidacaoStrategy criado
- [ ] ValidacaoProdutoStrategy implementado
- [ ] IProdutoService criado com todos os casos de uso
- [ ] Não esqueça dos `using` necessários no topo dos arquivos

---

## 💼 FASE 6: Camada Application (Use Cases)

### O que fazer:

#### 6.1 - Criar ProdutoService

**Onde**: `src/Application/UseCases/ProdutoService.cs`

**O que incluir**:

- Classe que implementa `IProdutoService`
- Campos privados readonly:
  - `_repository` do tipo IProdutoRepository
  - `_validacaoStrategy` do tipo IValidacaoStrategy<CriarProdutoDto>
- Construtor que recebe esses dois parâmetros e os atribui aos campos (Dependency Injection)
- Implemente cada método da interface:

**Método ObterPorIdAsync**:

- Chame o repository para buscar o produto
- Se encontrou, converta para DTO
- Retorne o DTO ou null

**Método ObterTodosAsync**:

- Busque todos os produtos do repository
- Para cada produto, converta para DTO
- Retorne a coleção de DTOs

**Método CriarAsync**:

- Chame a strategy para validar o DTO
- Se inválido, lance exceção com as mensagens de erro
- Chame o método Criar da entidade Produto passando os dados do DTO
- Chame o repository para adicionar
- Converta para DTO e retorne

**Método AtualizarAsync**:

- Busque o produto pelo Id no repository
- Se não encontrou, lance KeyNotFoundException
- Chame o método AtualizarDados da entidade
- Chame o repository para atualizar
- Converta para DTO e retorne

**Método RemoverAsync**:

- Busque o produto pelo Id
- Se não encontrou, lance exceção
- Chame o método Desativar da entidade
- Atualize no repository

**Método AdicionarEstoqueAsync**:

- Busque o produto
- Se não encontrou, lance exceção
- Chame o método AdicionarEstoque da entidade
- Atualize no repository

**Método RemoverEstoqueAsync**:

- Busque o produto
- Se não encontrou, lance exceção
- Chame o método RemoverEstoque da entidade (pode lançar exceção se estoque insuficiente)
- Atualize no repository

**Método privado MapearParaDto**:

- Crie um método privado estático
- Recebe Produto, retorna ProdutoDto
- Crie novo ProdutoDto e preencha todas as propriedades
- Use esse método em todos os lugares que precisar converter

**Conceitos aplicados**: SOLID (Single Responsibility, Dependency Inversion), Use Case Pattern, Dependency Injection

### ✅ Checklist Fase 6:

- [ ] ProdutoService criado e implementa IProdutoService
- [ ] Dependências injetadas via construtor
- [ ] Todos os métodos implementados
- [ ] Validação aplicada antes de criar
- [ ] Conversões entre Entidade e DTO funcionando
- [ ] Exceções lançadas em casos de erro

---

## 🏢 FASE 7: Camada Infrastructure (Dados)

### O que fazer:

#### 7.1 - Criar DbContext

**Onde**: `src/Infrastructure/Data/AppDbContext.cs`

**O que incluir**:

- Classe que herda de `DbContext` (do Entity Framework Core)
- Construtor que:
  - Recebe `DbContextOptions<AppDbContext>`
  - Passa para o construtor base
- Propriedades DbSet:
  - `DbSet<Produto> Produtos`
  - `DbSet<Categoria> Categorias`
- Override do método `OnModelCreating(ModelBuilder modelBuilder)`:
  - Chame o base antes
  - Configure a entidade Produto:
    - Defina chave primária (Id)
    - Configure Nome como obrigatório e com tamanho máximo (200 caracteres)
    - Configure Descricao com tamanho máximo (500 caracteres)
    - Configure Preco como decimal(18,2)
    - Configure CodigoBarras como obrigatório, tamanho máximo 50
    - Crie índice único para CodigoBarras
  - Configure a entidade Categoria:
    - Defina chave primária
    - Configure Nome como obrigatório, máximo 100 caracteres
    - Configure Descricao máximo 300 caracteres
    - Crie índice único para Nome

**Conceito aplicado**: Entity Framework Core, Configuração de Banco de Dados

### ✅ Checklist Fase 7:

- [ ] AppDbContext criado herdando de DbContext
- [ ] DbSets configurados
- [ ] Configurações de entidades implementadas
- [ ] Índices únicos criados

---

## 📂 FASE 8: Camada Infrastructure (Repositories)

### O que fazer:

#### 8.1 - Criar Repository Genérico

**Onde**: `src/Infrastructure/Repositories/Repository.cs`

**O que incluir**:

- Classe genérica `Repository<T>` com restrição `where T : BaseEntity`
- Implementa `IRepository<T>`
- Campos protected:
  - `_context` do tipo AppDbContext
  - `_dbSet` do tipo DbSet<T>
- Construtor que recebe AppDbContext:
  - Atribui ao campo \_context
  - Inicializa \_dbSet usando `context.Set<T>()`
- Implemente cada método da interface usando Entity Framework:

**ObterPorIdAsync**: Use `_dbSet.FindAsync(id)`
**ObterTodosAsync**: Use `_dbSet.ToListAsync()`
**ObterAtivosAsync**: Use `_dbSet.Where(e => e.Ativo).ToListAsync()`
**AdicionarAsync**: Use `_dbSet.AddAsync`, depois `_context.SaveChangesAsync()`
**AtualizarAsync**: Use `_dbSet.Update`, depois `SaveChangesAsync`
**RemoverAsync**: Busque a entidade, chame Desativar(), atualize
**ExisteAsync**: Use `_dbSet.AnyAsync(e => e.Id == id)`

Marque todos os métodos como `virtual` (para permitir override)

**Conceito aplicado**: Repository Pattern, Generics, SOLID (DRY)

---

#### 8.2 - Criar ProdutoRepository

**Onde**: `src/Infrastructure/Repositories/ProdutoRepository.cs`

**O que incluir**:

- Classe que herda `Repository<Produto>`
- Implementa `IProdutoRepository`
- Construtor que recebe AppDbContext e passa para o base
- Implemente os métodos específicos:

**BuscarPorCategoriaAsync**: Filtre \_dbSet por categoria e ativo
**BuscarPorCodigoBarrasAsync**: Use FirstOrDefaultAsync para buscar por código
**BuscarPorNomeAsync**: Use Where com Contains para buscar por nome
**BuscarEstoqueBaixoAsync**: Filtre onde QuantidadeEstoque <= quantidadeMinima e ativo

**Conceito aplicado**: Herança, Polimorfismo, SOLID (Open/Closed)

---

#### 8.3 - Criar CategoriaRepository

**Onde**: `src/Infrastructure/Repositories/CategoriaRepository.cs`

**O que incluir**:

- Herda `Repository<Categoria>`
- Implementa `ICategoriaRepository`
- Construtor passa AppDbContext para base
- Implemente `BuscarPorNomeAsync`

### ✅ Checklist Fase 8:

- [ ] Repository genérico criado com métodos CRUD
- [ ] ProdutoRepository herda e adiciona métodos específicos
- [ ] CategoriaRepository criado
- [ ] Todos os métodos são assíncronos

---

## 🔧 FASE 9: Camada Infrastructure (Services)

### O que fazer:

#### 9.1 - Criar LoggerService (Singleton)

**Onde**: `src/Infrastructure/Services/LoggerService.cs`

**O que incluir**:

- Classe marcada como `sealed` (não pode ser herdada)
- Campo privado estático nullable: `_instance` do tipo LoggerService
- Campo privado estático readonly: `_lock` (objeto para sincronização)
- Construtor PRIVADO vazio (impede criação externa)
- Propriedade estática pública `Instance`:
  - Tipo: LoggerService
  - Getter que implementa Double-Check Locking:
    - Se \_instance é null
    - Faça lock no \_lock
    - Verifique novamente se \_instance é null
    - Se ainda for, crie nova instância
    - Retorne \_instance
- Método público `Log(string message)`:
  - Escreva no console com timestamp
- Método público `LogError(string message, Exception ex)`:
  - Escreva erro no console com timestamp
  - Se exceção não for nula, imprima mensagem e stacktrace

**Conceito aplicado**: Singleton Pattern, Thread Safety

### ✅ Checklist Fase 9:

- [ ] LoggerService criado como Singleton
- [ ] Construtor privado
- [ ] Thread-safe (Double-Check Locking)
- [ ] Métodos Log e LogError implementados

---

## 🌐 FASE 10: Camada API (Controllers)

### O que fazer:

#### 10.1 - Criar ProdutosController

**Onde**: `src/API/Controllers/ProdutosController.cs`

**O que incluir**:

- Atributos da classe:
  - `[ApiController]`
  - `[Route("api/[controller]")]`
- Herda de `ControllerBase`
- Campo privado readonly: `_produtoService` do tipo IProdutoService
- Construtor que recebe IProdutoService e atribui ao campo

**Crie os seguintes endpoints**:

1. **GET /api/produtos** - ObterTodos
   - Atributo `[HttpGet]`
   - Atributos de documentação para status 200
   - Chame \_produtoService.ObterTodosAsync()
   - Retorne Ok(produtos)

2. **GET /api/produtos/{id}** - ObterPorId
   - Atributo `[HttpGet("{id}")]`
   - Recebe Guid id como parâmetro
   - Chame o serviço
   - Se null, retorne NotFound com mensagem
   - Senão, retorne Ok(produto)

3. **POST /api/produtos** - Criar
   - Atributo `[HttpPost]`
   - Recebe `[FromBody] CriarProdutoDto dto`
   - Use try-catch para capturar ArgumentException
   - Se erro, retorne BadRequest com mensagem
   - Se sucesso, retorne CreatedAtAction apontando para ObterPorId

4. **PUT /api/produtos/{id}** - Atualizar
   - Atributo `[HttpPut("{id}")]`
   - Recebe id e `[FromBody] AtualizarProdutoDto dto`
   - Use try-catch para:
     - KeyNotFoundException → NotFound
     - ArgumentException → BadRequest
   - Se sucesso, retorne Ok(produto)

5. **DELETE /api/produtos/{id}** - Remover
   - Atributo `[HttpDelete("{id}")]`
   - Recebe id
   - Try-catch para KeyNotFoundException
   - Se sucesso, retorne NoContent()

6. **POST /api/produtos/{id}/estoque/adicionar** - AdicionarEstoque
   - Atributo `[HttpPost("{id}/estoque/adicionar")]`
   - Recebe id e `[FromBody] int quantidade`
   - Try-catch para exceções
   - Retorne Ok com mensagem de sucesso

7. **POST /api/produtos/{id}/estoque/remover** - RemoverEstoque
   - Atributo `[HttpPost("{id}/estoque/remover")]`
   - Recebe id e quantidade
   - Try-catch para exceções (incluindo InvalidOperationException)
   - Retorne Ok com mensagem

Adicione comentários XML (///) acima de cada método descrevendo o que faz.

**Conceito aplicado**: REST API, Dependency Injection, Tratamento de Exceções

### ✅ Checklist Fase 10:

- [ ] Controller criado com atributos corretos
- [ ] Todos os 7 endpoints implementados
- [ ] Tratamento de exceções em cada endpoint
- [ ] Status codes corretos (200, 201, 204, 400, 404)
- [ ] Documentação XML nos métodos

---

## ⚙️ FASE 11: Configuração da API

### O que fazer:

#### 11.1 - Configurar Program.cs

**Onde**: `Program.cs` (raiz do projeto)

**Substitua todo o conteúdo padrão**:

**Adicione os usings necessários no topo**:

- Application.DTOs
- Application.Interfaces
- Application.Strategies
- Application.UseCases
- Domain.Interfaces
- Infrastructure.Data
- Infrastructure.Repositories
- Microsoft.EntityFrameworkCore

**Configure os serviços (antes de `var app = builder.Build()`)**:

1. **DbContext**:
   - Use AddDbContext para AppDbContext
   - Configure para usar MySQL
   - Recupere a connection string de `appsettings.json`
   - Use `UseMySql()` com `MySqlServerVersion`

2. **Services**:
   - Registre IProdutoService → ProdutoService (Scoped)

3. **Strategies**:
   - Registre IValidacaoStrategy<CriarProdutoDto> → ValidacaoProdutoStrategy (Scoped)

4. **Controllers**:
   - Adicione AddControllers()

5. **Swagger**:
   - Mantenha AddEndpointsApiExplorer
   - Configure AddSwaggerGen com informações do projeto

**Configure o pipeline (depois de `var app = builder.Build()`)**:

1. Mantenha configuração de Swagger para Development
2. Adicione UseHttpsRedirection
3. Adicione UseAuthorization
4. **IMPORTANTE**: Adicione MapControllers() (para os endpoints funcionarem)
5. Mantenha app.Run()

**Remova**: Todo código de WeatherForecast

**Conceito aplicado**: Dependency Injection, Configuração de API

### ✅ Checklist Fase 11:

- [ ] Usings adicionados
- [ ] DbContext configurado
- [ ] Todos os repositories registrados
- [ ] Todos os services registrados
- [ ] Strategies registrados
- [ ] Controllers adicionados
- [ ] MapControllers() incluído
- [ ] Código de exemplo removido

---

## 📦 FASE 12: Pacotes NuGet

### O que fazer:

#### 12.1 - Adicionar Pacotes Necessários

**Onde**: Terminal / Linha de comando

**Execute os seguintes comandos**:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

**Ou edite manualmente o arquivo .csproj**:

- Abra `controle_estoque_cshap.csproj`
- Dentro de `<ItemGroup>` onde estão os PackageReference
- Adicione as 3 linhas acima:
  - Microsoft.EntityFrameworkCore (versão 8.0.1)
  - Pomelo.EntityFrameworkCore.MySql (versão 8.0.0)
  - Microsoft.EntityFrameworkCore.Tools (versão 8.0.1)

Depois execute:

```bash
dotnet restore
```

**Conceito aplicado**: Gerenciamento de Dependências

### ✅ Checklist Fase 12:

- [ ] Entity Framework Core instalado
- [ ] Pomelo MySQL provider instalado
- [ ] Tools instalado
- [ ] dotnet restore executado com sucesso

---

## 🧪 FASE 13: Testar a Aplicação

### O que fazer:

#### 13.1 - Compilar

**Terminal**:

```bash
dotnet build
```

**Verifique**:

- Não deve ter erros de compilação
- Pode ter warnings sobre nullable (tudo bem)

---

#### 13.2 - Executar

**Terminal**:

```bash
dotnet run
```

**Observe**:

- A URL onde a aplicação está rodando (geralmente https://localhost:5001)
- Mensagens de inicialização

---

#### 13.3 - Testar no Swagger

**Navegador**:

1. Acesse: `https://localhost:5001/swagger` (ou a porta que aparecer)
2. Você verá a interface do Swagger com todos os endpoints

**Teste a sequência**:

1. **POST /api/produtos** (Criar):
   - Clique em "Try it out"
   - Preencha o JSON com dados de exemplo
   - Execute
   - Deve retornar 201 Created
   - Copie o Id retornado

2. **GET /api/produtos** (Listar):
   - Execute
   - Deve retornar o produto criado

3. **GET /api/produtos/{id}** (Buscar por ID):
   - Cole o Id copiado
   - Execute
   - Deve retornar os dados do produto

4. **PUT /api/produtos/{id}** (Atualizar):
   - Cole o Id
   - Altere alguns dados
   - Execute
   - Deve retornar 200 OK com dados atualizados

5. **POST /api/produtos/{id}/estoque/adicionar**:
   - Cole o Id
   - Informe quantidade (ex: 5)
   - Execute
   - Deve retornar sucesso

6. **DELETE /api/produtos/{id}** (Remover):
   - Cole o Id
   - Execute
   - Deve retornar 204 No Content

### ✅ Checklist Fase 13:

- [ ] Compilação sem erros
- [ ] Aplicação executando
- [ ] Swagger acessível
- [ ] Endpoint POST funciona
- [ ] Endpoint GET funciona
- [ ] Endpoint PUT funciona
- [ ] Endpoint DELETE funciona
- [ ] Endpoints de estoque funcionam

---

## 📝 FASE 14: Validação dos Conceitos

### Checklist Final - Você Aplicou:

#### SOLID

- [ ] **S**: Cada classe tem uma única responsabilidade?
- [ ] **O**: Você pode adicionar novos tipos sem modificar código existente?
- [ ] **L**: Subclasses funcionam no lugar das classes base?
- [ ] **I**: Interfaces são pequenas e focadas?
- [ ] **D**: Suas classes dependem de interfaces, não de implementações?

#### Design Patterns

- [ ] **Strategy**: Validação pode ser trocada facilmente?
- [ ] **Repository**: Acesso a dados está abstraído?
- [ ] **Singleton**: LoggerService tem apenas uma instância?
- [ ] **Factory Method**: Entidades são criadas via método Criar()?
- [ ] **Dependency Injection**: Dependências injetadas via construtor?

#### Clean Architecture

- [ ] **Domain**: Sem dependências externas, apenas regras de negócio?
- [ ] **Application**: Coordena operações, usa Domain e define interfaces?
- [ ] **Infrastructure**: Implementa interfaces, acessa banco de dados?
- [ ] **API**: Apenas recebe requisições e chama Application?

#### Herança e Polimorfismo

- [ ] BaseEntity é herdada por Produto e Categoria?
- [ ] Repository<T> é herdado por repositórios específicos?
- [ ] Métodos podem ser sobrescritos (virtual/override)?

#### Clean Code

- [ ] Nomes descritivos e claros?
- [ ] Métodos pequenos e focados?
- [ ] Sem duplicação de código?
- [ ] Validações nos lugares corretos?

---

## 🎯 Próximos Desafios (Após Dominar o Básico)

1. **Adicionar FluentValidation** para validações mais robustas
2. **Implementar AutoMapper** para conversões automáticas
3. **Criar testes unitários** com xUnit
4. **Adicionar autenticação** com JWT
5. **Adicionar paginação** nos endpoints de listagem
6. **Implementar logging com Serilog**
7. **Criar filtros e middleware** customizados
8. **Adicionar cache** com Redis
9. **Implementar CQRS** (Command Query Responsibility Segregation)
10. **Criar migrations** para versionamento do banco de dados

---

## 💡 Dicas Importantes

### Durante o Desenvolvimento:

1. **Faça uma fase por vez** - não pule etapas
2. **Compile frequentemente** - não acumule erros
3. **Leia as mensagens de erro** - elas dizem o que está errado
4. **Use IntelliSense** - Ctrl+Space mostra sugestões
5. **Pesquise quando travar** - mas tente primeiro

### Conceitos para Estudar Paralelo:

- **Async/Await** em C#
- **LINQ** (Language Integrated Query)
- **Entity Framework Core** básico
- **Attributes** em C# ([HttpGet], [Route], etc)
- **Exception Handling** (try-catch-finally)
- **Generics** (classes e métodos genéricos)

### Recursos Úteis:

- Documentação oficial Microsoft: docs.microsoft.com
- Pesquise sempre: "Como fazer X em C#"
- VS Code IntelliSense é seu melhor amigo
- Mensagens de erro do compilador são descritivas

---

## ✅ Quando Você Terminar

Você terá construído uma API completa que demonstra:

- ✅ Arquitetura limpa e bem organizada
- ✅ Código seguindo princípios SOLID
- ✅ Uso de Design Patterns profissionais
- ✅ Boas práticas de Clean Code
- ✅ API REST funcional e testável

**Parabéns! Você terá um projeto portfolio profissional!** 🎉

---

**Importante**: Este plano é seu guia. Siga passo a passo, com calma e atenção. Cada conceito aqui é fundamental para ser um bom desenvolvedor C#. Boa sorte! 🚀
