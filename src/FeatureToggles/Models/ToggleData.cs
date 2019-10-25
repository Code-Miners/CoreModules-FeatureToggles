

namespace FeatureToggles.Models
{
    public class ToggleData
    {
        public string UserId { get; }

        public string IpAddress { get; }

        public string UserRoles { get; }

        public ToggleData(string userId, string ipAddress, params string[] roles)
        {
            UserId = userId;
            IpAddress = ipAddress;

            if (roles != null && roles.Length > 0)
            {
                UserRoles = string.Join("|", roles);
            }
            else
            {
                UserRoles = string.Empty;
            }
        }
    }
}
