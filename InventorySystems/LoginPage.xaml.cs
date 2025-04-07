using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace InventorySystems
{
    public partial class LoginPage : ContentPage
    {
        // Define a dictionary with valid usernames and passwords
        private Dictionary<string, string> validCredentials = new Dictionary<string, string>
        {
            { "jake", "staas" },
            { "ethan", "pedrick" },
            { "devin", "white" },
            { "nhat", "nguyen" }
            // Add more username-password pairs as needed
        };

        public LoginPage()
        {
            InitializeComponent();
        }

        // Allows Enter to be Pushed
        private void OnLoginEntryCompleted(object sender, EventArgs e)
        {
            // Trigger the login button click event when Enter is pressed
            OnLoginClicked(sender, e);
        }
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Get the entered username and password from the entry fields
            string enteredUsername = usernameEntry.Text;
            string enteredPassword = passwordEntry.Text;

            // Check if the username exists in the dictionary and if the password matches
            if (validCredentials.ContainsKey(enteredUsername) && validCredentials[enteredUsername] == enteredPassword)
            {
                // If the credentials are valid, navigate to the MainPage
                await Navigation.PushAsync(new MainPage(enteredUsername));
            }
            else
            {
                // If the credentials are invalid, show an error message
                await DisplayAlert("Login Failed", "Invalid username or password. Please try again.", "OK");
            }
        }
    }
}
