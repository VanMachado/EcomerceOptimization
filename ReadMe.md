# E-Commerce Optimization Core API

Este projeto faz parte da monografia de conclusão de curso e tem como objetivo otimizar a estrutura de uma API de e-commerce, focando em autenticação, resiliência e padronização de respostas. O desenvolvimento foi realizado utilizando .NET 8.0, SQL Server, Docker e Swagger.

## 📌 Tarefas Realizadas

- **EOCORE-01**: Implementação de autenticação com resiliência para operações no banco de dados e requisições HTTP.
- **EOCORE-02**: Configuração da injeção de dependência para o Swagger e padronização das respostas relacionadas ao token.
- **EOCORE-03**: Desenvolvimento de endpoints com autenticação, mantendo o padrão de requisições e consultas resilientes.

## ⚙️ Tecnologias Utilizadas

- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [Docker](https://www.docker.com/)
- [Swagger (Swashbuckle)](https://swagger.io/tools/swagger-ui/)

## 🚀 Como Executar o Projeto

1. **Clone o repositório:**
   
   ```bash
   git clone https://github.com/VanMachado/EcomerceOptimization.git
   cd EcomerceOptimization
   ```

2. **Navegue até o diretório do projeto:**
   
   ```bash
   cd EcomerceOptimization
   ```

3. **Construa e execute os containers Docker:**
   
   ```bash
   docker-compose up --build
   ```
   
   > Certifique-se de que as portas necessárias estão disponíveis e que o Docker está instalado corretamente em sua máquina.
   
   

4. **Acesse a documentação da API via Swagger:**
   Abra o navegador e vá até: [http://localhost:5000/swagger](http://localhost:5000/swagger)
   
   > Caso a porta seja diferente, ajuste a URL conforme necessário.

# 

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
