window.addEventListener('DOMContentLoaded', () => {
    const alert = document.getElementById('state-alert');
    if (alert) {
        setTimeout(() => {
            alert.classList.remove('show');
            setTimeout(() => {
                alert.remove();
            }, 1000);
        }, 3000);
    }
});

document.addEventListener("DOMContentLoaded", function () {
    const fileInput = document.getElementById("ImageFile");
    const fileNameDisplay = document.getElementById("fileNameDisplay");

    if (fileInput && fileNameDisplay) {
        fileInput.addEventListener("change", function () {
            if (fileInput.files.length > 0) {
                fileNameDisplay.value = fileInput.files[0].name;
            } else {
                fileNameDisplay.value = "";
            }
        });
    }
});

document.addEventListener("DOMContentLoaded", function () {
    const input = document.querySelector("#searchInput");
    if (input) {
        input.addEventListener("keyup", function () {
            const value = this.value.toLowerCase();
            document.querySelectorAll("#materialTable tbody tr").forEach(function (row) {
                row.style.display = row.textContent.toLowerCase().includes(value) ? "" : "none";
            });
        });
    }
});


function initBarChart(canvasId, labels, data, labelText = "Počet", barColor = "#f39c12") {
    const ctx = document.getElementById(canvasId)?.getContext("2d");
    if (!ctx) return;

    new Chart(ctx, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [{
                label: labelText,
                data: data,
                backgroundColor: barColor,
                borderWidth: 1
            }]
        },
        options: {
            indexAxis: 'x',
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

function initMaterialChart(chartData) {
    const labels = chartData.map(x => x.materialName);
    const data = chartData.map(x => x.writtenOffCount);
    initBarChart("materialChart", labels, data, "Počet odepsání", "#f39c12");
}

function initTechnicianChart(chartData) {
    const labels = chartData.map(x => x.technicianName);
    const data = chartData.map(x => x.writtenOffCount);
    initBarChart("technicianChart", labels, data, "Počet odepsání", "#f39c12");
}