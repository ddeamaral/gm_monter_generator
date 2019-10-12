dotnet publish -c Release --self-contained -r linux-x64 /p:PublishSingleFile=true;
dotnet publish -c Release --self-contained -r win-x64 /p:PublishSingleFile=true;
dotnet publish -c Release --self-contained -r osc-x64 /p:PublishSingleFile=true;
