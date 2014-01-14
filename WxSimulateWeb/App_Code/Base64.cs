using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxSimulateWeb.App_Code
{
    public sealed class Base64
    {
        public static string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encData = Convert.ToBase64String(encData_byte);
                return encData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Endode, " + e.Message);
            }
        }

        public static string base64Decode(string data)
        {
            try
            {
                byte[] decData_byte = Convert.FromBase64String(data);
                string decData = System.Text.Encoding.UTF8.GetString(decData_byte);
                return decData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode," + e.Message);
            }
        }
    }
}