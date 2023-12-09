using System.ComponentModel;
using System.Linq;
using System.Windows;
using sportShop.Pages.AdminPages;
using sportShop.Pages.ClientPages;
using sportShop.Pages.ManagerPages;

namespace sportShop.ViewModels;

sealed public class AuthorizationPageViewModel : INotifyPropertyChanged
{
    private readonly MainWindow? _mainWindow;
    private string _login;

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Login)));
        }
    }

    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
        }
    }

    public RelayCommand SubmitAuthCommand { get; }
    public RelayCommand NavigateToRegistrationCommand { get; }

    public AuthorizationPageViewModel()
    {
        _mainWindow = Application.Current.MainWindow as MainWindow;

        _password = string.Empty;
        _login = string.Empty;
        SubmitAuthCommand = new RelayCommand(SubmitAuthCommandExecute);
        NavigateToRegistrationCommand = new RelayCommand(NavigateToRegistrationCommandExecute);
    }
    private void NavigateToRegistrationCommandExecute()
    {
        _mainWindow?.MainFrame.NavigationService.Navigate(new RegistrationPage());
    }

    private void SubmitAuthCommandExecute()
    {
        var dbContext = new DbContext();

        if (dbContext.Clients.Any(client => client.Password == _password && client.Login == _login))
            _mainWindow?.MainFrame.NavigationService.Navigate(new ClientProductView(dbContext.Clients.First(client => client.Password == _password && client.Login == _login)));

        else if (dbContext.Managers.Any(manager => manager.Password == _password && manager.Login == _login))
            _mainWindow?.MainFrame.NavigationService.Navigate(new ManagerProductView());

        else if (dbContext.Administrators.Any(administrator => administrator.Password == _password && administrator.Login == _login))
            _mainWindow?.MainFrame.NavigationService.Navigate(new AdminProductView());
        else
            MessageBox.Show("Пользователь не зарегестрирован!");

    }

    public event PropertyChangedEventHandler? PropertyChanged;
}