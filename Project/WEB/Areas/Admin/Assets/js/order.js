$(document).ready(function () {
    GetData();
});

$('#btnOpenModalAdd').click(function () {
    $('#modal-order-details').modal();
});
function GetData() {
    var paginationPage = parseInt($('.cdp').attr('actpage'), 10);
    $.ajax({
        url: '/Order/GetList?pageNumber=' + paginationPage + '&pageSize=10',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                var html = "";
                $.each(data.data, function (index, value) {
                    html += sizeRow(value);
                });
                $('#order-data').html(html);
                var html1 = paging(data.totalPage, paginationPage);
                $('.content_detail__pagination').html(html1);

            }
        }
    });
}
function GetDataEdit(id) {
    $.ajax({
        url: '/Order/Get/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                $('#modal-order-details').modal();
            } else {
                alert('fail');
            }
        }
    });
}

function paging(page, index) {
    if (index === 'undefined') {
        index = 1;
    }
    $('.cdp').attr('actpage', index);
    var html = '<a href = "#!-1" onclick="pagingChoose(this)" class="cdp_i" > prev</a>';
    for (var i = 1; i < page + 1; i++) {
        html += '<a href="#!' + i + '" onclick="pagingChoose(this)" class="cdp_i">' + i + '</a>';
    }
    html += '<a href="#!+1" onclick="pagingChoose(this)" class="cdp_i">next</a>';
    return html;
}

function pagingChoose(a) {
    var paginationPage = parseInt($('.cdp').attr('actpage'), 10);
    var go = $(a).attr('href').replace('#!', '');
    if (go === '+1') {
        paginationPage++;
    } else if (go === '-1') {
        paginationPage--;
    } else {
        paginationPage = parseInt(go, 10);
    }
    $('.cdp').attr('actpage', paginationPage);
    GetData();
}

function sizeRow(data) {
    var created = '';
    var modified = '';
    data.CreatedAt != null
        ? created = new Date(parseInt(data.CreatedAt.replace("/Date(", "").replace(")/", ""), 10)).toLocaleString()
        : created = '';
    data.ModifiedAt != null
        ? modified = new Date(parseInt(data.ModifiedAt.replace("/Date(", "").replace(")/", ""), 10)).toLocaleString()
        : modified = '';
    var html = '';
    html+='<tr>'
    html += '<td>' + data.OrderId + '</td>';
    html += '<td>' + data.CustomerName + '</td>';
    html += '<td>' + data.PhoneNumber + '</td>';
    html += '<td>' + data.AddressDetails + '</td>';
    html += '<td>' + data.Total + '</td>';   
    html += '<td>' + data.Status + '</td>';
    html += '<td>' + created + '</td>';
    html += '<td>' + modified + '</td>';
    html += '<td style="text-align:center">' +
        '<a href = "#" id = "edit-user" onclick="GetDataEdit('+data.OrderId+')" class="btn waves-effect waves-light btn-warning" style = "padding:5px" >' +
        '<i class="ion-information-circled"></i>' +
        '</a>' +
        '<button id="delete_user" class="btn waves-effect waves-light btn-danger disabled" style="padding:5px"> <i class="fa fa-remove"></i> </button>' +
        '</td>';
    html += '</tr>';    
    return html;
}