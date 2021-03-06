﻿using System;
using System.ComponentModel.DataAnnotations;
using ParkingATHWeb.Shared.Enums;
using ParkingATHWeb.ViewModels.Base;

namespace ParkingATHWeb.Areas.Portal.ViewModels.Message
{
    public class QuickMessageViewModel : SmartParkBaseViewModel
    {
        [Required(ErrorMessage = "Treść wiadomości nie może być pusta!")]
        [MinLength(20, ErrorMessage = "Wiadomość powinna zawierać minimum 20 znaków!")]
        public string Text { get; set; }
        public bool ToAdmin => true;
        public bool IsNotification => false;
        public PortalMessageEnum PortalMessageType => PortalMessageEnum.MessageToAdminFromUser;
        public bool IsDisplayed => false;
        public Guid? PreviousMessageId => null;
        public DateTime CreateDate => DateTime.Now;
        public int UserId { get; set; }
        public int ReceiverUserId { get; set; }
    }
}
