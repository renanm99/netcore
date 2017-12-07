$(document).ready(function() {
    
    var yql = "http://g1.globo.com/dynamo/rss2.xml";
    
    $.getJSON(yql, function(res) {
        console.log(res);   
    }, "jsonp");
    
});