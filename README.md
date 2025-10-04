# Desafio de Desenvolvimento .NET - Solução Refatorada

## 1. Resumo do Projeto

> Este repositório contém a solução para o "Desafio de Desenvolvimento WebApp", um sistema simples de registro de chamados. O projeto original, que possuía bugs e oportunidades de melhoria, foi completamente refatorado para atender a todos os requisitos solicitados, com foco na aplicação de boas práticas de arquitetura de software, princípios SOLID e padrões de projeto modernos do ecossistema .NET.
>

## 2. Arquitetura e Padrões Aplicados

A arquitetura original, baseada em chamadas diretas a camadas de negócio (BLL), foi substituída por uma abordagem mais moderna e desacoplada, fundamentada nos seguintes padrões e princípios:

### a. CQRS (Command Query Responsibility Segregation)

A lógica de negócio foi dividida em duas responsabilidades distintas:

* **Commands**: Operações que alteram o estado do sistema (Criar, Atualizar, Excluir).
* **Queries**: Operações que apenas leem o estado do sistema (Listar, Obter por ID).

Essa separação, implementada na camada de `WebApp_Desafio_BackEnd`, torna o código mais claro, focado e permite otimizações específicas para leitura e escrita.

### b. Padrão Mediator

Utilizamos a biblioteca `MediatR` para implementar o padrão Mediator. Os Controllers (camada de apresentação) não conhecem mais a implementação da lógica de negócio. Em vez disso, eles simplesmente criam e enviam objetos de `Command` ou `Query`. O MediatR se encarrega de encontrar e invocar o `Handler` correspondente, promovendo um baixo acoplamento e aderindo ao Princípio da Responsabilidade Única (SRP).

### c. Injeção de Dependência (Dependency Injection - DI)

Removemos todas as instanciações diretas (ex: `new ChamadosBLL()`). Agora, todas as dependências, como o `IMediator`, são injetadas nos construtores dos controllers pelo container de DI nativo do ASP.NET Core. A `ConnectionString` também é gerenciada pelo DI, sendo lida do `appsettings.json`.

### d. Biblioteca Compartilhada (Shared Library)

Para evitar a duplicação de código e garantir uma única fonte de verdade para os modelos de dados de transferência (ViewModels/DTOs), foi criado o projeto `WebApp_Desafio_Shared` (.NET Standard 2.0). Este projeto é referenciado tanto pelo `WebApp_Desafio_FrontEnd` quanto pelo `WebApp_Desafio_API`, seguindo o Princípio Don't Repeat Yourself (DRY).

## 3. Requisitos do Desafio Implementados

Todos os requisitos obrigatórios e extras do desafio foram cumpridos.

### ✅ Requisitos Obrigatórios

* **CRUD de Departamentos:** Implementado do zero utilizando a nova arquitetura CQRS, com Commands para Gravar/Excluir e Queries para Listar/Obter.
* **Relatório de Departamentos:** Criado o endpoint e a lógica para gerar o relatório `.rdlc`, configurando a propriedade do arquivo para ser copiado durante o build.
* **Correção de Erros em Chamados:** O principal bug corrigido foi uma grave vulnerabilidade de SQL Injection nos métodos de `ObterChamado` e `ExcluirChamado`. Todas as queries foram parametrizadas para garantir a segurança.
* **Duplo-Clique para Editar:** Implementado nas telas de listagem de Chamados e Departamentos via JavaScript (jQuery), melhorando a usabilidade da interface.
* **Validação de Entrada:** Implementada utilizando a biblioteca `FluentValidation`. As regras de validação (tamanho máximo, campos obrigatórios) foram desacopladas em classes de `Validator` específicas para cada `Command`.

### ⭐ Desafio Extra

* **Validação de Regra de Negócio (Data Retroativa):** A regra para não permitir a criação de chamados com data retroativa foi implementada no `GravarChamadoCommandHandler`. A lógica foi refinada para ser aplicada apenas na criação (`ID == 0`), permitindo a edição de chamados antigos sem problemas.
* **Normalização do Banco de Dados (Tabela Solicitantes):** Em vez de um simples autocomplete em um campo de texto, a solução foi elevada a um nível profissional. Foi criada uma nova tabela `solicitantes` no banco de dados, e a tabela `chamados` foi refatorada para usar uma chave estrangeira (`IdSolicitante`). Toda a aplicação foi ajustada para essa nova estrutura de dados normalizada, garantindo consistência e performance.

#### Outras Melhorias Pertinentes:

* **Migração do Provedor SQLite:** A aplicação foi migrada do pacote legado `System.Data.SQLite` para o moderno e recomendado `Microsoft.Data.Sqlite`.
* **Resolução de Incompatibilidade de Arquitetura (x86/x64):** A migração para `Microsoft.Data.Sqlite` resolveu de forma definitiva o erro `DllNotFoundException: SQLite.Interop.dll`, permitindo que a aplicação rode em `AnyCPU` sem a necessidade de forçar a arquitetura para `x86`.
* **Centralização de Configuração e ViewModels:** A `ConnectionString` foi movida para o `appsettings.json` e os ViewModels foram centralizados no projeto `WebApp_Desafio_Shared`.

## 4. Como Executar o Projeto

### Pré-requisitos

* Visual Studio 2022 (com a carga de trabalho *ASP.NET and web development*)
* .NET Core 2.1 SDK

### Passos

1.  Clone o repositório.
2.  Abra o arquivo `WebApp_Desafio_Desenvolvimento.sln` no Visual Studio.
3.  O Visual Studio deve restaurar os pacotes NuGet automaticamente. Caso contrário, clique com o botão direito na solução e selecione "Restore NuGet Packages".
4.  Defina o projeto `WebApp_Desafio_FrontEnd` como projeto de inicialização (Startup Project).
5.  Pressione `F5` ou clique no botão "Run" para iniciar a aplicação.

> O banco de dados (`DesafioDB.db`) está incluído no projeto e configurado para ser copiado para o diretório de saída, não sendo necessária nenhuma configuração adicional.

## 5. Estrutura da Solução

* **WebApp\_Desafio\_FrontEnd:** Projeto principal da aplicação web (ASP.NET Core 2.1 MVC). Contém os Controllers, Views e arquivos estáticos.
* **WebApp\_Desafio\_API:** Projeto secundário de API (ASP.NET Core 2.1). Foi refatorado para também utilizar a arquitetura CQRS/MediatR.
* **WebApp\_Desafio\_BackEnd:** O "coração" da aplicação (.NET Framework). Contém as implementações dos Handlers do CQRS e a camada de acesso a dados (DAL).
* **WebApp\_Desafio\_Shared:** Biblioteca de classes compartilhada (.NET Standard 2.0). Contém todos os ViewModels, DTOs e Enums usados pelos outros projetos.
* **WinForm\_Desafio\_Reports:** Projeto auxiliar (Windows Forms) utilizado apenas para o design visual dos relatórios `.rdlc`.