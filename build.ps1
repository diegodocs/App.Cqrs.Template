Framework 4.5.2
# Empty string checks are performed on all properties where used and they are reset to the default value if empty
# This is because they may be set to an empty string by the Go pipeline
properties { 
	$sln = "*.sln" # sln file to build
	$major = "1" # in Major.Minor.Build.Revision version
	$minor = "0" # in Major.Minor.Build.Revision version
	$build ="0" # in Major.Minor.Build.Revision version
	$revision = "0" # in Major.Minor.Build.Revision version
	$platform = "Any CPU" # sln platform configuration
	$config = "Release" 
	$enableCodeAnalysis = $true
	$testResults = "Build\Output\TestResults"
	$packages = "Build\Output\Deploy"
	$unitTestsProjectFilter = "*.UnitTests.csproj"
	$integrationTestsProjectFilter = "*.IntegrationTests.csproj"
	$acceptanceTestsProjectFilter = "*.AcceptanceTests.csproj"
	$smokeTestsProjectFilter = "*.SmokeTests.csproj"
	$nServiceBusEndpointProjectFilter = ".Endpoints."
	$webApplicationProjectFilter = ".Web"
	$nugetProjectFilter = "*"	
}

function DeleteLongPaths($dir)
{
    $tempFolder = "%TEMP%\DelFolder_LongPath"
    Write-Host "Deleting folder $($dir)"
    cmd /c "`""MD `"$tempFolder`""`""
    cmd /c "`""RoboCopy `"$tempFolder`" `"$dir`" /MIR"`""
    cmd /c "`""RD `"$tempFolder`""`""
    cmd /c "`""RD `"$dir`""`""
}

Task Default -depends Compile, UnitTest, IntegrationTest, Package, AcceptanceTest, SmokeTest

# Resolves the sln path by finding the first available sln file or using the property passed in
# Sets defaults for script properties - since they may be passed in as blank by Go pipeline
Task Init {
	# Build.ps1 may not be in same directory that we're executing build for
	Set-Location ([Environment]::CurrentDirectory)	

	If ($sln -eq "") {
		$sln = "*.sln"
	}

	$script:slns = @()

	Get-ChildItem '.\' -Filter $sln | ForEach-Object { $script:slns += $_.FullName }
	Write-Host "sln = $script:slns"

	$script:major = $major
	if ($major -eq "") {
		$script:major = '1'
	}

	$script:minor = $minor
	if ($minor -eq "") {
		$script:minor = '0'
	}

	$script:build = $build
	if ($build -eq "") {
		$script:build = '0'
	}

	$script:revision = $revision
	if ($revision -eq "") {
		$script:revision = '0'
	}
	Write-Host "version = $script:major.$script:minor.$script:build.$script:revision"

	$script:platform = $platform
	If ($platform -eq "") {
		$script:platform = "Any CPU"
	}
	Write-Host "platform = $script:platform"

	$script:config = $config
	If ($config -eq "") {
		$script:config = "Release" 
	}
	Write-Host "config = $script:config"

	$script:enableCodeAnalysis = $enableCodeAnalysis
	If ($enableCodeAnalysis -eq "") {
		$script:enableCodeAnalysis = $true
	}
	Write-Host "enableCodeAnalysis = $script:enableCodeAnalysis"
	
	$script:testResults = $testResults
	If ($testResults -eq "") {
		$script:testResults = "Build\Output\TestResults"
	}
	Write-Host "testResults = $script:testResults"

	$script:packages = $packages
	if ($packages -eq "") {
		$script:packages = "Build\Output\Deploy"
	}
	Write-Host "packages = $script:packages"
	
	$script:unitTestsProjectFilter = $unitTestsProjectFilter
	If ($unitTestsProjectFilter -eq "") {
		$script:unitTestsProjectFilter = "*.UnitTests.csproj"
	}
	Write-Host "unitTestsProjectFilter = $script:unitTestsProjectFilter"
	
	$script:integrationTestsProjectFilter = $integrationTestsProjectFilter
	if ($integrationTestsProjectFilter -eq "") {
		$script:integrationTestsProjectFilter = "*.IntegrationTests.csproj"
	}
	Write-Host "integrationTestsProjectFilter = $script:integrationTestsProjectFilter"

	$script:acceptanceTestsProjectFilter = $acceptanceTestsProjectFilter
	If ($acceptanceTestsProjectFilter -eq "") {
		$script:acceptanceTestsProjectFilter = "*.AcceptanceTests.csproj"
	}
	Write-Host "acceptanceTestsProjectFilter = $script:acceptanceTestsProjectFilter"
	
	$script:smokeTestsProjectFilter = $smokeTestsProjectFilter
	If ($smokeTestsProjectFilter -eq "") {
		$script:smokeTestsProjectFilter = "*.SmokeTests.csproj"
	}
	Write-Host "smokeTestsProjectFilter = $script:smokeTestsProjectFilter"
	
	$script:nServiceBusEndpointProjectFilter = $nServiceBusEndpointProjectFilter
	If ($nServiceBusEndpointProjectFilter -eq "") {
		$script:nServiceBusEndpointProjectFilter = ".Endpoints."
	}
	Write-Host "nServiceBusEndpointProjectFilter = $script:nServiceBusEndpointProjectFilter"
	
	$script:webApplicationProjectFilter = $webApplicationProjectFilter
	if ($webApplicationProjectFilter -eq "") {
		$script:webApplicationProjectFilter = ".Web"
	}
	Write-Host "webApplicationProjectFilter = $script:webApplicationProjectFilter"

	$script:nugetProjectFilter = $nugetProjectFilter
	if ($nugetProjectFilter -eq "") {
		$script:nugetProjectFilter = "*"
	}
	Write-Host "nugetProjectFilter = $script:nugetProjectFilter"	
	
	Copy-Item "$PSScriptRoot\Nuget.Config" ".nuget\Nuget.Config" -Force
}

