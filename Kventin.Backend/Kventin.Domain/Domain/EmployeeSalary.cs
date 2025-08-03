using Kventin.DataAccess.Domain.Base;

namespace Kventin.DataAccess.Domain
{
    /// <summary>
    /// Выплата зарплаты
    /// </summary>
    public class EmployeeSalary : BasePayment
    {
        /// <summary>
        /// Id сотрудника
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Сотрудник
        /// </summary>
        public required User Employee { get; set; }
    }
}
