using SixLaborsCaptcha.Core;
using ZALUPA.Database.Classes;
using ZALUPA.Database;
using ZALUPA.Database.Engine;

namespace ZALUPA;

public partial class MainPage : ContentPage
{
    private User CurrentUser { get; set; }
    private string _validCaptcha = string.Empty;
    private int invalidInputCount = 0;
    public MainPage()
    {
        InitializeComponent();
        CaptchaStackLayout.IsVisible = false;
    }

    private async void LogIn()
    {
        if (CaptchaStackLayout.IsVisible)
        {
            if (string.IsNullOrEmpty(CaptchaEntry.Text) || CaptchaEntry.Text != _validCaptcha)
            {
                await DisplayAlertAsync("Ошибка", "Неверная капча", "OK");
                GenerateCaptcha();
                CaptchaEntry.Text = string.Empty;
                return;
            }
            CaptchaStackLayout.IsVisible = false;
            invalidInputCount = 0;
        }

        Actions act = new();
        User? u = act.GetUser(Login.Text, Password.Text);
    
        if (u == null)
        {
            invalidInputCount++;
        
            if (invalidInputCount >= 3)
            {
                CaptchaStackLayout.IsVisible = true;
                GenerateCaptcha();
            }
        
            await DisplayAlertAsync("Ошибка", "Пользователь не найден", "OK");
        }
        else
        {
            CurrentUser = u;
            invalidInputCount = 0;
            CaptchaStackLayout.IsVisible = false;
            await DisplayAlertAsync("Успех", $"Добро пожаловать, {u.Login}", "OK");
        }
    }
    private void LoginButton(object? sender, EventArgs e)
    {
        LogIn();
    }
    private void GenerateCaptcha()
    {
        invalidInputCount = 0;
        SixLaborsCaptchaModule generator = new SixLaborsCaptchaModule(new SixLaborsCaptchaOptions
        {
            Width = 300,
            Height = 200,
            FontSize = 20,
            NoiseRate = 50,
            DrawLines = 15
        });
        _validCaptcha = SixLaborsCaptcha.Core.Extensions.GetUniqueKey(6); 
        byte[] captchaImageBytes = generator.Generate(_validCaptcha);
        var imageSource = ImageSource.FromStream(() => new MemoryStream(captchaImageBytes)); 
        CaptchaImage.Source = imageSource; 
    }
}