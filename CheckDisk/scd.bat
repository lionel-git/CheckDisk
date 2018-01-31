rem @echo off
set SVCNAME=CheckDisk
sc.exe delete %SVCNAME%
sc.exe create %SVCNAME% binPath= %~dp0\%SVCNAME%.exe DisplayName= %SVCNAME%
sc.exe description %SVCNAME% "Regularly poll some disk drives"