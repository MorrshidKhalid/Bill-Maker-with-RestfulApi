using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Users;
using static BMDataContract.BMContract.Person;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;
using BMDataContract;

namespace BMData
{

    public class UserDTO : PersonDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int Permission { get; set; }


        public UserDTO
            (
            int userID, string userName, string password, bool isActive, int permission,
            int personID, string firstName, string secondName, string lastName, string email, bool gender, string address, string phone)
            : base(personID, firstName, secondName, lastName, email, gender, address, phone)
        {
            UserID = userID;
            UserName = userName;
            Password = password;
            Permission = permission;

            this.IsActive = isActive;
        }


    }

    public class UserInfoDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int Permission { get; set; }

        public UserInfoDTO(int userID, string userName, string password, bool isActive, int permission)
        {
            UserID = userID;
            UserName = userName;
            Password = password;
            IsActive = isActive;
            Permission = permission;
        }
    }
    public class UserDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        public static List<UserDTO> GetAllUsers()
        {
            List<UserDTO> userDTOs = new();
            string query = $@"SELECT 
                              {USER_COLUMN_PK},
                              {USER_COLUMN_USERNAME},
                              {USER_COLUMN_IS_ACTIVE},
                              {USER_COLUMN_PERMISSION},
                              {BMContract.JOIN_USERS_PERSON_INFO}";

            SqlCommand command = new(query, connection);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    userDTOs.Add(new UserDTO
                        (
                        (int)reader[USER_COLUMN_PK],
                        (string)reader[USER_COLUMN_USERNAME],
                        "????",
                        (bool)reader[USER_COLUMN_IS_ACTIVE],
                        (int)reader[USER_COLUMN_PERMISSION],
                        (int)reader[USER_COLUMN_PERSON_ID],
                        (string)reader[PERSON_COLUMN_FIRST_NAME],
                        (string)reader[PERSON_COLUMN_SECOND_NAME],
                        (string)reader[PERSON_COLUMN_LAST_NAME],
                        (string)reader[PERSON_COLUMN_EMAIL],
                        (bool)reader[PERSON_COLUMN_GENDOR],
                        (string)reader[PERSON_COLUMN_ADDRESS],
                        (string)reader[PERSON_COLUMN_PHONE]
                        ));
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return userDTOs;
        }

        public static int AddNewUser(UserDTO userDTO, int personID)
        {
            int insertedID = -1;

            string query = $@"INSERT INTO 
                            {USERS} 
                            ({USER_COLUMN_PERSON_ID}, {USER_COLUMN_USERNAME}, {USER_COLUMN_PASSWORD}, {USER_COLUMN_IS_ACTIVE}, {USER_COLUMN_PERMISSION})
                            VALUES 
                            (@personID, @username, @password, @isActive, @permission);
                            {BMContract.SCOPE_IDENTITY}";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@username", userDTO.UserName);
            command.Parameters.AddWithValue("@password", userDTO.Password);
            command.Parameters.AddWithValue("@isActive", userDTO.IsActive);
            command.Parameters.AddWithValue("@permission", userDTO.Permission);

            try
            {
                connection.Open();
                DBLib.ExecAndGetInsertedID(ref insertedID, command);
            }
            catch
            {
                insertedID = -1;
            }
            finally
            {
                connection.Close();
            }
            return insertedID;
        }
        
        public static UserDTO? GetUserByID(int userID)
        {
            UserDTO? userDTO = null;

            string query = @$"SELECT 
                              {USER_COLUMN_PK},
                              {USER_COLUMN_USERNAME},
                              {USER_COLUMN_PASSWORD},
                              {USER_COLUMN_IS_ACTIVE},
                              {USER_COLUMN_PERMISSION},
                              {BMContract.JOIN_USERS_PERSON_INFO}
                              WHERE {USER_COLUMN_PK} = @userID";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@userID", userID);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    userDTO = new UserDTO
                        (
                        (int)reader[USER_COLUMN_PK],
                        (string)reader[USER_COLUMN_USERNAME],
                       (string)reader[USER_COLUMN_PASSWORD],
                        (bool)reader[USER_COLUMN_IS_ACTIVE],
                        (int)reader[USER_COLUMN_PERMISSION],
                        (int)reader[USER_COLUMN_PERSON_ID],
                        (string)reader[PERSON_COLUMN_FIRST_NAME],
                        (string)reader[PERSON_COLUMN_SECOND_NAME],
                        (string)reader[PERSON_COLUMN_LAST_NAME],
                        (string)reader[PERSON_COLUMN_EMAIL],
                        (bool)reader[PERSON_COLUMN_GENDOR],
                        (string)reader[PERSON_COLUMN_ADDRESS],
                        (string)reader[PERSON_COLUMN_PHONE]
                        );
                }    
            }
            catch
            {
                userDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return userDTO;
        }

        public static UserDTO? GetUserByUsername(string username)
        {
            UserDTO? userDTO = null;

            string query = @$"SELECT 
                              {USER_COLUMN_PK},
                              {USER_COLUMN_USERNAME},
                              {USER_COLUMN_PASSWORD},
                              {USER_COLUMN_IS_ACTIVE},
                              {USER_COLUMN_PERMISSION},
                              {BMContract.JOIN_USERS_PERSON_INFO}
                              WHERE {USER_COLUMN_USERNAME} = @username";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@username", username);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userDTO = new UserDTO
                        (
                        (int)reader[USER_COLUMN_PK],
                        (string)reader[USER_COLUMN_USERNAME],
                       (string)reader[USER_COLUMN_PASSWORD],
                        (bool)reader[USER_COLUMN_IS_ACTIVE],
                        (int)reader[USER_COLUMN_PERMISSION],
                        (int)reader[USER_COLUMN_PERSON_ID],
                        (string)reader[PERSON_COLUMN_FIRST_NAME],
                        (string)reader[PERSON_COLUMN_SECOND_NAME],
                        (string)reader[PERSON_COLUMN_LAST_NAME],
                        (string)reader[PERSON_COLUMN_EMAIL],
                        (bool)reader[PERSON_COLUMN_GENDOR],
                        (string)reader[PERSON_COLUMN_ADDRESS],
                        (string)reader[PERSON_COLUMN_PHONE]
                        );
                }
            }
            catch
            {
                userDTO = null;
            }
            finally
            {
                connection.Close();
            }

            return userDTO;
        }

        public static bool Update(UserDTO userDTO)
        {
            int rowEffected = -1;

            string query = $@"UPDATE {USERS} 
                              SET {USER_COLUMN_USERNAME} = @username,
                                  {USER_COLUMN_PASSWORD} = @password,
                                  {USER_COLUMN_IS_ACTIVE} = @isActive,
                                  {USER_COLUMN_PERMISSION} = @permission
                                  WHERE {USER_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", userDTO.UserID);
            command.Parameters.AddWithValue("@username", userDTO.UserName);
            command.Parameters.AddWithValue("@password", userDTO.Password);
            command.Parameters.AddWithValue("@isActive", userDTO.IsActive);
            command.Parameters.AddWithValue("@permission", userDTO.Permission);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch
            {
                rowEffected = -1;
            }
            finally
            {
                connection.Close();
            }

            return rowEffected != -1;
        }

        public static bool Delete(int id)
        {
            int rowEffected = -1;

            string query = $@"DELETE {USERS} WHERE {USER_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", id);

            try
            {
                connection.Open();
                rowEffected = command.ExecuteNonQuery();
            }
            catch
            {
                rowEffected = -1;
            }
            finally
            {
                connection.Close();
            }

            return rowEffected > 0;
        }

        public static bool IsUserExists(string username)
        {
            bool isFound = false;

            string query = $@"SELECT 1 {USER_COLUMN_USERNAME} FROM {USERS} WHERE {USER_COLUMN_USERNAME} = @username";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@username", username);

            try
            {
                connection.Open();
                DBLib.HasRow(ref isFound, command);
            }
            catch
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }


            return isFound;
        }

    }
}
