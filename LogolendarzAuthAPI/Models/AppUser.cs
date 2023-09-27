using Microsoft.AspNetCore.Identity;

namespace LogolendarzAuthAPI.Models
{
  public class AppUser : IdentityUser
  {
        public string Name { get; set; }
    }
}
