#!/bin/bash

# Replaces mixed-mode SQLite path with managed path for Mono Linux/MacOS users
# args: file_in file_out

if [ $# -ne 2 ]; then
    echo "error: requires input and output file paths"
    exit
fi

file_in=$1
file_out=$2

sed -e 's/dep\\sqlite\\sqlite-mixed/dep\\sqlite\\sqlite-managed/g' $file_in > $file_out
