

# Authenticate the PowerShell session with the Azure API
Login-AzAccount


# Create an Azure Resource Group in the Appropriate Region
New-AzResourceGroup -Name 'PSBatch' -Location 'West Europe' -Force


# Create a new Batch Account
New-AzBatchAccount –AccountName 'BATCH_ACCOUNT_NAME' –Location 'West Europe' –ResourceGroupName 'PSBatch'


# Retrieve the Batch Account authentication credentials
$context = Get-AzBatchAccountKey -AccountName 'BATCH_ACCOUNT_NAME'


# Create a Cloud Service Configuration
$configuration = New-Object  -TypeName "Microsoft.Azure.Commands.Batch.Models.PSCloudServiceConfiguration"  -ArgumentList @(4,"*")


# Create a Pool
New-AzBatchPool -Id 'RenderPool' -VirtualMachineSize "Small" -CloudServiceConfiguration $configuration -TargetDedicatedComputeNodes 1 -BatchContext $context


# Create a Low Priority Pool
New-AzBatchPool -Id 'LowPriorityRenderPool' -VirtualMachineSize "Small" -CloudServiceConfiguration $configuration -TargetLowPriorityComputeNodes 1 -BatchContext $context




