

function ListadoInforme() {
    let buscarInicioActividad = document.getElementById("BuscarInicioActividad").value;
    let buscarFinActividad = document.getElementById("BuscarFinActividad").value;
    
    $.ajax({
        url: '../../Seguimiento/ListadoInforme',
        data: { 
            id: null,  // Incluye el parámetro 'id' si es necesario
            BuscarInicioActividad: buscarInicioActividad,
            BuscarFinActividad: buscarFinActividad,
        },
        type: 'POST',
        dataType: 'json',
        success: function (ejerciciosFisicosMostrar) {
            let contenidoTabla = ``;
            let agrupadoPorTipo = {};

            $.each(ejerciciosFisicosMostrar, function (index, ejercicio) {
                // Agrupar por TipoEjercicioDescripcion
                if (!agrupadoPorTipo[ejercicio.tipoEjercicioDescripcion]) {
                    agrupadoPorTipo[ejercicio.tipoEjercicioDescripcion] = [];
                }
                agrupadoPorTipo[ejercicio.tipoEjercicioDescripcion].push(ejercicio);
            });

            // Construir el contenido de la tabla
            for (let tipoEjercicio in agrupadoPorTipo) {
                contenidoTabla += `
                <tr>
                    <td>${tipoEjercicio}</td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                `;
                
                $.each(agrupadoPorTipo[tipoEjercicio], function (index, ejercicio) {
                    contenidoTabla += `
                    <tr>
                        <td></td>
                        <td>${ejercicio.fechaInicioString}</td>
                        <td>${ejercicio.fechaFinString}</td>
                        <td>${ejercicio.intervaloEjercicio} - min</td>
                        <td>${ejercicio.estadoEmocionalInicio}</td>
                        <td>${ejercicio.estadoEmocionalFin}</td>
                        <td>${ejercicio.observaciones}</td>
                    </tr>
                    `
                });
            }

            document.getElementById("tbody-ejerciciosportipo").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al procesar la solicitud.');
        }
    });
}
