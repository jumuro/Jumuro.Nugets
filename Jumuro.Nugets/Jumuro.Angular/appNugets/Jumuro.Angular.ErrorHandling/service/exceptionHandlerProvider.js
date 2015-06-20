// By default, AngularJS will catch errors and log them to
// the Console. We want to keep that behavior; however, we
// want to intercept it so that we can also log the errors
// to the server for later analysis.
angular.module('espa.errorHandling')
.provider("$exceptionHandler",  exceptionHandlerProvider); 

function exceptionHandlerProvider() {

    var _defaults = this.defaults = {
        sendEmailLocalHost: false,
        appName: '',
        appTechnology: 'AngularJS'
    };

    this.setDefaults = function (defaults) {
        if (defaults.sendEmailLocalHost)
        {
            _defaults.sendEmailLocalHost = defaults.sendEmailLocalHost;
        }
        if (defaults.appName)
        {
            _defaults.appName = defaults.appName;
        }
        
        if (defaults.appTechnology)
        {
            _defaults.appTechnology = defaults.appTechnology;
        }
        
    };
    
    this.$get = ['$log', '$window', 'stacktraceService', 'errorHandlingAppConfigConstants',
        function ($log, $window, stacktraceService, appConfig) {

        // I log the given error to the remote server.
        function log(exception, cause) {

            // Pass off the error to the default error handler
            // on the AngualrJS logger. This will output the
            // error to the console (and let the application
            // keep running normally for the user).
            $log.error.apply($log, arguments);

            // Now, we need to try and log the error the server.
            // --
            // TODO: In production, We should some debouncing 
            // logic here to prevent the same client from
            // logging the same error over and over again.
            try {

                var errorMessage = exception.toString();
                var stackTrace = stacktraceService.print({ e: exception }); 
                var navigator = $window.navigator;

                var isLocalHost = $window.location.href.toLowerCase().indexOf("localhost") > 0;

                if (!isLocalHost || _defaults.sendEmailLocalHost)
                {
                    //set the error log URL
                    if (!appConfig.appConfig.errorHanlingApiUrl) {
                        throw new Error('The -errorHanlingApiUrl- appConfig key is not present in the config file of the espa.errorHandling module');
                    }

                    var apiUrl = appConfig.appConfig.errorHanlingApiUrl;

                    //Get the user name


                    // Log the JavaScript error to the server.
                    $.ajax({
                        type: "POST",
                        url: apiUrl,
                        xhrFields: {
                            withCredentials: true
                        },
                        contentType: "application/json",
                        data: angular.toJson({
                            appName: _defaults.appName,
                            appTechnology: _defaults.appTechnology,
                            url: $window.location.href,
                            errorMessage: errorMessage,
                            stackTrace: stackTrace,
                            navigator: {
                                appVersion: navigator.appVersion,
                                cookieEnabled: navigator.cookieEnabled,
                                language: navigator.language,
                                userAgent: navigator.userAgent,
                                vendor: navigator.vendor,
                            }
                        })
                    });
                }
            

            } catch (loggingError) {

                // For ESPA - log the log-failure.
                $log.warn("Error logging failed");
                $log.log(loggingError);

            }

        }


        // Return the logging function.
        return (log);

    }];
        

    

}
