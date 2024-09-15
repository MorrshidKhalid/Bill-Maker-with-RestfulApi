using static BMDataContract.BMContract.Tables;
using static BMDataContract.BMContract.Person;
using static DBAccessSettings;
using Microsoft.Data.SqlClient;
using BMDataContract;

namespace BMData
{
    public class PersonDTO
    {
        public int PersonID { get; set; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public bool Gender { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public PersonDTO(int personID, string firstName, string secondName, string lastName, string email, bool gender, string address, string phone)
        {
            PersonID = personID;
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            Email = email;
            Gender = gender;
            Address = address;
            Phone = phone;
        }
    }

    public class PersonDB
    {
        // Connection to Database.
        readonly static SqlConnection connection = new(ConnectionString);

        public static int AddNewPerson(PersonDTO personDTO)
        {
            int insertedID = -1;
            string query = $@"INSERT INTO {PEOPLE}
                            (
                            {PERSON_COLUMN_FIRST_NAME},
                            {PERSON_COLUMN_SECOND_NAME},
                            {PERSON_COLUMN_LAST_NAME},
                            {PERSON_COLUMN_GENDOR},
                            {PERSON_COLUMN_EMAIL},
                            {PERSON_COLUMN_ADDRESS},
                            {PERSON_COLUMN_PHONE}
                            ) 
                            VALUES 
                            (
                            @firstName, @secondName, @lastName,
                            @gender, @email, @address, @phone
                            ); {BMContract.SCOPE_IDENTITY}";


            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@firstName", personDTO.FirstName);
            command.Parameters.AddWithValue("@secondName", personDTO.SecondName);
            command.Parameters.AddWithValue("@lastName", personDTO.LastName);
            command.Parameters.AddWithValue("@email", personDTO.Email);
            command.Parameters.AddWithValue("@gender", personDTO.Gender);
            command.Parameters.AddWithValue("@address", personDTO.Address);
            command.Parameters.AddWithValue("@phone", personDTO.Phone);

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

        public static bool UpdatePerson(PersonDTO personDTO)
        {
            int rowEffected = -1;

            string query = $@"UPDATE {PEOPLE}
                            SET
                            {PERSON_COLUMN_FIRST_NAME} = @firstName,
                            {PERSON_COLUMN_SECOND_NAME} = @secondName,
                            {PERSON_COLUMN_LAST_NAME} = @lastName,
                            {PERSON_COLUMN_GENDOR} = @gender,
                            {PERSON_COLUMN_EMAIL} = @email,
                            {PERSON_COLUMN_ADDRESS} = @address,
                            {PERSON_COLUMN_PHONE} = @phone
                            WHERE {PERSON_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", personDTO.PersonID);
            command.Parameters.AddWithValue("@firstName", personDTO.FirstName);
            command.Parameters.AddWithValue("@secondName", personDTO.SecondName);
            command.Parameters.AddWithValue("@lastName", personDTO.LastName);
            command.Parameters.AddWithValue("@email", personDTO.Email);
            command.Parameters.AddWithValue("@gender", personDTO.Gender);
            command.Parameters.AddWithValue("@address", personDTO.Address);
            command.Parameters.AddWithValue("@phone", personDTO.Phone);

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

        public static bool DeletePerson(int personID)
        {
            int rowEffected = -1;

            string query = $"Delete {PEOPLE} WHERE {PERSON_COLUMN_PK} = @id";

            SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@id", personID);

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

        public static List<PersonDTO>? GetAllPeople()
        {
            List<PersonDTO>? personDTOs = null;

            string query = $@"SELECT * FROM {PEOPLE}";

            SqlCommand command = new(query, connection);

            try
            {
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    personDTOs?.Add(new PersonDTO
                        (
                        (int)reader[PERSON_COLUMN_PK],
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
                personDTOs = null;
            }
            finally
            {
                connection.Close();
            }

            return personDTOs;
        }

    }
}
