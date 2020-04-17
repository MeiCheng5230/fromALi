using System;
using System.Collections.Generic;

namespace PXin.Model
{
    /// <summary>
    /// P信用户全表
    /// </summary>
    public partial class TchatUserFull
    {
        public TchatUserFull()
        {
            Id = 0;
            Nodeid = 0;
            Token = string.Empty;
            Provinceid = 0;
            Cityid = 0;
            Showrealname = 0;
        }

        public int Id { get; set; }
        public int Nodeid { get; set; }
        public string Token { get; set; }
        public DateTime Createtime { get; set; }
        public string Gtclientid { get; set; }
        public string Devicetoken { get; set; }
        public string Nickname { get; set; }
        public string Sex { get; set; }
        public int Provinceid { get; set; }
        public string Provincename { get; set; }
        public int Cityid { get; set; }
        public string Cityname { get; set; }
        public string Personalsign { get; set; }
        public int Showrealname { get; set; }
        public string Nodecode { get; set; }
        public string Nodename { get; set; }
        public string Mobileno { get; set; }
        public string Email { get; set; }
        public string Appphoto { get; set; }
        public int? Gradeid { get; set; }
        public string Gradename { get; set; }
        public string Teamname { get; set; }
        public int IsValidfriend { get; set; }

    }
}