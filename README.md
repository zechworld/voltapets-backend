# Backend para el proyecto Volta Pets

## Tecnologías usadas
+ .NET Core 3.1 - Framework del lenguaje C# para construir la API de la aplicación
+ Cloudinary - Hosting de imágenes web para albergar el registro de imágenes realizadas por los usuarios de la aplicación
+ Driver PostgreSQL - Driver necesario para generar la conexión con la base de datos realizada con PostgreSQL
+ Entity Framework - Manejo de entidades para generación de contextos y facilitar la integración entre los modelos


## Instalación
Para instalar la API del proyecto primero se tener instalado el gestor de paquetes nativo de C# llamado`Nuget`.
#
1. En primer lugar, se debe estar en la carpeta donde se encuentre el el archivo `VoltaPetsAPI.sln`. Luego, se deberán utilizar dos comandos para poder instalar las dependencias utilizadas en el proyecto:

```csharp
nuget restore // Instalará las dependencias necesarias
dotnet restore // Sincronizará el proyecto con las dependencias instaladas
```
#
2. En segundo lugar, se deberá configurar el archivo `appsettings.json`. Este archivo queda ignorado por defecto en el archivo `gitignore`por lo que se deberá realizar una copia del archivo `appsettings.json.dist` y quitar la extensión `.dist`

#
3. Dentro del archvio `appsettings.json` se establecen todas las variables de entorno que permiten la configuración con los distintos servicios utilizados. 

#
4. Para configurar la conexión con la base de datos, no es necesario utilizar una instancia local de Postgre dado que la API está conectada con la base de datos albergada en **Google Cloud**. En caso de utilizar localhost se debe cambiar el `connection string`en caso de utilizar una base de datos local, cambiar los siguientes parámetros:

+ **Server:** Dirección IP en donde está albergado el servidor de la API. En caso de usar localhost esté quedaría con la dirección `127.0.0.1`.

+ **Database:** Nombre de la base de datos.

+ **User id:** Nombre de usuario (en local usualmente es postgre).

+ **Password:** Contraseña de la base de datos local

#
5. Finalmente, se puede iniciar la aplicación utilizando el botón con ícono de play o utilizando el comando:

```csharp
dotnet run
```