# Compiles the sln
Task Compile -depends Init, Clean, Version {
	foreach($sln in $script:slns) {        
		nuget.exe restore $sln
		
		exec { msbuild $sln "/p:TreatWarningsAsErrors=true" "/p:VisualStudioVersion=14.0" "/p:Configuration=$script:config" "/p:Platform=$script:platform" /target:Build }
		
		If ($script:enableCodeAnalysis -eq $true) {
			WriteColoredOutput "Running ReSharper code inspection for $sln" -foregroundcolor Cyan
			
			If (!(Test-Path ($script:testResults))) {
				New-Item $script:testResults -type directory
			}
				
			$resharper = 'ReSharper'		
			$output = "$(Resolve-Path $script:testResults)\InspectCode.xml"

			If (!(Test-Path "$env:temp\$resharper")) {
				New-Item "$env:temp\$resharper" -type directory
			}
			
			$location = Get-Location
			Set-Location "$env:temp\$resharper"

			Install-NuGetPackage JetBrains.ReSharper.CommandLineTools -Version 9.2.20150819.165728

			# Paths may have gotten too long
			Get-ChildItem -Path ".\" -Recurse -Filter "node_modules" -EA SilentlyContinue | Where-Object {$_.Attributes -eq "Directory"} | % {
					DeleteLongPaths $_.FullName				
			}

			Try {
				Copy-Item "$PSScriptRoot\Default.DotSettings" "$sln.DotSettings" -Force

				If (Test-Path $output) {
					Remove-Item -Force $output
				}

				$inspectCode = (Get-ChildItem '.\' -Recurse -Filter 'InspectCode.exe' | Select-Object -First 1)

				$platform = $script:platform -replace ' ', ''
				exec { & "$($inspectCode.FullName)" "$sln" "/properties:TreatWarningsAsErrors=true;Configuration=$script:config;Platform=$platform" /output:"$output"}
				
				If (Select-Xml -Path "$output" -XPath "/Report/IssueTypes/IssueType[@Severity!='SUGGESTION' and @Category!='XML Errors']") {
					$err = "$sln has ReSharper code inspection issues. Check the contents of $output"
					throw $err
				}
			}
			Finally {
				Set-Location $location
			}
		}		
	}	
}

