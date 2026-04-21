using API.Domain.Constants;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(DataContext context, UserManager<AppUser> userManager)
    {
        await SeedRolesAsync(context);
        await SeedUsersAsync(userManager);

        await SeedMeetingTypesAsync(context);
        await SeedMeetingCategoriesAsync(context);
    }

    // ─── Roles ───────────────────────────────────────────────────────────────

    private static async Task SeedRolesAsync(DataContext context)
    {
        var roles = new List<AppRole>
        {
            // Admin role with full permissions
            new AppRole
            {
                Name        = Roles.Admin,
                NameAr      = "مدير",
                DescriptionEn = "Full access to all system features.",
                DescriptionAr = "وصول كامل إلى جميع ميزات النظام."
            },
            // User role with limited permissions
            new AppRole
            {
                Name        = Roles.User,
                NameAr      = "مستخدم",
                DescriptionEn = "Access to basic features and functionalities.",
                DescriptionAr = "الوصول إلى الميزات والوظائف الأساسية."
            },
            new AppRole
            {
                Name        = Roles.Organizer,
                NameAr      = "منظم",
                DescriptionEn = "Can create and manage meetings.",
                DescriptionAr = "يمكنه إنشاء وإدارة الاجتماعات."
            },
            // Other roles can be added here in the future, such as "Manager" or "Support"
            
        };

        var CurrentRoles = await context.Roles.ToListAsync();
        foreach (var role in roles)
        {
            if (CurrentRoles.Any(r => r.Name != role.Name))
                await context.Roles.AddAsync(role);
        }

        await context.SaveChangesAsync();
    }

    // ─── Users ────────────────────────────────────────────────────────────────

    private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {
        var now = DateTime.Now;

        var seedUsers = new List<(AppUser User, string Password, string Role)>
        {
            (
                new AppUser
                {
                    UserName  = "admin",
                    Email     = "aamus2024@gmail.com",
                    FirstName = "System",
                    LastName  = "Admin",
                    Gender    = Gender.Male,
                    CreatedAt = now,
                    UpdatedAt = now,
                    IsActive  = true,
                    EmailConfirmed = true
                },
                "Admin@1234",
                Roles.Admin
            ),
                (
                    new AppUser
                    {
                        UserName  = "Omar",
                        Email     = "Omar@example.com",
                        FirstName = "Omar",
                        LastName  = "Example",
                        Gender    = Gender.Male,
                        CreatedAt = now,
                        UpdatedAt = now,
                        IsActive  = true
                    },
                    "Omar@1234",
                    Roles.User
            )

        };
        Console.WriteLine("Seeding users...");
        foreach (var (user, password, role) in seedUsers)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email!);
            if (existingUser is null)
            {
                var result = await userManager.CreateAsync(user, password);
                Console.WriteLine($"Creating user {user.Email}: {(result.Succeeded ? "Success" : "Failed")}");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, role);
                Console.WriteLine($"Assigning role '{role}' to user {user.Email}: Success");
            }
            else
            {
                // Ensure seeded users always have their expected role, even if user already existed.
                if (!await userManager.IsInRoleAsync(existingUser, role))
                {
                    await userManager.AddToRoleAsync(existingUser, role);
                    Console.WriteLine($"Assigning missing role '{role}' to existing user {existingUser.Email}: Success");
                }
            }
        }
        Console.WriteLine("User seeding completed.");
    }

    private static async Task SeedMeetingTypesAsync(DataContext context)
    {
        if (context.MeetingTypes.Any())
            return; // Already seeded

        var meetingTypes = new List<MeetingType>
        {
            new MeetingType
            {
                Name = "مجلس إدارة",
                Description = "اجتماعات لمناقشة استراتيجيات الشركة واتخاذ القرارات الهامة.",
            },
            new MeetingType
            {

                Name = "لجنة",
                Description = "اجتماعات تركز على مناقشة موضوع معين أو مشروع محدد، مثل لجنة تخطيط المشروع أو لجنة مراجعة الأداء."
            },

        };

        context.MeetingTypes.AddRange(meetingTypes);
        await context.SaveChangesAsync();
    }

    // meeting categories 
    private static async Task SeedMeetingCategoriesAsync(DataContext context)
    {
        if (context.MeetingCategories.Any())
            return; // Already seeded

        var categories = new List<MeetingCategory>
        {
            new MeetingCategory
            {
                Name = "دوري",
                Description = "اجتماعات منتظمة تحدث بشكل دوري، مثل الاجتماعات الأسبوعية أو الشهرية لمتابعة تقدم العمل ومناقشة القضايا المستمرة.",
            },
            new MeetingCategory
            {
                Name = "طارئ",
                Description = "اجتماعات غير مخطط لها تحدث استجابة لحدث أو مشكلة طارئة تتطلب اهتمامًا فوريًا، مثل اجتماع لمناقشة أزمة أو مشكلة حرجة في المشروع.",
            },
            new MeetingCategory
            {
               Name = "أمانة",
                Description = "اجتماعات تركز على مسائل الأمانة والامتثال، مثل اجتماعات لمراجعة السياسات والإجراءات أو مناقشة قضايا الامتثال التنظيمي."
            }
        };

        context.MeetingCategories.AddRange(categories);
        await context.SaveChangesAsync();
    }
}
