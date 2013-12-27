param($rootPath, $toolsPath, $package, $project)

# Ensure that scaffolding works
if (-not (Get-Command Invoke-Scaffolder)) { return }

# The reason is issue (http://nuget.codeplex.com/workitem/595). Now we need to remove dummy file.
if ($project) { $projectName = $project.Name }
Get-ProjectItem "NuGetDymmy.txt" -Project $projectName | %{ $_.Delete() }

Set-DefaultScaffolder -Name Type -Scaffolder TypeScaffolding.Type -SolutionWide -DoNotOverwriteExistingSetting

$project.Object.References.Add("System.ComponentModel.DataAnnotations")

# TODO: modify ".cs" when decide to support VB too
# $lang = Get-ProjectLanguage
# if ($lang -eq $null) { $lang = "cs" }

Write-Host " "
Write-Host "TypeScaffolding installed with dependencies and DataAnnotations reference."
Write-Host " "
Write-Host "You can generate your types using the following simple commands (without '//' comments):"
Write-Host "PM> Scaffold Type MyModel // simplest MyModel.cs with Id, Name and annotations"
Write-Host "PM> Scaffold Type MyModel -Folder Models -Force -NoAnnotations // simplest .\Models\MyModel.cs"
Write-Host " "
Write-Host "Also you can use more comprehensive commands (just take a look at the output):"
Write-Host "PM> Scaffold Type MyModel Id,Name,Test?,ParentId,TestName[99],Count?,CreatedDate Models -Force"
Write-Host "PM> Scaffold Type MyModel Id:long,Name:string[40],Count:long? Models -Force"
Write-Host " "
Write-Host "Visit the http://typescaffolding.codeplex.com/ to learn about output and conventions customization."