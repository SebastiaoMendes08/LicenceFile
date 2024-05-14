@echo off
echo Installing service...
sc create "ServiceFile" binPath= "%~dp0%ServiceFile.exe" obj= "NT AUTHORITY\NetworkService" start= auto > NUL
netsh advfirewall firewall add rule name="ServiceFile" dir=in action=allow program="%~dp0%servicefile.exe" enable=yes > NUL
netsh advfirewall firewall add rule name="ServiceFile" dir=out action=allow program="%~dp0%servicefile.exe" enable=yes > NUL
echo Service is starting...
sc start ServiceFile > NUL

:WAIT
timeout /t 5 > NUL
for /f "tokens=4" %%s in ('sc query ServiceFile ^| find "STATE"') do if NOT "%%s"=="RUNNING" goto WAIT
echo Service is now running!

servicefile