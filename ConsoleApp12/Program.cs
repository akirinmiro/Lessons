using System;
using System.Collections.Generic;
using System.Threading;


namespace LessonS
{
    class Program
    {
        private static List<Lesson> lessons = new List<Lesson>
        {
            new Lesson { Subject = "Математика", StartTime = new TimeSpan(20, 53, 0), EndTime = new TimeSpan(20, 55, 0) },
            new Lesson { Subject = "Физическая культура", StartTime = new TimeSpan(21, 0, 0), EndTime = new TimeSpan(21, 2, 0) },
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Ожидание начала урока...");

            Timer timer = new Timer(CheckLesson, null, 0, 1000);

            Console.ReadLine();
        }

        static bool lessonStarted = false;
        static Lesson currentLesson = null;
        static bool lessonDurationPrinted = false;
        static bool lessonEndedPrinted = false;
        static TimeSpan timeToNextLesson = TimeSpan.MaxValue;

        static void CheckLesson(object state)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (!lessonStarted)
            {
                Lesson nextLesson = null;
                TimeSpan newTimeToNextLesson = TimeSpan.MaxValue;

                foreach (Lesson lesson in lessons)
                {
                    if (now < lesson.StartTime && lesson.StartTime - now < newTimeToNextLesson)
                    {
                        newTimeToNextLesson = lesson.StartTime - now;
                        nextLesson = lesson;
                    }
                }

                if (nextLesson != null)
                {
                    timeToNextLesson = newTimeToNextLesson;
                    Console.WriteLine($"Время до следующего урока: {timeToNextLesson.Hours} ч. {timeToNextLesson.Minutes} мин.");
                    lessonStarted = true;
                    currentLesson = nextLesson;
                    lessonDurationPrinted = false;
                    lessonEndedPrinted = false;
                }
            }
            else
            {
                if (now >= currentLesson.StartTime && now < currentLesson.EndTime)
                {
                    if (!lessonDurationPrinted)
                    {
                        Console.WriteLine($"Текущий урок: {currentLesson.Subject}, Длительность: {(currentLesson.EndTime - currentLesson.StartTime).Hours} ч. {(currentLesson.EndTime - currentLesson.StartTime).Minutes} мин.");
                        lessonDurationPrinted = true;
                    }
                    lessonEndedPrinted = false;
                }
                else if (now >= currentLesson.EndTime && !lessonEndedPrinted)
                {
                    Console.WriteLine($"Урок {currentLesson.Subject} закончился");
                    lessonEndedPrinted = true;
                }

                if (now >= currentLesson.EndTime && timeToNextLesson != TimeSpan.MaxValue)
                {
                    Console.WriteLine($"Ожидание начала следующего урока...");
                    lessonStarted = false;
                    currentLesson = null;
                    lessonDurationPrinted = false;
                }

              
            }
        }

        class Lesson
        {
            public string Subject { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }
    }
}