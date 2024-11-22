window.onload = getInfo();


function getInfo() {
  let personaID = document.getElementById("PersonaID").value;
  $.ajax({
    url: "../../Personas/ObtenerDatosUsuario",
    data: { personaID: personaID },
    type: "POST",
    dataType: "json",
    success: function (data) {
      document.getElementById("NombreCompleto").value = data.nombreCompleto;
      document.getElementById("Genero").value = data.genero;
      document.getElementById("FechaNacimiento").value = data.fechaNacimiento.split('T')[0];
      document.getElementById("Peso").value = data.peso;
      document.getElementById("Altura").value = data.altura;
    
      calcularIMC_TMB();
    },
    error: function (xhr, status) {
      console.log("Disculpe, existió un problema al cargar el listado");
    },
  });
}

function ActualizarInformacion() {
  let personaID = document.getElementById("PersonaID").value;
  let nombreCompleto = document.getElementById("NombreCompleto").value;
  let genero = document.getElementById("Genero").value;
  let fechaNacimiento = document.getElementById("FechaNacimiento").value.split('T')[0];
  let peso = document.getElementById("Peso").value;
  let altura = document.getElementById("Altura").value;
  $.ajax({
    url: "../../Personas/GuardarDatosUsuario",
    data: {
      personaID: personaID,
      nombreCompleto: nombreCompleto,
      genero: genero,
      fechaNacimiento: fechaNacimiento,
      peso: peso,
      altura: altura,
    },
    type: "POST",
    dataType: "json",
    success: function (data) {
      console.log(data);
    },
    error: function (xhr, status) {
      console.log("Disculpe, existió un problema al cargar el listado");
    },
  });
}



