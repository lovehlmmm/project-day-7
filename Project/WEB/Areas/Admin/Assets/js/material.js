
$(document).ready(function () {
    String.prototype.format = function () {
        a = this;
        for (k in arguments) {
            a = a.replace("{" + k + "}", arguments[k]);
        }
        return a;
    }
    $('#form-new-group').validate({
        rules: {
            groupname: {
                required: true
            },
            maxitem: {
                required: true,
                number: true,
                min: 1
            }
        },
        submitHandler: function (form) {
            var name = $('input[name=groupname]').val();
            var active = $('input[name=activegr]:checked').length > 0 ? 'active' : 'inactive';
            var maxitem = parseInt($('input[name=maxitem]').val());
            var type = parseInt($('#form-new-group').data('type'));
            var id = $('input[name=groupid]').val();
            var groupall = { GroupName: name, MaxItem: maxitem, Status: active };

            if (type === 1) {
                AddGroup(groupall);
            } else {
                UpdateGroup(groupall, id);
            }
            return false;
        }
    })
    $('#showGroup').click(function () {
        GetModalGroup();
        $('#modalGroup').modal('show');
        OpenAddGroupModal();
        OpenEditModal();
        DeleteGroupClick();

    });

    //$("#modalAddGroup").click(function () {
    //     $("#modalAddGroup").modal();
    //});


    GetData();
    //validate form create and update material
    $('#form_new_update').validate({
        rules: {
            name: {
                required: true
            },
            price: {
                number: true,
                min: 0
            },
            material: {
                required: true
            }
        },
        messages: {
            name: {
                required: "Please enter material name"
            },
            material: {
                required: "Please enter material"
            }
        },
        submitHandler: function (form) {
            var name = $('input[name=name]').val();
            var active = $('input[name=active]:checked').length > 0 ? 'active' : 'inactive';
            var price = $('input[name=price]').val();
            var id = $('input[name=id]').val();
            var material = $('input[name=material]').val();
            var a = $('#form_new_update').data('type');
            var groupid = $('#groupselect').val();
            var file = $('.img-file').prop('files')[0];
            var size = { Name: name, Price: price, Status: active, Details: material, GroupId: groupid };

            if ($('#form_new_update').data('type') === '1') {
                Add(size, file);
            } else {
                Update(size, id, file);
            }
            return false;
        }
    });


});

function OpenAddGroupModal() {

    $('#newGroup').click(function () {
        ClearFormGroup();
        $('#form-new-group').data('type', '1');
        $('#modalAddGroup').modal();
    });
}

function OpenEditModal() {
    $('.edit_group').click(function () {
        $('#form-new-group').data('type', '2');
        var id = $(this).data('id');
        GetDataEditGroup(id);
    });
}




$('#btnOpenModalAdd').click(function () {
    ClearForm();
    $('#form_new_update').data('type', '1');
    $('#titleSizeModal').text('New');
    GetGroup();
    $('#new-modal').modal();
});


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


function Update(data, id, file) {
    var materials = new FormData();
    materials.append('materials', JSON.stringify(data));
    materials.append('id', id);
    materials.append('materialimg', file);

    $.ajax({
        url: '/Material/Update',
        type: 'POST',
        data: materials,
        async: false,
        processData: false,
        contentType: false
    }).success(function (result) {
        if (result.status) {
            swal("Success", "You Update success!", "success");
            $('#new-modal').modal('toggle');
            GetData();
        } else {
            alert('fail');
        }
    }).error(function (xhr, status) {
    });
}



function ClearForm() {
    $('input[name=name]').val("");
    $('input[name=material]').val("");
    $('input[name=active]').prop('checked', false);
    $('input[name=price]').val(0);
}
function ClearFormGroup() {
    $('input[name=groupname]').val("");
    $('input[name=activegr]').prop('checked', false);
    $('input[name=maxitem]').val("");

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
    console.log(data.GroupId);
    $('#groupselect').val(data.GroupId);


}


