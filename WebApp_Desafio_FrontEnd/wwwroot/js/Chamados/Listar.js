$(document).ready(function () {
    var table = $('#dataTables-Chamados').DataTable({
        paging: false,
        ordering: true,
        info: false,
        searching: true,
        processing: true,
        serverSide: false,
        ajax: {
            url: config.contextPath + 'Chamados/Datatable',
            dataSrc: 'data'
        },
        columns: [
            { data: 'ID' },
            { data: 'Assunto' },
            { data: 'Solicitante' },
            { data: 'Departamento' },
            { data: 'DataAberturaWrapper', title: 'Data Abertura' },
        ],
        language: {
            search: "Buscar:",
            lengthMenu: "Mostrar _MENU_ registros",
            info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
            infoEmpty: "Mostrando 0 a 0 de 0 registros",
            infoFiltered: "(filtrado de _MAX_ registros no total)",
            paginate: {
                first: "Primeiro",
                last: "Último",
                next: "Próximo",
                previous: "Anterior"
            }
        }
    });

    // Evento para selecionar a linha com um clique
    $('#dataTables-Chamados tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    // REQUISITO 4: Evento de duplo-clique para editar
    $('#dataTables-Chamados tbody').on('dblclick', 'tr', function () {
        var data = table.row(this).data();
        if (data && data.ID) {
            window.location.href = config.contextPath + 'Chamados/Editar/' + data.ID;
        }
    });

    $('#btnRelatorio').click(function () {
        window.location.href = config.contextPath + 'Chamados/Report';
    });

    $('#btnAdicionar').click(function () {
        window.location.href = config.contextPath + 'Chamados/Cadastrar';
    });

    $('#btnEditar').click(function () {
        var data = table.row('.selected').data();
        if (data && data.ID) {
            window.location.href = config.contextPath + 'Chamados/Editar/' + data.ID;
        } else {
            Swal.fire('Atenção', 'Selecione um chamado para editar.', 'warning');
        }
    });

    $('#btnExcluir').click(function () {
        var data = table.row('.selected').data();
        if (!data || !data.ID) {
            Swal.fire('Atenção', 'Selecione um chamado para excluir.', 'warning');
            return;
        }

        Swal.fire({
            text: "Tem certeza de que deseja excluir o chamado '" + data.Assunto + "'?",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: 'Sim, excluir!',
            cancelButtonText: 'Cancelar'
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    url: config.contextPath + 'Chamados/Excluir/' + data.ID,
                    type: 'DELETE',
                    success: function (result) {
                        Swal.fire({
                            type: result.Type,
                            text: result.Message,
                        }).then(function () {
                            table.ajax.reload();
                        });
                    },
                    error: function (result) {
                        Swal.fire({
                            text: 'Ocorreu um erro ao excluir o chamado.',
                            confirmButtonText: 'OK',
                            icon: 'error'
                        });
                    }
                });
            }
        });
    });
});