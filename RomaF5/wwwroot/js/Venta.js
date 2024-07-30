//// Buscador de productos
//$('#searchInput').on('input', function () {
//    var searchTerm = $(this).val().toLowerCase();
//    $('.productRow').each(function () {
//        var productName = $(this).find('label').text().toLowerCase();
//        if (productName.includes(searchTerm)) {
//            $(this).show();
//        } else {
//            $(this).hide();
//        }
//    });
//});

//// Cálculo del total del carrito y actualización de la lista de productos seleccionados
//$('.col-md-6').on('change', '.productRow select', function () {
//    // Limpiar la lista de productos seleccionados antes de agregar los nuevos
//    $('#VentasProductos').empty();

//    // Recorrer cada producto para agregarlo a la lista VentasProductos en el formulario
//    $('.productRow').each(function () {
//        var productoId = $(this).find('input[name$=".ProductoId"]').val();
//        var cantidad = parseInt($(this).find('select').val());
//        if (cantidad > 0) {
//            // Agregar el producto a la lista VentasProductos en el formulario
//            $('<input>').attr({
//                type: 'hidden',
//                name: 'VentasProductos[' + productoId + '].ProductoId',
//                value: productoId
//            }).appendTo('#VentasProductos');

//            $('<input>').attr({
//                type: 'hidden',
//                name: 'VentasProductos[' + productoId + '].Cantidad',
//                value: cantidad
//            }).appendTo('#VentasProductos');
//        }
//    });

//    // Código para calcular el total del carrito y actualizar la interfaz de usuario
//    var total = 0;
//    $('.productRow').each(function () {
//        var precioUnitario = parseFloat($(this).find('td:nth-child(2)').text());
//        var cantidad = parseInt($(this).find('select').val());
//        total += precioUnitario * cantidad;
//    });

//    $('#totalLabel').text('Total: $' + total.toFixed(2));
//});









// Cálculo del total del carrito
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
$('.col-md-6').on('change', '.productRow select', function () {

 
    var productosSeleccionados = [];
    $('.productRow').each(function () {
        var productoId = $(this).find('input[name$=".ProductoId"]').val();
        var cantidad = parseInt($(this).find('select').val());
        var precio = parseFloat($(this).data('precio'));
        if (cantidad > 0) {
       
            productosSeleccionados.push({ productoId, cantidad, precio });
          
        }
    });

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
});



