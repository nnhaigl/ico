
namespace ICOCore.Infrastructures.Enums
{
    public enum UserReferralStatusEnum : byte
    {
        /// <summary>
        /// Chưa click vào link confirm trong mail
        /// </summary>
        NOT_CONFIRMED = 0,
        /// <summary>
        /// Sau Khi click vào link trong mail cần chờ ng giới thiệu phê duyệt
        /// </summary>
        PENDING = 1,
        /// <summary>
        /// Người giới thiệu đã phê duyệt.
        /// </summary>
        ACTIVE = 2,
        /// <summary>
        /// Admin đã xác nhận thông tin cá nhân (PID , Ảnh v.v.)
        /// </summary>
        COMPLETE = 3,
        INACTIVE = 4
    }
}
