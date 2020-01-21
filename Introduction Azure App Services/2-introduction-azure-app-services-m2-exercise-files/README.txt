To make the solution work, you have to provide some of your own credentials:

In ColorWebsite / Web.config
-fill the value of PluralsightColorWebsite_db with the connectionstring to a SQL database. This can be a local or cloud based SQL database

In ColorWebsite.Console / App.config
-In AppSetting cert: Fill in your Azure management cert key, which you can find when you go to https://manage.windowsazure.com/publishsettings/
 See http://blog.waardedoorit.nl/how-to-use-the-microsoft-azure-management-libraries for more info
-In AppSetting subscriptionId: Fill in your Azure subcription ID, which you can find in the Azure Portal
-Don't worry about this project. You don't need it to run the Website with


In ColorWebsite.WebJob / App.config
-fill the value of PluralsightColorWebsite_db with the connectionstring to a SQL database. This can be a local or cloud based SQL database