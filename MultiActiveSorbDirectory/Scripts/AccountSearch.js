

var $rows = $('#userTable tr').not('thead tr');
$('#search').keyup(function () {
    var val = $.trim($(this).val()).replace(/ +/g, ' ').toLowerCase();

    $rows.show().filter(function () {
        var text = $(this).text().replace(/\s+/g, ' ').toLowerCase();
        return !~text.indexOf(val);
    }).hide();
});

$('#userTable tbody').on('click', 'tr', function () {
    var table = $(this).closest("table");
    $("tr", table).removeClass("highlight");
    $(this).addClass("highlight");
    //window.location.href = $(this).data('');
    alert($(this).find("td:first").text());
    //setTableValues();
});

//$(function () {
//    var userTable = $('#userTable').DataTable({
//        "dom": '<"top"i>rt<"bottom"flp><"clear">',
//        "bSort": true
//    });
//});