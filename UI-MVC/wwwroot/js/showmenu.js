console.log('The Index bundle has been loaded')

var menu = document.getElementById('menu');
var buttonOpen = document.getElementById('activateMenu');
var buttonClose = document.getElementById('closeMenu');

buttonOpen.onclick = function() {
    console.log("clickOpen");
    menu.style = "display: flex";
};

buttonClose.onclick = function() {
    console.log("clickClose");
    menu.style = "display: none";
};