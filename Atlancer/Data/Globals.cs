namespace Atlancer.Data
{

    public class Globals
    {

        public enum UserTypes
        {
            Freelancer,
            Client
        }

        public static string UserType { get; set; }
        public static string UserId { get; set; }
        public static string UserName { get; set; }
        public static string GigId { get; set; }
        public static IFormFile GigImage1 { get; set; }
        public static IFormFile GigImage2 { get; set; }
        public static IFormFile GigImage3 { get; set; }
        public static string GigImageName1 { get; set; }
        public static string GigImageName2 { get; set; }
        public static string GigImageName3 { get; set; }
        public static string Email { get; set; }
        public static double BidAmount { get; set; }
        public static string FreelancerId { get; set; }
        public static string Password { get; set; }
        public static string ProfileImageName { get; set; }
        public static string ProjectId { get; set; }
        public static IFormFile ProfileImage { get; set; }
        public static string BidId { get; set; }
    }
}
