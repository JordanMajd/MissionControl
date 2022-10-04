dotnet build

$pluginPath = "C:\Program Files (x86)\Steam\steamapps\common\Mars First Logistics Demo\BepInEx\plugins\MissionControl"

if (!(Test-Path $pluginPath)) {
      New-Item $pluginPath -ItemType Directory
}

Copy-Item -Force ".\bin\Debug\net6.0\MissionControl.dll" -Destination $pluginPath
