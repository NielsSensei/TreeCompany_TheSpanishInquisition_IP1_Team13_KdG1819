import 'jquery'
import 'jquery-validation'
import 'jquery-validation-unobtrusive'

console.log('JQuery validation bundle has been loaded');

var submit = document.getElementById("Submit");

submit.addEventListener("click", validateProjDate);
submit.addEventListener("click", validatePhaseDate);

function validateProjDate() {
    console.log("validate date");
    var startDate = document.getElementById("StartDate").value;
    var endDate = document.getElementById("EndDate").value;
    var error = document.getElementById("EndDateError");

    if ((Date.parse(endDate) <= Date.parse(startDate))) {
        error.innerText = "Eind datum kan niet voor begindatum zijn";
        document.getElementById("EndDate").value = "";
    }
}
function validatePhaseDate() {
    console.log("validate date");
    var startDate = document.getElementById("StartDatePhase").value;
    var endDate = document.getElementById("EndDatePhase").value;
    var error = document.getElementById("EndDateErrorPhase");

    if ((Date.parse(endDate) <= Date.parse(startDate))) {
        error.innerText = "Eind datum kan niet voor begindatum zijn";
        document.getElementById("EndDatePhase").value = "";
    }
} 
