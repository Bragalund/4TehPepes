# Pepes Webcrawler

Webcrawler for getting wallpapers/pictures from 4chan.  

## Prerequisites  

.net core 3.1 installed  

Change this CONST VARIABLE(PathForSavingImages) into where you want to store your images in this file: [pepsCrawler/Helpers/StringConstants.cs](pepsCrawler/Helpers/StringConstants.cs)  

## How to run  

```  
dotnet run -p pepsCrawler\pepsCrawler.csproj  
```  

## How to build executable

```()
dotnet publish --self-contained true -p:PublishTrimmed=true -r win10-x64  
```

Run in commandline:
```()
./pepsCrawler/bin/Debug/netcoreapp3.1/pepsCrawler.exe
or  
.\pepsCrawler\bin\Debug\netcoreapp3.1\pepsCrawler.exe
```