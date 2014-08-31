
namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsType(this string obj, string type)
        {
            return obj.Equals(type) || obj.StartsWith(type);
        }
    }
}
