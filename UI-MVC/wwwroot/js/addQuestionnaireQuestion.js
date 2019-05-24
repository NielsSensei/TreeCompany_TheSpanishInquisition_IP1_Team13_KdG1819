let modalbody = document.querySelector(".addQQuestion");
let dropdown = document.getElementById("QuestionType");
let optionsContainer = document.querySelector(".addOptionsContainer");
let dropdownvalue = dropdown.options[dropdown.selectedIndex].value;
let optionsButton = document.querySelector(".optionsButton");
let containerToPutOptionsIn = document.querySelector(".optionsContainer");
let addedOptions = 0;

//console.log("AddQuestionnaireQuestion active!");
//console.log(dropdownvalue);

/**
 * @author Sacha Beulens
 * @documentation Xander Veldeman
 * 
 * Kijkt welke dropdownvalue geselecteerd heeft als een vraag SINGLE, DROP of MULTI is voegt hij de addOptie toe waardoor
 * de rest van de javascriptfile kan getriggered worden.
 *
 * @see Domain.UserInput.QuestionnaireQuestion
 * @see Domain.UserInput.QuestionType
 * 
 */
function requiresOptions() {
    dropdownvalue = dropdown.options[dropdown.selectedIndex].value;

    if (dropdownvalue.toUpperCase() === "SINGLE" || dropdownvalue.toUpperCase() === "MULTI" || 
        dropdownvalue.toUpperCase() === "DROP") {
        optionsContainer.classList.remove("hidden");
    } else {
        if (!optionsContainer.classList.contains("hidden")) {

            optionsContainer.classList.add("hidden");

        }
    }
}

/**
 * @author Sacha Beulens
 * @documentatie Niels van Zandbergen & Xander Veldeman
 * 
 * Als er een Optie verwijdert wordt bijvoorbeeld de eerste (index 0) dan zorgt deze functie ervoor dat er geen
 * ArgumentOutOfBoundsException gebeurd en kent de functie voor elk element dat nog bestaande is in de container een
 * nieuwe ID toe.
 */
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

/**
 * @author Sacha Beulens
 * @documentatie Xander Veldeman
 * 
 * Simpele verwijdering van een optie, deze functie wordt gekoppeld aan een button die bijgevoegd is aan addOption.
 * 
 * @see addQuestionnaireQuestion.js.AddOption
 * 
 * Remove an option when the buttion is ticked
 */
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

/**
 * @author Sacha Beulens
 * @documenation Xander Veldeman
 * 
 * Voegt een optie toe aan de modal die de gebruiker kan invullen en wordt mee gepersisteerd als optie van 
 * QuestionnaireQuestion.
 * 
 */
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