#Generalizing and creting a custom Iamge using powershell
#setup pre-stage the RDP connection for windows VM 'psdemo-win-2'

Login-AzureRmAccount

#Start Connection with Azure.
Connect-AzureRmAccount -Subscription "Free Trail"

#This is the command to execute in VM's CMD to install the sysprep.
%WINDIR%\system32\sysprep\sysprep.exe /generalize /shutdown /oobe

#Let's get the status of the VM and make sure the VM is shutdown.
Get-AzureRmVm `
-ResourceGroupName 'RG02' `
-Name 'psdemo-win-2' `
-Status `

#Find out Resource Group
$rg = Get-AzureRmResourceGroup `
-Name 'RG02' `
-Location 'centralus'

#Find out Vm name inside Resource Group
$vm = Get-AzureRmVM `
-ResourceGroupName $rg.ResourceGroupName `
-Name 'psdemo-win-2'

#Stop TheA VM in azure
Stop-AzureRmVm `
-ResourceGroupName $rg.ResourceGroupName `
-Name $vm.Name `
-Force

#Let's get the status of the VM and make sure the VM is shutdown.
Get-AzureRmVm `
-ResourceGroupName $rg.ResourceGroupName `
-Name $vm.Name `
-Status

#Mark the VM as Generalized
Set-AzureRmVm `
-ResourceGroupName $rg.ResourceGroupName `
-Name $vm.Name `
-Generalized


#Start an Image Configuration from our source Vm.
$Image = New-AzurermImageConfig `
-Location $rg.Location `
-SourceVirtualMachineId $vm.Id

#Create Vm from Custom Image config we just creted, simply  specify the image config as a Azure Image.
New-AzureRmImage `
-ResourceGroupName $rg.ResourceGroupName `
-Image $Image `
-ImageName "psdemo-win-ci-1"


#Summart Image information
Get-AzureRmImage `
-ResourceGroupName $rg.ResourceGroupName

#Create user Object that will be used for windows username/password.
$password = ConvertTo-SecureString 'password1234$%^' -AsPlainText -Force
$WindowsCred = New-Object System.Management.Automation.PSCredential ('demoadmin', $password)


#Let's create a VM from our new image, we'll use a more terse definition for this VM creation
New-AzureRMvm `
-ResourceGroupName $rg.ResourceGroupName `
-Name  'psdemo-win-1c' `
-ImageName 'psdemo-win-ci-1' `
-Location 'centralus' `
-Credential $WindowsCred `
-VirtualNetworkName  'psdemo-vnet-2' `
-SubnetName  'psdemo-subnet-2' `
-SecurityGroupName 'psdemo-win-nsg-2' `
-OpenPorts 3389

#Checkorut the status of our provisioned Vm
Get-AzureRmVM `
-ResourceGroupName $rg.resourceGroupName `
-Name 'psdemo-win-1c'

#You can remove the deallocated azure Vm
Remove-AzureRmvm `
-ResourceGroupName $rg.ResourceGroupName `
-Name 'psdemo-win-2' `
-Force

#And still leaves the image in our Resource Group
Get-AzureRMVm `
-ResourceGroupName $rg.ResourceGroupName `
-ImageName 'psdemo-win-c1-1'

