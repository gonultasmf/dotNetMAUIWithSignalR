﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNetMaui.Mobile.Models
{
    public  class Message
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }   
        public string Content { get; set; }
        public DateTime SendDateTime { get; set; }
        public string SendDateTimeStr => SendDateTime.ToString("HH:mm");
        public bool IsRead { get; set; }
    }
}
