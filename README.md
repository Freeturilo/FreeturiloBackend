# Freeturilo Backend

## Development
 - IDE: Visual Studio 2019
 - C#
 - SDK: .NET 5.0

## Build
1. Clone the repository
2. Open directory in VS2019
3. Read chapter `Modules` and follow the steps
4. Build the solution

## Tests

You can test all modules at once from root directory by executing `run_tests.bat` from Developer Command Prompt in VS2019. You can also test modules separately. To do so, change current directory to `*.Test` and execute `generate_report.bat` script.

After tests passes there are reports generated (`*.Test\coverage\index.html`). You can check lines, methods and branches code coverage.

## Modules

### 1.Freeturilo Web API
 - Purpose
    
    Module provides REST API for users and administrators of Freeturilo system.

 - Pre-build

    You need to create two files in directory `FreeturiloWebApi`:

    - `.google-token` containing key to the Google Directions API
    - `.sendgrid-token` containing token to SendGrid service
    
 - Usage

    Launch the application using VS2019. Documentation of endpoints is available on address https://freeturilowebapi.azurewebsites.net/swagger/index.html.



### 2. NextBike Data Parser
 - Purpose

    Simple library to deserialized XML formatted responses from NextBike API.

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

    Background worker updating information about stations. It requests NextBike  and Freeturilo APIs periodically and compares responses.

- Pre-build:

    You need to create `.admin-passwd` file in which there will be admin's password to Freeturilo server.



