document.addEventListener("load",init, false);

function init(){
    var comboBox = document.querySelector('select');
    comboBox.addEventListener("onchange",,false);
    
    initializeIdeaTable();
    loadIdeas("all")
}

function initializeIdeaTable() {
    var ideaTabel = document.querySelector('table');
    ideaTabel.append('<tr><th>Id</th><th>CentraleVraag</th><th>User</th><th>Review door Moderator nodig?</th>' +
        '<th>Review door Admin nodig?</th><th>Titel</th><th>Aantal Stemmen</th><th>Aantal Shares</th>' +
        '<th>Aantal Retweets</th></tr>');
}

function loadIdeas(filter){
    
}