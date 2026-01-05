🚀 Gestão de Clientes API
📖 Descrição

API RESTful para gerenciamento de clientes, desenvolvida em ASP.NET Core (.NET) como parte de um desafio técnico.
O projeto segue os princípios da Clean Architecture, DDD e CQRS, garantindo organização, testabilidade e baixo acoplamento entre as camadas.

Principais tecnologias:

ASP.NET Core

Clean Architecture

Domain-Driven Design (DDD)

CQRS (Commands e Queries)

NHibernate (Infraestrutura)

Swagger / OpenAPI

xUnit + Moq (Testes unitários)

Logging (ILogger)

⚙️ Pré-requisitos

Para executar o projeto, é necessário:

.NET SDK (9)

Visual Studio, VS Code ou Rider

🔧 Instalação e Execução
1. Clonar e restaurar pacotes
git clone https://github.com/marianagaldino1/GestaoClientes.git
cd GestaoClientes
dotnet restore

2. Executar a API
dotnet run --project GestaoClientes.API

📑 Swagger

Ao iniciar a aplicação, o Swagger é carregado automaticamente:

https://localhost:{porta}/swagger

Por meio dele é possível testar todos os endpoints disponíveis.

🧪 Testes

O projeto possui testes unitários para a camada de Application.

Para executá-los:

dotnet test


Cobertura inclui:

Criação de cliente

Consulta de cliente por ID

Validações de domínio (CNPJ, nome obrigatório, duplicidade)

📌 Funcionalidades

Criar cliente

Consultar cliente por ID

Validação de CNPJ

Prevenção de CNPJ duplicado

💡 Próximos Passos e Melhorias

Caso houvesse mais tempo, poderiam ser adicionados:

Autenticação e autorização (JWT)

Persistência com banco relacional real

Observabilidade (Serilog + sinks)
