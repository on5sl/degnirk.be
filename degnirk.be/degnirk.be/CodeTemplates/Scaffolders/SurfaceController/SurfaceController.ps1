[T4Scaffolding.Scaffolder(Description = "Enter a description here")][CmdletBinding()]
param(
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$Model,
	[string[]]$Properties,        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$outputPath = "Controllers\SurfaceControllers\"+$Model+"SurfaceController"  # The filename extension will be added based on the template's <#@ Output Extension="..." #> directive
$outputViewPath = "Views\"+$Model+"Surface\"+$Model  # The filename extension will be added based on the template's <#@ Output Extension="..." #> directive
$namespace = (Get-Project $Project).Properties.Item("DefaultNamespace").Value
$modelName = $Model + "Model"
$foundModelType = Get-ProjectType $modelName -Project $Project -BlockUi -ErrorAction SilentlyContinue


if(!$foundModelType){ Scaffold Type $modelName $Properties Models }

$foundModelType = Get-ProjectType $modelName -Project $Project
if(!$foundModelType){ return }	

$modelTypeNameSpace = [T4Scaffolding.NameSpaces]::GetNameSpace($foundModelType.FullName)
Add-ProjectItemViaTemplate $outputPath -Template SurfaceControllerTemplate `
	-Model @{ 
			Namespace = $namespace; 
			ControllerName = $Model; 
			ModelNameSpace = $modelTypeNameSpace;  
			ModelType = [MarshalByRefObject]$foundModelType;
			ExampleValue = "Hello, world!" } `
	-SuccessMessage "Added SurfaceController output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force


Add-ProjectItemViaTemplate $outputViewPath -Template SurfaceControllerViewTemplate `
	-Model @{ 
			Namespace = $namespace; 
			ModelName = $Model;
			ModelNamespace = $modelTypeNameSpace;  
			ModelType = [MarshalByRefObject]$foundModelType;
			ExampleValue = "Hello, world!" } `
	-SuccessMessage "Added View output at {0}" `
	-TemplateFolders $TemplateFolders -Project $Project -CodeLanguage $CodeLanguage -Force:$Force