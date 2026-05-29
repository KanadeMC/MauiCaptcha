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
        // Если капча видна — проверяем её
        if (CaptchaStackLayout.IsVisible)
        {
            if (string.IsNullOrEmpty(CaptchaEntry.Text) || CaptchaEntry.Text != _validCaptcha)
            {
                await DisplayAlertAsync("Ошибка", "Неверная капча", "OK");
                // Обновляем капчу при каждой новой попытке
                GenerateCaptcha();
                CaptchaEntry.Text = string.Empty; // очищаем поле
                return;
            }
            // Капча верна — сбрасываем флаги
            CaptchaStackLayout.IsVisible = false;
            invalidInputCount = 0;
        }

        // Проверка логина/пароля
        Actions act = new();
        User? u = act.GetUser(Login.Text, Password.Text);
    
        if (u == null)
        {
            invalidInputCount++;
        
            // Если ошибок стало 3 или больше — показываем капчу
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
            // Успешный вход — сбрасываем всё
            invalidInputCount = 0;
            CaptchaStackLayout.IsVisible = false;
            await DisplayAlertAsync("Успех", $"Добро пожаловать, {u.Login}", "OK");
            // Здесь переход на следующий экран
        }
    }
    private void LoginButton(object? sender, EventArgs e)
    {
        LogIn();
    }
    private void GenerateCaptcha()
    {
        invalidInputCount = 0; // Обновляем количество неправильных вводов
        SixLaborsCaptchaModule generator = new SixLaborsCaptchaModule(new SixLaborsCaptchaOptions
        {
            Width = 300,
            Height = 200,
            FontSize = 20,
            NoiseRate = 50,
            DrawLines = 15
        });
        _validCaptcha = SixLaborsCaptcha.Core.Extensions.GetUniqueKey(6); // Генерируем ключ-строку
        byte[] captchaImageBytes = generator.Generate(_validCaptcha); // Генерируем изображение
        var imageSource = ImageSource.FromStream(() => new MemoryStream(captchaImageBytes)); // Превращаем байты изображения в ImageSource через создание картинки в MemoryStream
        CaptchaImage.Source = imageSource; // Меняем картинку в UI
    }
}