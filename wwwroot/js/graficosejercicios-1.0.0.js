window.onload = GraficoTorta();


document.addEventListener('DOMContentLoaded', function() {
  var today = new Date();
  var currentMonth = today.getMonth() + 1; // Los meses en JavaScript son 0-11
  var currentYear = today.getFullYear();

  var monthSelect = document.getElementById('MesEjercicioBuscar');
  var yearSelect = document.getElementById('AnioEjercicioBuscar');

  monthSelect.value = currentMonth;
  yearSelect.value = currentYear;
});

let graficoEjercicio;
let graficoTortaEjercicio;


function GraficoTorta(){

  let mesBuscar = document.getElementById("MesEjercicioBuscar").value;
  let anioBuscar = document.getElementById("AnioEjercicioBuscar").value;
  let personaID = document.getElementById("PersonaID").value;

  

  $.ajax({
    // la URL para la petición
    url: '../../Seguimiento/GraficoTortaTipoEjerciciosPorMes',
    // la información a enviar
    // (también es posible utilizar una cadena de datos)
    data: { 
            Mes: mesBuscar,
            Anio: anioBuscar,
            PersonaID: personaID
          },
    // especifica si es una petición POST o GET
    type: 'POST',
    // el tipo de información que se espera de respuesta
    dataType: 'json',
    // código a ejecutar si la petición es satisfactoria
    success: function (vistaTipoEjercicioFisico) { 

      let labels = [];
      let data = [];
      let fondo = [];

      $.each(vistaTipoEjercicioFisico, function (index, tipoEjercicio) {  
        labels.push(tipoEjercicio.descripcion);
        data.push(tipoEjercicio.cantidadMinutos);
        let color = GenerarColor();
        fondo.push(color);
      });
      
      let ctxTorta = document.getElementById("graficoTorta");
      graficoTortaEjercicio = new Chart(ctxTorta, {
        type: 'pie',
        data: {
          labels: labels,
          datasets: [{
            data: data,
            backgroundColor: fondo,
            borderWidth: 3,
          }],
        },
      });
      MostrarGrafico();
    },
    error: function (xhr, status) {
      Swal.fire({
          title: "Disculpe",
          text: "existió un problema al cargar el grafico.",
          icon: "warning",
      });
    }
  });  
}



$(document).ready(function () {
  // Destruir el gráfico y volver a mostrarlo al cargar la página
  if (graficoEjercicio) {
      graficoEjercicio.destroy();
  }
  MostrarGrafico();

  // Destruir el gráfico y volver a mostrarlo cuando cambia el valor de #TipoEjercicioID
  $("#TipoEjercicioID").change(function () {
      if (graficoEjercicio) {
          graficoEjercicio.destroy();
      }
      MostrarGrafico();
  });
});


$("#MesEjercicioBuscar, #AnioEjercicioBuscar").change(function () {
  graficoEjercicio.destroy();
  graficoTortaEjercicio.destroy();
  GraficoTorta();
});

function MostrarGrafico(){
  let tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
  let mesEjercicioBuscar = document.getElementById("MesEjercicioBuscar").value;
  let anioEjercicioBuscar = document.getElementById("AnioEjercicioBuscar").value;
  let personaID = document.getElementById("PersonaID").value;

  $.ajax({
    // la URL para la petición
    url: '../../Seguimiento/GraficoTipoEjercicioMes',
    // la información a enviar
    // (también es posible utilizar una cadena de datos)
    data: { TipoEjercicioID: tipoEjercicioID, 
            Mes: mesEjercicioBuscar, 
            Anio: anioEjercicioBuscar,
            PersonaID: personaID
          },
    // especifica si será una petición POST o GET
    type: 'POST',
    // el tipo de información que se espera de respuesta
    dataType: 'json',
    // código a ejecutar si la petición es satisfactoria;
    // la respuesta es pasada como argumento a la función
    success: function (ejerciciosPorDias) {
      
      let labels = [];
      let data = [];
      let diasConEjercicios = 0;
      let minutosTotales = 0;

      $.each(ejerciciosPorDias, function (index, ejercicioDia) {  
        labels.push(ejercicioDia.mes + " " + ejercicioDia.dia);
        data.push(ejercicioDia.cantidadMinutos);
        
        minutosTotales += ejercicioDia.cantidadMinutos;
        
        if (ejercicioDia.cantidadMinutos > 0){
          diasConEjercicios += 1;
        }
      });

      let inputTipoEjercicio = document.getElementById("TipoEjercicioID");

      let ejercicioNombre = inputTipoEjercicio.options[inputTipoEjercicio.selectedIndex].text;

      let diasSinEjercicios = ejerciciosPorDias.length - diasConEjercicios;
      $("#totalEjercicios").text(minutosTotales + " MINUTOS EN " + diasConEjercicios + " DÍAS");
      $("#sinEjercicios").text(diasSinEjercicios + " DÍAS SIN " + ejercicioNombre);
      
      let ctx = document.getElementById('graficoPorDias');
      graficoEjercicio = new Chart(ctx, {
          type: 'bar',
          data: {
            labels: labels,
            datasets: [{
              label: '# of Votes',
              data: data,
              borderWidth: 1
            }]
          },
          options: {
            scales: {
              y: {
                beginAtZero: true
              }
            }
          }
        });
    },


    error: function (xhr, status) {
        Swal.fire({
            title: "Disculpe",
            text: "existió un problema al cargar el grafico.",
            icon: "warning",
        });
    }
});
}


function GenerarColor() {
  let rr, gg, bb;

  // Función para generar un valor aleatorio entre un rango dado
  function ajustarColor(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
  }

  // Generar colores entre azul, violeta, y rojo, evitando verdes
  rr = ajustarColor(128, 200); // 128 a 255 para rojo y violeta
  gg = ajustarColor(0, 128); // 0 a 127 para evitar verdes
  bb = ajustarColor(128, 256); // 128 a 255 para azul y violeta

  // Convertimos a hexadecimal y formateamos para que tenga siempre dos dígitos.
  let colorHex = `#${rr.toString(16).padStart(2, '0')}${gg.toString(16).padStart(2, '0')}${bb.toString(16).padStart(2, '0')}`;
  return colorHex;
}

