window.onload = ListadoDeportistas();

function ListadoDeportistas() {
  let genero = document.getElementById("Genero").value;
  $.ajax({
      url: '../../Administracion/ObtenerPersonasConRoles',
      data: { genero: genero },
      type: 'POST',
      dataType: 'json',
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
      error: function (xhr, status) {
          Swal.fire({
              title: "Disculpe",
              text: "Existió un problema al cargar la lista de deportistas.",
              icon: "warning",
          });
      }
  });
}


function calcularIMC_TMB() {
  // Obtén los valores de peso, altura, género y edad
  let peso = parseFloat(document.getElementById("Peso").value);
  let altura = parseFloat(document.getElementById("Altura").value) / 100; // Convertir a metros
  let genero = document.getElementById("Genero").value; // Género seleccionado
  let edad = parseInt(document.getElementById("Edad").value); // Edad enviada desde el backend

  if (peso && altura && edad) {
    // Calcular IMC
    let imc = (peso / (altura * altura)).toFixed(2);
    document.getElementById("IMC").value = imc + " (" + obtenerCategoriaIMC(imc) + ")";

    // Calcular TMB (según Harris-Benedict)
    let tmb;
    if (genero === "1") {
      // Hombre
      tmb = (88.36 + (13.4 * peso) + (4.8 * (altura * 100)) - (5.7 * edad)).toFixed(2);
    } else if (genero === "2") {
      // Mujer
      tmb = (447.6 + (9.2 * peso) + (3.1 * (altura * 100)) - (4.3 * edad)).toFixed(2);
    }
    document.getElementById("TMB").value = tmb + " kcal";
  }
}

function obtenerCategoriaIMC(imc) {
  if (imc < 18.5) return "Bajo peso";
  else if (imc < 24.9) return "Normal";
  else if (imc < 29.9) return "Sobrepeso";
  else return "Obesidad";
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
  