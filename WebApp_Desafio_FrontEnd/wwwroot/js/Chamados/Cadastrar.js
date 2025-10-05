$(document).ready(function () {

    $('.glyphicon-calendar').closest("div.date").datepicker({
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: false,
        format: 'dd/mm/yyyy',
        autoclose: true,
        language: 'pt-BR'
    });

    $('#solicitante-select').select2({
        placeholder: "Digite para buscar um solicitante",
        minimumInputLength: 2,
        language: "pt-BR",
        ajax: {
            url: config.contextPath + 'Chamados/SearchSolicitantes',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    term: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data
                };
            },
            cache: true
        }
    });


    $('#btnCancelar').click(function () {
        Swal.fire({
            html: "Deseja cancelar essa operação? O registro não será salvo.",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: 'Sim, cancelar!',
            cancelButtonText: 'Não'
        }).then(function (result) {
            if (result.value) {
                window.location.href = config.contextPath + 'Chamados/Listar';
            }
        });
    });

    $('#btnSalvar').click(function () {

        if ($('#form').valid() != true) {
            return;
        }

        let chamado = SerielizeForm($('#form'));
        let url = $('#form').attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: chamado,
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
                let errorMessage = "Ocorreu um erro ao processar sua solicitação.";
                if (errorResponse && errorResponse.Message) {
                    errorMessage = errorResponse.Message;
                }

                Swal.fire({
                    text: errorMessage,
                    confirmButtonText: 'OK',
                    icon: 'error'
                });
            }
        });
    });
});