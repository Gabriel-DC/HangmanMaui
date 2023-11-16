using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HangmanMaui.Models
{
    public class HangmanContext : INotifyPropertyChanged
    {
        private List<string> _availableWords = new List<string>()
        {
            "gabriel", "bochechas", "tela", "notebook", "castelo", "maui"
        }.ConvertAll(word => word.ToUpper());

        private string _selectedWord;
        private List<char> _usedChars;

        private int _errorCount;

        public int ErrorsCount
        {
            get => _errorCount;
            set
            {
                _errorCount = value;
                OnPropertyChanged();
            }
        }        

        private string _currentWord;

        public string CurrentWord
        {
            get => _currentWord;
            set
            {
                _currentWord = value;
                OnPropertyChanged();
            }
        }

        public HangmanContext()
        {
            InitGame();
        }

        public void InitGame() => ReloadGame();

        public void ReloadGame()
        {
            ErrorsCount = 0;
            _selectedWord = _availableWords[Random.Shared.Next(_availableWords.Count - 1)];
            CurrentWord = string.Join("", _selectedWord.ToList().ConvertAll(character => "_"));
            _usedChars = [];
        }

        public void NewCharInput(char input)
        {
            char newInput = char.ToUpper(input);

            if(_usedChars.Contains(newInput) || _errorCount > 6) 
                return;

            _usedChars.Add(newInput);

            if(_selectedWord.Contains(newInput, StringComparison.OrdinalIgnoreCase))
            {
                ReplaceAllOcurrences(newInput);
                return;
            }

            ErrorsCount++;
        }

        private void ReplaceAllOcurrences(char newChar, int index = 0)
        {
            int charIndex = _selectedWord.IndexOf(newChar, index);

            if(charIndex > -1)
            {
                char[] charArr = CurrentWord.ToCharArray();
                
                charArr[charIndex] = newChar;

                CurrentWord = new string(charArr);

                ReplaceAllOcurrences(newChar, charIndex + 1);
            }
        }

        public string GetSelectedWord() => _selectedWord;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
