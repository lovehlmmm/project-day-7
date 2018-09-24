$(document).ready(function () {
    GetData();
});

$('#btnOpenModalAdd').click(function () {
    ClearForm();
    $('#form_new_update').data('type', '1');
    $('#titleSizeModal').text('New');
    $('#new-modal').modal();
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

$('#form_new_update').validate({
    rules: {
        name: {
            required: true,
            minlength: 5,
            maxlength:20
        },
        material: {
            required: true,
            minlength: 5,
            maxlength: 20
        },
        price: {
            required: true,
            number: true,
            min:1
        }
    },
    submitHandler: function (form) { // for demo
        var name = $('input[name=name]').val();
        var active = $('input[name=active]:checked').length > 0 ? 'active' : 'inactive';
        var price = $('input[name=price]').val();
        var id = $('input[name=id]').val();
        var material = $('input[name=material]').val();
        var a = $('#form_new_update').data('type');
        var size = { Name: name, Price: price, Status: active, Details: material };
        if ($('#form_new_update').data('type') === '1') {
            Add(size);
        } else {
            Update(size, id);
        }
        return false; // for demo
    },
    messages: {
        name: {
            required:"Please enter material name"
        },
        material: {
            required: "Please enter material"
        }
    }
});
function Update(data, id) {
    $.ajax({
        url: '/Material/Update',
        type: 'POST',
        dataType: 'json',
        data: { material: data, id: id },
        success: function (response) {
            if (response.status) {
                swal("Success", "You Update success!", "success");
                $('#new-modal').modal('toggle');
                GetData();
            } else {
                alert('fail');
            }
        }
    });
}
function ClearForm() {
    $('input[name=name]').val("");
    $('input[name=material]').val("");
    $('input[name=active]').prop('checked', false);
    $('input[name=price]').val(0);
}
function DataToForm(data) {
    var status = true;
    if (data.Status !== 'active') {
        status = false;
    }
    $('input[name=name]').val(data.Name);
    $('input[name=active]').prop('checked', status);
    $('input[name=price]').val(data.Price);
    $('input[name=material]').val(data.Details);
    $('input[name=id]').val(data.Id);
}

function GetDataEdit(id) {
    $.ajax({
        url: '/Material/Get/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                $('#form_new_update').data('type', '2');
                var a = $('#form_new_update').data('type');
                $('#title').text('Update');
                DataToForm(response.data);
                $('#new-modal').modal();
            } else {
                alert('fail');
            }
        }
    });
}

function GetData() {
    var paginationPage = parseInt($('.cdp').attr('actpage'), 10);
    $.ajax({
        url: '/Material/GetList?pageNumber='+paginationPage+'&pageSize=10',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                var html = "";
                $.each(data.data, function (index, value) {
                    html += sizeRow(value);
                });
                $('#materialData').html(html);
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

function Add(material) {
    $.ajax({
        url: '/Material/Create',
        type: 'POST',
        dataType: 'json',
        data: { material: material },
        success: function (response) {
            if (response.status) {
                swal("Success", "You created success!", "success");
                $('#new-modal').modal('toggle');
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
    data.CreatedAt !== null
        ? created = new Date(parseInt(data.CreatedAt.replace("/Date(", "").replace(")/", ""), 10)).toLocaleString()
        : created = '';
    data.ModifiedAt !== null
        ? modified = new Date(parseInt(data.ModifiedAt.replace("/Date(", "").replace(")/", ""), 10)).toLocaleString()
        : modified = '';
    var html = '';
    html += ('<tr>< td><input id="checkbox" data-id="' + data.Id + '" type="checkbox">');
    html += ('</td><th scope = "row" > ' + data.Id + '</th>');
    html += ('<td>' + data.Name + '</td>');
    html += ('<td>' + data.Details + '</td>');
    html += ('<td>' + data.Price + '</td>');
    html += ('<td>' + data.Status + '</td>');
    html += ('<td>' +created + '</td>');
    html += ('<td>' + modified+ '</td>');
    html += ('<td style="text-align:center">');
    html += ('<button id="edit_size" data-id="' +
        data.Id +
        '" onclick="GetDataEdit(' + data.Id + ')" class="btn waves-effect waves-light btn-warning" style="padding:5px"> <i class="fa fa-edit"></i></button>');
    html += ('<button  class="btn waves-effect waves-light btn-danger disabled" style="padding:5px" data-id = "' + data.Id + '" onclick="Delete(' + data.Id + ')"><i class="fa fa-remove delete_size"></i> </button>');
    html += ('</td></tr >');
    return html;
}

function DeleteItem(id) {
    $.ajax({
        url: '/Material/Delete/' + id,
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

