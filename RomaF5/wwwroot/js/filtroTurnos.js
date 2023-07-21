document.addEventListener("DOMContentLoaded", function () {
    var tablaVentas = document.getElementById("tablaTurnos");

    var fechaFiltroInput = document.getElementById("fechaFiltro");
    var btnFiltrar = document.getElementById("btnFiltrar");
    var btnLimpiarFiltro = document.getElementById("btnLimpiarFiltro");

    btnFiltrar.addEventListener("click", function () {
        var fechaSeleccionada = fechaFiltroInput.value;
        filtrarVentasPorFecha(fechaSeleccionada);
        calcularTotalDia();
    });

    btnLimpiarFiltro.addEventListener("click", function () {
        limpiarFiltro();
        calcularTotalDia();
    });

    function filtrarVentasPorFecha(fecha) {
        var filasVentas = tablaVentas.querySelectorAll("tbody tr");

        for (var i = 0; i < filasVentas.length; i++) {
            var fechaVenta = filasVentas[i].getAttribute("data-fecha");

            if (fecha === "") {
                filasVentas[i].style.display = "";
            } else {
                if (fechaVenta === fecha) {
                    filasVentas[i].style.display = "";
                } else {
                    filasVentas[i].style.display = "none";
                }
            }
        }
    }

    function limpiarFiltro() {
        fechaFiltroInput.value = "";
        filtrarVentasPorFecha("");
    }

    function calcularTotalDia() {
        var filasVentasVisibles = tablaVentas.querySelectorAll("tbody tr:not([style='display: none;'])");
        var totalDia = 0;

        for (var i = 0; i < filasVentasVisibles.length; i++) {
            var ventaTotal = parseFloat(filasVentasVisibles[i].querySelector("td:nth-child(4)").textContent);
            totalDia += ventaTotal;
        }

        var totalDiaElement = document.getElementById("totalDia");
        totalDiaElement.textContent = "Total del Día: " + totalDia.toFixed(2);
    }
});


