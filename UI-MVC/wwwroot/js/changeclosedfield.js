//console.log("changeclosedfield active!");

let addedFieldStrings = 0;

/**
 * @author Niels Van Zandbergen
 * @documentation Niels Van Zandbergen
 *
 * @param index: Index is hier gelijkgesteld aan het aan te passen idee. 
 * 
 * Deze row met index is ook het aanspreek punt voor de andere functies.
 *
 * @returns {HTMLDivElement}
 *
 */
function addDivRow(index){
    let out = document.createElement("div");
    out.classList.add("editcontainerRow");
    out.classList.add("row");
    out.setAttribute("id", index);

    return out;
}


/**
 * @author Niels Van Zandbergen
 *
 * @returns {HTMLDivElement}
 *
 */
function addDivColumn(){
    let out = document.createElement("div");
    out.classList.add("editcontainerColumn");
    out.classList.add("col-6");

    return out;
}

/**
 * @author Niels Van Zandbergen
 * @documentation Niels Van Zandbergen
 *
 * @param index: Index is hier gelijkgesteld aan het aan te passen idee.
 *
 * @returns {HTMLInputElement}
 *
 */
function addInputPart(index){
    let out = document.createElement("input");
    out.setAttribute("type", "text");
    out.setAttribute("class", "form-control");
    out.classList.add("editStringsInput");
    out.setAttribute("id", index);
    out.setAttribute("name", "FieldStrings[" + index + "]");

    return out;
}

/**
 * @author Niels Van Zandbergen
 * @documentation Niels Van Zandbergen
 *
 * @param index: Index is hier gelijkgesteld aan het aan te passen idee.
 *
 * @returns {HTMLAnchorElement}
 *
 */
function addRemoveButton(index){
    let out = document.createElement("a");
    out.classList.add("btn");
    out.classList.add("btn-danger");
    out.classList.add("destroyEditString");
    out.innerHTML = "X";
    out.setAttribute("id", index);

    return out;
}

/**
 * @author Niels Van Zandbergen
 * @documentation Niels Van Zandbergen
 *
 * @param Idea: Idea is hier gelijkgesteld aan het aan te passen idee. 
 * 
 * Hier halen we de row van de addDivRow mee op.
 *
 * @see changeclosedfield.js.addDivRow.
 *
 * @returns {HTMLDivElement}
 *
 */
function collectContainer(Idea){
    let containers = document.querySelectorAll(".editContainer");
    let out = undefined;

    for(let i = 0; i < containers.length; i++){
        console.log(containers[i].getAttribute("id"));
        if(containers[i].getAttribute("id") === Idea){
            out = containers[i];
        }
    }

    return out;
}

/**
 * @authors Sacha Buelens & Niels Van Zandbergen
 * @documentation Niels Van Zandbergen
 *
 * @param Idea: Idea is hier gelijkgesteld aan het aan te passen idee.
 * 
 * Net zoals assignIndexes herziet hem alle indexen binnen dezelfde container om Exceptions te voorkomen.
 *
 * @see addQuestionnaireQuestion.js.assignIndexes
 *
 */
function redoIndexes(Idea){
    let container = collectContainer(Idea);
    addedFieldStrings = 0;

    let allInputs =  container.querySelectorAll(".editStringsInput");
    let allRemoveButtons = container.querySelectorAll(".destroyEditString");
    let allrows = container.querySelectorAll(".editcontainerRow");

    for (let i = 0; i < allInputs.length; i++) {
        addedFieldStrings = i;

        allInputs[i].setAttribute("name", "FieldStrings[" + addedFieldStrings + "]");
        allInputs[i].setAttribute("id", addedFieldStrings);
        allrows[i].setAttribute("id", addedFieldStrings);
        allRemoveButtons[i].setAttribute("id", addedFieldStrings);
    }

    addedFieldStrings = allInputs.length;

}

/**
 * @authors Sacha Buelens & Niels Van Zandbergen
 * @documentation Niels Van Zandbergen
 *
 * @param Idea: Idea is hier gelijkgesteld aan het aan te passen idee. Dit is ook het aanspreekpunt voor de container
 * waar we de closedfieldoption gaan verwijderen.
 * @param index: Dit is de index binnen de container.
 *
 */
function destroyClosedFieldPart(Idea, index){
    // console.log("Removing element: " + Idea + "." + index);

    let containerrows = document.querySelectorAll(".editcontainerRow");

    for (let i = 0; i < containerrows.length; i++) {
        if(containerrows[i].getAttribute("id") === index){
            containerrows[i].remove();
        }
    }

    redoIndexes(Idea);
}

/**
 * @authors Sacha Buelens & Niels Van Zandbergen
 * @documentation Niels Van Zandbergen
 *
 * @param Idea: Idea is hier gelijkgesteld aan het aan te passen idee. Dit is ook het aanspreekpunt voor de container
 * waar we de closedfieldoption gaan toevoegen. 
 * 
 * Waarom worden er voor elke destroybutton nog eens de eventhandlers toegevoegd? Goede vraag, de reden hiervoor is dat als we 
 * de modal ophalen voor EditIdea we voor elke option deze ook toevoegen aan de lijst in uniforme stijl met de deze javascript 
 * maar deze buttons deden voordien niets. 
 *
 */
function addClosedFieldPart(Idea){
    let container = collectContainer(Idea);

    let inputcontainer = addDivRow(addedFieldStrings);
    let leftinputcolumn = addDivColumn();
    let rightinputcolumn = addDivColumn();
    let input = addInputPart(addedFieldStrings);

    let destroybutton = addRemoveButton(addedFieldStrings);
    destroybutton.addEventListener("click",function(){
        destroyClosedFieldPart(Idea, destroybutton.getAttribute("id"))
    });

    leftinputcolumn.appendChild(input);
    rightinputcolumn.appendChild(destroybutton);
    inputcontainer.appendChild(leftinputcolumn);
    inputcontainer.appendChild(rightinputcolumn);
    container.appendChild(inputcontainer);

    addedFieldStrings++;

    console.log("New Part added for: " + Idea);
}

const buttons = document.querySelectorAll(".newFieldStringsButton");
for(let i = 0; i < buttons.length; i++){
    buttons[i].addEventListener("click", function(){
        let id = this.getAttribute("id");
        console.log("Editing for: " + id);
        addClosedFieldPart(id);
    });
}

const destroybuttons = document.querySelectorAll(".destroyEditString");
for(let i = 0; i < destroybuttons.length; i++){
    destroybuttons[i].addEventListener("click", function (){
        let idea = this.getAttribute("idea");
        let id = this.getAttribute("id");
        console.log("Destroying exisiting part: " + idea + "." + id);
        destroyClosedFieldPart(idea, id);
    });
}