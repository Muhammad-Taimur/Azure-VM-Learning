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
#enter ssh-keygen and press enter
#press enter to all settings. now your key is saved in c:\Users\.ssh\id_rsa.pub
#To check the ssh in cmd type   TYPE id_rsa
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

#Get the Public IP address of the VM created.
$MyIP = New-AzureRmPublicIPAddress `
-ResourceGroupName $rg.ResourceGroupName `
-Name $pip.Name | Select-Object -ExpandProperty IpAddress

#Connect to Our Vm  via SSH
ssh -l demoadmin $MyIP


#Lets Create Windows VM with little less code using POWERSHELL SPLATTING.

#Craete Windows Creds Objects this will use for WINDOWS username/password
$WindowsPassword = ConvertTo-SecureString "Password1234$%^" -AsPlainText -Force
$WindowsCred = New-Object System.Management.Automation.PSCredential ("demoadmin", $WindowsPassword)

#We are using the image name parameter for the list of images.
New-AzureRmvm -Image

$vmParams = @{
    ResourceGroupName = 'RG02'
    Name = 'psdemo-win-2'
    Location = 'centralus'
    Size = 'Standard_B1ms'
    Image = 'Win2016Datacenter'
    PublicIpAddressName = 'psdesmo-win-2-pip-2'
    Credential = $WindowsCred
    VirtualNetworkName = 'psdemo-vnet-2'
    SubnetName = 'psdemo-subnet-2'
    SecurityGroupName = 'psdemo-win-nsg-2'
    OpenPorts = 3389
}

#This command will crete the VM and get all the properties of vm what is written inside vmParams  
New-AzureRmVM @vmParams

#Get the Public IP address of the VM created.
Get-AzureRmPublicIPAddress `
-ResourceGroupName $rg.ResourceGroupName `
-Name 'psdesmo-win-2-pip-2' | Select-Object -ExpandProperty IpAddress


Get-AzureRmVM -Name 'psdemo-win-' -ResourceGroupName RG02 -Status

Get-AzureRmvm -Name 'psdemo-win-2' -ResourceGroupName RG02 -Status

Get-AzureRmVm -name 'psdemo-win-2' -ResourceGroupName RG02 -Status | select Statuses | select displayStatus

#Get all the VMs in resourcegroup
get-azurermvm | select name , location | Format-List

#This is the command to check the Stauts of VM is it Running or stopped.
((Get-AzureRmVM -ResourceGroupName "RG02" -Name "psdemo-win-2" -Status).Statuses[1]).code

#Stop Azure VM Command

Stop-AzureRmVM -ResourceGroupName "RG02" -Name "psdemo-win-2" -Force


get-azurermvm -ResourceGroupName "RG02" | foreach {start-azurermvm -Name $_.Name -ResourceGroupName "RG02" }

get-azurermvm -ResourceGroupName "RG02" | foreach {stop-azurermvm -Name $_.Name -ResourceGroupName "RG02" -Confirm:$false -Force }


#VM SECRETES 
#vmwindows  Taimur1.
#ssh-public-key 
#ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQCnqT3Nz1cuV4a/4nt5ww4yfqp/HKrtKiPDp+ybZ3/qfWlmCCNFB7SGZBL5biPPNmQuWCEufIFsS7EbevaIM9iRGvTpGG5qok7AcCgAyum2WFuFYnTiA/44YuCkNJeLKt6CptxADdDMmCgxgKxyJht6bVCU4STl7Ywr+UfTBHhGORupz347Tt4ckOzTw1CEy1LnOgARBDv49Kvam9B94G8HDr1rrkuhty7Mr7WZquF5PhYuGmENV9DbgiVodOESubKgfnRj29O7FaGzJgbsYvHOvUnT9jDC8s43ArpHjYiR/W9r+V1RDubB14gd3bwzWRPiQYdQr09FPVcahv+S/CXt taimur@DESKTOP-QIE9VOO

#gMeIBQO80l6jJtvBZ8lpDZQqexmwN/zgP9lbboTF7oQ taimur@DESKTOP-QIE9VOO




