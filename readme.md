# Pepes Webcrawler

Simple webcrawler for getting wallpapers/pictures from 4chan.
Single thread. Can be optimized to multithreaded for better performance.  

## Prerequisites  

.net core 3.1 installed  [https://dotnet.microsoft.com/download/dotnet-core](https://dotnet.microsoft.com/download/dotnet-core)

Change this CONST VARIABLE "PathForSavingImages" into where you want to store your images.
The variable is in this file: [pepsCrawler/Helpers/StringConstants.cs](pepsCrawler/Helpers/StringConstants.cs)  

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

## Possible extensions

To remove nude images:  
[https://github.com/yahoo/open_nsfw](https://github.com/yahoo/open_nsfw)
