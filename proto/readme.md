# proto

Community-driven Google Protobuf definitions for a game client/server.

## Known Issues

* In Java, you will need to reference the class structure, as the class wrappers needed to be renamed or prefixed with 'C'.  
* In C#, a couple class definitions may be conflicting and need to be changed manually.

## Building

On Windows run `generate.bat`, on Linux/Mac OS X run `generate.sh`.