#-----BEGIN RSA PRIVATE KEY-----
#MIIEowIBAAKCAQEAsC52F+N4YbWbo8+2/XOFqLRz8D3uSv3UhcbRucplQnh/EZdn
#kk6MHTtChoTKMKpmc7cgNnIE9BuEHCVoSIWEIKpLHCc/XkRHvNFHIJh7VHsteB/C
#biuNX47B4ytp0DgCET1nIJa9PI1yGK8gx+gFlhzbXt4D4WWyabVYF949xo/Y3WdK
#FF0oEn0GLF1fDNBtkUlSJlY+72YsgBOlZiQ/U9pAcg6WVe+ijtmhp2ifjwXIgf6d
#AXbFH+6p/wKeLETFyn7OAO/N53oqCAnSdji2pyoa4E9QGdalbxY4d64qSzbcRwKm
#5FYTM1jnpIZXeBYztS0cJxe482eOb7JM46LOKQIDAQABAoIBAQCetdwQfQwPCWjh
#0tbHz2+SoKzouQGXcL4onQiFU/yQOrhNgpT8yeGS27V8NNdnq7mLeGZ+ZYxs0vTZ
#3iDpY825F29+NyTwqJXvVJ+8j2BapHQ7iHDAil9au+GR4aP6vNmv6h6izug+SjWE
#Yw8mxq9xoSFFfr8EJ3bnn0NyjPQdkCvRTyx4qqiBUJvf7p0vLP669iNXzYptxYyf
#5TPIN0nZDyC7qji762oitIJfXTeWoPQb/d9aIFvNpyjAdUA5v8Pzq5U96Javb7/M
#F71yjANfpVGiCk3lH1TzPd8SxuzkGdjKb9/iL9HFVve6Nk29lQ3LZLRzqpVwxSgP
#41eFqhEBAoGBAOi/y9V6P6Fzn/22fqqoYOHUS5+F7ZkMzHdI93G9GPxxHxS+x/86
#VJfxXgfYh+1kyIlEKgZio/5QavH8SmojrGRX8c032QB5lX2W8CCEnUPOrBh3/ckr
#BMH/jS7EecNP6UnjM6tc6z6GdSVsKxhHZaTk7Wqyvb7fL9EySDhWr7LRAoGBAMHI
#CFaR/AK9Yqbk0EcT8BsKdSZZ4ZMcyJSTKNj3k5CWtG7j9VZFwuzt2NMAxY5SnZFm
#SYMiMZ45S8nyX0GgpetmxpVuq+vlLhI/X/yaYfxbPiG8IyXAr6iBHoxhnW0eA+5H
#/BGgPSu9s8UPP8yIfEQf/3LoXbfyOmOs+nyUgUvZAoGAflAwoC+TPtzQVFHpVlbB
#FW4wiGeXtbsTcB1CZRC58a62rnyHb8VJSZitblaeFkDe8Ff08rgvxgIAuEkyXX30
#vhRYXwZTF4Xkkl8K/Krb6oPMNA9SxQ06rMoy5dGtP0ksE3RhgzuPU8SG6QNWM/vz
#dtTi4EgW3/KiMcc3GJQ7EDECgYASJiPxx7Zso0QsEV4YahugzLfwIZbo6lc24xl+
#SKG/dv3rLNp7fAknm5clG/tkuwQa7BOSfo9bHE6m2VZmlR81DukmcbkUXOCVwO3C
#gMsQkZMeIbrA/Gz3QTCVQUc3QwpnNMK8+97+y8OcfzMget/4mW6ZWn38jmk9kKPd
#KyN48QKBgAlpt3jW5DJNG5KnyTkN1GNxRXP4kj6oEf/OPDv/fXdF/TP8OxpdT2/t
#gI7pz8JDjAcqbteAcEDmxW3G8QlumZwcJqcnU7AyZstUI8JU9XhSRX4LqQFFAFIx
#iWbe3qqzPcp7SBZyKZ+wfdWlR1IVMiTSJWSkyW7VkwNF4gIP0Niz
#-----END RSA PRIVATE KEY-----

