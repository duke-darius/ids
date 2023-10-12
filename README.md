# iceni-driving-school
A simple driving school pupil management dashboard

### how to run

#### API:
Ensure the 'appsettings.json' file is appropriately configured.
- ConnectionStrings:Ctx -> a MSSQL server constr
- Jwt:Secret -> the super secret string acting as the JWT key
- Jwt:Audience -> Intended audience for JWT
- Jwt:Issuer -> Issuer of JWT

```shell
cd Iceni.Api
dotnet run
# OR
dotnet watch
```


#### UI:
Ensure 'appsettings.json' is configured.
Only need 1 setting: BaseApiUrl -> the localhost url that the API is running on

```shell
cd Iceni.Admin.Wasm
dotnet run
# OR
dotnet watch
```