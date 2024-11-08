window.onload = ListadoDeportistas();

function ListadoDeportistas() {
    let genero = document.getElementById("Genero").value;
    $.ajax({
        // La URL para la petición
        url: '../../Administracion/ObtenerPersonasConRoles',  // Cambia la URL según sea necesario
        // Los datos a enviar
        data: { genero: genero },
        // Especifica si será una petición POST o GET
        type: 'POST',
        // El tipo de información que se espera de respuesta
        dataType: 'json',
        // Código a ejecutar si la petición es satisfactoria;
        // La respuesta es pasada como argumento a la función
        success: function (deportistas) {

            let contenidoTabla = ``;

            $.each(deportistas, function (index, deportista) {
                contenidoTabla += `
                <tr>
                    <td>${deportista.nombreCompleto}</td>
                    <td>${deportista.email}</td>
                    <td>${deportista.fechaNacimiento}</td>
                    <td>${deportista.genero}</td>
                    <td>${deportista.peso}</td>
                    <td>${deportista.altura}</td>
                    <td>${deportista.rol}</td>
                </tr>
                `;
            });

            // Asignar el contenido de la tabla
            document.getElementById("tbody-informeDeportistas").innerHTML = contenidoTabla;
        },
        // Código a ejecutar si la petición falla
        error: function (xhr, status) {
            Swal.fire({
                title: "Disculpe",
                text: "Existió un problema al cargar la lista de deportistas.",
                icon: "warning",
            });
        }
    });
}


function LimpiarFiltro() {
    document.getElementById("Genero").value = "0";
    ListadoDeportistas(0);  
}




function ImprimirInforme() {
    var doc = new jsPDF();
    
    // Agregar el título
    doc.setFontSize(16);
    doc.setFont("helvetica", "bold");
    doc.text(105, 15, "WORKOUT WATCH!", null, null, "center");
  
    // Agregar el subtítulo
    doc.setFontSize(12);
    doc.setFont("helvetica", "normal");
    doc.text(20, 25, "Informe de deportistas registrados");

  
    // Configurar el contenido de la página
    var totalPagesExp = "{total_pages_count_string}";
    var pageContent = function (data) {
      var pageHeight = doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
      var pageWidth = doc.internal.pageSize.width || doc.internal.pageSize.getWidth();
  
      // FOOTER
      var str = "Pagina " + data.pageCount;
      if (typeof doc.putTotalPages == "function") {
        str = str + " de " + totalPagesExp;
      }
  
      doc.setLineWidth(8);
      doc.setDrawColor(238, 238, 238);
      doc.line(14, pageHeight - 11, pageWidth - 14, pageHeight - 11);
  
      doc.setFontSize(10);
      doc.setFontStyle("bold");
      doc.text(str, 17, pageHeight - 10);
    };
  
    // Obtiene la tabla de deportistas
    var elem = document.getElementById("tabla-deportistas");
    var res = doc.autoTableHtmlToJson(elem);
  
    // Generar la tabla
    doc.autoTable(res.columns, res.data, {
      addPageContent: pageContent,
      theme: "grid",
      startY: 30, // Posicionar la tabla debajo del título y subtítulo
      styles: { fillColor: [255, 128, 0], halign: "center" },
      columnStyles: {
        0: { halign: "center", fillColor: [255, 255, 255], fontSize: 7 },
        1: { halign: "left", fillColor: [255, 255, 255], fontSize: 7 },
        2: { halign: "left", fillColor: [255, 255, 255], fontSize: 7 },
        3: { halign: "left", fillColor: [255, 255, 255], fontSize: 7 },
        4: { halign: "right", fillColor: [255, 255, 255], fontSize: 7 },
        5: { halign: "right", fillColor: [255, 255, 255], fontSize: 7 },
        6: { halign: "left", fillColor: [255, 255, 255], fontSize: 7 },
      },
      margin: { top: 10 },
    });
  
    if (typeof doc.putTotalPages === "function") {
      doc.putTotalPages(totalPagesExp);
    }
  
    // Mostrar el PDF en una nueva ventana
    var string = doc.output("datauristring");
    var iframe = "<iframe width='100%' height='100%' src='" + string + "'></iframe>";
    var x = window.open();
    x.document.open();
    x.document.write(iframe);
    x.document.close();
  }
  