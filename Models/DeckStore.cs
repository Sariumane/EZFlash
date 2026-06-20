using System.IO;
using System.Text.Json;

namespace EZFlash.Models
{
    public class DeckStore
    {
        private JsonSerializerOptions _options = new();
        private string _appDir;
        private string _deckPath;
        private string _reviewPath;

        public DeckCollection Inventory { get; private set; } = new();

        public DeckStore()
        {
            _options.WriteIndented = true;

            _appDir = AppContext.BaseDirectory;
            _deckPath = Path.Combine(_appDir, "decks");
            _reviewPath = Path.Combine(_appDir, "reviews");

            Directory.CreateDirectory(_deckPath);
            Directory.CreateDirectory(_reviewPath);

            LoadAllDecksFromDisk();
        }

        public void LoadAllDecksFromDisk()
        {
            foreach(string deckPath in Directory.EnumerateFiles(_deckPath, "*.json"))
            {
                Deck? deck = ReadDeckFromDisk(deckPath);
                if (deck != null)
                    Inventory.AddDeck(deck);
            }
        }

        public void SaveAllDecksToDisk()
        {
            if(Inventory.Decks != null)
                foreach (Deck deck in Inventory.Decks)
                    SaveDeckToDisk(deck);
        }

        public void SaveDeckToDisk(Deck deck)
        {
            string serializedDeck = JsonSerializer.Serialize<Deck>(deck, _options);
            File.WriteAllText(Path.Combine(_deckPath, deck.Name + ".json"), serializedDeck);
        }

        private Deck? ReadDeckFromDisk(string deckPath)
        {
            try
            {
                string serializedDeck = File.ReadAllText(deckPath);

                return JsonSerializer.Deserialize<Deck>(serializedDeck);
            }
            catch (IOException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}
