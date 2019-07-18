 $nssm = "";

if ([Environment]::Is64BitOperatingSystem) {
 $nssm = Resolve-Path ".\win64\nssm.exe"
}
else {
 $nssm = Resolve-Path ".\win32\nssm.exe"
}

$service = Resolve-Path ".\BCSTransfer.exe";
$arguments = 'install BCSTransfer "' + $service.Path + '"'
Start-Process -FilePath $nssm.Path -ArgumentList $arguments