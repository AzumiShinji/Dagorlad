sc query "Service_of_Dagorlad" | findstr /i "RUNNING" 1>nul 2>&1 && (
    echo serviceName is running.
    net stop "Service_of_Dagorlad"
) || (
    echo serviceName is not running 
)