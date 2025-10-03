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
                    type: result.type,
                    title: result.title,
                    text: result.message,
                }).then(function () {
                    // Redireciona para a tela de listagem após o sucesso
                    window.location.href = config.contextPath + result.controller + '/' + result.action;
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