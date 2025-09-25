using UnityEngine;
using UnityEngine.UI;
using Dan.Main;

namespace LeaderboardCreatorDemo
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _entryTextObjects;
        [SerializeField] private InputField _usernameInputField;

        private void Start()
        {
            LoadEntries();
        }

        private void LoadEntries()
        {

            Leaderboards.PWP.GetEntries(entries =>
            {
                
                //foreach (var t in _entryTextObjects)
                //    t.text = "";

                var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                {
                    LeaderboardBar bar = _entryTextObjects[i].GetComponent<LeaderboardBar>();
                    bar.index.text = $"{entries[i].Rank}";
                    bar.name.text = $"{entries[i].Username}";
                    bar.score.text = $"{entries[i].Score}";
                }
            });
        }
        public void UploadEntry(int score)
        {
            Leaderboards.PWP.UploadNewEntry(_usernameInputField.text, score, isSuccessful =>
            {
                print("submitted entry");
                if (isSuccessful)
                    LoadEntries();
            });
        }

    }
}
