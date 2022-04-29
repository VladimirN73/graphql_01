
set sln=./gql_01.sln

dotnet restore %sln%
      
dotnet build --no-restore %sln%

pause