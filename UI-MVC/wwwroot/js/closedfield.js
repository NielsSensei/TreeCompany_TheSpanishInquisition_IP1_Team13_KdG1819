console.log("closedfield active!");

let addedFieldStrings = 0;

function addDivRow(index){
    let out = document.createElement("div");
    out.classList.add("inputcontainerRow");
    out.classList.add("row");
    out.setAttribute("id", index);
    
    return out;
}

function addDivColumn(){
    let out = document.createElement("div");
    out.classList.add("inputcontainerColumn");
    out.classList.add("col-6");
    
    return out;
}

function addInputPart(index){
    let out = document.createElement("input");
    out.setAttribute("type", "text");
    out.setAttribute("class", "form-control");
    out.classList.add("fieldStringsInput");
    out.setAttribute("id", index);
    out.setAttribute("name", "FieldStrings[" + index + "]");
    
    return out;
}

function addRemoveButton(index){
    let out = document.createElement("a");
    out.classList.add("btn");
    out.classList.add("btn-danger");
    out.classList.add("destroyFieldString");
    out.innerHTML = "X";
    out.setAttribute("id", index);
    
    return out;
}

function collectContainer(Parent){
    let containers = document.querySelectorAll(".fieldStringContainer");
    let out = undefined;

    for(let i = 0; i < containers.length; i++){
        if(containers[i].getAttribute("id") === Parent){
            out = containers[i];
        }
    }
    
    return out;
}

function redoIndexes(Parent){
    let container = collectContainer(Parent);
    addedFieldStrings = 0;

    let allInputs =  container.querySelectorAll(".fieldStringsInput");
    let allRemoveButtons = container.querySelectorAll(".destroyFieldString");
    let allrows = container.querySelectorAll(".inputcontainerRow");

    for (let i = 0; i < allInputs.length; i++) {
        addedFieldStrings = i;

        allInputs[i].setAttribute("name", "FieldStrings[" + addedFieldStrings + "]");
        allInputs[i].setAttribute("id", addedFieldStrings);
        allrows[i].setAttribute("id", addedFieldStrings);
        allRemoveButtons[i].setAttribute("id", addedFieldStrings);
    }

    addedFieldStrings = allInputs.length;
    
}

function destroyClosedFieldPart(Parent, index){
    console.log("Removing element: " + Parent + "." + index);
    
    let containerrows = document.querySelectorAll(".inputcontainerRow");

    for (let i = 0; i < containerrows.length; i++) {
        if(containerrows[i].getAttribute("id") === index){
            containerrows[i].remove();
        }
    }
    
    redoIndexes(Parent);
}

function addClosedFieldPart(Parent){
    let container = collectContainer(Parent);
    
    let inputcontainer = addDivRow(addedFieldStrings);
    let leftinputcolumn = addDivColumn();
    let rightinputcolumn = addDivColumn();
    let input = addInputPart(addedFieldStrings);
    
    let destroybutton = addRemoveButton(addedFieldStrings);
    destroybutton.addEventListener("click",function(){
        destroyClosedFieldPart(Parent, destroybutton.getAttribute("id"))
    });

    leftinputcolumn.appendChild(input);
    rightinputcolumn.appendChild(destroybutton);
    inputcontainer.appendChild(leftinputcolumn);
    inputcontainer.appendChild(rightinputcolumn);
    container.appendChild(inputcontainer);

    addedFieldStrings++;
    
    console.log("New Part added for: " + Parent);
}

const buttons = document.querySelectorAll(".addFieldStringsButton");
for(let i = 0; i < buttons.length; i++){
    buttons[i].addEventListener("click", function(){
        let id = this.getAttribute("id");
        console.log("ParentIdea will be: " + id);
        addClosedFieldPart(id);
    });
}