# Cleans the sln
Task Clean -depends Init  {
	foreach($sln in $script:slns) {
		exec { msbuild $sln "/p:Configuration=$script:config" "/p:Platform=$script:platform" /target:Clean }
	}
	If (Test-Path '.\packages') { 
		Try {
			Remove-Item '.\packages' -Recurse -Force 
		}
		Catch {
			Write-Warning 'Could not clear NuGet packages directory.'
		}
	};
	Get-ChildItem -Path ".\" -Recurse -Filter "node_modules" -EA SilentlyContinue | Where-Object {$_.Attributes -eq "Directory"} | % {
			DeleteLongPaths $_.FullName
	}
}

# Applies versioning to AssemblyInfo file
Task Version -depends Init {
	$path = Get-Location
	Update-AssemblyInfoFiles ".\" "$script:major.$script:minor.$script:build.$script:revision"
}

# Runs all NUnit unit test projects
Task UnitTest -depends Init {	
	If (!(Test-Path ($script:testResults))) {
		New-Item $script:testResults -type directory
	}

	$nunit = Get-NUnitRunner

	Get-ChildItem -Path ".\" -Recurse -Filter $script:unitTestsProjectFilter -EA SilentlyContinue | % {
		$dll = Join-Path $_.Directory.FullName "bin\$script:config\$($_.Name.Replace(".csproj", ".dll"))"
		WriteColoredOutput "Running NUnit tests for $dll" -foregroundcolor Cyan
		$xml = "$script:testResults\" + ($_.Name -replace '.csproj', '.xml')
		exec { & $nunit /xml:$xml /nologo $dll}
	}
}

# Runs all NUnit integration test projects
Task IntegrationTest -depends Init {	
	If (!(Test-Path ($script:testResults))) {
		New-Item $script:testResults -type directory
	}

	$nunit = Get-NUnitRunner

	Get-ChildItem -Path ".\" -Recurse -Filter $script:integrationTestsProjectFilter -EA SilentlyContinue | % {
		$dll = Join-Path $_.Directory.FullName "bin\$script:config\$($_.Name.Replace(".csproj", ".dll"))"
		WriteColoredOutput "Running NUnit tests for $dll" -foregroundcolor Cyan
		$xml = "$script:testResults\" + ($_.Name -replace '.csproj', '.xml')
		exec { & $nunit /xml:$xml /nologo $dll}
	}
}

# Runs all SpecFlow acceptance test projects
Task AcceptanceTest -depends Init {
	Run-SpecFlowTests $script:testResults $script:acceptanceTestsProjectFilter
}

# Runs all SpecFlow smoke test projects
Task SmokeTest -depends Init {
	Run-SpecFlowTests $script:testResults $script:smokeTestsProjectFilter
}

