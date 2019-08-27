namespace RD_Facial_Recognition.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Linq;

    public class DataStoreAccess : IDataStoreAccess
    {
        private SQLiteConnection _sqLiteConnection;


        public DataStoreAccess(string databasePath)
        {
            _sqLiteConnection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", databasePath));
        }
        public string SaveFace(int userId, string username, byte[] faceBlob)
        {
            try
            {
                //var exisitingUserId = GetUserId(username);
                //if (exisitingUserId == 0) exisitingUserId = GenerateUserId();
                _sqLiteConnection.Open();
                var insertQuery = "INSERT INTO faces(username, faceSample, userId) VALUES(@username,@faceSample,@userId)";
                var cmd = new SQLiteCommand(insertQuery, _sqLiteConnection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.Add("faceSample", DbType.Binary, faceBlob.Length).Value = faceBlob;
                var result = cmd.ExecuteNonQuery();
                return string.Format("{0} face(s) saved successfully", result);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                _sqLiteConnection.Close();
            }

        }

        public List<Face> CallFaces(string username)
        {
            var faces = new List<Face>();
            try
            {
                _sqLiteConnection.Open();
                var query = username.ToLower().Equals("ALL_USERS".ToLower()) ? "SELECT * FROM faces" : "SELECT * FROM faces WHERE username=@username";
                var cmd = new SQLiteCommand(query, _sqLiteConnection);
                if (!username.ToLower().Equals("ALL_USERS".ToLower())) cmd.Parameters.AddWithValue("username", username);
                var result = cmd.ExecuteReader();
                if (!result.HasRows) return null;

                while (result.Read())
                {
                    var bytesBlob = new byte[result.GetBytes(1, 0, null, 0, int.MaxValue)];
                    result.GetBytes(1, 0, bytesBlob, 0, bytesBlob.Length);
                    var face = new Face
                    {
                        Image = (byte[])result["faceSample"],
                        //Id = Convert.ToInt32(result["id"]),
                        Label = result.GetString(0),
                        UserId = result.GetInt32(2)
                    };
                    faces.Add(face);
                }
                faces = faces.OrderBy(f => f.UserId).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                _sqLiteConnection.Close();
            }
            return faces;
        }

        public int GetUserId(string username)
        {
            var userId = 0;
            try
            {
                _sqLiteConnection.Open();
                var selectQuery = "SELECT userId FROM faces WHERE username=@username LIMIT 1";
                var cmd = new SQLiteCommand(selectQuery, _sqLiteConnection);
                cmd.Parameters.AddWithValue("username", username);
                var result = cmd.ExecuteReader();
                if (!result.HasRows) return 0;
                while (result.Read())
                {
                    userId = Convert.ToInt32(result["userId"]);

                }
            }
            catch
            {
                return userId;
            }
            finally
            {
                _sqLiteConnection.Close();
            }
            return userId; ;
        }

        public string GetUsername(int userId)
        {
            var username = "";
            try
            {
                _sqLiteConnection.Open();
                var selectQuery = "SELECT username FROM faces WHERE userId=@userId LIMIT 1";
                var cmd = new SQLiteCommand(selectQuery, _sqLiteConnection);
                cmd.Parameters.AddWithValue("userId", userId);
                var result = cmd.ExecuteReader();
                if (!result.HasRows) return username;
                while (result.Read())
                {
                    username = result.GetString(0);

                }
            }
            catch (Exception ex)
            {
                return username;
            }
            finally
            {
                _sqLiteConnection.Close();
            }
            return username; ;
        }

        public List<string> GetAllUsernames()
        {
            var usernames = new List<string>();
            try
            {
                _sqLiteConnection.Open();
                var query = "SELECT DISTINCT username FROM faces";
                var cmd = new SQLiteCommand(query, _sqLiteConnection);
                var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    usernames.Add((string)result["username"]);
                }
                usernames.Sort();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                _sqLiteConnection.Close();
            }
            return usernames;
        }

        public bool DeleteUser(string username)
        {
            var toReturn = false;
            try
            {
                _sqLiteConnection.Open();
                var query = "DELETE FROM faces WHERE username=@username";
                var cmd = new SQLiteCommand(query, _sqLiteConnection);
                cmd.Parameters.AddWithValue("username", username);
                var result = cmd.ExecuteNonQuery();
                if (result > 0) toReturn = true;
            }
            catch (Exception ex)
            {
                return toReturn;
            }
            finally
            {
                _sqLiteConnection.Close();
            }
            return toReturn;
        }

        public int GenerateUserId()
        {
            var date = DateTime.Now.ToString("MMddHHmmss");
            return Convert.ToInt32(date);
        }

        public bool IsUsernameValid(string username)
        {
            throw new NotImplementedException();
        }

        public string SaveAdmin(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
