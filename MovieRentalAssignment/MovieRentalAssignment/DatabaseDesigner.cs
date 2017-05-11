using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MovieRentalAssignment
{
    class DatabaseManager
    {
        SqlConnection SqlConn = new SqlConnection(@"Data Source=DESKTOP-ULKVIT4;Initial Catalog=MovieRentalAssignment;Integrated Security=True");  //setting connection with database
        SqlDataReader SqlReader;
        SqlCommand SqlStr = new SqlCommand();
       

        string query = "";
        

        public DataTable ListCustomers()            //listing all the customers
        {

            DataTable dt = new DataTable();
            query = "Select * From Customer";


            try
            {
                SqlDataReader SqlReader;
                SqlCommand SqlStr = new SqlCommand();
                SqlStr.Connection = SqlConn;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();

                    if (SqlReader.HasRows)
                    {
                        dt.Load(SqlReader);
                    }

                    SqlConn.Close();
                    return dt;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SqlConn.Close();
                return null;
            }
        }


        public DataTable ListMovies()                   //listing all the movies
        {

            DataTable dt = new DataTable();
            query = "Select * From Movies";


            try
            {
                SqlDataReader SqlReader;
                SqlCommand SqlStr = new SqlCommand();
                SqlStr.Connection = SqlConn;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();

                    if (SqlReader.HasRows)
                    {
                        dt.Load(SqlReader);
                    }

                    SqlConn.Close();
                    return dt;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SqlConn.Close();
                return null;
            }
        }

        public DataTable ListRentedMovies()             //listing the rented movies
        {

            DataTable dt = new DataTable();
            query = "Select * From RentedMovies JOIN Customer s on s.FirstName = FirstName where CustIDFK = CustID";                 


            try
            {
                SqlDataReader SqlReader;
                SqlCommand SqlStr = new SqlCommand();
                SqlStr.Connection = SqlConn;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();

                    if (SqlReader.HasRows)
                    {
                        dt.Load(SqlReader);
                    }

                    SqlConn.Close();
                    return dt;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SqlConn.Close();
                return null;
            }
        }


        public DataTable ListRentedOutMovies()             //listing the rented movies that have not been returned when checkbox is checked
        {

            DataTable dt = new DataTable();
            query = "Select * From RentedMovies JOIN Customer s on s.FirstName = s.FirstName where CustIDFK = CustID and DateReturned is null";


            try
            {
                SqlDataReader SqlReader;
                SqlCommand SqlStr = new SqlCommand();
                SqlStr.Connection = SqlConn;

                using (SqlStr = new SqlCommand(query, SqlConn))             
                {
                    SqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();

                    if (SqlReader.HasRows)
                    {
                        dt.Load(SqlReader);
                    }

                    SqlConn.Close();
                    return dt;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SqlConn.Close();
                return null;
            }
        }

        public bool CustExist(string firstname, int phone)           //Checks if the customer is already in the system
        {
            bool custExists = false;
            try
            {
                SqlStr.Connection = SqlConn;
                query = "SELECT TOP 1 1 FROM Customer WHERE FirstName = '" + firstname + "' and Phone = " + phone;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();

                    SqlReader = SqlStr.ExecuteReader();
                }
                if (SqlReader.HasRows)
                {
                    custExists = true;
                }
                    SqlConn.Close();
                }
            
            catch (Exception ex)
            {

                MessageBox.Show("Database Exeption " + ex.Message);
                SqlConn.Close();
            }
            return custExists;
        }

        public bool AddCustomer(string firstname, string lastname, string address, string phone)     //adding a new customer
        {
            
            bool custAdded = false;
            try
            {
                SqlStr.Connection = SqlConn;
                query = "Insert into Customer(firstname, lastname, address, phone) values (@FirstName, @LastName, @Address, @Phone)";

                using (SqlStr = new SqlCommand(query, SqlConn))
                {

                    SqlStr.Parameters.AddWithValue("@FirstName", firstname);
                    SqlStr.Parameters.AddWithValue("@LastName", lastname);
                    SqlStr.Parameters.AddWithValue("@Address", address);
                    SqlStr.Parameters.AddWithValue("@Phone", phone);

                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();

                    if (affectedRecords > 0)
                    {
                        custAdded = true;
                    }

                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Database Exeption " + ex.Message);
                SqlConn.Close();
            }
            return custAdded;
        }



        public void DeleteCustomer (int custID)                        //Deleting customer (function)
            {
            query = " Delete from Customer where CustID = @CustID";

            try
            {
                SqlStr.Connection = SqlConn;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@CustID", custID);
                    SqlConn.Open();
                    int affectedRecords = SqlStr.ExecuteNonQuery();

                    if (affectedRecords > 0)
                    {
                        MessageBox.Show("Customer Deleted");
                    }

                    SqlConn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SqlConn.Close();
            }
        }

        public bool EditCustomer(string firstname, string lastname, string address, string phone, int CustID )     //editing customer function
        {

            bool custEdited = false;
            try
            {
                SqlStr.Connection = SqlConn;
                query = "Update Customer set FirstName = @FirstName, LastName = @LastName, Address = @Address, Phone = @Phone where CustID = @CustID";

                using (SqlStr = new SqlCommand(query, SqlConn))
                {

                    SqlStr.Parameters.AddWithValue("@FirstName", firstname);
                    SqlStr.Parameters.AddWithValue("@LastName", lastname);
                    SqlStr.Parameters.AddWithValue("@Address", address);
                    SqlStr.Parameters.AddWithValue("@Phone", phone);
                    SqlStr.Parameters.AddWithValue("@CustID", CustID);

                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();

                    if (affectedRecords > 0)
                    {
                        custEdited = true;
                    }
                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Database Exeption: " + ex.Message);
                SqlConn.Close();
            }
            return custEdited;
        }


        public bool MovieExist(string movieName)       //Checking if the movie is already in the system
        {
            bool movieExists = false;
            try
            {
                SqlStr.Connection = SqlConn;
                query = "SELECT TOP 1 1 FROM Movies WHERE Title = '" + movieName + "'";

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();

                    SqlReader = SqlStr.ExecuteReader();
                }
                if (SqlReader.HasRows)
                {
                    movieExists = true;
                }
                SqlConn.Close();
            }

            catch (Exception ex)
            {

                MessageBox.Show("Database Exeption " + ex.Message);
                SqlConn.Close();
            }
            return movieExists;
        }

        public bool AddMovie(string rating, string title, string year, double rental_cost, string copies, string plot, string genre)     //function for adding a new movie
        {

            bool movieAdded = false;
            try
            {
                SqlStr.Connection = SqlConn;
                query = "Insert into Movies(rating, title, year, rental_cost, copies, plot, genre) values (@Rating, @Title, @Year, @Rental_Cost, @Copies, @Plot, @Genre )";

                using (SqlStr = new SqlCommand(query, SqlConn))
                {                                               
                   
                    SqlStr.Parameters.AddWithValue("@Rating", rating);                                               
                    SqlStr.Parameters.AddWithValue("@Title", title);                                                
                    SqlStr.Parameters.AddWithValue("@Year", year);
                    SqlStr.Parameters.AddWithValue("@Rental_Cost", rental_cost);                                                 
                    SqlStr.Parameters.AddWithValue("@Copies", copies);
                    SqlStr.Parameters.AddWithValue("@Plot", plot);                                                 
                    SqlStr.Parameters.AddWithValue("@Genre", genre);                                               
                                                                
                                                                           
                    

                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();

                    if (affectedRecords > 0)
                    {
                        movieAdded = true;
                    }
                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Database Exeption " + ex.Message);
                SqlConn.Close();
            }
            return movieAdded;
        }

        public void DeleteMovie(int movieID)                        //Deleting customer (function)
        {
            query = " Delete from Movies where MovieID = @MovieID";
            
            try
            {
                SqlStr.Connection = SqlConn;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@MovieID", movieID);
                    SqlConn.Open();
                    int affectedRecords = SqlStr.ExecuteNonQuery();

                    if (affectedRecords > 0)
                    {
                        MessageBox.Show("Movie Deleted");
                    }

                    SqlConn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SqlConn.Close();
            }
        }




        public bool EditMovie(string title, string rating,  string year, string plot, string genre, string copies, double rental_cost, int MovieID)     //function for adding a new movie
        {

            bool movieEdited = false;
            try
            {
                SqlStr.Connection = SqlConn;
                query = "Update Movies set Rating = @Rating, Title = @Title, Year = @Year, Rental_Cost = @Rental_Cost, Copies = @Copies, Plot = @Plot, Genre = @Genre WHERE MovieID = @MovieID";
             
                using (SqlStr = new SqlCommand(query, SqlConn))
                {

                    SqlStr.Parameters.AddWithValue("@Rating", rating);
                    SqlStr.Parameters.AddWithValue("@Title", title);
                    SqlStr.Parameters.AddWithValue("@Year", year);
                    SqlStr.Parameters.AddWithValue("@Copies", copies);
                    SqlStr.Parameters.AddWithValue("@Genre", genre);
                    SqlStr.Parameters.AddWithValue("@Plot", plot);
                    SqlStr.Parameters.AddWithValue("@Rental_Cost", rental_cost);
                    SqlStr.Parameters.AddWithValue("@MovieID", MovieID);


                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    Int32 affectedRecords = SqlStr.ExecuteNonQuery();

                    if (affectedRecords > 0)
                    {
                        movieEdited = true;
                    }
                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Database Exeption: " + ex.Message);
                SqlConn.Close();
            }
            return movieEdited;
        }

        public bool CheckRentedMovies(int custID)      //checking if customer has rented out 2 movies, if they have they cant get another movie out
        {
            bool checkMovieRented = false;
            int affectedRecords;
            try
            {
                SqlStr.Connection = SqlConn;

                query = "Select count (*) from RentedMovies Where DateReturned is null and CustIDFK = " + custID + " Group by CustIDFK ";
                using (SqlCommand cmd = new SqlCommand(query, SqlConn))
                {
                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    affectedRecords = Convert.ToInt16(cmd.ExecuteScalar());
                }
                if (affectedRecords < 2)
                {
                    checkMovieRented = true;
                }
                SqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception: " + ex.Message);
                SqlConn.Close();
            }
            return checkMovieRented;
        }

        public bool MovieRentedOut(int movieID)      //checking if a movie is out so it cannot be deleted               
        {
            bool checkMovieOut = false;
            
            try
            {                                                                                                                               
                SqlStr.Connection = SqlConn;

                query = "Select * from RentedMovies Where MovieIDFK = " + movieID + " and DateReturned is null";   
                using (SqlCommand cmd = new SqlCommand(query, SqlConn))
                {
                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();
                }
                if (SqlReader.HasRows)
                {
                    checkMovieOut = true;
                }
                SqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception: " + ex.Message);
                SqlConn.Close();
            }
            return checkMovieOut;
        }

        public bool CustMovieRentedOut(int custID)      //checking if a cust has a movie out              
        {
            bool checkCustMovieOut = false;
            
            try
            {                                                                                                                               
                SqlStr.Connection = SqlConn;

                query = "Select * from RentedMovies Where CustIDFK = " + custID + " and DateReturned is null";
                using (SqlCommand cmd = new SqlCommand(query, SqlConn))
                {
                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();
                }
                if (SqlReader.HasRows)
                {
                    checkCustMovieOut = true;
                }
                SqlConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Exception: " + ex.Message);
                SqlConn.Close();
            }
            return checkCustMovieOut;
        }

        public bool RentMovie(int custID, int movieID, DateTime dateRented)     //function for renting a movie
        {
            DateTime dateTimeVariable = DateTime.Now;
            bool movieRented = false;
            int affectedRecords;           
                try
                {
                    SqlStr.Connection = SqlConn;
                    
                    query = "INSERT into RentedMovies (CustIDFK, MovieIDFK, DateRented) VALUES (" + custID + ", " + movieID + ", '" + dateRented + "') ;  UPDATE Movies set Copies = Copies - 1 WHERE MovieID = " + movieID;

                    using (SqlCommand cmd = new SqlCommand(query, SqlConn))
                    {
                        SqlStr.CommandText = query;
                        SqlConn.Open();
                        affectedRecords = cmd.ExecuteNonQuery();
                    }

                    if (affectedRecords > 0)
                    {
                        movieRented = true;
                    }
                    SqlConn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Exception: " + ex.Message);
                    SqlConn.Close();
                }
                return movieRented;
            }
        


        public bool ReturnMovie(int custID, int movieID, DateTime dateReturned)     //returning movie function          
        {
            DateTime dateTimeVariable = DateTime.Now;
            bool MovieReturned = false;
            int affectedRecords;
            try
            {
                SqlStr.Connection = SqlConn;
                query = "Update RentedMovies set DateReturned = '" + dateReturned + "' where MovieIDFK = " + movieID + " AND CustIDFK = " + custID + " ; UPDATE Movies set Copies = Copies + 1 WHERE MovieID = " + movieID;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlStr.CommandText = query;
                    SqlConn.Open();
                    affectedRecords = SqlStr.ExecuteNonQuery();

                    if (affectedRecords > 0)
                    {
                        MovieReturned = true;
                    }
                    SqlConn.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Database Exeption: " + ex.Message);
                SqlConn.Close();
            }
            return MovieReturned;
        }


        public DataTable SearchCustomers(string firstName)                 //search function for customer, searches by first name
        {

            DataTable dt = new DataTable();
            query = "Select * From Customer where FirstName Like '%' + @FirstName + '%'";


            try
            {
                SqlDataReader SqlReader;

                SqlStr.Connection = SqlConn;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@FirstName", firstName);
                    SqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();

                    if (SqlReader.HasRows)
                    {
                        dt.Load(SqlReader);
                    }

                    SqlConn.Close();
                    return dt;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SqlConn.Close();
                return null;
            }
        }

        public DataTable SearchMovies(string movieName)            //search function for movies, searches by movie title
        {

            DataTable dt = new DataTable();
            query = "Select * From Movies where Title Like '%' + @MovieName + '%'";


            try
            {
                SqlDataReader SqlReader;

                SqlStr.Connection = SqlConn;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@MovieName", movieName);
                    SqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();

                    if (SqlReader.HasRows)
                    {
                        dt.Load(SqlReader);
                    }

                    SqlConn.Close();
                    return dt;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SqlConn.Close();
                return null;
            }
        }


        public DataTable SearchRentals(string custID)            //search function for rentals, searches by customer ID
        {

            DataTable dt = new DataTable();
            query = "Select * From RentedMovies where CustIDFK Like '%' + @CustIDFK + '%'";


            try
            {
                SqlDataReader SqlReader;

                SqlStr.Connection = SqlConn;

                using (SqlStr = new SqlCommand(query, SqlConn))
                {
                    SqlStr.Parameters.AddWithValue("@CustIDFK", custID);
                    SqlConn.Open();
                    SqlReader = SqlStr.ExecuteReader();

                    if (SqlReader.HasRows)
                    {
                        dt.Load(SqlReader);
                    }

                    SqlConn.Close();
                    return dt;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                SqlConn.Close();
                return null;
            }
        }
    }
}
