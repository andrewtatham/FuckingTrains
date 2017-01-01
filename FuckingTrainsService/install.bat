sc stop FuckingTrainsService 
sc delete FuckingTrainsService 
sc create FuckingTrainsService start=auto binpath="D:\Users\Andrew\Documents\FuckingTrains\FuckingTrainsService\bin\Debug\FuckingTrainsService.exe"
sc start FuckingTrainsService
timeout 2
sc query FuckingTrainsService