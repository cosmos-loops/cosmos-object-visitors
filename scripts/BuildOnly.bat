﻿@echo off

echo =======================================================================
echo Cosmos.ObjectVisitors (Build Only)
echo =======================================================================

::go to parent folder
cd ..

::create nuget_packages
if not exist nuget_packages (
    md nuget_packages
    echo Created nuget_packages folder.
)

::clear nuget_packages
for /R "nuget_packages" %%s in (*) do (
    del "%%s"
)
echo Cleaned up all nuget packages.
echo.

::start to package all projects

::Cosmos-object-visitors
dotnet pack src/Cosmos.Extensions.ObjectVisitors -c Release -o nuget_packages --no-restore

for /R "nuget_packages" %%s in (*symbols.nupkg) do (
    del "%%s"
)

::get back to build folder
cd scripts