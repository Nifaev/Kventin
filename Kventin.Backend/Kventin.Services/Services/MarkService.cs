using Kventin.DataAccess;
using Kventin.DataAccess.Domain;
using Kventin.DataAccess.Enums;
using Kventin.Services.Dtos.Marks;
using Kventin.Services.Infrastructure.Exceptions;
using Kventin.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Kventin.Services.Services
{
    public class MarkService(KventinContext db) : IMarkService
    {
        private readonly KventinContext _db = db;

        public async Task AssignMarksForExercise(int teacherId, AssignMarksForExerciseDto dto)
        {
            var exercise = await _db.Exercises
            .Include(x => x.StudyGroup)
                .ThenInclude(x => x.Students)
            .Include(x => x.IndividualStudent)
            .FirstOrDefaultAsync(x => x.Id == dto.ExerciseId)
            ?? throw new EntityNotFoundException("Задание с таким Id не найдено");

            var studentIds = dto.StudentMarks
                .Select(x => x.StudentId)
                .ToList();

            if (exercise.IsIndividual && 
                studentIds.Count != 1 && 
                exercise.IndividualStudentId.HasValue &&
                !studentIds.Contains(exercise.IndividualStudentId.Value))
            {
                throw new ArgumentException("Вы можете поставить оценку за индивидуальное задание только тому ученику, которому оно выдано");
            }

            var teacher = await _db.Users.FindAsync(teacherId);

            var students = await _db.Users
            .Where(x => studentIds.Contains(x.Id) &&
                        x.Roles.Any(y => y.Name == "Student"))
            .ToListAsync();

            foreach (var studentMark in dto.StudentMarks)
            {
                var student = students.FirstOrDefault(x => x.Id == studentMark.StudentId);

                if (student == null)
                    continue;

                var marksToAdd = studentMark.Marks
                    .Select(x => new Mark
                    {
                        Value = x.MarkValue,
                        Teacher = teacher!,
                        Student = student,
                        Comment = x.TeacherComment,
                        MarkType = MarkType.ForExercise,
                    })
                    .ToList();

                exercise.Marks.AddRange(marksToAdd);
            }

            await _db.SaveChangesAsync();
        }

        public async Task AssignMarksForLesson(int teacherId, AssignMarksForLessonDto dto)
        {
            var lesson = await _db.Lessons.FindAsync(dto.LessonId)
                ?? throw new EntityNotFoundException("Занятие с таким Id не найдено");

            var teacher = await _db.Users.FindAsync(teacherId);

            var studentIds = dto.StudentMarks
                .Select(x => x.StudentId)
                .ToList();

            var students = await _db.Users
                .Where(x => studentIds.Contains(x.Id) &&
                            x.Roles.Any(y => y.Name == "Student"))
                .ToListAsync();

            foreach (var studentMark in dto.StudentMarks)
            {
                var student = students.FirstOrDefault(x => x.Id == studentMark.StudentId);

                if (student == null)
                    continue;

                var marksToAdd = studentMark.Marks
                    .Select(x => new Mark
                    {
                        Value = x.MarkValue,
                        Teacher = teacher!,
                        Student = student,
                        Comment = x.TeacherComment,
                        MarkType = MarkType.ForLesson,
                    })
                    .ToList();

                if (!lesson.StudentsAttended.Any(x => x.Id == student.Id))
                    lesson.StudentsAttended.Add(student);

                lesson.Marks.AddRange(marksToAdd);
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteMark(int markId)
        {
            var mark = await _db.Marks.FindAsync(markId)
                ?? throw new EntityNotFoundException("Оценка с таким Id не найдена");

            _db.Marks.Remove(mark);

            await _db.SaveChangesAsync();
        }

        public async Task UpdateMark(int markId, MarkShortInfoDto dto)
        {
            var mark = await _db.Marks.FindAsync(markId)
                ?? throw new EntityNotFoundException("Оценка с таким Id не найдена");

            mark.Value = dto.MarkValue;

            if (!string.IsNullOrWhiteSpace(dto.TeacherComment))
                mark.Comment = dto.TeacherComment;

            await _db.SaveChangesAsync();
        }
    }
}
