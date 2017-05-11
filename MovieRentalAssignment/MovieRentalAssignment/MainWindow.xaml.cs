using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieRentalAssignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        



    {
        DatabaseManager objDB = new DatabaseManager();

        public MainWindow()
        {
            InitializeComponent();
            dgCustomers.ItemsSource = objDB.ListCustomers().DefaultView;            //loads the data into the datagridview boxes
            dgMovies.ItemsSource = objDB.ListMovies().DefaultView;
            dgRentals.ItemsSource = objDB.ListRentedMovies().DefaultView;
        }
                                                                                                        
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)    //add customer button
        {
            var regexItem = new Regex("^[a-zA-Z ]*$");
            if (regexItem.IsMatch(txtFirstName.Text) && regexItem.IsMatch(txtLastName.Text))
            {
                bool custExists = objDB.CustExist(txtFirstName.Text, Convert.ToInt32(txtPhone.Text));
                if (custExists == false)
                {
                    if (txtFirstName.Text != "" && txtLastName.Text != "" && txtAddress.Text != "" && txtPhone.Text != "")
                    {
                        bool customerAdded = objDB.AddCustomer(txtFirstName.Text, txtLastName.Text, txtAddress.Text, txtPhone.Text);

                        if (customerAdded == true)
                        {
                            MessageBox.Show("Customer added");
                            txtFirstName.Clear();
                            txtLastName.Clear();
                            txtAddress.Clear();
                            txtPhone.Clear();
                            dgCustomers.ItemsSource = objDB.ListCustomers().DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("Enter all user details");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Customer is already in the system");
                }
            }
            else
            {
                MessageBox.Show("No special characters alowed");
            }
        }

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)          //delete customer button  checks if the customer has a movie out, if they do they arent deleted
        {
            bool checkCustMovieOut = objDB.CustMovieRentedOut(Convert.ToInt32(txtCustID.Text));              
            if (checkCustMovieOut == false)
            {
                DataRowView row = (DataRowView)dgCustomers.SelectedItems[0];
                int custid = Convert.ToInt16(row["CustID"]);
                objDB.DeleteCustomer(custid);

                dgCustomers.ItemsSource = objDB.ListCustomers().DefaultView;
            }
            else
            {
                MessageBox.Show("The customer must return a movie first");
            }
        }

        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)       //shows info about selected customer in the text boxes
        {
            if ( dgCustomers.SelectedIndex > -1)
            {
                DataRowView row = (DataRowView)dgCustomers.SelectedItems[0];
                txtFirstName.Text = Convert.ToString(row["FirstName"]);
                txtLastName.Text = Convert.ToString(row["LastName"]);
                txtAddress.Text = Convert.ToString(row["Address"]);
                txtPhone.Text = Convert.ToString(row["Phone"]);
                txtCustID.Text = Convert.ToString(row["CustID"]);
            }
        }

        private void btnUpdateCustomer_Click(object sender, RoutedEventArgs e)           // update the customer button
        {
            if(dgCustomers.SelectedIndex > -1)
            {
                if (txtFirstName.Text != "" && txtLastName.Text != "" && txtAddress.Text != "" && txtPhone.Text != "")
                {
                    DataRowView row = (DataRowView)dgCustomers.SelectedItems[0];
                    
                    string bookname = txtFirstName.Text;
                    string lastname = txtLastName.Text;
                    string address = txtAddress.Text;
                    string phone = txtPhone.Text;
                    int CustID = Convert.ToInt16(txtCustID.Text);
                    objDB.EditCustomer(bookname, lastname, address, phone, CustID);

                    bool custEdited = objDB.EditCustomer(txtFirstName.Text, txtLastName.Text, txtAddress.Text, txtPhone.Text, Convert.ToInt16(txtCustID.Text));

                    if (custEdited == true)
                    {
                        MessageBox.Show("Customer Edited");
                        txtFirstName.Clear();
                        txtLastName.Clear();
                        txtAddress.Clear();
                        txtPhone.Clear();
                        txtCustID.Clear();
                        
                        dgCustomers.ItemsSource = objDB.ListCustomers().DefaultView;
                    }
                }
                else
                {
                    MessageBox.Show("Complete all fields");
                }
            }
            else
            {
                MessageBox.Show("Select Customer to edit");
            }
        }

        private void btnAddMovie_Click(object sender, RoutedEventArgs e)         // add movie button 
        {
            int number = 0;
            if (int.TryParse(txtYear.Text.Trim(), out number))
            {
                bool movieExists = objDB.MovieExist(txtMovieName.Text);
                if (movieExists == false)                                                                   //checks if movie exists already
                {
                    if (txtRating.Text != "" && txtMovieName.Text != "" && txtGenre.Text != "" && txtYear.Text != "" && txtCopies.Text != "")
                    {
                        int year = Convert.ToInt16(txtYear.Text);
                        int currentYear = DateTime.Now.Year;
                        double cost = 0;
                        if (year == currentYear)
                        {
                            cost = 5;
                        }
                        else
                        {
                            cost = 2;
                        }
                        bool movieAdded = objDB.AddMovie(txtRating.Text, txtMovieName.Text, txtYear.Text, cost, txtCopies.Text, txtPlot.Text, txtGenre.Text);

                        if (movieAdded == true)
                        {
                            MessageBox.Show("Movie added");
                            txtRating.Clear();
                            txtMovieName.Clear();
                            txtGenre.Clear();
                            txtPlot.Clear();
                            txtYear.Clear();
                            txtCost.Clear();
                            txtCopies.Clear();
                            txtMovieID.Clear();
                            dgMovies.ItemsSource = objDB.ListMovies().DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("Enter all movie details");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The movie is already in the system");
                }
            } 
            else
            {
                MessageBox.Show("Invalid input");
            }       
        }

        private void btnDeleteMovie_Click(object sender, RoutedEventArgs e)       // deletes the movie, checks if anyone has the movie out, if they do it cannot be deleted
        {
            bool checkMovieOut = objDB.MovieRentedOut(Convert.ToInt16(txtMovieID.Text));               
            if (checkMovieOut == false)
            {
                DataRowView row = (DataRowView)dgMovies.SelectedItems[0];
                int movieID = Convert.ToInt16(row["MovieID"]);
                objDB.DeleteMovie(movieID);

                dgMovies.ItemsSource = objDB.ListMovies().DefaultView;
            }
            else
            {
                MessageBox.Show("The movie must be returned first");
            }
        }

        private void dgMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)       //adds the info about the selected movie to the text boxes
        {
            if (dgMovies.SelectedIndex > -1)
            {
                DataRowView row = (DataRowView)dgMovies.SelectedItems[0];
                txtMovieID.Text = Convert.ToString(row["MovieID"]);
                txtGenre.Text = Convert.ToString(row["Genre"]);
                txtMovieName.Text = Convert.ToString(row["Title"]);
                txtPlot.Text = Convert.ToString(row["Plot"]);
                txtYear.Text = Convert.ToString(row["Year"]);
                txtRating.Text = Convert.ToString(row["Rating"]);
                txtCost.Text = Convert.ToString(row["Rental_Cost"]);
                txtCopies.Text = Convert.ToString(row["Copies"]);
            }
        }

        private void btnUpdateMovie_Click(object sender, RoutedEventArgs e)          // updates the selected movie, calling the EditMovie function
        {
            if (dgMovies.SelectedIndex > -1)
            {
                if (txtMovieName.Text != "" && txtRating.Text != "" && txtYear.Text != "" && txtCopies.Text != "" && txtGenre.Text != "" && txtCost.Text != "")
                {
                    DataRowView row = (DataRowView)dgMovies.SelectedItems[0];


                    string rating = txtRating.Text;
                    string moviename = txtMovieName.Text;                    
                    string year = txtYear.Text;
                    string rentalcost = Convert.ToString(5);
                    if (Convert.ToInt16(year) < DateTime.Now.Year)
                    {
                        rentalcost = Convert.ToString(2);
                    }
                    
                    string plot = txtPlot.Text;
                    string genre = txtGenre.Text;
                    string copies = txtCopies.Text;
                    
                    int MovieID = Convert.ToInt16(txtMovieID.Text);
                    objDB.EditMovie(rating, moviename, year,  plot, genre, copies, Convert.ToDouble(rentalcost), MovieID);

                    bool movieEdited = objDB.EditMovie(txtMovieName.Text, txtRating.Text, txtYear.Text, txtPlot.Text, txtGenre.Text, txtCopies.Text, Convert.ToDouble(txtCost.Text), Convert.ToInt16(txtMovieID.Text));

                    if (movieEdited == true)
                    {
                        MessageBox.Show("Movie Edited");
                        txtMovieName.Clear();
                        txtPlot.Clear();
                        txtRating.Clear();
                        txtYear.Clear();
                        txtCopies.Clear();
                        txtMovieID.Clear();                                 //clearing the text boxes
                        txtCost.Clear();
                        txtGenre.Clear();

                        dgMovies.ItemsSource = objDB.ListMovies().DefaultView;
                    }
                }
                else
                {
                    MessageBox.Show("Complete all fields");
                }
            }
            else
            {
                MessageBox.Show("Select Movie to edit");
            }
        }

        private void btnIssue_Click(object sender, RoutedEventArgs e)         // calls the RentMovie function when button is clicked
        {
            string custID = txtCustID.Text;
            DateTime dateTimeVariable = DateTime.Now;

                                                                                                                        
            if (Convert.ToInt16(txtCopies.Text) >= 1)          
            {
                if (txtMovieID.Text != "" && txtCustID.Text != "")
                {
                    bool custChecked = objDB.CheckRentedMovies(Convert.ToInt16(txtCustID.Text));
                    if (custChecked == true)
                    {
                        bool movieIssued = objDB.RentMovie(Convert.ToInt16(txtCustID.Text), Convert.ToInt16(txtMovieID.Text), dateTimeVariable);

                        if (movieIssued == true)
                        {
                            MessageBox.Show("Movie issued");
                            txtMovieName.Clear();
                            txtPlot.Clear();
                            txtRating.Clear();
                            txtYear.Clear();
                            txtMovieID.Clear();
                            txtGenre.Clear();
                            txtCost.Clear();
                            txtCopies.Clear();
                            txtCustID.Clear();
                            txtAddress.Clear();
                            txtFirstName.Clear();
                            txtLastName.Clear();
                            txtPhone.Clear();
                            dgRentals.ItemsSource = objDB.ListRentedMovies().DefaultView;
                            dgMovies.ItemsSource = objDB.ListMovies().DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("Enter all details");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Customer already has 2 movies out");
                    }
                }
            }
            else
            {
                MessageBox.Show("There are no more copies left!");
                txtMovieName.Clear();
                txtPlot.Clear();
                txtRating.Clear();
                txtYear.Clear();
                txtMovieID.Clear();
                txtGenre.Clear();
                txtCost.Clear();
                txtCopies.Clear();
                txtCustID.Clear();
                txtAddress.Clear();
                txtFirstName.Clear();
                txtLastName.Clear();
                txtPhone.Clear();
            }
        }

        private void dgRentals_SelectionChanged(object sender, SelectionChangedEventArgs e)  //changes apporpriate text boxes for selected item
        {
            if (dgRentals.SelectedIndex > -1)
            {
                DataRowView row = (DataRowView)dgRentals.SelectedItems[0];
                txtMovieID.Text = Convert.ToString(row["MovieIDFK"]);
                txtCustID.Text = Convert.ToString(row["CustIDFK"]);
                
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)               // returning movie, calling the ReturnMovie function
        {
            DateTime dateTimeVariable = DateTime.Now;
            if (dgRentals.SelectedIndex > -1)
            {
                if (txtCustID.Text != "" && txtMovieID.Text != "" )
                {
                    DataRowView row = (DataRowView)dgRentals.SelectedItems[0];

                    int MovieID = Convert.ToInt16(txtMovieID.Text);
                    int CustID = Convert.ToInt16(txtCustID.Text);

                    bool movieReturned = objDB.ReturnMovie(Convert.ToInt16(txtCustID.Text), Convert.ToInt16(txtMovieID.Text), dateTimeVariable);

                    if (movieReturned == true)
                    {
                        MessageBox.Show("Movie returned");                        
                        txtMovieID.Clear();
                        txtCustID.Clear();

                        dgRentals.ItemsSource = objDB.ListRentedMovies().DefaultView;
                        dgMovies.ItemsSource = objDB.ListMovies().DefaultView;
                    }
                }
                else
                {
                    MessageBox.Show("Error");
                    txtMovieID.Clear();
                    txtCustID.Clear();
                }
            }
            else
            {
                MessageBox.Show("Select Movie to Return");
            }
        }

        private void btnClearMovieInfo_Click(object sender, RoutedEventArgs e)       //clears movie info text boxes
        {
            txtMovieName.Clear();
            txtPlot.Clear();
            txtRating.Clear();
            txtYear.Clear();
            txtCopies.Clear();
            txtMovieID.Clear();
            txtCost.Clear();
            txtGenre.Clear();
        }

        private void btnClearCustInfo_Click(object sender, RoutedEventArgs e)       //clears custinfo txt boxes
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtCustID.Clear();
        }

        private void txtCustSearch_TextChanged(object sender, TextChangedEventArgs e)  //lists searched items
        {
            dgCustomers.ItemsSource = objDB.SearchCustomers(txtCustSearch.Text).DefaultView;
        }

        private void txtMovieSearch_TextChanged(object sender, TextChangedEventArgs e)  //lists searched items
        {
            dgMovies.ItemsSource = objDB.SearchMovies(txtMovieSearch.Text).DefaultView;
        }

        private void txtRentalSearch_TextChanged(object sender, TextChangedEventArgs e)  //lists searched items
        {
            dgRentals.ItemsSource = objDB.SearchRentals(txtRentalSearch.Text).DefaultView;
        }

        private void cbShowRentedMovies_Checked(object sender, RoutedEventArgs e)  //lists movies that have not been returned when checkbox is checked
        {
            dgRentals.ItemsSource = objDB.ListRentedOutMovies().DefaultView;
        }

        private void cbShowRentedMovies_Unchecked(object sender, RoutedEventArgs e) // lists all movies that have been issued in the past, or that are still out, when the checkbox is unchecked
        {
            dgRentals.ItemsSource = objDB.ListRentedMovies().DefaultView;
        }

        //private void txtYear_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    //allows backspace key
        //    string tString = txtYear.Text;
        //    if (tString.Trim() == "") return;
        //    for (int i = 0; i < tString.Length; i++)
        //    {
        //        if (!char.IsNumber(tString[i]))
        //        {
        //            MessageBox.Show("Please enter a valid number");
        //            txtYear.Text = "";
        //            return;
        //        }

        //    }
        //}
    }
}


