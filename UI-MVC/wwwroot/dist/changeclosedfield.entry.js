/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./wwwroot/js/changeclosedfield.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./wwwroot/js/changeclosedfield.js":
/*!*****************************************!*\
  !*** ./wwwroot/js/changeclosedfield.js ***!
  \*****************************************/
/*! no static exports found */
/***/ (function(module, exports) {

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

/***/ })

/******/ });
//# sourceMappingURL=changeclosedfield.entry.js.map