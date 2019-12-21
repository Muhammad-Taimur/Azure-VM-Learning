#Setup 
#1. Logged into Azure CLI with az login
#2 ensure that you are using Powershell terminal

#Demo Outline
#1. Create a Linux VM,Specifying individual resource, Connect via SSH.
#2. Create a Linux VM,using a quick short configuration.
#3. Create a Windows VM, Specifying individual resource, Connect via RDP.

#I install Azure CLI using below command line.
#Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'


#Now here we start with the AZURE Creating VMs.

#First thing is to login in Azure portal, and the in below command specify the subscription.

#Install Azure CLI in cpmputer by using below commands:
#Install-Module Azure -AllowClobber
#Install-module AzureRM

#Start the connection with Azure.
#Connect-AzureRmAccount -Subscription 'Free Trail' 
#runas.exe /user:Administrator "Install-Module Az"
#Install-Module AzureRM -Scope Requires -RunAsAdministrator

#This command login your account from Azure. (open us a browser with Azure poral Signin Page)

Login-AzureRmAccount

#Create RM Group and store it in 1 variable. 
 #New-AzureRmResourceGroup -Name RG02 -Location "South Central US"

#Display RM gorup Created and store that in variable.
$rg = Get-AzureRmResourceGroup `
-Name 'RG02' `
-Location 'Central US'

$rg
#Most important powershell in using ( `) if you want to change the line of the same command like shows below.

#Create a Subnet Configuration and save that in a variable.
$subnetConfig = New-AzureRmVirtualNetworkSubnetConfig `
-Name 'psdemo-subnet-2' `
-AddressPrefix '10.2.1.0/24'

$subnetConfig | format-list

#Create an Virtual Nework
$vnet = New-AzureRmVirtualNetwork `
-ResourceGroupName $rg.ResourceGroupName `
-Location $rg.Location `
-Name 'psdemo-vnet-2' `
-AddressPrefix '10.2.0.0/16' `
-Subnet $subnetConfig

$vnet | Format-List

#Public IP Address
$pip = New-AzureRmPublicIPAddress `
-ResourceGroupName $rg.ResourceGroupName `
-Location $rg.Location `
-Name 'psdemo-linux-2-pip-1' `
-AllocationMethod Static

$pip | Format-List

#Create a netowrk security group rule for SSH
#Example of a more gragular approach to crete a security ruel.

$rule1 = New-AzureRmNetworkSecurityRuleConfig `
-Name ssh-rule `
-Description 'Allow SSh' `
-Access Allow `
-Protocol TCP `
-Direction Inbound `
-Priority 100 `
-SourceAddressPrefix Internet `
-SourcePortRange * `
-DestinationAddressPrefix * `
-DestinationPortRange 22

$rule1 | Format-List

#Creating Azure Security Group, with the new created rule.

$nsg = New-AzureRmNetworkSecurityGroup `
-ResourceGroupName $rg.ResourceGroupName `
-Location $rg.Location `
-Name 'psdemo-linux-nsg-2' `
-SecurityRules $rule1

$nsg | Format-List

#This command to show more abnout nsg in detail.
$nsg | more

#Creating a virtula netowrk card and associate with the public IP Address and NSG
#First lets get an object representing our current subnet.

#$_. is using for an action which is performing in each object in pipline. 
$subnet = $vnet.Subnets | Where-Object { $_.Name -eq 'psdemo-subnet-2' }

#display the value of subnet for testting purpose
echo $subnet

$nic = New-AzureRmNetworkInterface `
-ResourceGroupName $rg.ResourceGroupName `
-Location $rg.Location`
-Name 'psdemo-lin-2-nic-1' `
-Subnet $subnet `
-PublicIpAddress $pip `
-NetworkSecurityGroup $nsg

$nic | format-list

#Create a Virtula Machine

$LinuxVMConfig = New-AzureRmVMConfig `
-VMName 'psdemo-linux-2' `
-VMSize 'Standard_D1_v2'

$LinuxVMConfig | format-list

#Set the ComputerName, OS type and  auth methods.
#$password = ConvertTo-SecureString 'password123412123$%^*' -AsPlainText -Force
#$LinuxCred = New-Object System.Management.Automation.PSCredential ('demoadmin', $password)

$password = ConvertTo-SecureString "Password" -AsPlainText -Force
$Credential = New-Object System.Management.Automation.PSCredential ("demoadmin", $password); 


$LinuxVMConfig = Set-AzureRmVMOperatingSystem `
-VM $LinuxVMConfig `
-Linux `
-ComputerName 'psdemo-linux-2' `
-Credential $Credential `
-DisablePasswordAuthentication ` 
#-Credential $Credential 

#Read SSH key which is our system and append/Add this to VM.
#Note: If the SSh key is not generated in system below code will give error.

#To generate SSH do below:
#open command prompt (cmd)
#enter ssh-keygen szand press enter
#press enter to all settings. now your key is saved in c:\Users\.ssh\id_rsa.pub
#Open your git client and set it to use open SSH

#echo $sshpublickey
Add-AzureRmVMSSHPublickey `
-VM $LinuxVMConfig `
-KeyData 'ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQCnqT3Nz1cuV4a/4nt5ww4yfqp/HKrtKiPDp+ybZ3/qfWlmCCNFB7SGZBL5biPPNmQuWCEufIFsS7EbevaIM9iRGvTpGG5qok7AcCgAyum2WFuFYnTiA/44YuCkNJeLKt6CptxADdDMmCgxgKxyJht6bVCU4STl7Ywr+UfTBHhGORupz347Tt4ckOzTw1CEy1LnOgARBDv49Kvam9B94G8HDr1rrkuhty7Mr7WZquF5PhYuGmENV9DbgiVodOESubKgfnRj29O7FaGzJgbsYvHOvUnT9jDC8s43ArpHjYiR/W9r+V1RDubB14gd3bwzWRPiQYdQr09FPVcahv+S/CXt taimur@DESKTOP-QIE9VOO' `
-Path "/home/public/.ssh/authorized_keys"
#-Path "users/demoadmin/.ssh/authorized_keys"

$sshpublickey

#Get the VM image and set it in VM config file. in this case we are using RHEL Lates which is SKUS 8
Get-AzureRMVMImageSku -Location $rg.Location -PublisherName "Redhat" -Offer "rhel"

$LinuxVMConfig = Set-AzureRmVMSourceImage `
-VM $LinuxVMConfig `
-PublisherName 'Redhat' `
-Offer 'Rhel' `
-Skus '7.4' `
-Version 'latest'

$LinuxVMConfig

#Assign the Created netowrk interface NIC to the VM
$LinuxVMConfig = Add-azureRMVMNetworkInterface `
-VM $LinuxVMConfig `
-Id $nic.id

New-AzureRmVM `
-ResourceGroupName $rg.ResourceGroupName `
-Location $rg.Location `
-VM $LinuxVMConfig


$LinuxVMConfig




#New-AzureRmVM -ResourceGroupName $rg.ResourceGroupName -Location $rg.Location -VM $LinuxVMConfig -Verbose
#Get-AzureRMComputeResourceSku | where {$_.Locations -icontains "centralus"}
#$vm = Set-AzureRmVMBootDiagnostics -VM $LinuxVMConfig -enable -ResourceGroupName $rg.ResourceGroupName -StorageAccountName "freerg02psde122117480"