window.onload = ListadoEventosDeportivos();


function ListadoEventosDeportivos() {
    $.ajax({
        url: '../../EventosDeportivos/ListadoEventosDeportivos',
        type: 'POST',
        dataType: 'json',
        success: function (EventosDeportivos) {
            $("#ModalEventosDeportivos").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(EventosDeportivos, function (index, eventoDeportivo) {  
                // Verifica el estado de "Eliminado" para decidir qué botones mostrar
                let estadoBoton = eventoDeportivo.eliminado ? 
                    `<button type="button" class="btn btn-success botonTabla" title="Activar Registro" onclick="EliminarRegistro(${eventoDeportivo.eventoDeportivoID}, false)">
                        <i class="bi bi-check-circle iconoBM"></i>Reactivar
                    </button>` :
                    `<button type="button" class="btn btn-warning botonTabla" title="Desactivar Registro" onclick="EliminarRegistro(${eventoDeportivo.eventoDeportivoID}, true)">
                        <i class="bi bi-trash iconoBM"></i>Desactivar
                    </button>`;

                let filaClase = eventoDeportivo.eliminado ? "table-dark" : ""; // Clase de Bootstrap para color gris

                // Si el evento está eliminado, no se muestra el botón de editar
                let editarBoton = eventoDeportivo.eliminado ? "" : 
                    `<button type="button" class="btn btn-secondary botonTabla" title="Editar Registro" onclick="AbrirModalEditar(${eventoDeportivo.eventoDeportivoID})">
                        <i class="bi bi-pencil-square iconoBM"></i>Editar
                    </button>`;

                contenidoTabla += `
                <tr class="${filaClase}">
                    <td class="tablaDescripcion">${eventoDeportivo.descripcion}</td>
                    <td class="text-center">
                        ${editarBoton}
                    </td>
                    <td class="text-center">
                        ${estadoBoton}
                    </td>
                </tr>`;
            });

            document.getElementById("tbody-eventosDeportivos").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            Swal.fire({
                icon: "error",
                title: "Oops...",
                text: "Algo salió mal!",
            });
        }
    });
}

function LimpiarModal(){
    document.getElementById("EventoDeportivoID").value = 0;
    document.getElementById("Descripcion").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("NUEVO EVENTO DEPORTIVO")
}



function AbrirModalEditar(eventoDeportivoID){
    $.ajax({
        // la URL para la petición
        url: '../../EventosDeportivos/ListadoEventosDeportivos',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: eventoDeportivoID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (eventoDeportivo) {
            let eventoDeportivoEditar = eventoDeportivo[0];

            document.getElementById("EventoDeportivoID").value = eventoDeportivoID;
            $("#ModalTitulo").text("Editar evento deportivo");
            document.getElementById("Descripcion").value = eventoDeportivoEditar.descripcion;
            $("#ModalEventosDeportivos").modal("show");
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            Swal.fire({
                title: "Disculpe",
                text: "existió un problema al consultar el registro para ser modificado.",
                icon: "warning",
            });
        }
    });
}


function GuardarRegistro(){
    //GUARDAMOS EN UNA VARIABLE LO ESCRITO EN EL INPUT DESCRIPCION
    let eventoDeportivoID = document.getElementById("EventoDeportivoID").value;
    let descripcion = document.getElementById("Descripcion").value;
    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO 
    //SON DE INGRESO DE Y NO SE NECESITA VERIFICAR EN BASE DE DATOS.
    //LUEGO POR OTRO LADO HACER VERIFICACIÓN DE DATOS EN EL BACK,
    //SI EXISTE EL ELEMENTO SI NECESITAMOS BASE DE DATOS.
    $.ajax({
        // la URL para la petición
        url: '../../EventosDeportivos/GuardarEventoDeportivo',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { 
            eventoDeportivoID: eventoDeportivoID, 
            descripcion: descripcion
        },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (resultado) {

            if(resultado != ""){
                Swal.fire(resultado);
            }
            ListadoEventosDeportivos();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            Swal.fire({
                title: "Disculpe",
                text: "existió un problema al guardar el registro",
                icon: "warning",
            });
        }
    });    
}

function EliminarRegistro(eventoDeportivoID) {
    $.ajax({
        url: '../../EventosDeportivos/ListadoEventosDeportivos',
        data: { eventoDeportivoID },
        type: 'GET',
        dataType: 'json',
        success: function (eventoDeportivo) {
            const nuevoEstado = !eventoDeportivo.eliminado; // Cambiar estado al opuesto

            // Mensaje ambigüo
            const titulo = "¿Estás seguro de cambiar el estado?";
            const mensaje = "Este registro cambiará su estado actual.";

            // Mostrar alerta de confirmación
            Swal.fire({
                title: titulo,
                text: mensaje,
                icon: "warning",
                showCancelButton: true,
                cancelButtonText: "No, cancelar",
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Sí, cambiar estado"
            }).then((result) => {
                if (result.isConfirmed) {
                    cambiarEstado(eventoDeportivoID, nuevoEstado);
                }
            });
        },
        error: function () {
            Swal.fire({
                title: "Error",
                text: "No se pudo obtener la información del tipo de ejercicio.",
                icon: "error",
            });
        }
    });
}

function cambiarEstado(eventoDeportivoID, nuevoEstado) {
    $.ajax({
        url: '../../EventosDeportivos/EliminarEventoDeportivo',
        data: { eventoDeportivoID, nuevoEstado },
        type: 'POST',
        dataType: 'json',
        success: function (resultado) {
            if (resultado.mensaje) {
                // Mostrar mensaje de advertencia en caso de error
                Swal.fire({
                    title: "Atención",
                    text: resultado.mensaje,
                    icon: "warning",
                });
            } else {
                // Mostrar el estado final sin especificar si fue activado o desactivado
                const mensajeFinal = "El estado del evento deportivo ha sido cambiado.";
                Swal.fire({
                    title: "Estado actualizado",
                    text: mensajeFinal,
                    icon: "success",
                });
            }
            ListadoEventosDeportivos(); 
        },
        error: function () {
            Swal.fire({
                title: "Disculpe",
                text: "Existió un problema al cambiar el estado del registro.",
                icon: "warning",
            });
        }
    });
}
