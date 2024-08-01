$(document).ready(function () {
    $('#btnUpdateCourt').click(function (event) {
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
            return false; // Exit function if validation fails
        }
        if ($("#CoInfo").val().trim() == "") {
            alert("Court info should not be empty");
            isValid = false;
            return false; // Exit function if validation fails
        }
        var price = $("#CoPrice").val().trim();
        if (price === "") {
            alert("Price should not be empty");
            isValid = false;
            return false; // Exit function if validation fails

        } else if (isNaN(price)) {
            alert("Price is invalid");
            isValid = false;
            return false; // Exit function if validation fails
        }
        else if (parseFloat(price) <= 0 ) {
            alert("Price should be more than 0");
            isValid = false;
            return false; // Exit function if validation fails
        }
        return isValid;
    }
});