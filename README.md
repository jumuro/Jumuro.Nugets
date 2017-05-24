# Paquetes NuGet .NET

### Jumuro.Security.Cryptography

Este paquete contiene la interfaz `IHashProvider` que contiene la firma de distintos métodos para generar el hash de una cadena de caracteres usando diferentes algoritmos. También contiene la clase `HashProvider` que implementa la interfaz. Para hacernos una idea a continuación se muestra el código de la interfaz:

Además el paquete tiene una clase de extensiones, `CryptographyExtensions`, con dos métodos para transformar un array de bytes en array de caracteres, `ToHexString()` y `ToBase64String()`. 

### Jumuro.WebApi.Extensions.ActionResults

Este paquete contiene una clase de extensiones, `ApiControllerExtensions`, aplicada sobre objetos de tipo `ApiController` para generar objetos personalizados que implementan la interfaz `IHttpActionResult` que es lo que devuelven las acciones de nuestros controladores de Web API 2. Por ejemplo, entre otras se ha creado una sobrecarga del método `Ok<T>()` que genera una respuesta `Http` con `HttpStatusCode 200` (Ok), un objeto de tipo `T` en el *content* y además añade una cabecera con un mensaje de texto.
 
---
 
# Paquetes NuGet AngularJS

### Jumuro.Angular.CrudREST

Contiene una implementación de las llamadas CRUD (*Create*, *Read*, *Update* y *Delete*) de Http:

 - *Create*: `restPost()`
 - *Read*: `restGet()`
 - *Update*: `restPut()`
 - *Delete*: `restDelete()`

### Jumuro.Angular.Grid

Implementa una directiva AngularJS para crear un grid con bastante funcionalidad extra.

### Jumuro.Angular.HttpInterceptor

Contiene un servicio AngularJS para procesar peticiones y respuestas Http.

### Jumuro.Angular.Modal

Implementa un servicio AngularJS para mostrar ventanas modales de confirmación, notificación y error.

### Jumuro.Angular.OAuth

Dada la relación directa con la parte central de este proyecto, merece la pena que veamos éste paquete con más detalle.

Implementa dos servicios, el primero se llama `oAuthService` y se utiliza para gestionar la solicitud de tokens de acceso mediante el envío de las credenciales de usuario y mediante el envío del token de refresco. 

Como vemos en el bloque de código de más abajo, el método `logIn()` es el encargado de obtener el token de acceso mediante el envío de las credenciales de usuario. Hace una petición a la ruta `/token` de nuestro servidor enviando `grant_type=password` y las credenciales del usuario: `username`, `password` y `client_id`. Una respuesta correcta traerá consigo los tokens de acceso y de refresco, dicha respuesta se guardará en una cookie.

El método `refreshToken()` es el encargado de obtener el token de acceso mediante el envío del token de refresco. Hace una petición a la ruta `/token` de nuestro servidor enviando `grant_type=refresh_token`, el `refresh_token` y el  `client_id`. Igual que antes, una respuesta correcta traerá consigo los tokens de acceso y de refresco que se guardarán en la cookie.

El método `logOut()` simplemente borra la cookie que contiene los tokens.

``` JS
'use strict';

angular
    .module('jumuro.oAuth')
    .service('oAuthService', oAuthService);

oAuthService.$inject = ['$http', '$q', '$injector', 'ipCookie', 'oAuthConstants', 'oAuthAppConfigConstants', '$location'];

function oAuthService($http, $q, $injector, ipCookie, oAuthConstants, oAuthAppConfigConstants, $location) {

    var service = {
        refreshToken: refreshToken,
        logOut: logOut,
        logIn: logIn,
        hasCookie: hasCookie,
        getUserInfo: getUserInfo,
        isAuthenticated: isAuthenticated
    };

    return service;

    function refreshToken() {
        var deferred = $q.defer();

        //get the cookie
        var authData = ipCookie(oAuthConstants.oAuthCookieName);

        if (authData) {
            var data = "grant_type=refresh_token&refresh_token=" + authData.refresh_token + "&client_id=" + authData.client_id;

            $http
                .post(oAuthAppConfigConstants.appConfig.oAuthURL, data, {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                })
                .success(function (response) {
                    ipCookie(oAuthConstants.oAuthCookieName, response, { path: oAuthConstants.appPathName });
                    deferred.resolve(response);
                })
                .error(function (err, status) {
                    logOut();
                    deferred.reject(err);
                });
        }

        return deferred.promise;
    };

    function logIn(postData) {
        var deferred = $q.defer();

        var data = "grant_type=password&username=" + postData.username + "&password=" + postData.password + "&client_id=" + oAuthAppConfigConstants.appConfig.oAuthClientId;

        $http
            .post(oAuthAppConfigConstants.appConfig.oAuthURL, data, {
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            })
            .success(function (data, status, headers, config) {
                // Create cookie
                ipCookie(oAuthConstants.oAuthCookieName, data, { path: oAuthConstants.appPathName });
                // Store user in a local storage
                localStorage.setItem("login-info", JSON.stringify({ username: postData.username }));

                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.reject(data);
            });

        return deferred.promise;
    };

    function getUserInfo() {
        var userInfo = JSON.parse(localStorage.getItem("login-info"));

        return userInfo;
    }

    function hasCookie() {
        return ipCookie(oAuthConstants.oAuthCookieName);
    }

    function logOut() {
        // Delete current cookie if it already exsits
        ipCookie.remove(oAuthConstants.oAuthCookieName, { path: oAuthConstants.appPathName });

        localStorage.removeItem("login-info");

        $location.path('/login');
    };

    function isAuthenticated() {
        var ok = ipCookie(oAuthConstants.oAuthCookieName);

        return ok;
    };
}
```


