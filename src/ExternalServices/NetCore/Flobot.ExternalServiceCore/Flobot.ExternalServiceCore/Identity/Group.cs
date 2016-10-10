using System;

namespace Flobot.ExternalServiceCore.Identity
{
    [Flags]
    public enum Group
    {
        Default = 1,
        PsychoRaid = 2,
        PictureStoreCM = 4,
        Administrators = 2048
    }
}
