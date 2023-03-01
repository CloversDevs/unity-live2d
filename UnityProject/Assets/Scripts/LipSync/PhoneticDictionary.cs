using System.Collections.Generic;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Class that contains a dictionary of phonemes for words.
    /// Once the phonetic values are loaded they can be generated from words.
    /// </summary>
    public class PhoneticDictionary
    {
        /// <summary>
        /// Words in the dictionary.
        /// This list has a 1 to 1 mapped correspondence with the Phonemes List.
        /// </summary>
        private readonly List<string> Words = new();
        
        /// <summary>
        /// Phonemes in the dictionary.
        /// This list has a 1 to 1 mapped correspondence with the Words List.
        /// </summary>
        private readonly List<List<string>> Phonemes = new();

        /// <summary>
        /// Add a new word with its phonemes.
        /// </summary>
        public void Add(string word, List<string> phonenes)
        {
            Words.Add(word);
            Phonemes.Add(phonenes);
        }

        /// <summary>
        /// Get the phonemes for the given word.
        /// </summary>
        /// <returns>List of phonemes and their indexes for the indices</returns>
        public List<PhonemeInText> GetPhonemes(string word)
        {
            var indices = GetPhonemeIndex(word.ToUpperInvariant());
            var phonemes = new List<string>();
            foreach (var index in indices)
            {
                phonemes.AddRange(Phonemes[index]);
            }

            return AssignPhonemePositions(word, phonemes);
        }

        /// <summary>
        /// Try to get phonemes that match as best as possible the requested word.
        /// TODO: Check for holes in this, and make sure it works as intended
        /// </summary>
        private int[] GetPhonemeIndex(string word)
        {
            int index = Words.BinarySearch(word);

            if (index >= 0)
            {
                // word is found in the list
                return new [] { index };
            }
            
            // word is not found in the list
            // BinarySearch returns the bitwise complement of the index of the next largest element
            index = ~index;

            // find the index of the previous element
            int prevIndex = index - 1;

            // check if the previous element is a prefix of the search word
            if (prevIndex >= 0 && word.StartsWith(Words[prevIndex]))
            {
                // return the index of the previous element
                return new [] { prevIndex };
            }
            
            // the search word is not a prefix of any element in the list
            // find the indices of the prefixes that could match the search word
            List<int> indices = new List<int>();
            for (int i = 0; i < Words.Count; i++)
            {
                if (word.StartsWith(Words[i]))
                {
                    indices.Add(i);
                }
            }

            return indices.ToArray();
        }
        
        /// <summary>
        /// Map the phonemes as best as possible to the characters in the word.
        /// TODO: Check for holes in this, and make sure it works as intended
        /// </summary>
        private List<PhonemeInText> AssignPhonemePositions(string word, List<string> phonemes)
        {
            var result = new List<PhonemeInText>();

            var phonemeIndex = 0;
            var spaceBetweenPhonemes = word.Length / (phonemes.Count + 1);

            for (var i = 0; i < word.Length; i++)
            {
                var isPhoneme = spaceBetweenPhonemes == 0 || i % spaceBetweenPhonemes == 0;
                if (phonemeIndex >= phonemes.Count || !isPhoneme) continue;
                
                result.Add(new(phonemes[phonemeIndex], i));
                phonemeIndex++;
            }

            return result;
        }
    }
}