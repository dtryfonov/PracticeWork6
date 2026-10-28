using System.Collections;

CourseCatalog catalog =
[
    new("Основи C#", 18, 20, 4.7, 1400m),
    new("Єдність C#", 24, 26, 4.4, 1300m),
    new("Інтерфейси C#", 24, 22, 4.9, 1600m),
    new("Алгоритми", 30, 35, 4.9, 1800m),
    new("Бази даних", 16, 18, 3.8, 1100m)
];

Console.WriteLine("Каталог курсів за рейтингом:");
foreach (Course course in catalog)
{
    Console.WriteLine("  " + course);
}

Console.WriteLine("\nНе більше двох курсів дешевших за 1500 грн:");
foreach (Course course in catalog.CheaperThan(1500m, 2))
{
    Console.WriteLine("  " + course);
}

sealed class Course : IComparable<Course>
{
    public Course(string title, int lessons, int durationHours, double rating, decimal price)
    {
        Title = title;
        Lessons = lessons;
        DurationHours = durationHours;
        Rating = rating;
        Price = price;
    }

    public string Title { get; }
    public int Lessons { get; }
    public int DurationHours { get; }
    public double Rating { get; }
    public decimal Price { get; set; }

    public int CompareTo(Course? other)
    {
        return other is null ? 1 : other.Rating.CompareTo(Rating);
    }

    public override string ToString()
    {
        return $"{Title,-18} уроків: {Lessons,2}; годин: {DurationHours,2}; " +
               $"рейтинг: {Rating:F1}; ціна: {Price:F0} грн";
    }
}

sealed class CourseCatalog : IEnumerable<Course>
{
    private readonly List<Course> courses = [];

    public CourseCatalog()
    {
    }

    public CourseCatalog(IEnumerable<Course> courses)
    {
        this.courses.AddRange(courses);
    }

    public void Add(Course course)
    {
        courses.Add(course);
    }

    public IEnumerator<Course> GetEnumerator()
    {
        List<Course> sorted = [.. courses];
        sorted.Sort();

        foreach (Course course in sorted)
        {
            yield return course;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<Course> CheaperThan(decimal price, int count)
    {
        int returned = 0;

        foreach (Course course in this)
        {
            if (returned == count)
            {
                yield break;
            }

            if (course.Price < price)
            {
                returned++;
                yield return course;
            }
        }
    }
}