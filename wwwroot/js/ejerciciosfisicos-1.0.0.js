window.onload = ListadoEjerciciosFisicos();

// Función para cargar opciones de estado emocional en un dropdown
function CargarOpcionesEstadoEmocional(data, dropdownID) {
    const dropdown = $(`#${dropdownID}`);
    dropdown.empty();

    // Agregar la opción "Seleccionar" al principio del dropdown
    dropdown.append(`<option value="0" disabled selected>Seleccionar</option>`);

    if (data && data.length > 0) {
        data.forEach(valor => {
            // Agrega cada valor como una opción en el dropdown
            dropdown.append(`<option value="${valor.estadoEmocionalID}">${valor.descripcion}</option>`);
        });
    } else {
        dropdown.append(`<option value="">No se encontraron valores de estado emocional</option>`);
    }
}

// Realizar la petición AJAX para obtener las opciones de estado emocional al iniciar
$.ajax({
    url: '/EjerciciosFisicos/ObtenerEstadoEmocionalEnum',
    type: 'GET',
    dataType: 'json',
    success: function(data) {
        CargarOpcionesEstadoEmocional(data, 'estadoEmocionalInicio');
    },
    error: function(xhr, status, error) {
        console.error('Error al obtener los valores del enum:', error);
        $('#estadoEmocionalInicio').append(`<option value="0">Error al obtener los valores del enum</option>`);
    }
});
// Realizar la petición AJAX para obtener las opciones de estado emocional al finalizar
$.ajax({
    url: '/EjerciciosFisicos/ObtenerEstadoEmocionalEnum',
    type: 'GET',
    dataType: 'json',
    success: function(data) {
        CargarOpcionesEstadoEmocional(data, 'estadoEmocionalFin');
    },
    error: function(xhr, status, error) {
        console.error('Error al obtener los valores del enum:', error);
        $('#estadoEmocionalFin').append(`<option value="0">Error al obtener los valores del enum</option>`);
    }
});




// Función para cargar opciones en un dropdown desde los datos obtenidos del servidor
// function CargarOpcionesTipoDeEjercicios(data, dropdownID) {
//     const dropdown = $(`#${dropdownID}`);
//     dropdown.empty();

//     // Agregar la opción "Seleccionar" al principio del dropdown
//     dropdown.append(`<option value="" disabled selected>Seleccionar</option>`);

//     // Si hay datos disponibles, agregar opciones al dropdown
//     if (data && data.length > 0) {
//         data.forEach(item => {
//             dropdown.append(`<option value="${item.tipoEjercicioID}">${item.descripcion}</option>`);
//         });
//     } else {
//         // Si no hay datos disponibles, mostrar un mensaje en el dropdown
//         dropdown.append(`<option value="">No se encontraron valores</option>`);
//     }
// }

// // Realizar la petición AJAX para obtener los tipos de ejercicios al iniciar
// $.ajax({
//     url: '/EjerciciosFisicos/ListaEjerciciosFisicos',
//     type: 'GET',
//     dataType: 'json',
//     success: function(data) {
//         CargarOpcionesTipoDeEjercicios(data, 'selectTipoEjercicio');
//     },
//     error: function(xhr, status, error) {
//         console.error('Error al obtener los tipos de ejercicios:', error);
//         $('#selectTipoEjercicio').append(`<option value="">Error al obtener los tipos de ejercicios</option>`);
//     }
// });




// HASTA ACÁ NO TENQUES NADA




function NuevoRegistro(){
    $("#ModalTitulo").text("NUEVO TIPO DE EJERCICIO")
}


function LimpiarModal(){
    document.getElementById("ejercicioFisicoID").value = 0;
    document.getElementById("tipoEjercicioID").value = 0;
    document.getElementById("inicio").value = "";
    document.getElementById("fin").value = "";
    document.getElementById("estadoEmocionalInicio").value = 0;
    document.getElementById("estadoEmocionalFin").value = 0;
    document.getElementById("observaciones").value = ""
}

