initQuantity();

function initQuantity() {
    // Handle product quantity input
    if ($('.product_quantity').length) {

        var incButton = $('.quantity_inc_button');
        var decButton = $('.quantity_dec_button');

        incButton.on('click', function () {
            var price = $(this).closest(".cart_item").find(".cart_item_price").text();
            var subtotal = $(this).closest(".cart_item").find(".cart_item_total");
            var input = $(this).closest(".product_quantity").find('.quantity_input');
            var originalVal = input.val();
            var endVal = parseFloat(originalVal) + 1;
            var productID = $(this).closest(".cart_item").find('input[type=hidden]').val();

            input.val(endVal);
            subtotal.text((price * endVal).toFixed(2));
            addToCart(productID, 1);



        })

        decButton.on('click', function () {
            var price = $(this).closest(".cart_item").find(".cart_item_price").text();
            var subtotal = $(this).closest(".cart_item").find(".cart_item_total");
            var input = $(this).closest(".product_quantity").find('.quantity_input');
            var originalVal = input.val();
            var productID = $(this).closest(".cart_item").find('input[type=hidden]').val();

            if (originalVal > 1) {
                var endVal = parseFloat(originalVal) - 1;
                input.val(endVal);
                subtotal.text((price * endVal).toFixed(2));
                addToCart(productID, -1);


            }

        })


    }
}

//quantity_input validateNum
var countInputs = document.querySelectorAll(".quantity_input");

countInputs.forEach(function (x) {
    var productID = $(x).closest(".cart_item").find('input[type=hidden]').val();
    var originCount = x.value;
    x.addEventListener('blur', function () {
        var currentCount = x.value;
        validateNum(productID, x, originCount, currentCount);
    })
});

function validateNum(pID, input, originCount, currentCount) {
    let reg = /^\d+$/;
    if (!reg.test(input.value)) {
        Swal.fire('please enter number', '', 'warning').then(() => {
            input.value = originCount;
            addToCart(pID, originCount, "addMany");
        });
    }
    else {
        addToCart(pID, currentCount, "addMany");

    }
}

