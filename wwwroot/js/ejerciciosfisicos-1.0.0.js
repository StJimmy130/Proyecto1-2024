$.ajax({
    url: '/EjerciciosFisicos/ObtenerEstadoEmocionalEnum',
    type: 'GET',
    dataType: 'json',
    success: function(data) {
        const estadoEmocionalInicio = $('#estadoEmocionalInicio');
        estadoEmocionalInicio.empty();
        
        if (data && data.length > 0) {
            data.forEach(valor => {
                // Agrega cada valor como un elemento de lista al dropdown
                estadoEmocionalInicio.append(`<li><a class="dropdown-item" href="#">${valor}</a></li>`);
            });
        } 
        else{
            estadoEmocionalInicio.text('No se encontraron valores de estado emocional.');
        }
    },
    error: function(xhr, status, error) {
        console.error('Error al obtener los valores del enum:', error);
        $('#estadoEmocionalInicio').text('Error al obtener los valores del enum.');
    }
});


$.ajax({
    url: '/EjerciciosFisicos/ObtenerEstadoEmocionalEnum',
    type: 'GET',
    dataType: 'json',
    success: function(data) {
        const estadoEmocionalFin = $('#estadoEmocionalFin');
        estadoEmocionalFin.empty();
        
        if (data && data.length > 0) {
            data.forEach(valor => {
                // Agrega cada valor como un elemento de lista al dropdown
                estadoEmocionalFin.append(`<li><a class="dropdown-item" href="#">${valor}</a></li>`);
            });
        } else {
            estadoEmocionalFin.text('No se encontraron valores de estado emocional.');
        }
    },
    error: function(xhr, status, error) {
        console.error('Error al obtener los valores del enum:', error);
        $('#estadoEmocionalFin').text('Error al obtener los valores del enum.');
    }
});


function GuardarEjerciciosFisicos() {
    // GUARDAMOS EN VARIABLES LOS VALORES INGRESADOS EN EL FORMULARIO
    let ejercicioFisicoID = document.getElementById("ejercicioFisicoID").value;
    let tipoEjercicioID = document.getElementById("tipoEjercicioID").value;
    let inicio = document.getElementById("inicio").value;
    let fin = document.getElementById("fin").value;
    let estadoEmocionalInicio = document.getElementById("estadoEmocionalInicio").value;
    let estadoEmocionalFin = document.getElementById("estadoEmocionalFin").value;
    let observaciones = document.getElementById("observaciones").value;

    // VERIFICAR LOS DATOS EN EL FRONT
    if (!inicio || !fin || !estadoEmocionalInicio || !estadoEmocionalFin) { 
        alert("Por favor, complete todos los campos obligatorios.");
        return; // SALIR DE LA FUNCIÓN SI FALTAN DATOS
    }

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
        success: function(resultadoGuardarEjercicioFisico) {
            if (resultadoGuardarEjercicioFisico !== "") {
                alert(resultadoGuardarEjercicioFisico);
            }
        },
        error: function(xhr, status) {
            console.log('Disculpe, existió un problema al guardar el registro');
        }
    });
}
