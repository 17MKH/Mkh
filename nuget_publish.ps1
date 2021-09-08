Write-Host '请输入Key：'

$key = Read-Host
$dir = './_packages'

if(Test-Path -Path $dir){
    Remove-Item $dir -Recurse
}

dotnet build -c Release

Get-ChildItem -Path $dir | ForEach-Object -Process{
    dotnet nuget push $_.fullname -s https://api.nuget.org/v3/index.json -k $key
}

pause