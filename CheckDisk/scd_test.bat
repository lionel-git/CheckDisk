rem @echo off
set SVCNAME=CheckDiskTest
sc.exe delete %SVCNAME%
sc.exe create %SVCNAME% binPath= %~dp0\CheckDisk.exe DisplayName= %SVCNAME% start= demand
sc.exe description %SVCNAME% "Regularly poll some disk drives, test version"