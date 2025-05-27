namespace ChatApp.Front.Utils
{
    public static class RoleParser
    {
        public static string ParseStringToText(string? role)
        {
            if (role == "2" || role == "Member")
            {
                return "Member";
            } else if (role == "1" || role == "Admin")
            {
                return "Admin";
            } else
            {
                return "";
            }   
        }

        public static int ParseStringToInt(string? role)
        {
            if (role == "2" || role == "Member")
            {
                return 2;
            } else if (role == "1" || role == "Admin")
            {
                return 1;
            } else
            {
                return 0;
            }
        }

        public static string ParseIntToString(int? role)
        {
            if (role == 2)
            {
                return "Member";
            } else if (role == 1)
            {
                return "Admin";
            } else
            {
                return "Unspecified";
            }
        }
    }
}
