sc stop TrainCommuteCheckService 
sc delete TrainCommuteCheckService 
sc create TrainCommuteCheckService start=auto binpath="D:\Users\Andrew\Documents\TrainCommuteCheck\TrainCommuteCheckService\bin\Debug\TrainCommuteCheckService.exe"
sc start TrainCommuteCheckService
timeout 2
sc query TrainCommuteCheckService