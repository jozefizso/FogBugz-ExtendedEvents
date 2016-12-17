using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Extended Events")]
[assembly: AssemblyDescription("Show events from external sources like Subversion, TeamCity and Jenkins.")]
[assembly: AssemblyCompany("Jozef Izso")]
[assembly: AssemblyProduct("FBExtendedEvents")]
[assembly: AssemblyCopyright("Copyright © 2015-2016 Jozef Izso")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: Guid("dc011599-24b0-4a15-acc5-bc3fa9a446d8")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyVersion("1.6.1.*")]
[assembly: AssemblyFileVersion("1.6.1.0")]
