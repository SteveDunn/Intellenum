param($verbosity = "minimal", $job = "short") # ShortRun MediumRun LongRun LegacyJitX86 LegacyJitX64 RyuJitX64

$artifacts = ".\artifacts"
$localPackages = ".\local-global-packages"

function WriteStage([string]$message)
{
    Write-Host "############################################" -ForegroundColor Cyan
    Write-Host "**** " $message -ForegroundColor Cyan
    Write-Host "############################################" -ForegroundColor Cyan
    Write-Output ""
}

<#
.SYNOPSIS
  Taken from psake https://github.com/psake/psake
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
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

WriteStage("Building release version of Intellenum...")

if(Test-Path $artifacts) { Remove-Item $artifacts -Force -Recurse }

New-Item -Path $artifacts -ItemType Directory

New-Item -Path $localPackages -ItemType Directory -ErrorAction SilentlyContinue

if(Test-Path $localPackages) { Remove-Item $localPackages\Intellenum.* -Force -ErrorAction SilentlyContinue }

WriteStage("Cleaning, restoring, and building release version of Intellenum...")

WriteStage("... clean ...")
exec { & dotnet clean Intellenum.sln -c Release --verbosity $verbosity}

WriteStage("... restore ...")
exec { & dotnet restore Intellenum.sln --no-cache --verbosity $verbosity }

exec { & dotnet build Intellenum.sln -c Release -p Thorough=true --no-restore --verbosity $verbosity}

exec { & dotnet pack src/Intellenum.Pack.csproj -c Release -o $artifacts --no-build --verbosity $verbosity }

################################################################
WriteStage("Running benchmarks ... job is $job")
exec { & dotnet clean .\src\Benchmarks\Benchmarks.csproj --verbosity $verbosity }
exec { & dotnet restore .\src\Benchmarks\Benchmarks.csproj --verbosity $verbosity }
exec { & dotnet run --project .\src\Benchmarks\Benchmarks.csproj -c release -- --job $job --verbosity $verbosity }
################################################################

WriteStage("Done!")
