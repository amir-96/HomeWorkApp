namespace Domain.BaseModels
{
  public static class Roles
  {
    public const string Admin = "Admin";
    public const string Teacher = "Teacher";
    public const string Student = "Student";

    private static readonly Dictionary<string, string> RoleTranslations = new Dictionary<string, string>
        {
            { "Admin", "مدیر" },
            { "Teacher", "استاد" },
            { "Student", "دانشجو" }
        };

    public static List<string> GetRoles()
    {
      return RoleTranslations.Keys.ToList();
    }

    public static string TranslateRole(string roleName)
    {
      if (RoleTranslations.ContainsKey(roleName))
      {
        return RoleTranslations[roleName];
      }
      return roleName;
    }
  }
}