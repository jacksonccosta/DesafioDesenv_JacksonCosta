# Desafio de Desenvolvimento .NET - Solução Refatorada e Modernizada
### Jackson Costa

## 1. Resumo do Projeto

> Este repositório contém a solução para o "Desafio de Desenvolvimento WebApp", um sistema simples de registro de chamados. O projeto original foi completamente refatorado para atender a todos os requisitos solicitados e, mais importante, para demonstrar a aplicação de boas práticas de arquitetura de software, princípios SOLID e padrões de projeto modernos do ecossistema .NET, elevando a solução a um nível de especialista.

## 2. Arquitetura e Padrões Aplicados

A arquitetura original foi substituída por uma abordagem moderna, desacoplada e robusta, fundamentada nos seguintes padrões e princípios:

### a. CQRS (Command Query Responsibility Segregation) com Padrão Mediator

A lógica de negócio foi dividida em **Commands** (operações de escrita que alteram o estado) e **Queries** (operações de leitura que não alteram o estado). Utilizamos a biblioteca `MediatR` para orquestrar esse fluxo, garantindo que os `Controllers` apenas criem e enviem requisições, sem conhecer a implementação da lógica de negócio. Isso promove baixo acoplamento e adere ao Princípio da Responsabilidade Única (SRP).

### b. Injeção de Dependência (DI) e Inversão de Controle (IoC)

Removemos todas as instanciações diretas de dependências. Todas as camadas (`Controller`, `Handlers`, `DALs`) agora recebem suas dependências (como `IMediator`, `ApplicationDbContext`, etc.) através de injeção de dependência no construtor. Isso adere ao **Princípio da Inversão de Dependência (DIP)**, tornando o código modular, desacoplado e altamente testável.

### c. Middleware e Pipeline Behaviors do MediatR

Implementamos padrões avançados para centralizar responsabilidades transversais (cross-cutting concerns):
* **Middleware de Tratamento de Exceções:** Um middleware global agora captura todas as exceções não tratadas da aplicação, registrando o erro e retornando uma resposta JSON padronizada e amigável. Isso eliminou completamente os blocos `try-catch` repetitivos dos `Controllers`.
* **Pipeline Behavior para Validação:** Criamos um pipeline no MediatR que intercepta todos os `Commands` recebidos. Usando `FluentValidation`, ele executa as regras de validação associadas antes que o `Command` chegue ao seu `Handler`, garantindo que a lógica de negócio só execute com dados válidos.

### d. Camada de Acesso a Dados com EF Core (Padrão Repositório)

A camada de acesso a dados foi completamente modernizada:
* **ADO.NET foi substituído por Entity Framework Core**, o ORM (Object-Relational Mapper) padrão do ecossistema .NET.
* A persistência agora é gerenciada por um `DbContext` (`ApplicationDbContext`), e as operações de banco de dados são realizadas de forma **assíncrona (`async/await`)**.
* O esquema do banco de dados é gerenciado via **EF Core Migrations**, permitindo o versionamento e a evolução da estrutura do banco de dados a partir do código.

## 3. Principais Melhorias e Refatorações de Nível Especialista

Durante o processo, as seguintes melhorias foram implementadas:

* **Modernização da Camada de Dados:** Substituição completa do ADO.NET manual pelo **Entity Framework Core**, com uso de `DbContext`, `Migrations` e consultas assíncronas com LINQ. Isso reduziu drasticamente o código boilerplate e aumentou a segurança e a manutenibilidade.

* **Centralização de Erros e Validação:** Implementação de um **Middleware de Erros** e de um **Pipeline de Validação** com MediatR e FluentValidation. Essa mudança limpou os `Controllers` e `Handlers`, centralizando responsabilidades e tornando o código mais limpo e aderente ao princípio DRY (Don't Repeat Yourself).

* **Refatoração do Modelo de Domínio:** A entidade `Chamado` foi corrigida para usar **Propriedades de Navegação** do EF Core (`public Solicitante Solicitante { get; set; }`) em vez de propriedades `string`, representando corretamente os relacionamentos do banco de dados e simplificando as consultas com o uso do `.Include()`.

* **Unidade de Trabalho nos Command Handlers:** Os `CommandHandlers` (ex: `GravarChamadoCommandHandler`) foram refatorados para gerenciar sua própria **Unidade de Trabalho** com transações explícitas do `DbContext`. Isso resolveu problemas de concorrência com o SQLite (`database is locked`) e garantiu a atomicidade das operações de escrita.

* **Estabilização do Projeto e Dependências:** Foi realizado um diagnóstico profundo para corrigir instabilidades de compilação e execução, envolvendo:
    * Alinhamento das versões do .NET (`Target Framework`) em todos os projetos.
    * Resolução de conflitos severos de versões de pacotes NuGet (como `FluentValidation`, `Microsoft.Data.Sqlite`, `Microsoft.AspNetCore.Mvc.Testing`), garantindo a compatibilidade total com o .NET Core 2.1.
    * Correção dos arquivos de projeto (`.csproj`) para um formato moderno e consistente.

* **Correções no Front-End:** Resolução de erros de JavaScript, como o erro **404 para o arquivo `utils.js`**, e implementação da **localização para português (pt-BR)** das mensagens de validação do jQuery Validate.

## 4. Testes Unitários Aprimorados

A suíte de testes unitários foi refatorada e aprimorada para se alinhar à nova arquitetura.

* **Habilitação para Testes com DI:** A introdução de interfaces e a aplicação correta da Injeção de Dependência foram cruciais para tornar os `Handlers` testáveis.
* **Evolução dos Testes de Commands:** Os testes para `CommandHandlers` (que realizam escrita no banco) foram evoluídos. Em vez de apenas simular ("mocar") as interfaces DAL, eles agora utilizam o **Provedor de Banco de Dados Em Memória do EF Core**. Essa abordagem permite testes de unidade mais robustos e confiáveis, que validam a lógica do handler em conjunto com a interação real do EF Core.
* **Tecnologias Utilizadas:** A suíte de testes foi construída utilizando um conjunto de ferramentas padrão e modernas no ecossistema .NET: **xUnit**, **Moq** e **FluentAssertions**.

## 5. Como Executar o Projeto

### Pré-requisitos

* Visual Studio 2022 (com a carga de trabalho *ASP.NET and web development*)
* .NET Core 2.1 SDK

### Passos

1.  Clone o repositório.
2.  Abra o arquivo `WebApp_Desafio_Desenvolvimento.sln` no Visual Studio.
3.  O Visual Studio deve restaurar os pacotes NuGet automaticamente. Caso contrário, clique com o botão direito na solução e selecione "Restore NuGet Packages".
4.  **Para criar o banco de dados pela primeira vez:**
    * Abra o **Package Manager Console** (`Tools > NuGet Package Manager > Package Manager Console`).
    * Selecione `WebApp-Desafio-FrontEnd` como o "Default project".
    * Execute o comando: `Update-Database`
5.  Defina o projeto `WebApp_Desafio_FrontEnd` como projeto de inicialização (Startup Project).
6.  Pressione `F5` para iniciar a aplicação.

> O banco de dados (`desafio_dev_efcore.db`) é gerenciado pelo EF Core Migrations. A connection string está no `appsettings.json`.