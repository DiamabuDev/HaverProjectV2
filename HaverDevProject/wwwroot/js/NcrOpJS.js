$(document).ready(function () {
    GetNcrs();
});

function GetNcrs() {
    $.ajax({
        url: '/NcrProcurement/GetNcrs',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        success: function (response) {
            if (response.length === 0) {
                $('#tblBody').html('<tr><td colspan="3">No New NCRs available</td></tr>');
            } else {
                $('#tblBody').empty(); // Clear existing rows
                for (let i = 0; i < response.length; i++) {
                    const ncrOp = response[i];
                    const $row = $('<tr>');
                    $row.append(`<td>${ncrOp.ncrNumber}</td>`);
                    $row.append(`<td>${ncrOp.supplierName}</td>`);//Supplier
                    $row.append(`<td><button class="btn btn-success" onclick="startNcrProcurement('${ncrOp.ncrNumber}')"><i class="bi bi-play-fill"></i> Start</button></td>`);
                    $('#tblBody').append($row);
                }
            }
        },
        error: function () {
            alert('Error fetching data.'); // Provide more specific error message if possible
        }
    });
}

function startNcrProcurement(ncrNumber) {
    window.location.href = '/NcrProcurement/Create?ncrNumber=' + ncrNumber;
}