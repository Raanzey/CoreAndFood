using System.ComponentModel.DataAnnotations;

namespace CoreAndFood.Data.Models
{
	public class Account
	{
        [Key]
        public int AdminID { get; set; }
        public string UserName { get; set; }
        public string Email  { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
