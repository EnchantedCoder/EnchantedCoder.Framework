xcopy Framework\Havit.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.pdb \\topol.havit.local\library\framework /y
xcopy Framework\Havit.xml \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Business.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Business.pdb \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Business.xml \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Data.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Data.pdb \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Data.SqlServer.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Data.SqlServer.xml \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Data.xml \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Drawing.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Drawing.pdb \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Drawing.xml \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Enterprise.Web.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Enterprise.Web.pdb \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Enterprise.Web.xml \\topol.havit.local\library\framework /y
xcopy Framework\Havit.PayMuzo.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.PayMuzo.pdb \\topol.havit.local\library\framework /y
xcopy Framework\Havit.PayMuzo.xml \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Services.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Services.pdb \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Services.xml \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Web.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Web.pdb \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Web.xml \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Xml.dll \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Xml.pdb \\topol.havit.local\library\framework /y
xcopy Framework\Havit.Xml.xml \\topol.havit.local\library\framework /y

xcopy NuGet\Havit.Data.Glimpse.*.nupkg \\topol.havit.local\Library\NuGet\packages /y

#xcopy Framework\*.dll \\topol.havit.local\library\framework /y
#xcopy Framework\*.pdb \\topol.havit.local\library\framework /y
#xcopy Framework\*.xml \\topol.havit.local\library\framework /y

xcopy Documentation \\topol.havit.local\Inetpub\havit.local\hfw\Documentation /e /y

$null = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")