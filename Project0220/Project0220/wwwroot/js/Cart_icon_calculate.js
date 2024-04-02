    function calculateTotal() {
        var totalQuantity = 0;
        $(".update").each(function () {
            // 直接獲取並累加數量
            var quantity = parseInt($(this).val());
            totalQuantity += quantity;
        });
        // 更新總數量顯示到 #totalQuantity 元素
        $("#totalQuantity").text(totalQuantity);
        // 同時更新到購物車徽章 .cart-badge 元素
        $(".cart-badge").text(totalQuantity);
    }