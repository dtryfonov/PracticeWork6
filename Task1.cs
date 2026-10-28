using System.Globalization;

List<Course> courses =
[
    new("Основи C#", 18, 20, 4.7, 1400m),
    new("Єдність C#", 24, 26, 4.4, 1300m),
    new("Інтерфейси C#", 24, 22, 4.9, 1600m),
    new("Алгоритми", 30, 35, 4.9, 1800m),
    new("Бази даних", 16, 18, 3.8, 1100m)
];

courses.Sort();
Print("Курси за рейтингом (від найвищого)", courses);

courses.Sort(new ByDurationThenTitle());
Print("Курси за тривалістю, потім за назвою", courses);

static void Print(string title, IEnumerable<Course> courses)
{
    Console.WriteLine(title + ":");
    foreach (Course course in courses)
    {
        Console.WriteLine("  " + course);
    }

    Console.WriteLine();
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
        if (other is null) return 1;
        return other.Rating.CompareTo(Rating);
    }

    public Course Copy() => new(Title, Lessons, DurationHours, Rating, Price);

    public override string ToString() =>
        $"{Title,-18} уроків: {Lessons,2}; годин: {DurationHours,2}; " +
        $"рейтинг: {Rating:F1}; ціна: {Price:F0} грн";
}

sealed class ByDurationThenTitle : IComparer<Course>
{
    private static readonly StringComparer UkrainianNames =
        StringComparer.Create(new CultureInfo("uk-UA"), ignoreCase: false);

    public int Compare(Course? x, Course? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        int result = x.DurationHours.CompareTo(y.DurationHours);
        return result != 0 ? result : UkrainianNames.Compare(x.Title, y.Title);
    }
}