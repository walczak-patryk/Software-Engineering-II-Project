$folderName = "_Debug"
Start-Process -FilePath ".\$folderName\CommunicationServer\CommunicationServer.exe"
Start-Process -FilePath ".\$folderName\GameMaster\GameMaster.exe"
Start-Process -FilePath ".\$folderName\GamePlayer\GamePlayer.exe"
Start-Process -FilePath ".\$folderName\GamePlayer\GamePlayer.exe"

Read-Host "Done"