# Radio Systems Azure Authentication Module

Radio Systems Azure Authentication is a module that enables Azure Authentication to Orchard using Azure Active Directory and OpenID connect. The module overrides the default login and integrates into the standard pipeline for Orchard.

## How to use

1. If you don't have an Azure Active Directory, set one up by following "insert link". Create an application in Azure that will correspond to your Orchard instance. The settings you will need to configure Orchard Azure Active Directory Authentication will be on the configure tab of the created application.
2. Before enabling the module, your must add an admin user to Orchard via the Orchard admin. Ensure that the user name matches the Azure Active Directory user name for the user you wish to have admin privileges.
3. After enabling the module, before navigating away from the admin, go to site settings under the **Azure** group. Fill in the information from your Azure Active Directory
4. You may have to restart your Orchard site for the new settings to take effect.
5. Navigate to the Orchard home screen. Clear site cookies, and watch Azure Authentication take over!

## Installing via Source Code

If you are including the module form source, there are some steps that have to be followed in order for the module to run. 

1. First install referenced NuGet packages for the module.
2. Then, modify the Orchard.Web projects web.config file with these lines
```xml
    <dependentAssembly>
        <assemblyIdentity name="Microsoft.Owin" publicKeyToken="31bf3856ad364e35" culture="neutral" />
            <bindingRedirect oldVersion="0.0.0.0-3.0.1.0" newVersion="3.0.1.0" />
    </dependentAssembly>
    <dependentAssembly>
        <assemblyIdentity name="Microsoft.Owin.Host.SystemWeb" 
	      publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-3.0.1.0" newVersion="3.0.1.0" />
    </dependentAssembly>
    <dependentAssembly>
        <assemblyIdentity name="Microsoft.IdentityModel.Protocol.Extensions" 
            publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-1.0.2.33" newVersion="1.0.2.33" />
    </dependentAssembly>
    <dependentAssembly>
        <assemblyIdentity name="System.IdentityModel.Tokens.Jwt" 
            publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.20622.1351" newVersion="4.0.20622.1351" />
    </dependentAssembly>
    <dependentAssembly>
        <assemblyIdentity name="Microsoft.IdentityModel.Protocol.Extensions" 
            publicKeyToken="31bf3856ad364e35"      
            culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-1.0.2.33" newVersion="1.0.2.33" />
    </dependentAssembly>
    <dependentAssembly>
        <!-- 2.0.0.0 by System.Data.SqlServerCe, Version=4.0.0.0 -->
        <assemblyIdentity name="System.Transactions" 
             publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.0.0" newVersion="4.0.0.0" />
    </dependentAssembly>
    <dependentAssembly>
        <!-- 2.0.0.0 by System.Data.SqlServerCe, Version=4.0.0.0 -->
        <assemblyIdentity name="System.Data" 
            publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.0.0" newVersion="4.0.0.0" />
    </dependentAssembly>
```
3. In the assemblies section of the Orchard.Web web.config, modify the entry for Microsoft.Owin.Host.SystemWeb
```xml
    <add assembly="Microsoft.Owin.Host.SystemWeb, **Version=3.0.1.0**" />
```
4. These modifications should ensure that the module runs correctly.

## Issues

Please open an issue in the Github issue tracker. We will try to respond within a few days concerning the issue.

## Contributions

Contributions are gladly accepted.  Please submit a pull request on the develop branch, coding guidelines follow the [guidelines](http://docs.orchardproject.net/Documentation/Code-conventions) as outlined by the [Orchard Project](http://www.orchardproject.net/). 

## License

This module is licensed under the Apache 2.0 license, a copy is included in the repository.