# Packages all NServiceBus and Web projects
Task Package -depends Init {
	If (Test-Path ($script:packages)) {
		Remove-Item -Recurse -Force $script:packages
	}
	
	If (Test-Path -Path $script:packages) {
		Remove-Item -Recurse -Force $script:packages
	}

	New-Item $script:packages -type directory

	$hasPackages = $false

	Get-ChildItem -Path ".\" -Recurse -Filter "*.csproj" -EA SilentlyContinue | % {
	
		Write-Host  $_.Directory.FullName
		$projectDirectory = $_.Directory.FullName
		
		$binFolder = (Join-Path $projectDirectory "\bin")
		If (!(Test-Path -Path $binFolder)) {
			Compile
		}
		
		$project = Get-ChildItem -Path "$projectDirectory\*" -Filter "*.csproj" -EA SilentlyContinue | Select -First 1
		Write-Host  $project
		
		 If (!($project -eq $null)) {
			 $consoleApp = $projectDirectory + "\program.cs"
			 If ((Test-Path $consoleApp) -or ($project -match $script:nServiceBusEndpointProjectFilter)) {
				 WriteColoredOutput "Packaging $($project.Directory.Name)" -foregroundcolor Cyan
				 Create-ConsoleAppPackage $project.Directory.FullName $script:config (Join-Path $script:packages $project.Directory.Name)
				 $hasPackages = $true
			 } ElseIf ($project -match $script:webApplicationProjectFilter) {
				WriteColoredOutput "Packaging $($project.Directory.Name)" -foregroundcolor Cyan
				Create-WebApplicationPackage $project.Directory.FullName $project.FullName (Join-Path $script:packages $project.Directory.Name)
				$hasPackages = $true
			}
		 }	
	}

	Get-ChildItem -Path ".\" -Recurse -Filter "*.nuspec" -EA SilentlyContinue | % {
	
		$projectDirectory = $_.Directory.FullName
		
		$project = Get-ChildItem -Path "$projectDirectory\*" -Filter "*.csproj"  -EA SilentlyContinue | Select -First 1
		
		If (!($project -eq $null)) {
			If ($script:nugetProjectFilter -eq "*" -or $project -match $script:nugetProjectFilter) {
				If (!(Test-Path (Join-Path $packages "NuGetPackages"))) {
					New-Item (Join-Path $packages "NuGetPackages") -type directory
				}
				WriteColoredOutput "Packaging $($project.Name)" -foregroundcolor Cyan
				nuget.exe pack "$project" -IncludeReferencedProjects -Build -OutputDirectory "$packages\NuGetPackages" -Prop Configuration="$script:config"	
				$hasPackages = $true
			}
		}	
	}

	If (!$hasPackages) {
		# Build process requires at least 1 item in packages directory
		Set-Content (Join-Path "$script:packages" "No Deployment Packages") ""
	}
}

# Functions
# ---------

function Install-NuGetPackage([string] $name) {
	Write-Host "Installing $name NuGet package."
	nuget.exe install $name -OutputDirectory '.\packages'
	Write-Host "Installed $name NuGet package."
}

function Get-NUnitRunner {
	$nunit = $null
	
	Get-ChildItem -Path ".\packages\" -Recurse -Filter nunit-console-x86.exe | Foreach-Object {$nunit = $_.FullName}	
	
	If ($nunit -eq $null) {
		Install-NuGetPackage 'NUnit.Runners'
		Get-ChildItem -Path ".\packages\" -Recurse -Filter nunit-console-x86.exe | Foreach-Object {$nunit = $_.FullName}	
	}
	
	Write-Host $nunit
	Return $nunit
}

function Run-SpecFlowTests ([string] $resultsDirectory, [string] $projectFilter) {	
	If (!(Test-Path ($resultsDirectory))) {
		New-Item $resultsDirectory -type directory
	}
	
	$nunit = Get-NUnitRunner
	
	$specflow = $null
	Get-ChildItem -Path ".\packages\" -Recurse -Filter specflow.exe | Foreach-Object {$specflow = $_.FullName}	
	If ($specflow -eq $null) {
		Install-NuGetPackage 'SpecFlow'
		Install-NuGetPackage 'SpecFlow.NUnit'
		Get-ChildItem -Path ".\packages\" -Recurse -Filter specflow.exe | Foreach-Object {$specflow = $_.FullName}	
	}

	Get-ChildItem -Path ".\" -Recurse -Filter $projectFilter  -EA SilentlyContinue | % {						
		$name = $_.Name

		# http://stackoverflow.com/questions/11363202/specflow-fails-when-trying-to-generate-test-execution-report
		Out-File -FilePath "$specflow.config" -Encoding utf8 -InputObject @' 
<?xml version="1.0" encoding="utf-8" ?> 
<configuration> 
<startup> 
    <supportedRuntime version="v4.0.30319" /> 
</startup> 
</configuration> 
'@  
		$errorsRunningNUnit = $false
		$errorsRunningSpecFlow = $false
		$xml = "$resultsDirectory\" + ($_.Name -replace '.csproj', '.xml')
		$html = "$resultsDirectory\" + ($_.Name -replace '.csproj', '.html')
		$dll = Join-Path $_.Directory.FullName "bin\$script:config\$($_.Name.Replace(".csproj", ".dll"))"			
		$csproj = $_.FullName
		WriteColoredOutput "Running NUnit tests for $dll" -foregroundcolor Cyan
		
		$nunitFailed = $false
		$err = ""
		Try
		{
			exec { & $nunit $dll /xml:$xml /nologo}
		} 
		Catch
		{
			$err = "An error occurred running NUnit tests for $dll."
		} 
		Finally
		{
			kill -erroraction 'silentlycontinue' -processname iexplore, IEDriverServer
		}

		WriteColoredOutput "Running SpecFlow tests for $csproj" -foregroundcolor Cyan
		
		Try
		{
			exec {& $specflow nunitexecutionreport $csproj /xmlTestResult:$xml /out:$html }
		} 
		Catch
		{
			$errSpecFlow = "An error occurred generating the SpecFlow report for $name."
			If ($err -ne "") {
				$err += $errSpecFlow
			} Else {
				$err = $errSpecFlow
			} 
		} 
		Finally
		{
			kill -erroraction 'silentlycontinue' -processname iexplore, IEDriverServer
		}
		
		If (Test-Path $html) {
			# Put in default location for build server
			Copy-Item $html (Join-Path $resultsDirectory "SpecFlow.html") -Force
		}

		If ($err -ne "") {
			throw $err
		}
	}
}

