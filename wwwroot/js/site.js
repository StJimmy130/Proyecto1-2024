// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// MODELO PERSONA
    // //NO MAPEADO DE EDAD
    // [NotMapped]
    //     public int Edad => (int)(DateTime.Now - FechaNacimiento).TotalDays / 365;
    // //EDAD EN VISTAMODELO
    // public int Edad { get; set; }


    // LLAMARLO EN LA FUNCIÓN GET, LUEGO DE CARGAR LOS DATOS
    //calcularIMC_TMB();

// function calcularIMC_TMB() {
//     // Obtén los valores de peso, altura, género y edad
//     let peso = parseFloat(document.getElementById("Peso").value);
//     let altura = parseFloat(document.getElementById("Altura").value) / 100; 
//     let genero = document.getElementById("Genero").value; 
//     let edad = parseInt(document.getElementById("Edad").value); 
  
//     if (peso && altura && edad) {
//       let imc = (peso / (altura * altura)).toFixed(2);
//       document.getElementById("IMC").value = imc + " (" + obtenerCategoriaIMC(imc) + ")";
  
//       let tmb;
//       if (genero === "1") {
//         tmb = (88.36 + (13.4 * peso) + (4.8 * (altura * 100)) - (5.7 * edad)).toFixed(2);
//       } 
//       else if (genero === "2") {
//         tmb = (447.6 + (9.2 * peso) + (3.1 * (altura * 100)) - (4.3 * edad)).toFixed(2);
//       }
//       document.getElementById("TMB").value = tmb + " kcal";
//     }
//   }
  
//   function obtenerCategoriaIMC(imc) {
//     if (imc < 18.5) return "Bajo peso";
//     else if (imc < 24.9) return "Normal";
//     else if (imc < 29.9) return "Sobrepeso";
//     else return "Obesidad";
//   }




// INPUTS PARA VISTA PERSONA EN CASO DE MOSTRAR LOS DATOS AHÍ
// <div class="coolinput col-md-4 mb-4">
//    <label class="text" for="Edad">Edad</label>
//    <input type="number" id="Edad" name="Edad" class="input" placeholder="Edad" readonly />
// </div> 
// <div class="coolinput col-md-4 mb-4">
//   <label class="text" for="IMC">IMC / Categoría</label>
//   <input type="text" step="0.01" id="IMC" name="IMC" class="input" placeholder="ICM / Categoría" readonly />
// </div>
// <div class="coolinput col-md-4 mb-4">
//   <label class="text" for="TMB">TMB</label>
//   <input type="text" step="0.01" id="TMB" name="TMB" class="input" placeholder="TMB" readonly />
// </div>







// <td>${deportista.edad}</td>
// <td>${calcularIMC(deportista.peso, deportista.altura)}</td>
// <td>${calcularCaloriasMantenimiento(deportista.peso, deportista.altura, deportista.edad, deportista.genero)}</td>



// function calcularIMC(peso, altura) {
//     peso = parseFloat(peso); 
//     altura = parseFloat(altura) / 100; 
  
//     if (peso <= 0 || altura <= 0) {
//         return "Datos inválidos";
//     }
  
//     const imc = (peso / (altura * altura)).toFixed(2); // Calcula el IMC
//     let categoria;
  
//     if (imc < 18.5) {
//         categoria = "Bajo peso";
//     } else if (imc >= 18.5 && imc < 24.9) {
//         categoria = "Peso normal";
//     } else if (imc >= 25 && imc < 29.9) {
//         categoria = "Sobrepeso";
//     } else {
//         categoria = "Obesidad";
//     }
  
//     return `${imc} (${categoria})`;
//   }
  
  
  
//   function calcularCaloriasMantenimiento(peso, altura, edad, genero, nivelActividad = 1.2) {
//     peso = parseFloat(peso);
//     altura = parseFloat(altura);
//     edad = parseInt(edad);
  
//     if (peso <= 0 || altura <= 0 || edad <= 0 || (genero !== "Hombre" && genero !== "Mujer")) {
//       return "Datos inválidos";
//     }
  
//     let tmb;
  
//     if (genero === "Hombre") {
//       tmb = 88.362 + (13.397 * peso) + (4.799 * altura) - (5.677 * edad);
//     } else if (genero === "Mujer") {
//       tmb = 447.593 + (9.247 * peso) + (3.098 * altura) - (4.330 * edad);
//     }
//     else{
//       return "Datos inválidos";
//     }
  
//     // Calorías de mantenimiento según nivel de actividad
//     const caloriasMantenimiento = (tmb * nivelActividad).toFixed(2);
  
//     return `${caloriasMantenimiento} kcal`;
//   }
  
  