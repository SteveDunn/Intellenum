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
