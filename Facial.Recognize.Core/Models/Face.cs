namespace Facial.Recognize.Core.Models
{
    using Facial.Recognize.Core.Dto;
    using System;

    public class Face
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Label { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public FaceDto Projection()
        {
            return new FaceDto
            {
                Id = Id,
                Image = Image,
                Label = Label,
                UserId = UserId
            };
        }
    }
}
