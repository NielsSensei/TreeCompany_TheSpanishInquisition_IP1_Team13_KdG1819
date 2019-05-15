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

    if (dropdownvalue === "SINGLE" || dropdownvalue === "MULTI" || dropdownvalue === "DROP") {
        optionsContainer.classList.remove("hidden");
    } else {
        if (!optionsContainer.classList.contains("hidden")) {

            optionsContainer.classList.add("hidden");

        }
    }
}

function reassignIndex() {
    addedOptions = 0;

    let allInputs = document.querySelectorAll(".optionsInput");
    let allRemoveButtons = document.querySelectorAll(".destroyOption");
    let allcontainers = document.querySelectorAll(".inputcontainer");

    for (var i = 0; i < allInputs.length; i++) {
        addedOptions = i;

        allInputs[i].setAttribute("name", "Options[" + addedOptions + "]");
        allcontainers[i].setAttribute("data-option", addedOptions);
        allRemoveButtons[i].setAttribute("data-option", addedOptions);

    }

    addedOptions = allInputs.length;
}

function removeOption() {
    console.log("Removing element: " + this.getAttribute("data-option"));

    let index = this.getAttribute("data-option");
    let allcontainers = document.querySelectorAll(".inputcontainer");

    for (var i = 0; i < allcontainers.length; i++) {

        if (index === allcontainers[i].getAttribute("data-option")) {
            console.log("Attribute compare -> Index: " + index + " Allcontainers[" + i + "]: " + allcontainers[i].getAttribute("data-option").value);
            allcontainers[i].remove();
        }
    }

    reassignIndex();
}


function addOption() {
    console.log("Adding option: " + addedOptions);
    let input = document.createElement("input");
    input.setAttribute("type", "text");
    input.setAttribute("class", "form-control");
    input.classList.add("optionsInput");
    input.setAttribute("name", "Options[" + addedOptions + "]");

    let inputcontainer = document.createElement("div");
    inputcontainer.classList.add("inputcontainer");
    inputcontainer.setAttribute("data-option",addedOptions);

    let removebutton = document.createElement("a");
    removebutton.classList.add("btn");
    removebutton.classList.add("btn-danger");
    removebutton.classList.add("destroyOption");
    removebutton.innerHTML = "X";
    removebutton.setAttribute("data-option", addedOptions);
    removebutton.addEventListener("click", removeOption);

    inputcontainer.appendChild(input);
    inputcontainer.appendChild(removebutton);


    containerToPutOptionsIn.appendChild(inputcontainer);
    addedOptions++;
}

document.querySelector(".addOptionsButton").addEventListener("click", addOption);
dropdown.addEventListener("change", requiresOptions);