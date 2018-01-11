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
    $(this).addClass("highlight");
    //bootbox.alert("This is the default alert!\nAlso: " + $(this).find("td:first").text());
    var modal = bootbox.dialog({
        message: $(".form-content").html(),
        title: "Your awesome modal",
        buttons: [
          {
              label: "Save",
              className: "btn btn-primary pull-left",
              callback: function () {

                  alert($('form #email').val());

                  return false;
              }
          },
          {
              label: "Close",
              className: "btn btn-default pull-left",
              callback: function () {
                  console.log("just do something on close");
              }
          }
        ],
        show: false,
        onEscape: function () {
            modal.modal("hide");
        }
    });

    modal.modal("show");
    $("tr", table).removeClass("highlight");
});