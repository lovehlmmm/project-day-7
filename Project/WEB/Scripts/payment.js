$('#showCredit').click(function () {
    //$('#modalCredit').modal();
     GetModalCredit();
});


$('.showAddCredit').click(function () {
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
                url: '/PaymentCheckOut/GetCredit?id='+checkId,
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
        })
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
        number: $form.find('[name=cardNumber]').val().replace(/\s/g, ''),
        cvc: $form.find('[name=cardCVC]').val(),
        exp_month: expiry.month,
        exp_year: expiry.year
    };

    Stripe.card.createToken(ccData, function stripeResponseHandler(status, response) {
        if (response.error) {
            /* Visual feedback */
            $form.find('.subscribe').html('Try again').prop('disabled', false);
            /* Show Stripe errors on the form */
            $form.find('.payment-errors').text(response.error.message);
            $form.find('.payment-errors').closest('.row').show();
        } else {
            /* Visual feedback */
            $form.find('.subscribe').html('Processing <i class="fa fa-spinner fa-pulse"></i>');
            /* Hide Stripe errors on the form */
            $form.find('.payment-errors').closest('.row').hide();
            $form.find('.payment-errors').text("");
            // response contains id and card, which contains additional card details            
            console.log(response.id);
            console.log(response.card);
            var token = response.id;
            // AJAX - you would send 'token' to your server here.
            $.post('/account/stripe_card_token', {
                token: token
            })
                // Assign handlers immediately after making the request,
                .done(function (data, textStatus, jqXHR) {
                    $form.find('.subscribe').html('Payment successful <i class="fa fa-check"></i>');
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $form.find('.subscribe').html('There was a problem').removeClass('success').addClass('error');
                    /* Show Stripe errors on the form */
                    $form.find('.payment-errors').text('Try refreshing the page and trying again.');
                    $form.find('.payment-errors').closest('.row').show();
                });
        }
    });
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