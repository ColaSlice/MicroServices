#/bin/bash
clear
cd LoggerMachine
echo '======================'
echo 'Building LoggerMachine'
echo '======================'

dotnet publish -c Release

clear
echo '======================'
echo 'Build done'
echo '======================'
sleep 5

cd ..
cd LoginService
clear

echo '======================'
echo 'Building LoginService'
echo '======================'

docker build --rm -t prod/loginservice:testing .

clear
echo '======================'
echo 'Build done'
echo '======================'
sleep 5

cd ..
cd MicroServiceProxy
clear

echo '======================'
echo 'Building ProxyService'
echo '======================'

docker build --rm -t prod/proxyservice:testing .

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

