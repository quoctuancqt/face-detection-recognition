namespace Facial.Recognize.Core.Dto
{
    using Facial.Recognize.Core.Models;
    using System;

    public class FaceDto
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public string Label { get; set; }
        public int UserId { get; set; }

        public Face Projection()
        {
            return new Face
            {
                Image = Image,
                Label = Label,
                UserId = UserId,
                CreatedAt = DateTime.Now
            };
        }
    }
}
