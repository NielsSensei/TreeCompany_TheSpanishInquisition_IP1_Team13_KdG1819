console.log('reportidea has been loaded');

var menu = document.getElementById('Reportmenu');
var buttonClose = document.getElementById('closeReport');
var aTag = document.getElementById('bevestigReport');

buttonClose.onclick = function() {
    console.log("clickClose");
    menu.style = "display: none";
};

var buttonsOpen = document.querySelectorAll("#activateReport");

console.log(buttonsOpen.length);

for(var i = 0; i < buttonsOpen.length; i++){
    buttonsOpen[i].addEventListener("click",function(){
        console.log("clickOpen");
        console.log(this.className);
        menu.style = "display: flex";

        var att = document.createAttribute("asp-route-idea");       
        att.value = this.className;                           
        aTag.setAttributeNode(att);
    },false);
}