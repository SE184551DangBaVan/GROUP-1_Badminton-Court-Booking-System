/* Date chooser */
$document.addEventListener("DOMContentLoaded", function () {
    const weekRangeElement = document.getElementById("week-range");
    const prevButton = document.getElementById("prev-week");
    const nextButton = document.getElementById("next-week");
    const scheduleTable = document.getElementById("schedule").getElementsByTagName("thead")[0].getElementsByTagName("tr")[0];
    const cancelAllButton = document.getElementById("cancel-all");
    const totalPriceElement = document.getElementById("total-price");

    let currentStartDate = new Date();

    function updateWeekRange() {
        const startDate = new Date(currentStartDate);
        const endDate = new Date(startDate);
        endDate.setDate(endDate.getDate() + 6);
        weekRangeElement.textContent = `${formatDate(startDate)} - ${formatDate(endDate)}`;
        updateTableHeaders(startDate);
        addEventListenersToBookableCells();
        updateTotalPrice();
    }

    function formatDate(date) {
        const options = { month: 'short', day: 'numeric' };
        return date.toLocaleDateString('en-US', options);
    }

    function updateTableHeaders(startDate) {
        const today = new Date();
        today.setHours(0, 0, 0, 0); // Reset time to the start of the day

        for (let i = 1; i <= 7; i++) {
            const header = scheduleTable.cells[i];
            const date = new Date(startDate);
            date.setDate(date.getDate() + (i - 1));
            const dayName = date.toLocaleDateString('en-US', { weekday: 'short' });
            header.innerHTML = `${dayName}<br>${formatDate(date)}`;

            const dayIndex = i - 1;
            if (date < today) {
                blankOutColumn(dayIndex);
            } else {
                enableColumn(dayIndex);
            }
        }
    }

    function blankOutColumn(dayIndex) {
        const rows = document.querySelectorAll("#schedule tbody tr");
        rows.forEach(row => {
            row.cells[dayIndex + 1].innerHTML = "";
            row.cells[dayIndex + 1].classList.remove("bookable", "booked");
        });
    }

    function enableColumn(dayIndex) {
        const rows = document.querySelectorAll("#schedule tbody tr");
        rows.forEach(row => {
            const cell = row.cells[dayIndex + 1];
            if (!cell.classList.contains("booked")) {
                cell.classList.add("bookable");
                cell.textContent = "Book Now";
            }
        });
    }

    function clickBooking(event) {
        const cell = event.target;
        if (cell.classList.contains("bookable") && !cell.classList.contains("booked")) {
            cell.classList.add("booked");
            cell.classList.remove("bookable");
            cell.textContent = "Booked";
            updateTotalPrice();
        }
    }

    function cancelAllBookings() {
        const bookedCells = document.querySelectorAll(".booked");
        bookedCells.forEach(cell => {
            cell.classList.remove("booked");
            cell.classList.add("bookable");
            cell.textContent = "Book Now";
        });
        updateTotalPrice();
    }

    function addEventListenersToBookableCells() {
        const bookableCells = document.querySelectorAll(".bookable");
        bookableCells.forEach(cell => {
            cell.removeEventListener("click", clickBooking); // Prevent duplicate listeners
            cell.addEventListener("click", clickBooking);
        });
    }

    function updateTotalPrice() {
        const bookedCells = document.querySelectorAll(".booked").length;
        const totalPrice = bookedCells * 10;
        totalPriceElement.textContent = totalPrice;
    }

    prevButton.addEventListener("click", function () {
        currentStartDate.setDate(currentStartDate.getDate() - 7);
        updateWeekRange();
    });

    nextButton.addEventListener("click", function () {
        currentStartDate.setDate(currentStartDate.getDate() + 7);
        updateWeekRange();
    });

    cancelAllButton.addEventListener("click", cancelAllBookings);

    updateWeekRange();
});