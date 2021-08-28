@echo off
set /a gencert_debug=0
set "gencert_certinfo=/C=CA/L=Vancouver/O=United Front Games/CN=United Front Games"
set "gencert_pass=1234"
set "gencert_openssl=C:\Program Files\OpenSSL-Win64\bin\"

echo Modnation SSL cert generator
echo By derole
echo ----------------------------

rem check permissions
net session > nul 2>&1
if not %errorLevel% == 0 (
	echo Please run this script with administrator rights!
	pause
	exit
)

rem check OpenSSL is installed
if not exist "%gencert_openssl%" (
	echo Please install OpenSSL!
	pause
	exit
)

rem cd to OpenSSL directory
cd /d "%gencert_openssl%"

rem make a temporary folder to hold cert
mkdir .\certs >nul 2>&1
mkdir .\certs\temp >nul 2>&1

rem gen cert
rem WARNING: for production you may want to change the password, 
rem please note this will also need to be changed for the server to load the cert correctly
echo Generating cert...
if %gencert_debug% == 0 (
	openssl req -x509 -newkey rsa:2048 -keyout .\certs\temp\key.pem -out .\certs\temp\cert.pem -days 65535 -nodes -subj "%gencert_certinfo%" >nul 2>&1
	openssl pkcs12 -inkey .\certs\temp\key.pem -in .\certs\temp\cert.pem -export -out .\certs\output.pfx -password pass:%gencert_pass% >nul 2>&1
) else (
	openssl req -x509 -newkey rsa:2048 -keyout .\certs\temp\key.pem -out .\certs\temp\cert.pem -days 65535 -nodes -subj "%gencert_certinfo%"
	openssl pkcs12 -inkey .\certs\temp\key.pem -in .\certs\temp\cert.pem -export -out .\certs\output.pfx -password pass:%gencert_pass%
)

rem check that the cert was created properly
if not %errorLevel% == 0 (
	echo An error occured creating the cert!
	pause
	exit
)
echo Cert created!
echo ----------------------------

rem clean up
rmdir .\certs\temp /s /q

rem show cert
%SystemRoot%\explorer.exe .\certs

rem debug
if %gencert_debug% == 1 (
	pause
)
rem pause