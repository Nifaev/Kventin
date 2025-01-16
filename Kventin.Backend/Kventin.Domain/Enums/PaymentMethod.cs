namespace Kventin.DataAccess.Enums
{
    /// <summary>
    /// Способ оплаты
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// Оплата картой
        /// </summary>
        Card = 0,

        /// <summary>
        /// Оплата наличными
        /// </summary>
        Cash = 1,

        /// <summary>
        /// Оплата переводом
        /// </summary>
        BankTransfer = 2
    }
}
