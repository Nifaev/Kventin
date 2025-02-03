using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Enums;
using Kventin.Services.Dtos.Schedule;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Infrastructure.Extensions;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class ScheduleService(KventinContext db) : IScheduleService
    {
        private readonly KventinContext _db = db;

        public async Task CreateSchedule(ScheduleDto dto)
        {
            var schedule = await _db.Schedules
                .SingleOrDefaultAsync(x => x.StartYear == dto.StartYear &&
                                           x.EndYear == dto.EndYear);

            if (schedule != null)
                throw new EntityAlreadyCreatedException($"Расписание на учебный год {dto.StartYear}/{dto.EndYear} уже существует.");

            schedule = new Schedule
            {
                StartYear = dto.StartYear,
                EndYear = dto.EndYear,
            };

            await _db.Schedules.AddAsync(schedule);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ReturnScheduleItemDto>> GetSchedule(ScheduleDto dto)
        {
            var scheduleId = await _db.Schedules
                .Where(x => x.StartYear == dto.StartYear &&
                            x.EndYear == dto.EndYear)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (scheduleId == 0)
                throw new EntityNotFoundException($"Расписание на учебный год {dto.StartYear}/{dto.EndYear} не найдено");

            var scheduleItems = await _db.ScheduleItems
                .Include(x => x.Teacher)
                .Include(x => x.Subject)
                .Include(x => x.StudyGroup)
                .Where(x => x.ScheduleId == scheduleId)
                .ToListAsync();

            var result = scheduleItems
                .Select(x =>  new ReturnScheduleItemDto
                {
                    ScheduleId = scheduleId,
                    ScheduleItemId = x.Id,
                    DayOfWeek = x.DayOfWeek.GetDescription(),
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Classroom = x.Classroom,
                    TeacherShortName = x.Teacher.GetShortName(),
                    SubjectName = x.Subject.Name,
                    IsOnline = x.IsOnline,
                    GroupName = x.StudyGroup.Name
                })
                .ToList();

            return result;
        }

        public async Task AddScheduleItem(AddScheduleItemDto dto)
        {
            var teacher = await _db.Users.FindAsync(dto.TeacherId);
            var subject = await _db.Subjects.FindAsync(dto.SubjectId);
            var group = await _db.StudyGroups.FindAsync(dto.GroupId);

            var schedule = await _db.Schedules.FindAsync(dto.ScheduleId);

            var scheduleItem = new ScheduleItem
            {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Classroom = dto.Classroom,
                DayOfWeek = dto.DayOfWeek,
                IsOnline = dto.IsOnline,
                Subject = subject!,
                StudyGroup = group!,
                Teacher = teacher!,
                Schedule = schedule!
            };

            await _db.ScheduleItems.AddAsync(scheduleItem);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateScheduleItem(UpdateScheduleItemDto dto, int itemId)
        {
            var scheduleItem = await _db.ScheduleItems.FindAsync(itemId)
                ?? throw new EntityNotFoundException("Элемент расписания с укзанным Id не найден");

            if (dto.TeacherId.HasValue)
                scheduleItem.Teacher = await _db.Users.FindAsync(dto.TeacherId.Value)
                    ?? throw new EntityNotFoundException("Преподаватель с указанным Id не найден");

            if (dto.SubjectId.HasValue)
                scheduleItem.Subject = await _db.Subjects.FindAsync(dto.SubjectId.Value)
                    ?? throw new EntityNotFoundException("Предмет с указанным Id не найден");

            if (dto.StudyGroupId.HasValue)
                scheduleItem.StudyGroup = await _db.StudyGroups.FindAsync(dto.StudyGroupId.Value)
                    ?? throw new EntityNotFoundException("Группа с указанным Id не найдена");

            scheduleItem.IsOnline = dto.IsOnline ?? scheduleItem.IsOnline;
            scheduleItem.Classroom = dto.Classroom ?? scheduleItem.Classroom;
            scheduleItem.DayOfWeek = dto.DayOfWeek ?? scheduleItem.DayOfWeek;
            scheduleItem.StartTime = dto.StartTime ?? scheduleItem.StartTime;
            scheduleItem.EndTime = dto.EndTime ?? scheduleItem.EndTime;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteScheduleItem(int itemId)
        {
            var scheduleItem = await _db.ScheduleItems.FindAsync(itemId);

            if (scheduleItem != null)
            {
                _db.ScheduleItems.Remove(scheduleItem);
                await _db.SaveChangesAsync();
            }
        }
    }
}
