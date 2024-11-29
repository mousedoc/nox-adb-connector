# nox-adb-connector
Easy connect for Nox App Player ADB ('nox_adb')

## Features
- Run `nox_adb connect` with various ports
- (Not include `nox_adb reverse`) 

## How to use
- First, Change your Nox App Player settings to usb debugging ON
- Execute [nox-adb-connector.exe](https://github.com/mousedoc/nox-adb-connector/releases)

## How to build
[Runtime identifier](https://docs.microsoft.com/ko-kr/dotnet/core/rid-catalog)
```
# -r == Runtime identifier 
dotnet publish -r win-x64 -c release -p:PublishSingleFile=true --no-self-contained
```


## Reference
- https://www.bignox.com/blog/how-to-connect-android-studio-with-nox-app-player-for-android-development-and-debug/
