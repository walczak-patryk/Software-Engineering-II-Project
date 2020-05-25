$folderName = "_Debug"

Remove-Item $folderName -Recurse -ErrorAction Ignore
New-Item -ItemType Directory -Force -Path ".\$folderName"

Copy-Item  -Force -Recurse '.\The Game\GameMaster\bin\Debug\netcoreapp3.1\'	-Destination ".\$folderName\GameMaster"
Copy-Item  -Force -Recurse '.\The Game\CommunicationServer\bin\Debug\netcoreapp3.1\'	-Destination ".\$folderName\CommunicationServer"
Copy-Item  -Force -Recurse '.\The Game\GameServices\GameServices\bin\Debug\netcoreapp3.1\'	-Destination ".\$folderName\GamePlayer"
Copy-Item  -Force -Recurse '.\The Game\GameGraphicalInterface\bin\Debug\netcoreapp3.1\'	-Destination ".\$folderName\GameGraphicalInterface"

Read-Host "Done"