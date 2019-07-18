$nssm = "";

if ([Environment]::Is64BitOperatingSystem) {
 $nssm = Resolve-Path ".\win64\nssm.exe"
}
else {
 $nssm = Resolve-Path ".\win32\nssm.exe"
}

Start-Process -FilePath $nssm.Path -ArgumentList "start BCSTransfer"