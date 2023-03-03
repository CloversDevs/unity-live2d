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
    public enum Viseme
    {
        Silence = 0,
        A = 1,
        E = 2,
        I = 3,
        O = 4,
        U = 5,
        F_T_Th_V = 6,
        J_Ch_Sh = 7,
        B_M_P = 8,
        C_D_G_K_N_S_X_Z = 9,
        Q_W = 10,
        R_L = 11,
    }
    
    /// <summary>
    /// Converter that creates visemes from the provided text.
    /// </summary>
    public class TextToVisemes
    {
        /// <summary>
        /// Map from names of phonemes to the Viseme animation ids.
        /// </summary>
        public static readonly Dictionary<string, Viseme> _phonemeToViseme = new()
        { 
            { "AH", Viseme.A }, 
            { "EY", Viseme.E }, 
            { "Z", Viseme.C_D_G_K_N_S_X_Z }, 
            { "D", Viseme.C_D_G_K_N_S_X_Z }, 
            { "IY", Viseme.I }, 
            { "EH", Viseme.E }, 
            { "M", Viseme.B_M_P }, 
            { "F", Viseme.F_T_Th_V }, 
            { "AO", Viseme.A }, 
            { "R", Viseme.C_D_G_K_N_S_X_Z }, 
            { "T", Viseme.F_T_Th_V }, 
            { "UW", Viseme.U }, 
            { "W", Viseme.C_D_G_K_N_S_X_Z }, 
            { "N", Viseme.C_D_G_K_N_S_X_Z }, 
            { "IH", Viseme.I }, 
            { "P", Viseme.B_M_P }, 
            { "L", Viseme.C_D_G_K_N_S_X_Z }, 
            { "AY", Viseme.A }, 
            { "AA", Viseme.A }, 
            { "B", Viseme.B_M_P }, 
            { "ER", Viseme.E }, 
            { "G", Viseme.C_D_G_K_N_S_X_Z }, 
            { "K", Viseme.C_D_G_K_N_S_X_Z }, 
            { "S", Viseme.C_D_G_K_N_S_X_Z }, 
            { "TH", Viseme.F_T_Th_V }, 
            { "V", Viseme.F_T_Th_V }, 
            { "HH", Viseme.J_Ch_Sh }, 
            { "AE", Viseme.A }, 
            { "OW",Viseme.O }, 
            { "NG", Viseme.C_D_G_K_N_S_X_Z }, 
            { "SH", Viseme.J_Ch_Sh }, 
            { "ZH", Viseme.J_Ch_Sh }, 
            { "Y", Viseme.E }, 
            { "AW", Viseme.A }, 
            { "JH", Viseme.J_Ch_Sh }, 
            { "CH", Viseme.J_Ch_Sh }, 
            { "UH", Viseme.U }, 
            { "DH", Viseme.C_D_G_K_N_S_X_Z }, 
            { "OY", Viseme.O }
        };
        
        /// <summary>
        /// Path to the simplified phoneme dictionary.
        /// </summary>
        public readonly string DICTIONARY_SIMPLIFIED_PATH = "cmudict/cmudict-short.7b";
        
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
            var filePath = Path.Combine(Application.streamingAssetsPath, DICTIONARY_SIMPLIFIED_PATH);

            var startTime = Time.realtimeSinceStartup;
            var lines = await File.ReadAllLinesAsync(filePath);

            var count = 0;
            foreach (var line in lines)
            {
                var contents = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var values = new List<string>(new ArraySegment<string>(contents, 1, contents.Length - 1));

                _dictionary.Add(contents[0], values);
                count++;
            }
            
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
