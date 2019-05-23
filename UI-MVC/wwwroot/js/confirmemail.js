console.log('Confirmemail has been loaded');

let count = 10;
let redirect = "~/";
let element = document.getElementById('countdown');

/**
 * @author Xander Veldeman
 * @documentation Xander  Veldeman
 * 
 * Timer van 10 seconden voor een redirect.
 * 
 * @returns {Promise<void>}
 */
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