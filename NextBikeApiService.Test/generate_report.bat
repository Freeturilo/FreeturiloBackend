
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Exclude=\"[*]FreeturiloWebApi.*,[*]NextBikeDataParser.*\"
reportgenerator -reports:".\coverage.cobertura.xml" -targetdir:".\coverage" -reporttypes:Html