namespace Flobot.Identity
{
    public class PermissionsService : IPermissionsService
    {
        public bool HasPermissions(Role callerRole, Group callerGroup, Role targetRole, Group targetGroup)
        {
            return targetRole <= callerRole && callerGroup.HasFlag(targetGroup) || callerGroup.HasFlag(Group.Administrators) || callerRole == Role.Admin;
        }
    }
}