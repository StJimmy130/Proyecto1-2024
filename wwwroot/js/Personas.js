window.onload = getInfo();


function getInfo() {
  let personaID = document.getElementById("PersonaID").value;
  $.ajax({
    url: "../../Personas/ObtenerDatosUsuario",
    data: { personaID: personaID },
    type: "POST",
    dataType: "json",
    success: function (data) {

      $("#NombreCompleto").val(data.nombreCompleto);
      $("#Genero").val(data.genero);
      $("#FechaNacimiento").val(data.fechaNacimiento);
      $("#Peso").val(data.peso);
      $("#Altura").val(data.altura);


    },
    error: function (xhr, status) {
      console.log("Disculpe, existió un problema al cargar el listado");
    },
  });
}

function ActualizarInformacion() {
  let personaID = document.getElementById("PersonaID").value;
  let nombreCompleto = $("#NombreCompleto").val();
  let genero = $("#Genero").val();
  let fechaNacimiento = $("#FechaNacimiento").val();
  let peso = $("#Peso").val();
  let altura = $("#Altura").val();
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