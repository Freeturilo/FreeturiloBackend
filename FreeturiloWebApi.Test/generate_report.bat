
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
reportgenerator -reports:".\coverage.cobertura.xml" -targetdir:".\coverage" -reporttypes:Html