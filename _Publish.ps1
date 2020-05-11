New-Item -ItemType Directory -Force -Path ".\Exe"
cp '.\The Game\GameMaster\bin\Debug\netcoreapp3.1\GameMaster.exe'								'.\Exe\GameMaster.exe'
cp '.\The Game\CommunicationServer\bin\Debug\netcoreapp3.1\CommunicationServer.exe'				'.\Exe\CommunicationServer.exe'
cp '.\The Game\GameServices\GameServices\bin\Debug\netcoreapp3.1\GamePlayer.exe'				'.\Exe\GamePlayer.exe'