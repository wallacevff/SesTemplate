# Gerar JWT Key
    node -p "crypto.randomBytes(256).toString('base64');"

# Migrations
## Terminal Gerenciador de Pacotes
- Listar Migrations

      Get-Migration -StartUp SesTemplate.Api -Project SesTemplate.Infra.Data

- Criar Migration

      Add-Migration -StartUp SesTemplate.Api -Project SesTemplate.Infra.Data <Nome da Migration>
- Remover Migration

      Remove-Migration -StartUp SesTemplate.Api -Project SesTemplate.Infra.Data
- Atualizar o Banco de Dados

      Update-Database -StartUp SesTemplate.Api -Project SesTemplate.Infra.Data <Nome da Migration>
- Listar Migrations

      Get-Migration -s SesTemplate.Api -p SesTemplate.Infra.Data

## Terminal Comum
- Criar Migration

      dotnet ef migrations add -s SesTemplate.Api -p SesTemplate.Infra.Data <Nome da Migration>
- Remover Migration

      dotnet ef migrations remove -s SesTemplate.Api -p SesTemplate.Infra.Data
- Atualizar o Banco de Dados

      dotnet ef database update -s SesTemplate.Api -p SesTemplate.Infra.Data
- Listar Migrations

      dotnet ef migrations list -s SesTemplate.Api -p SesTemplate.Infra.Data