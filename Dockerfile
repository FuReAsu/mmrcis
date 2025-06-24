#Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

COPY . /app

RUN cd /app && dotnet publish --ucr --no-self-contained -o /published

#RUN
FROM mcr.microsoft.com/dotnet/aspnet:9.0

COPY --from=build /published /app

RUN sed -i 's/localhost/mmrcis-mssql/g' /app/appsettings.json 

WORKDIR /app

CMD ["dotnet","mmrcis.dll"]
