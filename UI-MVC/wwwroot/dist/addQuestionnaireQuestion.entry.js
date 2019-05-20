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
/******/ 	return __webpack_require__(__webpack_require__.s = "./wwwroot/js/addQuestionnaireQuestion.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./wwwroot/js/addQuestionnaireQuestion.js":
/*!************************************************!*\
  !*** ./wwwroot/js/addQuestionnaireQuestion.js ***!
  \************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

let modalbody = document.querySelector(".addQQuestion");
let dropdown = document.getElementById("QuestionType");
let optionsContainer = document.querySelector(".addOptionsContainer");
let dropdownvalue = dropdown.options[dropdown.selectedIndex].value;
let optionsButton = document.querySelector(".optionsButton");
let containerToPutOptionsIn = document.querySelector(".optionsContainer");
let addedOptions = 0;

console.log("AddQuestionnaireQuestion active!");
console.log(dropdownvalue);

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

/***/ })

/******/ });
//# sourceMappingURL=addQuestionnaireQuestion.entry.js.map