# DotNetDash
DotNetDash is RobotDotNet's SmartDashboard replacement. It is designed from the start to be easily extensible.

It is currently under heavy development, and may have breaking changes.


[![Build status](https://ci.appveyor.com/api/projects/status/0g63crtkakhifcde/branch/master?svg=true)](https://ci.appveyor.com/project/robotdotnet/dotnetdash/branch/master)

## Supported Operating Systems
The current implementation is based on WPF, so it is Windows only.

In the future, cross platform support will be added by switching from WPF to [Avalonia](https://github.com/AvaloniaUI/Avalonia).

## Extensibility
### XAML Plugins
You can write plugins using just XAML Markup if you only need basic data-binding capabilities.  For example, a Digital Input display can be coded as follows:
```xaml
<dash:XamlView
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:dash="https://robotdotnet.github.io/dotnetdash"
            DashboardType="Digital Input">
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="{Binding Name}" />
        <CheckBox IsChecked="{Binding Booleans[Value]}" IsEnabled="false" />
    </StackPanel>
</dash:XamlView>
```

You must set the `DashboardType` to the SmartDashboard type of the sendable.

The `Name` property binds to the `Name` property on the table. For all other table entries, you bind to them by `DataType[Key_Name]`. So, for a boolean Value key, you would bind to `Booleans[Value]` as above.

### C# Plugins
For anything more advanced, you need to create a C# plugin. There will be more documentation on this in the future, but some basic documentation is below. You can see examples of this here with the LiveWindow and CANSpeedController support.

To start off, you will need to do the following:

1. Create a subclass of `TableProcessor`. This class will create the view and the backing context for DotNetDash. Below are some properties and methods you must provide:
  * `GetViewCore`: Create an instance of your view.
  * `Name`: Specifies a user-friendly name for your processor (used when changing the processor in use).
  * (optional) `GetTableContext`: Create an instance of the backing context (must derive from `NetworkTableContext`). Only use this if you need a custom context.
2. Create an implementation of `ITableProcessorFactory` that provides your `TableProcessor` for a given SmartDashboard type.
