# Originally taken from https://github.com/jbogard/MediatR and https://github.com/psake/psake

function Exec
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

$artifactsPath = (Get-Item -Path ".\").FullName + "\artifacts"
if(Test-Path $artifactsPath) { Remove-Item $artifactsPath -Force -Recurse }

$branch = @{ $true = $env:APPVEYOR_REPO_BRANCH; $false = $(git symbolic-ref --short -q HEAD) }[$env:APPVEYOR_REPO_BRANCH -ne $NULL];
$revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$suffix = @{ $true = ""; $false = "$($branch.Substring(0, [math]::Min(10,$branch.Length)))-$revision"}[$branch -eq "master" -and $revision -ne "local"]
$commitHash = $(git rev-parse --short HEAD)
$buildSuffix = @{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)-$($commitHash)" }[$suffix -ne ""]
$versionSuffix = @{ $true = "--version-suffix=$($suffix)"; $false = ""}[$suffix -ne ""]

echo "build: Package version suffix is $suffix"
echo "build: Build version suffix is $buildSuffix"

# Build
echo "BUILD"

exec { & dotnet build Firestorm.sln -c Release --version-suffix=$buildSuffix -nowarn:618 }

# Test
echo "TEST"

$testDirs  = @(Get-ChildItem -Path tests -Include "*.Tests" -Directory -Recurse)
$testDirs += @(Get-ChildItem -Path tests -Include "*.IntegrationTests" -Directory -Recurse)
$testDirs += @(Get-ChildItem -Path tests -Include "*FunctionalTests" -Directory -Recurse)

ForEach ($folder in $testDirs) { 
    echo "Testing $folder.FullName"
    exec { & dotnet test $folder.FullName -c Release --no-build --no-restore }
}

# Pack
echo "PACK"

exec { & dotnet pack -c Release -o $artifactsPath --include-symbols --no-build $versionSuffix }