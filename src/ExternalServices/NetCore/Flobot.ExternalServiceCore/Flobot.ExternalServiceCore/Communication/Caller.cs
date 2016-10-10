using Flobot.ExternalServiceCore.Identity;

namespace Flobot.ExternalServiceCore.Communication
{
    public class Caller
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Role Role { get; set; }

        public Group Group { get; set; }
    }
}
