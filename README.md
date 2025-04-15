Lancement du service b avec Dapr 
 cd .\ServiceB\ 
dapr run --app-id service-b --app-port 5266 --app-protocol http --dapr-http-port 3501 -- dotnet run

Lancement du service a avec Dapr 
 cd .\ServiceA\ 
 dapr run --app-id service-a --app-protocol http --dapr-http-port 3502 -- dotnet run
