window.onload = ListadoInformePorLugar();

function ListadoInformePorLugar() {
    let buscarInicioActividad = document.getElementById("BuscarInicioActividad").value;
    let buscarFinActividad = document.getElementById("BuscarFinActividad").value;
    let personaID = document.getElementById("PersonaID").value;
    
    $.ajax({
        url: '../../Seguimiento/ListadoInformePorLugar',
        data: { 
            id : null,
            BuscarInicioActividad: buscarInicioActividad,
            BuscarFinActividad: buscarFinActividad,
            PersonaID : personaID
        },
        type: 'POST',
        dataType: 'json',
        success: function (ejerciciosFisicosPorLugarMostrar) {
            let contenidoTabla = ``;
            let agrupadoPorLugar = {};

            $.each(ejerciciosFisicosPorLugarMostrar, function (index, ejercicio) {
                
                if (!agrupadoPorLugar[ejercicio.lugarString]) {
                    agrupadoPorLugar[ejercicio.lugarString] = [];
                }
                agrupadoPorLugar[ejercicio.lugarString].push(ejercicio);
            });

            for (let lugarString in agrupadoPorLugar) {
                contenidoTabla += `
                <tr>
                    <td class="text-start" colspan="6">${lugarString}</td>
                </tr>
                `;
                
                $.each(agrupadoPorLugar[lugarString], function (index, ejercicio) {
                    contenidoTabla += `
                    <tr>
                        <td></td>
                        <td>${ejercicio.fechaInicioString}</td>
                        <td>${ejercicio.fechaFinString}</td>
                        <td>${ejercicio.intervaloEjercicio} - min</td>
                        <td>${ejercicio.tipoEjercicioDescripcion}</td>
                        <td>${ejercicio.observaciones}</td>
                    </tr>
                    `
                });
            }
            document.getElementById("tbody-ejerciciosPorLugar").innerHTML = contenidoTabla;
        },
        error: function (xhr, status) {
            alert('Disculpe, existió un problema al procesar la solicitud.');
        }
    });
}
