using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotNetMaui.Mobile.Models;

namespace dotNetMaui.Mobile.Services.Message
{
    public class MessageInitializeReponse:BaseResponse
    {
        public User FriendInfo { get; set; }
        public IEnumerable <Models.Message> Messages { get; set; }
    }
}