El segundo servicio incluido en este paquete, `oAuthHttpInterceptor`, es un interceptor Http y se encarga de añadir el token de acceso a la cabecera de cada petición Http y de gestionar los
errores que tienen que ver con el proceso de autorización, como por ejemplo el *Http Status Code* 401, *Unauthorized*.

Como vemos en el bloque de código de más abajo, el método `request()` lee los datos almacenados por el servicio anterior en la cookie y añade la cabecera de autorización `'Bearer [access_token]'` a la petición HTTP.

El método `responseError()` procesa una eventual respuesta HTTP errónea. El código de error que más nos interesa es el 401 – *Unauthorized*. Previsiblemente, este error se producirá cuando el token de acceso haya expirado, en cuyo caso se leen los datos de la cookie y se realiza una llamada al método `refreshToken()` del servicio `oAuthService`. Además, si la petición de refresco del token es satisfactoria, se vuelve a realizar la llamada que originó el error 401, consiguiendo así que la renovación del token de acceso mediante el token de refresco sea totalmente transparente al usuario.

``` JS
'use strict';

angular
    .module('jumuro.oAuth')
    .factory('oAuthHttpInterceptor', oAuthHttpInterceptor);

oAuthHttpInterceptor.$inject = ['$q', '$injector', 'ipCookie', 'oAuthConstants', 'toaster', '$location'];

function oAuthHttpInterceptor($q, $injector, ipCookie, oAuthConstants, toaster, $location) {
    var service = {
        request: request,
        responseError: responseError
    };

    return service;

    function request(config) {
        if ($location.path() !== '/login') {
            config.headers = config.headers || {};

            //get the cookie
            var authData = ipCookie(oAuthConstants.oAuthCookieName);

            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.access_token;
            }
            else {
                var authService = $injector.get('oAuthService');
                authService.logOut();
            }
        }

        return config;
    }

    function responseError(rejection) {
        if (rejection.status === 400) {
            var toaster = toaster || $injector.get('toaster');
            
            if (rejection.data && rejection.data.message) {
                toaster.pop('error', "Error", rejection.data.message);
            }
            else if (rejection.data && rejection.data.error) {
                if (rejection.data.error === 'invalid_grant') {
                    toaster.pop('error', "Error", rejection.data.error_description);
                }
            }
        }
        else if (rejection.status === 401) {
            var authService = $injector.get('oAuthService');
            var authData = ipCookie(oAuthConstants.oAuthCookieName);
            var $http = $http || $injector.get('$http');
            var deferred = $q.defer();

            if (authData) {
                authService.refreshToken().then(function () {
                    // This repeats the request with the original parameters
                    return deferred.resolve($http(rejection.config));
                });
            }
            return deferred.promise;
        }
        else if (rejection.status === 403) {
            var toaster = $injector.get('toaster');
            toaster.pop('error', "Access Denied", "You are not authorized to do this request.");

            //window.location.path = oAuthConstants.appPathName;
        }
        else if (rejection.status === 0 || rejection.status === 500) {
            if (rejection.data && rejection.data.message) {
                toaster.pop('error', "Error", rejection.data.message);
            } else {
                toaster.pop('error', "Error", rejection.data);
            }
        }

        return $q.reject(rejection);
    }
}
```

### Jumuro.Angular.Spinner

Implementa una directiva para bloquear la ventana y mostrar un *spinner* durante las peticiones Http. Al realizar la petición se muestra el *spinner* y al recibir la respuesta se oculta.

### Jumuro.Angular.Validations

Contiene varias directivas de validación personalizadas.

### Jumuro.Angular.WebApi

Contiene dos clases de constantes, una donde la aplicación que incluya este paquete añadirá la/s URL/s de servidor y otra donde se añada la URL relativa a alguna de las URLs incluidas en la otra clase para cada operación del API de servidor.
