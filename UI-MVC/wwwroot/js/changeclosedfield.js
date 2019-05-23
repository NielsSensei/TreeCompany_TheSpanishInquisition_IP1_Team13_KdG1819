//console.log("changeclosedfield active!");

let addedFieldStrings = 0;

/**
 * @author Niels Van Zandbergen
 * 
 * Creates a bootstrap row div
 * 
 * @param index
 * @returns {HTMLDivElement}
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
 * creates a bootstrap column div
 * @returns {HTMLDivElement}
 */
function addDivColumn(){
    let out = document.createElement("div");
    out.classList.add("editcontainerColumn");
    out.classList.add("col-6");

    return out;
}

/**
 * @author Niels Van Zandbergen
 * 
 * creates a bootstrap form-control input
 * 
 * @param index
 * @returns {HTMLInputElement}
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
 * 
 * Creates a remove button
 * 
 * @param index
 * @returns {HTMLAnchorElement}
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
 * 
 * Gets all the elements with class editcontainer
 * 
 * @param Idea
 * @returns {undefined}
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
 * @author Niels Van Zandbergen
 * 
 * sets the id's of the inputs, removebuttons and rows
 * 
 * @param Idea
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
 * @author Niels Van Zandbergen
 * 
 * Removes the elements from the .editcontainerrow and redoes the indexes
 * 
 * @param Idea
 * @param index
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
 * @author Niels Van Zandbergen
 * 
 * Add a closedfield for options
 * 
 * @param Idea
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

/**
 * @author Niels Van Zandbergen
 * 
 * Adds the event listeners to the add field buttons
 * 
 * @type {NodeListOf<Element>}
 */
const buttons = document.querySelectorAll(".newFieldStringsButton");
for(let i = 0; i < buttons.length; i++){
    buttons[i].addEventListener("click", function(){
        let id = this.getAttribute("id");
        console.log("Editing for: " + id);
        addClosedFieldPart(id);
    });
}

/**
 * @author Niels Van Zandbergen
 * 
 * adds the event listeners for all of the remove field buttons
 * 
 * @type {NodeListOf<Element>}
 */
const destroybuttons = document.querySelectorAll(".destroyEditString");
for(let i = 0; i < destroybuttons.length; i++){
    destroybuttons[i].addEventListener("click", function (){
        let idea = this.getAttribute("idea");
        let id = this.getAttribute("id");
        console.log("Destroying exisiting part: " + idea + "." + id);
        destroyClosedFieldPart(idea, id);
    });
}