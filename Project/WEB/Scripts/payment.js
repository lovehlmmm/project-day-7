
$(document).ready(function () {
    GetCredit();
});

$('#showCredit').click(function () {
    //$('#modalCredit').modal();
    GetModalCredit();
});


$('.showAddCredit').click(function () {
    $form.find('.subscribe').html('Add Credit Card').prop('disabled', true);
    $('input[name=cardNumber]').val("");
    $('input[name=cardCVC]').val("");
    $('input[name=cardExpiry').val(""); 
    $('#modaladdcredit').modal();
});


function GetModalCredit() {
    $.ajax({
        url: '/PaymentCheckOut/GetModalCredit',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        async: false,
        dataType: 'html'
    }).success(function (result) {
        $('#modalCredit .modal-body').html(result);
        $('#modalCredit').modal();
        $('#saveChooseCredit').click(function () {
            var checkId = $('input[name=chooseCredit]:checked').data('id');
            if (checkId === undefined) {
                return;
            }
            $.ajax({
                url: '/PaymentCheckOut/GetCredit?id=' + checkId,
                type: 'GET',
                dataType: 'json',
                async: false,
                success: function (data) {
                    if (data.status === true) {
                        $('.creditDetails').text(data.credit.CreditNumber);
                        $('.creditEx').text(data.credit.Expire);
                        $('.creditDetails').data('id', data.credit.CreditCardId);
                        $('#modalCredit').modal('hide');
                    } else {
                        return false;

                    }
                }
            });
        });
    }).error(function (xhr, status) {
    });
}

var $form = $('#payment-form');
$form.find('.subscribe').on('click', payWithStripe);

/* If you're using Stripe for payments */
function payWithStripe(e) {
    e.preventDefault();

    /* Abort if invalid form data */
    if (!validator.form()) {
        return;
    }

    /* Visual feedback */
    $form.find('.subscribe').html('Validating <i class="fa fa-spinner fa-pulse"></i>').prop('disabled', true);

    var PublishableKey = 'pk_test_2qdVOIZLdYtFTJSeQYMmjyom'; // Replace with your API publishable key
    Stripe.setPublishableKey(PublishableKey);

    /* Create token */
    var expiry = $form.find('[name=cardExpiry]').payment('cardExpiryVal');
    var ccData = {
        CreditNumber: $form.find('[name=cardNumber]').val().replace(/\s/g, ''),
        CVC: $form.find('[name=cardCVC]').val(),
        Expire: expiry.month + '/' + expiry.year
    };
    var data = new FormData();
    data.append("creditCard", JSON.stringify(ccData));
    AddCreditCard(data);
}
/* Fancy restrictive input formatting via jQuery.payment library*/
$('input[name=cardNumber]').payment('formatCardNumber');
$('input[name=cardCVC]').payment('formatCardCVC');
$('input[name=cardExpiry').payment('formatCardExpiry');

/* Form validation using Stripe client-side validation helpers */
jQuery.validator.addMethod("cardNumber", function (value, element) {
    return this.optional(element) || Stripe.card.validateCardNumber(value);
}, "Please specify a valid credit card number.");

jQuery.validator.addMethod("cardExpiry", function (value, element) {
    /* Parsing month/year uses jQuery.payment library */
    value = $.payment.cardExpiryVal(value);
    return this.optional(element) || Stripe.card.validateExpiry(value.month, value.year);
}, "Invalid expiration date.");

jQuery.validator.addMethod("cardCVC", function (value, element) {
    return this.optional(element) || Stripe.card.validateCVC(value);
}, "Invalid CVC.");

validator = $form.validate({
    rules: {
        cardNumber: {
            required: true,
            cardNumber: true
        },
        cardExpiry: {
            required: true,
            cardExpiry: true
        },
        cardCVC: {
            required: true,
            cardCVC: true
        }
    },
    highlight: function (element) {
        $(element).closest('.form-control').removeClass('success').addClass('error');
    },
    unhighlight: function (element) {
        $(element).closest('.form-control').removeClass('error').addClass('success');
    },
    errorPlacement: function (error, element) {
        $(element).closest('.form-group').append(error);
    }
});

paymentFormReady = function () {
    if ($form.find('[name=cardNumber]').hasClass("success") &&
        $form.find('[name=cardExpiry]').hasClass("success") &&
        $form.find('[name=cardCVC]').val().length > 1) {
        return true;
    } else {
        return false;
    }
}

$form.find('.subscribe').prop('disabled', true);
var readyInterval = setInterval(function () {
    if (paymentFormReady()) {
        $form.find('.subscribe').prop('disabled', false);
        clearInterval(readyInterval);
    }
}, 250);

function AddCreditCard(data) {
    $.ajax({
        url: '/UserInfo/AddCreditCard',
        type: 'POST',
        async: false,
        data: data,
        processData: false,
        contentType: false
    }).success(function (result) {
        if (result.status) {
            $('#modaladdcredit').modal('hide');
            GetCredit(result.card);
            
            
        } else {
            swal("Error", result.message, "error");
        }
    }).error(function (xhr, status) {
    });
}
function GetCredit(data) {
    $.ajax({
        url: '/PaymentCheckOut/GetCard',
        contentType: 'application/html;charset=utf-8',
        type: 'get',
        async: false,
        dataType: 'html'
    }).success(function (result) {
        $('#show-credit').html(result);
        $('#showCredit').click(function () {
            GetModalCredit();
        });
        clickCheckOut();
        $('.creditDetails').text(data.CreditNumber);
        $('.creditDetails').data('id', data.CreditCardId);
        $('.creditEx').text(data.Expire);
    }).error(function (xhr, status) {

    });
}

function clickCheckOut() {
    $('#checkout').click(function () {
        var id = $('.creditDetails').data('id');
        swal({
            title: "Are you sure?",
            text: "Make sure your shipping address and payment method are secure.!",
            icon: "success",
            buttons: true,
            dangerMode: true,
        })
            .then((willCheckout) => {
                if (willCheckout) {
                    $('#loading').show();
                    ConfirmPayment(id);
                }
            });
    })

}


function ConfirmPayment(id) {
    var data = new FormData();
    data.append('id', id);
    $.ajax({
        url: '/PaymentCheckOut/Payment',
        type: 'POST',
        async: false,
        data: data,
        processData: false,
        contentType: false
    }).success(function (result) {
        if (result.status) {
            $('#loading').hide();
            swal("OK! Payment success  !", {
                icon: "success"
            });
            window.location.replace("/details/" + result.OrderId);
        } else {
            $('#loading').hide();
            swal("Error", result.message, "error");
        }
    }).error(function (xhr, status) {
    });
}