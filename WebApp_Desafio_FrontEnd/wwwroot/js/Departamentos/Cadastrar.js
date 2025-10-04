$(document).ready(function () {

    $('#btnCancelar').click(function () {
        Swal.fire({
            html: "Deseja cancelar essa operação? O registro não será salvo.",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: 'Sim, cancelar!',
            cancelButtonText: 'Não'
        }).then(function (result) {
            if (result.value) {
                // Redireciona para a tela de listagem
                window.location.href = config.contextPath + 'Departamentos/Listar';
            }
        });
    });

    $('#btnSalvar').click(function () {

        if ($('#form').valid() != true) {
            FormularioInvalidoAlert();
            return;
        }

        let departamento = SerielizeForm($('#form'));
        let url = $('#form').attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: departamento,
            success: function (result) {
                Swal.fire({
                    type: result.Type,
                    title: result.Title,
                    text: result.Message,
                }).then(function () {
                    window.location.href = config.contextPath + result.Controller + '/' + result.Action;
                });
            },
            error: function (result) {
                let errorResponse = result.responseJSON;
                Swal.fire({
                    text: errorResponse.message,
                    confirmButtonText: 'OK',
                    icon: 'error'
                });
            }
        });
    });
});