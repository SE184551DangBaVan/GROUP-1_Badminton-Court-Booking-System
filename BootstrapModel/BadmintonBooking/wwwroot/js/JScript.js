/* Date chooser */
document.addEventListener("DOMContentLoaded", function () {
    const weekRangeElement = document.getElementById("week-range");
    const prevButton = document.getElementById("prev-week");
    const nextButton = document.getElementById("next-week");
    const scheduleTable = document.getElementById("schedule").getElementsByTagName("thead")[0].getElementsByTagName("tr")[0];
    const cancelAllButton = document.getElementById("cancel-all");
    const totalPriceElement = document.getElementById("total-price");

    let currentStartDate = new Date('2024-05-30');
    let scheduleData = [];

    function initializeScheduleData() {
        scheduleData = [];
        const startDate = new Date(currentStartDate);
        for (let i = 0; i < 7; i++) {
            const date = new Date(startDate);
            date.setDate(startDate.getDate() + i);
            const day = date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });

            for (let hour = 9; hour <= 20; hour++) {
                scheduleData.push({
                    date: day,
                    time: `${hour}:00`,
                    booked: false
                });
            }
        }
    }

    function updateWeekRange() {
        const startDate = new Date(currentStartDate);
        const endDate = new Date(startDate);
        endDate.setDate(endDate.getDate() + 6);
        weekRangeElement.textContent = `${formatDate(startDate)} - ${formatDate(endDate)}`;
        updateTableHeaders(startDate);
        initializeScheduleData();
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

            // Update schedule data
            const [time, day] = getTimeAndDayFromCell(cell);
            const timeslot = scheduleData.find(slot => slot.date === day && slot.time === time);
            if (timeslot) {
                timeslot.booked = true;
            }

            updateTotalPrice();
        }
    }

    function cancelAllBookings() {
        const bookedCells = document.querySelectorAll(".booked");
        bookedCells.forEach(cell => {
            cell.classList.remove("booked");
            cell.classList.add("bookable");
            cell.textContent = "Book Now";

            // Update schedule data
            const [time, day] = getTimeAndDayFromCell(cell);
            const timeslot = scheduleData.find(slot => slot.date === day && slot.time === time);
            if (timeslot) {
                timeslot.booked = false;
            }
        });
        updateTotalPrice();
    }

    function getTimeAndDayFromCell(cell) {
        const rowIndex = cell.parentElement.rowIndex - 1; // Adjust for header row
        const colIndex = cell.cellIndex - 1; // Adjust for time column

        const time = document.querySelector(`#schedule tbody tr:nth-child(${rowIndex + 1}) td:first-child`).textContent;
        const day = document.querySelector(`#schedule thead tr th:nth-child(${colIndex + 2})`).textContent.split('<br>')[1];

        return [time, day];
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

