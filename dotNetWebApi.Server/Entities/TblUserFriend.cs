using System;

namespace dotNetWebApi.Server.Entities;

public class TblUserFriend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
    }
