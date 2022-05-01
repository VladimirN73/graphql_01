
graphql_01/readme.txt

History (newest on top)
01.05.22
add query 'product', query: {product(id: 1){name}}
it works. navigate to ui/playground and ask for {products{name}} 
30.04.22
check https://christian-schou.dk/how-to-implement-graphql-in-asp-net-core/
next error - {"errors":[{"message":"GraphQL query is missing."}]}
solved by .AddHttpMiddleware<AxSchema,  GraphQLHttpMiddleware<AxSchema>>()
try to run app - get error No service for type 'GraphQL.Server.Transports.AspNetCore.GraphQLHttpMiddleware has been registered.
add app.Module.Data
add .editorconfig
29.04.22 
add project app.Module.Common
add project app.Module.Graphql
add packages GraphQL, GraphQL.Server.Transports.AspNetCore, GraphQL.Server.Ui.Playground
update packages
created