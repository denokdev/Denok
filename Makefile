.PHONY : build build-local build-osx build-linux clean

include .env
$(eval export $(cat .env | sed 's/#.*//g' | xargs))


clean:
	rm -rf denokweb && \
	rm -rf denokred

build-osx:
	rm -rf app/ && mkdir app && dotnet restore && dotnet publish Denok.Web/Denok.Web.csproj -c Release -r osx-x64 --self-contained true -o ./app/

build-linux:
	rm -rf app/ && mkdir app && dotnet restore && dotnet publish Denok.Web/Denok.Web.csproj -c Release -r linux-x64 --self-contained true -o ./app/

build-local:
	rm -rf denokweb/ && mkdir denokweb && \
	dotnet restore && \
	dotnet publish Denok.Web/Denok.Web.csproj -c Release -o ./denokweb/ && cp .env denokweb/.env && \
	rm -rf denokred/ && mkdir denokred && \
	dotnet publish Denok.Redirector/Denok.Redirector.csproj -c Release -o ./denokred/ && cp .env denokred/.env

build:
	dotnet publish Denok.Web/Denok.Web.csproj -c Release -o ./denokweb/ && \
	dotnet publish Denok.Redirector/Denok.Redirector.csproj -c Release -o ./denokred/
