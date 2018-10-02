
$(document).ready(function () {
    GetData();
    $('select[name=filter_user]').change(function () {
        $('.cdp').attr('actpage', '1');
        GetData();
    });
    $('input[name=search-bar]').keyup(function (e) {
        if (e.keyCode === 13) {
            $('.cdp').attr('actpage', '1');
            GetData();
        }
    });
    $('.queryBtn').click(function () {
        $('.cdp').attr('actpage', '1');
        GetData();
    });

});
function GetData() {
    var paginationPage = parseInt($('.cdp').attr('actpage'), 10);
    var filter = $('select[name=filter_user]').val().trim();
    var search = $('input[name=search-bar]').val();
    var filterDate = $('#filter-date').text();
    $('#loading').show();
    $.ajax({
        url: '/User/GetList?pageNumber=' + paginationPage + '&pageSize=10&filter=' + filter + '&search=' + search + '&date=' + filterDate,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.status === true) {
                var html = "";
                $.each(data.data, function (index, value) {
                    html += sizeRow(value);
                });
                $('#user-data').html(html);
                var html1 = paging(data.totalPage, paginationPage);
                $('.content_detail__pagination').html(html1);
                $('#loading').hide();
                ChangeStatus();
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
    html += '<tr>';
    html += '<td>' + data.Username + '</td>';
    html += '<td>' + data.CustomerName + '</td>';
    html += '<td>' + data.Role + '</td>';
    html += '<td><select data-id="' + data.Username + '" class="form-control status" id = "user-status" >';
    html += data.Status === 'active' ? '<option selected value="active">Active</option><option>Inactive</option>' : '<option value="inactive">Active</option><option  selected>Inactive</option>';
    html += '</select></td>';
    html += '<td>' + created + '</td>';
    html += '<td>' + modified + '</td>'
    html += '<td style="text-align:center">' +
        '<a href = "#" id = "edit-user" class="btn waves-effect waves-light btn-warning" style = "padding:5px" >' +
        '<i class="ion-information-circled"></i>' +
        '</a>' +
        '<button id="delete_user" class="btn waves-effect waves-light btn-danger disabled" style="padding:5px"> <i class="fa fa-remove"></i> </button>' +
        '</td>';
    html += '</tr>';
    return html;
}
function ChangeStatus() {
    $('.status').each(function () {
        //Store old value
        $(this).data('lastValue', $(this).val());
        
    });
    $(".status").change(function () {
        var lastRole = $(this).data('lastValue');
        var newRole = $(this).val();
        var username = $(this).data('id');
        swal({
            title: "Are you sure?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        url: '/User/ChangeStatus?username=' + username + '&mode=' + 1,
                        type: 'GET',
                        success: function (response) {
                            if (response.status) {
                                swal("Good job!", response.message, "success");
                                $(this).data('lastValue', newRole);
                            }
                        }
                    });    
                } else {
                    $(this).val(lastRole);  
                }
            });
    });
}