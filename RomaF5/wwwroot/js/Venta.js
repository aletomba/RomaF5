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
    $('.productRow .form-control').on('input', function () {
        // Limpiar la lista de productos seleccionados
        $('#productosSeleccionados').empty();

        // Calcular el precio total
        var precioTotal = 0;

        $('.productRow').each(function () {
            var productoId = $(this).data('producto-id');
            var cantidad = parseFloat($(this).find('.form-control').val()) || 0;
            var precioUnitario = parseFloat($(this).data('precio'));
            var precioProducto = cantidad * precioUnitario;

            if (cantidad > 0) {
                precioTotal += precioProducto;

                // Mostrar los productos seleccionados en la lista
                var productoNombre = $(this).find('label').text();
                $('#productosSeleccionados').append('<li>' + productoNombre + ' - Cantidad: ' + cantidad + ' - Precio Unitario: $' + precioUnitario.toFixed(2) + '</li>');
            }
        });

        // Actualizar el precio total en el carrito en el HTML
        $('#totalPrecioCarrito').text( precioTotal.toFixed(2));
    });


});

