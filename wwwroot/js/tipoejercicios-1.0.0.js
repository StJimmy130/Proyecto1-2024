window.onload = ListadoTipoEjercicios();


function ListadoTipoEjercicios() {
    $.ajax({
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        type: 'POST',
        dataType: 'json',
        success: function (tipoDeEjercicios) {
            $("#ModalTipoEjercicio").modal("hide");
            LimpiarModal();
            let contenidoTabla = ``;

            $.each(tipoDeEjercicios, function (index, tipoDeEjercicio) {
                // Verifica el estado de "Eliminado" para decidir qué mostrar
                let estadoBoton = tipoDeEjercicio.eliminado ? 
                    `<button type="button" class="btn btn-success botonTabla" title="Activar Registro" onclick="EliminarRegistro(${tipoDeEjercicio.tipoEjercicioID})">
                        <i class="bi bi-check-circle iconoBM"></i>Reactivar
                    </button>` :
                    `<button type="button" class="btn btn-warning botonTabla" onclick="EliminarRegistro(${tipoDeEjercicio.tipoEjercicioID})">
                        <i class="bi bi-trash iconoBM"></i>Desactivar
                    </button>`;

                let filaClase = tipoDeEjercicio.eliminado ? "table-dark" : ""; // Clase de Bootstrap para color gris

                // Si el tipo de ejercicio está eliminado, no se muestra el botón de editar
                let editarBoton = tipoDeEjercicio.eliminado ? "" : 
                    `<button type="button" class="btn btn-secondary botonTabla" title="Editar Registro" onclick="AbrirModalEditar(${tipoDeEjercicio.tipoEjercicioID})">
                        <i class="bi bi-pencil-square iconoBM"></i>Editar
                    </button>`;

                contenidoTabla += `
                <tr class="${filaClase}">
                    <td class="tablaDescripcion">${tipoDeEjercicio.descripcion}</td>
                    <td class="tablaDescripcion">${tipoDeEjercicio.met}</td>
                    <td class="text-center">
                        ${editarBoton}
                    </td>
                    <td class="text-center">
                        ${estadoBoton} 
                    </td>
                </tr>`;
            });

            document.getElementById("tbody-tipoejercicios").innerHTML = contenidoTabla;

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
    document.getElementById("TipoEjercicioID").value = 0;
    document.getElementById("descripcion").value = "";
    document.getElementById("MET").value = 0;
}

function NuevoRegistro(){
    $("#ModalTitulo").text("NUEVO TIPO DE EJERCICIO")
}



function AbrirModalEditar(tipoEjercicioID){
    $.ajax({
        // la URL para la petición
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: tipoEjercicioID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (tipoDeEjercicios) {
            let tipoDeEjercicio = tipoDeEjercicios[0];

            document.getElementById("TipoEjercicioID").value = tipoEjercicioID;
            $("#ModalTitulo").text("Editar Tipo de Ejercicio");
            document.getElementById("descripcion").value = tipoDeEjercicio.descripcion;
            document.getElementById("MET").value = tipoDeEjercicio.met;
            $("#ModalTipoEjercicio").modal("show");
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
    let tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
    let descripcion = document.getElementById("descripcion").value;
    let met = document.getElementById("MET").value;
    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO 
    //SON DE INGRESO DE Y NO SE NECESITA VERIFICAR EN BASE DE DATOS.
    //LUEGO POR OTRO LADO HACER VERIFICACIÓN DE DATOS EN EL BACK,
    //SI EXISTE EL ELEMENTO SI NECESITAMOS BASE DE DATOS.
    $.ajax({
        // la URL para la petición
        url: '../../TipoEjercicios/GuardarTipoEjercicio',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { 
            tipoEjercicioID: tipoEjercicioID, 
            descripcion: descripcion,
            met: met
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
            ListadoTipoEjercicios();
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

function EliminarRegistro(tipoEjercicioID) {
    $.ajax({
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        data: { tipoEjercicioID },
        type: 'GET',
        dataType: 'json',
        success: function (tipoEjercicio) {
            const nuevoEstado = !tipoEjercicio.eliminado; // Cambiar estado al opuesto

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
                    cambiarEstado(tipoEjercicioID, nuevoEstado);
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

function cambiarEstado(tipoEjercicioID, nuevoEstado) {
    $.ajax({
        url: '../../TipoEjercicios/EliminarTipoEjercicio',
        data: { tipoEjercicioID, nuevoEstado },
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
                const mensajeFinal = "El estado del tipo de ejercicio ha sido cambiado.";
                Swal.fire({
                    title: "Estado actualizado",
                    text: mensajeFinal,
                    icon: "success",
                });
            }
            ListadoTipoEjercicios(); 
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
