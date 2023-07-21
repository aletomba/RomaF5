$(document).ready(function () {
    $('#searchInput').keyup(function () {
        var searchText = $(this).val().toLowerCase();
        $('.productRow').each(function () {
            var productName = $(this).find('label').text().toLowerCase();
            if (productName.includes(searchText)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    // Agregar evento para manejar cambios en la cantidad
    $('.productRow select').change(function () {
        // Construir la lista de productos seleccionados
        var productosSeleccionados = [];
        $('.productRow').each(function () {
            var productoId = $(this).find('input[name$=".ProductoId"]').val();
            var cantidad = parseInt($(this).find('select').val());
            if (cantidad > 0) {
                productosSeleccionados.push({ productoId, cantidad });
            }
        });

        // Limpiar la lista de productos seleccionados
        $('#productosSeleccionados').empty();

        // Mostrar los productos seleccionados en la lista
        productosSeleccionados.forEach(function (producto) {
            var productoNombre = $('.productRow input[value="' + producto.productoId + '"]').closest('.productRow').find('label').text();
            $('#productosSeleccionados').append('<li>' + productoNombre + ' - Cantidad: ' + producto.cantidad + '</li>');
        });
    });
});
