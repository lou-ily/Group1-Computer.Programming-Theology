using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AugustinianSanctuary;

public sealed class AugustinianSanctuaryModel : PageModel
{
    private static readonly GospelVerse[] GospelVerses =
    [
        new("Matthew 5:1-12", "Blessed are the merciful, for they will be shown mercy."),
        new("John 15:9-12", "Remain in my love and let love guide what you do."),
        new("Matthew 11:28-30", "Come to me, all you who are weary, and I will give you rest."),
        new("John 14:27", "Peace I leave with you; my peace I give you."),
        new("Matthew 22:37-39", "Love God with all your heart and love your neighbor as yourself.")
    ];

    public GospelVerse DailyGospel { get; private set; } = GospelVerses[0];
    public string? PrayerMessage { get; private set; }

    [BindProperty]
    public PrayerInput Prayer { get; set; } = new();

    public void OnGet()
    {
        DailyGospel = GospelVerses[DateTime.UtcNow.DayOfYear % GospelVerses.Length];
    }

    public IActionResult OnPostPrayer()
    {
        if (!ModelState.IsValid)
        {
            OnGet();
            return Page();
        }

        PrayerMessage = "Your prayer request has been received.";
        Prayer = new();
        OnGet();
        return Page();
    }

    public static Dare GetRandomDare(DareCategory category)
    {
        var dares = category == DareCategory.Myself
            ? [
                new Dare("Prayer", "A Quiet Moment", "Spend a few quiet minutes in prayer.", category),
                new Dare("Gratitude", "Gratitude List", "Write down three things you are grateful for.", category),
                new Dare("Reflection", "Reflect on Scripture", "Read a Bible passage and reflect on its message.", category)
            ]
            : [
                new Dare("Kindness", "Give a Compliment", "Give someone a sincere compliment.", category),
                new Dare("Service", "Help Someone", "Help someone without expecting anything in return.", category),
                new Dare("Prayer", "Pray for Someone", "Pray for someone who may be struggling.", category)
            ];

        return dares[Random.Shared.Next(dares.Length)];
    }
}

public sealed class PrayerInput
{
    [BindProperty, System.ComponentModel.DataAnnotations.Required]
    public string Name { get; set; } = string.Empty;

    [BindProperty, System.ComponentModel.DataAnnotations.Required]
    public string Request { get; set; } = string.Empty;
}

public sealed record GospelVerse(string Reference, string Verse);

public sealed record Dare(string Category, string Title, string Description, DareCategory Type);

public enum DareCategory
{
    Myself,
    Others
}
