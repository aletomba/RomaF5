$(document).ready(function () {
    // Filtrar ventas por fecha
    $('#btnFiltrar').click(function () {
        var fechaFiltro = $('#fechaFiltro').val();
        $.ajax({
            url: '/Ventas/FiltrarPorFecha',
            type: 'GET',
            data: { fecha: fechaFiltro },
            success: function (result) {
                $('#tablaTurnos tbody').empty();
                if (Array.isArray(result)) {
                    $.each(result, function (index, venta) {
                        var productosVendidos = '';
                        $.each(venta.VentasProductos, function (index, vp) {
                            if (vp.Producto && vp.Producto.Nombre) {
                                productosVendidos += vp.Producto.Nombre + '<br />';
                            }
                        });
                        $('#tablaTurnos tbody').append('<tr><td>' + venta.Fecha + '</td><td>' + (venta.Cliente ? venta.Cliente.Nombre : '') + '</td><td>' + productosVendidos + '</td><td>' + venta.Total + '</td><td><a href="/Ventas/Edit/' + venta.Id + '">Editar</a> | <a href="/Ventas/Details/' + venta.Id + '">Detalles</a> | <a href="/Ventas/Delete/' + venta.Id + '">Borrar</a></td></tr>');
                    });

                    // Calcular y mostrar el total de las ventas para ese día
                    var totalDia = result.reduce(function (total, venta) {
                        return total + venta.Total;
                    }, 0);
                    $('#totalDia').text('Total del día: ' + totalDia.toFixed(2));
                } else {
                    console.error('El resultado no es un array:', result);
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });

    // Limpiar filtro
    $('#btnLimpiarFiltro').click(function () {
        $('#fechaFiltro').val('');
        $('#tablaTurnos tbody').empty();
        $('#totalDia').text('');
    });
});



