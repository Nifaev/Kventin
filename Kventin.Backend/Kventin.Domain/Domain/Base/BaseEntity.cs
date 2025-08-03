namespace Kventin.DataAccess.Domain.Base
{
    /// <summary>
    /// Базовая сущность
    /// </summary>
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            CreateDateTime = DateTime.Now;
        }

        /// <summary>
        /// Id сущности
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата и время создания
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Дата и время удаления
        /// </summary>
        public DateTime? DeleteDateTime { get; set; }
    }
}
