using System.ComponentModel;

namespace Application.Viewmodels
{
    public class VMChangePass
    {
        [DisplayName("User name")]
        public string Username { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("New Password")]
        public string NewPassword { get; set; }
    }
}
