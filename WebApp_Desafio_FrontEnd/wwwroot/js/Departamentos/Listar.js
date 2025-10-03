$(document).ready(function () {

    var table = $('#dataTables-Departamentos').DataTable({
        paging: false,
        ordering: true,
        info: false,
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

    // Evento para selecionar a linha com um clique
    $('#dataTables-Departamentos tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    // REQUISITO 4: Evento de duplo-clique para editar
    $('#dataTables-Departamentos tbody').on('dblclick', 'tr', function () {
        var data = table.row(this).data();
        if (data && data.ID) {
            // Nota: A tela de edição de departamentos precisa ser criada.
            // O código abaixo assume que a rota será /Departamentos/Editar/id
            window.location.href = config.contextPath + 'Departamentos/Editar/' + data.ID;
        }
    });


    $('#btnRelatorio').click(function () {
        window.location.href = config.contextPath + 'Departamentos/Relatorio';
    });
});