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
        private readonly int _weeksCount;
        private readonly int _daysCount;
        private readonly DateOnly _today;
        private readonly DateOnly _lastDate;
        private readonly int _take = 100;

        public LessonGenerator(string connectionString, int weeksCount)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KventinContext>();

            optionsBuilder.UseSqlServer(connectionString);

            _db = new KventinContext(optionsBuilder.Options);

            _weeksCount = weeksCount;

            _daysCount = _weeksCount * 7;

            _today = DateOnly.FromDateTime(DateTime.Now);

            _lastDate = DateOnly.FromDateTime(DateTime.Now).AddDays(_daysCount);
        }

        public async Task GenerateLessons()
        {
            // Если на текущий момент январь - август, то текущий год - конец 
            // Если сентябрь - декабрь - начало
            var schedule = _today.Month >= 1 && _today.Month <= 8
                ? await _db.Schedules.FirstOrDefaultAsync(x => x.EndYear == _today.Year)
                : await _db.Schedules.FirstOrDefaultAsync(x => x.StartYear == _today.Year);

            // Если расписание еще не создано, то ничего не делаем
            if (schedule == null)
                return;

            var scheduleItemsQuery = _db.ScheduleItems
                .Include(x => x.Subject)
                .Include(x => x.Teacher)
                .Include(x => x.StudyGroup)
                .Where(x => x.ScheduleId == schedule.Id);

            var scheduleItems = await GetEntitiesPaged(scheduleItemsQuery);

            var scheduleItemIds = scheduleItems.Select(x => x.Id).ToList();

            var lessonsQuery = _db.Lessons
                .Include(x => x.ScheduleItem)
                .Where(x => x.ScheduleItem != null &&
                            scheduleItemIds.Contains(x.ScheduleItem.Id) &&
                            x.Date > _today &&
                            x.Date <= _lastDate)
                .Select(x => new
                {
                    LessonId = x.Id,
                    ScheduleItemId = x.ScheduleItem!.Id,
                    x.Date,
                });

            var lessons = await GetEntitiesPaged(lessonsQuery);

            foreach (var scheduleItem in scheduleItems)
            {
                var createdLessons = lessons
                    .Where(x => x.ScheduleItemId == scheduleItem.Id)
                    .ToList();

                for (int weekNumber = 0; weekNumber < _weeksCount; weekNumber++)
                {
                    var thisWeekLessonDate = scheduleItem.DayOfWeek.GetNext(_today, weekNumber);

                    var wasLessonCreated = createdLessons.Any(x => x.Date == thisWeekLessonDate);

                    if (!wasLessonCreated)
                    {
                        await MapScheduleItemToLesson(scheduleItem, thisWeekLessonDate);
                    }
                }
            }

            await _db.SaveChangesAsync();
        }

        private async Task<List<T>> GetEntitiesPaged<T>(IQueryable<T> query)
        {
            var itemsCount = await query.CountAsync();
            var scheduleItems = new List<ScheduleItem>();
            var pageCount = Math.Ceiling(itemsCount / (double)_take);
            var result = new List<T>();

            if (itemsCount == 0)
                return result;

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
            {
                var items = await query
                    .Skip(pageNumber * _take)
                    .Take(_take)
                    .ToListAsync();

                result.AddRange(items);
            }

            return result;
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

            scheduleItem.Lessons.Add(lesson);
            await _db.Lessons.AddAsync(lesson);
        }
    }
}