function ListadoEjerciciosFisicos(){
    $.ajax({
        // la URL para la petición
        url: '../../EjerciciosFisicos/ListadoTipoEjerciciosFisicos',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: {  },
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (ejerciciosFisicos) {

            $("#ModalEjercicioFisico").modal("hide");
            LimpiarModal()
            //$("#tbody-tipoejercicios").empty();
            let contenidoTabla = ``;

            $.each(ejerciciosFisicos, function (index, ejercicioFisico) {  
                
                contenidoTabla += `
                <tr>
                    <td class="tablaDescripcion">${ejercicioFisico.tipoEjercicioDescripcion}</td>
                    <td class="tablaDescripcion">${ejercicioFisico.fechaInicioString}</td>
                    <td class="tablaDescripcion">${ejercicioFisico.fechaFinString}</td>
                    <td class="tablaDescripcion">${ejercicioFisico.estadoEmocionalInicio}</td>
                    <td class="tablaDescripcion">${ejercicioFisico.estadoEmocionalFin}</td>
                    <td class="tablaDescripcion">${ejercicioFisico.observaciones}</td>
                    <td class="text-center">
                        <button type="button" class="btn btn-secondary botonTabla" title="Editar Registro" onclick="AbrirModalEditar(${ejercicioFisico.ejercicioFisicoID})">
                            <i class="bi bi-pencil-square iconoBM"></i> Editar
                        </button>
                    </td>
                    <td class="text-center">
                        <button type="button" class="btn btn-danger botonTabla" title="Eliminar Registro" onclick="ValidacionEliminar(${ejercicioFisico.ejercicioFisicoID})">
                            <i class="bi bi-trash3 iconoBM"></i> Eliminar
                        </button>
                    </td>
                </tr>
            `;

            });

            document.getElementById("tbody-ejerciciosfisicos").innerHTML = contenidoTabla;

        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            Swal.fire({
                title: "Disculpe",
                text: "existió un problema al cargar la lista de ejercicios.",
                icon: "warning",
            });
        }
    });
}


function GuardarEjerciciosFisicos(){
    // GUARDAMOS EN VARIABLES LOS VALORES INGRESADOS EN EL FORMULARIO
    let ejercicioFisicoID = document.getElementById("ejercicioFisicoID").value;
    let tipoEjercicioID = document.getElementById("tipoEjercicioID").value;
    let inicio = document.getElementById("inicio").value;
    let fin = document.getElementById("fin").value;
    let estadoEmocionalInicio = document.getElementById("estadoEmocionalInicio").value;
    let estadoEmocionalFin = document.getElementById("estadoEmocionalFin").value;
    let observaciones = document.getElementById("observaciones").value;


    // REALIZAR LA PETICIÓN AJAX A LA BASE DE DATOS
    $.ajax({
        url: '../../EjerciciosFisicos/GuardarEjerciciosFisicos',
        data: { 
            ejercicioFisicoID: ejercicioFisicoID,
            tipoEjercicioID: tipoEjercicioID,
            inicio: inicio,
            fin: fin,
            estadoEmocionalInicio: estadoEmocionalInicio,
            estadoEmocionalFin: estadoEmocionalFin,
            observaciones: observaciones
        },
        type: 'POST',
        dataType: 'json',
        success: function(resultado) 
        {
            if(resultado != ""){
                swal.fire(resultado);
            }
            ListadoEjerciciosFisicos()
        },
        error: function(xhr, status) {
            Swal.fire({
                title: "Disculpe",
                text: "existió un problema al al guardar el registro.",
                icon: "warning",
            });
        }
    });
}


function ValidacionEliminar(ejercicioFisicoID){
    Swal.fire
    ({
        title: "¿Estas seguro?",
        text: "Este dato se eliminará!",
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
        EliminarRegistro(ejercicioFisicoID)
        }
    });
}


function EliminarRegistro(ejercicioFisicoID){
    $.ajax({
        // la URL para la petición
        url: '../../EjerciciosFisicos/EliminarEjercicioFisico',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { ejercicioFisicoID: ejercicioFisicoID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (resultado) {           
            ListadoEjerciciosFisicos();
        },

        // código a ejecutar si la petición falla;
        // son pasados como argumentos a la función
        // el objeto de la petición en crudo y código de estatus de la petición
        error: function (xhr, status) {
            Swal.fire({
                title: "Disculpe",
                text: "existió un problema al eliminar el registro.",
                icon: "warning",
            });
        }
    });    

}



function AbrirModalEditar(ejercicioFisicoID){
    $.ajax({
        // la URL para la petición
        url: '../../EjerciciosFisicos/EjerciciosFisicos',
        // la información a enviar
        // (también es posible utilizar una cadena de datos)
        data: { ejercicioFisicoID: ejercicioFisicoID},
        // especifica si será una petición POST o GET
        type: 'POST',
        // el tipo de información que se espera de respuesta
        dataType: 'json',
        // código a ejecutar si la petición es satisfactoria;
        // la respuesta es pasada como argumento a la función
        success: function (ejercicioFisico) {
            let ejerciciosFisicos = ejercicioFisico[0];

            document.getElementById("ejercicioFisicoID").value = ejercicioFisicoID;
            $("#ModalTitulo").text("Editar Tipo de Ejercicio");
            document.getElementById("tipoEjercicioID").value = ejerciciosFisicos.tipoEjercicioID;
            document.getElementById("inicio").value = ejerciciosFisicos.inicio;
            document.getElementById("fin").value = ejerciciosFisicos.fin;
            document.getElementById("estadoEmocionalInicio").value = ejerciciosFisicos.estadoEmocionalInicio;
            document.getElementById("estadoEmocionalFin").value = ejerciciosFisicos.estadoEmocionalFin;
            document.getElementById("observaciones").value = ejerciciosFisicos.observaciones;

            $("#ModalEjercicioFisico").modal("show");
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


