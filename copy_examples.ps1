$assetsPath = "C:\Program Files (x86)\Steam\steamapps\common\Mars First Logistics Demo\Mars First Logistics_Data"
Copy-Item -Force ".\Examples\example-pack.json" -Destination $assetsPath
Copy-Item -Force ".\Examples\examplebundle" -Destination $assetsPath