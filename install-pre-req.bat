REM Resolve PowerShell
IF EXIST c:\windows\sysnative\NUL (
	SET ps="c:\windows\sysnative\windowspowershell\v1.0\powershell.exe"
) else (
	SET ps="c:\windows\system32\windowspowershell\v1.0\powershell.exe"
)
REM Install PowerShell 4
WHERE choco
IF %ERRORLEVEL% NEQ 0 %PS% -NoProfile -ExecutionPolicy unrestricted -Command "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET PATH=%PATH%;%systemdrive%\ProgramData\chocolatey\bin
choco install powershell4 --acceptlicense -y
choco install nuget.commandline --acceptlicense -y
choco install psake --acceptlicense -y
choco install dotnet4.6 --acceptlicense -y
choco install netfx-4.6-devpack --acceptlicense -y
choco install microsoft-build-tools -version 14.0.23107.10 --acceptlicense -y
choco install curl --acceptlicense -y
