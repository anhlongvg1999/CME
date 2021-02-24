using System;
using System.Collections.Generic;
using System.Text;

namespace TSoft.Framework.ApiUtils
{
    public class RequestUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string AvatarFullPath { get; }
        public bool IsSuperUser { get; set; }
        public Guid ApplicationId { get; set; }
        public List<string> ListApps { get; set; }
        public List<string> ListRoles { get; set; }
        public List<string> ListRights { get; set; }
    }
}
