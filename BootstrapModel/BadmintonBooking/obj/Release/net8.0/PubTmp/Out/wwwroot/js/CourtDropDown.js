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
    $('#btnCreateCourt').click(function (event) {
        // Prevent form submission if validation fails
        if (!ValidationForm()) {
            event.preventDefault();
        }
    });
    function ValidationForm() {
        var isValid = true;
        if ($("#CoName").val().trim() === "") {
            alert("Court name should not be empty");
            isValid = false;
            return false;
        }
        if ($("#CoInfo").val().trim() == "") {
            alert("Court info should not be empty");
            isValid = false;
            return false;
        }
        var price = $("#CoPrice").val().trim();
        if (price === "") {
            alert("Price should not be empty");
            isValid = false;
            return false;

        } else if (isNaN(price)) {
            alert("Price is invalid");
            isValid = false;
            return false;
        }
        else if (parseFloat(price) <= 0) {
            alert("Price should be more than 0");
            isValid = false;
            return false;
        }
        // Additional validation can be added here
        return isValid;
    }
});