# https://gist.github.com/toddb/1133511
function Update-AssemblyInfoFiles ([string] $path = "", [string] $version, [System.Array] $excludes = $null, $make_writeable = $false) {

	If ($version -notmatch "[0-9]+(\.([0-9]+|\*)){1,3}") {
		$err = "Version number incorrect format: $version."
		throw $err
	}
	
	$versionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$versionAssembly = 'AssemblyVersion("' + $version + '")';
	$versionFilePattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$versionAssemblyFile = 'AssemblyFileVersion("' + $version + '")';
 
	Get-ChildItem -Path $path -Recurse -Filter AssemblyInfo.cs  -EA SilentlyContinue | % {
		$filename = $_.FullName
		
		$update_assembly_and_file = $true
		
		# set an exclude flag where only AssemblyFileVersion is set
		If ($excludes -ne $null)
			{ $excludes | % { If ($filename -match $_) { $update_assembly_and_file = $false	} } }
		
 
		# We are using a source control (TFS) that requires to check-out files before 
		# modifying them. We don't want checkins so we'll just toggle
		# the file as writeable/readable	
	
		If ($make_writable) { Writeable-AssemblyInfoFile($filename) }
 
		# see http://stackoverflow.com/questions/3057673/powershell-locking-file
		# I am getting really funky locking issues. 
		# The code block below should be:
		#     (Get-Content $filename) | % {$_ -replace $versionPattern, $version } | set-content $filename
 
		$tmp = ($file + ".tmp")
		If (Test-Path ($tmp)) { Remove-Item $tmp }
 
		If ($update_assembly_and_file) {
			(Get-Content $filename) | % {$_ -replace $versionFilePattern, $versionAssemblyFile } | % {$_ -replace $versionPattern, $versionAssembly } | Out-File -Encoding utf8 $tmp
			Write-Host Updating file AssemblyInfo and AssemblyFileInfo: $filename --> $versionAssembly / $versionAssemblyFile
		} Else {
			(Get-Content $filename) | % {$_ -replace $versionFilePattern, $versionAssemblyFile } | Out-File -Encoding utf8 $tmp
			Write-Host Updating file AssemblyInfo only: $filename --> $versionAssemblyFile
		}
 
		If ((Test-Path ($filename)) -and (Get-Content ($filename)) -ne (Get-Content ($tmp))) {
			Remove-Item $filename
			Move-Item $tmp $filename -force	
		} Else {
			Remove-Item $tmp
		}
 
		If ($make_writable) { ReadOnly-AssemblyInfoFile($filename) }		
 
	}
}

