using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace CarParts.Common
{
    public class Generator
    {
        public static bool IsOk { get; set; }
        public static string IsReport { get; set; }
        public static string Message { get; set; }

        public static string AdminPhotoPath = HostingEnvironment.MapPath("~/Images/Admin_Images/");
        public static string CustomerPhotoPath = HostingEnvironment.MapPath("~/Images/Customer_Images/");
        public static string CompanyImagePath = HostingEnvironment.MapPath("~/Images/Company_Images/");
        public static string MainCategoryImagePath = HostingEnvironment.MapPath("~/Images/MainCategory_Images/");
        public static string SubCategoryImagePath = HostingEnvironment.MapPath("~/Images/SubCategory_Images/");
        public static string ProductCategoryImagePath = HostingEnvironment.MapPath("~/Images/ProductCategory_Images/");
        public static string ProductImagePath = HostingEnvironment.MapPath("~/Images/Product_Images/");
        public static string ProductBrandImagePath = HostingEnvironment.MapPath("~/Images/ProductBrand_Images/");
        public static string MasterCompanyImagePath = HostingEnvironment.MapPath("~/Images/MasterCompany_Images/");
        public static string FnFPath = HostingEnvironment.MapPath("~/Images/fnf.png");
        public static string PaidPath = HostingEnvironment.MapPath("~/Images/paid.png");
        public const int ReadStreamBufferSize = 1024 * 1024;
        public static List<string> WeekDays = new List<string> { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thrusday", "Friday" };
        public static List<string> Months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public static List<string> PaymentSettingsName = new List<string> { "Parcentage", "Amount" };
        public static List<string> FloorImageSizeUnit = new List<string> { "MB", "KB" };
        public static List<string> PaymentMethodList = new List<string> { "Digital", "Cash" };
        public static string CompanySignaturePath = HostingEnvironment.MapPath("~/Images/Company_Signature/");
        private static Random random = new Random();


        // We have a read-only dictionary for mapping file extensions and MIME names. 
        public static readonly IReadOnlyDictionary<string, string> MimeNames;

        public static byte[] ConvertStreamToByteArray(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {

                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static string GenerateBase64StringFromByteArray(byte[] imageBytes)
        {
            if (imageBytes != null)
            {
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool SendEmail(string senderEmailHostAddress, int? senderEmailPort, string senderEmailAddress, string senderEmailCredential, string senderDisplayName, string recipientAddress, string messageSubject, string messageBody)
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(senderEmailAddress, senderDisplayName)
            };
            mail.To.Add(new MailAddress(recipientAddress));

            mail.Subject = messageSubject;
            mail.Body = messageBody;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.SubjectEncoding = System.Text.Encoding.Default;

            SmtpClient client = new SmtpClient
            {
                Host = senderEmailHostAddress,
                Port = (int)senderEmailPort,
                EnableSsl = true,
                Timeout = 10000,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmailAddress, senderEmailCredential)
            };
            try
            {
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public static string GenerateRandomCodeStringByByteSize(int _byte)
        {
            var randomNumber = new byte[_byte];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                string s = Convert.ToBase64String(randomNumber);
                s = Regex.Replace(s, @"[$&+,:;=?@#|'<>/\\.^*()%!-]", "");
                return s;
            }
        }

        public static string GenerateTrnx(int _byte)
        {
            var randomNumber = new byte[_byte];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                string s = Convert.ToBase64String(randomNumber);
                return s;
            }
        }

        public static string RandomStringByLength(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void CreatePartialContent(Stream inputStream, Stream outputStream, long start, long end)
        {
            int count = 0;
            long remainingBytes = end - start + 1;
            long position = start;
            byte[] buffer = new byte[ReadStreamBufferSize];

            inputStream.Position = start;
            do
            {
                try
                {
                    if (remainingBytes > ReadStreamBufferSize)
                        count = inputStream.Read(buffer, 0, ReadStreamBufferSize);
                    else
                        count = inputStream.Read(buffer, 0, (int)remainingBytes);
                    outputStream.Write(buffer, 0, count);
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error);
                    break;
                }
                position = inputStream.Position;
                remainingBytes = end - position + 1;
            } while (position <= end);
        }

        public static byte[] GetBytesFromImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                return File.ReadAllBytes(imagePath);
            }
            else
            {
                return File.ReadAllBytes(FnFPath);
            }
        }

        public static string BaseURL()
        {
            Uri myuri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            string pathQuery = myuri.PathAndQuery;
            string hostName = myuri.ToString().Replace(pathQuery, "");
            return hostName;
        }
    }
}