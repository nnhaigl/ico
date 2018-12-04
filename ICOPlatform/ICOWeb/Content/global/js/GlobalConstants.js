var _ORDER_STATUS = {
    NEW: 0,
    APPROVED: 1,
    IN_DISPUTE: 2,
    LOCK: 3,
    COMPLETE: 4,
};

var _ORDER_TYPE = {
    SELL: 0, // đăng bán
    BUY: 1 // đăng mua
};

var _TRANS_STATUS = {
    WAITING_TRANS_BTC_TO_STORE: 0, // chờ ng bán chuyển tiền vào kho
    WAITING_PAYMENT: 1, // Chờ ng mua thanh toán
    PAID: 2, // Ng mua đã xác nhận thanh toán => Đợi ng bán confirm nhận tiền
    CONFIRM_PAYMENT: 3, // ng bán đã confirm nhận tiền => Chờ chuyển BTC từ Store cho người mua
    COMPLETE: 4, // Giao dịch thành công
    IN_DISPUTE: 5, // Tranh chấp
    CANCEL: 6, // Giao dịch bị hủy bỏ
    LOCK: 7, // Giao dịch đang bị khóa
    TIMEOUT : 8 // Giao dịch timeout khi ng mua không thanh toán
};

var _SESSION_STORAGE = {
    USER: "_sess_user"
};

var _REFERRAL_STATUS = {
    PENDING: 1,
    ACTIVE: 2,
    COMPLETE: 3,
    INACTIVE: 4
};