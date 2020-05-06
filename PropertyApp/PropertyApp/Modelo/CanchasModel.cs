using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyApp.Modelo
{
    public class CanchasModel
    {
        public int PageId { get; set; }
        public int UserId { get; set; }
        public string PageName { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string Avatar { get; set; }
        public string Cover { get; set; }
        public int PageCategory { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Google { get; set; }
        public string Vk { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int CallActionType { get; set; }
        public string CallActionTypeUrl { get; set; }
        public string BackgroundImage { get; set; }
        public int BackgroundImageStatus { get; set; }
        public string Instgram { get; set; }
        public string Youtube { get; set; }
        public string Verified { get; set; }
        public string Active { get; set; }
        public string Registered { get; set; }
        public string Boosted { get; set; }
        //public virtual WoUsers User { get; set; }
    }
}
