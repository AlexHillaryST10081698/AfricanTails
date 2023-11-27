using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using AfricanTails.Classes;
using System.Diagnostics;

namespace AfricanTails.Classes
{
    internal class DatabaseHandler
    {
        //variables
        private string AfricanTailsconnectionString;
        public DatabaseHandler() 
        {
             AfricanTailsconnectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
        }
        #region Add A New User to the Database
        public void RegisterUserToDatabase(string registerUsername, string registerFirstname, string registerLastname, string registerPassword)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to insert data into the RegisterUser table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO RegisterUser (UserID, UserName, UserSurname, UserPassword) VALUES (@UserID, @UserName, @UserSurname, @UserPassword)";
                    command.Parameters.AddWithValue("@UserID", registerUsername); 
                    command.Parameters.AddWithValue("@UserName", registerFirstname);
                    command.Parameters.AddWithValue("@UserSurname", registerLastname);
                    command.Parameters.AddWithValue("@UserPassword", registerPassword);

                    // Execute the SQL command to insert the data
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        public bool VerifyLogin(string username, string password)
        {
            string storedHashedPassword = GetHashedPassword(username);

            if (string.IsNullOrEmpty(storedHashedPassword))
            {
                return false; // User not found
            }

            string hashedPassword = HashPassword(password);

            return string.Equals(storedHashedPassword, hashedPassword);
        }
        #region Checks if user exist in Databse For login
        private string GetHashedPassword(string username)
        {
            //Sql query 
            string query = "SELECT UserPassword FROM RegisterUser WHERE UserID = @UserID";

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@UserID", username);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    return result != null ? result.ToString() : null;
                }
            }
        }
        #endregion

        #region Hashes the Password
        private string HashPassword(string password)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha1.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
        #endregion

        #region Adding A New Animal To Database
        public void AddAnimalToDatabase(string animalID, string name, string breed)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to insert data into the Animal table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO Animal (AnimalID, Name, Breed) VALUES (@AnimalID, @Name, @Breed)";
                    command.Parameters.AddWithValue("@AnimalID", animalID);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Breed", breed);

                    // Execute the SQL command to insert the data
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Add Animal Attributes to Database
        public void AddAnimalAttributeToDatabase(string animalID, string colour, string sex, long microchip, string status, DateTime dateofBirth, DateTime dateAdopted, DateTime dateFostered)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to insert data into the AnimalAttribute table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO AnimalAttribute (AnimalID, Colour, Sex, Microchip, Status, DateofBirth, DateAdopted, DateFostered) VALUES (@AnimalID,@Colour, @Sex, @Microchip, @Status, @DateofBirth, @DateAdopted, @DateFostered)";
                    command.Parameters.AddWithValue("@AnimalID", animalID);
                    command.Parameters.AddWithValue("@Colour", colour);
                    command.Parameters.AddWithValue("@Sex", sex);
                    command.Parameters.AddWithValue("@Microchip", microchip);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@DateofBirth", dateofBirth);
                    command.Parameters.AddWithValue("@DateAdopted", dateAdopted);
                    command.Parameters.AddWithValue("@DateFostered", dateFostered);

                    // Execute the SQL command to insert the data
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion
        public bool AnimalIDExists(string animalID)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to check if the AnimalID exists in the Animal table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM Animal WHERE AnimalID = @AnimalID";
                    command.Parameters.AddWithValue("@AnimalID", animalID);

                    // Execute the SQL command to get the count of rows with the specified AnimalID
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // If count is greater than 0, the AnimalID already exists
                    return count > 0;
                }
            }
        }

        public List<Animal> GetAnimalData()
        {
            List<Animal> animalList = new List<Animal>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to select data from both Animal and AnimalAttribute tables
                string query = "SELECT Animal.AnimalID, Name, Breed, Colour, Sex, Microchip, Status, DateofBirth, DateAdopted, DateFostered " +
                               "FROM Animal " +
                               "JOIN AnimalAttribute ON Animal.AnimalID = AnimalAttribute.AnimalID";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Animal animal = new Animal
                            {
                                AnimalID = reader["AnimalID"].ToString(),
                                Name = reader["Name"].ToString(),
                                Breed = reader["Breed"].ToString(),
                                Colour = reader["Colour"].ToString(),
                                Sex = reader["Sex"].ToString(),
                                Microchip = Convert.ToInt64(reader["Microchip"]),
                                Status = reader["Status"].ToString(),
                                DateofBirth = Convert.ToDateTime(reader["DateofBirth"]),
                                DateAdopted = Convert.ToDateTime(reader["DateAdopted"]),
                                DateFostered = Convert.ToDateTime(reader["DateFostered"])
                            };

                            animalList.Add(animal);
                        }
                    }
                }
            }

            return animalList;
        }
        public List<Animal> GetAnimalDataSortedAlphabetically()
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Select animal data and order by Name
                    command.CommandText = "SELECT * FROM Animal ORDER BY Name";

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        List<Animal> animals = new List<Animal>();

                        while (reader.Read())
                        {
                            Animal animal = new Animal
                            {
                                AnimalID = reader["AnimalID"].ToString(),
                                Name = reader["Name"].ToString(),
                                Breed = reader["Breed"].ToString(),
                                // Add more properties as needed
                            };

                            animals.Add(animal);
                        }

                        return animals;
                    }
                }
            }
        }

        public void DeleteAnimal(string animalID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to delete the animal from the Animal table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM Animal WHERE AnimalID = @AnimalID";
                    command.Parameters.AddWithValue("@AnimalID", animalID);

                    // Execute the SQL command to delete the animal
                    int rowsAffected = command.ExecuteNonQuery();
                }

                // Delete associated attributes from AnimalAttribute table (assuming you have one)
                DeleteAnimalAttributes(animalID);
            }
        }

        #region Delete entry in animal attribute
        private void DeleteAnimalAttributes(string animalID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to delete the attributes from the AnimalAttribute table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM AnimalAttribute WHERE AnimalID = @AnimalID";
                    command.Parameters.AddWithValue("@AnimalID", animalID);

                    // Execute the SQL command to delete the attributes
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Edit Animal in The Database
        public void EditAnimalInDatabase(string animalID, string newName, string newBreed, string newColour, string newSex, long newMicrochip, string newStatus, DateTime newDateOfBirth, DateTime newDateAdopted, DateTime newDateFostered)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Update Animal table
                    command.CommandText = "UPDATE Animal SET Name = @NewName, Breed = @NewBreed WHERE AnimalID = @AnimalID";
                    command.Parameters.AddWithValue("@NewName", newName);
                    command.Parameters.AddWithValue("@NewBreed", newBreed);
                    command.Parameters.AddWithValue("@AnimalID", animalID);

                    int rowsAffectedAnimal = command.ExecuteNonQuery();

                    // Update AnimalAttribute table
                    command.CommandText = "UPDATE AnimalAttribute SET Colour = @NewColour, Sex = @NewSex, Microchip = @NewMicrochip, Status = @NewStatus, DateofBirth = @NewDateOfBirth, DateAdopted = @NewDateAdopted, DateFostered = @NewDateFostered WHERE AnimalID = @AnimalID";
                    command.Parameters.AddWithValue("@NewColour", newColour);
                    command.Parameters.AddWithValue("@NewSex", newSex);
                    command.Parameters.AddWithValue("@NewMicrochip", newMicrochip);
                    command.Parameters.AddWithValue("@NewStatus", newStatus);
                    command.Parameters.AddWithValue("@NewDateOfBirth", newDateOfBirth.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@NewDateAdopted", newDateAdopted.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@NewDateFostered", newDateFostered.ToString("yyyy-MM-dd"));

                    int rowsAffectedAttribute = command.ExecuteNonQuery();

                    // Check if the update was successful in both tables
                    if (rowsAffectedAnimal > 0 && rowsAffectedAttribute > 0)
                    {
                        // Both updates were successful
                    }
                    else
                    {
                        // Handle the case where the update failed in one or both tables
                    }
                }
            }
        }
        #endregion
        #region Add Medical Record to Database
        public void AddMedicalRecordToDatabase(string medicalRecordID, string animalID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO MedicalRecord (MedicalRecordID, AnimalID) VALUES (@MedicalRecordID, @AnimalID)";
                    command.Parameters.AddWithValue("@MedicalRecordID", medicalRecordID);
                    command.Parameters.AddWithValue("@AnimalID", animalID);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion
        #region Add Medical Record Detail to Database
        public void AddMedicalRecordDetailToDatabase(string medicalRecordID, DateTime sterilization, DateTime firstVaccination, DateTime secondVaccination, DateTime thirdVaccination, string dewormedTest, DateTime dewormedTestDate, string rabiesTest, DateTime rabiesTestDate, string medicalTreatmentTest, DateTime medicalTreatmentTestDate, string fleaTreatmentTest, DateTime fleaTreatmentTestDate)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO MedicalRecordDetail (MedicalRecordID, Sterilization, FirstVaccination, SecondVaccination, ThirdVaccination, DewormedTest, DewormedTestDate, RabiesTest, RabiesTestDate, MedicalTreatmentTest, MedicalTreatmentTestDate, FleaTreatmentTest, FleaTreatmentTestDate) VALUES (@MedicalRecordID, @Sterilization, @FirstVaccination, @SecondVaccination, @ThirdVaccination, @DewormedTest, @DewormedTestDate, @RabiesTest, @RabiesTestDate, @MedicalTreatmentTest, @MedicalTreatmentTestDate, @FleaTreatmentTest, @FleaTreatmentTestDate)";

                    command.Parameters.AddWithValue("@MedicalRecordID", medicalRecordID);
                    command.Parameters.AddWithValue("@Sterilization", sterilization);
                    command.Parameters.AddWithValue("@FirstVaccination", firstVaccination);
                    command.Parameters.AddWithValue("@SecondVaccination", secondVaccination);
                    command.Parameters.AddWithValue("@ThirdVaccination", thirdVaccination);
                    command.Parameters.AddWithValue("@DewormedTest", dewormedTest);
                    command.Parameters.AddWithValue("@DewormedTestDate", dewormedTestDate);
                    command.Parameters.AddWithValue("@RabiesTest", rabiesTest);
                    command.Parameters.AddWithValue("@RabiesTestDate", rabiesTestDate);
                    command.Parameters.AddWithValue("@MedicalTreatmentTest", medicalTreatmentTest);
                    command.Parameters.AddWithValue("@MedicalTreatmentTestDate", medicalTreatmentTestDate);
                    command.Parameters.AddWithValue("@FleaTreatmentTest", fleaTreatmentTest);
                    command.Parameters.AddWithValue("@FleaTreatmentTestDate", fleaTreatmentTestDate);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion
        #region Add CatSpecific Medical Record to Database
        public void AddCatSpecificMedicalRecordToDatabase(string medicalRecordID, string FIVFELVTest, DateTime FIVFELVTestDate)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO CatSpecific_MedicalRecord (MedicalRecordID, FIVFELVTest, FIVFELVTestDate) VALUES (@MedicalRecordID, @FIVFELVTest, @FIVFELVTestDate)";

                    command.Parameters.AddWithValue("@MedicalRecordID", medicalRecordID);
                    command.Parameters.AddWithValue("@FIVFELVTest", FIVFELVTest);
                    command.Parameters.AddWithValue("@FIVFELVTestDate", FIVFELVTestDate);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion
        #region Add DogSpecific Medical Record to Database
        public void AddDogSpecificMedicalRecordToDatabase(string medicalRecordID, string distemperTest, DateTime distemperTestDate, string parvoTest, DateTime parvoTestDate)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO DogSpecific_MedicalRecord (MedicalRecordID, DistemperTest, DistemperTestDate, ParvoTest, ParvoTestDate) " +
                                          "VALUES (@MedicalRecordID, @DistemperTest, @DistemperTestDate, @ParvoTest, @ParvoTestDate)";

                    command.Parameters.AddWithValue("@MedicalRecordID", medicalRecordID);
                    command.Parameters.AddWithValue("@DistemperTest", distemperTest);
                    command.Parameters.AddWithValue("@DistemperTestDate", distemperTestDate);
                    command.Parameters.AddWithValue("@ParvoTest", parvoTest);
                    command.Parameters.AddWithValue("@ParvoTestDate", parvoTestDate);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion



        #region Adds Foster To Database
        public void AddFosterToDatabase(string fosterID, string fosterFullname, string fosterContactNumber, string fosterEmail, string fosterAddress)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to insert data into the Foster table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO Foster (FosterID, FosterFullname, FosterContactNumber, FosterEmail, FosterAddress) VALUES (@FosterID, @FosterFullname, @FosterContactNumber, @FosterEmail, @FosterAddress)";
                    command.Parameters.AddWithValue("@FosterID", fosterID);
                    command.Parameters.AddWithValue("@FosterFullname", fosterFullname);
                    command.Parameters.AddWithValue("@FosterContactNumber", fosterContactNumber);
                    command.Parameters.AddWithValue("@FosterEmail", fosterEmail);
                    command.Parameters.AddWithValue("@FosterAddress", fosterAddress);

                    // Execute the SQL command to insert the data
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Adds Staff Member to Database
        public void AddStaffToDatabase(string staffID, string staffName, string staffSurname, string staffPassword)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO Staff (StaffID, StaffName, StaffSurname, StaffPassword) VALUES (@StaffID, @StaffName, @StaffSurname, @StaffPassword)";
                    command.Parameters.AddWithValue("@StaffID", staffID);
                    command.Parameters.AddWithValue("@StaffName", staffName);
                    command.Parameters.AddWithValue("@StaffSurname", staffSurname);
                    command.Parameters.AddWithValue("@StaffPassword", staffPassword);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Adds an Adoptee to the Database
        public void AddAdoptionToDatabase(string adoptionID, string adopterFullname, string adopterAddress, string adopterContactNumber, string adopterEmail)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to insert data into the Adoption table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO Adoption (AdoptionID, AdopterFullname, AdopterAddress, AdopterContactNumber, AdopterEmail) VALUES (@AdoptionID, @AdopterFullname, @AdopterAddress, @AdopterContactNumber, @AdopterEmail)";
                    command.Parameters.AddWithValue("@AdoptionID", adoptionID);
                    command.Parameters.AddWithValue("@AdopterFullname", adopterFullname);
                    command.Parameters.AddWithValue("@AdopterAddress", adopterAddress);
                    command.Parameters.AddWithValue("@AdopterContactNumber", adopterContactNumber);
                    command.Parameters.AddWithValue("@AdopterEmail", adopterEmail);

                    // Execute the SQL command to insert the data
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        #endregion
        public bool AdoptionIDExists(string adoptionID)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to check if the AdoptionID exists in the Adoption table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM Adoption WHERE AdoptionID = @AdoptionID";
                    command.Parameters.AddWithValue("@AdoptionID", adoptionID);

                    // Execute the SQL command to get the count of rows with the specified AdoptionID
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // If count is greater than 0, the AdoptionID already exists
                    return count > 0;
                }
            }
        }

        #region Filter Alphabetically by Animal Name
        public void GetAnimalsAlphabetically(out List<Animal> animals)
        {
            animals = new List<Animal>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Animal ORDER BY Name", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Animal animal = new Animal
                            {
                                AnimalID = reader["AnimalID"].ToString(),
                                Name = reader["Name"].ToString(),
                                Breed = reader["Breed"].ToString()
                            };

                            animals.Add(animal);
                        }
                    }
                }
            }
        }
        #endregion

        #region Sort Animals from Oldest To Newest 
        public void GetAnimalsNewestToOldest(out List<Animal> animals)
        {
            animals = new List<Animal>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT A.*, AA.DateofBirth FROM Animal AS A JOIN AnimalAttribute AS AA ON A.AnimalID = AA.AnimalID ORDER BY AA.DateofBirth DESC", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Animal animal = new Animal
                            {
                                AnimalID = reader["AnimalID"].ToString(),
                                Name = reader["Name"].ToString(),
                                Breed = reader["Breed"].ToString()
                            };

                            animals.Add(animal);
                        }
                    }
                }
            }
        }
        #endregion

        #region Sort Animials From Newest To Oldest
        public void GetAnimalsOldestToNewest(out List<Animal> animals)
        {
            animals = new List<Animal>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT A.*, AA.DateofBirth FROM Animal AS A JOIN AnimalAttribute AS AA ON A.AnimalID = AA.AnimalID ORDER BY AA.DateofBirth", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Animal animal = new Animal
                            {
                                AnimalID = reader["AnimalID"].ToString(),
                                Name = reader["Name"].ToString(),
                                Breed = reader["Breed"].ToString()
                            };

                            animals.Add(animal);
                        }
                    }
                }
            }
        }
        #endregion

        public bool StaffIDExists(string staffID)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to check if the StaffID exists in the Staff table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM Staff WHERE StaffID = @StaffID";
                    command.Parameters.AddWithValue("@StaffID", staffID);

                    // Execute the SQL command to get the count of rows with the specified StaffID
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // If count is greater than 0, the StaffID already exists
                    return count > 0;
                }
            }
        }

        public List<Staff> SearchStaff(string searchText)
        {
            List<Staff> staffList = new List<Staff>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Staff WHERE StaffName LIKE @SearchText OR StaffSurname LIKE @SearchText OR StaffID LIKE @SearchText";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchText", $"%{searchText}%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Staff staff = new Staff
                            {
                                StaffID = reader["StaffID"].ToString(),
                                StaffName = reader["StaffName"].ToString(),
                                StaffSurname = reader["StaffSurname"].ToString(),
                                StaffPassword = reader["StaffPassword"].ToString()
                            };

                            staffList.Add(staff);
                        }
                    }
                }
            }

            return staffList;
        }
        public void DeleteStaffFromDatabase(string staffID)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to delete data from the Staff table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM Staff WHERE StaffID = @StaffID";
                    command.Parameters.AddWithValue("@StaffID", staffID);

                    // Execute the SQL command to delete the data
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }

        public List<Staff> GetStaffData()
        {
            List<Staff> staffList = new List<Staff>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to select all data from the Staff table
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Staff", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Staff staff = new Staff
                            {
                                StaffID = reader["StaffID"].ToString(),
                                StaffName = reader["StaffName"].ToString(),
                                StaffSurname = reader["StaffSurname"].ToString(),
                                StaffPassword = reader["StaffPassword"].ToString()
                            };

                            staffList.Add(staff);
                        }
                    }
                }
            }

            return staffList;
        }
        public void EditStaffInDatabase(string staffID, string newName, string newSurname)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE Staff SET StaffName = @NewName, StaffSurname = @NewSurname WHERE StaffID = @StaffID";
                    command.Parameters.AddWithValue("@NewName", newName);
                    command.Parameters.AddWithValue("@NewSurname", newSurname);
                    command.Parameters.AddWithValue("@StaffID", staffID);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        public List<Staff> GetStaffDataSortedAlphabetically()
        {
            List<Staff> staffList = new List<Staff>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to select specific columns from the Staff table sorted alphabetically
                using (SQLiteCommand command = new SQLiteCommand("SELECT StaffID, StaffName, StaffSurname, StaffPassword, DateAdded FROM Staff ORDER BY StaffName, StaffSurname", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Staff staff = new Staff
                            {
                                StaffID = reader["StaffID"].ToString(),
                                StaffName = reader["StaffName"].ToString(),
                                StaffSurname = reader["StaffSurname"].ToString(),
                                StaffPassword = reader["StaffPassword"].ToString(),
                                DateAdded = Convert.ToDateTime(reader["DateAdded"])
                            };

                            staffList.Add(staff);
                        }
                    }
                }
            }

            return staffList;
        }



        public List<Adoption> GetAdoptionsData()
        {
            List<Adoption> adoptionList = new List<Adoption>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to select data from the Adoption and AnimalAdopted tables
                string query = "SELECT A.*, AA.AnimalID FROM Adoption AS A " +
                               "JOIN AnimalAdopted AS AA ON A.AdoptionID = AA.AdoptionID";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Adoption adoption = new Adoption
                            {
                                AdoptionID = reader["AdoptionID"].ToString(),
                                AdopterFullname = reader["AdopterFullname"].ToString(),
                                AdopterAddress = reader["AdopterAddress"].ToString(),
                                AdopterContactNumber = reader["AdopterContactNumber"].ToString(),
                                AdopterEmail = reader["AdopterEmail"].ToString(),
                                // Add other properties as needed
                            };

                            adoptionList.Add(adoption);
                        }
                    }
                }
            }

            return adoptionList;
        }
        // In your DatabaseHandler class
        public List<Adoption> GetAdoptionData()
        {
            List<Adoption> adoptionList = new List<Adoption>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to select all data from the Adoption table
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Adoption", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Adoption adoption = new Adoption
                            {
                                AdoptionID = reader["AdoptionID"].ToString(),
                                AdopterFullname = reader["AdopterFullname"].ToString(),
                                AdopterContactNumber = reader["AdopterContactNumber"].ToString(),
                                AdopterEmail = reader["AdopterEmail"].ToString(),
                                AdopterAddress = reader["AdopterAddress"].ToString()
                            };

                            adoptionList.Add(adoption);
                        }
                    }
                }
            }

            return adoptionList;
        }


        public void EditAdoptionInDatabase(string adoptionID, string newFullName, string newContactNumber, string newEmail, string newAddress)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE Adoption SET AdopterFullname = @NewFullName, AdopterContactNumber = @NewContactNumber, AdopterEmail = @NewEmail, AdopterAddress = @NewAddress WHERE AdoptionID = @AdoptionID";
                    command.Parameters.AddWithValue("@NewFullName", newFullName);
                    command.Parameters.AddWithValue("@NewContactNumber", newContactNumber);
                    command.Parameters.AddWithValue("@NewEmail", newEmail);
                    command.Parameters.AddWithValue("@NewAddress", newAddress);
                    command.Parameters.AddWithValue("@AdoptionID", adoptionID);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
        public List<Adoption> GetAdoptionDataSortedAlphabetically()
        {
            List<Adoption> adoptionList = new List<Adoption>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to select all data from the Adoption table sorted alphabetically
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Adoption ORDER BY AdopterFullname", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Adoption adoption = new Adoption
                            {
                                AdoptionID = reader["AdoptionID"].ToString(),
                                AdopterFullname = reader["AdopterFullname"].ToString(),
                                AdopterAddress = reader["AdopterAddress"].ToString(),
                                AdopterContactNumber = reader["AdopterContactNumber"].ToString(),
                                AdopterEmail = reader["AdopterEmail"].ToString()
                            };

                            adoptionList.Add(adoption);
                        }
                    }
                }
            }

            return adoptionList;
        }

        public bool FosterIDExists(string fosterID)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to check if the FosterID exists in the Foster table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT COUNT(*) FROM Foster WHERE FosterID = @FosterID";
                    command.Parameters.AddWithValue("@FosterID", fosterID);

                    // Execute the SQL command to get the count of rows with the specified FosterID
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // If count is greater than 0, the FosterID already exists
                    return count > 0;
                }
            }
        }


        public List<Foster> SearchFosters(string searchText)
        {
            List<Foster> fosterList = new List<Foster>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Foster WHERE FosterID LIKE @SearchText OR FosterFullname LIKE @SearchText OR FosterEmail LIKE @SearchText OR FosterContactNumber LIKE @SearchText OR FosterAddress LIKE @SearchText";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchText", $"%{searchText}%");

                    // Log the SQL query to check if it's correct
                    Debug.WriteLine($"Executing SQL query: {query}");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Foster foster = new Foster
                            {
                                FosterID = reader["FosterID"].ToString(),
                                FosterFullname = reader["FosterFullname"].ToString(),
                                FosterContactNumber = reader["FosterContactNumber"].ToString(),
                                FosterEmail = reader["FosterEmail"].ToString(),
                                FosterAddress = reader["FosterAddress"].ToString()
                            };

                            fosterList.Add(foster);
                        }
                    }
                }
            }

            // Log the number of results
            Debug.WriteLine($"Found {fosterList.Count} matching fosters.");

            return fosterList;
        }
        // Inside your DatabaseHandler class
        public List<Foster> GetFosterDataSortedByNewest()
        {
            List<Foster> fosterData = GetFosterData(); // Assuming you have a method to get all foster data

            // Sort fosters by DateAdded in descending order (newest to oldest)
            fosterData = fosterData.OrderByDescending(f => f.DateAdded).ToList();

            return fosterData;
        }

        public List<Foster> GetFosterData()
        {
            List<Foster> fosterList = new List<Foster>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to select all data from the Foster table
                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Foster", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Foster foster = new Foster
                            {
                                FosterID = reader["FosterID"].ToString(),
                                FosterFullname = reader["FosterFullname"].ToString(),
                                FosterContactNumber = reader["FosterContactNumber"].ToString(),
                                FosterEmail = reader["FosterEmail"].ToString(),
                                FosterAddress = reader["FosterAddress"].ToString()
                            };

                            fosterList.Add(foster);
                        }
                    }
                }
            }

            return fosterList;
        }

        public void EditFosterInDatabase(string fosterID, string newFullName, string newContactNumber, string newEmail, string newAddress)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "UPDATE Foster SET FosterFullname = @NewFullName, FosterContactNumber = @NewContactNumber, FosterEmail = @NewEmail, FosterAddress = @NewAddress WHERE FosterID = @FosterID";
                    command.Parameters.AddWithValue("@NewFullName", newFullName);
                    command.Parameters.AddWithValue("@NewContactNumber", newContactNumber);
                    command.Parameters.AddWithValue("@NewEmail", newEmail);
                    command.Parameters.AddWithValue("@NewAddress", newAddress);
                    command.Parameters.AddWithValue("@FosterID", fosterID);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }



        public List<Adoption> SearchAdoptions(string searchText)
        {
            List<Adoption> adoptionList = new List<Adoption>();

            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to select data from the Adoption table based on the search criteria
                string query = "SELECT * FROM Adoption WHERE AdoptionID LIKE @SearchText OR AdopterFullname LIKE @SearchText OR AdopterEmail LIKE @SearchText OR AdopterContactNumber LIKE @SearchText";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchText", $"%{searchText}%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Adoption adoption = new Adoption
                            {
                                AdoptionID = reader["AdoptionID"].ToString(),
                                AdopterFullname = reader["AdopterFullname"].ToString(),
                                AdopterEmail = reader["AdopterEmail"].ToString(),
                                AdopterContactNumber = reader["AdopterContactNumber"].ToString(),
                                AdopterAddress = reader["AdopterAddress"].ToString(),
                                // Add other properties as needed
                            };

                            adoptionList.Add(adoption);
                        }
                    }
                }
            }

            return adoptionList;
        }
        public void DeleteAdoption(string adoptionID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to delete the record from the Adoption table
                string query = "DELETE FROM Adoption WHERE AdoptionID = @AdoptionID";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AdoptionID", adoptionID);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteFosterFromDatabase(string fosterID)
        {
            // Create a connection to the SQLite database
            using (SQLiteConnection connection = new SQLiteConnection(AfricanTailsconnectionString))
            {
                connection.Open();

                // Create a SQL command to delete data from the Foster table
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM Foster WHERE FosterID = @FosterID";
                    command.Parameters.AddWithValue("@FosterID", fosterID);

                    // Execute the SQL command to delete the data
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }

    }
}
