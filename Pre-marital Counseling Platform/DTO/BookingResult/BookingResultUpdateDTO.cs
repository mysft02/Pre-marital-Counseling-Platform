﻿namespace SWP391.DTO.BookingResult
{
    public class BookingResultUpdateDTO
    {
        public Guid BookingResultId { get; set; }
        public Guid BookingId { get; set; }
        public string Description { get; set; }
    }
}
