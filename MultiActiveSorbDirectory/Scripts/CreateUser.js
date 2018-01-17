//form status global variables
var firstNameLetter = "";
var firstName = "";
var middleInitial = "";
var lastNameLetter = "";
var lastName = "";
var listOfUsers;

$(document).ready(function () {
    $('#firstNameDiv').addClass("has-warning");
    $('#initialsDiv').addClass("has-warning");
    $('#lastNameDiv').addClass("has-warning");
    $('#mobilePhoneDiv').addClass("has-warning");
    $('#officeExtensionDiv').addClass("has-warning");
    $('#titleDiv').addClass("has-warning");
    $('#employeeIdDiv').addClass("has-warning");
    $('#aliasDiv').addClass("has-warning");
    $('#emailDiv').addClass("has-warning");
    $('#telephoneDiv').addClass("has-warning");
    $.ajax({
        url: "/CreateUser/populateSupervisors",
        type: "POST",
        dataType: 'json',
        success: function (response) {
            $.each(response, function (i, item) {
                $('#manager').append($("<option />").val(item.sAMAccountName).text(item.displayName));
            });
        }
    })
});

function hasErrors() {
    if ($('#firstNameDiv').hasClass("has-warning") || $('#firstNameDiv').hasClass("has-error")) { alert("this1"); return true; }
    if ($('#initialsDiv').hasClass("has-warning") || $('#initialsDiv').hasClass("has-error")) { alert("this2"); return true; }
    if ($('#lastNameDiv').hasClass("has-warning") || $('#lastNameDiv').hasClass("has-error")) { alert("this3"); return true; }
    if ($('#addressDiv').hasClass("has-warning") || $('#addressDiv').hasClass("has-error")) { alert("this4"); return true; }
    if ($('#cityDiv').hasClass("has-warning") || $('#cityDiv').hasClass("has-error")) { alert("this5"); return true; }
    if ($('#zipCodeDiv').hasClass("has-warning") || $('#zipCodeDiv').hasClass("has-error")) { alert("this6"); return true; }
    if ($('#mobilePhoneDiv').hasClass("has-warning") || $('#mobilePhoneDiv').hasClass("has-error")) { alert("this7"); return true; }
    if ($('#officeExtensionDiv').hasClass("has-warning") || $('#officeExtensionDiv').hasClass("has-error")) { alert("this8"); return true; }
    if ($('#titleDiv').hasClass("has-warning") || $('#titleDiv').hasClass("has-error")) { alert("this9"); return true; }
    if ($('#employeeIdDiv').hasClass("has-warning") || $('#employeeIdDiv').hasClass("has-error")) { alert("this10"); return true; }
    if ($('#aliasDiv').hasClass("has-warning") || $('#aliasDiv').hasClass("has-error")) { alert("this11"); return true; }
    if ($('#emailDiv').hasClass("has-warning") || $('#emailDiv').hasClass("has-error")) { alert("this12"); return true; }
    if ($('#telephoneDiv').hasClass("has-warning") || $('#telephoneDiv').hasClass("has-error")) { alert("this13"); return true; }
    if ($('#manager').val() == "") { alert("this14"); return true; }
    if ($('#department').val() == "") { alert("this15"); return true; }
    alert("Passed");
    return false;
}

function updateAlias() {
    $('#alias').val(firstNameLetter + middleInitial + lastNameLetter);
    if ($('#alias').val().length == 3) {
        $('#aliasDiv').removeClass("has-error");
        $('#aliasDiv').removeClass("has-warning");
    }
}
function updateEmail() {
    $('#email').val(firstNameLetter.toUpperCase() + lastName);
    if ($('#email').val().length !=0) {
        $('#emailDiv').removeClass("has-error");
        $('#emailDiv').removeClass("has-warning");
    }
}

$('#firstName').keyup(function () {
    var word = $('#firstName').val();
    if (word.length < 1) {
        $('#firstNameDiv').addClass("has-error");
        firstNameLetter = "";
        firstName = "";
    }
    else {
        $('#firstNameDiv').removeClass("has-error");
        $('#firstNameDiv').removeClass("has-warning");
        firstNameLetter = word[0].toLowerCase();
        firstName = word;
        updateAlias();
        updateEmail();
    }
});
$('#initials').keyup(function () {
    var word = $('#initials').val();
    if (word.length > 1) {
        $('#initialsDiv').removeClass("has-error");
        $('#initialsDiv').removeClass("has-warning");
        $('#initialsDiv').addClass("has-error");
    }
    else if (word.length == 0) {
        $('#initialsDiv').removeClass("has-error");
        $('#initialsDiv').removeClass("has-warning");
        $('#initialsDiv').addClass("has-warning");
        middleInitial = "";
    }
    else {
        middleInitial = word.toLowerCase();
        $('#initialsDiv').removeClass("has-error");
        $('#initialsDiv').removeClass("has-warning");
    }
    updateAlias();
    updateEmail();
});
$('#lastName').keyup(function () {
    var word = $('#lastName').val();
    if (word.length < 1) {
        $('#lastNameDiv').addClass("has-error");
        lastNameLetter = "";
        lastName = "";
    }
    else {
        $('#lastNameDiv').removeClass("has-warning");
        $('#lastNameDiv').removeClass("has-error");
        lastNameLetter = word[0].toLowerCase();
        lastName = word;
    }
    updateAlias();
    updateEmail();
});
$('#zipCode').keyup(function () {
    var word = $('#zipCode').val();
    if (word.length > 5) {
        $('#zipCodeDiv').removeClass("has-error");
        $('#zipCodeDiv').removeClass("has-warning");
        $('#zipCodeDiv').addClass("has-error");
    }
    else if (word.length < 5) {
        $('#zipCodeDiv').removeClass("has-error");
        $('#zipCodeDiv').removeClass("has-warning");
        $('#zipCodeDiv').addClass("has-error");
    }
    else {
        $('#zipCodeDiv').removeClass("has-error");
        $('#zipCodeDiv').removeClass("has-warning");
    }
});
$('#title').keyup(function () {
    var word = $('#title').val();
    if (word.length < 1) {
        $('#titleDiv').addClass("has-error");
    }
    else {
        $('#titleDiv').removeClass("has-error");
        $('#titleDiv').removeClass("has-warning");
    }
});
$('#mobile').keyup(function () {
    var word = $('#mobile').val();
    if (word.length < 1) {
        $('#mobilePhoneDiv').addClass("has-error");
    }
    else {
        $('#mobilePhoneDiv').removeClass("has-error");
        $('#mobilePhoneDiv').removeClass("has-warning");
    }
});
$('#officeExtension').keyup(function () {
    var word = $('#officeExtension').val();
    if (word.length < 1) {
        $('#officeExtensionDiv').addClass("has-error");
    }
    else {
        $('#officeExtensionDiv').removeClass("has-error");
        $('#officeExtensionDiv').removeClass("has-warning");
    }
});
$('#employeeId').keyup(function () {
    var word = $('#employeeId').val();
    if (word.length < 1) {
        $('#employeeIdDiv').addClass("has-error");
    }
    else {
        $('#employeeIdDiv').removeClass("has-error");
        $('#employeeIdDiv').removeClass("has-warning");
    }
});
$('#telephone').keyup(function () {
    var word = $('#telephone').val();
    if (word.length < 1) {
        $('#telephoneDiv').addClass("has-error");
    }
    else {
        $('#telephoneDiv').removeClass("has-error");
        $('#telephoneDiv').removeClass("has-warning");
    }
});