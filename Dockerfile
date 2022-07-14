FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY *.sln .
COPY ToDoList/*.csproj ./ToDoList/
COPY ToDoAppTest/*.csproj ./ToDoAppTest/
COPY todoapp/*.csproj ./todoapp/
EXPOSE 5000
EXPOSE 45665
#
RUN dotnet restore 
#
# copy everything else and build app
COPY ToDoList/. ./ToDoList/
COPY ToDoAppTest/. ./ToDoAppTest/
COPY todoapp/. ./todoapp/ 
#
WORKDIR /app/todoapp
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app 
#
COPY --from=build /app/todoapp/out ./
EXPOSE 5000
EXPOSE 45665
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "todoapp.dll"]
