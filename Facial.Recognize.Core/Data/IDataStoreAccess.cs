namespace Facial.Recognize.Core.Data
{
    using Facial.Recognize.Core.Dto;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDataStoreAccess
    {
        Task<List<FaceDto>> CallFacesAsync(string username);
        Task<FaceDto> GetByIdAsync(int userId);
        Task SaveFaceAsync(FaceDto face);
        Task DeleteByIdAsync(int userId);
    }
}
