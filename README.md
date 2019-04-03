# Keep-Alive
A simple and lightweight keep-alive utility.

## Requirements
When you have the .NET Core Runtime installed on your OS of choice you are ready to go.
Otherwise you can download it here: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

## Usage
### Command-Line
```
dotnet RS.KeepAlive.dll [arguments] [options]

Arguments:
  <URL>                     URL(s) that are called regularly

Options:
  -h|--help                 Show help information
  -i|--interval <INTERVAL>  Interval in minutes in which the URL(s) are called
```

Example
```
dotnet RS.KeepAlive.dll "https://example.com"
```
or for multiple URLs
```
dotnet RS.KeepAlive.dll "https://example.com" "https://other.example.com"
```

### Docker
You can quickly run from one of the pre-built images `xzelsius/keep-alive:latest` or `xzelsius/keep-alive:debian-arm32`

Type the following command
```
docker run -rm -it xzelsius/keep-alive:latest <URL> [-i <INTERVAL>]
```

## Copyright and License

Copyright © Raphael Strotz under the MIT license.
