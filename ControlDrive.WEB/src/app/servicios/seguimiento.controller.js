(function() {
'use strict';
    angular.module('controldriveApp')
        .controller('SeguimientoController', function ($scope, $filter, $window, $modal, $state, toastr, PeriodoSvc, ServicioSvc, SeguimientoSvc, EstadoSvc, FechaSvc) {
            $scope.servicio = {};
            $scope.servicios = [];
            $scope.seguimiento = {};
            $scope.search = {
            }

            
            // $scope.pc.cities
            $scope.$parent.$parent.app.viewName = "Seguimiénto";

            $scope.servicios = [].concat($scope.servicios);
            $scope.periodo = PeriodoSvc.ObtenerPeriodoActual();
            //$scope.periodo = "23/11/2015";

            // $scope.maxHeight = window.innerHeight-180;

            $scope.VerSeguimientos = function (servicio) {
                $scope.servicio = servicio;
                $scope.showEditRow(servicio)
            }

            $scope.isSaving = false;
            $scope.ActualizarServicio = function () {
                ServicioSvc.Guardar($scope.servicio)
                    .then(function () {
                        toastr.success('Servicio actualizado correctamente.');
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.GuardarSeguimiento = function () {
                $scope.isSaving = true;
                if (!$scope.seguimiento) {
                    $scope.seguimiento = {};
                }
                $scope.seguimiento.ServicioId = $scope.servicio.Id;
                $scope.seguimiento.NuevoEstado = $scope.servicio.EstadoCodigo;
                
                SeguimientoSvc.Guardar($scope.seguimiento)
                    .then(function () {
                        $scope.seguimiento = {};
                        toastr.success('Seguimiento guardado correctamente.');
                        $scope.ObtenerServicios();
                        $scope.isSaving = false;
                    }, function (response) {
                        $scope.isSaving = false;
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerSeguimientos = function () {
                var idServicio = parseInt($scope.servicio.Id);
                SeguimientoSvc.ObtenerPorServicio(idServicio)
                    .then(function (response) {
                          $scope.servicio.Seguimientos = response.data;
                    }, function (response) {
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerEstados = function () {
                EstadoSvc.Obtener()
                    .then(function (response) {
                        $scope.estados = response.data;
                    })
                    .catch(function (response){
                        toastr.error(response.data.ExceptionMessage);
                    });
            }
            $scope.ObtenerEstados();

            $scope.ObtenerServicios = function () {
                ServicioSvc.ObtenerParaSeguimiento($scope.periodo)
                    .then(function(response){
                        $scope.servicios = response.data;
                    })
                    .catch(function (response){
                        toastr.error(response.data.ExceptionMessage);
                    });
            }

            $scope.ObtenerServicios();


            $scope.ExportarCSV = function () {
                var periodo = PeriodoSvc.FormatearParaApi($scope.periodo)
                ServicioSvc.ObtenerPorPeriodoCSV(periodo);
            }

            $scope.MostrarDetalles = function (servicio) {
                $modal.open({
                    templateUrl: 'detalleServicio.html',
                    size: 'md',
                    controller: function ($scope, $modalInstance, servicio) {
                        $scope.servicio = servicio;
                        $scope.cancel = function () {
                            $modalInstance.dismiss('cancel');
                        };
                        $scope.Editar = function (servicio) {
                            $scope.cancel();
                            var url = $state.href('app.editar', { id: servicio.Id })
                            $window.open(url,'_blank');
                            // $state.go('app.editar', { id: servicio.Id })
                        }
                    },
                    resolve: {
                        servicio: function () {
                            return servicio;
                        }
                    }
                });
            }

            $scope.EnviarCorreoConductor = function () {
                var serviciosSeleccionados = [];
                angular.forEach($scope.serviciosActivos, function (servicio) {
                    if (servicio.Seleccionado && servicio.Conductor)
                        serviciosSeleccionados.push(servicio);
                });
                if (serviciosSeleccionados.length > 0) {
                    ServicioSvc.NotificarServiciosAConductor(serviciosSeleccionados)
                        .then(function(){
                            toastr.success(response.data);
                        })
                        .catch(function(response){
                            toastr.error(response.data.ExceptionMessage);
                        })
                }
                else {
                    toastr.warning('El servicio seleccionado no tiene asignado un conductor.');
                }
            }


            $scope.EnviarCorreoRuta = function () {
                var serviciosSeleccionados = [];
                angular.forEach($scope.serviciosActivos, function (servicio) {
                    if (servicio.Seleccionado && servicio.Ruta)
                        serviciosSeleccionados.push(servicio);
                });
                if (serviciosSeleccionados.length > 0) {
                    ServicioSvc.NotificarServiciosARuta(serviciosSeleccionados)
                        .then(function(response){
                            toastr.success(response.data);
                        })
                        .catch(function(response){
                            toastr.error(response.data.ExceptionMessage);
                        })
                } else {
                    toastr.warning('El servicio seleccionado no tiene asignada una ruta.');
                }
            }

            $scope.Seleccionar = function (seleccion) {
                angular.forEach($scope.servicios, function (servicio) {
                    servicio.Seleccionado = seleccion;
                });
            }

            $scope.showEditRow = function (r) {
                if ($scope.active != r) {
                    $scope.active = r;
                }
                else {
                    $scope.active = null;
                }
            };

            // Based on an implementation here: web.student.tuwien.ac.at/~e0427417/jsdownload.html
            $scope.downloadFile = function(httpPath) {
                // Use an arraybuffer
                $http.get(httpPath, { responseType: 'arraybuffer'})
                .success( function(data, status, headers) {

                    var octetStreamMime = 'application/octet-stream';
                    var success = false;

                    // Get the headers
                    headers = headers();

                    // Get the filename from the x-filename header or default to "download.bin"
                    var filename = headers['x-filename'] || 'download.bin';

                    // Determine the content type from the header or default to "application/octet-stream"
                    var contentType = headers['content-type'] || octetStreamMime;

                    try
                    {
                        // Try using msSaveBlob if supported
                        console.log("Trying saveBlob method ...");
                        var blob = new Blob([data], { type: contentType });
                        if(navigator.msSaveBlob)
                            navigator.msSaveBlob(blob, filename);
                        else {
                            // Try using other saveBlob implementations, if available
                            var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
                            if(saveBlob === undefined) throw "Not supported";
                            saveBlob(blob, filename);
                        }
                        console.log("saveBlob succeeded");
                        success = true;
                    } catch(ex)
                    {
                        console.log("saveBlob method failed with the following exception:");
                        console.log(ex);
                    }

                    if(!success)
                    {
                        // Get the blob url creator
                        var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
                        if(urlCreator)
                        {
                            // Try to use a download link
                            var link = document.createElement('a');
                            if('download' in link)
                            {
                                // Try to simulate a click
                                try
                                {
                                    // Prepare a blob URL
                                    console.log("Trying download link method with simulated click ...");
                                    var blob = new Blob([data], { type: contentType });
                                    var url = urlCreator.createObjectURL(blob);
                                    link.setAttribute('href', url);

                                    // Set the download attribute (Supported in Chrome 14+ / Firefox 20+)
                                    link.setAttribute("download", filename);

                                    // Simulate clicking the download link
                                    var event = document.createEvent('MouseEvents');
                                    event.initMouseEvent('click', true, true, window, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
                                    link.dispatchEvent(event);
                                    console.log("Download link method with simulated click succeeded");
                                    success = true;

                                } catch(ex) {
                                    console.log("Download link method with simulated click failed with the following exception:");
                                    console.log(ex);
                                }
                            }

                            if(!success)
                            {
                                // Fallback to window.location method
                                try
                                {
                                    // Prepare a blob URL
                                    // Use application/octet-stream when using window.location to force download
                                    console.log("Trying download link method with window.location ...");
                                    var blob = new Blob([data], { type: octetStreamMime });
                                    var url = urlCreator.createObjectURL(blob);
                                    window.location = url;
                                    console.log("Download link method with window.location succeeded");
                                    success = true;
                                } catch(ex) {
                                    console.log("Download link method with window.location failed with the following exception:");
                                    console.log(ex);
                                }
                            }

                        }
                    }

                    if(!success)
                    {
                        // Fallback to window.open method
                        console.log("No methods worked for saving the arraybuffer, using last resort window.open");
                        window.open(httpPath, '_blank', '');
                    }
                })
                .error(function(data, status) {
                    console.log("Request failed with status: " + status);

                    // Optionally write the error out to scope
                    $scope.errorDetails = "Request failed with status: " + status;
                });
            };

        });
})();