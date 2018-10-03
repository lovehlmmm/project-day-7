
const Canceled = "Canceled"
const Confirmed = "Confirmed"
const Pending = "Pending"
const Received = "Received"
const Processing = "Processing"
$(document).ready(function () {
    
    GetData();
    $('select[name=filter_order]').change(function () {
        GetData();
    })
    $('input[name=search-bar]').on('input', function () {
        GetData();
    });
    $('.queryBtn').click(function () {
        $('.cdp').attr('actpage', '1');
        GetData();
    });
});
function GetImage(url) {
    $('#image-details').attr('src', url);
    $('#imageModal').show();
    $('.close').click(function () {
        $('#imageModal').hide();
    })
};
$('#btnOpenModalAdd').click(function () {
    $('#modal-order-details').modal();
});
function GetData() {
    var paginationPage = parseInt($('.cdp').attr('actpage'), 10);
    var filter = $('select[name=filter_order]').val().trim();
    var search = $('input[name=search-bar]').val();
    var filterDate = $('#filter-date').text();
    $('#loading').show();
    $.ajax({
        url: '/Order/GetList?pageNumber=' + paginationPage + '&pageSize=10&filter=' + filter + '&search=' + search + '&date=' + filterDate,
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
                $('#loading').hide();
            }
        }
    });
}

function GetDataEdit(id) {

    $.ajax({
        url: '/Order/Get/' + id,
        type: 'GET',
        success: function (response) {
            $('#body-modal-order-details').html(response);
            $('#modal-order-details').modal();
        }
    });
}
//function GetDataEdit(id) {
//    var url = '/Order/Get/' + id;

//    $('#body-modal-order-details').load(url, function () {
//        $('#modal-order-details').modal('show');
//    })

//}
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
    data.CreatedAt !== null
        ? created = new Date(parseInt(data.CreatedAt.replace("/Date(", "").replace(")/", ""), 10)).toLocaleString()
        : created = '';
    data.ModifiedAt !== null
        ? modified = new Date(parseInt(data.ModifiedAt.replace("/Date(", "").replace(")/", ""), 10)).toLocaleString()
        : modified = '';
    var mode = "";
    switch (data.Status) {

        case Pending:
            mode = Confirmed;
            break;
        case Confirmed:
            mode = Received;
            break;
    }
    var html = '';
    html += '<tr>'
    html += '<td>' + data.OrderId + '</td>';
    html += '<td>' + data.CustomerName + '</td>';
    html += '<td>' + data.PhoneNumber + '</td>';
    html += '<td>' + data.AddressDetails + '</td>';
    html += '<td>' + data.FolderImage + '</td>';
    html += '<td>' + data.Total + '</td>';
    html += '<td>' + data.Status + '</td>';
    html += '<td>' + created + '</td>';
    html += '<td>' + modified + '</td>';
    html += '<td style="text-align:center">';
    if (data.Status !== Received & data.Status !== Canceled) {
        html += '<button id="option_order" class="btn waves-effect waves-light btn-success"  onclick="ChangeStatusOrder(' + data.OrderId + ',' + mode + ',this)" style="padding:5px"> <i class="fa fa-check"></i> </button>';
    }
    html += '<a href = "#" id = "edit-user" onclick="GetDataEdit(' + data.OrderId + ')" class="btn waves-effect waves-light btn-warning" style = "padding:5px" >' +
        '<i class="ion-information-circled"></i>' +
        '</a>' +
        '<button id="delete_user" class="btn waves-effect waves-light btn-danger disabled" style="padding:5px"> <i class="fa fa-remove"></i> </button>' +
        '</td>';
    html += '</tr>';
    return html;
}

function DetailsData(data) {

}
function ChangeStatus(id, mode, e) {
    var result;
    switch (mode) {
        case 'Confirmed':
            result = AjaxChangeConfirm(id, mode, 'Confirm Success', '', e);
            break;
        case 'Canceled':
            result = AjaxChangeCancel(id, mode, 'Cancel Success', '', e);
            break;
        default:
    }

}
function ChangeStatusOrder(id, mode, e) {
    switch (mode) {
        case Confirmed:
            AjaxChangeConfirm(id, mode, 'Confirm Success', 'Confirm order');
            break;
        case Received:
            AjaxChangeConfirm(id, mode, 'Received Success', 'Receive');
            break;
    }
}
function AjaxChangeCancel(id, mode, message = '', text = '', e = null) {
    swal({
        title: "Are you sure?",
        text: text,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                swal("Write the reason:", {
                    content: "input"
                })
                   .then((value) => {
                        $.ajax({
                            url: '/Order/ChangeStatus?id=' + id + '&mode=' + mode + '&reason=' + value,
                            type: 'GET',
                            success: function (response) {
                                if (response.status) {
                                    swal(message, {
                                        icon: "success",
                                    });
                                    if (e !== null) {
                                        $(e).closest('tr').remove();

                                    }

                                }
                            }
                        });
                    });


            } else {
                swal("Your imaginary file is safe!");
            }
        });
}
function AjaxChangeConfirm(id, mode, message = '', text = '', e = null) {
    swal({
        title: "Are you sure?",
        text: text,
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: '/Order/ChangeStatus?id=' + id + '&mode=' + mode,
                    type: 'GET',
                    success: function (response) {
                        if (response.status) {
                            swal(message, {
                                icon: "success",
                                text: message
                            });
                            if (e !== null) {
                                $(e).closest('tr').remove();
                            }

                        }
                    }
                });

            }
        });

}

