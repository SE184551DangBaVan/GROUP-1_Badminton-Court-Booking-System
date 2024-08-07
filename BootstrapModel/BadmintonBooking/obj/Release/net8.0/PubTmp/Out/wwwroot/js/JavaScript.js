﻿document.addEventListener("DOMContentLoaded", function () {
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
    const confirmButton = document.getElementById("payment");
    const confirmForm = document.getElementById("payment-form");

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
        const startDate = new Date(currentStartDate);
        const today = new Date();
        const currentHour = today.getHours();
        for (let i = 0; i < 7; i++) {
            const date = new Date(startDate);
            date.setDate(startDate.getDate() + i);
            const day = date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });

            for (let hour = 9; hour <= 20; hour++) {
                const time = formatTime(hour);
                const booked = scheduleData.some(slot => slot.date === day && slot.time === time && slot.booked);
                const isToday = date.toDateString() === today.toDateString();
                const cell = findCellByTimeAndDate(time, day);
                if (cell) {
                    if (isToday && hour < currentHour) {
                        cell.classList.remove("bookable", "booked");
                        cell.textContent = "";
                    } else if (booked) {
                        cell.classList.add("booked");
                        cell.classList.remove("bookable");
                        cell.textContent = "Booked";
                    }
                }

                scheduleData.push({
                    date: day,
                    time: time,
                    booked: booked
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
        const rows = document.querySelectorAll("#schedule tbody tr");
        rows.forEach(row => {
            for (let i = 1; i < row.cells.length; i++) {
                const cell = row.cells[i];
                cell.classList.remove("booked", "bookable");
                cell.textContent = "";
            }
        });


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

    async function clickBooking(event) {
        const cell = event.target;
        console.log("Cell clicked:", cell);

        if (cell.classList.contains("bookable") && !cell.classList.contains("booked")) {
            const [time, day] = getTimeAndDayFromCell(cell);

            cell.classList.add("booked");
            cell.classList.remove("bookable");
            cell.textContent = "Booked";

            try {
                const success = await sendBookingData({ time, date: day, booked: true });
                if (success) {
                    scheduleData.push({ date: day, time: time, booked: true });
                } else {
                    alert("It has already been booked!");
                    cell.classList.remove("booked", "booked");
                    cell.textContent = "";
                }
            } catch (error) {
                console.error("Booking error:", error);
                alert("An error occurred while booking. Please try again.");
                return;
            }
            updateTotalPrice();
        }
    }

    function sendBookingData(slot) {
        return fetch('/Manager/CreateBooking', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(slot),
        })
            .then(response => {
                if (response.status === 401) {
                    window.location.href = '/Account/Login';
                    return false;
                } else if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                if (data.success === false) {
                    return false;
                } else {
                    console.log('Success:', data);
                    return true;
                }
            })
            .catch((error) => {
                console.error('Error:', error);
                return false; // Return false on error
            });
    }

    function cancelAllBookings() {
        const bookedCells = document.querySelectorAll(".booked");
        bookedCells.forEach(cell => {
            cell.classList.remove("booked");
            cell.classList.add("bookable");
            cell.textContent = "Book Now";
            scheduleData.forEach(slot => slot.booked = false);

            fetch('/Booking/Cancel', {
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

                let totalPrice = 0;
                totalPrice = bookedCellsCount * price;

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
    function confirmClick(event) {

        var confirmMessage = "Are you certain you want to proceed with your bookings?";

        if (!confirm(confirmMessage)) {
            event.preventDefault();
            return;
        }

        // Check if there are any booked cells
        const bookedCells = document.querySelectorAll(".booked");
        if (bookedCells.length === 0) {
            alert("Please book before continuing!");
            event.preventDefault();
            return;
        }
        confirmForm.submit();
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

    confirmButton.addEventListener("click", confirmClick);


    cancelAllButton.addEventListener("click", cancelAllBookings);

    updateWeekRange();
});




