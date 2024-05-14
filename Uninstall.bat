@echo off
sc stop ServiceFile
sc delete ServiceFile
echo Serviço parado e desinstalado com sucesso.
pause