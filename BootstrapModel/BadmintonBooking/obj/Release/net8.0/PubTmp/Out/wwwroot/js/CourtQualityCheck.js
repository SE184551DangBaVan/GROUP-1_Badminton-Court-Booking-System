document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('survey-form');
    const conditions = ['CdSurfaceCondition', 'CdLightningCondition', 'CdNetCondition', 'CdCleanlinessCondtion'];

    function calculateOverallCondition() {
        let total = 0;
        let count = 0;

        conditions.forEach(condition => {
            const selected = document.querySelector(`input[name="${condition}"]:checked`);
            if (selected) {
                total += parseInt(selected.value);
                count++;
            }
        });

        if (count === conditions.length) {
            const average = Math.round(total / count);
            document.getElementById('CdOverallCondition').value = average;
            return true;
        } else {
            alert('Please rate all conditions before submitting.');
            return false;
        }
    }

    form.addEventListener('submit', function (event) {
        if (!calculateOverallCondition()) {
            event.preventDefault();
        }
    });
});