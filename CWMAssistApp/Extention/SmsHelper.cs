namespace CWMAssistApp.Extention
{
    public static class SmsHelper
    {
        public static string ConvertPhoneNumberToSmsType(string phoneNumber)
        {
            var response = phoneNumber.Replace(" ", "");
            response = response.Replace("(", "");
            response = response.Replace(")", "");
            response = response.TrimStart('0');
            return response;
        }
    }
}
