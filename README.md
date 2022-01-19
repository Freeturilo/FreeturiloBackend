# Freeturilo Backend

## Development
 - IDE: Visual Studio 2019
 - Language: C#
 - SDK: .NET 5.0

## Build
1. Clone the repository
2. Open the solution in VS2019
3. Read chapter `Modules` and follow the steps
4. Build the solution

## Tests

You can test all modules at once from root directory by executing `run_tests.bat` in Developer Command Prompt in VS2019. You can also test modules separately. To do so, change current directory to `*.Test` and execute the `generate_report.bat` script.

After tests are passed, reports are generated (`*.Test\coverage\index.html`). You can check code coverage of lines, methods and branches of the solution.

## Modules

### 1.Freeturilo Web API
 - Purpose
    
    Module provides REST API for users and administrators of the Freeturilo system.

 - Pre-build

    You need to create two files in directory `FreeturiloWebApi`:

    - `.google-token` containing a key to the Google Maps API
    - `.sendgrid-token` containing a token to the SendGrid service

    You also need to fill in the connection string to your database in `FreeturiloWebApi\appsettings.json`.
    
 - Usage

    Launch the application using VS2019. Documentation of endpoints is available at https://freeturilowebapi.azurewebsites.net/swagger/index.html.



### 2. NextBike Data Parser
 - Purpose

    Simple library to deserialize XML formatted responses from NextBike API.

 - Requirements
    - `xsd` (https://docs.microsoft.com/en-us/dotnet/standard/serialization/xml-schema-definition-tool-xsd-exe)
 
 - Pre-build

    You need to generate `markers.cs` file from `markers.xsd` using `xsd` tool.
    Change directory to `NextBikeDataParser` and execute:

    ```
    xsd markers.xsd /classes /namespace:NextBikeDataParser
    ```

### 3. NextBike API Service
- Purpose

    Background worker updating data about stations. It sends requests to NextBike and Freeturilo APIs periodically and compares responses.

- Pre-build:

    You need to create `.admin-passwd` file in which there will be admin's password to Freeturilo server.
