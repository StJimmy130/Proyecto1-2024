window.onload = ListadoLugares();


function ListadoLugares(){
    let personaID = document.getElementById("PersonaID").value;
    $.ajax({
        // la URL para la petición
        url: '../../Lugares/ListadoLugares',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {  
            personaID: personaID
        },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (lugares) {

            $("#ModalLugares").modal("hide");
            LimpiarModal()
            let contenidoTabla = ``;

            $.each(lugares, function (index, lugares) {  
                
                contenidoTabla += `
                <tr>
                    <td class="tablaDescripcion">${lugares.nombre}</td>
                    <td class="text-center">
                        <button type="button" class="btn btn-secondary botonTabla" title="Editar Registro" onclick="AbrirModalEditar(${lugares.lugarID})">
                            <i class="bi bi-pencil-square iconoBM"></i>Editar
                        </button>
                    </td>
                    <td class="text-center">
                        <button type="button" class="btn btn-danger botonTabla" title="Eliminar Registro" onclick="ValidacionEliminar(${lugares.lugarID})">
                            <i class="bi bi-trash3 iconoBM"></i>Eliminar
                        </button>
                    </td>
                </tr>
            `;

            });

            document.getElementById("tbody-lugares").innerHTML = contenidoTabla;

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
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
    document.getElementById("LugarID").value = 0;
    document.getElementById("Nombre").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("NUEVO LUGAR")
}

function ValidacionEliminar(lugarID) {
    Swal.fire({
        title: "¿Estás seguro?",
        text: "Este registro se eliminará!",
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "No, cancelar!",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, borrar!"
    }).then((result) => {
        if (result.isConfirmed) {
            // Realizar la solicitud AJAX para eliminar el registro si el usuario confirma
            $.ajax({
                url: '../../Lugares/EliminarLugar',
                data: { lugarID: lugarID },
                type: 'POST',
                dataType: 'json',
                success: function (resultado) {
                    if (resultado.exito) {
                        // Mostrar mensaje de éxito si se elimina correctamente
                        Swal.fire({
                            title: "Borrado!",
                            text: "Tu registro ha sido eliminado.",
                            icon: "success",
                        });
                        ListadoLugares(); // Actualiza la lista después de eliminar
                    } else {
                        // Mostrar advertencia si no se puede eliminar debido a relaciones
                        Swal.fire({
                            title: "No se puede eliminar",
                            text: resultado.mensaje,
                            icon: "error",
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        title: "Disculpe",
                        text: "Existió un problema al eliminar el registro",
                        icon: "warning",
                    });
                }
            });
        }
    });
}


function AbrirModalEditar(lugarID){
    $.ajax({
        // la URL para la petición
        url: '../../Lugares/ListadoLugares',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { id: lugarID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (lugares) {
            let lugar = lugares[0];

            document.getElementById("LugarID").value = lugarID;
            $("#ModalTitulo").text("Editar Lugar");
            document.getElementById("Nombre").value = lugar.nombre;
            $("#ModalLugares").modal("show");
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
    let personaID = document.getElementById("PersonaID").value;
    let lugarID = document.getElementById("LugarID").value;
    let nombre = document.getElementById("Nombre").value;
    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO 
    //SON DE INGRESO DE Y NO SE NECESITA VERIFICAR EN BASE DE DATOS.
    //LUEGO POR OTRO LADO HACER VERIFICACIÓN DE DATOS EN EL BACK,
    //SI EXISTE EL ELEMENTO SI NECESITAMOS BASE DE DATOS.
    $.ajax({
        // la URL para la petición
        url: '../../Lugares/GuardarLugares',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { 
            personaID: personaID,
            lugarID: lugarID, 
            nombre: nombre
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
            ListadoLugares();
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

