using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Enums;
using Kventin.Services.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Infrastructure.Tools
{
    public class LessonGenerator
    {
        private readonly KventinContext _db;
        private readonly  DateOnly _today = DateOnly.FromDateTime(DateTime.Now);

        public LessonGenerator(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KventinContext>();

            optionsBuilder.UseSqlServer(connectionString);

            _db = new KventinContext(optionsBuilder.Options);
        }

        public async Task GenerateLessons()
        {
            var inTwoWeeks = _today.AddDays(14);

            // Если на текущий момент январь - август, то текущий год - конец 
            // Если сентябрь - декабрь - начало
            var schedule = _today.Month >= 1 && _today.Month <= 8
                ? await _db.Schedules.FirstOrDefaultAsync(x => x.EndYear == _today.Year)
                : await _db.Schedules.FirstOrDefaultAsync(x => x.StartYear == _today.Year);

            // Если расписание еще не создано, то ничего не делаем
            if (schedule == null)
                return;

            var scheduleItems = await _db.ScheduleItems
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                .Include(x => x.StudyGroup)
                .Where(x => x.ScheduleId == schedule.Id)
                .ToListAsync();

            if (scheduleItems.Count == 0)
                return;

            var scheduleItemIds = scheduleItems.Select(x => x.Id).ToList();

            // Нужен ли Include?
            // Выбираем уже созданные занятия, чтобы не дублировать их
            // Не включаем сегодняшнюю дату, т.к. метод выполняется в 20:00 в воскресенье
            var lessons = await _db.Lessons
                .Include(x => x.ScheduleItem)
                .Where(x => x.ScheduleItem != null &&
                            scheduleItemIds.Contains(x.ScheduleItem.Id) &&
                            x.Date > _today &&
                            x.Date <= inTwoWeeks)
                .Select(x => new
                {
                    LessonId = x.Id,
                    ScheduleItemId = x.ScheduleItem!.Id,
                    x.Date,
                })
                .ToListAsync();

            foreach (var scheduleItem in scheduleItems)
            {
                var createdLessons = lessons
                    .Where(x => x.ScheduleItemId == scheduleItem.Id)
                    .ToList();

                var intoWeek = scheduleItem.DayOfWeek.GetNext(_today);
                var intoTwoWeeks = scheduleItem.DayOfWeek.GetNext(_today, 1);

                var lessonIntoWeekCreated = createdLessons.Any(x => x.Date == intoWeek);
                var lessonIntoTwoWeeksCreated = createdLessons.Any(x => x.Date == intoTwoWeeks);

                if (lessonIntoWeekCreated && lessonIntoTwoWeeksCreated)
                    continue;
                else if (lessonIntoWeekCreated)
                    await MapScheduleItemToLesson(scheduleItem, intoTwoWeeks);
                else if (lessonIntoTwoWeeksCreated)
                    await MapScheduleItemToLesson(scheduleItem, intoWeek);
                else
                {
                    await MapScheduleItemToLesson(scheduleItem, intoWeek);
                    await MapScheduleItemToLesson(scheduleItem, intoTwoWeeks);
                }
            }

            await _db.SaveChangesAsync();
        }

        private async Task MapScheduleItemToLesson(ScheduleItem scheduleItem, DateOnly lessonDate)
        {
            var lesson = new Lesson()
            {
                StartTime = scheduleItem.StartTime,
                EndTime = scheduleItem.EndTime,
                Date = lessonDate,
                Subject = scheduleItem.Subject,
                Teacher = scheduleItem.Teacher,
                StudyGroup = scheduleItem.StudyGroup,
                IsOnline = scheduleItem.IsOnline,
                Classroom = scheduleItem.Classroom,
                Status = LessonStatus.Planned,
                ScheduleItem = scheduleItem,
            };

            await _db.Lessons.AddAsync(lesson);

            scheduleItem.Lessons.Add(lesson);
        }
    }
}
