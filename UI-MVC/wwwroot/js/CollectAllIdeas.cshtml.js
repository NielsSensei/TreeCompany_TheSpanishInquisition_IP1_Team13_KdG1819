document.addEventListener("load", init, false);

function init(){
    var comboFilter = document.getElementById('comboFilter');
    comboFilter.addEventListener("onchange",CollectAllIdeas(comboFilter.value), false);
    
    CollectAllIdeas('all');
}

function CollectAllIdeas(filter){
    var ideasTable = document.getElementById('ideasTable');

    for(var i = table.rows.length - 1; i > 0; i--)
    {
        table.deleteRow(i);
    }
    
    
}

/*
Hoe converteer ik dit naar deze file, er is iets mis met de functie based on $ voor een reden.
<script type="text/javascript">  
    $(document).ready(function () {  
        $("#comboFilter").change(function () {  
            $("#ideasfilter").empty();  
             $.ajax({
            type: 'POST',
            url: '@Url.Action("CollectIdeas")',
            dataType: 'json',
            data: { id: $("#comboFilter").val() },
            success: function(ideas) {
                $("#ideasTable").append(
                    '<tr>'+
                    '<th>Id</th>' +
                    '<th>CentraleVraag</th>' +
                    '<th>User</th>' +
                    '<th>Review door Moderator nodig?</th>' +
                    '<th>Review door Admin nodig?</th>' +
                    '<th>Titel</th>' +
                    '<th>Aantal Stemmen</th>' +
                    '<th>Aantal Shares</th>' +
                    '<th>Aantal Retweets</th>' +
                    '</tr>'
                );
                ideas.forEach(idea => {
                    $("#ideasTable").append(
                    '<tr>' +
                    '<td>' + idea.Id + '</td>' +
                    '<td>' + idea.IdeaQuestion.Id + '</td>' +
                    '<td>' + idea.User.Id + '</td>' +
                    '<td>' + idea.Reported + '</td>' +
                    '<td>' + idea.ReviewByAdmin + '</td>' +
                    '<td>' + idea.Title + '</td>' +
                    '<td>' + idea.VoteCount + '</td>' +
                    '<td>' + idea.ShareCount + '</td>' +
                    '<td>' + idea.RetweetCount + '</td>' +
                    '</tr>'
                );
            });
            },
            error: function(ex) {
                console.log('Failed to retrieve ideas.' + ex);
            }
        });
        return false;
    });
    });  
</script>
 */

       
