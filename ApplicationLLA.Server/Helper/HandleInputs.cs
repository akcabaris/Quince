using System.Text.RegularExpressions;

namespace ApplicationLLA.Server.Helper
{
    public static class HandleInputs
    {
        public static string HandleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ").Trim();
        }

        public static bool IsGenderValueValid(this string value)
        {
            if(value == "Male" || value == "Female" || value == "Unspecified")
            {
                return true;
            }
            return false;
        }

        public static bool IsPhoneNumberValidFormatTR(this Int64 value)
        {
            if(value <=5999999999 && value >= 5000000000)
            {
                return true;
            }
            return false;
        }
        public static bool IsCategoryExists(this string value, List<string> categoryList) {

            bool isExist = false;

            foreach(string category in categoryList)
            {
                if(category == value)
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        // I have to Check city and county for Creating Post, I'm gonna add list for this Later


        public static bool IsCurrencyValid(this string value, List<string> currencyList)
        {
            bool isValid = false;

            foreach (string category in currencyList)
            {
                if (category == value)
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        public static bool IsWorkUnitValid(this string value, List<string> workUnitList)
        {
            bool isValid = false;

            foreach (string category in workUnitList)
            {
                if (category == value)
                {
                    isValid = true;
                }
            }
            return isValid;

        }
        public static bool IsCityValidTR(this string value, List<string> cityList)
        {
            bool isValid = false;

            foreach (string category in cityList)
            {
                if (category == value)
                {
                    isValid = true;
                }
            }
            return isValid;

        }
    }
}
