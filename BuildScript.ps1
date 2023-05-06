clear
cd ./LoggerMachine/
echo '======================'
echo 'Building [1/5]: LoggerMachine'
echo '======================'

dotnet publish -c Release

clear
echo '======================'
echo 'Build done'
echo '======================'
sleep 5

cd ..
cd ./LoginService/
clear

echo '======================'
echo 'Building [2/5]: LoginService'
echo '======================'

docker build --rm -t prod/loginservice:testing .

clear
echo '======================'
echo 'Build done'
echo '======================'
sleep 5

cd ..
cd ./MicroServiceProxy/
clear

echo '======================'
echo 'Building [3/5]: ProxyService'
echo '======================'

docker build --rm -t prod/proxyservice:testing .

clear
echo '======================'
echo 'Build done'
echo '======================'
sleep 5

cd ..
cd ./MessageService/
clear

echo '======================'
echo 'Building [4/5]: MessageService'
echo '======================'

docker build --rm -t prod/messageservice:testing .

clear
echo '======================'
echo 'Build done'
echo '======================'
sleep 5

cd ..
cd ./DatabaseService/
clear

echo '======================'
echo 'Building [5/5]: DatabaseService'
echo '======================'

docker build --rm -t prod/databaseservice:testing .

clear
echo '======================'
echo 'Build done'
echo '======================'
sleep 5

cd ..
clear
echo '======================'
echo 'Done building stuff'
echo '======================'

docker image ls