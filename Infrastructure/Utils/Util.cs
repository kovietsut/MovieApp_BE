using System.Net.Mail;

namespace VoteMovie.Infrastructure.Utils
{
    public static class Util
    {
        public static bool IsEmail(string emailAddress)
        {
            try
            {
                var m = new MailAddress(emailAddress);
                return m.Address == emailAddress;
            }
            catch (FormatException exception)
            {
                return false;
            }
        }

        public static bool IsNull(object value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }

            return true;
        }

        public static bool IsInt(object value)
        {
            if (IsNull(value))
                return false;

            int data;
            return int.TryParse(value.ToString(), out data);
        }

        public static int ToSafeInt(this string value)
        {
            return IsInt(value) ? int.Parse(value) : 0;
        }
    }
}
