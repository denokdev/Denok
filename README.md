# Denok

A self hosted Link Shortener built with `dotnet`

[<img src="./assets/denok_arch.png" width="400">](https://github.com/wuriyanto48/kece)
<br/><br/>

### Requirements
- Dotnet https://dotnet.microsoft.com/en-us/download
- Docker and `docker-compose` https://www.docker.com/community/open-source

### Preparation
You need at least three domains, or three sub domains to run the two `Denok` components.
First domain for `Web Dashboard`, second domain for `Redirector`, 
third domain to catch error when `Redirector` cannot find unique URL code in database. 

Two domain must be in the `environment variable`. Open the `.env.development` file, and adjust with yours.

For example:
```
DOMAIN_NAME=https://link.mydom.com
DOMAIN_NOT_FOUND=http://help.mydom.com
```

### Build
For build and run, simply `run`
```shell
$ docker-compose up -d
```