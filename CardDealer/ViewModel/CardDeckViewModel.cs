using CardDealer.Model;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CardDealer.ViewModel
{
    public class CardDeckViewModel
    {
        // Simple ViewModel arrangement - Instantiates the Card Deck object and relays commands from the view to the model.
        // Error handling also done from here for this simple example.

        #region Private Variables        
        private ICommand _command;
        #endregion

        #region Public Properties

        public CardDeck CardDeck { get; set; }

        public ICommand Command
        {
            get
            {
                return _command ?? (_command = new RelayCommand<string>(x =>
                {
                    try
                    {
                            // Handle the command process
                            switch (x)
                        {
                            case "Shuffle":
                                {
                                    if (CardDeck != null) CardDeck.BeginShuffle();
                                    break;
                                }
                            case "Draw":
                                {
                                    if (CardDeck!= null) CardDeck.BeginDrawCard();
                                    break;
                                }
                            default:
                                {
                                    MessageBox.Show($"Invalid Command: {x}");
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}", "Card Dealer");
                    }
                }));
            }
        }
        #endregion

        #region Constructor
        public CardDeckViewModel()
        {
            try
            {
                CardDeck = new CardDeck();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\n{ex.StackTrace}", "Card Dealer");
            }
        }
        #endregion
    }
}
