sc query "Service_of_Dagorlad" | findstr /i "RUNNING" 1>nul 2>&1 && (
    echo serviceName is running.
    net stop "Service_of_Dagorlad"
) || (
    echo serviceName is not running 
    net start "Service_of_Dagorlad"  
    "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\SvcUtil.exe" net.tcp://localhost:9002/Dagorlad  /out:./../../../Dagorlad/classes/ServiceProxy.cs
)
PAUSE