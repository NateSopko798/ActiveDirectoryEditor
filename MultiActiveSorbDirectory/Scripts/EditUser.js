function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
}

//var _first = "";
//var _initial = "";
//var _last = "";
//var _address = "";
//var _city = "";
//var _state = "";
//var _zipCode = "";
//var _country = "";
//var _mobile = "";
//var _office = "";
//var _title = "";
//var _department = "";
//var _employeeId = "";
//var _alias = "";
var _manager = "";
//var _email = "";
//var _telephone = "";

function populate(first, initial, last, address, city, st, zip, country, mobile, office, title, 
    department, employeeid, alias, email, telephone, supervisor) {
    //_first = first;
    //_initial = initial;
    //_last = last;
    //_address = address;
    //_city = city;
    //_st = st;
    //_zip = zip;
    //_country = country;
    //_mobile = mobile;
    //_office = office;
    //_title = title;
    //_department = department;
    //_employeeid = employeeid;
    //_alias = alias;
    _manager = supervisor;
    //_email = email;
    //_telephone = telephone;

    $('#firstName').val(first);
    $('#initials').val(initial);
    $('#lastName').val(last);
    $('#address').val(address);
    $('#city').val(city);
    $('#state').val(st);
    $('#zipCode').val(zip);
    $('#country').val(country);
    $('#mobile').val(mobile);
    $('#officeExtension').val(office);
    $('#title').val(title);
    $('#department').val(department);
    $('#employeeId').val(employeeid);
    $('#alias').val(alias);
    $('#email').val(email);
    $('#telephone').val(telephone);
}

$(document).ready(function () {
    var user = getUrlParameter('alias');
    var managerAlias = "";
    if (user !== undefined) {
        $.ajax({
            url: "/EditUser/fillForm",
            type: "POST",
            data: { alias: user },
            dataType: 'json',
            success: function (response) {
                populate(response.givenName, response.initials, response.SN, response.streetAddress
                    , response.l, response.st, response.postalCode, response.c, response.mobile
                    , response.physicalDeliveryOfficeName, response.title, response.department
                    , response.employeeID, response.sAMAccountName, response.mailNickname
                    , response.telephoneNumber, response.managerAlias);
                managerAlias = response.managerAlias;
                $.ajax({
                    url: "/CreateUser/populateSupervisors",
                    type: "POST",
                    dataType: 'json',
                    success: function (response) {
                        response = response.sort(function (a, b) {
                            return a.displayName > b.displayName ? 1 : a.displayName < b.displayName ? -1 : 0;
                        });
                        $.each(response, function (i, item) {
                            $('#manager').append($("<option />").val(item.sAMAccountName).text(item.displayName));
                        });
                        $.ajax({
                            url: "/EditUser/fillManager",
                            type: "POST",
                            data: { manager: managerAlias },
                            dataType: 'json',
                            success: function (response) {
                                $('#manager').val(response.displayName);
                            }
                        });
                    }
                });
            }
        });
    }
});

$('#submitForm').click(function () {
    //do anything here before post
    var errors = hasErrors();
    if (errors !== "") {
        alert("Please fix " + errors + " box before submitting new user");
        return;
    }
    document.getElementById("editUserForm").submit();
});

function hasErrors() {
    if ($('#firstNameDiv').hasClass("has-warning") || $('#firstNameDiv').hasClass("has-error")) { return "First Name"; }
    if ($('#initialsDiv').hasClass("has-warning") || $('#initialsDiv').hasClass("has-error")) { return "Initial"; }
    if ($('#lastNameDiv').hasClass("has-warning") || $('#lastNameDiv').hasClass("has-error")) { return "Last Name"; }
    if ($('#addressDiv').hasClass("has-warning") || $('#addressDiv').hasClass("has-error")) { return "Street Address"; }
    if ($('#cityDiv').hasClass("has-warning") || $('#cityDiv').hasClass("has-error")) { return "City"; }
    if ($('#zipCodeDiv').hasClass("has-warning") || $('#zipCodeDiv').hasClass("has-error")) { return "Zip Code"; }
    if ($('#mobilePhoneDiv').hasClass("has-warning") || $('#mobilePhoneDiv').hasClass("has-error")) { return "Mobile Phone"; }
    if ($('#officeExtensionDiv').hasClass("has-warning") || $('#officeExtensionDiv').hasClass("has-error")) { return "Office Extension"; }
    if ($('#titleDiv').hasClass("has-warning") || $('#titleDiv').hasClass("has-error")) { return "Job Title"; }
    if ($('#employeeIdDiv').hasClass("has-warning") || $('#employeeIdDiv').hasClass("has-error")) { return "Employee ID"; }
    if ($('#aliasDiv').hasClass("has-warning") || $('#aliasDiv').hasClass("has-error")) { return "Alias"; }
    if ($('#emailDiv').hasClass("has-warning") || $('#emailDiv').hasClass("has-error")) { return "Email"; }
    if ($('#telephoneDiv').hasClass("has-warning") || $('#telephoneDiv').hasClass("has-error")) { return "Telephone"; }
    if ($('#manager').val() === "") { return "Manager"; }
    if ($('#department').val() === "") { return "Department"; }
    return "";
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
    else if (word.length === 0) {
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
$('#employeeId').keyup(function () {
    var word = $('#employeeId').val();
    if (word.length > 5) {
        $('#employeeIdDiv').removeClass("has-error");
        $('#employeeIdDiv').removeClass("has-warning");
        $('#employeeIdDiv').addClass("has-error");
    }
    else if (word.length < 5) {
        $('#employeeIdDiv').removeClass("has-error");
        $('#employeeIdDiv').removeClass("has-warning");
        $('#employeeIdDiv').addClass("has-error");
    }
    else {
        $('#employeeIdDiv').removeClass("has-error");
        $('#employeeIdDiv').removeClass("has-warning");
    }
});
$('#alias').keyup(function () {
    var word = $('#alias').val();
    if (word.length < 1) {
        $('#aliasDiv').addClass("has-error");
    }
    else {
        $('#aliasDiv').removeClass("has-error");
        $('#aliasDiv').removeClass("has-warning");
    }
});
$('#city').keyup(function () {
    var word = $('#city').val();
    if (word.length < 1) {
        $('#cityDiv').addClass("has-error");
    }
    else {
        $('#cityDiv').removeClass("has-error");
        $('#cityDiv').removeClass("has-warning");
    }
});