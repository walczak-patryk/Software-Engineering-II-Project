$folderName = "_Release"
$local = Get-Location

Remove-Item $folderName -Recurse -ErrorAction Ignore
New-Item -ItemType Directory -Force -Path "$local\$folderName"

cd -Path "$local\The Game\CommunicationServer"
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true --output "$local/$folderName" /p:PublishTrimmed=true

cd -Path "$local\The Game\GameServices\GameServices"
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true --output "$local/$folderName" /p:PublishTrimmed=true

cd -Path "$local\The Game\GameMaster"
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true --output "$local/$folderName" /p:PublishTrimmed=true

cd -Path "$local\The Game\GameGraphicalInterface"
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true --output "$local/$folderName" /p:PublishTrimmed=true

foreach($file in Get-ChildItem "$local\$folderName")
{
    if ($file -Like "*.pdb" )
    {
		Remove-item $file.FullName
    }
}

Read-Host "Done"