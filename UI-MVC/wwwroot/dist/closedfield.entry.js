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
/******/ 	return __webpack_require__(__webpack_require__.s = "./wwwroot/js/closedfield.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./wwwroot/js/closedfield.js":
/*!***********************************!*\
  !*** ./wwwroot/js/closedfield.js ***!
  \***********************************/
/*! no static exports found */
/***/ (function(module, exports) {

//console.log("closedfield active!");

let addedFieldStrings = 0;

/**
 * @author Niels Van Zandbergen
 * @param index
 * @returns {HTMLDivElement}
 */
function addDivRow(index){
    let out = document.createElement("div");
    out.classList.add("inputcontainerRow");
    out.classList.add("row");
    out.setAttribute("id", index);
    
    return out;
}

/**
 * @author Niels Van Zandbergen
 * @returns {HTMLDivElement}
 */
function addDivColumn(){
    let out = document.createElement("div");
    out.classList.add("inputcontainerColumn");
    out.classList.add("col-6");
    
    return out;
}

/**
 * @author Niels Van Zandbergen
 * @param index
 * @returns {HTMLInputElement}
 */
function addInputPart(index){
    let out = document.createElement("input");
    out.setAttribute("type", "text");
    out.setAttribute("class", "form-control");
    out.classList.add("fieldStringsInput");
    out.setAttribute("id", index);
    out.setAttribute("name", "FieldStrings[" + index + "]");
    
    return out;
}

/**
 * @author Niels Van Zandbergen
 * @param index
 * @returns {HTMLAnchorElement}
 */
function addRemoveButton(index){
    let out = document.createElement("a");
    out.classList.add("btn");
    out.classList.add("btn-danger");
    out.classList.add("destroyFieldString");
    out.innerHTML = "X";
    out.setAttribute("id", index);
    
    return out;
}

/**
 * @author Niels Van Zandbergen
 * @param Parent
 * @returns {undefined}
 */
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

/**
 * @author Niels Van Zandbergen
 * @param Parent
 */
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

/**
 * @author Niels Van Zandbergen
 * @param Parent
 * @param index
 */
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

/**
 * @author Niels Van Zandbergen
 * @param Parent
 */
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

/**
 * @author Niels Van Zandbergen
 * @type {NodeListOf<Element>}
 */
const buttons = document.querySelectorAll(".addFieldStringsButton");
for(let i = 0; i < buttons.length; i++){
    buttons[i].addEventListener("click", function(){
        let id = this.getAttribute("id");
        console.log("ParentIdea will be: " + id);
        addClosedFieldPart(id);
    });
}

/***/ })

/******/ });
//# sourceMappingURL=closedfield.entry.js.map