$(document).ready(function () {

    $('#searchInput').on('input', function () {
        var searchTerm = $(this).val().toLowerCase();
        $('.productRow').each(function () {
            var productName = $(this).find('label').text().toLowerCase();
            if (productName.includes(searchTerm)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
    
    // Obtener el tipo de cliente seleccionado
    var clienteTipo = $('#ClienteId option:selected').text();
    console.log('clienteTipo:', clienteTipo);
    // Agregar evento change al dropdown de clientes
    $('#ClienteId').change(function () {
        console.log('Cliente cambiado');
        clienteTipo = $(this).find('option:selected').text();
        console.log('clienteTipo:', clienteTipo);
        actualizarPrecios();
    });

    // Agregar evento change a las cantidades de productos
    $('.productRow select').change(function () {
        actualizarPrecios();
    });

    // Función para actualizar precios
    function actualizarPrecios() {
        var productosSeleccionados = [];
        console.log('Productos seleccionados:', productosSeleccionados);
        $('.productRow').each(function () {
            var productoId = $(this).find('input[name$=".ProductoId"]').val();
            var cantidad = parseInt($(this).find('select').val());
            var precioMinorista = parseFloat($(this).data('precio-minorista'));
            var precioMayorista = parseFloat($(this).data('precio-mayorista'));
            var precio = clienteTipo === 'Minorista' ? precioMinorista : precioMayorista;
            if (cantidad > 0) {
                productosSeleccionados.push({ productoId, cantidad, precio });
            }
        });

        // Actualizar la lista de productos seleccionados y el total
        $('#productosSeleccionados').empty();
        productosSeleccionados.forEach(function (producto) {
            var productoNombre = $('.productRow input[value="' + producto.productoId + '"]').closest('.productRow').find('label').text();
            $('#productosSeleccionados').append('<li>' + productoNombre + ' - Cantidad: ' + producto.cantidad + '</li>');
        });

        var total = 0;
        productosSeleccionados.forEach(function (producto) {
            total += producto.precio * producto.cantidad;
        });
        $('#totalLabel').text('Total: $' + total.toFixed(2));

        // Actualizar campos ocultos
        var productosJSON = JSON.stringify(productosSeleccionados);
        $('#productos').val(productosJSON);
        $('#total').val(total.toFixed(2));
    }
});



