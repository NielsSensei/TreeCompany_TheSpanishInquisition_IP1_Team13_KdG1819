console.log('Confirmemail has been loaded');

var count = 10;
var redirect = "~/";
var element = document.getElementById('countdown');

window.onload = async function () {
    //console.log(count);
    for (; count > 0; count--) {
        //console.log(count);
        element.innerHTML = count;
        await sleep(1000);
    }
    window.location.href = "/Identity/Account/Login";
};

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}