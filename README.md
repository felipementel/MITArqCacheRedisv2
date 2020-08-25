# Cache Redis em .net Core WebAPI

Projeto de demonstração de como utilizar o Redis como cache para webAPI em .net Core 3.1 - Seguindo alguns princípios de arquitetura e boas praticas.

### Tecnologias utilizadas

- .net Core 3.1
- Redis
- Docker
- AutoMapper
- Azure SQL Database
- Fluent Validation
- Application Insights
- Entity Framework Core + Fluent API
- xUnit + NSubstitute

** Projeto ainda na versão 0.8 - e não concluído

Setup:

1. Disponibilizar um servidor SQLServer e preencher o arquivo appsettings.json do projeto na solution folder 1
2. Disponibilizar um servidor para o Redis (aqui vou deixar o exemplo de como usar o Redis no Docker)

 "ConnectionStrings": {
    "Redis": "CONEXAO_REDIS",
    "Aplicacao": "CONEXAO_SQL_SERVER"
  },

+ SQL Server ou SQL Database

Pode-se usar o SQL Database do Azure ou um servidor SQL Server

+ Montar ambiente do Redis no Docker

  docker pull redis

  docker run --name redisteste -p 6379:6379 redis 

> Abra uma nova aba no seu terminal

> docker exec -it redisteste redis-cli
>
> Teste: 
>
> Digite o comando ping e tenha o retorno PONG
>
> 127.0.0.1:6379> ping
>
> PONG
>
>
> ** Feito isso, volte a configuração do appsettings.json e preencha a conexao com o Redis com o seguinte valor:
>
> localhost, port:6379

+ Desmontar ambiente do Redis no Docker

> docker stop <container id> | para de rodar a imagem

> docker rm <container id> | exclui o container

Para teste de comando do Redis, veja o tutorial abaixo


http://try.redis.io/
