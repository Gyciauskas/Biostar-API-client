using ConsoleApp1.Models;
using System;
using System.Collections.Generic;

namespace SupremaMiddleware.Server.Models
{
    public class Row
    {
        public string user_id { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string photo_exists { get; set; }
        public string pin_exists { get; set; }
        public string password_exists { get; set; }
        public string updated_count { get; set; }
        public string last_modified { get; set; }
        public string idx_last_modified { get; set; }
        public DateTime start_datetime { get; set; }
        public DateTime expiry_datetime { get; set; }
        public string security_level { get; set; }
        public string display_duration { get; set; }
        public string display_count { get; set; }
        public string inherited { get; set; }
        public UserGroupId user_group_id { get; set; }
        public string disabled { get; set; }
        public string expired { get; set; }
        public string idx_user_id { get; set; }
        public string idx_user_id_num { get; set; }
        public string idx_name { get; set; }
        public string idx_phone { get; set; }
        public string idx_email { get; set; }
        public string fingerprint_template_count { get; set; }
        public string face_count { get; set; }
        public string card_count { get; set; }
        public List<AccessGroup> access_groups { get; set; }
    }
}
