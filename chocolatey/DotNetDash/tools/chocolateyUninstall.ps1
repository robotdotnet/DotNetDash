$desktopPath = [Environment]::GetFolderPath("Desktop")
$lnkPath = $desktopPath + "\DotNetDash.lnk"

If (Test-Path $lnkPath)
{  
	Remove-Item $lnkPath
}