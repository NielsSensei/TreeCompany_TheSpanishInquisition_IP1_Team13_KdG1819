import 'jquery'
import 'jquery-validation'
import 'jquery-validation-unobtrusive'

console.log('JQuery validation bundle has been loaded');

$().ready(function () {
    $("#ProjectForm").validate({
        
        rules: {
            Title: "required"
        },
        messages: {
            Title: "Vul aub de titel in"
        }
        
        
    })
});