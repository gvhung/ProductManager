#ProductManager

##Descrição Geral
Este projecto consiste numa aplicação web, desenvolvida em ASP.NET MVC, que permite efetuar a gestão de um catálogo de produtos, agrupados por categorias e subcategorias, e também a gestão de empregados integrada com o sistema de autenticação da aplicação.

A aplicação web utiliza uma base de dados própria, em SQL Server, para gerir o sistema de autenticação e utiliza chamadas a um web service para manipular os restantes dados da aplicação: Produtos, Subcategorias, Categorias e Empregados.

O web service foi desenvolvido em ASP.NET Web Api e foi implementado o padrão OData em todos os controladores. O acesso à base de dados SQL Server, onde estão os dados da aplicação é feito através de Entity Framework.

Estão solução utiliza três camadas:
###1. Data Access Layer
Nesta camada existem dois projectos, Entities e DataAccess. 
O projeto Entities é referenciado em todos os outros projetos da solução e disponibiliza todas as classes e enums necessários para implementar a estrutura de dados.
O projeto DataAccess disponibiliza a classe que permite instanciar o objeto de contexto para acesso à base de dados com Entity Framework, é também disponilizado um interface desta classe para permitir uma melhor abstração do contexto durante a realização de testes.

###2. Web Service
Nesta camada estão presentes o projeto WebApi e respetivo projeto de testes.
Ambos referenciam os projetos da camada Data Access Layer.

###3. MVC Application
Nesta camada estão presentes o projeto MVC e respetivo projeto de testes.
Ambos apenas referenciam o projetos Entities.
O projeto de testes foi criado mas ainda não foi implementado nenhum teste.

#

##Detalhes Técnicos

###ProductManager.Entities
- Todas as classes têm na propriedade DateModified como valor por defeito DateTime.Now, desta forma não há necessidade de implementar mecanismos para atribuição do valor para esta propriedade.
- Na classe Employee foi acrescentado a propriedade Email para servir de elo de ligação entre a tabela de empregados da base de dados principal e a tabela de utilizadores da base de dados de autenticação do interface web.
- Foi criada a propriedad FullName na classe Employee para ser mais fácil obter uma string com o primeiro e último nome do empregado mantendo-se assim o requisito do projecto de guardar o nome em três campos: FirstName, MiddleName e LastName.
- A propriedade Title foi implementada com o enum PersonTitle.

###ProductManager.DataAccess
- O acesso aos dados via EntityFramework foi implementado com a abordagem Code First.
- Foi criado o interface IProductManagerContext para facilitar a abstração do contexto o projecto de testes.
- A classe ProductManagerInitializer contém a implementação para inicializar a base de dados principal com dados "demo" sempre que o modelo de dados sofra alterações.

###ProductManager.WebApi
- Este projeto utiliza pacote Nuget Microsoft.AspNet.OData de forma a implementar o padrão OData v4.
- Os controllers foram implementados para permitirem a injeção de dependencias para que se consiga efetuar teste sobre os mesmos sem utilizar o ambiente de produção.
- Todos os controllers são do tipo ODataController e foram criados através de um template instalado com a extensão do Visual Studio OData v4 Web API Scaffolding.

###ProductManager.WebApi.Tests
- São feitos teste as métodos Get,Post,Put,Patch e Delete de todos os controladores.
- Foi utilizado o padrão de repositório para efetuar os teste sem comprometer a base de dados que está em produção,
- O repositório deriva da classe IProductManagerContext e inicializa com os mesmos dados demo cada vez que arrancam os testes.

###ProductManager.MVC
- Este projeto utiliza pacote Nuget Simple.OData.Client para facilitar a obtenção de dados a partir de um web service odata.
- Para o sistema de autenticação foi instalado o pacote Nuget Microsoft.AspNet.Identity.Samples poque é uma solução mais completa do que aquela que está no template MVC do Visual Studio. Assim fica automaticamente implementado o sistema de gestão de autenticação com roles. Este modelo de autenticação utiliza Entity Framework com a abordagem Code First.
- Os controllers foram implementados para permitir a injeção de dependencias para que se consiga efetuar teste sobre os mesmos sem utilizar o ambiente de produção.
- Foi criada a classe ProductManagerODataClient para gerar objetos do tipo ODataClient. Esta classe tem uma propriedade privada (address) do tipo string, que armazena o endereço do webservice a utilizar, e dois métodos um deles para devolver o objeto configurado para ligar ao endereço da propriedade address e outro método que aceita uma string como argumento para devolver um objeto que se liga ao endereço passado em parâmetro, este método foi idealizado para utilizar nos testes.
- As views desta aplicação foram adaptadas a partir das que são criadas por scaffolding.

