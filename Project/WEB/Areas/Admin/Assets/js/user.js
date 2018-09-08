
$(document).ready(function () {
    GetData();
});
function GetData() {
    var paginationPage = parseInt($('.cdp').attr('actpage'), 10);
    $.ajax({
        url: '/User/GetList?pageNumber=' + paginationPage + '&pageSize=10',
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
    html += '<td>' + data.Username + '</td>';
    html += '<td>' + data.Customer + '</td>';
    html += '<td>' + data.Role + '</td>';
    html += data.Status === 'active' ? '<option selected value="active">Active</option><option>Inactive</option>' : '<option value="inactive">Active</option><option  selected>Inactive</option>'
        
        
        <td>
        <select class="form-control" id="exampleFormControlSelect1">
        <option>1</option>
        <option>2</option>
        <option>3</option>
        <option>4</option>
        <option>5</option>
        </select>
            </div>
        </td>
        <td></td>
        <td></td>
        <td style="text-align:center">
            <a href="#" id="edit_material" class="btn waves-effect waves-light btn-warning" style="padding:5px">
                <i class="ion-information-circled"></i>
            </a>
            <button id="delete_material" class="btn waves-effect waves-light btn-danger disabled" style="padding:5px"> <i class="fa fa-remove"></i> </button>
        </td>
    return html;
}