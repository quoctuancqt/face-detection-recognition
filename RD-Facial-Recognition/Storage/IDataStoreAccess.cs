namespace RD_Facial_Recognition.Storage
{
    using System.Collections.Generic;

    public interface IDataStoreAccess
    {
        string SaveFace(int userId, string username, byte[] faceBlob);
        List<Face> CallFaces(string username);
        bool IsUsernameValid(string username);
        string SaveAdmin(string username, string password);
        bool DeleteUser(string username);
        int GetUserId(string username);
        int GenerateUserId();
        string GetUsername(int userId);
        List<string> GetAllUsernames();
    }
}
