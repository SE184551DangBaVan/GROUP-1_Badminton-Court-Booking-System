$(document).ready(function () {
    $('#customAddressCheckbox').change(function () {
        if ($(this).is(':checked')) {
            $('#CoAddressDropdown').removeAttr('required').hide();
            $('#CoAddressTextbox').attr('required', 'required').show();
        } else {
            $('#CoAddressTextbox').removeAttr('required').hide();
            $('#CoAddressDropdown').attr('required', 'required').show();
        }
    });

    $('form').submit(function (event) {
        var isDropdownVisible = $('#CoAddressDropdown').is(':visible');
        var dropdownValue = $('#CoAddressDropdown').val();
        var textboxValue = $('#CoAddressTextbox').val();

        if (isDropdownVisible && !dropdownValue) {
            event.preventDefault();
            alert("Please select an address from the dropdown.");
        } else if (!isDropdownVisible && !textboxValue) {
            event.preventDefault();
            alert("Please enter an address in the textbox.");
        }
    });
});
