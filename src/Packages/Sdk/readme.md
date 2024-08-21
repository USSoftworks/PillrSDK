# The PillrSDK 

## Installation
The package can found [here](https://www.nuget.org/packages/USSoftworks.Pillr).
The recommended method for installing the ***PillrSDK*** is to use the top-level [`<Sdk/>`](https://learn.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk?view=vs-2022#use-the-top-level-sdk-element) element:
```xml
<Project>
  <Sdk Name="Pillr.Sdk" Version="0.0.0" />
</Project>
```
***PillrSDK*** can (and should) be utilized alongside other target-oriented SDKs such as `Microft.NET.Sdk` or `Microsoft.NET.Razor`.
