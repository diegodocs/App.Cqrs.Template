REM Resolve PowerShell
IF EXIST c:\windows\sysnative\NUL (
	SET ps="c:\windows\sysnative\windowspowershell\v1.0\powershell.exe"
) else (
	SET ps="c:\windows\system32\windowspowershell\v1.0\powershell.exe"
)

@echo off

SET vsTargetsName=MSBuild.Microsoft.VisualStudio.Web.targets
nuget install %vsTargetsName%

setlocal ENABLEEXTENSIONS
FOR /F "usebackq tokens=2,* skip=2" %%L IN (
    `reg query "HKEY_LOCAL_MACHINE\Software\Microsoft\MSBuild\ToolsVersions\14.0" /v MSBuildToolsRoot`
) DO SET msBuildPath=%%M

SET nugetTargets="%cd%\MSBuild.Microsoft.VisualStudio.Web.targets.14.0.0\tools\VSToolsPath"
SET finalTargets="%msBuildPath%Microsoft\VisualStudio\v14.0\"

xcopy /S/Y %nugetTargets% %finalTargets%


REM Run build
PUSHD "%~dp1"
psake """%~dp0Build.ps1""" %*
