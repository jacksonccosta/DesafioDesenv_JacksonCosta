function SerielizeForm($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

function FormularioInvalidoAlert() {
    Swal.fire({
        text: 'Por favor, preencha todos os campos obrigatórios.',
        confirmButtonText: 'OK',
        icon: 'warning'
    });
}