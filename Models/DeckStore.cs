using System.IO;
using System.Text.Json;

namespace EZFlash.Models
{
    public class DeckStore
    {
        private JsonSerializerOptions _options = new();
        private string _appDir;
        private string _deckPath;

        public DeckCollection Inventory { get; private set; } = new();
        public int InvalidDeckFileCount { get; private set; }

        public DeckStore()
        {
            _options.WriteIndented = true;

            _appDir = AppContext.BaseDirectory;
            _deckPath = Path.Combine(_appDir, "decks");

            Directory.CreateDirectory(_deckPath);

            LoadAllDecksFromDisk();
        }

        public void LoadAllDecksFromDisk()
        {
            InvalidDeckFileCount = 0;

            List<Deck> deckFiles = new();

            foreach (string deckPath in Directory.EnumerateFiles(_deckPath, "*.json"))
            {
                Deck? deck = ReadDeckFromDisk(deckPath);
                if (deck != null)
                {
                    deckFiles.Add(deck);
                }
                else
                {
                    InvalidDeckFileCount++;
                }
            }

            Inventory.Decks = new(deckFiles.OrderBy(deck => deck.Name, StringComparer.CurrentCultureIgnoreCase));
        }


        public void SaveAllDecksToDisk()
        {
            if (Inventory.Decks != null)
                foreach (Deck deck in Inventory.Decks)
                    SaveDeckToDisk(deck);
        }

        public void SaveDeckToDisk(Deck deck)
        {
            string serializedDeck = JsonSerializer.Serialize<Deck>(deck, _options);
            File.WriteAllText(Path.Combine(_deckPath, deck.Id.ToString() + ".json"), serializedDeck);
        }

        private Deck? ReadDeckFromDisk(string deckPath)
        {
            try
            {
                string serializedDeck = File.ReadAllText(deckPath);

                Deck? deck = JsonSerializer.Deserialize<Deck>(serializedDeck);

                if (deck == null)
                    return null;

                if (deck.Id == Guid.Empty)
                    return null;

                if (string.IsNullOrWhiteSpace(deck.Name))
                    return null;

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(deckPath);

                if (fileNameWithoutExtension != deck.Id.ToString())
                    return null;

                return deck;
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

        public void DeleteDeckFromDisk(Deck deck)
        {
            if (deck != null)
            {
                File.Delete(Path.Combine(_deckPath, deck.Id.ToString()) + ".json");
                Inventory.RemoveDeck(deck);
            }
        }

    }
}