###ProductManager.MVC.Test
- Este projeto foi criado mas não foi implementado.
- Para a implementação dos testes está planeado arrancar uma instância self-hosted do web service utilizando um repositório com dados demo. Desta forma serão executados os controllers através do construtor que aceita o endereço do webservice como argumento, assim o métodos vão chamar um web service diferente do que está em produção.

#

##Configuração
Para implementar este projeto é necessário garantir previamente a correcta configuração para três pârametros:
1. Indicar a connection string (ProductManager) com a localização da base de dados principal no ficheiro web.config do projeto WebApi.
2. Indicar a connection string (ProductManagerMVCIdentity) com a localização da base de dados para o sistema de autenticação no ficheiro web.config do projeto MVC.
3. Indicar o endereço (ex: http://[endereço:porta]/odata/ onde será implementado o web service na váriavel address dentro classe ProductManagerODataClient do projeto MVC.
Quando o web service é executada pela primeira vez são preenchidos automaticamente dados de exemplo na base de dados principal conforme definido na classe ProductManagerInitializer do projeto DataAcces. 
Quando a aplicação MVC é executada pela primeira vez são criados dois utilizadores com o mesmo email dos que foram inicializados na base de dados principal. Assim já existe pelo menos um utilizador no role Admin de forma a ser possível utilizar a aplicação sem ter inicializada a base de dados principal.
Os dados dos utilizador inicializados estão definidos dentro da classe ApplicationDbInitializer em argumentos nas chamadas ao construtor da classe InitializeIdentityForEF. Os dados incialiazado são os seguintes:
	InitializeIdentityForEF(context,"George Thomas", "georgeft@example.com", "Pa$$w0rd", "Admin");
	InitializeIdentityForEF(context, "John Smith", "johnws@example.com", "Pa$$w0rd", "User");
Assim sendo, o accesso à aplicação pela primeira vez deve ser feito com este login:
- **Utilizador**: georgeft@example.com
- **Password**: Pa$$w0rd

#

##Funcionalidades Web Service
O web service apresenta no endpoint /odata a implementação de consultas padrão OData para as entidades: /Employees, /Categories, /SubCategories e /Products.

#

##Funcionalidades MVC Application
Para utilizar a aplicação é necessário estar autenticado. Caso um utilizador anónimo tente aceder à aplicação será redirecionado para a página /Account/Login onde poderá efetuar a sua autenticação ou aceder à página de registo de novos utilizadores /Account/Register. Quando é criado um novo utilizador é criado automaticamente um novo empregado através do web service, todos os utilizadores criados neste método pertencem ao role User.
###Inicio: /
A página inicial da aplicação é a lista com todos os produtos (/Product).
###Contas de Utilizador /Account
Para além dos processos de autenticação (/Login e /Logoff) e registo de novos utilizadores (/Register) também é possível o utilizador alterar e/ou recuperar a sua password através das páginas /ResetPassword e /ForgotPassword. O método de recuperação de password requer uma prévia configuração de um serviço de envio de emails.
###Empregados: /UserAdmin
Nesta secção é possível criar, editar e eliminar os utilizadores da aplicação, todas as operações sobre utilizadores serão replicadas automaticamente para a tabela de empregados através do web service. Só têm acesso a esta secção os utilizadores que pertencem ao role Admin. Quando se cria um novo utilizador através desta secção é possível escolher o role ao qual irá pertencer.
###Categorias: /Category
Só utilizadores que pertencem ao role Admin têm acesso a esta secção onde é possível criar, editar e eliminar categorias.
###SubCategorias: /SubCategory
Só utilizadores que pertencem ao role Admin têm acesso a esta secção onde é possível criar, editar e eliminar subcategorias.
###Produtos: /Product
Todos os utilizadores autenticados têm acesso a esta secção, no entanto os links de acesso às operações de editar e eliminar um determinado produto só estarão disponíveis se o utilizador for proprietário desse produto. Caso um utilizador não proprietário tente aceder por endereço à edição ou eliminação de um produto será reencaminhado para a página /Details desse produto sendo informado que não tem permissão para efetuar essa operação.
Quando se cria um novo produto o campo Employee é preenchido automaticamente com o utilizador que está autenticado naquele momento, sendo possível alterar o valor desse campo e criar um novo produto sendo outro utilizador o proprietário.
Os utilizadores que pertencem ao role Admin podem aceder a todas as operações sobre produtos sem restrições.