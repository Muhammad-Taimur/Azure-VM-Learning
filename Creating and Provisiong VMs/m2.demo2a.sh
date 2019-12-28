#Setup 
#1. Logged into Azure CLI with az login
#2 ensure that you are using bash terminal

#Demo Outline
#1. Create a Linux VM,Specifying individual resource, Connect via SSH.
#2. Create a Linux VM,using a quick short configuration.
#3. Create a Windows VM, Specifying individual resource, Connect via RDP.

#I install Azure CLI using below command line.

#Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'

#Now here we start with the AZURE Creating VMs.

#First thing is to login in Azure portal, and the in below command specify the subscription.


az login

az login --subscription "Free Trail"

