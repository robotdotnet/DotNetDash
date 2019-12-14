using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8907117d-e11d-4f1d-8a26-7d0f1d8aceab")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: InternalsVisibleTo("DotNetDash.Test")]

[assembly: XmlnsDefinition("https://robotdotnet.github.io/dotnetdash", nameof(DotNetDash))]
[assembly: XmlnsPrefix("https://robotdotnet.github.io/dotnetdash", "dash")]
