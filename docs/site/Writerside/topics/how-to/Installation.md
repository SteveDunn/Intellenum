# Installing Intellenum

<note>
These tutorials assume a working knowledge of .NET and C#, so won't include the basics necessary to start the
tutorials, e.g. things like creating new projects, creating new types, compiling, viewing error output, etc.
</note>

<tabs>
<tab title=".NET CLI">
<code xml:lang="bash">dotnet add package Intellenum --version 1.0.0</code>
</tab>
<tab title="Package Manger">
<code xml:lang="bash">NuGet\Install-Package Intellenum -Version 1.0.0</code>
</tab>
<tab title="Package Reference">
<code-block>
<![CDATA[
    <PackageReference Include="Intellenum" Version="1.0.0" />
]]>
</code-block>
</tab>
</tabs>

<note>
Change `1.0.0` for the <a href="https://www.nuget.org/packages/Intellenum">latest version listed on NuGet</a>
</note>


When added to your project, the **source generator** generates the wrappers for your enums and the **code analyzer**
will let you know if you try to create invalid items.

## .NET Framework support

If you have a .NET Framework project and are using the old style projects (the one before the 'SDK style' projects), 
then you'll need to do a few things differently:

add the reference using PackageReference in the `.csproj` file:
```xml
  <ItemGroup>
      <PackageReference Include="Intellenum" 
            Version="[LATEST_VERSION_HERE - E.G. 1.0.0]" 
            PrivateAssets="all" />
  </ItemGroup>
```
Next, set the language version to latest (or anything 8 or more):
```xml
  <PropertyGroup>
+    <LangVersion>latest</LangVersion>
    <Configuration Condition=
      " '$(Configuration)' == '' ">Debug</Configuration>
```
