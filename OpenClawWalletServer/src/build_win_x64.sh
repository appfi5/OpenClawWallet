WebUIDir=Configuration-UI
hostDir=wwwroot

echo $WebUIDir

cd $WebUIDir
npm install 
npm run build

cd ..

if [ ! -d "$hostDir" ];
then
  mkdir -p $hostDir
  echo "Directory created: $hostDir"
fi

cp -R $WebUIDir/dist/* $hostDir

dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true -o ./output