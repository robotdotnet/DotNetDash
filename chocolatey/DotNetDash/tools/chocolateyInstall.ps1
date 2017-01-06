$packageName = 'dotnetdash'
$url = "https://github.com/RobotDotNet/DotNetDash/releases/download/$env:chocolateyPackageVersion/DotNetDash.zip"
$unzipLocation = $(Split-Path -parent $MyInvocation.MyCommand.Definition)
$desktopPath = [Environment]::GetFolderPath("Desktop")
$lnkPath = $desktopPath + "\DotNetDash.lnk"
$exePath = $unzipLocation + "\" + $packageName + ".exe"

Write-Host "Uninstalling previous version"
Remove-Item $unzipLocation -Force -Recurse -Exclude $unzipLocation\logs,$unzipLocation\Plugins -ErrorAction SilentlyContinue
Write-Host "Done"

Install-ChocolateyZipPackage $packageName $url $unzipLocation
Install-ChocolateyShortcut -shortcutFilePath $lnkPath -targetPath $exePath
