using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Converter that creates visemes from the provided text.
    /// </summary>
    public class TextToVisemes
    {
        /// <summary>
        /// Path to the viceme dictionary.
        /// </summary>
        public readonly string DICTIONARY_PATH = "cmudict/cmudict-0.7b";
        
        /// <summary>
        /// Vicemes loaded in memory.
        /// </summary>
        private readonly PhoneticDictionary _dictionary = new();
        
        /// <summary>
        /// Create phonemes for the given text.
        /// </summary>
        /// <returns>Phonemes and positions</returns>
        public List<PhonemeInText> GetSentencePhonemes(string sentence)
        {
            var cleanedSentence = Regex.Replace(sentence, @"[\p{P}\p{S}]+", " ");
            var words = SplitWithIndex(cleanedSentence);
            var result = new List<PhonemeInText>();
            foreach (var wordAndPosition in words)
            {
                var phonemes = GetWordPhonemes(wordAndPosition.Phoneme);
                foreach (var phonemeAndPosition in phonemes)
                {
                    phonemeAndPosition.Index += wordAndPosition.Index;
                    result.Add(phonemeAndPosition);
                }
            }

            return result;
        }

        /// <summary>
        /// Load phonetic dictionary from disk.
        /// </summary>
        public async Task Load()
        {
            var filePath = Path.Combine(Application.streamingAssetsPath, DICTIONARY_PATH);

            var startTime = Time.realtimeSinceStartup;
            var lines = await File.ReadAllLinesAsync(filePath);

            var allPhonemes = new List<string>();
            var count = 0;
            foreach (string line in lines)
            {
                // Discard empty lines
                if (line.Length == 0)
                {
                    continue;
                }

                var firstLetter = line[0];
                var startsWithLetter = char.IsLetter(firstLetter) && firstLetter is >= 'A' and <= 'Z';
                // Discard lines that don't start with a word
                if (!startsWithLetter)
                {
                    continue;
                }

                var contents = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                // Discard lines that don't contain a word and a pronunciation
                if (contents.Length < 2)
                {
                    continue;
                }

                for (var i = 0; i < contents.Length; i++)
                {
                    contents[i] = contents[i].Replace(" ", "");
                    contents[i] = Regex.Replace(contents[i], @"[\d\s]+", "");
                }

                var values = new List<string>(new ArraySegment<string>(contents, 1, contents.Length - 1));
                
                foreach (var phoneme in values)
                {
                    if (allPhonemes.Contains(phoneme))
                    {
                        continue;
                    }
                    allPhonemes.Add(phoneme);
                }
                
                _dictionary.Add(contents[0], values);
                count++;
            }

            var phonemesLog = $"All Phonemes ({allPhonemes.Count}):";
            var reducedPhonemes = new List<string>();
            foreach (var phoneme in allPhonemes)
            {
                phonemesLog += $" {phoneme}";
                var simplifiedPhoneme = Regex.Replace(phoneme, @"\d+", "");
                if (reducedPhonemes.Contains(simplifiedPhoneme))
                {
                    continue;
                }
                reducedPhonemes.Add(simplifiedPhoneme);
            }
            Debug.LogError(phonemesLog);
            
            phonemesLog = $"Reduced Phonemes ({reducedPhonemes.Count}):";
            foreach (var phoneme in reducedPhonemes)
            {
                phonemesLog += $" {phoneme}";
            }
            Debug.LogError(phonemesLog);
            Debug.LogError($"Lines read:{count} Time to parse:{Time.realtimeSinceStartup - startTime}");
        }
        
        /// <summary>
        /// Create phonemes for the given word.
        /// </summary>
        private List<PhonemeInText> GetWordPhonemes(string word)
        {
            try
            {
                return _dictionary.GetPhonemes(word);
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError($"Word '{word}' was not found in the dictionary.");
                return new() { new("AA", 0) };
            }
        }
        
        /// <summary>
        /// Split the text by spaces and return the string indices of each split.
        /// </summary>
        private PhonemeInText[] SplitWithIndex(string input)
        {
            var splitResults = new List<PhonemeInText>();
            var index = 0;
            foreach (var word in input.Split(' '))
            {
                if (word.Length > 0)
                {
                    splitResults.Add(new(word, index));
                    // add 1 to account for the space
                    index += word.Length + 1; 
                    continue;
                }
                
                // add 1 to account for the removed space
                index += 1; 
            }
            return splitResults.ToArray();
        }
    }
}
