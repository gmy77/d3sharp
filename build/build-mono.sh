#!/bin/bash
# Replaces mixed-mode SQLite path with managed path for Mono Linux/MacOS users

SCRIPT=`readlink -f $0`
SCRIPTPATH=`dirname $SCRIPT`
echo $SCRIPTPATH

sed -e 's/dep\\sqlite\\sqlite-mixed/dep\\sqlite\\sqlite-managed/g' $SCRIPTPATH/../src/Mooege/Mooege-VS2010.csproj > $SCRIPTPATH/../src/Mooege/Mooege-Mono.csproj

xbuild $SCRIPTPATH/Mooege-Mono.sln

