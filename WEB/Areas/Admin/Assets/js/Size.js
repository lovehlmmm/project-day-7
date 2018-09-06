$(document).ready(function () {
    GetData();
});

$('#btnOpenModalAdd').click(function () {
    ClearForm();
    $('#form_size').data('type', '1');
    $('#titleSizeModal').text('New Size')
    $('#new-size-modal').modal();
});
//$('#form_size').submit(function () {
//    var size_name = $('input[name=size_name]').val();
//    var size_active = $('input[name=size_active]:checked').length > 0 ? 'active' : 'inactive';
//    var size = $('input[name=size]').val();
//    var size_price = $('input[name=size_pricse]').val();
//    var size = { SizeName: size_name, SizeDetails: size, SizePrice: size_price, Status: size_active }

//    return false;
//});

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

$('#form_size').submit(function () {

    var size_name = $('input[name=size_name]').val();
    var size_active = $('input[name=size_active]:checked').length > 0 ? 'active' : 'inactive';
    var size_details = $('input[name=size]').val();
    var size_price = $('input[name=size_price]').val();
    var id = $('input[name=size-id]').val();
    var size = { SizeName: size_name, SizeDetails: size_details, SizePrice: size_price, Status: size_active }
    if ($('#form_size').data('type') === '1') {
        Add(size);
    } else {
        Update(size, id);
    }
    return false;
});

function Update(data, id) {
    $.ajax({
        url: '/Size/Update',
        type: 'POST',
        dataType: 'json',
        data: { size: data, id: id },
        success: function (response) {
            if (response.status) {
                swal("Success", "You Update success!", "success");
                $('#new-size-modal').modal('toggle');
                GetData();
            } else {
                alert('fail');
            }
        }
    });
}
function ClearForm() {
    $('input[name=size_name]').val("");
    $('input[name=size_active]').prop('checked', false);
    $('input[name=size]').val("");
    $('input[name=size_price]').val(0);
}
function DataToForm(data) {
    var status = true;
    if (data.Status !== 'active') {
        status = false;
    }
    $('input[name=size_name]').val(data.SizeName);
    $('input[name=size_active]').prop('checked', status);
    $('input[name=size]').val(data.SizeDetails);
    $('input[name=size_price]').val(data.SizePrice);
    $('input[name=size-id]').val(data.SizeId);
}

function GetDataEdit(id) {
    $.ajax({
        url: '/Size/Get/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                $('#form_size').data('type', '2');
                $('#titleSizeModal').text('Update Size');
                DataToForm(response.data);
                $('#new-size-modal').modal();
            } else {
                alert('fail');
            }
        }
    });
}

function GetData() {
    var paginationPage = parseInt($('.cdp').attr('actpage'), 10);
    $.ajax({
        url: '/Size/GetList?pageNumber='+paginationPage+'&pageSize=10',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                var html = "";
                $.each(data.data, function (index, value) {
                    html += sizeRow(value);
                });
                $('#sizeData').html(html);
                var html1 = paging(data.totalPage,paginationPage);
                $('.content_detail__pagination').html(html1);

            }
        }
    });
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

function Add(size) {
    $.ajax({
        url: '/Size/Create',
        type: 'POST',
        dataType: 'json',
        data: { size: size },
        success: function (response) {
            if (response.status) {
                swal("Success", "You created success!", "success");
                $('#new-size-modal').modal('toggle');
                GetData();
            } else {
                alert('fail');
            }
        }
    });
}
function sizeRow(data) {
    var created = '';
    var modified = '';
    data.CreatedAt != null
        ? created = new Date(parseInt(data.CreatedAt.replace("/Date(", "").replace(")/", ""), 10))
        : created = '';
    data.ModifiedAt != null
        ? modified = new Date(parseInt(data.ModifiedAt.replace("/Date(", "").replace(")/", ""), 10))
        : modified = '';
    var html = '';
    html += ('<tr>< td><input id="checkbox" data-id="' + data.SizeId + '" type="checkbox">');
    html += ('</td><th scope = "row" > ' + data.SizeId + '</th>');
    html += ('<td>' + data.SizeName + '</td>');
    html += ('<td>' + data.SizeDetails + '</td>');
    html += ('<td>' + data.SizePrice + '</td>');
    html += ('<td>' + data.Status + '</td>');
    console.log(data.ModifiedAt);
    html += ('<td>' + + '</td>');
    html += ('<td>' + + '</td>');
    html += ('<td style="text-align:center">');
    html += ('<button id="edit_size" data-id="' +
        data.SizeId +
        '" onclick="GetDataEdit(' + data.SizeId + ')" class="btn waves-effect waves-light btn-warning" style="padding:5px"> <i class="fa fa-edit"></i></button>');
    html += ('<button  class="btn waves-effect waves-light btn-danger disabled" style="padding:5px"> <i  data-id = "' + data.SizeId + '" onclick="Delete(' + data.SizeId + ')" class="fa fa-remove delete_size"></i> </button>');
    html += ('</td></tr >');
    return html;
}

function DeleteItem(id) {
    $.ajax({
        url: '/Size/Delete/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                swal("Poof! Has been deleted!", {
                    icon: "success",
                });
                GetData();
            } else {
                alert('fail');
            }
        }
    });
}

function Delete(id) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will be able to recover at history!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                DeleteItem(id);

            } else {
                swal("You canceled delete action!");
            }
        });
}

