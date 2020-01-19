param([string]$ConfigurationName="Test",
      [string]$SolutionDir="Test",
      [string]$TargetDir="Test",
      [string]$ProjectName="Test"
)
Write-Host $ProjectName
if ($ConfigurationName -eq "Debug") {powershell.exe -Command mkdir ($SolutionDir + 'PublishOutput') -Force; Compress-Archive -CompressionLevel Optimal -Path ($TargetDir + "\*") -DestinationPath ($SolutionDir + 'PublishOutput\' + $ProjectName + '.zip') -Force}
