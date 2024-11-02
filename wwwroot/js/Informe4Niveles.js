window.onload = Informe4Niveles;

function Informe4Niveles() {
    let personaID = document.getElementById("PersonaID").value;
    $.ajax({
        url: '../../Seguimiento/Informe4Nivel',
        type: 'POST',
        data: { PersonaID: personaID },
        dataType: 'json',
        success: function (vistaEventos) {
            let contenidoTabla = ``;

            $.each(vistaEventos, function (index, evento) {
                contenidoTabla += `
                <tr>
                    <td colspan="9" class="text-start text-white fw-bold bg-info">${evento.descripcion}</td>
                </tr>
                `;

                $.each(evento.vistaLugares, function (index, lugar) {

                    contenidoTabla += `
                    <tr>
                        <td  class="text-center"></td>
                        <td colspan="8" class="text-start text-white fw-bold bg-warning">${lugar.nombre}</td>
                    </tr>
                    `;

                    $.each(lugar.vistaTiposEjercicios, function (index, tipoEjercicio) {
                        contenidoTabla += `
                        <tr>
                            <td colspan="2" class="text-center"></td>
                            <td colspan="7" class="text-start text-white fw-bold bg-success">${tipoEjercicio.descripcion}</td>
                        </tr>
                        `;

                        $.each(tipoEjercicio.vistaEjerciciosFisicos, function (index, ejercicio) {
                            contenidoTabla += `
                            <tr class="text-white">
                                <td colspan="3" class="text-center"></td>
                                <td class="bg-secondary">${ejercicio.fechaInicioString}</td>
                                <td class="bg-secondary">${ejercicio.fechaFinString}</td>
                                <td class="text-center bg-secondary">${ejercicio.estadoEmocionalInicio}</td>
                                <td class="text-center bg-secondary">${ejercicio.estadoEmocionalFin}</td>
                                <td class="text-center bg-secondary">${ejercicio.intervaloEjercicio}</td>
                                <td class="text-center bg-secondary">${ejercicio.observaciones}</td>
                            </tr>
                            `;
                        });
                    });
                });
            });

            document.getElementById("tbody-informe4Niveles").innerHTML = contenidoTabla;
        },

        error: function (xhr, status) {
            console.log('Disculpe, existió un problema al cargar el listado :(');
        }
    });
}