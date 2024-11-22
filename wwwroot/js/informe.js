window.onload = ListadoInforme();


function ListadoInforme() {
    let buscarInicioActividad = document.getElementById("BuscarInicioActividad").value;
    let buscarFinActividad = document.getElementById("BuscarFinActividad").value;
    let personaID = document.getElementById("PersonaID").value;
    
    $.ajax({
        url: '../../Seguimiento/ListadoInforme',
        data: { 
            id: null,  // Incluye el parámetro 'id' si es necesario
            BuscarInicioActividad: buscarInicioActividad,
            BuscarFinActividad: buscarFinActividad,
            PersonaID: personaID
        },
        type: 'POST',
        dataType: 'json',
        success: function (ejerciciosFisicosMostrar) {
            let contenidoTabla = ``;
            let agrupadoPorTipo = {};

            $.each(ejerciciosFisicosMostrar, function (index, ejercicio) {
                
                if (!agrupadoPorTipo[ejercicio.tipoEjercicioDescripcion]) {
                    agrupadoPorTipo[ejercicio.tipoEjercicioDescripcion] = [];
                }
                agrupadoPorTipo[ejercicio.tipoEjercicioDescripcion].push(ejercicio);
            });

            for (let tipoEjercicio in agrupadoPorTipo) {
                contenidoTabla += `
                <tr>
                    <td class="text-start" colspan="8">${tipoEjercicio}</td>
                </tr>
                `;
                
                $.each(agrupadoPorTipo[tipoEjercicio], function (index, ejercicio) {
                    contenidoTabla += `
                    <tr>
                        <td colspan="1"></td>
                        <td>${ejercicio.fechaInicioString}</td>
                        <td>${ejercicio.fechaFinString}</td>
                        <td>${ejercicio.intervaloEjercicio} - min</td>
                        <td>${ejercicio.caloriasQuemadas}</td>
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




