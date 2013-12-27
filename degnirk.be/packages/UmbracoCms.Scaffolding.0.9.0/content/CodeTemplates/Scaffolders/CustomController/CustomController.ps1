[T4Scaffolding.Scaffolder(Description = "Enter a description of W00tNewAndNice here")][CmdletBinding()]
param(
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ControllerName,        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $falseS
)

$outputPath = "Controllers\CustomControllers\"+$ControllerName+"Controller"  # The filename extension will be added based on the template's <#@ Output Extension="..." #> directive
$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value

Add-ProjectItemViaTemplate $outputPath -Template CustomControllerTemplate `
	-Model @{ Namespace = $namespace; ControllerName = $ControllerName; ExampleValue = "Hello, world!" } `
	-SuccessMessage "Added CustomController output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force