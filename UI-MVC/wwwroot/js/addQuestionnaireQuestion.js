let modalbody = document.querySelector(".addQQuestion");
let dropdown = document.getElementById("QuestionType");
let optionsContainer = document.querySelector(".addOptionsContainer");
let dropdownvalue = dropdown.options[dropdown.selectedIndex].value;
let optionsButton = document.querySelector(".optionsButton");
let containerToPutOptionsIn = document.querySelector(".optionsContainer");
let addedOptions = 0;

console.log("AddQuestionnaireQuestion active!");


function requiresOptions() {
    dropdownvalue = dropdown.options[dropdown.selectedIndex].value;
    console.log(dropdownvalue);

    if (dropdownvalue === "SINGLE" || dropdownvalue === "MULTI" || dropdownvalue === "DROP") {
        console.log(dropdownvalue);
        optionsContainer.classList.remove("hidden");


    } else {
        if (!optionsContainer.classList.contains("hidden")) {

            optionsContainer.classList.add("hidden");

        }
    }
}

function addOption() {
    let input = document.createElement("input");
    input.setAttribute("type", "text");
    input.setAttribute("class", "form-control")
    input.setAttribute("asp-for", "@Options[" + addedOptions + "]");

    let inputcontainer = document.createElement("div");
    inputcontainer.classList.add("inputcontainer");

    let removebutton = document.createElement("span");
    removebutton.classList.add("btn");
    removebutton.classList.add("btn-danger");
    removebutton.innerHTML = "X";

    inputcontainer.appendChild(input);
    inputcontainer.appendChild(removebutton);


    containerToPutOptionsIn.appendChild(inputcontainer);
    addedOptions++;
}


document.querySelector(".addOptionsButton").addEventListener("click", addOption);
dropdown.addEventListener("change", requiresOptions);




