using HangmanMaui.Models;
using System.Diagnostics;

namespace HangmanMaui
{
    public partial class MainPage : ContentPage
    {
        private readonly HangmanContext _context;
        private readonly List<Button> _buttons = [];

        public MainPage()
        {
            InitializeComponent();
            _context = new HangmanContext();
            BindingContext = _context;
            DesignKeyBoard();
        }

        public void DesignKeyBoard()
        {
            for (char letter = 'A'; letter <= 'Z'; letter++)
                _buttons.Add(new Button() { Text = letter.ToString(), HeightRequest = 50, Margin = 10 });

            foreach (Button button in _buttons)
            {
                button.Clicked += Button_Clicked;
                FlexLayoutButtons.Children.Add(button);
            }
        }

        public async void Button_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string? letter = button.Text;

            if (letter is null || _context.ErrorsCount >= 6)
                return;

            button.IsEnabled = false;
            _context.NewCharInput(char.Parse(letter!));

            if(_context.ErrorsCount >= 6)
            {
                await DisplayAlert("Você Perdeu!", $"A palavra correta era: {_context.GetSelectedWord()}", "OK");
                return;
            }

            if(_context.GetSelectedWord().Equals(_context.CurrentWord, StringComparison.OrdinalIgnoreCase))
            {
                await DisplayAlert("Você Acertou!!", $"Parabéns!!", "OK");
                return;
            }
        }

        private void BtnResetGame_Clicked(object sender, EventArgs e)
        {
            _buttons.ForEach(button => button.IsEnabled = true);
            _context.ReloadGame();
        }


    }

}
