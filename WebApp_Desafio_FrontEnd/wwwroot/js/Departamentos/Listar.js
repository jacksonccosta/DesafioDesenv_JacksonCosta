$(document).ready(function () {

    var table = $('#dataTables-Departamentos').DataTable({
        paging: true,
        ordering: true,
        info: true,
        searching: true,
        processing: true,
        serverSide: false,
        ajax: {
            url: config.contextPath + 'Departamentos/Datatable',
            dataSrc: 'data'
        },
        columns: [
            { data: 'ID' },
            { data: 'Descricao', title: 'Descrição' },
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

    $('#dataTables-Departamentos tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#dataTables-Departamentos tbody').on('dblclick', 'tr', function () {
        var data = table.row(this).data();
        if (data && data.ID) {
            window.location.href = config.contextPath + 'Departamentos/Editar/' + data.ID;
        }
    });

    $('#btnRelatorio').click(function () {
        window.location.href = config.contextPath + 'Departamentos/Relatorio';
    });

    $('#btnAdicionar').click(function () {
        window.location.href = config.contextPath + 'Departamentos/Cadastrar';
    });

    $('#btnEditar').click(function () {
        var data = table.row('.selected').data();
        if (data && data.ID) {
            window.location.href = config.contextPath + 'Departamentos/Editar/' + data.ID;
        } else {
            Swal.fire('Atenção', 'Selecione um departamento para editar.', 'warning');
        }
    });

    $('#btnExcluir').click(function () {
        var data = table.row('.selected').data();
        if (!data || !data.ID) {
            Swal.fire('Atenção', 'Selecione um departamento para excluir.', 'warning');
            return;
        }

        Swal.fire({
            title: 'Confirmar Exclusão',
            text: "Tem certeza de que deseja excluir o departamento '" + data.Descricao + "'?",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: 'Sim, excluir!',
            cancelButtonText: 'Cancelar'
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    url: config.contextPath + 'Departamentos/Excluir/' + data.ID,
                    type: 'DELETE',
                    success: function (result) {
                        Swal.fire({
                            type: result.type,
                            title: result.title,
                            text: result.message,
                        }).then(function () {
                            table.ajax.reload();
                        });
                    },
                    error: function (result) {
                        let errorResponse = result.responseJSON;
                        Swal.fire({
                            title: errorResponse.title,
                            text: errorResponse.message,
                            icon: 'error'
                        });
                    }
                });
            }
        });
    });
});