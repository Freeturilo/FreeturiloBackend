# NextBike Data Parser

Library provides class necessary to deserialized data from NextBike API.

## Requirements

* xsd.exe

    `markers.xsd` file contains the schema used to generate the class `markers`. Name of class is forced by the data format.

    To generate `Schema.cs` file run
    ```
    xsd markers.xsd /clasees /namespace:NextBikeDataParser
    ```