function GetGroup() {
    $.ajax({
        url: '/Material/GetGroup',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                var groupSelect = ' <option value="0" selected>Choose...</option>';
                $.each(response.data, function (index, value) {
                    groupSelect += '<option value="{0}">{1}</option>'.format(value.Id, value.GroupName);
                });
                $('#groupselect').html(groupSelect);
            } else {
                alert('fail');
            }
        }
    });
}
function GetDataEdit(id) {
    GetGroup();
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
        url: '/Material/GetList?pageNumber=' + paginationPage + '&pageSize=10',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                var html = "";
                $.each(data.data, function (index, value) {
                    html += sizeRow(value);
                });
                $('#materialData').html(html);
                var html1 = paging(data.totalPage, paginationPage);
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

function Add(material, file) {
    var materials = new FormData();
    materials.append('materials', JSON.stringify(material));
    materials.append('materialimg', file);

    $.ajax({
        url: '/Material/Create',
        type: 'POST',
        data: materials,
        async: false,
        processData: false,
        contentType: false
    }).success(function (result) {
        if (result.status) {
            swal("Success", "You created success!", "success");
            $('#new-modal').modal('toggle');
            GetData();
        } else {
            alert('fail');
        }
    }).error(function (xhr, status) {
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
    html += ('<td>' + data.Image + '</td>');
    html += ('<td>' + data.Group.GroupName + '</td>');
    html += ('<td>' + data.Status + '</td>');
    html += ('<td>' + created + '</td>');
    html += ('<td>' + modified + '</td>');
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

function DataToFormGroup(data) {
    var status = true;
    if (data.Status !== 'active') {
        status = false;
    }
    $('input[name=groupname]').val(data.GroupName);
    $('input[name=activegr]').prop('checked', status);
    $('input[name=maxitem]').val(data.MaxItem);
    $('input[name=groupid]').val(data.Id);
}

function GetDataEditGroup(id) {
    $.ajax({
        url: '/Material/GetDataGroup/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                DataToFormGroup(response.data);
                $('#modalAddGroup').modal();
            } else {
                alert('fail');
            }
        }
    });
}

function UpdateGroup(group, id) {
    var data = new FormData();
    data.append('group', JSON.stringify(group));
    data.append('id', id);
    $.ajax({
        url: '/Material/UpdateGroup',
        type: 'POST',
        data: data,
        async: false,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.status) {
                swal("Success", "You Update success!", "success");
                GetModalGroup();
                $('#modalAddGroup').modal('toggle');
            } else {
                alert('fail');
            }
        }
    });
}


function AddGroup(group) {
    var data = new FormData();
    data.append('group', JSON.stringify(group));
    $.ajax({
        url: '/Material/NewGroup',
        type: 'POST',
        data: data,
        async: false,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.status) {
                swal("Success", "You created success!", "success");
                GetModalGroup();

                $('#modalAddGroup').modal('hide');
            } else {
                alert('fail');
            }
        }
    });
}



function GetModalGroup() {
    $.ajax({
        url: '/Material/GetModalGroup',
        type: 'get',
        dataType: 'html',
        async: false,
        success: function (result) {

            $('#modalGroup .modal-body').html(result);
            OpenAddGroupModal();
            OpenEditModal();
            DeleteGroupClick();
            //clickNew();
        }
    });
}


function DeleteItemGroup(id) {
    $.ajax({
        url: '/Material/DeleteGroup/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.status) {
                swal("Poof! Has been deleted!", {
                    icon: "success",
                });
                GetModalGroup();
            } else {
                alert('fail');
            }
        }
    });
}
function CloseModal(e) {
    $(e).parents('.modal-close').modal('toggle');
    //$(e).parents('.modal-close').css('display', "none");
    //$(e).parents('.modal-close').attr('aria-hidden', true);
}
function DeleteGroup(id) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will be able to recover at history!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                DeleteItemGroup(id);

            } else {
                swal("You canceled delete action!");
            }
        });
}
function DeleteGroupClick() {
    $('.delete_group').click(function () {
        var id = $(this).data('id');
        DeleteGroup(id);
    })
}