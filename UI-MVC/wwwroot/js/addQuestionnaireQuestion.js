let modalbody = document.querySelector(".addQQuestion");
let dropdown = document.getElementById("QuestionType");
let optionsButton = document.querySelector(".addOption");
let dropdownvalue = dropdown.options[dropdown.selectedIndex].value;


console.log("DropdownId" + dropdownId);

console.log(dropdownvalue);





function requiresOptions() {
    dropdownvalue = dropdown.options[dropdown.selectedIndex].value;
    console.log(dropdownvalue);

    if (dropdownvalue === "SINGLE" || dropdownvalue === "MULTI" || dropdownvalue === "DROP") {
        console.log(dropdownvalue);
        return true;
    } else return false;
}

optionsButton.addEventListener("click", requiresOptions);



