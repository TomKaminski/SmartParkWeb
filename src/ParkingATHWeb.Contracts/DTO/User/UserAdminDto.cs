﻿namespace ParkingATHWeb.Contracts.DTO.User
{
    public class UserAdminDto : UserBaseDto
    {
        public int OrdersCount { get; set; }
        public int GateUsagesCount { get; set; }
    }
}
