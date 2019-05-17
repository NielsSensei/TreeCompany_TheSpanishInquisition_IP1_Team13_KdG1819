let closedFieldBody = document.querySelector(".addClosedField");
let fieldStringsContainer = document.querySelector(".addFieldStringsContainer");
let fieldButton = document.querySelector(".fieldStringButton");
let containerToPutFieldStringsIn = document.querySelector(".fieldStringContainer");
let addedFieldStrings = 0;

console.log("closedfield active!");

function reassignIndex() {
    addedFieldStrings = 0;

    let allInputs = document.querySelectorAll(".fieldStringsInput");
    let allRemoveButtons = document.querySelectorAll(".destroyFieldString");
    let allcontainers = document.querySelectorAll(".inputcontainer");

    for (var i = 0; i < allInputs.length; i++) {
        addedFieldStrings = i;

        allInputs[i].setAttribute("name", "FieldStrings[" + addedFieldStrings + "]");
        allcontainers[i].setAttribute("data-option", addedFieldStrings);
        allRemoveButtons[i].setAttribute("data-option", addedFieldStrings);

    }

    addedFieldStrings = allInputs.length;
}

function removeFieldString() {
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


function addFieldString() {
    console.log("Adding Field String: " + addedFieldStrings);
    let input = document.createElement("input");
    input.setAttribute("type", "text");
    input.setAttribute("class", "form-control");
    input.classList.add("fieldStringsInput");
    input.setAttribute("name", "FieldStrings[" + addedFieldStrings + "]");

    let inputcontainer = document.createElement("div");
    inputcontainer.classList.add("inputcontainer");
    inputcontainer.setAttribute("data-option",addedFieldStrings);

    let removebutton = document.createElement("a");
    removebutton.classList.add("btn");
    removebutton.classList.add("btn-danger");
    removebutton.classList.add("destroyFieldString");
    removebutton.innerHTML = "X";
    removebutton.setAttribute("data-option", addedFieldStrings);
    removebutton.addEventListener("click", removeFieldString);

    inputcontainer.appendChild(input);
    inputcontainer.appendChild(removebutton);


    containerToPutFieldStringsIn.appendChild(inputcontainer);
    addedFieldStrings++;
}

document.querySelector(".addFieldStringsButton").addEventListener("click", addFieldString);