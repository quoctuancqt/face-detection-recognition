namespace Facial.Recognize.Core.Data
{
    using Facial.Recognize.Core.Dto;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataStoreAccess : IDataStoreAccess
    {
        private readonly TrainningFaceContext _trainningFaceContext;

        public DataStoreAccess(TrainningFaceContext trainningFaceContext)
        {
            _trainningFaceContext = trainningFaceContext;
        }
        public async Task SaveFaceAsync(FaceDto face)
        {
            _trainningFaceContext.Faces.Add(face.Projection());

            await _trainningFaceContext.SaveChangesAsync();
        }

        public async Task<List<FaceDto>> CallFacesAsync(string username)
        {
            var query = _trainningFaceContext.Faces.AsQueryable();

            if (string.IsNullOrEmpty(username))
            {
                query = query.Where(x => x.Label.Equals(username, StringComparison.OrdinalIgnoreCase));
            }

            return await query.Select(x => x.Projection()).ToListAsync();
        }

        public async Task<FaceDto> GetByIdAsync(int userId)
        {
            var face = await _trainningFaceContext.Faces.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (face == null) throw new Exception("User not found.");

            return face.Projection();
        }

        public async Task DeleteByIdAsync(int userId)
        {
            var face = await _trainningFaceContext.Faces.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (face == null) throw new Exception("User not found.");

            _trainningFaceContext.Faces.Remove(face);

            await _trainningFaceContext.SaveChangesAsync();
        }
    }
}
