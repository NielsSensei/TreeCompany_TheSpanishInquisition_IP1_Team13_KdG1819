console.log("changeclosedfield active!");

let addedFieldStrings = 0;

function addDivRow(index){
    let out = document.createElement("div");
    out.classList.add("editcontainerRow");
    out.classList.add("row");
    out.setAttribute("id", index);

    return out;
}

function addDivColumn(){
    let out = document.createElement("div");
    out.classList.add("editcontainerColumn");
    out.classList.add("col-6");

    return out;
}

function addInputPart(index){
    let out = document.createElement("input");
    out.setAttribute("type", "text");
    out.setAttribute("class", "form-control");
    out.classList.add("editStringsInput");
    out.setAttribute("id", index);
    out.setAttribute("name", "FieldStrings[" + index + "]");

    return out;
}

function addRemoveButton(index){
    let out = document.createElement("a");
    out.classList.add("btn");
    out.classList.add("btn-danger");
    out.classList.add("destroyEditString");
    out.innerHTML = "X";
    out.setAttribute("id", index);

    return out;
}

function collectContainer(Idea){
    let containers = document.querySelectorAll(".editStringContainer");
    let out = undefined;

    for(let i = 0; i < containers.length; i++){
        console.log(containers[i].getAttribute("id"));
        if(containers[i].getAttribute("id") === Idea){
            out = containers[i];
        }
    }

    return out;
}

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

function destroyClosedFieldPart(Idea, index){
    console.log("Removing element: " + Idea + "." + index);

    let containerrows = document.querySelectorAll(".editcontainerRow");

    for (let i = 0; i < containerrows.length; i++) {
        if(containerrows[i].getAttribute("id") === index){
            containerrows[i].remove();
        }
    }

    redoIndexes(Idea);
}

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