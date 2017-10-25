@echo off

if exist version.txt (
  set /p Version=<version.txt
) else ( 
  set /p Version= New version number? (e.g. 1.0.0) 
)

if exist configuration.txt (
  set /p Configuration=<configuration.txt
) else ( 
  set /p Configuration= Configuration? (e.g. Release or Debug) 
)

@echo on

xcopy /s /i base v%Version%

cd v%Version%

..\fart -i "*.nuspec" "$(FsVersion)" "%Version%"
..\fart -i "*.nuspec" "$(FsConfiguration)" "%Configuration%"

forfiles /s /m *.nuspec /c "cmd /c ..\nuget pack @path"

cd ..
:pause