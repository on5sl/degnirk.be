[T4Scaffolding.Scaffolder(Description = "Enter a description of MacroPartial here")][CmdletBinding()]
param(
    [parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ViewName,        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$outputPath = "Views\MacroPartials\"+$ViewName  # The filename extension will be added based on the template's <#@ Output Extension="..." #> directive
$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

Add-ProjectItemViaTemplate $outputPath -Template MacroPartialTemplate `
	-Model @{ Namespace = $namespace; ViewName = $ViewName;} `
	-SuccessMessage "Added MacroPartial output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force