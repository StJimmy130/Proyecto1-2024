window.onload = ListadoTipoEjercicios();


function ListadoTipoEjercicios(){
    $.ajax({
        // la URL para la petición
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {  },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (tipoDeEjercicios) {

            $("#ModalTipoEjercicio").modal("hide");
            LimpiarModal()
            //$("#tbody-tipoejercicios").empty();
            let contenidoTabla = ``;

            $.each(tipoDeEjercicios, function (index, tipoDeEjercicio) {  
                
                contenidoTabla += `
                <tr>
                    <td class="tablaDescripcion">${tipoDeEjercicio.descripcion}</td>
                    <td class="text-center">
                        <button type="button" class="btn btn-secondary botonTabla" title="Editar Registro" onclick="AbrirModalEditar(${tipoDeEjercicio.tipoEjercicioID})">
                            <i class="bi bi-pencil-square iconoBM"></i>Editar
                        </button>
                    </td>
                    <td class="text-center">
                        <button type="button" class="btn btn-danger botonTabla" title="Eliminar Registro" onclick="ValidacionEliminar(${tipoDeEjercicio.tipoEjercicioID})">
                            <i class="bi bi-trash3 iconoBM"></i>Eliminar
                        </button>
                    </td>
                </tr>
            `;

            });

            document.getElementById("tbody-tipoejercicios").innerHTML = contenidoTabla;

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
    document.getElementById("TipoEjercicioID").value = 0;
    document.getElementById("descripcion").value = "";
}

function NuevoRegistro(){
    $("#ModalTitulo").text("NUEVO TIPO DE EJERCICIO")
}

function ValidacionEliminar(tipoEjercicioID){
    Swal.fire
    ({
        title: "¿Estas seguro?",
        text: "Este registro se eliminará!",
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "No, cancelar!",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, borrar!"
    })
    .then((result) => 
    {
        if (result.isConfirmed) {
        Swal.fire({
            title: "Borrado!",
            text: "Tu registro ha sido eliminado.",
            icon: "success",
        });
        EliminarRegistro(tipoEjercicioID)
        }
    });
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

function EliminarRegistro(tipoEjercicioID){
    $.ajax({
        // la URL para la petición
        url: '../../TipoEjercicios/EliminarTipoEjercicio',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { tipoEjercicioID: tipoEjercicioID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (resultado) {           
            ListadoTipoEjercicios();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            Swal.fire({
                title: "Disculpe",
                text: "existió un problema al eliminar el registro",
                icon: "warning",
            });
        }
    });    

}