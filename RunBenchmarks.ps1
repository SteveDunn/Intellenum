param($verbosity = "minimal", [switch] $short = $false)

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

################################################################
WriteStage("Running benchmarks ... short job?: $short")
exec { & dotnet clean .\src\Benchmarks\Benchmarks.csproj --verbosity $verbosity }
exec { & dotnet restore .\src\Benchmarks\Benchmarks.csproj --verbosity $verbosity }
exec { & dotnet build .\src\Benchmarks\Benchmarks.csproj -c Release --no-restore --verbosity detailed }

if($short)
{
    exec { & pushd; cd .\src\Benchmarks; dotnet run --project Benchmarks.csproj -c release --no-build -- --job short --verbosity $verbosity }
}
else
{
    exec { & pushd; cd .\src\Benchmarks; dotnet run --project Benchmarks.csproj -c release -- --verbosity $verbosity }
}

exec { & popd }

################################################################

WriteStage("Done!")
