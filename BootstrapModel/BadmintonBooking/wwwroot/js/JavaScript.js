document.addEventListener("DOMContentLoaded", function () {
    const weekRangeElement = document.getElementById("week-range");
    const prevButton = document.getElementById("prev-week");
    const nextButton = document.getElementById("next-week");
    const scheduleTable = document.getElementById("schedule").getElementsByTagName("thead")[0].getElementsByTagName("tr")[0];
    const cancelAllButton = document.getElementById("cancel-all");
    const totalPriceElement = document.getElementById("total-price");
    const type = document.getElementById("types");
    const hours = document.getElementById("total-hours");
    const weeks = document.getElementById("total-weeks");
    const form = document.querySelector("form");

    let currentStartDate = new Date();
    let scheduleData = [];

    form.addEventListener("keydown", function (event) {
        if (event.key === "Enter" && event.target.tagName !== "TEXTAREA") {
            event.preventDefault();
        }
    });

    if (weeks) {
        weeks.addEventListener("change", updateTotalPrice);
    }
    if (hours) {
        hours.addEventListener("input", updateTotalPrice);
    }
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
                    time: formatTime(hour),
                    booked: false
                });
            }
        }
    }

    function formatTime(hour) {
        const ampm = hour >= 12 ? 'PM' : 'AM';
        const formattedHour = hour % 12 || 12;
        return `${formattedHour}:00 ${ampm}`;
    }

    function updateWeekRange() {
        const startDate = new Date(currentStartDate);
        const endDate = new Date(startDate);
        endDate.setDate(endDate.getDate() + 6);
        weekRangeElement.textContent = `${formatDate(startDate)} - ${formatDate(endDate)}`;
        updateTableHeaders(startDate);
        initializeScheduleData();
        addEventListenersToBookableCells();
        fetchBookedTimeslots();
    }

    function formatDate(date) {
        const options = { month: 'short', day: 'numeric' };
        return date.toLocaleDateString('en-US', options);
    }

    function updateTableHeaders(startDate) {
        const today = new Date();
        today.setHours(0, 0, 0, 0);

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
            console.log([time, day]);
            const timeslot = scheduleData.find(slot => slot.date === day && slot.time === time);
            if (timeslot) {
                timeslot.booked = true;
            }
            sendBookingData({ time, date: day, booked: true });
            updateTotalPrice();
        }
    }

    function sendBookingData(slot) {
        fetch('/Manger/CreateBooking', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(slot),
        })
            .then(response => {
                if (response.status === 401) {
                    window.location.href = '/Account/Login';
                } else if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log('Success:', data);
            })
            .catch((error) => {
                console.error('Error:', error);
            });
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
            fetch('/Manager/Cancel', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                }
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    console.log('Success:', data);
                    // Handle success if needed
                })
                .catch(error => {
                    console.error('Error:', error);
                });

        });
        updateTotalPrice();
    }

    function getTimeAndDayFromCell(cell) {
        const rowIndex = cell.parentElement.rowIndex;
        const colIndex = cell.cellIndex;

        const time = document.querySelector(`#schedule tbody tr:nth-child(${rowIndex}) td:first-child`).textContent;
        const dayHeader = document.querySelector(`#schedule thead tr th:nth-child(${colIndex + 1})`).innerHTML;
        const day = dayHeader.split('<br>')[1].trim();

        return [time, day];
    }

    function addEventListenersToBookableCells() {
        const bookableCells = document.querySelectorAll(".bookable");
        bookableCells.forEach(cell => {
            cell.removeEventListener("click", clickBooking);
            cell.addEventListener("click", clickBooking);
        });
    }

    function updateTotalPrice() {
        fetch('/Manager/GetPrice')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(price => {
                const bookedCellsCount = document.querySelectorAll(".booked").length;
                const types = type.value;

                let totalPrice = 0;
                if (types === "Fixed") {
                    totalPrice = bookedCellsCount * price * parseInt(weeks.value);
                } else if (types === "Flexible") {
                    totalPrice = price * hours.value;
                } else {
                    totalPrice = bookedCellsCount * price;
                }

                totalPriceElement.textContent = totalPrice;
                document.getElementById("price-input").value = totalPrice;
            })
            .catch(error => console.error('Error:', error));

    }


    function fetchBookedTimeslots() {
        fetch('/Manager/GetBookSlots')
            .then(response => response.json())
            .then(data => {
                data.forEach(slot => {
                    const time = formatTo12Hour(slot.tsStart);
                    const date = formatToDate(slot.tsDate);

                    const cell = findCellByTimeAndDate(time, date);
                    if (cell) {
                        cell.classList.remove("bookable", "booked");
                        cell.textContent = "";
                    }
                });
                updateTotalPrice();
            })
            .catch(error => console.error('Error:', error));
    }

    function formatTo12Hour(time24) {
        const [hour, minute] = time24.split(':');
        const hourInt = parseInt(hour, 10);
        const ampm = hourInt >= 12 ? 'PM' : 'AM';
        const hour12 = hourInt % 12 || 12;
        return `${hour12}:${minute} ${ampm}`;
    }

    function formatToDate(date) {
        const dateObj = new Date(date);
        return dateObj.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
    }

    function findCellByTimeAndDate(time, date) {
        const rows = document.querySelectorAll("#schedule tbody tr");
        for (let row of rows) {
            const timeCell = row.cells[0].textContent;
            if (timeCell === time) {
                for (let i = 1; i < row.cells.length; i++) {
                    const dateCell = document.querySelector(`#schedule thead tr th:nth-child(${i + 1})`).innerHTML.split('<br>')[1];
                    if (dateCell === date) {
                        return row.cells[i];
                    }
                }
            }
        }
        return null;
    }

    if (prevButton && nextButton) {
        prevButton.addEventListener("click", function () {
            currentStartDate.setDate(currentStartDate.getDate() - 7);
            updateWeekRange();
        });

        nextButton.addEventListener("click", function () {
            currentStartDate.setDate(currentStartDate.getDate() + 7);
            updateWeekRange();
        });
    }

    cancelAllButton.addEventListener("click", cancelAllBookings);

    updateWeekRange();
});

