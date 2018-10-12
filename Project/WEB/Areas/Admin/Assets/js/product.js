$(document).ready(function () {
    GetData();
});

$('#btnOpenModalAdd').click(function () {
    ClearForm();
    $('#form-product').data('type', '1');
    $('#titleModal').text('New Product');
    $('#submit-form').text('Create');
    $('#product-modal').modal();
});
//$('#form_size').submit(function () {
//    var product-name = $('input[name=product-name]').val();
//    var product-active = $('input[name=product-active]:checked').length > 0 ? 'active' : 'inactive';
//    var size = $('input[name=size]').val();
//    var size_price = $('input[name=size_pricse]').val();
//    var size = { ProductName: product-name, ProductSize: size, ProductPrice: size_price, Status: product-active }

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

$('#form-product').submit(function () {

    var productName = $('input[name=product-name]').val();
    var productActive = $('input[name=product-active]:checked').length > 0 ? 'active' : 'inactive';
    var productSize = $('input[name=product-size]').val();
    var productPrice = $('input[name=product-price]').val();
    var id = $('input[name=product-id]').val();
    var productMaterial = $('input[name=product-material]').val();
    var product = { ProductName: productName, ProductSize: productSize, ProductMaterial:productMaterial, ProductPrice: productPrice, Status: productActive };
    if ($('#form-product').data('type') === '1') {
        Add(product);
    } else {
        Update(product, id);
    }
    return false;
});

function Update(data, id) {
    $.ajax({
        url: '/Product/Update',
        type: 'POST',
        dataType: 'json',
        data: { product: data, id: id },
        success: function (response) {
            if (response.status) {
                swal("Success", "You Update success!", "success");
                $('#product-modal').modal('toggle');
                GetData();
            } else {
                alert('fail');
            }
        }
    });
}
function ClearForm() {
    $('input[name=product-name]').val("");
    $('input[name=product-active]').prop('checked', false);
    $('input[name=product-size]').val("");
    $('input[name=product-price]').val(0);
}
function DataToForm(data) {
    var status = true;
    if (data.Status !== 'active') {
        status = false;
    }
    $('input[name=product-name]').val(data.ProductName);
    $('input[name=product-active]').prop('checked', status);
    $('input[name=product-size]').val(data.ProductSize);
    $('input[name=product-price]').val(data.ProductPrice);
    $('input[name=product-id]').val(data.ProductId);
}

function GetDataEdit(id) {
    $.ajax({
        url: '/Product/Get/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                $('#form-product').data('type', '2');
                $('#titleModal').text('Update Product');
                $('#submit-form').text('Save changes');
                DataToForm(response.data);
                $('#product-modal').modal();
            } else {
                alert('fail');
            }
        }
    });
}

function GetData() {
    var paginationPage = parseInt($('.cdp').attr('actpage'), 10);
    $.ajax({
        url: '/Product/GetList?pageNumber='+paginationPage+'&pageSize=10',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                var html = "";
                $.each(data.data, function (index, value) {
                    html += sizeRow(value);
                });
                $('#product-data').html(html);
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

function Add(product) {
    $.ajax({
        url: '/Product/Create',
        type: 'POST',
        dataType: 'json',
        data: { product: product },
        success: function (response) {
            if (response.status) {
                swal("Success", "You created success!", "success");
                $('#product-modal').modal('toggle');
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
        ? created = new Date(parseInt(data.CreatedAt.replace("/Date(", "").replace(")/", ""), 10)).toLocaleString()
        : created = '';
    data.ModifiedAt != null
        ? modified = new Date(parseInt(data.ModifiedAt.replace("/Date(", "").replace(")/", ""), 10)).toLocaleString()
        : modified = '';
    var html = '';
    html += ('<tr>< td><input id="checkbox" data-id="' + data.ProductId + '" type="checkbox">');
    html += ('</td><th scope = "row" > ' + data.ProductId + '</th>');
    html += ('<td>' + data.ProductName + '</td>');
    html += ('<td>' + data.ProductSize + '</td>');
    html += ('<td>' + data.ProductPrice + '</td>');
    html += ('<td>' + data.Status + '</td>');
    console.log(data.ModifiedAt);
    html += ('<td>' + created+ '</td>');
    html += ('<td>' +modified + '</td>');
    html += ('<td style="text-align:center">');
    html += ('<button id="edit_size" data-id="' +
        data.ProductId +
        '" onclick="GetDataEdit(' + data.ProductId + ')" class="btn waves-effect waves-light btn-warning" style="padding:5px"> <i class="fa fa-edit"></i></button>');
    html += ('<button id="delete_size"  data-id = "' + data.ProductId + '" onclick="Delete(' + data.ProductId + ')"  class="btn waves-effect waves-light btn-danger disabled" style="padding:5px"> <i  class="fa fa-remove"></i> </button>');
     html += ('</td></tr >');
    return html;
}

function DeleteItem(id) {
    $.ajax({
        url: '/Product/Delete/' + id,
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

