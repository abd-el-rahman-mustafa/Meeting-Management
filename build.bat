@echo off
echo =============================
echo  Building Angular...
echo =============================
cd Client
call npm run build
cd ..

echo =============================
echo  Publishing .NET Core API...
echo =============================
cd API
call dotnet publish -c Release -o ../publish
cd ..

echo =============================
echo  Done! Output is in /publish
echo =============================
pause