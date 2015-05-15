@echo off
if exist "%VS120COMNTOOLS%vsvars32.bat" call "%VS120COMNTOOLS%vsvars32.bat" & goto VARSSET
IF EXIST "%VS110COMNTOOLS%vsvars32.bat" call "%VS110COMNTOOLS%vsvars32.bat" & goto VARSSET
IF EXIST "%VS100COMNTOOLS%vsvars32.bat" call "%VS100COMNTOOLS%vsvars32.bat" & goto VARSSET
echo "Could not detect VS version!" & goto ERROR
:VARSSET


mkdir nupkg_archive

msbuild.exe "src\Simplicity\Simplicity.csproj" /p:configuration=Release
if %ERRORLEVEL% neq 0 goto ERROR

.nuget\nuget.exe pack src\Simplicity\Simplicity.csproj -Prop Configuration=Release
if %ERRORLEVEL% neq 0 goto ERROR

for %%f in (*.nupkg) do (
	.nuget\nuget.exe push %%f
	if %ERRORLEVEL% neq 0 goto ERROR
)

copy *.nupkg nupkg_archive\
del *.nupkg

goto SUCCESS


:SUCCESS
	echo  SUCCESS                                   
	goto END                                             

:ERROR
	echo .
	echo Ffffff    Aaa     Iiii  Ll                                                               
	echo Ff       Aa Aa     Ii   Ll                                                               
	echo Fffff   Aa   Aa    Ii   Ll                                                              
	echo Ff      Aaaaaaa    Ii   Ll                                                               
	echo Ff     Aa     Aa  Iiii  Llllll                                                           
	echo .
                                                                                         
:END
