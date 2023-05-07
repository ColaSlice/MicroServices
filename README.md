# MicroServices
A collection of services working together to create a microservice

~~Right now, there is some funky stuff going on with compiling the services, that I'm still working with.
So right now, I would recommend just starting them from your IDE of choice.
For these services to work, you need to have both the LoginService And the MicroServiceProxy running
at the same time. Then make the API calls to the MicroServiceProxy.~~

For building the microservice for docker, run the bash/powershell script called BuildScript.* .
To run the microservice, run:
- docker run --rm --name=login-service --net=data-network -p 5001:5001 prod/loginservice:testing
- docker run --rm --name=proxy-service --net=data-network -p 5002:5002 prod/proxyservice:testing
- docker run --rm --name=message-service --net=data-network -p 5003:5003 prod/messageservice:testing
- docker run --rm --name=database-service --net=data-network -p 5004:5004 prod/databaseservice:testing

Or, for most ease of use, you can just create a Systemd service (or any other init system service. Just make sure dotnet and aspnet runtime is supported)

(This is an example with the microservice running on the local machine)
To get a list of endpoints for the proxy service, make a GET request to this URL: 
- http://localhost:5002/api/SystemInfo/getendpoints
